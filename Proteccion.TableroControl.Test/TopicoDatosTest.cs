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
    public class TopicoDatosTest
    {
        private readonly Mock<TableroControlContext> mockContext;

        public TopicoDatosTest()
        {
            // Arrange - We're mocking our dbSet & dbContext in-memory data
            IQueryable<Topico> parametros = new List<Topico>
            {
                new Topico
                {
                   IdTopico =1,
                   Identificador ="Tipo",
                   TextoMostrar ="Topico 1",
                   Valor = "Valor 1",
                   Orden = 1
                },
                new Topico
                {
                   IdTopico=2,
                   Identificador ="Categoria",
                   TextoMostrar ="Topico 2",
                   Valor = "Valor 2",
                   Orden = 1
                }

            }.AsQueryable();

            // To query our database we need to implement IQueryable 
            var mockSet = new Mock<DbSet<Topico>>();
            mockSet.As<IQueryable<Topico>>().Setup(m => m.Provider).Returns(parametros.Provider);
            mockSet.As<IQueryable<Topico>>().Setup(m => m.Expression).Returns(parametros.Expression);
            mockSet.As<IQueryable<Topico>>().Setup(m => m.ElementType).Returns(parametros.ElementType);
            mockSet.As<IQueryable<Topico>>().Setup(m => m.GetEnumerator()).Returns(parametros.GetEnumerator());

            mockContext = new Mock<TableroControlContext>();
            mockContext.Setup(c => c.Topico).Returns(mockSet.Object);
        }

        [Fact]
        public void ObtenerTopicos_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new TopicoDatos(mockContext.Object);
            var actual = repository.ObtenerTopicos().ToList();

            //// Asset
            Assert.Equal(2, actual.Count);
        }

        [Fact]
        public void ObtenerTopico_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new TopicoDatos(mockContext.Object);
            var actual = repository.ObtenerTopico(1);

            //// Asset
            Assert.Equal("Valor 1", actual.Valor);
        }

        [Fact]
        public void ObtenerUltimoOrden_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new TopicoDatos(mockContext.Object);
            var actual = repository.ObtenerUltimoOrden("Tipo");

            //// Asset
            Assert.Equal(2, actual);
        }

        [Fact]
        public void ObtenerIdentificadoresTopicos_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new TopicoDatos(mockContext.Object);
            var actual = repository.ObtenerIdentificadoresTopicos();

            //// Asset
            Assert.Equal(2, actual.Count());
        }

        [Fact]
        public void InsertarTopico_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new TopicoDatos(mockContext.Object);
            var topico = new Topico
            {
                IdTopico = 3,
                Identificador = "Tipo",
                TextoMostrar = "Topico 2",
                Valor = "Valor 2",
                Orden = 2
            };

            repository.InsertarTopico(topico);

            //// Asset
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void ExisteTopico_Invocacion_DevuelveTrue()
        {
            // Act - fetch books
            var repository = new TopicoDatos(mockContext.Object);
            var actual = repository.ExisteTopico("Tipo", "Valor 1");

            //// Asset
            Assert.True(actual);
        }

        [Fact]
        public void EliminarTopico_Invocacion_DevuelveTrue()
        {
            // Act - fetch books
            var repository = new TopicoDatos(mockContext.Object);
            repository.EliminarTopico(1);

            //// Asset
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void ActualizarTopico_Invocacion_DevuelveTrue()
        {
            // Act - fetch books
            var repository = new TopicoDatos(mockContext.Object);
            var topico = new Topico
            {
                IdTopico = 1,
                Identificador = "Tipo",
                TextoMostrar = "Topico Act",
                Valor = "Valor 2",
                Orden = 2
            };
            repository.ActualizarTopico(topico);

            //// Asset
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
