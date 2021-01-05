using ExcelDataReader;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Newtonsoft.Json.Linq;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Proteccion.TableroControl.Datos.DataContext;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Enumeraciones;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Proteccion.TableroControl.Datos.DAO
{
    public class OrigenDatos : IOrigenDatos
    {
        private readonly TableroControlContext context;
        private readonly IParametroDatos parametro;
        private readonly Database Contexto;
        private readonly string connectionString;

        private const string DATETIME = "DATETIME";
        private const string TIME = "TIME";
        private const string FLOAT = "FLOAT";

        #region "Constructor"

        public OrigenDatos(IParametroDatos parametro, TableroControlContext context)
        {
            this.context = context;
            this.parametro = parametro;
            connectionString = context.Database?.GetDbConnection()?.ConnectionString;
            Contexto = new SqlDatabase(connectionString ?? "Prueba");
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrigenDato> ObtenerConfiguracionesOrigen()
        {
            var listado = context.OrigenDato.Include(x => x.TipoOrigen);
            return listado;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public IEnumerable<OrigenDato> ObtenerOrigenes()
        {
            var listado = context.OrigenDato.Where(x => x.Activo);
            return listado;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public OrigenDato ObtenerConfiguracionOrigen(int idOrigen)
        {
            try
            {
                using (var ctx = new TableroControlContext(connectionString))
                {
                    
                    var origen = ctx.OrigenDato
                        .Include(x => x.Campos)
                        .Include(x => x.Parametros)
                        .FirstOrDefault(x => x.IdOrigenDato == idOrigen);
                    origen.Campos = origen.Campos.OrderBy(a => a.Orden).ToList();
                    return origen;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool ExisteOrigen(string nombre)
        {
            var existe = context.OrigenDato.Any(x => x.Nombre.ToLower(CultureInfo.InvariantCulture) == nombre.ToLower(CultureInfo.InvariantCulture));
            return existe;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<EjecucionImportacion> ConsultarEjecuciones()
        {
            try
            {
                using (DbCommand cmd = Contexto.GetStoredProcCommand("Configuracion_ConsultarEjecuciones"))
                {
                    var listado = new List<EjecucionImportacion>();

                    using (var reader = Contexto.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            var ejecucion = new EjecucionImportacion
                            {
                                IdEjecucion = Convert.ToInt32(reader["IdEjecucion"].ToString()),
                                IdOrigenDato = Convert.ToInt32(reader["IdOrigenDato"].ToString()),
                                NombreOrigen = reader["Nombre"].ToString()
                            };

                            if (!string.IsNullOrEmpty(reader["FechaEjecucion"].ToString()))
                            {
                                ejecucion.FechaEjecucion = Convert.ToDateTime(reader["FechaEjecucion"].ToString());
                            }

                            ejecucion.Estado = reader["Estado"].ToString();
                            ejecucion.CantidadRegistros = Convert.ToInt32(reader["CantidadRegistros"].ToString());
                            ejecucion.TipoOrigen = (TipoOrigen)Convert.ToInt32(reader["IdTipoOrigen"].ToString());

                            listado.Add(ejecucion);
                        }
                    }

                    return listado;
                }
            }
            catch
            {
                return new List<EjecucionImportacion>();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrigen"></param>
        /// <returns></returns>
        public EjecucionImportacion EjecutarImportacion(int idOrigen, TipoOrigen tipo, bool validarDatos)
        {
            EjecucionImportacion importacion = null;

            switch (tipo)
            {
                case TipoOrigen.Excel:
                    importacion = ImportarExcel(idOrigen, validarDatos);
                    break;
                case TipoOrigen.BD:
                    importacion = ImportarBd(idOrigen);
                    break;
                case TipoOrigen.CSV:
                    importacion = ImportarCsv(idOrigen);
                    break;
                case TipoOrigen.LONGITUD_FIJA:
                    importacion = ImportarLongitudFija(idOrigen, tipo);
                    break;
                case TipoOrigen.SEPARADOR:
                    importacion = ImportarLongitudFija(idOrigen, tipo);
                    break;
            }

            return importacion;
        }


        private string PathArchivoPlano(ref EjecucionImportacion ejecucion, OrigenDato origen, string fechaProceso)
        {
            try
            {
                string nombreArchivo = origen.NombreArchivo;

                if (origen.ConcatenarFecha == true)
                {
                    var fechaConvert = Convert.ToDateTime(fechaProceso);
                    string fechaArchivo = string.Format("{0}{1}{2}", fechaConvert.Day, fechaConvert.Month, fechaConvert.Year);
                    var longitudExtension = nombreArchivo.IndexOf(".");
                    var nombreArchivoCoincidencia = string.Format("{0}{1}{2}", nombreArchivo.Substring(0, longitudExtension), fechaArchivo, nombreArchivo.Substring(longitudExtension));

                    var pathFecha = Path.Combine(Directory.GetCurrentDirectory(), origen.RutaArchivo);
                    DirectoryInfo directorio = new DirectoryInfo(pathFecha);
                    if (directorio.Exists)
                    {
                        var ultimoArchivo = directorio.GetFiles()
                            .Where(x => x.Name.Contains(nombreArchivoCoincidencia))
                            .OrderByDescending(j => j.LastWriteTime).FirstOrDefault();

                        if (ultimoArchivo == null)
                        {
                            ejecucion.Estado = string.Format("El archivo {0} no existe en {1}", origen.NombreArchivo, pathFecha);
                            return "";
                        }
                        else
                        {
                            nombreArchivo = ultimoArchivo.Name;
                        }

                    }
                    else
                    {
                        ejecucion.Estado = string.Format("El archivo {0} no existe en {1}", origen.NombreArchivo, pathFecha);
                        return "";
                    }
                }

                string archivo = string.Format("{0}/{1}", origen.RutaArchivo, nombreArchivo);

                var path = Path.Combine(Directory.GetCurrentDirectory(), archivo);

                //Se valida la existencia del archivo
                if (!File.Exists(path))
                {
                    ejecucion.Estado = string.Format("El archivo {0} no existe en {1}", origen.NombreArchivo, path);
                    return "";
                }
                return path;
                 
            }
            catch
            {
                return "";
            }

        }

        private EjecucionImportacion ImportarLongitudFija(int idOrigen, TipoOrigen tipo)
        {
            try
            {
                var origen = ObtenerConfiguracionOrigen(idOrigen);
                var fechaProceso = parametro.ObtenerParametro("FechaProceso")?.Valor;
                string nombreTabla = string.Format("{0}.Carga_{1}", origen.EsquemaTabla, origen.NombreTabla);
                int cantidad = 0;
                EjecucionImportacion ejecucion = new EjecucionImportacion
                {
                    IdOrigenDato = idOrigen,
                    NombreOrigen = origen.Nombre
                };

                var path = this.PathArchivoPlano(ref ejecucion, origen, fechaProceso);

                if (String.IsNullOrEmpty(path))
                {
                    return ejecucion;
                }                

                using (var stream = File.OpenText(path))
                {
                    short contador = 0;
                    string linearead = stream.ReadLine();
                    while ((linearead != null) && (contador < origen.LineaInicioLectura))
                    {
                        contador++;
                        linearead = stream.ReadLine();
                    }
                    if (contador != origen.LineaInicioLectura)
                    {
                        ejecucion.Estado = string.Format("El archivo {0} no contiene información suficiente para inicar su lectura.", origen.NombreArchivo);
                        return ejecucion;
                    }
                                        
                    DataTable dataTable = ConstruirTablaDesdeArchivo(origen, stream, linearead, tipo);
                    cantidad = dataTable.Rows.Count;

                    //Se agrega la fecha de proceso
                    foreach (DataRow row in dataTable.Rows)
                    {
                        row["_FechaProceso"] = fechaProceso;
                    }
                       
                    //Se elimina el contenido de la tabla
                    EjecutarQuery(string.Format("TRUNCATE TABLE {0}", nombreTabla));

                    //Se insertan los datos en la tabla
                    InsertarDatos(ref ejecucion, nombreTabla, dataTable, cantidad, idOrigen, origen.NombreArchivo);
                    
                }

                return ejecucion;
            }
            catch
            {
                return new EjecucionImportacion
                {
                    IdOrigenDato = idOrigen,
                    Estado = string.Format("No es posible procesar la información para el origen de datos con ID {0} y tipo {1}", idOrigen, tipo)
                };
            }
        }

        private DataTable CrearDatatableLongitudFija(IEnumerable<CampoOrigen> camposList, int numeroCampos)
        {
            DataTable dataTable = new DataTable();
            DataColumn[] columnas = new DataColumn[numeroCampos];
            short contador = 0;
            foreach (CampoOrigen campo in camposList)
            {
                columnas[contador] = new DataColumn(campo.Nombre);
                contador += 1;
            }
            dataTable.Columns.AddRange(columnas);
            dataTable.Columns.Add("_FechaProceso", typeof(DateTime));
            dataTable.Columns.Add("_Id", typeof(int));

            return dataTable;
        }

        private DataTable ConstruirTablaDesdeArchivo(OrigenDato origen, StreamReader stream, string linearead, TipoOrigen tipo)
        {
            int numeroCampos = origen.Campos.Count;
            var camposList = origen.Campos.OrderBy(a => a.Orden);
            DataTable dataTable = this.CrearDatatableLongitudFija(camposList, numeroCampos);
            if (tipo.Equals(TipoOrigen.LONGITUD_FIJA))
            {
                LeerLineasLongitudFija(stream, linearead, numeroCampos, camposList, dataTable);
            }
            else
            {
                string separador = origen.Separador.Trim().ToUpper();
                if (separador.Equals("TAB"))
                    separador = "\t";
                LeerLineasPirSeparador(stream, linearead, numeroCampos, camposList, dataTable, separador);
            }

            return dataTable;
        }

        private void LeerLineasLongitudFija(StreamReader stream, string linearead, int numeroCampos, IOrderedEnumerable<CampoOrigen> camposList, DataTable dataTable)
        {
            while (linearead != null)
            {
                int contador = 0;
                object[] filas = new object[numeroCampos];
                foreach (CampoOrigen campo in camposList)
                {
                    string datoCampo = linearead.Substring(0, campo.LongitudCampo);
                    if (campo.EliminarCamposBlanco)
                        datoCampo = datoCampo.Trim();
                    if (campo.TipoDato.Equals(DATETIME))
                    {
                        int numero = Int32.Parse(datoCampo);
                        datoCampo = numero != 0 ? DateTime.ParseExact(datoCampo, "yyyyMMdd", null).ToString() : null;
                    }
                    else if (campo.TipoDato.Equals(TIME))
                    {
                        datoCampo = datoCampo.Insert(2, ":").Insert(5, ":");
                    }
                    else if (campo.TipoDato.Equals(FLOAT))
                    {
                        double numeroLectura = double.Parse(datoCampo.Trim());
                        double decimales = campo.PosicionInicial > 0 ? Math.Pow(10, campo.PosicionInicial) : 1;
                        datoCampo = String.Format("{0}", (numeroLectura / decimales));
                    }
                    filas[contador] = datoCampo;
                    contador++;
                    linearead = linearead.Remove(0, campo.LongitudCampo);
                }
                dataTable.Rows.Add(filas);
                linearead = stream.ReadLine();
            }
        }

        private void LeerLineasPirSeparador(StreamReader stream, string linearead, int numeroCampos, IOrderedEnumerable<CampoOrigen> camposList, DataTable dataTable, string separador)
        {
            while (linearead != null)
            {
                int contador = 0;
                object[] filas = new object[numeroCampos];
                string[] camposLinea = linearead.TrimStart().Split(separador);
                foreach (CampoOrigen campo in camposList)
                {
                    string datoCampo = camposLinea[contador];
                    if (campo.EliminarCamposBlanco)
                        datoCampo = datoCampo.Trim();
                    if (campo.TipoDato.Equals(DATETIME))
                    {
                        datoCampo = String.IsNullOrEmpty(datoCampo)? null : DateTime.ParseExact(datoCampo, "dd/MM/yyyy", null).ToString();
                    }
                    else if (campo.TipoDato.Equals(TIME))
                    {
                        datoCampo = datoCampo.Insert(2, ":").Insert(5, ":");
                    }
                    else if (campo.TipoDato.Equals(FLOAT))
                    {
                        datoCampo = String.IsNullOrEmpty(datoCampo) ? null : String.Format("{0}", double.Parse(datoCampo.Trim()));
                    }
                    filas[contador] = datoCampo;
                    contador++;
                }
                dataTable.Rows.Add(filas);
                linearead = stream.ReadLine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrigen"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        private EjecucionImportacion ImportarCsv(int idOrigen)
        {
            try
            {
                var origen = ObtenerConfiguracionOrigen(idOrigen);
                string nombreArchivo = origen.NombreArchivo;
                var fechaProceso = parametro.ObtenerParametro("FechaProceso")?.Valor;
                EjecucionImportacion ejecucion = new EjecucionImportacion
                {
                    IdOrigenDato = idOrigen,
                    NombreOrigen = origen.Nombre
                };

                if (origen.ConcatenarFecha == true)
                {
                    var fechaConvert = Convert.ToDateTime(fechaProceso);
                    string fechaArchivo = string.Format("{0}{1}{2}", fechaConvert.Month, fechaConvert.Day, fechaConvert.Year);
                    var nombreArchivoCoincidencia = string.Format("{0}_{1}", nombreArchivo.Substring(0, nombreArchivo.Length - 4), fechaArchivo);

                    var pathFecha = Path.Combine(Directory.GetCurrentDirectory(), origen.RutaArchivo);
                    DirectoryInfo directorio = new DirectoryInfo(pathFecha);
                    if (directorio.Exists)
                    {
                        var ultimoArchivo = directorio.GetFiles()
                            .Where(x => x.Name.Contains(nombreArchivoCoincidencia) && x.Extension == ".csv")
                            .OrderByDescending(j => j.LastWriteTime).FirstOrDefault();

                        if (ultimoArchivo == null)
                        {
                            ejecucion.Estado = string.Format("El archivo {0} no existe en {1}", origen.NombreArchivo, pathFecha);
                            return ejecucion;
                        }

                        nombreArchivo = ultimoArchivo.Name;
                    }
                    else
                    {
                        ejecucion.Estado = string.Format("El archivo {0} no existe en {1}", origen.NombreArchivo, pathFecha);
                        return ejecucion;
                    }
                }

                string archivo = string.Format("{0}/{1}", origen.RutaArchivo, nombreArchivo);
                string nombreTabla = string.Format("{0}.Carga_{1}", origen.EsquemaTabla, origen.NombreTabla);
                int cantidad = 0;

                var path = Path.Combine(Directory.GetCurrentDirectory(), archivo);

                //Se valida la existencia del archivo
                if (!File.Exists(path))
                {
                    ejecucion.Estado = string.Format("El archivo {0} no existe en {1}", origen.NombreArchivo, path);
                    return ejecucion;
                }

                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateCsvReader(stream, new ExcelReaderConfiguration
                    {
                        // Gets or sets the encoding to use when the input XLS lacks a CodePage
                        // record, or when the input CSV lacks a BOM and does not parse as UTF8. 
                        // Default: cp1252. (XLS BIFF2-5 and CSV only)
                        FallbackEncoding = Encoding.GetEncoding(1252),

                        // Gets or sets an array of CSV separator candidates. The reader 
                        // autodetects which best fits the input data. Default: , ; TAB | # 
                        // (CSV only)
                        AutodetectSeparators = new char[] { origen.Separador[0] }
                    }))
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            UseColumnDataType = true,
                            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                            {
                                EmptyColumnNamePrefix = "Column",
                                UseHeaderRow = false
                            }
                        });

                        var table = result.Tables[0];
                        table.Columns.Add("_FechaProceso", typeof(DateTime));
                        table.Columns.Add("_Id", typeof(int));

                        cantidad = table.Rows.Count;

                        //Se agrega la fecha de proceso
                        foreach (DataRow row in table.Rows)
                        {
                            row["_FechaProceso"] = fechaProceso;
                        }

                        //Se elimina el contenido de la tabla
                        EjecutarQuery(string.Format("TRUNCATE TABLE {0}", nombreTabla));

                        //Se insertan los datos en la tabla
                        InsertarDatos(ref ejecucion, nombreTabla, table, cantidad, idOrigen, origen.NombreArchivo);
                    }
                }

                return ejecucion;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrigen"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        private EjecucionImportacion ImportarBd(int idOrigen)
        {
            try
            {
                using (DbCommand cmd = Contexto.GetStoredProcCommand("Configuracion_EjecutarImportaciones"))
                {
                    cmd.CommandTimeout = 0;
                    Contexto.AddInParameter(cmd, "@IdOrigen", DbType.Int32, idOrigen);

                    EjecucionImportacion ejecucion = null;

                    using (var reader = Contexto.ExecuteReader(cmd))
                    {
                        if (reader.Read())
                        {
                            ejecucion = new EjecucionImportacion
                            {
                                IdOrigenDato = idOrigen,
                                Estado = reader["Estado"].ToString(),
                                CantidadRegistros = Convert.ToInt32(reader["Registros"].ToString()),
                                NombreOrigen = reader["NombreOrigen"].ToString()
                            };
                        }
                    }

                    return ejecucion;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrigen"></param>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        private EjecucionImportacion ImportarExcel(int idOrigen, bool validarDatos = false)
        {
            try
            {
                var origen = ObtenerConfiguracionOrigen(idOrigen);
                var fechaProceso = parametro.ObtenerParametro("FechaProceso")?.Valor;
                EjecucionImportacion ejecucion = new EjecucionImportacion
                {
                    IdOrigenDato = idOrigen,
                    NombreOrigen = origen.Nombre
                };

                //Validar si el origen es Sftp
                if (!string.IsNullOrEmpty(origen.RutaDestinoSftp) && !string.IsNullOrEmpty(origen.RutaOrigenSftp))
                {
                    var Respuesta = GestionarOrigenSftp(origen, fechaProceso);
                    if (Respuesta != "OK")
                    {
                        ejecucion.Estado = Respuesta;
                        return ejecucion;
                    }
                }

                var archivo = ValidarArchivo(origen, fechaProceso, ref ejecucion);
                if (archivo == null)
                {
                    return ejecucion;
                }

                var path = Path.Combine(Directory.GetCurrentDirectory(), archivo);
                string nombreTabla = string.Format("{0}.Carga_{1}", origen.EsquemaTabla, origen.NombreTabla);
                int cantidad = 0;

                var campos = origen.Campos.OrderBy(x => x.Orden).ToList();
                string extension = Path.GetExtension(archivo).ToLower(CultureInfo.InvariantCulture);


                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    var destino = new MemoryStream();
                    stream.CopyTo(destino);
                    stream.Position = 0;

                    bool continuar = ValidarEncabezado(origen, stream, campos, validarDatos, extension, ref ejecucion);
                    if (!continuar)
                    {
                        return ejecucion;
                    }

                    using (var reader = ExcelReaderFactory.CreateReader(destino))
                    {
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            UseColumnDataType = true,
                            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                            {
                                EmptyColumnNamePrefix = "Column",
                                UseHeaderRow = true,
                                ReadHeaderRow = (rowReader) =>
                                {

                                    for (int i = 1; i < origen.FilaEncabezado; i++)
                                    {
                                        rowReader.Read();
                                    }

                                }
                            }
                        });

                        var table = result.Tables[0];
                        table.Columns.Add("_FechaProceso", typeof(DateTime));
                        table.Columns.Add("_Id", typeof(Int32));

                        cantidad = table.Rows.Count;

                        if (cantidad <= 0)
                        {
                            ejecucion.Estado = "El archivo no contiene información para importar";
                            return ejecucion;
                        }

                        //Se elimina la ultima fila
                        if (origen.EliminarFila)
                        {
                            cantidad -= 1;
                            table.Rows.RemoveAt(cantidad);
                        }

                        if (cantidad <= 0)
                        {
                            ejecucion.Estado = "El archivo no contiene información para importar";
                            return ejecucion;
                        }

                        //Se agrega la fecha de proceso
                        foreach (DataRow row in table.Rows)
                        {
                            row["_FechaProceso"] = fechaProceso;
                        }

                        //Se elimina el contenido de la tabla
                        EjecutarQuery(string.Format("TRUNCATE TABLE {0}", nombreTabla));

                        //Se insertan los datos en la tabla
                        InsertarDatos(ref ejecucion, nombreTabla, table, cantidad, idOrigen, origen.NombreArchivo);
                    }
                }

                return ejecucion;
            }
            catch
            {
                return null;
            }

        }

        private string GestionarOrigenSftp(OrigenDato origen, string fechaProceso)
        {
            string Respuesta = string.Empty;
            //obtener parametros de configuracion de la bd
            var dbPrms = parametro.ObtenerParametros();

            //validar concatenacion fechaproceso
            string customFileName = origen.NombreArchivo;

            if (origen.ConcatenarFecha == true)
            {
                customFileName = CompletarNombre(customFileName, fechaProceso);
            }

            SftpSettings OrigenSftp = LlenarConfiguracionSftp(1, customFileName, dbPrms, origen);
            SftpSettings DestinoSftp = LlenarConfiguracionSftp(2, customFileName, dbPrms, origen);

            //Realizar el traslado del archivo
            Respuesta = new StfpHelper(OrigenSftp, DestinoSftp).TransferirArchivo();

            if (Respuesta != "OK")
            {
                Respuesta = string.Format("Error en transferencia SFTP: {0}", Respuesta);
            }

            return Respuesta;
        }
            

        private string CompletarNombre(string customFileName, string fechaProceso)
        {
            var fechaConvert = Convert.ToDateTime(fechaProceso);
            string fechaArchivo = fechaConvert.ToString("MMddyyyy");
            var ext = customFileName.Substring(customFileName.LastIndexOf('.'));
            var nombreArchivoCoincidencia = string.Format("{0}_{1}", customFileName.Substring(0, customFileName.Length - ext.Length), fechaArchivo);
            return string.Format("{0}{1}", nombreArchivoCoincidencia, ext);
        }
            

        private SftpSettings LlenarConfiguracionSftp(Int16 tipo, string customFileName, IEnumerable<Parametro> dbPrms, OrigenDato origen)
        {
            SftpSettings settings;
            if (tipo == 1)
            {
                //Origen
                settings = new SftpSettings
                {
                    FileName = customFileName,
                    Host = dbPrms.FirstOrDefault(p => p.Nombre == "SftpOrigenHost").Valor,
                    Password = dbPrms.FirstOrDefault(p => p.Nombre == "SftpOrigenPassword").Valor,
                    Port = int.Parse(dbPrms.FirstOrDefault(p => p.Nombre == "SftpOrigenPuerto").Valor),
                    RemoteDirectory = origen.RutaOrigenSftp,
                    TimeOut = int.Parse(dbPrms.FirstOrDefault(p => p.Nombre == "SftpOrigenTimeOut").Valor),
                    UserName = dbPrms.FirstOrDefault(p => p.Nombre == "SftpOrigenUsuario").Valor
                };
            }
            else
            {
                //Destino
                settings = new SftpSettings
                {
                    FileName = customFileName,
                    Host = dbPrms.FirstOrDefault(p => p.Nombre == "SftpDestinoHost").Valor,
                    Password = dbPrms.FirstOrDefault(p => p.Nombre == "SftpDestinoPassword").Valor,
                    Port = int.Parse(dbPrms.FirstOrDefault(p => p.Nombre == "SftpDestinoPuerto").Valor),
                    RemoteDirectory = origen.RutaDestinoSftp,
                    TimeOut = int.Parse(dbPrms.FirstOrDefault(p => p.Nombre == "SftpDestinoTimeOut").Valor),
                    UserName = dbPrms.FirstOrDefault(p => p.Nombre == "SftpDestinoUsuario").Valor
                };
            }

            return settings;
        }

        private bool ValidarEncabezado(OrigenDato origen, FileStream stream, List<CampoOrigen> campos, bool validarDatos, string extension, ref EjecucionImportacion ejecucion)
        {
            ISheet sheet;
            int cantidad = 0;

            if (extension == ".xls")
            {
                HSSFWorkbook hssfwb = new HSSFWorkbook(stream);
                sheet = hssfwb.GetSheetAt(0);
            }
            else
            {
                XSSFWorkbook hssfwb = new XSSFWorkbook(stream);
                sheet = hssfwb.GetSheetAt(0);
            }

            IRow headerRow = sheet.GetRow(origen.FilaEncabezado > 0 ? origen.FilaEncabezado - 1 : 0);

            if (headerRow == null)
            {
                ejecucion.Estado = "El origen de datos no esta configurado correctamente";
                return false;
            }

            StringBuilder errores = new StringBuilder();
            int cellCount = headerRow.LastCellNum;

            //Se valida la estructura del archivo
            for (int j = 0; j < campos.Count; j++)
            {
                ICell cell = headerRow.GetCell(j);
                var columna = cell.ToString();
                if (campos[j].NombreExcel.ToUpper(CultureInfo.InvariantCulture) != columna.ToUpper(CultureInfo.InvariantCulture))
                {
                    cantidad++;
                    errores.Append(string.Format("{1}{0}", campos[j].NombreExcel, cantidad == 1 ? "" : ";"));
                }
            }

            if (!string.IsNullOrEmpty(errores.ToString()))
            {
                ejecucion.Errores = errores.ToString();
                ejecucion.Estado = string.Format("Se ha(n) encontrado {0} inconsistencia(s) en la estructura del archivo. Campos configurados: {1} - Campos en Excel: {2}", cantidad, campos.Count, cellCount);
                return false;
            }

            if (validarDatos)
            {
                ValidarEstructuraDatos(ref ejecucion, origen, sheet, campos);

                if (ejecucion.Validaciones.Count > 0)
                {
                    ejecucion.Errores = errores.ToString();
                    ejecucion.Estado = string.Format("Se ha(n) encontrado {0} filas con inconsistencia(s) en los datos del archivo", ejecucion.Validaciones.Count);
                    return false;
                }
            }

            return true;
        }

        private string ValidarArchivo(OrigenDato origen, string fechaProceso, ref EjecucionImportacion ejecucion)
        {
            string nombreArchivo = origen.NombreArchivo;

            if (origen.ConcatenarFecha == true)
            {
                var fechaConvert = Convert.ToDateTime(fechaProceso);
                string fechaArchivo = fechaConvert.ToString("MMddyyyy");
                var ext = nombreArchivo.Substring(nombreArchivo.LastIndexOf('.'));
                var nombreArchivoCoincidencia = string.Format("{0}_{1}", nombreArchivo.Substring(0, nombreArchivo.Length - ext.Length), fechaArchivo);

                var pathFecha = Path.Combine(Directory.GetCurrentDirectory(), origen.RutaArchivo);
                DirectoryInfo directorio = new DirectoryInfo(pathFecha);
                if (directorio.Exists)
                {
                    var ultimoArchivo = directorio.GetFiles()
                        .Where(x => x.Name.Contains(nombreArchivoCoincidencia) && x.Extension == ext)
                        .OrderByDescending(j => j.LastWriteTime).FirstOrDefault();

                    if (ultimoArchivo == null)
                    {
                        ejecucion.Estado = string.Format("El archivo {0} no existe en {1}", nombreArchivoCoincidencia, pathFecha);
                        return null;
                    }

                    nombreArchivo = ultimoArchivo.Name;
                }
                else
                {
                    ejecucion.Estado = string.Format("El archivo {0} no existe en {1}", nombreArchivoCoincidencia, pathFecha);
                    return null;
                }
            }

            string archivo = string.Format("{0}/{1}", origen.RutaArchivo, nombreArchivo);

            var path = Path.Combine(Directory.GetCurrentDirectory(), archivo);
            //Se valida la existencia del archivo
            if (!File.Exists(path))
            {
                ejecucion.Estado = string.Format("El archivo {0} no existe en {1}", origen.NombreArchivo, path);
                return null;
            }

            return archivo;
        }

        [ExcludeFromCodeCoverage]
        private void InsertarDatos(ref EjecucionImportacion ejecucion, string nombreTabla, DataTable table, int cantidad, int idOrigen, string nombreArchivo)
        {
            using (SqlConnection destinationConnection = new SqlConnection(connectionString))
            {
                destinationConnection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    bulkCopy.DestinationTableName = nombreTabla;

                    try
                    {
                        bulkCopy.WriteToServer(table);

                        ejecucion.Estado = "Sin inconvenientes";
                        ejecucion.CantidadRegistros = cantidad;

                        //Guargar ejecución
                        EjecucionOrigen ultimaEjecucion = new EjecucionOrigen
                        {
                            IdOrigenDato = idOrigen,
                            Estado = ejecucion.Estado,
                            FechaEjecucion = DateTime.Now,
                            CantidadRegistros = ejecucion.CantidadRegistros
                        };

                        GuardarEjecucion(ultimaEjecucion);

                        //Se marca como exitosa la importacion
                        ejecucion.Exitoso = true;
                    }
                    catch (Exception ex)
                    {
                        ejecucion.Estado = string.Format("Ha ocurrido un error importando el archivo: {0} - Error : {1}", nombreArchivo, ex.Message);
                    }
                }
            }
        }

        [ExcludeFromCodeCoverage]
        private void ValidarEstructuraDatos(ref EjecucionImportacion ejecucion, OrigenDato origen, ISheet sheet, List<CampoOrigen> campos)
        {
            int ultimaFila = origen.EliminarFila ? sheet.LastRowNum - 1 : sheet.LastRowNum;
            for (int i = (origen.FilaEncabezado > 0 ? origen.FilaEncabezado : 1); i <= ultimaFila; i++) //Read Excel File
            {
                ErrorDato error = new ErrorDato
                {
                    Fila = i + 1
                };

                StringBuilder validaciones = new StringBuilder();
                IRow row = sheet.GetRow(i);

                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;

                for (int j = 0; j < campos.Count; j++)
                {
                    string valor = row.GetCell(j).ToString();

                    if (string.IsNullOrEmpty(valor))
                        continue;

                    var campo = campos[j];

                    GenerarValidaciones(ref validaciones, campo, valor);
                }

                //Si hay inconsistencias se inserta el error
                if (!string.IsNullOrEmpty(validaciones.ToString()))
                {
                    error.Errores = validaciones.ToString();
                    ejecucion.Validaciones.Add(error);
                }
            }

        }

        [ExcludeFromCodeCoverage]
        private void GenerarValidaciones(ref StringBuilder validaciones, CampoOrigen campo, string valor)
        {
            if (campo.TipoDato == "INT" && !int.TryParse(valor, out int campoConvert))
            {
                validaciones.Append(string.Format("{0} : INT no válido; ", campo.NombreExcel));
                return;
            }

            if (campo.TipoDato.Contains("VARCHAR"))
            {
                string tipoDato = campo.TipoDato;
                int inicio = tipoDato.IndexOf('(') + 1;
                int fin = tipoDato.LastIndexOf(')');

                //Se valida la longitud
                string longitud = campo.TipoDato.Substring(inicio, fin - inicio);
                ValidarLongitudCampo(validaciones, valor, longitud, campo.Nombre);
            }

            if (campo.TipoDato.Contains("DECIMAL") && !double.TryParse(valor, out double campoDouble))
            {
                validaciones.Append(string.Format("{0} : Decimal no válido; ", campo.NombreExcel));
                return;
            }

            if (campo.TipoDato.Contains("DATETIME") && !DateTime.TryParse(valor, out DateTime campoFecha))
            {
                validaciones.Append(string.Format("{0} : Fecha no válida; ", campo.NombreExcel));
                return;
            }

            if (campo.TipoDato == "FLOAT" && !Single.TryParse(valor, out float campoFloat))
            {
                validaciones.Append(string.Format("{0} : Float no válido; ", campo.NombreExcel));
                return;
            }

            if (campo.TipoDato == "BIT" && !Boolean.TryParse(valor, out bool campoBit))
            {
                validaciones.Append(string.Format("{0} : Bit no válido; ", campo.NombreExcel));
                return;
            }

            if (campo.TipoDato == "BIGINT" && !Int64.TryParse(valor, out Int64 campoBigInt))
            {
                validaciones.Append(string.Format("{0} : Bigint no válido; ", campo.NombreExcel));
                return;
            }
        }

        [ExcludeFromCodeCoverage]
        private void ValidarLongitudCampo(StringBuilder validaciones, string valor, string longitud, string nombre)
        {
            if (longitud == "MAX" && valor.Length > 8000)
            {
                validaciones.Append(string.Format("{0} : Longitud máxima(8000) excedida; ", nombre));
            }
            else if (valor.Length > Convert.ToInt32(longitud))
            {
                validaciones.Append(string.Format("{0} : Longitud máxima({1}) excedida; ", nombre, longitud));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origen"></param>
        /// <returns></returns>
        public bool InsertarConfiguracionOrigen(OrigenDato origen)
        {
            context.OrigenDato.Add(origen);
            return context.SaveChanges() > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origen"></param>
        /// <returns></returns>
        public bool ActualizarConfiguracionOrigen(OrigenDato origen)
        {
            try
            {
                var origenActualizar = context.OrigenDato
                .Include(x => x.Campos)
                .Include(x => x.Parametros)
                .Single(x => x.IdOrigenDato == origen.IdOrigenDato);
                origenActualizar.Activo = origen.Activo;
                origenActualizar.Descripcion = origen.Descripcion;
                origenActualizar.EsquemaProcedimiento = origen.EsquemaProcedimiento;
                origenActualizar.EsquemaTabla = origen.EsquemaTabla;
                origenActualizar.FilaEncabezado = origen.FilaEncabezado;
                origenActualizar.Nombre = origen.Nombre;
                origenActualizar.NombreArchivo = origen.NombreArchivo;
                origenActualizar.NombreTabla = origen.NombreTabla;
                origenActualizar.Procedimiento = origen.Procedimiento;
                origenActualizar.RutaArchivo = origen.RutaArchivo;
                origenActualizar.EliminarFila = origen.EliminarFila;
                origenActualizar.ConcatenarFecha = origen.ConcatenarFecha;
                origenActualizar.Separador = origen.Separador;
                origenActualizar.RutaOrigenSftp = origen.RutaOrigenSftp;
                origenActualizar.RutaDestinoSftp = origen.RutaDestinoSftp;
                origenActualizar.ColumnaInicioLectura = origen.ColumnaInicioLectura;
                origenActualizar.LineaInicioLectura = origen.LineaInicioLectura;

                //Se actualizan los campos
                if (origenActualizar.Campos?.Count > 0)
                {
                    //Se eliminan los campos existentes y se agregan los nuevos
                    context.RemoveRange(origenActualizar.Campos);
                    if (origen.Campos.Count > 0)
                        origenActualizar.Campos = origen.Campos;
                }

                //Se actualizan los filtros
                if (origenActualizar.Parametros?.Count > 0)
                {
                    //Se eliminan los parametros existentes y se agregan los nuevos
                    context.RemoveRange(origenActualizar.Parametros);
                    if (origen.Parametros.Count > 0)
                        origenActualizar.Parametros = origen.Parametros;
                }

                context.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origen"></param>
        /// <returns></returns>
        public bool EliminarOrigen(int idOrigen)
        {
            var origen = context.OrigenDato.Single(x => x.IdOrigenDato == idOrigen);
            string nombreTabla = string.Format("{0}.Carga_{1}", origen.EsquemaTabla, origen.NombreTabla);
            string query = string.Format("DROP TABLE {0}", nombreTabla);

            try
            {
                context.Remove(origen);
                context.SaveChanges();
                EjecutarQuery(query);

                return true;
            }
            catch
            {
                return false;
            }
        }

        [ExcludeFromCodeCoverage]
        private void EjecutarQuery(string query)
        {
            using (DbCommand cmd = Contexto.GetSqlStringCommand(query))
            {
                Contexto.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parametro"></param>
        /// <returns></returns>
        public void CrearEstructura(int idOrigen)
        {
            using (DbCommand cmd = Contexto.GetStoredProcCommand("Configuracion_CrearEstructura"))
            {
                Contexto.AddInParameter(cmd, "@IdOrigen", DbType.Int32, idOrigen);
                Contexto.ExecuteNonQuery(cmd);
            }
        }

        public bool GuardarEjecucion(EjecucionOrigen ejecucion)
        {
            try
            {
                using (var ctx = new TableroControlContext(connectionString))
                {
                    ctx.EjecucionOrigen.Add(ejecucion);
                    return ctx.SaveChanges() > 0;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOrigen"></param>
        /// <returns></returns>
        public JArray ObtenerDatosTabla(int idOrigen)
        {
            try
            {
                var origen = ObtenerConfiguracionOrigen(idOrigen);
                var tabla = string.Format("{0}.Carga_{1}", origen.EsquemaTabla, origen.NombreTabla);

                using (DbCommand cmd = Contexto.GetSqlStringCommand(string.Format("SELECT * FROM {0}", tabla)))
                {
                    var listado = new JArray();

                    using (var reader = Contexto.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            JObject registro = new JObject();

                            try
                            {
                                foreach (var campo in origen.Campos)
                                {
                                    string nombreCampo = string.Format("{0}{1}", char.ToLowerInvariant(campo.Nombre[0]), campo.Nombre.Substring(1));
                                    string tipoDato = campo.TipoDato;
                                    string valor = reader[campo.Nombre]?.ToString();

                                    if (reader[campo.Nombre] == DBNull.Value)
                                    {
                                        registro.Add(nombreCampo, null);
                                    }
                                    else
                                    {
                                        MapearCampos(ref registro, nombreCampo, tipoDato, valor);
                                    }
                                }
                            }
                            catch
                            {
                                return new JArray();
                            }

                            listado.Add(registro);
                        }
                    }

                    return listado;
                }
            }
            catch
            {
                return null;
            }

        }

        [ExcludeFromCodeCoverage]
        private void MapearCampos(ref JObject registro, string nombreCampo, string tipoDato, string valor)
        {
            if (tipoDato.Contains("VARCHAR"))
            {
                registro.Add(nombreCampo, valor);
            }

            if (tipoDato.Equals("INT"))
            {
                registro.Add(nombreCampo, Convert.ToInt32(valor));
            }

            if (tipoDato.Contains("FLOAT"))
            {
                registro.Add(nombreCampo, Convert.ToSingle(valor));
            }

            if (tipoDato.Contains("BIT"))
            {
                registro.Add(nombreCampo, Convert.ToBoolean(valor));
            }

            if (tipoDato.Contains("DATETIME"))
            {
                registro.Add(nombreCampo, Convert.ToDateTime(valor));
            }

            if (tipoDato.Contains("DECIMAL"))
            {
                registro.Add(nombreCampo, Convert.ToDouble(valor));
            }

            if (tipoDato.Contains("BIGINT"))
            {
                registro.Add(nombreCampo, Convert.ToInt64(valor));
            }
        }
    }

    /// <summary>
    /// Clase para la transferencia de archivos entre servidores SFTP
    /// </summary>
    public class StfpHelper
    {
        public SftpSettings Sso { get; }
        public SftpSettings Ssd { get; }

        public StfpHelper(SftpSettings sftpSettingsOrigen, SftpSettings sftpSettingsDestino)
        {
            //Configuracion Sftp
            Sso = sftpSettingsOrigen;
            Ssd = sftpSettingsDestino;
        }

        public string TransferirArchivo()
        {
            string Respuesta = string.Empty;

            try
            {
                Validaciones();

                //Crear directorio local para la descarga del archivo
                string localDirectory = AppDomain.CurrentDomain.BaseDirectory;
                localDirectory = Path.Combine(localDirectory, "tempFiles");

                if (!Directory.Exists(localDirectory)) Directory.CreateDirectory(localDirectory);

                using (SftpClient sftp = new SftpClient(Sso.Host, Sso.Port, Sso.UserName, Sso.Password))
                {
                    try
                    {
                        if (Sso.TimeOut > 0)
                        {
                            sftp.OperationTimeout = new TimeSpan(0, 0, 0, 0, Sso.TimeOut);
                        }

                        sftp.Connect();

                        sftp.ChangeDirectory(Sso.RemoteDirectory);

                        var files = sftp.ListDirectory(Sso.RemoteDirectory);

                        int idx = Sso.FileName.LastIndexOf('.');
                        int lng = Sso.FileName.Length;
                        string fileNoExtension = Sso.FileName.Remove(idx, (lng - idx));

                        foreach (var file in files)
                        {
                            //nombre sin extension
                            if (!file.Name.StartsWith(".") && file.Name.ToLower().StartsWith(fileNoExtension.ToLower()))
                            {
                                Sso.FileName = file.Name;
                                Respuesta = TransferirArchivo(Sso, Ssd, sftp, localDirectory);
                            }
                        }

                        if (Respuesta == string.Empty) { Respuesta = "No se encontró el archivo de origen " + Sso.FileName + " en " + sftp.WorkingDirectory; }

                        sftp.Disconnect();
                    }
                    catch (Exception e)
                    {
                        Respuesta = e.Message.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Respuesta = ex.Message.ToString();
            }

            return Respuesta;
        }

        private string TransferirArchivo(SftpSettings Sso, SftpSettings Ssd, SftpClient sftp, string localDirectory)
        {
            string Respuesta = string.Empty;

            using (Stream downloadedFile = System.IO.File.OpenWrite(Path.Combine(localDirectory, Sso.FileName)))
            {
                sftp.DownloadFile(Sso.FileName, downloadedFile);
            }

            if (File.Exists(Path.Combine(localDirectory, Sso.FileName)))
            {
                Respuesta = UploadFile(Ssd, Path.Combine(localDirectory, Sso.FileName), Sso.FileName);

                try
                {
                    File.Delete(Path.Combine(localDirectory, Sso.FileName));
                }
                catch
                {
                    //No se requiere hacer nada. Sólo evita una posible excepción.
                }
            }

            return Respuesta;
        }

        private void Validaciones()
        {
            if (!Sso.RemoteDirectory.StartsWith("/")) Sso.RemoteDirectory = string.Concat("/", Sso.RemoteDirectory);
            if (!Sso.RemoteDirectory.EndsWith("/")) Sso.RemoteDirectory = string.Concat(Sso.RemoteDirectory, "/");

            if (!Ssd.RemoteDirectory.StartsWith("/")) Ssd.RemoteDirectory = string.Concat("/", Ssd.RemoteDirectory);
            if (!Ssd.RemoteDirectory.EndsWith("/")) Ssd.RemoteDirectory = string.Concat(Ssd.RemoteDirectory, "/");
        }

        private string UploadFile(SftpSettings s, string LocalFullPathName, string OldFileName)
        {
            string Respuesta = string.Empty;

            using (SftpClient sftp = new SftpClient(s.Host, s.Port, s.UserName, s.Password))
            {
                try
                {
                    sftp.Connect();

                    sftp.ChangeDirectory(s.RemoteDirectory);

                    using (Stream stream = System.IO.File.OpenRead(LocalFullPathName))
                    {
                        sftp.UploadFile(stream, OldFileName);
                    }

                    var files = sftp.ListDirectory(s.RemoteDirectory);
                    foreach (var file in files)
                    {
                        if (file.Name.ToLower() == OldFileName.ToLower())
                        {
                            Respuesta = "OK";
                            break;
                        }
                    }

                    sftp.Disconnect();
                }
                catch (Exception ex)
                {
                    Respuesta = $"Upload File {ex.Message.ToString()}";
                }
            }

            return Respuesta;
        }
    }
}
