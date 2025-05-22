using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Concurrent;
using UnitTests.Controllers.WebApp.Models;
using WebApp.Controllers;

namespace UnitTests.Controllers
{
    public class EventsControllerTest
    {
        // Helper to reset the singleton's queue for isolation
        private static void ClearEventQueue()
        {
            while (EventReaderSingleton.Instance.EventQueue.TryDequeue(out _)) { }
        }

        [Fact]
        public void Get_ReturnsEmpty_WhenNoEvents()
        {
            // Arrange
            ClearEventQueue();
            var loggerMock = new Mock<ILogger<EventsController>>();
            var controller = new EventsController(loggerMock.Object);

            // Act
            var result = controller.Get();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void Constructor_LogsInitialization()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<EventsController>>();

            // Act
            var controller = new EventsController(loggerMock.Object);

            // Assert
            loggerMock.Verify(
                l => l.Log(
                    LogLevel.Trace,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("EventsController initialized.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }

        [Fact]
        public void Get_LogsMethodCall()
        {
            // Arrange
            ClearEventQueue();
            var loggerMock = new Mock<ILogger<EventsController>>();
            var controller = new EventsController(loggerMock.Object);

            // Act
            controller.Get();

            // Assert
            loggerMock.Verify(
                l => l.Log(
                    LogLevel.Trace,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v!.ToString()!.Contains("Get() called.")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
                Times.Once);
        }
    }

    // Minimal stub for EventReaderSingleton for test context
    namespace WebApp.Models
    {
        public class EventReaderSingleton
        {
            private static readonly EventReaderSingleton _instance = new();
            public static EventReaderSingleton Instance => _instance;
            public ConcurrentQueue<string> EventQueue { get; } = new ConcurrentQueue<string>();
            private EventReaderSingleton() { }
        }
    }
}