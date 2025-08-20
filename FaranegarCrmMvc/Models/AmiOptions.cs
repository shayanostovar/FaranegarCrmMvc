namespace FaranegarCrmMvc.Models
{
    public class AmiOptions
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } = 5038;
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int ReconnectDelaySeconds { get; set; } = 5;
    }
}
