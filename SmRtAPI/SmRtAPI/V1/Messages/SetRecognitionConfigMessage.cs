﻿using System.Collections.Generic;
using Speechmatics.Realtime.Client.Messages;

namespace Speechmatics.Realtime.Client.V1.Messages
{
    internal class SetRecognitionConfigMessage : BaseMessage
    {
        public string message => "SetRecognitionConfig";
        public Dictionary<string, object> config { get; }

        public SetRecognitionConfigMessage(AdditionalVocabSubMessage additionalVocab = null, 
            string outputLocale = null,
            DynamicTranscriptConfiguration dynamicTranscript = null
            )
        {
            config = new Dictionary<string, object>();
            if (additionalVocab != null)
            {
                config["additional_vocab"] = additionalVocab.Data;
            }
            if (outputLocale != null)
            {
                config["output_locale"] = outputLocale;
            }
            if (dynamicTranscript != null)
            {
                if (dynamicTranscript.UseDefaults)
                {
                    config["dynamic_transcript"] = new
                    {
                        enabled = dynamicTranscript.Enabled
                    };
                }
                else
                {
                    config["dynamic_transcript"] = new
                    {
                        max_chars = dynamicTranscript.MaxChars,
                        min_context = dynamicTranscript.MinContext,
                        max_delay = dynamicTranscript.MaxDelay,
                        enabled = dynamicTranscript.Enabled
                    };
                }
            }
        }
    }
}
