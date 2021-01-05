using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Proteccion.TableroControl.Dominio.Interfaces
{
    public interface IAzure
    {
        string Code { get; set; }
        Task<UserInfo> ObtenerInformacionUsuario();
    }
}
