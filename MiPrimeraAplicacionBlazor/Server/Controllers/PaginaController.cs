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
    public class PaginaController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("api/Pagina/ListarPagina")]
        public List<PaginaCLS> ListarPagina()
        {
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                listaPagina = (from pagina in bd.Pagina
                               where pagina.Bhabilitado == 1
                               select new PaginaCLS
                               {
                                   iidPagina = pagina.Iidpagina,
                                   mensaje = pagina.Mensaje,
                                   accion = pagina.Accion,
                                   nombreVisible = pagina.Bvisible == 1 ? "Si" : "No"

                               }).ToList();
            }

            return listaPagina;
        }

        [HttpGet]
        [Route("api/Pagina/EliminarPagina/{iidPagina}")]
        public int EliminarPagina(string iidPagina)
        {
            int respuesta = 0;

            using (var bd = new BDBibliotecaContext())
            {
                try
                {
                    Pagina oPagina = bd.Pagina.Where(p => p.Iidpagina == int.Parse(iidPagina)).First();
                    oPagina.Bhabilitado = 0;

                    bd.SaveChanges();
                    respuesta = 1;
                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }

            return respuesta;
        }

        [HttpGet]
        [Route("api/Pagina/FiltrarPagina/{valor?}")]
        public List<PaginaCLS> FiltrarPagina(string valor)
        {
            List<PaginaCLS> listaPagina = new List<PaginaCLS>();

            using(var bd = new BDBibliotecaContext())
            {
                if(valor == null || valor == "")
                {
                    listaPagina = (from pagina in bd.Pagina
                                   where pagina.Bhabilitado == 1
                                   select new PaginaCLS
                                   {
                                       iidPagina = pagina.Iidpagina,
                                       mensaje = pagina.Mensaje,
                                       accion = pagina.Accion,
                                       nombreVisible = pagina.Bvisible == 1 ? "Si" : "No"

                                   }).ToList();
                }
                else
                {
                listaPagina = (from pagina in bd.Pagina
                               where pagina.Bhabilitado == 1 && pagina.Mensaje.Contains(valor)
                               select new PaginaCLS
                               {
                                   iidPagina = pagina.Iidpagina,
                                   mensaje = pagina.Mensaje,
                                   accion = pagina.Accion,
                                   nombreVisible = pagina.Bvisible == 1 ? "Si" : "No"

                               }).ToList();
                }
            }

            return listaPagina;
        }

        [HttpPost]
        [Route("api/Pagina/Guardar")]
        public int Guardar([FromBody] PaginaCLS oPaginaCLS)
        {
            int respuesta = 0;

            try
            {
                using(var bd = new BDBibliotecaContext())
                {
                    if(oPaginaCLS.iidPagina == 0)
                    {
                        Pagina oPagina = new Pagina();

                        oPagina.Mensaje = oPaginaCLS.mensaje;
                        oPagina.Accion = oPaginaCLS.accion;
                        oPagina.Bvisible = Convert.ToInt32(oPaginaCLS.bvisible);
                        oPagina.Bhabilitado = 1;

                        bd.Pagina.Add(oPagina);
                        bd.SaveChanges();

                        respuesta = 1;
                    }
                    else
                    {
                        Pagina oPagina = bd.Pagina.Where(p => p.Iidpagina == oPaginaCLS.iidPagina).First();

                        oPagina.Mensaje = oPaginaCLS.mensaje;
                        oPagina.Accion = oPaginaCLS.accion;
                        oPagina.Bvisible = Convert.ToInt32(oPaginaCLS.bvisible);

                        bd.SaveChanges();

                        respuesta = 1;
                    }
                }
            }
            catch(Exception ex)
            {
                respuesta = 0;
            }

            return respuesta;
        }

        [HttpGet]
        [Route("api/Pagina/RecuperarDatos/{iidPagina}")]
        public PaginaCLS RecuperarDatos(string iidPagina)
        {
            PaginaCLS oPaginaCLS = new PaginaCLS();

            using(var bd = new BDBibliotecaContext())
            {
                oPaginaCLS = (from pagina in bd.Pagina
                              where pagina.Iidpagina == int.Parse(iidPagina)
                              select new PaginaCLS
                              {
                                  iidPagina = pagina.Iidpagina,
                                  mensaje = pagina.Mensaje,
                                  accion = pagina.Accion,
                                  bvisible = Convert.ToBoolean(pagina.Bvisible)

                              }).First();

                return oPaginaCLS;
            }
        }

        [HttpGet]
        [Route("api/Pagina/RecuperarIdPag/{nombre}")]
        public int RecuperarIdPag(string nombre)
        {
            int idPagina = 0;

            try
            {
                using(var bd = new BDBibliotecaContext())
                {
                    idPagina = bd.Pagina.Where(p => p.Accion == "/" + nombre).First().Iidpagina;
                }
            }
            catch(Exception ex)
            {
                idPagina = 0;
            }

            return idPagina;
        }
    }
}
