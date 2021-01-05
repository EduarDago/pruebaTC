namespace Proteccion.TableroControl.Proxy.BL
{
    public interface ILogEjecucionProxy
    {
        string EjecutarQuery(string sql);
        bool InsertarLog(string usuario, string script, string resultado);
    }
}