using Microsoft.Extensions.Options;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Proteccion.TableroControl.Presentacion.Helpers
{
    public class ApiHelper : IAzure
    {
        public Token Token { get; set; }
        public AzureConfig AzureConfig { get; }
        public string Code { get; set; }

        public ApiHelper(IOptions<AzureConfig> azureConfig)
        {
            AzureConfig = azureConfig.Value;
        }

        private async Task<Token> ObtenerToken()
        {
            string urlToken = string.Format(AzureConfig.UrlToken, AzureConfig.TenantId);

            var keyValues = new List<KeyValuePair<string, string>>();
            keyValues.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
            keyValues.Add(new KeyValuePair<string, string>("client_id", AzureConfig.ClientId));
            keyValues.Add(new KeyValuePair<string, string>("redirect_uri", AzureConfig.UrlRedirect));
            keyValues.Add(new KeyValuePair<string, string>("resource", "https://graph.windows.net"));
            keyValues.Add(new KeyValuePair<string, string>("client_secret", AzureConfig.ClientSecret));
            keyValues.Add(new KeyValuePair<string, string>("scope", "User.Read User.Read.All Group.Read.All"));
            keyValues.Add(new KeyValuePair<string, string>("code", Code));

            var cliente = new HttpClient();
            var content = new FormUrlEncodedContent(keyValues);
            var response = await cliente.PostAsync(urlToken, content);

            if (response.IsSuccessStatusCode)
            {
                //Token = await response.Content.ReadAsAsync<Token>();
                return new Token();
            }
            else
            {
                return null;
            }
        }

        public async Task<UserInfo> ObtenerInformacionUsuario()
        {
            if (Token == null)
                await ObtenerToken();

            if (Token != null)
            {
                var cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token.AccessToken);

                var urlCliente = string.Format(AzureConfig.UrlUserInfo, AzureConfig.TenantId);
                var response = await cliente.GetAsync(urlCliente);


                if (response.IsSuccessStatusCode)
                {
                    var userInfo = new UserInfo();

                    var isAdmin = await IsMemberOf(AzureConfig.AdminGroupId, userInfo.ObjectId);
                    userInfo.IsAdmin = isAdmin;

                    if (!isAdmin)
                    {
                        var isEjecutor = await IsMemberOf(AzureConfig.EjecutorGroupId, userInfo.ObjectId);
                        userInfo.IsEjecutor = isEjecutor;
                    }

                    return userInfo;
                }
            }

            return null;
        }

        private async Task<bool> IsMemberOf(string groupId, string memberId)
        {
            if (Token == null)
                await ObtenerToken();

            if (Token != null)
            {
                var cliente = new HttpClient();
                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Token.AccessToken);

                var urlMemberOf = string.Format(AzureConfig.UrlIsMemberOf, AzureConfig.TenantId);
                var body = new
                {
                    groupId,
                    memberId
                };

                //var response = await cliente.PostAsJsonAsync(urlMemberOf, body);

                //if (response.IsSuccessStatusCode)
                //{
                  //  var userInfo = await response.Content.ReadAsAsync<UserInfo>();
                    return true;
                //}
            }

            return false;
        }

    }
}
