using System.Collections.Generic;
using Speechmatics.Realtime.Client.Messages;

namespace Speechmatics.Realtime.Client.Messages
{
    internal class SetRecognitionConfigMessage : BaseMessage
    {
        public override string message => "SetRecognitionConfig";
        public Dictionary<string, object> transcription_config { get; }

        public SetRecognitionConfigMessage(AdditionalVocabSubMessage additionalVocab = null, 
            string outputLocale = null
            )
        {
            transcription_config = new Dictionary<string, object>();
            transcription_config["language"] = "en";
            if (additionalVocab != null)
            {
                transcription_config["additional_vocab"] = additionalVocab.Data;
            }
            if (outputLocale != null)
            {
                transcription_config["output_locale"] = outputLocale;
            }
        }
    }
}
