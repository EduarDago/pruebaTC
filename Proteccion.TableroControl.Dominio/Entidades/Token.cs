using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class Token
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiresIn { set; get; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "resource")]
        public string Resource { set; get; }
    }
}
