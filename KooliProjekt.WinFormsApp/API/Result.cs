using System;

namespace KooliProjekt.WinFormsApp.API
{
    /// <summary>
    /// Result - represents operation result (success or error)
    /// Used for API call results
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }
        public Exception? Exception { get; }

        protected Result(bool isSuccess, string errorMessage, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        public static Result Success()
        {
            return new Result(true, string.Empty);
        }

        public static Result Failure(string errorMessage, Exception? exception = null)
        {
            return new Result(false, errorMessage, exception);
        }

        public static Result Failure(Exception exception)
        {
            return new Result(false, exception.Message, exception);
        }
    }
}
