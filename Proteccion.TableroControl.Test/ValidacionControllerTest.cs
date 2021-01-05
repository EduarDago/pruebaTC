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
    public class ValidacionControllerTest
    {
        readonly Mock<IComunProxy> mockComun;
        readonly Mock<IValidacionProxy> mockValidacion;
        readonly Mock<IManagerSession> mockUserSession;
        readonly Mock<ILogger<ValidacionController>> mockLogger;

        public ValidacionControllerTest()
        {
            mockValidacion = new Mock<IValidacionProxy>();
            mockUserSession = new Mock<IManagerSession>();
            mockLogger = new Mock<ILogger<ValidacionController>>();
            mockComun = new Mock<IComunProxy>();
        }

        [Fact]
        public void Index_RetornaViewResult_ConListadoDeValidaciones()
        {
            // Arrange
            mockValidacion.Setup(repo => repo.ObtenerValidaciones())
                .Returns(GetTestData());

            var controller = new ValidacionController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockUserSession.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_RetornaViewResult_ConNotificacion()
        {
            // Arrange
            mockValidacion.Setup(repo => repo.ObtenerValidaciones())
                .Returns(GetTestData());

            mockUserSession.Setup(x => x.Notificacion).Returns(() => new Notificacion());

            var controller = new ValidacionController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockUserSession.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_RetornaViewResult_ConExcepcion()
        {
            // Arrange
            mockValidacion.Setup(repo => repo.ObtenerValidaciones())
                .Throws(new Exception());

            var controller = new ValidacionController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockUserSession.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Nuevo_RetornaViewResult_ConViewModel(bool excepcion)
        {
            // Arrange
            if (excepcion)
            {
                mockComun.Setup(x => x.ObtenerTopicos(It.IsAny<string>())).Throws(new Exception());
            }

            var controller = new ValidacionController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockUserSession.Object);

            // Act
            var result = controller.Nuevo();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(false, false, false, false, false, false)]
        [InlineData(true, false, true, false, false, false)]
        [InlineData(true, false, false, true, false, false)]
        [InlineData(true, false, false, false, false, false)]
        [InlineData(true, true, true, false, false, false)]
        [InlineData(true, true, false, false, true, false)]
        [InlineData(true, true, false, false, false, false)]
        [InlineData(false, false, false, false, false, true)]
        public void GuardarValidacion_RetornaViewResult_ConViewModel(bool existeSP, bool insertaNuevo, bool existeValidacion, bool actOk, bool insOk, bool excepcion)
        {
            // Arrange
            var viewModel = new DetalleValidacionViewModel
            {
                Validacion = new Validacion
                {
                    IdValidacion = insertaNuevo ? 0 : 1,
                    Sp = "Prueba",
                    Nombre = "Otro"
                }
            };

            mockComun.Setup(x => x.ValidarExistenciaSp(It.IsAny<string>(), It.IsAny<string>())).Returns(existeSP);
            mockValidacion.Setup(x => x.ObtenerValidacion(It.IsAny<int>())).Returns(new Validacion { Nombre = "" });
            mockValidacion.Setup(x => x.ValidarExistencia(It.IsAny<string>())).Returns(existeValidacion);
            mockValidacion.Setup(x => x.ActualizarValidacion(It.IsAny<Validacion>())).Returns(actOk);
            mockValidacion.Setup(x => x.InsertarValidacion(It.IsAny<Validacion>())).Returns(insOk);

            var controller = new ValidacionController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockUserSession.Object);

            if (excepcion)
            {
                viewModel = null;
            }

            // Act
            var result = controller.GuardarValidacion(viewModel);

            // Assert
            if (actOk || insOk)
            {
                Assert.IsType<RedirectToActionResult>(result);
            }
            else
            {
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public void Detalle_RetornaViewResult_ConViewModel()
        {
            // Arrange
            var controller = new ValidacionController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockUserSession.Object);

            // Act
            var result = controller.Detalle(1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Editar_RetornaViewResult_ConViewModel()
        {
            // Arrange
            var controller = new ValidacionController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockUserSession.Object);

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
                mockValidacion.Setup(x => x.EliminarValidacion(It.IsAny<int>())).Throws(new Exception());
            }

            var controller = new ValidacionController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockUserSession.Object);

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

        private List<Validacion> GetTestData()
        {
            var data = new List<Validacion>()
            {
                new Validacion
                {
                    IdValidacion = 1,
                    Nombre = "Prueba1"
                },
                new Validacion
                {
                    IdValidacion = 2,
                    Nombre = "Prueba2"
                }
            };

            return data;
        }
    }
}
