using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Proteccion.TableroControl.Dominio.Interfaces
{
    public interface IEmail
    {
        Task<bool> SendEmailAzureAsync(string asunto, string contenido, string email);
    }
}
