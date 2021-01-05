using Moq;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Proteccion.TableroControl.Test
{
    public class ValidacionProxyTest
    {
        readonly Mock<IValidacionDatos> mockDatos;

        public ValidacionProxyTest()
        {
            mockDatos = new Mock<IValidacionDatos>();
        }

        [Fact]
        public void ObtenerValidaciones_DevuelveEmpty()
        {
            // Arrange
            var proxy = new ValidacionProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerValidaciones();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ObtenerValidacionesActivas_DevuelveEmpty()
        {
            // Arrange
            var proxy = new ValidacionProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerValidacionesActivas(0, 0);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ObtenerValidacion_DevuelveNull()
        {
            // Arrange
            var proxy = new ValidacionProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerValidacion(0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void InsertarValidacion_DevuelveFalse()
        {
            // Arrange
            var proxy = new ValidacionProxy(mockDatos.Object);

            // Act
            var result = proxy.InsertarValidacion(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ActualizarValidacion_DevuelveFalse()
        {
            // Arrange
            var proxy = new ValidacionProxy(mockDatos.Object);

            // Act
            var result = proxy.ActualizarValidacion(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidarExistencia_DevuelveFalse()
        {
            // Arrange
            var proxy = new ValidacionProxy(mockDatos.Object);

            // Act
            var result = proxy.ValidarExistencia("");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EjecutarValidacion_DevuelveNull()
        {
            // Arrange
            var proxy = new ValidacionProxy(mockDatos.Object);

            // Act
            var result = proxy.EjecutarValidacion(0, "");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ObtenerInconsistencias_DevuelveNull()
        {
            // Arrange
            var proxy = new ValidacionProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerInconsistencias(0, "");

            // Assert
            Assert.Null(result);
        }
    }
}
