using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using WebApp.Models;

namespace WebApp.Controllers.Tests;

public sealed class ChatGptMessagesControllerTest
{
    // Helper class to mock EventReaderSingleton and its queue
    private class MockEventReaderSingleton : IDisposable
    {
        private EventReader Instance { get; } = new EventReader();
        public virtual ConcurrentQueue<DemoMessage> ChatGptMessageQueue { get; }

        public MockEventReaderSingleton(IEnumerable<DemoMessage> messages)
        {
            // Replace singleton instance with a mock
            ChatGptMessageQueue = new ConcurrentQueue<DemoMessage>(messages);
            Instance.ChatGptMessageQueue = ChatGptMessageQueue;
        }

        public void Dispose()
        {
            // Restore original singleton instance
            Instance.Dispose();
        }

        // Custom singleton for testing
        private class EventReaderSingletonForTest : MockEventReaderSingleton
        {
            public override ConcurrentQueue<DemoMessage> ChatGptMessageQueue { get; }

            public EventReaderSingletonForTest(ConcurrentQueue<DemoMessage> queue) : base(queue.ToList())
            {
                // Replace the singleton instance with a mock
                ChatGptMessageQueue = queue;
            }
        }
    }

    [Fact]
    public void Get_ReturnsOkWithMessages_WhenMessagesExist()
    {
        // Arrange
        var messages = new List<DemoMessage>
        {
            new("Hello"),
            new("Hi there!")
        };

        using (new MockEventReaderSingleton(messages))
        {
            var controller = new ChatGptMessagesController();
            controller.ChatGptMessageQueue = new ConcurrentQueue<DemoMessage>(messages);

            // Act
            var result = controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMessages = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Equal(2, returnedMessages.Count());
        }
    }

    [Fact]
    public void Get_ReturnsOkWithEmptyCollection_WhenNoMessagesExist()
    {
        // Arrange
        using (new MockEventReaderSingleton(new List<DemoMessage>()))
        {
            var controller = new ChatGptMessagesController();

            // Act
            var result = controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMessages = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);
            Assert.Empty(returnedMessages);
        }
    }
}