namespace EduTrack.ViewModels
{
    public class EmailSettings
    {
        public string DeliveryMethod { get; set; } = "PickupDirectory"; // PickupDirectory or Smtp
        public string FromName { get; set; } = "EduTrack";
        public string FromEmail { get; set; } = "no-reply@edutrack.local";

        // SMTP settings
        public string SmtpHost { get; set; } = "localhost";
        public int SmtpPort { get; set; } = 25;
        public bool UseSsl { get; set; } = false;
        public string? Username { get; set; }
        public string? Password { get; set; }

        // Pickup directory (for local dev with smtp4dev/MailHog or file pickup)
        public string PickupDirectory { get; set; } = "Emails";
    }
}