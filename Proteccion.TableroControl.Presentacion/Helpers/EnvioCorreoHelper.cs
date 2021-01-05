using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Proteccion.TableroControl.Presentacion.Helpers
{
    public class EnvioCorreoHelper : IEmail
    {
        public EmailSettings EmailSettings { get; }
        public AzureConfig AzureConfig { get; }

        public EnvioCorreoHelper(IOptions<EmailSettings> emailSettings, IOptions<AzureConfig> azureConfig)
        {
            EmailSettings = emailSettings.Value;
            AzureConfig = azureConfig.Value;
        }

        public async Task<bool> SendEmailAzureAsync(string asunto, string contenido, string email)
        {
            var token = await ObtenerToken();
            if (token != null)
            {
                var recipients = new List<ToRecipient>();
                foreach (var item in email.Split(';'))
                {
                    recipients.Add(new ToRecipient
                    {
                        EmailAddress = new EmailAddress
                        {
                            Address = item
                        }
                    });
                }

                var envio = new EnvioRequest
                {
                    Message = new Message
                    {
                        Subject = asunto,
                        Body = new Body
                        {
                            Content = contenido,
                            ContentType = "HTML"
                        },
                        ToRecipients = recipients
                    }
                };

                var cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);

                var json = JsonConvert.SerializeObject(envio);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var urlEnvio = AzureConfig.UrlSendMail;
                var response = await cliente.PostAsync(urlEnvio, content);

                return response.IsSuccessStatusCode;
            }

            return false;
        }

        private async Task<Token> ObtenerToken()
        {
            string urlToken = string.Format(AzureConfig.UrlToken, AzureConfig.TenantId);

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            keyValues.Add(new KeyValuePair<string, string>("client_id", AzureConfig.ClientId));
            keyValues.Add(new KeyValuePair<string, string>("resource", "https://graph.microsoft.com/"));
            keyValues.Add(new KeyValuePair<string, string>("client_secret", AzureConfig.ClientSecret));
            keyValues.Add(new KeyValuePair<string, string>("scope", "https://graph.microsoft.com/.default"));

            var cliente = new HttpClient();
            var content = new FormUrlEncodedContent(keyValues);
            var response = await cliente.PostAsync(urlToken, content);

            if (response.IsSuccessStatusCode)
            {
                //var token = await response.Content.ReadAsAsync<Token>();
                return new Token();
            }
            else
            {
                return null;
            }
        }
    }
}
