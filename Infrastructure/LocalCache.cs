namespace APIIntegration.Infrastructure;

using APIIntegration.Core;
using APIIntegration.Core.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using APIIntegration.Config;

class LocalCache : ILocalCache
{
    private readonly string _connectionString;
    private readonly ILogger<LocalCache> _logger;
    private bool _initialized = false;
    private readonly SemaphoreSlim _initLock = new(1, 1);

    public LocalCache(IOptions<DatabaseSettings> dbSettings, ILogger<LocalCache> logger)
    {
        _logger = logger;
        var dbPath = dbSettings.Value.Path;
        _connectionString = $"Data Source={dbPath}";
    }

    private async Task EnsureInitializedAsync()
    {
        if (_initialized) return;

        await _initLock.WaitAsync();
        try
        {
            if (_initialized) return;

            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var createTableCmd = connection.CreateCommand();
            createTableCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Messages (
                    MessageId TEXT PRIMARY KEY,
                    Payload TEXT NOT NULL,
                    Status INTEGER NOT NULL,
                    Type INTEGER NOT NULL,
                    LastUpdate TEXT NOT NULL,
                    RetryCount INTEGER NOT NULL DEFAULT 0
                );";
            
            await createTableCmd.ExecuteNonQueryAsync();
            _initialized = true;
            _logger.LogInformation("LocalCache database initialized successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize LocalCache database.");
            throw;
        }
        finally
        {
            _initLock.Release();
        }
    }

    public async Task SaveMessageAsync(Message message, CancellationToken cancellationToken)
    {
        await EnsureInitializedAsync();
        
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT OR REPLACE INTO Messages (MessageId, Payload, Status, Type, LastUpdate, RetryCount)
                VALUES (@messageId, @payload, @status, @type, @lastUpdate, @retryCount);";

            command.Parameters.AddWithValue("@messageId", message.MessageId);
            command.Parameters.AddWithValue("@payload", message.Payload);
            command.Parameters.AddWithValue("@status", (int)message.Status);
            command.Parameters.AddWithValue("@type", (int)message.Type);
            command.Parameters.AddWithValue("@lastUpdate", message.LastUpdate.ToString("o"));
            command.Parameters.AddWithValue("@retryCount", message.RetryCount);

            await command.ExecuteNonQueryAsync(cancellationToken);
            _logger.LogDebug("Message saved to LocalCache. MessageId={MessageId}", message.MessageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save message. MessageId={MessageId}", message.MessageId);
            throw;
        }
    }

    public async Task<Message?> GetMessageAsync(string messageId, CancellationToken cancellationToken)
    {
        await EnsureInitializedAsync();
        
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT MessageId, Payload, Status, Type, LastUpdate, RetryCount
                FROM Messages
                WHERE MessageId = @messageId;";

            command.Parameters.AddWithValue("@messageId", messageId);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (await reader.ReadAsync(cancellationToken))
            {
                return MapMessage(reader);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get message. MessageId={MessageId}", messageId);
            throw;
        }
    }

    public async Task<Message?> GetNextLanMessageAsync(CancellationToken cancellationToken)
    {
        await EnsureInitializedAsync();
        
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT MessageId, Payload, Status, Type, LastUpdate, RetryCount
                FROM Messages
                WHERE Status = @status
                ORDER BY RetryCount ASC, LastUpdate ASC
                LIMIT 1;";

            command.Parameters.AddWithValue("@status", (int)MessageStatus.Pending);

            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            if (await reader.ReadAsync(cancellationToken))
            {
                return MapMessage(reader);
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get next LAN message.");
            throw;
        }
    }

    public async Task<IReadOnlyList<Message>> GetPendingMessageAsync(CancellationToken cancellationToken)
    {
        await EnsureInitializedAsync();
        
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT MessageId, Payload, Status, Type, LastUpdate, RetryCount
                FROM Messages
                WHERE Status = @status
                ORDER BY LastUpdate ASC;";

            command.Parameters.AddWithValue("@status", (int)MessageStatus.Pending);

            var messages = new List<Message>();
            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                messages.Add(MapMessage(reader));
            }

            return messages;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get pending messages.");
            throw;
        }
    }

    public async Task MarkAsSentAsync(string messageId, CancellationToken cancellationToken)
    {
        await EnsureInitializedAsync();
        
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Messages
                SET Status = @status, LastUpdate = @lastUpdate
                WHERE MessageId = @messageId;";

            command.Parameters.AddWithValue("@status", (int)MessageStatus.Sent);
            command.Parameters.AddWithValue("@lastUpdate", DateTime.UtcNow.ToString("o"));
            command.Parameters.AddWithValue("@messageId", messageId);

            await command.ExecuteNonQueryAsync(cancellationToken);
            _logger.LogDebug("Message marked as Sent. MessageId={MessageId}", messageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to mark message as Sent. MessageId={MessageId}", messageId);
            throw;
        }
    }

    public async Task IncrementRetryAsync(string messageId, CancellationToken cancellationToken)
    {
        await EnsureInitializedAsync();
        
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Messages
                SET RetryCount = RetryCount + 1, LastUpdate = @lastUpdate
                WHERE MessageId = @messageId;";

            command.Parameters.AddWithValue("@lastUpdate", DateTime.UtcNow.ToString("o"));
            command.Parameters.AddWithValue("@messageId", messageId);

            await command.ExecuteNonQueryAsync(cancellationToken);
            _logger.LogDebug("Retry count incremented. MessageId={MessageId}", messageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to increment retry count. MessageId={MessageId}", messageId);
            throw;
        }
    }

    public async Task MarkAsAcknowledgedAsync(string messageId, CancellationToken cancellationToken)
    {
        await EnsureInitializedAsync();
        
        try
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);

            var command = connection.CreateCommand();
            command.CommandText = @"
                UPDATE Messages
                SET Status = @status, LastUpdate = @lastUpdate
                WHERE MessageId = @messageId;";

            command.Parameters.AddWithValue("@status", (int)MessageStatus.Acknowledged);
            command.Parameters.AddWithValue("@lastUpdate", DateTime.UtcNow.ToString("o"));
            command.Parameters.AddWithValue("@messageId", messageId);

            await command.ExecuteNonQueryAsync(cancellationToken);
            _logger.LogDebug("Message marked as Acknowledged. MessageId={MessageId}", messageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to mark message as Acknowledged. MessageId={MessageId}", messageId);
            throw;
        }
    }

    private Message MapMessage(SqliteDataReader reader)
    {
        return new Message
        {
            MessageId = reader.GetString(0),
            Payload = reader.GetString(1),
            Status = (MessageStatus)reader.GetInt32(2),
            Type = (MessageType)reader.GetInt32(3),
            LastUpdate = DateTime.Parse(reader.GetString(4)),
            RetryCount = reader.GetInt32(5)
        };
    }
}
