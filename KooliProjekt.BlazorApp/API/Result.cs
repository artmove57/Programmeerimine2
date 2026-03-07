using System;
using System.Collections.Generic;
using System.Linq;

namespace KooliProjekt.BlazorApp.API
{
    /// <summary>
    /// Result - represents operation result (success or error)
    /// Now includes validation errors list for detailed error handling
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public Exception? Exception { get; }

        /// <summary>
        /// Validation errors - dictionary of field names and their error messages
        /// </summary>
        public Dictionary<string, List<string>> Errors { get; }

        protected Result(bool isSuccess, string errorMessage, Dictionary<string, List<string>>? errors = null, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Errors = errors ?? new Dictionary<string, List<string>>();
            Exception = exception;
        }

        public static Result Success()
        {
            return new Result(true, string.Empty);
        }

        public static Result Failure(string errorMessage, Exception? exception = null)
        {
            return new Result(false, errorMessage, null, exception);
        }

        public static Result Failure(Exception exception)
        {
            return new Result(false, exception.Message, null, exception);
        }

        /// <summary>
        /// Create failure result with validation errors
        /// </summary>
        public static Result ValidationFailure(Dictionary<string, List<string>> errors)
        {
            var errorMessage = string.Join("; ", errors.SelectMany(e => e.Value));
            return new Result(false, errorMessage, errors);
        }

        /// <summary>
        /// Check if result has validation errors
        /// </summary>
        public bool HasValidationErrors => Errors.Any();
    }
}
