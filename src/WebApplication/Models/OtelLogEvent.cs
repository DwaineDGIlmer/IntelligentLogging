//using Core.Contracts;
//using Core.Extensions;
//using Loggers.Contracts;
//using System.Diagnostics;
//using System.Text.Json;

//namespace WebApp.Models;

///// <summary>
///// Represents a log event in an OpenTelemetry-compliant format, including metadata such as timestamp, severity,
///// trace identifiers, and optional exception details.
///// </summary>
///// <remarks>This class is designed to facilitate structured logging in distributed systems by adhering to
///// OpenTelemetry conventions. It provides properties for common log event attributes, such as the log level,
///// message body, and trace/span identifiers. Additionally, it includes methods for serializing the log event to a
///// JSON format suitable for OpenTelemetry ingestion.</remarks>
//public class OtelLogEvents : ILogEvent
//{
//    /// <summary>
//    /// Initializes a new instance of the <see cref="OtelLogEvents"/> class with default values.
//    /// </summary>
//    public OtelLogEvents()
//    {
//        Timestamp = DateTimeOffset.UtcNow;
//        Body = string.Empty;
//        TraceId = string.Empty;
//        SpanId = string.Empty;
//        Level = LogLevel.Information;
//        Source = string.Empty;
//    }


//    /// <summary>
//    /// Gets or sets the timestamp of the log event in UTC.
//    /// </summary>
//    public DateTimeOffset Timestamp { get; set; }

//    /// <summary>
//    /// Gets or sets the main log message body.
//    /// </summary>
//    public string Body { get; set; }

//    /// <summary>
//    /// Gets or sets the trace identifier associated with the log event.
//    /// </summary>
//    public string TraceId { get; set; }

//    /// <summary>
//    /// Gets or sets the span identifier associated with the log event.
//    /// </summary>
//    public string SpanId { get; set; }

//    /// <summary>
//    /// Gets or sets the log level (severity) of the event.
//    /// </summary>
//    public LogLevel Level { get; set; }

//    /// <summary>
//    /// Gets or sets the source of the log event.
//    /// </summary>
//    public string Source { get; set; }

//    /// <summary>
//    /// Gets or sets the optional correlation identifier for distributed tracing.
//    /// </summary>
//    public string? CorrelationId { get; set; }

//    /// <summary>
//    /// Gets or sets the exception associated with the log event, if any.
//    /// </summary>
//    public Exception? Exception { get; set; }

//    /// <summary>
//    /// Gets or sets the stack trace associated with the log event, if any.
//    /// </summary>
//    public StackTrace? StackTrace { get; set; }

//    /// <summary>
//    /// Serializes the log event to a JSON string in OpenTelemetry-compliant format, omitting null values.
//    /// </summary>
//    /// <returns>A JSON string representing the log event.</returns>
//    public string Serialize()
//    {
//        var otelLog = new Dictionary<string, object?>
//        {
//            ["timestamp"] = Timestamp.ToUnixTimeMilliseconds() * 1_000_000, // nanoseconds
//            ["severity_text"] = Level.ToString().ToUpperInvariant(),
//            ["severity_number"] = (int)Level,
//            ["body"] = Body,
//            ["trace_id"] = string.IsNullOrWhiteSpace(TraceId) ? null : TraceId,
//            ["span_id"] = string.IsNullOrWhiteSpace(SpanId) ? null : SpanId,
//            ["attributes"] = new Dictionary<string, object?>
//            {
//                ["source"] = Source,
//                ["correlation_id"] = CorrelationId,
//                ["exception.type"] = Exception?.GetType().FullName,
//                ["exception.message"] = Exception?.Message,
//                ["exception.stacktrace"] = Exception != null ? (StackTrace?.ToString() ?? Exception.StackTrace) : null
//            }
//        };

//        // Remove nulls for OTEL compliance
//        otelLog["attributes"] = ((Dictionary<string, object?>)otelLog["attributes"]!).RemoveNullValues();

//        // Will fail if the instance is not initialized
//        return JsonSerializer.Serialize(otelLog);
//    }

//    /// <summary>
//    /// Parses a JSON string representing OpenTelemetry log events and converts it into an <see cref="OtelLogEvents"/>
//    /// object.
//    /// </summary>
//    /// <remarks>This method extracts key properties such as <c>timestamp</c>, <c>body</c>, <c>trace_id</c>,
//    /// <c>span_id</c>, <c>severity_number</c>, and attributes like <c>source</c> and <c>correlation_id</c> from the
//    /// JSON structure. The <c>timestamp</c> is converted from Unix time in nanoseconds to a <see
//    /// cref="DateTimeOffset"/>.</remarks>
//    /// <param name="json">A JSON string containing OpenTelemetry log event data. Must not be <see langword="null"/> or empty.</param>
//    /// <returns>An <see cref="OtelLogEvents"/> object populated with the data extracted from the provided JSON string.</returns>
//    public static OtelLogEvents FromOtelJson(string json)
//    {
//        using var doc = JsonDocument.Parse(json);
//        var root = doc.RootElement;

//        var otel = new OtelLogEvents();

//        if (root.TryGetProperty("timestamp", out var ts))
//            otel.Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(ts.GetInt64() / 1_000_000);

//        if (root.TryGetProperty("body", out var body))
//            otel.Body = body.GetString() ?? "";

//        if (root.TryGetProperty("trace_id", out var traceId) && traceId.ValueKind != JsonValueKind.Null)
//            otel.TraceId = traceId.GetString() ?? "";

//        if (root.TryGetProperty("span_id", out var spanId) && spanId.ValueKind != JsonValueKind.Null)
//            otel.SpanId = spanId.GetString() ?? "";

//        if (root.TryGetProperty("severity_number", out var sevNum))
//            otel.Level = (LogLevel)sevNum.GetInt32();

//        if (root.TryGetProperty("attributes", out var attrs))
//        {
//            if (attrs.TryGetProperty("source", out var source))
//                otel.Source = source.GetString() ?? "";
//            if (attrs.TryGetProperty("correlation_id", out var corr))
//                otel.CorrelationId = corr.GetString();
//            // Add more attribute mappings as needed
//        }

//        return otel;
//    }

//    /// <summary>
//    /// Determines whether the specified string is a valid JSON object or array.
//    /// </summary>
//    /// <remarks>The method checks if the input string is non-empty, properly formatted as JSON (starting and
//    /// ending with curly braces for objects or square brackets for arrays), and can be successfully parsed as
//    /// JSON.</remarks>
//    /// <param name="input">The string to evaluate. This can be any text input.</param>
//    /// <returns><see langword="true"/> if the input is a valid JSON object or array; otherwise, <see langword="false"/>.</returns>
//    public static bool IsJson(string input)
//    {
//        if (string.IsNullOrWhiteSpace(input))
//            return false;

//        input = input.Trim();
//        if (!(input.StartsWith("{") && input.EndsWith("}")) &&
//            !(input.StartsWith("[") && input.EndsWith("]")))
//            return false;

//        try
//        {
//            using (JsonDocument.Parse(input)) { }
//            return true;
//        }
//        catch
//        {
//            return false;
//        }
//    }
//}