ApiIntegration Skeleton
Description 
This is a repositiory for a background service project that aims to act like a bridge between 
a cloud API and a local LAN-service.

Baseline:
Repository + DI setup
Architecture and core interfaces
Hosted Service Skeleton
Interface stubs
SqlLite cacke skeleton

Notice that all methods are placeholders, needs to be implemented when API specs are available

Folder structure
/APIIntegration
    /Config
    /Core
        /Models
    /Database
    /Infrastructur
    /Services
appsettings.json
Program.cs
README.md

Functionality
Core interfaces: Defined contract for cloud polling, LAN forwarding, local cache, idempotency and offline/replay logic

Infrastructure Stubs: Empty implementation that can be extended later.
Hosted Services skeletons: background services
Dependency injection: register all interfaces and stubs, ready for expansion. 

DB skeleton: SQLLite Messagees table with stub methods in localCache

Run project
1 Clone repos
https://github.com/chrille-v/APIIntegration.git
cd APIIntegration

2 dotnet build 
3 dotnet run

TODO
Implement CloudClient with real API-integration
Implement LanForward with LAN-logic
Implement LocalCach with SQLlite access
Implement IdempotencyService correctly
Implement retry/backoff
Add actual settings in appsettings.json

Note!
This repository is a skeleton and baseline. All methods that are not implemented throw a NotImplementedException or run placeholder loops.