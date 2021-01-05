using Moq;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Proteccion.TableroControl.Test
{
    public class ComunProxyTest
    {
        readonly Mock<IComunDatos> mockDatos;

        public ComunProxyTest()
        {
            mockDatos = new Mock<IComunDatos>();
        }

        [Fact]
        public void ValidarExistenciaSp_DevuelveFalse()
        {
            // Arrange
            var proxy = new ComunProxy(mockDatos.Object);

            // Act
            var result = proxy.ValidarExistenciaSp("","");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidarExistenciaTabla_DevuelveFalse()
        {
            // Arrange
            var proxy = new ComunProxy(mockDatos.Object);

            // Act
            var result = proxy.ValidarExistenciaTabla("", "");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ObtenerTopicos_DevuelveNull()
        {
            // Arrange
            var proxy = new ComunProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerTopicos("");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ObtenerParametro_DevuelveNull()
        {
            // Arrange
            var proxy = new ComunProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerParametro("");

            // Assert
            Assert.Null(result);
        }
    }
}
