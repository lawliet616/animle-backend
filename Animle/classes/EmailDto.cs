namespace Animle.interfaces
{
    public class EmailDto
    {
        public string Email { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string? From { get; set; }

    }
}
