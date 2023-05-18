﻿using Speechmatics.Realtime.Client.Messages;

namespace Speechmatics.Realtime.Client.V2.Messages
{
    /// <summary>
    /// Metadata that aggregates information for a whole parital or final message.
    /// </summary>
    public class RecognitionMetadata : BaseMessage
    {

        /// <summary>
        /// Concatenated tokens for this message
        /// </summary>
        public string transcript;

        /// <summary>
        /// Start time of first token in seconds.
        /// </summary>
        public float start_time;

        /// <summary>
        /// End time of last token in seconds.
        /// </summary>
        public float end_time;
    }
}