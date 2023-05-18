using Speechmatics.Realtime.Client.Messages;

namespace Speechmatics.Realtime.Client.Messages
{
    /// <summary>
    /// A partial transcript -- later messages may improve on it
    /// </summary>
    public class AddPartialTranscriptMessage : BaseMessage
    {
        /// <summary>
        /// Message type
        /// </summary>
        public override string message => "AddPartialTranscript";
        /// <summary>
        /// Start time
        /// </summary>
        public double start_time;
        /// <summary>
        /// Length of audio segment
        /// </summary>
        public double length;
        /// <summary>
        /// Transcript ext
        /// </summary>
        public string transcript;
        /// <summary>
        /// Individual word data
        /// </summary>
        public WordSubMessage[] words;
    }
}