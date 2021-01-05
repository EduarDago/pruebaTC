using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proteccion.TableroControl.Dominio.Entidades
{
    public class EnvioRequest
    {
        [JsonProperty(PropertyName = "message")]
        public Message Message { get; set; }

        [JsonProperty(PropertyName = "saveToSentItems")]
        public bool SaveToSentItems { get; set; }
    }

    public class Message
    {
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "body")]
        public Body Body { get; set; }

        [JsonProperty(PropertyName = "toRecipients")]
        public List<ToRecipient> ToRecipients { get; set; }
    }

    public class Body
    {
        [JsonProperty(PropertyName = "contentType")]
        public string ContentType { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }

    public class ToRecipient
    {
        [JsonProperty(PropertyName = "emailAddress")]
        public EmailAddress EmailAddress { get; set; }
    }

    public class EmailAddress
    {
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
    }
}
