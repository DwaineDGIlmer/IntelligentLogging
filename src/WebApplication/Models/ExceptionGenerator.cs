namespace WebApp.Models
{
    /// <summary>
    /// Provides functionality to generate and log random exceptions for testing purposes.
    /// </summary>
    public sealed class ExceptionGenerator
    {
        /// <summary>
        /// A list of factory methods for creating different types of exceptions.
        /// </summary>
        private static readonly List<Func<Exception>> ExceptionFactories =
        [
            () => new ArgumentNullException("param", "Parameter cannot be null."),
            () => new InvalidOperationException("Operation is not valid."),
            () => new DivideByZeroException("Attempted to divide by zero."),
            () => new FormatException("Input string was not in a correct format."),
            () => new IndexOutOfRangeException("Index was outside the bounds of the array."),
            () => new NotImplementedException("This method is not implemented."),
            () => new TimeoutException("The operation has timed out.")
        ];

        private static readonly Random Random = new();

        /// <summary>
        /// Generates a random exception from the predefined list and returns it.
        /// The exception is thrown and caught to ensure it contains a stack trace.
        /// </summary>
        /// <returns>
        /// An <see cref="Exception"/> instance with a stack trace.
        /// </returns>
        public Exception GenerateRandomException()
        {
            int index = Random.Next(ExceptionFactories.Count);
            try
            {
                throw ExceptionFactories[index]();
            }
            catch (Exception ex)
            {
                return ex; // Exception now has a stack trace
            }
        }

        /// <summary>
        /// Generates a random exception and logs it using the specified logging action.
        /// </summary>
        /// <param name="logAction">
        /// An <see cref="Action{Exception}"/> delegate to handle the logging of the generated exception.
        /// </param>
        public void GenerateAndLogRandomException(Action<Exception> logAction)
        {
            try
            {
                throw GenerateRandomException();
            }
            catch (Exception ex)
            {
                logAction?.Invoke(ex);
            }
        }
    }
}