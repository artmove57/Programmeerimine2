using System;

namespace KooliProjekt.WpfClient.API
{
    /// <summary>
    /// Result<T> - representeerib operatsiooni tulemust koos andmetega
    /// Kasutatakse API kutsete tulemuste tagastamiseks, kui on vaja tagastada andmeid
    /// </summary>
    /// <typeparam name="T">Tagastatavate andmete tüüp</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// Operatsiooni tulemusena saadud andmed (ainult kui IsSuccess = true)
        /// </summary>
        public T? Data { get; }

        private Result(bool isSuccess, T? data, string errorMessage, Exception? exception = null)
            : base(isSuccess, errorMessage, exception)
        {
            Data = data;
        }

        /// <summary>
        /// Loo õnnestunud tulemus koos andmetega
        /// </summary>
        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, string.Empty);
        }

        /// <summary>
        /// Loo ebaõnnestunud tulemus
        /// </summary>
        public new static Result<T> Failure(string errorMessage, Exception? exception = null)
        {
            return new Result<T>(false, default, errorMessage, exception);
        }

        /// <summary>
        /// Loo ebaõnnestunud tulemus Exception'ist
        /// </summary>
        public new static Result<T> Failure(Exception exception)
        {
            return new Result<T>(false, default, exception.Message, exception);
        }
    }
}
