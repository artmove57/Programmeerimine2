using System;
using System.Collections.Generic;

namespace KooliProjekt.BlazorApp.API
{
    /// <summary>
    /// Result<T> - represents operation result with data
    /// Includes validation errors support
    /// </summary>
    public class Result<T> : Result
    {
        public T? Data { get; }

        private Result(bool isSuccess, T? data, string errorMessage, Dictionary<string, List<string>>? errors = null, Exception? exception = null)
            : base(isSuccess, errorMessage, errors, exception)
        {
            Data = data;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, string.Empty);
        }

        public new static Result<T> Failure(string errorMessage, Exception? exception = null)
        {
            return new Result<T>(false, default, errorMessage, null, exception);
        }

        public new static Result<T> Failure(Exception exception)
        {
            return new Result<T>(false, default, exception.Message, null, exception);
        }

        /// <summary>
        /// Create failure result with validation errors
        /// </summary>
        public new static Result<T> ValidationFailure(Dictionary<string, List<string>> errors)
        {
            var errorMessage = string.Join("; ", errors.SelectMany(e => e.Value));
            return new Result<T>(false, default, errorMessage, errors);
        }
    }
}
