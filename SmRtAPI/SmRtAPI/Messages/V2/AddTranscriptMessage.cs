namespace Speechmatics.Realtime.Client.Messages.V2
{
    /// <summary>
    /// Detailed timings for a transcript
    /// </summary>
    public class AddTranscriptMessage : BaseMessage
    {
        /// <summary>
        /// Message type
        /// </summary>
        public override string message => "AddTranscript";
        /// <summary>
        /// Individual word data
        /// </summary>
        public WordSubMessage[] results;
    }
}