using Loggers.Publishers;
using System.Collections.Concurrent;
using System.Diagnostics.Tracing;

namespace WebApp.Models
{
    /// <summary>
    /// Listens for events from an <see cref="EventSource"/> and queues the event data as JSON strings.
    /// </summary>
    /// <remarks>This class extends <see cref="EventListener"/> to capture events from a specific <see
    /// cref="EventSource"/>  and enqueue the event payloads as JSON strings into a thread-safe <see
    /// cref="ConcurrentQueue{T}"/>. Only events with IDs 100 or 101 and non-empty payloads are processed. The first
    /// payload item is  expected to be a JSON string.</remarks>
    public class EventReader : EventListener
    {
        /// <summary>
        /// Gets the thread-safe queue used to store event messages.
        /// </summary>
        /// <remarks>This property provides a thread-safe mechanism for enqueuing and dequeuing event
        /// messages.  It is suitable for scenarios where multiple threads need to produce or consume events
        /// concurrently.</remarks>
        public ConcurrentQueue<string> EventQueue { get; } = new ConcurrentQueue<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventReader"/> class and enables event logging for the
        /// associated event source.
        /// </summary>
        /// <remarks>This constructor enables events for the event source defined by <see
        /// cref="EventSourcePublisher.Log"/>  with a logging level of <see cref="EventLevel.LogAlways"/>.  It is
        /// intended to set up the event reader for capturing all events from the specified source.</remarks>
        public EventReader()
        {
            // Enable the event source for the specified event source name
            EnableEvents(EventSourcePublisher.Log, EventLevel.LogAlways);
        }

        /// <summary>
        /// Handles the event written by the event source and processes its payload.
        /// </summary>
        /// <remarks>This method processes events with specific IDs (100 or 101) and enqueues the first
        /// payload item as a JSON string if it is not null or empty. Only events with a non-null and non-empty payload
        /// are considered. The method is invoked automatically when an event is written by the associated event
        /// source.</remarks>
        /// <param name="eventData">The event data containing information about the event, including its ID and payload.</param>
        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            // Check if the event is from the expected event source
            if ((eventData.EventId == 100 || eventData.EventId == 101) &&
                eventData.Payload != null && eventData.Payload.Count > 0)
            {
                // Only take the first payload item, which should be the JSON string
                var json = eventData.Payload[0]?.ToString();
                if (!string.IsNullOrEmpty(json))
                {
                    EventQueue.Enqueue(json);
                }
            }
        }
    }

    /// <summary>
    /// Provides a singleton instance of the <see cref="EventReader"/> class.
    /// </summary>
    /// <remarks>This class ensures that a single, shared instance of <see cref="EventReader"/> is available
    /// for use throughout the application. The instance is thread-safe and can be accessed directly via the  <see
    /// cref="Instance"/> field.</remarks>
    public static class EventReaderSingleton
    {
        /// <summary>
        /// Gets the singleton instance of the <see cref="EventReader"/> class.
        /// </summary>
        /// <remarks>This instance provides a thread-safe, shared <see cref="EventReader"/> for use throughout the application.</remarks>
        public static readonly EventReader Instance = new();
    }
}

