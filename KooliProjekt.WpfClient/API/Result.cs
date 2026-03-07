using System;

namespace KooliProjekt.WpfClient.API
{
    /// <summary>
    /// Result - representeerib operatsiooni tulemust (õnnestus või viga)
    /// Kasutatakse API kutsete tulemuste tagastamiseks
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Kas operatsioon õnnestus
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Veateade (ainult kui IsSuccess = false)
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Exception objekt (ainult kui IsSuccess = false)
        /// </summary>
        public Exception? Exception { get; }

        protected Result(bool isSuccess, string errorMessage, Exception? exception = null)
        {
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;
            Exception = exception;
        }

        /// <summary>
        /// Loo õnnestunud tulemus
        /// </summary>
        public static Result Success()
        {
            return new Result(true, string.Empty);
        }

        /// <summary>
        /// Loo ebaõnnestunud tulemus
        /// </summary>
        public static Result Failure(string errorMessage, Exception? exception = null)
        {
            return new Result(false, errorMessage, exception);
        }

        /// <summary>
        /// Loo ebaõnnestunud tulemus Exception'ist
        /// </summary>
        public static Result Failure(Exception exception)
        {
            return new Result(false, exception.Message, exception);
        }
    }
}
