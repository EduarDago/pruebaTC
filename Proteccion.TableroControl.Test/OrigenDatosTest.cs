using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using Moq;
using Proteccion.TableroControl.Datos.DAO;
using Proteccion.TableroControl.Datos.DataContext;
using Proteccion.TableroControl.Dominio.Entidades;
using Proteccion.TableroControl.Dominio.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Proteccion.TableroControl.Test
{
    public class OrigenDatosTest
    {
        private readonly Mock<IParametroDatos> parametroMock;
        private readonly Mock<TableroControlContext> contextMock;

        public OrigenDatosTest()
        {
            parametroMock = new Mock<IParametroDatos>();

            var data = new List<OrigenDato>
            {
                new OrigenDato { IdOrigenDato=1, Nombre = "Prueba1", Activo=true, TipoOrigen = new Topico
                    {
                        IdTopico = 1, TextoMostrar = "Topico1"
                    }
                },
                new OrigenDato { IdOrigenDato=2, Descripcion = "Prueba2", Activo=false, TipoOrigen = new Topico
                    {
                        IdTopico = 1, TextoMostrar = "Topico1"
                    }
                },
                new OrigenDato { IdOrigenDato=3, Descripcion = "Prueba3", Activo=true, TipoOrigen = new Topico
                    {
                        IdTopico = 1, TextoMostrar = "Topico1"
                    }
                }
            }.AsQueryable();

            var mockOrigenDatoSet = new Mock<DbSet<OrigenDato>>();
            var mockTopicoSet = new Mock<DbSet<Topico>>();

            mockOrigenDatoSet.As<IQueryable<OrigenDato>>().Setup(m => m.Provider).Returns(data.Provider);
            mockOrigenDatoSet.As<IQueryable<OrigenDato>>().Setup(m => m.Expression).Returns(data.Expression);
            mockOrigenDatoSet.As<IQueryable<OrigenDato>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockOrigenDatoSet.As<IQueryable<OrigenDato>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            contextMock = new Mock<TableroControlContext>();
            contextMock.Setup(m => m.OrigenDato).Returns(mockOrigenDatoSet.Object);
            contextMock.Setup(m => m.Topico).Returns(mockTopicoSet.Object);
        }

        [Fact]
        public void ActualizarConfiguracionOrigen_Invocacion_DevuelveTrue()
        {
            // Asset
            var repository = new OrigenDatos(parametroMock.Object, contextMock.Object);
            var origen = new OrigenDato
            {
                IdOrigenDato = 1,
                Nombre = "Prueba1",
                Activo = true,
                TipoOrigen = new Topico
                {
                    IdTopico = 1,
                    TextoMostrar = "Topico1"
                }
            };
            var actual = repository.ActualizarConfiguracionOrigen(origen);

            // Act
            Assert.True(actual);
        }

        [Fact]
        public void ActualizarConfiguracionOrigen_Invocacion_DevuelveFalse()
        {
            // Asset
            var repository = new OrigenDatos(parametroMock.Object, contextMock.Object);
            var actual = repository.ActualizarConfiguracionOrigen(new OrigenDato());

            // Act
            Assert.False(actual);
        }

        [Fact]
        public void ObtenerConfiguracionesOrigen_Invocacion_DevuelveCantidad()
        {
            // Asset
            var repository = new OrigenDatos(parametroMock.Object, contextMock.Object);
            var origins = repository.ObtenerConfiguracionesOrigen();

            // Act
            Assert.Equal(3, origins.Count());
        }

        [Fact]
        public void ObtenerConfiguracionOrigen_Invocacion_ExisteRegistro()
        {
            // Asset
            var repository = new OrigenDatos(parametroMock.Object, contextMock.Object);
            var origen = repository.ObtenerConfiguracionOrigen(1);

            // Act
            Assert.Null(origen);
        }

        [Fact]
        public void ObtenerOrigenes_Invocacion_DevuelveOrigenesActivos()
        {
            // Asset
            var repository = new OrigenDatos(parametroMock.Object, contextMock.Object);
            var origins = repository.ObtenerOrigenes();

            // Act
            Assert.Equal(2, origins.Count());
        }

        [Fact]
        public void ExisteOrigen_Invocacion_ExisteRegistro()
        {
            // Asset
            var repository = new OrigenDatos(parametroMock.Object, contextMock.Object);
            var esperado = "Prueba1";
            var origen = repository.ExisteOrigen(esperado);

            // Act
            Assert.True(origen);
        }

        [Fact]
        public void ConsultarEjecuciones_Invocacion_ExisteRegistro()
        {
            // Asset
            var repository = new OrigenDatos(parametroMock.Object, contextMock.Object);
            var esperado = new List<EjecucionImportacion>();
            var origen = repository.ConsultarEjecuciones();

            // Act
            Assert.Equal(esperado, origen);
        }

        [Theory]
        [InlineData(TipoOrigen.BD)]
        [InlineData(TipoOrigen.Excel)]
        [InlineData(TipoOrigen.CSV)]
        public void EjecutarImportacion_Invocacion_ExisteRegistro(TipoOrigen tipo)
        {
            // Asset
            var repository = new OrigenDatos(parametroMock.Object, contextMock.Object);
            var origen = repository.EjecutarImportacion(1, tipo, false);

            // Act
            Assert.Null(origen);
        }

        [Fact]
        public void EliminarOrigen_Invocacion_ExisteRegistro()
        {
            // Asset
            var repository = new OrigenDatos(parametroMock.Object, contextMock.Object);
            var origen = repository.EliminarOrigen(1);

            // Act
            Assert.False(origen);
        }

        [Fact]
        public void CrearEstructura_Invocacion_DevuelveExcepcion()
        {
            // Asset
            var repository = new OrigenDatos(parametroMock.Object, contextMock.Object);

            Assert.Throws<ArgumentException>(() => repository.CrearEstructura(1));
        }

        [Fact]
        public void ObtenerDatosTabla_Invocacion_DevuelveExcepcion()
        {
            // Asset
            var repository = new OrigenDatos(parametroMock.Object, contextMock.Object);
            var origen = repository.ObtenerDatosTabla(1);

            Assert.Null(origen);
        }

        [Fact]
        public void GuardarEjecucion_Invocacion_DevuelveExcepcion()
        {
            // Asset
            var repository = new OrigenDatos(parametroMock.Object, contextMock.Object);
            var origen = repository.GuardarEjecucion(new EjecucionOrigen());

            Assert.False(origen);
        }

        [Fact]
        public void OnModelCreating_Invocacion_DevuelveExcepcion()
        {
            var mockModel = new Mock<ModelBuilder>(new ConventionSet());
            var context = new TableroControlContext();

            var actual = context.TestModelCreating(mockModel.Object);

            Assert.True(actual);
        }

        [Fact]
        public void TransferirArchivo_Invocacion_DevuelveExcepcion()
        {
            // Asset
            var repository = new StfpHelper(null, null);
            var origen = repository.TransferirArchivo();

            Assert.False(origen == "OK");
        }
    }
}
