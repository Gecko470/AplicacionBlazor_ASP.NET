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
    public class TipoLibroController : Controller
    {
        //COMENTO ESTO PORQUE NO LO UTILIZO

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("api/TipoLibro/Get")]
        public List<TipoLibroCLS> Get()
        {
            List<TipoLibroCLS> lista = new List<TipoLibroCLS>();

            using (BDBibliotecaContext bd = new BDBibliotecaContext())
            {
                lista = (from tipoLibro in bd.TipoLibro
                         where tipoLibro.Bhabilitado == 1
                         select new TipoLibroCLS
                         {
                             IIDTIPOLIBRO = tipoLibro.Iidtipolibro,
                             NOMBRETIPOLIBRO = tipoLibro.Nombretipolibro,
                             DESCRIPCIONTIPOLIBRO = tipoLibro.Descripcion

                         }).ToList();

            }

            return lista;
        }

        [HttpGet]
        [Route("api/TipoLibro/Filtrar/{data?}")]
        public List<TipoLibroCLS> Filtrar(string data)
        {
            List<TipoLibroCLS> lista = new List<TipoLibroCLS>();

            using (BDBibliotecaContext bd = new BDBibliotecaContext())
            {
                if (data == null)
                {
                    lista = (from tipoLibro in bd.TipoLibro
                             where tipoLibro.Bhabilitado == 1
                             select new TipoLibroCLS
                             {
                                 IIDTIPOLIBRO = tipoLibro.Iidtipolibro,
                                 NOMBRETIPOLIBRO = tipoLibro.Nombretipolibro,
                                 DESCRIPCIONTIPOLIBRO = tipoLibro.Descripcion

                             }).ToList();
                }
                else
                {
                    lista = (from tipoLibro in bd.TipoLibro
                             where tipoLibro.Bhabilitado == 1 && tipoLibro.Nombretipolibro.Contains(data)
                             select new TipoLibroCLS
                             {
                                 IIDTIPOLIBRO = tipoLibro.Iidtipolibro,
                                 NOMBRETIPOLIBRO = tipoLibro.Nombretipolibro,
                                 DESCRIPCIONTIPOLIBRO = tipoLibro.Descripcion

                             }).ToList();
                }

            }

            return lista;
        }

        [HttpGet]
        [Route("api/TipoLibro/EliminarTipoLibro/{data?}")]
        public int EliminarTipoLibro(string data)
        {
            int respuesta = 0;

            try
            {
                using (var bd = new BDBibliotecaContext())
                {
                    TipoLibro oTipoLibro = new TipoLibro();
                    int idTipoLibro = int.Parse(data);
                    oTipoLibro = bd.TipoLibro.Where(p => p.Iidtipolibro == idTipoLibro).First();
                    oTipoLibro.Bhabilitado = 0;
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
        [Route("api/TipoLibro/Guardar")]
        public int Guardar([FromBody] TipoLibroCLS oTipoLibroCLS)
        {
            int respuesta = 0;

            try
            {
                using (var bd = new BDBibliotecaContext())
                {
                    if (oTipoLibroCLS.IIDTIPOLIBRO == 0)//Si el ID que nos viene del formulario es 0 es porque es un registro nuevo
                    {
                        TipoLibro oTipoLibro = new TipoLibro();

                        oTipoLibro.Nombretipolibro = oTipoLibroCLS.NOMBRETIPOLIBRO;
                        oTipoLibro.Descripcion = oTipoLibroCLS.DESCRIPCIONTIPOLIBRO;
                        oTipoLibro.Bhabilitado = 1;
                        bd.TipoLibro.Add(oTipoLibro);
                        bd.SaveChanges();

                        respuesta = 1;
                    }
                    else
                    {
                        TipoLibro oTipoLibro = bd.TipoLibro.Where(p => p.Iidtipolibro == oTipoLibroCLS.IIDTIPOLIBRO).First();

                        oTipoLibro.Nombretipolibro = oTipoLibroCLS.NOMBRETIPOLIBRO;
                        oTipoLibro.Descripcion = oTipoLibroCLS.DESCRIPCIONTIPOLIBRO;

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
        [Route("api/TipoLibro/obtenerTipoLibro/{iidTipoLibro}")]
        public TipoLibroCLS obtenerTipoLibro(int iidTipoLibro)
        {
            TipoLibroCLS oTipoLibroCLS = new TipoLibroCLS();

            using (var bd = new BDBibliotecaContext())
            {
                oTipoLibroCLS = (from tipoLibro in bd.TipoLibro
                                 where tipoLibro.Iidtipolibro == iidTipoLibro
                                 select new TipoLibroCLS
                                 {
                                     IIDTIPOLIBRO = tipoLibro.Iidtipolibro,
                                     NOMBRETIPOLIBRO = tipoLibro.Nombretipolibro,
                                     DESCRIPCIONTIPOLIBRO = tipoLibro.Descripcion

                                 }).First();

                return oTipoLibroCLS;
            }
        }
    }
}

