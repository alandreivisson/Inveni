using System;
using System.Threading.Tasks;
using Inveni.Settings;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;

namespace Inveni.Services {
    public class GMailService : IEmailService {
        private readonly GMailSettings _emailSettings;

        public GMailService(IOptions<GMailSettings> emailSettings) {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string emailDestinatario, string assunto, string mensagemTexto, string mensagemHtml) {
            var mensagem = new MimeMessage();
            mensagem.From.Add(new MailboxAddress(_emailSettings.NomeRemetente, _emailSettings.EmailRemetente));
            mensagem.To.Add(MailboxAddress.Parse(emailDestinatario));
            mensagem.Subject = assunto;

            var builder = new BodyBuilder { TextBody = mensagemTexto, HtmlBody = mensagemHtml };
            mensagem.Body = builder.ToMessageBody();

            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await smtpClient.ConnectAsync(_emailSettings.EnderecoServidor, _emailSettings.PortaServidor, _emailSettings.UsarSsl);

                    // Autenticação com o servidor SMTP do Gmail
                    await smtpClient.AuthenticateAsync(_emailSettings.EmailRemetente, _emailSettings.SmtpPassword);

                    // Envio da mensagem
                    await smtpClient.SendAsync(mensagem);

                    await smtpClient.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Erro ao enviar e-mail: {ex.Message}");
            }
        }
    }
}
