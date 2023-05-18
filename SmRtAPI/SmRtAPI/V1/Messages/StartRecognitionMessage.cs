﻿using Speechmatics.Realtime.Client.Enumerations;
using Speechmatics.Realtime.Client.Messages;

namespace Speechmatics.Realtime.Client.V1.Messages
{
    internal class StartRecognitionMessage : BaseMessage
    {
        public StartRecognitionMessage(AudioFormatSubMessage audioFormatSubMessage, string model, OutputFormat outputFormat, string authToken = "", int user = 1)
        {
            audio_format = audioFormatSubMessage;
            this.model = model;
            output_format = new OutputFormatSubMessage(outputFormat);
            auth_token = authToken;
            this.user = user;
        }

        public string message => "StartRecognition";
        public string model { get; }
        public AudioFormatSubMessage audio_format { get; }
        public OutputFormatSubMessage output_format { get; }
        public string auth_token { get; }
        public int user { get; }
    }
}