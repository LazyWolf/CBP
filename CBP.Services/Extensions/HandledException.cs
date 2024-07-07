namespace CBP.Services.Extensions
{
    public class HandledException : Exception
    {
        public HandledException() { }
        public HandledException(string message) : base(message) { }
        public HandledException(string message, string[] errors) : base(message)
        {
            Errors.AddRange(errors);
        }
        public HandledException(string message, List<string> errors) : base(message)
        {
            Errors = errors;
        }

        public List<string> Errors { get; set; } = [];
    }
}
