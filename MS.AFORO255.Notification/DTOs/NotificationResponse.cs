namespace MS.AFORO255.Notification.DTOs
{
    public class NotificationResponse
    {
        public int SendMailId { get; set; }
        public string SendDate { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Address { get; set; }
        public int AccountId { get; set; }
    }
}
