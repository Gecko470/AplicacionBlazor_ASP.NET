using Microsoft.AspNetCore.Mvc;
using MiPrimeraAplicacionBlazor.Server.Models;
using MiPrimeraAplicacionBlazor.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimeraAplicacionBlazor.Server.Controllers
{
    [ApiController]
    public class AutorController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("api/Autor/ListarAutor")]
        public List<AutorCLS> ListarAutor()
        {
            List<AutorCLS> listaAutor = new List<AutorCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                listaAutor = (from autor in bd.Autor
                              join pais in bd.Pais
                              on autor.Iidpais equals pais.Iidpais
                              join sexo in bd.Sexo
                              on autor.Iidsexo equals sexo.Iidsexo
                              where autor.Bhabilitado == 1
                              select new AutorCLS
                              {
                                  iidAutor = autor.Iidautor,
                                  nombreAutor = autor.Nombre + " " + autor.Appaterno + " " + autor.Apmaterno,
                                  nombrePais = pais.Nombre,
                                  nombreSexo = sexo.Nombre,
                                  descripcion = autor.Descripcion

                              }).ToList();
            }

            return listaAutor;
        }

        [HttpGet]
        [Route("api/Autor/ListarPais")]
        public List<PaisCLS> ListarPais()
        {
            List<PaisCLS> listaPais = new List<PaisCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                listaPais = (from pais in bd.Pais
                             where pais.Bhabilitado == 1
                             select new PaisCLS
                             {
                                 iidPais = pais.Iidpais,
                                 nombrePais = pais.Nombre

                             }).ToList();
            }

            return listaPais;
        }

        [HttpGet]
        [Route("api/Autor/ListarSexo")]
        public List<SexoCLS> ListarSexo()
        {
            List<SexoCLS> listaSexo = new List<SexoCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                listaSexo = (from sexo in bd.Sexo
                             where sexo.Bhabilitado == 1
                             select new SexoCLS
                             {
                                 iidSexo = sexo.Iidsexo,
                                 nombreSexo = sexo.Nombre

                             }).ToList();
            }

            return listaSexo;
        }

        [HttpGet]
        [Route("api/Autor/FiltrarAutor/{data?}")]
        public List<AutorCLS> FiltrarAutor(string data)
        {
            List<AutorCLS> listaAutor = new List<AutorCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                if (data == null || data == "Selecciona")
                {
                    listaAutor = (from autor in bd.Autor
                                  join pais in bd.Pais
                                  on autor.Iidpais equals pais.Iidpais
                                  join sexo in bd.Sexo
                                  on autor.Iidsexo equals sexo.Iidsexo
                                  where autor.Bhabilitado == 1
                                  select new AutorCLS
                                  {
                                      iidAutor = autor.Iidautor,
                                      nombreAutor = autor.Nombre + " " + autor.Appaterno + " " + autor.Apmaterno,
                                      nombrePais = pais.Nombre,
                                      nombreSexo = sexo.Nombre,
                                      descripcion = autor.Descripcion

                                  }).ToList();
                }
                else
                {
                    listaAutor = (from autor in bd.Autor
                                  join pais in bd.Pais
                                  on autor.Iidpais equals pais.Iidpais
                                  join sexo in bd.Sexo
                                  on autor.Iidsexo equals sexo.Iidsexo
                                  where autor.Bhabilitado == 1 && autor.Iidpais == int.Parse(data)
                                  select new AutorCLS
                                  {
                                      iidAutor = autor.Iidautor,
                                      nombreAutor = autor.Nombre + " " + autor.Appaterno + " " + autor.Apmaterno,
                                      nombrePais = pais.Nombre,
                                      nombreSexo = sexo.Nombre,
                                      descripcion = autor.Descripcion

                                  }).ToList();
                }
            }

            return listaAutor;
        }

        [HttpGet]
        [Route("api/Autor/EliminarAutor/{data?}")]
        public int EliminarAutor(string data)
        {
            int respuesta = 0;

            try
            {
                using (var bd = new BDBibliotecaContext())
                {
                    Autor oAutor = new Autor();
                    int idAutor = int.Parse(data);
                    oAutor = bd.Autor.Where(p => p.Iidautor == idAutor).First();
                    oAutor.Bhabilitado = 0;
                    bd.SaveChanges();
                    respuesta = 1;
                }
            }
            catch (Exception ex)
            {
                respuesta = 0;
            }

            return respuesta;
        }

        [HttpPost]
        [Route("api/Autor/GuardarDatos")]
        public int GuardarDatos(AutorCLS oAutorCLS)
        {
            int respuesta;

            try
            {
                using (var bd = new BDBibliotecaContext())
                {
                    if (oAutorCLS.iidAutor == 0)
                    {
                        Autor oAutor = new Autor();

                        oAutor.Nombre = oAutorCLS.nombre;
                        oAutor.Appaterno = oAutorCLS.aPaterno;
                        oAutor.Apmaterno = oAutorCLS.aMaterno;
                        oAutor.Descripcion = oAutorCLS.descripcion;
                        oAutor.Iidsexo = int.Parse(oAutorCLS.iidSexo);
                        oAutor.Iidpais = int.Parse(oAutorCLS.iidPais);
                        oAutor.Bhabilitado = 1;

                        bd.Autor.Add(oAutor);
                        bd.SaveChanges();

                        respuesta = 1;
                    }
                    else
                    {
                        Autor oAutor = new Autor();

                        oAutor = bd.Autor.Where(p => p.Iidautor == oAutorCLS.iidAutor).First();

                        oAutor.Nombre = oAutorCLS.nombre;
                        oAutor.Appaterno = oAutorCLS.aPaterno;
                        oAutor.Apmaterno = oAutorCLS.aMaterno;
                        oAutor.Descripcion = oAutorCLS.descripcion;
                        oAutor.Iidsexo = int.Parse(oAutorCLS.iidSexo);
                        oAutor.Iidpais = int.Parse(oAutorCLS.iidPais);
                        oAutor.Bhabilitado = 1;

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
        [Route("api/Autor/RecuperarAutor/{iidAutor}")]
        public AutorCLS RecuperarAutor(int iidAutor)
        {
            AutorCLS oAutorCLS = new AutorCLS();

            using (var bd = new BDBibliotecaContext())
            {
                oAutorCLS = (from autor in bd.Autor
                             where autor.Iidautor == iidAutor
                             select new AutorCLS
                             {
                                 iidAutor = autor.Iidautor,
                                 nombre = autor.Nombre,
                                 aPaterno = autor.Apmaterno,
                                 aMaterno = autor.Appaterno,
                                 descripcion = autor.Descripcion,
                                 iidSexo = autor.Iidsexo.ToString(),
                                 iidPais = autor.Iidpais.ToString()

                             }).First();
            }

            return oAutorCLS;
        }
    }
}
