using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ResultTypes
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }

        public static OperationResult<T> Success(T data) =>
            new() { IsSuccess = true, Data = data };

        public static OperationResult<T> Failure(string errorMessage) =>
            new() { IsSuccess = false, ErrorMessage = errorMessage };
    }
}
