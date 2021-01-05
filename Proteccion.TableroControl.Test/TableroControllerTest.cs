using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Presentacion.Controllers;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Proteccion.TableroControl.Test
{
    public class TableroControllerTest
    {
        readonly Mock<IValidacionProxy> mockValidacion;
        readonly Mock<IEstadisticaProxy> mockEstadistica;
        readonly Mock<IComunProxy> mockComun;
        readonly Mock<ILogEjecucionProxy> mockEjecucion;
        readonly Mock<IHttpContextAccessor> mockHttpContext;
        readonly Mock<ILogger<TableroController>> mockLogger;

        public TableroControllerTest()
        {
            mockValidacion = new Mock<IValidacionProxy>();
            mockComun = new Mock<IComunProxy>();
            mockEjecucion = new Mock<ILogEjecucionProxy>();
            mockLogger = new Mock<ILogger<TableroController>>();
            mockHttpContext = new Mock<IHttpContextAccessor>();
            mockEstadistica = new Mock<IEstadisticaProxy>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Index_RetornaViewResult(bool excepcion)
        {
            // Arrange
            if (excepcion)
            {
                mockComun.Setup(repo => repo.ObtenerTopicos(It.IsAny<string>())).Throws(new Exception());
            }

            var controller = new TableroController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockEjecucion.Object, mockHttpContext.Object, mockEstadistica.Object);

            // Act
            var result = controller.Index(0, 0);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void DetalleInconsistencias_RetornaViewResult()
        {
            // Arrange
            mockValidacion.Setup(repo => repo.ObtenerInconsistencias(It.IsAny<int>(), It.IsAny<string>())).Returns(new List<Inconsistencia>());
            mockHttpContext.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = new System.Security.Claims.ClaimsPrincipal()
            });

            var controller = new TableroController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockEjecucion.Object, mockHttpContext.Object, mockEstadistica.Object);

            // Act
            var result = controller.DetalleInconsistencias(0);

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void EjecucionScripts_RetornaViewResult()
        {
            // Arrange
            var controller = new TableroController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockEjecucion.Object, mockHttpContext.Object, mockEstadistica.Object);

            // Act
            var result = controller.EjecucionScripts();

            // Assert
            Assert.IsType<ViewResult>(result);
        }


        [Theory]
        [InlineData("SELECT 1", true)]
        [InlineData("INSERT INT CD", false)]
        public void ValidacionSql_RetornaViewResult(string query, bool expected)
        {
            // Arrange
            var controller = new TableroController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockEjecucion.Object, mockHttpContext.Object, mockEstadistica.Object);
            List<string> errores;

            // Act
            var result = controller.ValidacionSql(query, out errores);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("SELECT 1")]
        [InlineData("INSERT INT CD")]
        public void ValidarScript_RetornaViewResult(string query)
        {
            // Arrange
            var controller = new TableroController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockEjecucion.Object, mockHttpContext.Object, mockEstadistica.Object);

            // Act
            var result = controller.ValidarScript(query);

            // Assert
            Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public void EjecutarScript_RetornaViewResult()
        {
            // Arrange
            mockHttpContext.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = new System.Security.Claims.ClaimsPrincipal()
            });

            var controller = new TableroController(mockLogger.Object, mockComun.Object, mockValidacion.Object, mockEjecucion.Object, mockHttpContext.Object, mockEstadistica.Object);

            // Act
            var result = controller.EjecutarScript("");

            // Assert
            Assert.IsType<JsonResult>(result);
        }
    }
}
