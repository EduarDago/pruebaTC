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
    public class TopicoControllerTest
    {
        readonly Mock<ITopicoProxy> mockTopico;
        readonly Mock<IManagerSession> mockUserSession;
        readonly Mock<ILogger<TopicoController>> mockLogger;

        public TopicoControllerTest()
        {
            mockTopico = new Mock<ITopicoProxy>();
            mockUserSession = new Mock<IManagerSession>();
            mockLogger = new Mock<ILogger<TopicoController>>();
        }

        [Fact]
        public void Index_RetornaViewResult_ConListadoDeTopicos()
        {
            // Arrange
            mockTopico.Setup(repo => repo.ObtenerTopicos())
                .Returns(GetTestData());

            var controller = new TopicoController(mockLogger.Object, mockTopico.Object, mockUserSession.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_RetornaViewResult_ConNotificacion()
        {
            // Arrange
            mockTopico.Setup(repo => repo.ObtenerTopicos())
                .Returns(GetTestData());

            mockUserSession.Setup(x => x.Notificacion).Returns(() => new Notificacion());

            var controller = new TopicoController(mockLogger.Object, mockTopico.Object, mockUserSession.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_RetornaViewResult_ConExcepcion()
        {
            // Arrange
            mockTopico.Setup(repo => repo.ObtenerTopicos())
                .Returns(GetTestData());

            var controller = new TopicoController(mockLogger.Object, mockTopico.Object, null);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Nuevo_RetornaViewResult_ConViewModel()
        {
            // Arrange
            var controller = new TopicoController(mockLogger.Object, mockTopico.Object, mockUserSession.Object);

            // Act
            var result = controller.Nuevo();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        [InlineData(false, false, true)]
        public void NuevoPost_RetornaViewResult_ConViewModel(bool existeTopico, bool insertaNuevo, bool excepcion)
        {
            // Arrange
            var viewModel = new TopicoViewModel
            {
                Topico = new Topico
                {
                    Identificador = "Prueba",
                    Valor = "Prueba"
                }
            };

            mockTopico.Setup(x => x.ExisteTopico(It.IsAny<string>(), It.IsAny<string>())).Returns(existeTopico);
            mockTopico.Setup(x => x.InsertarTopico(It.IsAny<Topico>())).Returns(insertaNuevo);

            var controller = new TopicoController(mockLogger.Object, mockTopico.Object, mockUserSession.Object);

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
                mockTopico.Setup(x => x.ObtenerIdentificadoresTopicos()).Returns(() => new List<string>());
            }
            else
            {
                mockTopico.Setup(x => x.ObtenerIdentificadoresTopicos()).Throws(new Exception());
            }

            var controller = new TopicoController(mockLogger.Object, mockTopico.Object, mockUserSession.Object);

            // Act
            var result = controller.Editar(1);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        [InlineData(false, false, true)]
        public void EditarPost_RetornaViewResult_ConViewModel(bool topicoExiste, bool actualizar, bool excepcion)
        {
            // Arrange
            var viewModel = new TopicoViewModel
            {
                Topico = new Topico
                {
                    Valor = "Prueba"
                }
            };

            mockTopico.Setup(x => x.ObtenerTopico(It.IsAny<int>())).Returns(new Topico { Valor = "PruebaX" });
            mockTopico.Setup(x => x.ExisteTopico(It.IsAny<string>(), It.IsAny<string>())).Returns(topicoExiste);
            mockTopico.Setup(x => x.ActualizarTopico(It.IsAny<Topico>())).Returns(actualizar);

            var controller = new TopicoController(mockLogger.Object, mockTopico.Object, mockUserSession.Object);

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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Eliminar_RetornaViewResult(bool excepcion)
        {
            // Arrange
            if (excepcion)
            {
                mockTopico.Setup(x => x.EliminarTopico(It.IsAny<int>())).Throws(new Exception());
            }
           
            var controller = new TopicoController(mockLogger.Object, mockTopico.Object, mockUserSession.Object);

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
        public void ObtenerOrden_RetornaViewResult(bool excepcion)
        {
            // Arrange
            if (excepcion)
            {
                mockTopico.Setup(x => x.ObtenerUltimoOrden(It.IsAny<string>())).Throws(new Exception());
            }
            else
            {
                mockTopico.Setup(x => x.ObtenerUltimoOrden(It.IsAny<string>())).Returns(1);
            }
            

            var controller = new TopicoController(mockLogger.Object, mockTopico.Object, mockUserSession.Object);

            // Act
            var result = controller.ObtenerOrden("");

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        private List<Topico> GetTestData()
        {
            var data = new List<Topico>()
            {
                new Topico
                {
                    IdTopico = 1,
                    Valor = "2019-01-01"
                },
                new Topico
                {
                    IdTopico = 2,
                    Valor = "jonathan@makersolutions.co"
                }
            };

            return data;
        }
    }
}
