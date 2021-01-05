using Microsoft.EntityFrameworkCore;
using Moq;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Datos.DataContext;
using Proteccion.TableroControl.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Proteccion.TableroControl.Test
{
    public class LogEjecucionDatosTest
    {
        private readonly Mock<TableroControlContext> mockContext;

        public LogEjecucionDatosTest()
        {
            // Arrange - We're mocking our dbSet & dbContext in-memory data
            IQueryable<LogEjecucion> ejecuciones = new List<LogEjecucion>
            {
                new LogEjecucion
                {
                   IdLogEjecucion = 1,
                   FechaEjecucion = DateTime.Now,
                },
                new LogEjecucion
                {
                   IdLogEjecucion = 2,
                   FechaEjecucion = DateTime.Now
                }
            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSet = new Mock<DbSet<LogEjecucion>>();
            mockSet.As<IQueryable<LogEjecucion>>().Setup(m => m.Provider).Returns(ejecuciones.Provider);
            mockSet.As<IQueryable<LogEjecucion>>().Setup(m => m.Expression).Returns(ejecuciones.Expression);
            mockSet.As<IQueryable<LogEjecucion>>().Setup(m => m.ElementType).Returns(ejecuciones.ElementType);
            mockSet.As<IQueryable<LogEjecucion>>().Setup(m => m.GetEnumerator()).Returns(ejecuciones.GetEnumerator());

            mockContext = new Mock<TableroControlContext>();
            mockContext.Setup(c => c.LogEjecucion).Returns(mockSet.Object);
        }

        [Fact]
        public void GenerarEstadisticas_Invocacion_DevuelveExcepcion()
        {
            // Act - fetch books
            var repository = new LogEjecucionDatos(mockContext.Object);
            string resultado = repository.EjecutarQuery("Select 1");

            ////// Asset
            Assert.NotEqual(string.Empty, resultado);
        }

        [Fact]
        public void InsertarLog_Invocacion_DevuelveValor()
        {
            // Act - Add the book
            var repository = new LogEjecucionDatos(mockContext.Object);
            var ejecucion = new LogEjecucion
            {
                IdLogEjecucion = 8,
                FechaEjecucion = DateTime.Now,
                Usuario = "Test"
            };

            repository.InsertarLog(ejecucion);

            // Assert
            // These two lines of code verifies that a Parametro was added once and
            // we saved our changes once.
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
