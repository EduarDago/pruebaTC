using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class ParametroDatosTests
    {
        private readonly Mock<TableroControlContext> mockContext;

        public ParametroDatosTests()
        {
            // Arrange - We're mocking our dbSet & dbContext in-memory data
            IQueryable<Parametro> parametros = new List<Parametro>
            {
                new Parametro
                {
                   IdParametro = 1,
                   Nombre = "FechaProceso",
                   Valor = "Prueba1"
                },
                new Parametro
                {
                    IdParametro = 2,
                   Nombre = "Prueba2",
                   Valor = "ValorPrueba2"
                }

            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSet = new Mock<DbSet<Parametro>>();
            mockSet.As<IQueryable<Parametro>>().Setup(m => m.Provider).Returns(parametros.Provider);
            mockSet.As<IQueryable<Parametro>>().Setup(m => m.Expression).Returns(parametros.Expression);
            mockSet.As<IQueryable<Parametro>>().Setup(m => m.ElementType).Returns(parametros.ElementType);
            mockSet.As<IQueryable<Parametro>>().Setup(m => m.GetEnumerator()).Returns(parametros.GetEnumerator());

            mockContext = new Mock<TableroControlContext>();
            mockContext.Setup(c => c.Parametro).Returns(mockSet.Object);
        }

        [Fact]
        public void ObtenerParametros_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ParametroDatos(mockContext.Object);
            var actual = repository.ObtenerParametros().ToList();

            //// Asset
            Assert.Equal(2, actual.Count);
            Assert.Equal("FechaProceso", actual.FirstOrDefault().Nombre);
        }

        [Theory]
        [InlineData(1, "FechaProceso")]
        [InlineData(2, "Prueba2")]
        public void ObtenerParametroPorId_Invocacion_DevuelveParametro(int id, string expected)
        {
            // Act
            var repository = new ParametroDatos(mockContext.Object);
            var actual = repository.ObtenerParametro(id);

            //// Asset
            Assert.Equal(expected, actual.Nombre);
        }

        [Theory]
        [InlineData("FechaProceso", "FechaProceso")]
        [InlineData("Prueba2", "Prueba2")]
        public void ObtenerParametroPorNombre_Invocacion_DevuelveParametro(string nombre, string expected)
        {
            // Act
            var repository = new ParametroDatos(mockContext.Object);
            var actual = repository.ObtenerParametro(nombre);

            //// Asset
            Assert.Equal(expected, actual.Nombre);
        }

        [Theory]
        [InlineData("FechaProceso", true)]
        [InlineData("Prueba2", true)]
        [InlineData("FechaProcesog", false)]
        public void ExisteParametro_Invocacion_DevuelveValor(string nombre, bool expected)
        {
            // Act - fetch books
            var repository = new ParametroDatos(mockContext.Object);
            var result = repository.ExisteParametro(nombre);

            //// Asset
            Assert.Equal(result, expected);
        }

        [Fact]
        public void InsertarParametro_Invocacion_DevuelveValor()
        {
            // Act - Add the book
            var repository = new ParametroDatos(mockContext.Object);
            var parametro = new Parametro
            {
                IdParametro = 3,
                Nombre = "FechaProceso",
                Valor = "20190101"
            };

            repository.InsertarParametro(parametro);

            // Assert
            // These two lines of code verifies that a Parametro was added once and
            // we saved our changes once.
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void ActualizarParametro_Invocacion_DevuelveValor()
        {
            // Act - Add the book
            var repository = new ParametroDatos(mockContext.Object);
            var parametro = new Parametro
            {
                IdParametro = 2,
                Nombre = "Nuevo",
                Valor = "20190101"
            };

            repository.ActualizarParametro(parametro);

            // Assert
            // These two lines of code verifies that a Parametro was added once and
            // we saved our changes once.
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void EliminarParametro_Invocacion_DevuelveValor()
        {
            // Act - Add the book
            var repository = new ParametroDatos(mockContext.Object);
            repository.EliminarParametro(1);

            // Assert
            // These two lines of code verifies that a Parametro was added once and
            // we saved our changes once.
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void ActualizarFechaProceso_Invocacion_DevuelveValor()
        {
            // Act - Add the book
            var repository = new ParametroDatos(mockContext.Object);
            repository.ActualizarFechaProceso("2019-01-01");

            // Assert
            // These two lines of code verifies that a Parametro was added once and
            // we saved our changes once.
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
