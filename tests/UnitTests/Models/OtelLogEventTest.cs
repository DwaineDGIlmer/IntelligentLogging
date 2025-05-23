using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json;
using WebApp.Models;

namespace UnitTests.Models
{
    public class OtelLogEventsTests
    {
        [Fact]
        public void Constructor_InitializesDefaults()
        {
            var evt = new OtelLogEvents();

            Assert.Equal(LogLevel.Information, evt.Level);
            Assert.Equal(string.Empty, evt.Body);
            Assert.Equal(string.Empty, evt.TraceId);
            Assert.Equal(string.Empty, evt.SpanId);
            Assert.Equal(string.Empty, evt.Source);
            Assert.Null(evt.CorrelationId);
            Assert.Null(evt.Exception);
            Assert.Null(evt.StackTrace);
            Assert.True((DateTimeOffset.UtcNow - evt.Timestamp).TotalSeconds < 5);
        }

        [Fact]
        public void Serialize_ProducesValidJson_WithAllFields()
        {
            var evt = new OtelLogEvents
            {
                Timestamp = DateTimeOffset.UtcNow,
                Body = "Test message",
                TraceId = "abc123",
                SpanId = "def456",
                Level = LogLevel.Warning,
                Source = "UnitTest",
                CorrelationId = "corr-789",
                Exception = new InvalidOperationException("fail"),
                StackTrace = new StackTrace()
            };

            var json = evt.Serialize();

            Assert.True(OtelLogEvents.IsJson(json));
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            Assert.Equal("WARNING", root.GetProperty("severity_text").GetString());
            Assert.Equal((int)LogLevel.Warning, root.GetProperty("severity_number").GetInt32());
            Assert.Equal("Test message", root.GetProperty("body").GetString());
            Assert.Equal("abc123", root.GetProperty("trace_id").GetString());
            Assert.Equal("def456", root.GetProperty("span_id").GetString());

            var attrs = root.GetProperty("attributes");
            Assert.Equal("UnitTest", attrs.GetProperty("source").GetString());
            Assert.Equal("corr-789", attrs.GetProperty("correlation_id").GetString());
            Assert.Equal(typeof(InvalidOperationException).FullName, attrs.GetProperty("exception.type").GetString());
            Assert.Equal("fail", attrs.GetProperty("exception.message").GetString());
            Assert.NotNull(attrs.GetProperty("exception.stacktrace").GetString());
        }

        [Fact]
        public void Serialize_OmitsNulls()
        {
            var evt = new OtelLogEvents
            {
                Body = "No exception",
                Source = "Test"
            };

            var json = evt.Serialize();
            using var doc = JsonDocument.Parse(json);
            var attrs = doc.RootElement.GetProperty("attributes");

            Assert.Equal("Test", attrs.GetProperty("source").GetString());
            Assert.False(attrs.TryGetProperty("exception.type", out _));
            Assert.False(attrs.TryGetProperty("exception.message", out _));
            Assert.False(attrs.TryGetProperty("exception.stacktrace", out _));
        }

        [Fact]
        public void FromOtelJson_ParsesAllFields()
        {
            var now = DateTimeOffset.UtcNow;
            var json = $@"
            {{
                ""timestamp"": {now.ToUnixTimeMilliseconds() * 1_000_000},
                ""body"": ""Parsed message"",
                ""trace_id"": ""tid"",
                ""span_id"": ""sid"",
                ""severity_number"": {(int)LogLevel.Error},
                ""attributes"": {{
                    ""source"": ""Parser"",
                    ""correlation_id"": ""cid""
                }}
            }}";

            var evt = OtelLogEvents.FromOtelJson(json);

            Assert.Equal("Parsed message", evt.Body);
            Assert.Equal("tid", evt.TraceId);
            Assert.Equal("sid", evt.SpanId);
            Assert.Equal(LogLevel.Error, evt.Level);
            Assert.Equal("Parser", evt.Source);
            Assert.Equal("cid", evt.CorrelationId);
            Assert.True(Math.Abs((evt.Timestamp - now).TotalSeconds) < 1);
        }

        [Theory]
        [InlineData(null, false)]
        [InlineData("", false)]
        [InlineData("not json", false)]
        [InlineData("{}", true)]
        [InlineData("[]", true)]
        [InlineData("{\"a\":1}", true)]
        [InlineData("[1,2,3]", true)]
        [InlineData("{invalid}", false)]
        public void IsJson_WorksAsExpected(string input, bool expected)
        {
            Assert.Equal(expected, OtelLogEvents.IsJson(input));
        }
    }
}