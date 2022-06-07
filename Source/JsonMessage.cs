namespace SecMailClient{
	public class JsonMessage
    {
        public string From = "";
        public string Subject = "";
        public byte[] Content;
        public DateTime DateTime = DateTime.MinValue;
        public string To = "";
        public List<MessageAttachment> Attachments = new List<MessageAttachment>();

        public JsonMessage(string from, string subject, byte[] content)
        {
            From = from;
            Subject = subject;
            Content = content;
        }

        public JsonMessage Now()
        {
            DateTime = DateTime.Now;
            return this;
        }
        public string GetJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}