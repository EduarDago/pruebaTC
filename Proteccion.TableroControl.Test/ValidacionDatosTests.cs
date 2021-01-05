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
    public class ValidacionDatosTests
    {
        private readonly Mock<TableroControlContext> mockContext;

        public ValidacionDatosTests()
        {
            // Arrange - We're mocking our dbSet & dbContext in-memory data
            IQueryable<Validacion> parametros = new List<Validacion>
            {
                new Validacion
                {
                  IdValidacion = 1,
                  Nombre ="Validacion 1",
                  Sp = "SP1",
                  Activo = true,
                  IdTipoValidacion = 1,
                  IdEquipo = 1
                },
                new Validacion
                {
                   IdValidacion = 2,
                  Nombre ="Validacion 2",
                  Sp = "SP2",
                  Activo = true,
                  IdTipoValidacion = 1,
                  IdEquipo = 1
                }

            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSet = new Mock<DbSet<Validacion>>();
            mockSet.As<IQueryable<Validacion>>().Setup(m => m.Provider).Returns(parametros.Provider);
            mockSet.As<IQueryable<Validacion>>().Setup(m => m.Expression).Returns(parametros.Expression);
            mockSet.As<IQueryable<Validacion>>().Setup(m => m.ElementType).Returns(parametros.ElementType);
            mockSet.As<IQueryable<Validacion>>().Setup(m => m.GetEnumerator()).Returns(parametros.GetEnumerator());

            mockContext = new Mock<TableroControlContext>();
            mockContext.Setup(c => c.Validacion).Returns(mockSet.Object);
        }

        [Fact]
        public void ActualizarValidacion_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ValidacionDatos(mockContext.Object);
            var validacion = new Validacion
            {

                IdValidacion = 1,
                Nombre = "Validacion 1",
                Sp = "SP1 Act"

            };

            repository.ActualizarValidacion(validacion);

            //// Asset
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void EjecutarValidacion_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ValidacionDatos(mockContext.Object);
            var actual = repository.EjecutarValidacion(1, "Test");

            //// Asset
            Assert.Null(actual);
        }

        [Fact]
        public void EliminarValidacion_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ValidacionDatos(mockContext.Object);
            repository.EliminarValidacion(1);

            //// Asset
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Existe_Validacion_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ValidacionDatos(mockContext.Object);
            var actual = repository.Existe_Validacion("Validacion 1");

            //// Asset
            Assert.True(actual);
        }

        [Fact]
        public void InsertarValidacion_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ValidacionDatos(mockContext.Object);
            var validacion = new Validacion
            {

                IdValidacion = 3,
                Nombre = "Validacion 3",
                Sp = "SP1 Act"

            };

            repository.InsertarValidacion(validacion);

            //// Asset
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void ObtenerInconsistencias_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ValidacionDatos(mockContext.Object);
            var actual = repository.ObtenerInconsistencias(1, "Test");

            //// Asset
            Assert.Empty(actual);
        }

        [Fact]
        public void ObtenerValidacion_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ValidacionDatos(mockContext.Object);
            var actual = repository.ObtenerValidacion(1);

            //// Asset
            Assert.Equal("Validacion 1", actual.Nombre);
        }

        [Fact]
        public void ObtenerValidaciones_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ValidacionDatos(mockContext.Object);
            var actual = repository.ObtenerValidaciones();

            //// Asset
            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public void ObtenerValidacionesActivas_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ValidacionDatos(mockContext.Object);
            var actual = repository.ObtenerValidacionesActivas(1, 1);

            //// Asset
            Assert.Equal(2, actual.Count());
        }
    }
}
