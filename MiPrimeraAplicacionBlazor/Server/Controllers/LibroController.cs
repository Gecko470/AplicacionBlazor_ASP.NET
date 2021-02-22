using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiPrimeraAplicacionBlazor.Shared;
using MiPrimeraAplicacionBlazor.Server.Models;

namespace MiPrimeraAplicacionBlazor.Server.Controllers
{
    [ApiController]
    public class LibroController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("api/Libro/ListarLibro")]
        public List<LibroCLS> ListarLibro()
        {
            List<LibroCLS> listaLibro = new List<LibroCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                listaLibro = (from libro in bd.Libro
                              join autor in bd.Autor
                              on libro.Iidautor equals autor.Iidautor
                              where libro.Bhabilitado == 1
                              select new LibroCLS
                              {
                                  iidLibro = libro.Iidlibro,
                                  titulo = libro.Titulo,
                                  nombreAutor = autor.Nombre + " " + autor.Appaterno + " " + autor.Apmaterno,
                                  numPag = (int)libro.Numpaginas,
                                  stock = (int)libro.Stock

                              }).ToList();
            }

            return listaLibro;
        }

        [HttpPost]
        [Route("api/Libro/GuardarLibro")]
        public int GuardarLibro([FromBody] LibroCLS oLibroCLS)
        {
            int respuesta = 0;

            try
            {
                using (var bd = new BDBibliotecaContext())
                {

                    if (oLibroCLS.iidLibro == 0)
                    {
                        Libro oLibro = new Libro();

                        oLibro.Titulo = oLibroCLS.titulo;
                        oLibro.Resumen = oLibroCLS.resumen;
                        oLibro.Numpaginas = oLibroCLS.numPag;
                        oLibro.Stock = oLibroCLS.stock;
                        oLibro.Iidautor = int.Parse(oLibroCLS.iidAutor);
                        oLibro.Fotocaratula = oLibroCLS.fotoCaratula;
                        oLibro.Libropdf = oLibroCLS.libroPDF;
                        oLibro.Bhabilitado = 1;

                        bd.Libro.Add(oLibro);
                        bd.SaveChanges();

                        respuesta = 1;
                    }
                    else
                    {

                        Libro oLibro = bd.Libro.Where(p => p.Iidlibro == oLibroCLS.iidLibro).First();

                        oLibro.Titulo = oLibroCLS.titulo;
                        oLibro.Resumen = oLibroCLS.resumen;
                        oLibro.Numpaginas = oLibroCLS.numPag;
                        oLibro.Stock = oLibroCLS.stock;
                        oLibro.Iidautor = int.Parse(oLibroCLS.iidAutor);
                        oLibro.Fotocaratula = oLibroCLS.fotoCaratula;
                        oLibro.Libropdf = oLibroCLS.libroPDF;

                        bd.SaveChanges();

                        respuesta = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                respuesta = 0;
            }

            return respuesta;
        }

        [HttpGet]
        [Route("api/Libro/RecuperarLibro/{iidLibro}")]
        public LibroCLS RecuperarLibro(int iidLibro)
        {
            LibroCLS oLibroCLS = new LibroCLS();

            using (var bd = new BDBibliotecaContext())
            {
                Libro oLibro = bd.Libro.Where(p => p.Iidlibro == iidLibro).First();

                oLibroCLS.iidLibro = oLibro.Iidlibro;
                oLibroCLS.titulo = oLibro.Titulo;
                oLibroCLS.resumen = oLibro.Resumen;
                oLibroCLS.numPag = (int)oLibro.Numpaginas;
                oLibroCLS.stock = (int)oLibro.Stock;
                oLibroCLS.iidAutor = oLibro.Iidautor.ToString();
                oLibroCLS.libroPDF = oLibro.Libropdf;
                oLibroCLS.fotoCaratula = oLibro.Fotocaratula;
            }

            return oLibroCLS;
        }

        [HttpGet]
        [Route("api/Libro/EliminarLibro/{iidLibro}")]
        public int EliminarLibro(int iidLibro)
        {
            int respuesta = 0;
            try
            {
                using(var bd = new BDBibliotecaContext())
                {
                    Libro oLibro = bd.Libro.Where(p => p.Iidlibro == iidLibro).First();
                    oLibro.Bhabilitado = 0;


                    bd.SaveChanges();
                    respuesta = 1;
                }
            }
            catch(Exception ex)
            {
                respuesta = 0;
            }

            return respuesta;
        }

        [HttpGet]
        [Route("api/Libro/FiltrarLibro/{valor?}")]
        public List<LibroCLS> FiltrarLibro(string valor)
        {
            List<LibroCLS> listaLibro = new List<LibroCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                if(valor == null || valor == "Selecciona")
                {
                    listaLibro = (from libro in bd.Libro
                                  join autor in bd.Autor
                                  on libro.Iidautor equals autor.Iidautor
                                  where libro.Bhabilitado == 1
                                  select new LibroCLS
                                  {
                                      iidLibro = libro.Iidlibro,
                                      titulo = libro.Titulo,
                                      nombreAutor = autor.Nombre + " " + autor.Appaterno + " " + autor.Apmaterno,
                                      numPag = (int)libro.Numpaginas,
                                      stock = (int)libro.Stock

                                  }).ToList();
                }
                else
                {
                    listaLibro = (from libro in bd.Libro
                                  join autor in bd.Autor
                                  on libro.Iidautor equals autor.Iidautor
                                  where libro.Bhabilitado == 1 && libro.Iidautor == int.Parse(valor)
                                  select new LibroCLS
                                  {
                                      iidLibro = libro.Iidlibro,
                                      titulo = libro.Titulo,
                                      nombreAutor = autor.Nombre + " " + autor.Appaterno + " " + autor.Apmaterno,
                                      numPag = (int)libro.Numpaginas,
                                      stock = (int)libro.Stock

                                  }).ToList();
                }
            }

            return listaLibro;
        }

        [HttpGet]
        [Route("api/Libro/ListarLibroCompleto")]
        public List<LibroCLS> ListarLibroCompleto()
        {
            List<LibroCLS> listaLibro = new List<LibroCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                listaLibro = (from libro in bd.Libro
                              join autor in bd.Autor
                              on libro.Iidautor equals autor.Iidautor
                              where libro.Bhabilitado == 1
                              select new LibroCLS
                              {
                                  iidLibro = libro.Iidlibro,
                                  titulo = libro.Titulo,
                                  nombreAutor = autor.Nombre + " " + autor.Appaterno + " " + autor.Apmaterno,
                                  fotoCaratula = libro.Fotocaratula,
                                  resumen = libro.Resumen.Substring(0, 200),
                                  numPag = (int)libro.Numpaginas,
                                  stock = (int)libro.Stock

                              }).ToList();
            }

            return listaLibro;
        }

        [HttpGet]
        [Route("api/Libro/FiltrarLibroNombre/{valor?}")]
        public List<LibroCLS> FiltrarLibroNombre(string valor)
        {
            List<LibroCLS> listaLibro = new List<LibroCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                if (valor == null)
                {
                    listaLibro = (from libro in bd.Libro
                                  join autor in bd.Autor
                                  on libro.Iidautor equals autor.Iidautor
                                  where libro.Bhabilitado == 1
                                  select new LibroCLS
                                  {
                                      iidLibro = libro.Iidlibro,
                                      titulo = libro.Titulo,
                                      nombreAutor = autor.Nombre + " " + autor.Appaterno + " " + autor.Apmaterno,
                                      numPag = (int)libro.Numpaginas,
                                      resumen = libro.Resumen.Substring(0,200),
                                      fotoCaratula = libro.Fotocaratula,
                                      stock = (int)libro.Stock

                                  }).ToList();
                }
                else
                {
                    listaLibro = (from libro in bd.Libro
                                  join autor in bd.Autor
                                  on libro.Iidautor equals autor.Iidautor
                                  where libro.Bhabilitado == 1 && libro.Titulo.Contains(valor)
                                  select new LibroCLS
                                  {
                                      iidLibro = libro.Iidlibro,
                                      titulo = libro.Titulo,
                                      nombreAutor = autor.Nombre + " " + autor.Appaterno + " " + autor.Apmaterno,
                                      numPag = (int)libro.Numpaginas,
                                      resumen = libro.Resumen.Substring(0, 200),
                                      fotoCaratula = libro.Fotocaratula,
                                      stock = (int)libro.Stock

                                  }).ToList();
                }
            }

            return listaLibro;
        }
    }
}
