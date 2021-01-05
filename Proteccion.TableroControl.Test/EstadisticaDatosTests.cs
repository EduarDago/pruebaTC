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
    public class EstadisticaDatosTests
    {
        private readonly Mock<TableroControlContext> mockContext;

        public EstadisticaDatosTests()
        {
            // Arrange - We're mocking our dbSet & dbContext in-memory data
            IQueryable<EjecucionValidacion> ejecuciones = new List<EjecucionValidacion>
            {
                new EjecucionValidacion
                {
                   IdEjecucion = 1,
                   FechaEjecucion = DateTime.Now,
                },
                new EjecucionValidacion
                {
                   IdEjecucion = 2,
                   FechaEjecucion = DateTime.Now
                }
            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSet = new Mock<DbSet<EjecucionValidacion>>();
            mockSet.As<IQueryable<EjecucionValidacion>>().Setup(m => m.Provider).Returns(ejecuciones.Provider);
            mockSet.As<IQueryable<EjecucionValidacion>>().Setup(m => m.Expression).Returns(ejecuciones.Expression);
            mockSet.As<IQueryable<EjecucionValidacion>>().Setup(m => m.ElementType).Returns(ejecuciones.ElementType);
            mockSet.As<IQueryable<EjecucionValidacion>>().Setup(m => m.GetEnumerator()).Returns(ejecuciones.GetEnumerator());

            mockContext = new Mock<TableroControlContext>();
            mockContext.Setup(c => c.EjecucionValidacion).Returns(mockSet.Object);
        }

        [Fact]
        public void GenerarEstadisticas_Invocacion_DevuelveExcepcion()
        {
            // Act - fetch books
            var repository = new EstadisticaDatos(mockContext.Object);

            ////// Asset
            Assert.Throws<ArgumentException>(() => repository.GenerarEstadisticas());
        }

        [Fact]
        public void InsertarEjecucionValidacion_Invocacion_DevuelveValor()
        {
            // Act - Add the book
            var repository = new EstadisticaDatos(mockContext.Object);
            var ejecucion = new EjecucionValidacion
            {
                Cantidad = 200,
                Equipo = "Test",
                Usuario = "Test"
            };

            repository.InsertarEjecucionValidacion(ejecucion);

            // Assert
            // These two lines of code verifies that a Parametro was added once and
            // we saved our changes once.
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
