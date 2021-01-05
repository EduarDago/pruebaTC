using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class AzureConfig
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string UrlRedirect { get; set; }
        public string UrlAuthorizacion { get; set; }
        public string UrlToken { get; set; }
        public string UrlUserInfo { get; set; }
        public string UrlUserGroups { get; set; }
        public string UrlIsMemberOf { get; set; }
        public string AdminGroupId { get; set; }
        public string EjecutorGroupId { get; set; }
        public string UrlSendMail { get; set; }
    }
}
