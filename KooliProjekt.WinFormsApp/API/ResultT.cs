using System;

namespace KooliProjekt.WinFormsApp.API
{
    /// <summary>
    /// Result<T> - represents operation result with data
    /// </summary>
    public class Result<T> : Result
    {
        public T? Data { get; }

        private Result(bool isSuccess, T? data, string errorMessage, Exception? exception = null)
            : base(isSuccess, errorMessage, exception)
        {
            Data = data;
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, string.Empty);
        }

        public new static Result<T> Failure(string errorMessage, Exception? exception = null)
        {
            return new Result<T>(false, default, errorMessage, exception);
        }

        public new static Result<T> Failure(Exception exception)
        {
            return new Result<T>(false, default, exception.Message, exception);
        }
    }
}
