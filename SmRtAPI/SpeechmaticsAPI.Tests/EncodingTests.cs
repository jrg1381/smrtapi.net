using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Speechmatics.Realtime.Client;
using Speechmatics.Realtime.Client.Enumerations;
using Speechmatics.Realtime.Client.Messages;
using V2 = Speechmatics.Realtime.Client.Messages.V2;
using V1 = Speechmatics.Realtime.Client.Messages.V1;

namespace SpeechmaticsAPI.Tests
{
    [TestFixture]
    public class EncodingTests
    {
        [Test]
        public void Constructor()
        {
            var url = "wss://this/";
            var s = new SmRtApi(url, Stream.Null, new SmRtApiConfig("ru"));
            Assert.AreEqual(url, s.WsUrl.AbsoluteUri, "Get WS url back");
        }

        [Test]
        public void StartRecognitionToJson()
        {
            // This is just a sanity check for now
            var expected =
                "{\"message\":\"StartRecognition\",\"model\":\"en-US\",\"audio_format\":{\"sample_rate\":44100,\"type\":\"raw\",\"encoding\":\"pcm_s16le\"},\"output_format\":{\"type\":\"json\"},\"auth_token\":\"\",\"user\":1}";
            var audioFormat = new V1.AudioFormatSubMessage(AudioFormatType.Raw, AudioFormatEncoding.PcmS16Le, 44100);
            var msg = new V1.StartRecognitionMessage(audioFormat, "en-US", OutputFormat.Json);
            Assert.AreEqual(expected, msg.AsJson(), "Message serialization unexpected");
        }

        [Test]
        public void StartRecognitionV2ToJson()
        {
            // This is just a sanity check for now
            var expected = "{\"message\":\"StartRecognition\",\"audio_format\":{\"sample_rate\":44100,\"type\":\"raw\",\"encoding\":\"pcm_s16le\"},\"transcription_config\":{\"language\":\"en-US\"}}";
            var audioFormat = new V2.AudioFormatSubMessage(AudioFormatType.Raw, AudioFormatEncoding.PcmS16Le, 44100);
            var msg = new V2.StartRecognitionMessage(audioFormat, "en-US");
            Assert.AreEqual(expected, msg.AsJson(), "Message serialization unexpected");
        }

        [Test]
        public void SetRecognitionConfigV1ToJson()
        {
            // This is just a sanity check for now
            var expected =
                "{\"message\":\"SetRecognitionConfig\",\"config\":{\"additional_vocab\":[\"foo\",{\"content\":\"foo\",\"sounds_like\":[\"fooo\",\"barrr\"]}],\"output_locale\":\"en-GB\"}}";
            var x = new Dictionary<string, IEnumerable<string>> { ["foo"] = new List<string> { "fooo", "barrr" } };

            var config = new AdditionalVocabSubMessage(new[] { "foo" }, x);
            var msg = new V1.SetRecognitionConfigMessage(config, "en-GB");
            var y = msg.AsJson();
            Assert.AreEqual(expected, msg.AsJson(), "Message serialization unexpected");
        }

        [Test]
        public void SetRecognitionConfigV2ToJson()
        {
            // This is just a sanity check for now
            var expected =
            "{\"message\":\"SetRecognitionConfig\",\"transcription_config\":{\"language\":\"en\",\"additional_vocab\":[\"foo\",{\"content\":\"foo\",\"sounds_like\":[\"fooo\",\"barrr\"]}],\"output_locale\":\"en-GB\"}}";
            var x = new Dictionary<string, IEnumerable<string>> { ["foo"] = new List<string> { "fooo", "barrr" } };

            var config = new AdditionalVocabSubMessage(new[] { "foo" }, x);
            var msg = new V2.SetRecognitionConfigMessage(config,"en-GB");
            var y = msg.AsJson();
            Assert.AreEqual(expected, msg.AsJson(), "Message serialization unexpected");
        }
    }
}
