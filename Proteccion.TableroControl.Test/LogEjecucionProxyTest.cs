using Moq;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Proteccion.TableroControl.Test
{
    public class LogEjecucionProxyTest
    {
        readonly Mock<ILogEjecucionDatos> mockDatos;

        public LogEjecucionProxyTest()
        {
            mockDatos = new Mock<ILogEjecucionDatos>();
        }

        [Fact]
        public void InsertarLog_DevuelveFalse()
        {
            // Arrange
            var proxy = new LogEjecucionProxy(mockDatos.Object);

            // Act
            var result = proxy.InsertarLog("", "", "");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EjecutarQuery_DevuelveNull()
        {
            // Arrange
            var proxy = new LogEjecucionProxy(mockDatos.Object);

            // Act
            var result = proxy.EjecutarQuery("");

            // Assert
            Assert.Null(result);
        }
    }
}
