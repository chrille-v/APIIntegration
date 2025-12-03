using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIIntegration.Core.Models
{
    public class ApiResult
    {
        public bool Success { get; set; }
        public string Error { get; set; } = null!;

        public static ApiResult Ok() => new ApiResult { Success = true };
        public static ApiResult Fail(string e) => new ApiResult { Success = false, Error = e };
    }
}