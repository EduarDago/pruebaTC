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
    public class ComunDatosTest
    {
        private readonly Mock<TableroControlContext> mockContext;

        public ComunDatosTest()
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
        public void ValidarExistenciaSp_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ComunDatos(mockContext.Object);
            var actual = repository.ValidarExistenciaSp("","");

            ////// Asset
            Assert.False(actual);
        }

        [Fact]
        public void ValidarExistenciaTabla_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ComunDatos(mockContext.Object);
            var actual = repository.ValidarExistenciaTabla("", "");

            ////// Asset
            Assert.False(actual);
        }

        [Fact]
        public void ObtenerParametro_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ComunDatos(mockContext.Object);
            var actual = repository.ObtenerParametro("FechaProceso");

            ////// Asset
            Assert.Null(actual);
        }

        [Fact]
        public void ObtenerTopicos_Invocacion_DevuelveListado()
        {
            // Act - fetch books
            var repository = new ComunDatos(mockContext.Object);
            var actual = repository.ObtenerTopicos("Tipo");

            ////// Asset
            Assert.Empty(actual);
        }
    }
}
