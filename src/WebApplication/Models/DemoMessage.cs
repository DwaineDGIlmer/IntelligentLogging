namespace WebApp.Models;

/// <summary>
/// Represents a message with associated content and a timestamp indicating when the message was created.
/// </summary>
/// <remarks>This class provides a way to encapsulate a message and its associated metadata, such as the time of
/// creation. Instances of this class are immutable once created.</remarks>
sealed public class DemoMessage
{
    /// <summary>
    /// The content of the message.
    /// </summary>
    public string Message { get; } = string.Empty;

    /// <summary>
    /// Gets or sets the timestamp indicating when the event occurred.
    /// </summary>
    public string Timestamp { get; } = DateTime.UtcNow.ToString("o").Replace("T", Environment.NewLine);

    /// <summary>
    /// Used for testing.
    /// </summary>
    public DemoMessage() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DemoMessage"/> class with the specified message content.
    /// </summary>
    /// <param name="message">The message content to associate with this instance. Cannot be <see langword="null"/>.</param>
    public DemoMessage(string message)
    {
        Message = message;
    }
}
