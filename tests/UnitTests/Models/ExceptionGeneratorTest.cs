using WebApp.Models;

namespace UnitTests.Models
{
    public class ExceptionGeneratorTest
    {
        [Fact]
        public void GenerateRandomException_ReturnsExceptionWithStackTrace()
        {
            // Arrange
            var generator = new ExceptionGenerator();

            // Act
            var ex = generator.GenerateRandomException();

            // Assert
            Assert.NotNull(ex);
            Assert.False(string.IsNullOrWhiteSpace(ex.StackTrace));
            Assert.IsAssignableFrom<Exception>(ex);
        }

        [Fact]
        public void GenerateRandomException_ReturnsOneOfExpectedExceptionTypes()
        {
            // Arrange
            var generator = new ExceptionGenerator();
            var expectedTypes = new[]
            {
                typeof(ArgumentNullException),
                typeof(InvalidOperationException),
                typeof(DivideByZeroException),
                typeof(FormatException),
                typeof(IndexOutOfRangeException),
                typeof(NotImplementedException),
                typeof(TimeoutException)
            };

            // Act
            var ex = generator.GenerateRandomException();

            // Assert
            Assert.Contains(ex.GetType(), expectedTypes);
        }

        [Fact]
        public void GenerateAndLogRandomException_InvokesLogActionWithException()
        {
            // Arrange
            var generator = new ExceptionGenerator();
            Exception loggedException = null!;

            // Act
            generator.GenerateAndLogRandomException(ex => loggedException = ex);

            // Assert
            Assert.NotNull(loggedException);
            Assert.IsAssignableFrom<Exception>(loggedException);
        }

        [Fact]
        public void GenerateAndLogRandomException_DoesNotThrow_WhenLogActionIsNull()
        {
            // Arrange
            var generator = new ExceptionGenerator();

            // Act & Assert
            var exception = Record.Exception(() => generator.GenerateAndLogRandomException(null!));
            Assert.Null(exception);
        }
    }
}