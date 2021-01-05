using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Interfaces;
using Proteccion.TableroControl.Presentacion.Hubs;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.AspNetCore.SignalR;
using Proteccion.TableroControl.Dominio.Enumeraciones;

namespace Proteccion.TableroControl.Test
{
    public class NotificationHubTest
    {
        readonly Mock<IValidacionProxy> validacionMock;
        readonly Mock<IOrigenDatosProxy> origenMock;
        readonly Mock<IEstadisticaProxy> estadisticaMock;
        readonly Mock<IComunProxy> comunMock;
        readonly Mock<IParametroProxy> parametroMock;
        readonly Mock<IEmail> envioMock;
        readonly Mock<IHostingEnvironment> environmentMock;
        readonly Mock<ILogger<NotificacionHub>> loggerMock;

        public NotificationHubTest()
        {
            validacionMock = new Mock<IValidacionProxy>();
            origenMock = new Mock<IOrigenDatosProxy>();
            estadisticaMock = new Mock<IEstadisticaProxy>();
            comunMock = new Mock<IComunProxy>();
            parametroMock = new Mock<IParametroProxy>();
            envioMock = new Mock<IEmail>();
            environmentMock = new Mock<IHostingEnvironment>();
            loggerMock = new Mock<ILogger<NotificacionHub>>();
        }

        [Fact]
        public void EjecutarValidaciones_RetornaVoid()
        {
            // Arrange
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            validacionMock.Setup(x => x.EjecutarValidacion(It.IsAny<int>(), It.IsAny<string>())).Returns(new ResultadoEjecucionValidacion
            {
                Exitoso = false,
                Inconsistencias = new List<Inconsistencia>
                {
                    new Inconsistencia { Detalle = "Prueba" },
                    new Inconsistencia { Detalle = "Prueba2" }
                }
            });

            validacionMock.Setup(x => x.ObtenerValidaciones()).Returns(ObtenerValidaciones());
            environmentMock.Setup(x => x.WebRootPath).Returns("../../../");

            var hub = new NotificacionHub(envioMock.Object, environmentMock.Object, loggerMock.Object, parametroMock.Object, origenMock.Object, comunMock.Object, validacionMock.Object, estadisticaMock.Object)
            {
                Clients = mockClients.Object
            };

            // Act
            hub.EjecutarValidaciones(new List<int> { 1, 2, 3 }, "X", "2", new UsuarioLogueado { NombreUsuario = "Juan" });

            // Assert
            mockClients.Verify(clients => clients.All, Times.AtLeastOnce);
        }

        [Fact]
        public void EjecutarImportaciones_RetornaVoid()
        {
            // Arrange
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();
            origenMock.Setup(x => x.ConsultarEjecuciones()).Returns(ObtenerImportaciones());
            origenMock.Setup(x => x.EjecutarImportacion(It.IsAny<int>(), It.IsAny<TipoOrigen>(), It.IsAny<bool>())).Returns(new EjecucionImportacion { Exitoso = false });
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            environmentMock.Setup(x => x.WebRootPath).Returns("../../../");

            var hub = new NotificacionHub(envioMock.Object, environmentMock.Object, loggerMock.Object, parametroMock.Object, origenMock.Object, comunMock.Object, validacionMock.Object, estadisticaMock.Object)
            {
                Clients = mockClients.Object
            };

            // Act
            hub.EjecutarImportaciones(new List<int> { 1, 2, 3 }, new List<int> { 1, 2, 3 }, new UsuarioLogueado { NombreUsuario = "Juan" });

            // Assert
            mockClients.Verify(clients => clients.All, Times.AtLeastOnce);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void CambiarFechaProceso_RetornaVoid(bool mismaFecha)
        {
            // Arrange
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();

            parametroMock.Setup(x => x.ActualizarFechaProceso(It.IsAny<string>())).Returns(true);
            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);
            environmentMock.Setup(x => x.WebRootPath).Returns("../../../");

            if (mismaFecha)
            {
                parametroMock.Setup(x => x.ObtenerParametro(It.IsAny<string>())).Returns(new Parametro { Valor = "2019-01-01" });
            }

            var hub = new NotificacionHub(envioMock.Object, environmentMock.Object, loggerMock.Object, parametroMock.Object, origenMock.Object, comunMock.Object, validacionMock.Object, estadisticaMock.Object)
            {
                Clients = mockClients.Object
            };

            // Act
            hub.CambiarFechaProceso(new DateTime(2019, 01, 01), new UsuarioLogueado { NombreUsuario = "Juan" });

            // Assert
            mockClients.Verify(clients => clients.All, Times.AtLeastOnce);
        }

        private List<EjecucionImportacion> ObtenerImportaciones()
        {
            var listado = new List<EjecucionImportacion>
            {
                new EjecucionImportacion
                {
                    IdOrigenDato = 1,
                },
                new EjecucionImportacion
                {
                    IdOrigenDato = 2,
                },
                new EjecucionImportacion
                {
                    IdOrigenDato = 3,
                }
            };

            return listado;
        }

        private List<Validacion> ObtenerValidaciones()
        {
            var listado = new List<Validacion>
            {
                new Validacion
                {
                    IdValidacion = 1
                },
                new Validacion
                {
                    IdValidacion = 2
                },
                new Validacion
                {
                    IdValidacion = 3
                }
            };

            return listado;
        }
    }
}
