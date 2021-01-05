using Moq;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Proteccion.TableroControl.Test
{
    public class TopicoProxyTest
    {
        readonly Mock<ITopicoDatos> mockDatos;

        public TopicoProxyTest()
        {
            mockDatos = new Mock<ITopicoDatos>();
        }

        [Fact]
        public void ObtenerTopicos_DevuelveEmpty()
        {
            // Arrange
            var proxy = new TopicoProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerTopicos();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ObtenerTopico_DevuelveNull()
        {
            // Arrange
            var proxy = new TopicoProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerTopico(0);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ObtenerIdentificadoresTopicos_DevuelveEmpty()
        {
            // Arrange
            var proxy = new TopicoProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerIdentificadoresTopicos();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void InsertarTopico_DevuelveFalse()
        {
            // Arrange
            var proxy = new TopicoProxy(mockDatos.Object);

            // Act
            var result = proxy.InsertarTopico(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ActualizarTopico_DevuelveFalse()
        {
            // Arrange
            var proxy = new TopicoProxy(mockDatos.Object);

            // Act
            var result = proxy.ActualizarTopico(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ExisteTopico_DevuelveFalse()
        {
            // Arrange
            var proxy = new TopicoProxy(mockDatos.Object);

            // Act
            var result = proxy.ExisteTopico("", "");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ObtenerUltimoOrden_Devuelve0()
        {
            // Arrange
            var proxy = new TopicoProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerUltimoOrden("");

            // Assert
            Assert.Equal(0, result);
        }
    }
}
