using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Interfaces;
using Proteccion.TableroControl.Presentacion.Controllers;
using Proteccion.TableroControl.Presentacion.ViewModels;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Proteccion.TableroControl.Test
{
    public class OrigenControllerTest
    {
        readonly Mock<IOrigenDatosProxy> mockOrigen;
        readonly Mock<IComunProxy> mockComun;
        readonly Mock<ILogger<OrigenController>> mockLogger;

        public OrigenControllerTest()
        {
            mockOrigen = new Mock<IOrigenDatosProxy>();
            mockComun = new Mock<IComunProxy>();
            mockLogger = new Mock<ILogger<OrigenController>>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Index_RetornaViewResult_ConListadoDeOrigenes(bool excepcion)
        {
            // Arrange
            if (excepcion)
            {
                mockOrigen.Setup(repo => repo.ObtenerConfiguracionesOrigen()).Throws(new Exception());
            }
            else
            {
                mockOrigen.Setup(repo => repo.ObtenerConfiguracionesOrigen())
                .Returns(GetTestData());
            }

            var controller = new OrigenController(mockLogger.Object, mockOrigen.Object, mockComun.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Nuevo_RetornaViewResult(bool excepcion)
        {
            // Arrange
            if (excepcion)
            {
                mockComun.Setup(repo => repo.ObtenerTopicos(It.IsAny<string>())).Throws(new Exception());
            }
            else
            {
                mockComun.Setup(repo => repo.ObtenerTopicos(It.IsAny<string>())).Returns(new List<Topico>());
            }

            var controller = new OrigenController(mockLogger.Object, mockOrigen.Object, mockComun.Object);

            // Act
            var result = controller.Nuevo();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Detalle_RetornaViewResult(bool excepcion)
        {
            // Arrange
            if (excepcion)
            {
                mockComun.Setup(repo => repo.ObtenerTopicos(It.IsAny<string>())).Throws(new Exception());
            }
            else
            {
                mockComun.Setup(repo => repo.ObtenerTopicos(It.IsAny<string>())).Returns(new List<Topico>());
                mockOrigen.Setup(repo => repo.ObtenerConfiguracionOrigen(It.IsAny<int>())).Returns(new OrigenDato());
            }

            var controller = new OrigenController(mockLogger.Object, mockOrigen.Object, mockComun.Object);

            // Act
            var result = controller.Detalle(1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Editar_RetornaViewResult(bool excepcion)
        {
            // Arrange
            if (excepcion)
            {
                mockOrigen.Setup(repo => repo.ObtenerConfiguracionOrigen(It.IsAny<int>())).Throws(new Exception());
            }
            else
            {
                mockComun.Setup(repo => repo.ObtenerTopicos(It.IsAny<string>())).Returns(new List<Topico>());
                mockOrigen.Setup(repo => repo.ObtenerConfiguracionOrigen(It.IsAny<int>())).Returns(new OrigenDato()
                {
                    Campos = new List<CampoOrigen>()
                });
            }

            var controller = new OrigenController(mockLogger.Object, mockOrigen.Object, mockComun.Object);

            // Act
            var result = controller.Editar(1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Eliminar_RetornaViewResult(bool excepcion)
        {
            // Arrange
            if (excepcion)
            {
                mockOrigen.Setup(repo => repo.EliminarOrigen(It.IsAny<int>())).Throws(new Exception());
            }
            else
            {
                mockOrigen.Setup(repo => repo.EliminarOrigen(It.IsAny<int>())).Returns(true);
            }

            var controller = new OrigenController(mockLogger.Object, mockOrigen.Object, mockComun.Object);

            // Act
            var result = controller.Eliminar(1);

            // Assert
            if (excepcion)
            {
                Assert.IsType<ViewResult>(result);
            }
            else
            {
                Assert.IsType<RedirectToActionResult>(result);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Importar_RetornaViewResult(bool excepcion)
        {
            // Arrange
            if (excepcion)
            {
                mockOrigen.Setup(repo => repo.ConsultarEjecuciones()).Throws(new Exception());
            }
            else
            {
                mockOrigen.Setup(repo => repo.ConsultarEjecuciones()).Returns(new List<EjecucionImportacion>());
            }

            var controller = new OrigenController(mockLogger.Object, mockOrigen.Object, mockComun.Object);

            // Act
            var result = controller.Importar();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(false, false, false, false, false)]
        [InlineData(false, true, false, true, false)]
        [InlineData(false, true, false, false, false)]
        [InlineData(false, true, true, true, false)]
        [InlineData(false, true, true, false, true)]
        [InlineData(false, true, true, false, false)]
        [InlineData(true, false, false, false, false)]
        public void GuardarConfiguracion_RetornaJsonResult(bool excepcion, bool existeSP, bool insertar, bool existeOrigen, bool existeTabla)
        {
            // Arrange
            var origen = new OrigenDato
            {
                Procedimiento = "XXX",
                IdOrigenDato = insertar ? 0 : 1
            };

            if (excepcion)
            {
                origen = null;
            }
            else
            {
                mockComun.Setup(repo => repo.ValidarExistenciaSp(It.IsAny<string>(), It.IsAny<string>())).Returns(existeSP);
                mockComun.Setup(repo => repo.ValidarExistenciaTabla(It.IsAny<string>(), It.IsAny<string>())).Returns(existeTabla);
                mockOrigen.Setup(repo => repo.ObtenerConfiguracionOrigen(It.IsAny<int>())).Returns(new OrigenDato { Nombre = "P" });
                mockOrigen.Setup(repo => repo.ExisteOrigen(It.IsAny<string>())).Returns(existeOrigen);
            }

            var controller = new OrigenController(mockLogger.Object, mockOrigen.Object, mockComun.Object);

            // Act
            var result = controller.GuardarConfiguracion(origen);

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Visor_RetornaViewResult(bool excepcion)
        {
            // Arrange
            if (excepcion)
            {
                mockOrigen.Setup(repo => repo.ObtenerOrigenes()).Throws(new Exception());
            }
            else
            {
                mockOrigen.Setup(repo => repo.ObtenerOrigenes()).Returns(new List<OrigenDato>());
            }

            var controller = new OrigenController(mockLogger.Object, mockOrigen.Object, mockComun.Object);

            // Act
            var result = controller.Visor();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ObtenerDatos_RetornaJsonResult(bool excepcion)
        {
            // Arrange
            if (excepcion)
            {
                mockOrigen.Setup(repo => repo.ObtenerConfiguracionOrigen(It.IsAny<int>())).Throws(new Exception());
            }
            else
            {
                mockOrigen.Setup(repo => repo.ObtenerConfiguracionOrigen(It.IsAny<int>())).Returns(new OrigenDato
                {
                    Campos = new List<CampoOrigen>
                    {
                        new CampoOrigen{ Nombre = "Prueba"}
                    }
                });
                mockOrigen.Setup(repo => repo.ObtenerDatosTabla(It.IsAny<int>())).Returns(new Newtonsoft.Json.Linq.JArray());
            }

            var controller = new OrigenController(mockLogger.Object, mockOrigen.Object, mockComun.Object);

            // Act
            var result = controller.ObtenerDatos(1);

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        private List<OrigenDato> GetTestData()
        {
            var data = new List<OrigenDato>()
            {
                new OrigenDato
                {
                   IdOrigenDato = 1,
                   Descripcion = "Prueba 1"
                },
                new OrigenDato
                {
                    IdOrigenDato = 2,
                   Descripcion = "Prueba 2"
                }
            };

            return data;
        }
    }
}
