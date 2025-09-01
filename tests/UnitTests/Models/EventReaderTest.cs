using System.Diagnostics.Tracing;
using WebApp.Models;

namespace UnitTests.Models
{
    public sealed class EventReaderTest
    {
        private class TestEventSource : EventSource
        {
            public static readonly TestEventSource Log = new TestEventSource();

            [Event(100)]
            public void WriteJsonEvent(string json)
            {
                WriteEvent(100, json);
            }

            [Event(101)]
            public void WriteJsonEvent101(string json)
            {
                WriteEvent(101, json);
            }

            [Event(102)]
            public void WriteOtherEvent(string message)
            {
                WriteEvent(102, message);
            }
        }

        [Fact]
        public void EventReader_DoesNotEnqueue_InvalidEventId()
        {
            // Arrange
            var reader = new EventReader();
            string message = "not json";

            // Act
            TestEventSource.Log.WriteOtherEvent(message);

            // Allow time for async event delivery
            Thread.Sleep(100);

            // Assert
            Assert.Empty(reader.EventQueue);
        }

        [Fact]
        public void EventReaderSingleton_ReturnsSameInstance()
        {
            var instance1 = EventReaderSingleton.Instance;
            var instance2 = EventReaderSingleton.Instance;
            Assert.Same(instance1, instance2);
        }
    }
}