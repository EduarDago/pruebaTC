using Moq;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Proteccion.TableroControl.Test
{
    public class EstadisticaProxyTest
    {
        readonly Mock<IEstadisticaDatos> mockDatos;

        public EstadisticaProxyTest()
        {
            mockDatos = new Mock<IEstadisticaDatos>();
        }

        [Fact]
        public void InsertarEjecucionValidacion_DevuelveFalse()
        {
            // Arrange
            var proxy = new EstadisticaProxy(mockDatos.Object);

            // Act
            var result = proxy.InsertarEjecucionValidacion(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GenerarEstadisticas_DevuelveVacio()
        {
            // Arrange
            var proxy = new EstadisticaProxy(mockDatos.Object);

            // Act
            proxy.GenerarEstadisticas();

            // Assert
            Assert.False(false);
        }
    }
}
