using Core.Models;

namespace WebApp.Models.Tests
{
    public class DemoMessageTest
    {
        [Fact]
        public void Constructor_CopiesRoleAndContent_FromMessage()
        {
            // Arrange
            var originalMessage = new Message
            {
                Role = "user",
                Content = "Hello, world!"
            };

            // Act
            var demoMessage = new DemoMessage(originalMessage);

            // Assert
            Assert.Equal(originalMessage.Role, demoMessage.Role);
            Assert.Equal(originalMessage.Content, demoMessage.Content);
        }

        [Fact]
        public void Timestamp_IsSetToCurrentUtcTime_InIso8601FormatWithNewLine()
        {
            // Arrange
            var before = DateTime.UtcNow;
            var message = new Message { Role = "system", Content = "Test" };

            // Act
            var demoMessage = new DemoMessage(message);
            var timestamp = demoMessage.Timestamp;

            // Assert
            // The timestamp should contain a newline (from Replace("T", Environment.NewLine))
            Assert.Contains(Environment.NewLine, timestamp);

            // Parse the timestamp back to DateTime to check it's valid and recent
            var reconstructed = timestamp.Replace(Environment.NewLine, "T");
            var parsed = DateTime.Parse(reconstructed, null, System.Globalization.DateTimeStyles.RoundtripKind);

            Assert.True(parsed >= before && parsed <= DateTime.UtcNow.AddSeconds(1));
        }
    }
}