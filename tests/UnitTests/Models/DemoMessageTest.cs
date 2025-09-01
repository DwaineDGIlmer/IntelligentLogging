using WebApp.Models;

namespace UnitTests.Models
{
    public sealed class DemoMessageTest
    {
        [Fact]
        public void DefaultConstructor_SetsMessageToEmptyString()
        {
            var demoMessage = new DemoMessage();
            Assert.Equal(string.Empty, demoMessage.Message);
        }

        [Fact]
        public void DefaultConstructor_SetsTimestampToCurrentUtcTimeInIsoFormat()
        {
            var before = DateTime.UtcNow;
            var demoMessage = new DemoMessage();
            var after = DateTime.UtcNow;

            // Timestamp should be in ISO 8601 format with 'T' replaced by newline
            var timestamp = demoMessage.Timestamp;
            Assert.Contains(Environment.NewLine, timestamp);

            var reconstructed = timestamp.Replace(Environment.NewLine, "T");
            Assert.True(DateTime.TryParse(reconstructed, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime parsed));
            Assert.InRange(parsed, before.AddSeconds(-1), after.AddSeconds(1));
        }

        [Fact]
        public void Constructor_WithMessage_SetsMessageProperty()
        {
            var msg = "Hello, world!";
            var demoMessage = new DemoMessage(msg);
            Assert.Equal(msg, demoMessage.Message);
        }

        [Fact]
        public void Constructor_WithMessage_SetsTimestampToCurrentUtcTimeInIsoFormat()
        {
            var before = DateTime.UtcNow;
            var demoMessage = new DemoMessage("test");
            var after = DateTime.UtcNow;

            var timestamp = demoMessage.Timestamp;
            Assert.Contains(Environment.NewLine, timestamp);

            var reconstructed = timestamp.Replace(Environment.NewLine, "T");
            DateTime parsed;
            Assert.True(DateTime.TryParse(reconstructed, null, System.Globalization.DateTimeStyles.RoundtripKind, out parsed));
            Assert.InRange(parsed, before.AddSeconds(-1), after.AddSeconds(1));
        }
    }
}
