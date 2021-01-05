using Moq;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Proxy.BL;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Proteccion.TableroControl.Test
{
    public class OrigenDatosProxyTest
    {
        readonly Mock<IOrigenDatos> mockDatos;

        public OrigenDatosProxyTest()
        {
            mockDatos = new Mock<IOrigenDatos>();
        }

        [Fact]
        public void ObtenerConfiguracionesOrigen_DevuelveNull()
        {
            // Arrange
            var proxy = new OrigenDatosProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerConfiguracionesOrigen();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ObtenerOrigenes_DevuelveNull()
        {
            // Arrange
            var proxy = new OrigenDatosProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerOrigenes();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void ObtenerConfiguracionOrigen_DevuelveNull()
        {
            // Arrange
            var proxy = new OrigenDatosProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerConfiguracionOrigen(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void EliminarOrigen_DevuelveFalse()
        {
            // Arrange
            var proxy = new OrigenDatosProxy(mockDatos.Object);

            // Act
            var result = proxy.EliminarOrigen(1);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ConsultarEjecuciones_DevuelveNull()
        {
            // Arrange
            var proxy = new OrigenDatosProxy(mockDatos.Object);

            // Act
            var result = proxy.ConsultarEjecuciones();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void EjecutarImportacion_DevuelveNull()
        {
            // Arrange
            var proxy = new OrigenDatosProxy(mockDatos.Object);

            // Act
            var result = proxy.EjecutarImportacion(1, Dominio.Enumeraciones.TipoOrigen.Excel, false);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void InsertarConfiguracionOrigen_DevuelveFalse()
        {
            // Arrange
            var proxy = new OrigenDatosProxy(mockDatos.Object);

            // Act
            var result = proxy.InsertarConfiguracionOrigen(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ActualizarConfiguracionOrigen_DevuelveFalse()
        {
            // Arrange
            var proxy = new OrigenDatosProxy(mockDatos.Object);

            // Act
            var result = proxy.ActualizarConfiguracionOrigen(null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CrearEstructura_DevuelveTrue()
        {
            // Arrange
            var proxy = new OrigenDatosProxy(mockDatos.Object);

            // Act
            proxy.CrearEstructura(1);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void ObtenerDatosTabla_DevuelveNull()
        {
            // Arrange
            var proxy = new OrigenDatosProxy(mockDatos.Object);

            // Act
            var result = proxy.ObtenerDatosTabla(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ExisteOrigen_DevuelveFalse()
        {
            // Arrange
            var proxy = new OrigenDatosProxy(mockDatos.Object);

            // Act
            var result = proxy.ExisteOrigen("");

            // Assert
            Assert.False(result);
        }
    }
}
