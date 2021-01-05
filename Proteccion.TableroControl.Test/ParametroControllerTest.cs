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
using Xunit;

namespace Proteccion.TableroControl.Test
{
    public class ParametroControllerTest
    {
        readonly Mock<IParametroProxy> mockParametro;
        readonly Mock<IManagerSession> mockUserSession;
        readonly Mock<ILogger<ParametroController>> mockLogger;

        public ParametroControllerTest()
        {
            mockParametro = new Mock<IParametroProxy>();
            mockUserSession = new Mock<IManagerSession>();
            mockLogger = new Mock<ILogger<ParametroController>>();
        }

        [Fact]
        public void Index_RetornaViewResult_ConListadoDeParametros()
        {
            // Arrange
            mockParametro.Setup(repo => repo.ObtenerParametros())
                .Returns(GetTestData());

            var controller = new ParametroController(mockLogger.Object, mockParametro.Object, mockUserSession.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_RetornaViewResult_ConNotificacion()
        {
            // Arrange
            mockParametro.Setup(repo => repo.ObtenerParametros())
                .Returns(GetTestData());

            mockUserSession.Setup(x => x.Notificacion).Returns(() => new Notificacion());

            var controller = new ParametroController(mockLogger.Object, mockParametro.Object, mockUserSession.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_RetornaViewResult_ConExcepcion()
        {
            // Arrange
            mockParametro.Setup(repo => repo.ObtenerParametros())
                .Returns(GetTestData());

            var controller = new ParametroController(mockLogger.Object, mockParametro.Object, null);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Nuevo_RetornaViewResult_ConViewModel()
        {
            // Arrange
            var controller = new ParametroController(mockLogger.Object, mockParametro.Object, mockUserSession.Object);

            // Act
            var result = controller.Nuevo();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void NuevoPost_RetornaViewResult_ConViewModel(bool parametroExiste, bool insertaNuevo, bool excepcion)
        {
            // Arrange
            var viewModel = new ParametroViewModel
            {
                Parametro = new Parametro
                {
                    Nombre = "Prueba"
                }
            };

            mockParametro.Setup(x => x.ExisteParametro(It.IsAny<string>())).Returns(parametroExiste);
            mockParametro.Setup(x => x.InsertarParametro(It.IsAny<Parametro>())).Returns(insertaNuevo);

            var controller = new ParametroController(mockLogger.Object, mockParametro.Object, mockUserSession.Object);

            if (excepcion)
            {
                viewModel = null;
            }

            // Act
            var result = controller.Nuevo(viewModel);

            // Assert
            if (insertaNuevo)
            {
                Assert.IsType<RedirectToActionResult>(result);
            }
            else
            {
                Assert.IsType<ViewResult>(result);
            }

        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void Editar_RetornaViewResult_ConViewModel(bool excepcion)
        {
            // Arrange
            if (!excepcion)
            {
                mockParametro.Setup(x => x.ObtenerParametro(It.IsAny<int>())).Returns(() => new Parametro());
            }
            else
            {
                mockParametro.Setup(x => x.ObtenerParametro(It.IsAny<int>())).Throws(new Exception());
            }
            
            var controller = new ParametroController(mockLogger.Object, mockParametro.Object, mockUserSession.Object);

            // Act
            var result = controller.Editar(1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public void EditarPost_RetornaViewResult_ConViewModel(bool parametroExiste, bool actualizar, bool excepcion)
        {
            // Arrange
            var viewModel = new ParametroViewModel
            {
                Parametro = new Parametro
                {
                    Nombre = "Prueba"
                }
            };

            mockParametro.Setup(x => x.ObtenerParametro(It.IsAny<int>())).Returns(new Parametro { Nombre = "PruebaX" });
            mockParametro.Setup(x => x.ExisteParametro(It.IsAny<string>())).Returns(parametroExiste);
            mockParametro.Setup(x => x.ActualizarParametro(It.IsAny<Parametro>())).Returns(actualizar);

            var controller = new ParametroController(mockLogger.Object, mockParametro.Object, mockUserSession.Object);

            if (excepcion)
            {
                viewModel = null;
            }

            // Act
            var result = controller.Editar(viewModel);

            // Assert
            if (actualizar)
            {
                Assert.IsType<RedirectToActionResult>(result);
            }
            else
            {
                Assert.IsType<ViewResult>(result);
            }
        }

        [Fact]
        public void ObtenerFechaProceso_RetornaViewResult()
        {
            // Arrange
            mockParametro.Setup(x => x.ObtenerParametro(It.IsAny<string>())).Returns(() => new Parametro { Valor = "2019-01-01" });

            var controller = new ParametroController(mockLogger.Object, mockParametro.Object, mockUserSession.Object);

            // Act
            var result = controller.ObtenerFechaProceso();

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        private List<Parametro> GetTestData()
        {
            var data = new List<Parametro>()
            {
                new Parametro
                {
                    IdParametro = 1,
                    Nombre = "FechaProceso",
                    Valor = "2019-01-01"
                },
                new Parametro
                {
                    IdParametro = 2,
                    Nombre = "CorreoImportacion",
                    Valor = "jonathan@makersolutions.co"
                }
            };

            return data;
        }
    }
}
