using Moq;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Proteccion.TableroControl.Test
{
    public class ParametroProxyTest
    {
        readonly Mock<IParametroDatos> mockDatos;

        public ParametroProxyTest()
        {
            mockDatos = new Mock<IParametroDatos>();
        }

        [Fact]
        public void ObtenerParametros_DevuelveEmpty()
        {
            // Arrange
            var proxy = new ParametroProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerParametros();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ObtenerParametroS1_DevuelveNull()
        {
            // Arrange
            var proxy = new ParametroProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerParametro("");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ObtenerParametroS2_DevuelveNull()
        {
            // Arrange
            var proxy = new ParametroProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerParametro(0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void InsertarParametro_DevuelveFalse()
        {
            // Arrange
            var proxy = new ParametroProxy(mockDatos.Object);

            // Act
            var result = proxy.InsertarParametro(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ActualizarParametro_DevuelveFalse()
        {
            // Arrange
            var proxy = new ParametroProxy(mockDatos.Object);

            // Act
            var result = proxy.ActualizarParametro(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EliminarParametro_DevuelveFalse()
        {
            // Arrange
            var proxy = new ParametroProxy(mockDatos.Object);

            // Act
            var result = proxy.EliminarParametro(0);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ExisteParametro_DevuelveFalse()
        {
            // Arrange
            var proxy = new ParametroProxy(mockDatos.Object);

            // Act
            var result = proxy.ExisteParametro("");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ActualizarFechaProceso_DevuelveFalse()
        {
            // Arrange
            var proxy = new ParametroProxy(mockDatos.Object);

            // Act
            var result = proxy.ActualizarFechaProceso("");

            // Assert
            Assert.False(result);
        }
    }
}
