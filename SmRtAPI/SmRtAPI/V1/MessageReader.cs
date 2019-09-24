using System;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Speechmatics.Realtime.Client.Enumerations;
using Speechmatics.Realtime.Client.Messages;
using Speechmatics.Realtime.Client.V1.Interfaces;
using Speechmatics.Realtime.Client.V1.Messages;

namespace Speechmatics.Realtime.Client.V1
{
    internal class MessageReader
    {
        private readonly RtServerVersion _serverVersion;
        private string _lastPartial;
        private int _ackedSequenceNumbers;
        private readonly ClientWebSocket _wsClient;
        private readonly AutoResetEvent _resetEvent;
        private readonly AutoResetEvent _recognitionStarted;
        private readonly ISmRtApi _api;

        internal MessageReader(ISmRtApi smRtApi, ClientWebSocket client, AutoResetEvent resetEvent, AutoResetEvent recognitionStarted, RtServerVersion serverVersion)
        {
            _serverVersion = serverVersion;
            _api = smRtApi;
            _wsClient = client;
            _resetEvent = resetEvent;
            _recognitionStarted = recognitionStarted;
        }

        internal async Task Start()
        {
            var receiveBuffer = new ArraySegment<byte>(new byte[32768]);

            while (true)
            {
                var webSocketReceiveResult = await _wsClient.ReceiveAsync(receiveBuffer, _api.CancelToken);
                if (ProcessMessage(webSocketReceiveResult, receiveBuffer))
                {
                    _resetEvent.Set();
                    break;
                }
            }
        }

        private bool ProcessMessage(WebSocketReceiveResult result, ArraySegment<byte> message)
        {
            // Return true if the message should cause the loop to exit, false otherwise.

            var subset = new ArraySegment<byte>(message.Array, 0, result.Count);
            var messageAsString = Encoding.UTF8.GetString(subset.ToArray());
            var jsonObject = JObject.Parse(messageAsString);

            Trace.WriteLine("55: " + messageAsString);

            switch (jsonObject.Value<string>("message"))
            {
                case "RecognitionStarted":
                {
                    Trace.WriteLine("Recognition started");
                    _recognitionStarted.Set();
                    break;
                }
                case "DataAdded":
                {
                    // Log the ack
                    Interlocked.Increment(ref _ackedSequenceNumbers);
                    break;
                }
                case "AddTranscript":
                {
                    string transcript;
                    switch (_serverVersion)
                    {
                        // Normally you would use a v1/v2 version of the message reader via some kind of polymorphism, rather
                        // than have conditional code like this, but that can be done later when I have time.
                        case RtServerVersion.V1:
                        {
                            transcript = jsonObject.Value<string>("results");
                            _api.Configuration.AddTranscriptMessageCallback?.Invoke(JsonConvert.DeserializeObject<AddTranscriptMessage>(messageAsString));
                                    break;
                        }
                        case RtServerVersion.V2:
                        {
                            transcript = jsonObject["metadata"]["transcript"].Value<string>();
                            _api.Configuration.AddTranscriptMessageCallback?.Invoke(JsonConvert.DeserializeObject<AddTranscriptMessage>(messageAsString));
                            break;
                        }
                        default:
                        {
                            throw new ArgumentException("Unknown server version enumeration value");
                        }
                    }
                    _api.Configuration.AddTranscriptCallback?.Invoke(transcript);
                    break;
                }
                case "AddPartialTranscript":
                {
                    _lastPartial = jsonObject.Value<string>("transcript");
                    _api.Configuration.AddPartialTranscriptMessageCallback?.Invoke(JsonConvert.DeserializeObject<AddPartialTranscriptMessage>(messageAsString));
                    break;
                }
                case "EndOfTranscript":
                {
                    // Sometimes there is a partial without a corresponding transcript, let's pretend it was a transcript here.
                    _api.Configuration.AddTranscriptCallback?.Invoke(_lastPartial);
                    _api.Configuration.EndOfTranscriptCallback?.Invoke();
                    return true;
                }
                case "Error":
                {
                    _api.Configuration.ErrorMessageCallback?.Invoke(JsonConvert.DeserializeObject<ErrorMessage>(messageAsString));
                    return true;
                }
                case "Warning":
                {
                    _api.Configuration.WarningMessageCallback?.Invoke(JsonConvert.DeserializeObject<WarningMessage>(messageAsString));
                    break;
                }
                default:
                {
                    Trace.WriteLine(messageAsString);
                    break;
                }
            }
            return result.MessageType == WebSocketMessageType.Close;
        }
    }
}