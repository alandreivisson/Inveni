namespace Inveni.Settings {
    public class GMailSettings 
    {
        public string NomeRemetente { get; set; }
        public string EmailRemetente { get; set; }
        public string SmtpPassword { get; set; }
        public string EnderecoServidor { get; set; }
        public int PortaServidor { get; set; }
        public bool UsarSsl { get; set; }

    }
}
