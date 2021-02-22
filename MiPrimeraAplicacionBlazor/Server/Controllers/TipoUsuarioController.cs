using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiPrimeraAplicacionBlazor.Shared;
using MiPrimeraAplicacionBlazor.Server.Models;
using System.Transactions;

namespace MiPrimeraAplicacionBlazor.Server.Controllers
{
    [ApiController]
    public class TipoUsuarioController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("api/TipoUsuario/ListarTipoUsuario")]
        public List<TipoUsuarioCLS> ListarTipoUsuario()
        {
            List<TipoUsuarioCLS> lista = new List<TipoUsuarioCLS>();
            using (var bd = new BDBibliotecaContext())
            {
                lista = (from tipoUsuario in bd.TipoUsuario
                         where tipoUsuario.Bhabilitado == 1
                         select new TipoUsuarioCLS
                         {
                             iidTipoUsuario = tipoUsuario.Iidtipousuario,
                             nombre = tipoUsuario.Nombre,
                             descripcion = tipoUsuario.Descripcion

                         }).ToList();
            }

            return lista;
        }

        [HttpGet]
        [Route("api/TipoUsuario/Eliminar/{iidTipoUsuario}")]
        public int Eliminar(int iidTipoUsuario)
        {
            int respuesta = 0;

            try
            {
                using (var bd = new BDBibliotecaContext())
                {
                    TipoUsuario oTipoUsuario = bd.TipoUsuario.Where(p => p.Iidtipousuario == iidTipoUsuario).First();
                    oTipoUsuario.Bhabilitado = 0;
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

        [HttpGet]
        [Route("api/TipoUsuario/Filtrar/{valor?}")]
        public List<TipoUsuarioCLS> Filtrar(string valor)
        {
            List<TipoUsuarioCLS> lista = new List<TipoUsuarioCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                if (valor == null || valor == "")
                {
                    lista = (from tipoUsuario in bd.TipoUsuario
                             where tipoUsuario.Bhabilitado == 1
                             select new TipoUsuarioCLS
                             {
                                 iidTipoUsuario = tipoUsuario.Iidtipousuario,
                                 nombre = tipoUsuario.Nombre,
                                 descripcion = tipoUsuario.Descripcion

                             }).ToList();
                }
                else
                {
                    lista = (from tipoUsuario in bd.TipoUsuario
                             where tipoUsuario.Bhabilitado == 1 && tipoUsuario.Nombre.Contains(valor)
                             select new TipoUsuarioCLS
                             {
                                 iidTipoUsuario = tipoUsuario.Iidtipousuario,
                                 nombre = tipoUsuario.Nombre,
                                 descripcion = tipoUsuario.Descripcion

                             }).ToList();
                }
            }

            return lista;
        }

        [HttpPost]
        [Route("api/TipoUsuario/Guardar")]
        public int Guardar([FromBody] TipoUsuarioCLS oTipoUsuarioCLS)
        {
            int respuesta = 0;
            try
            {
                using (var bd = new BDBibliotecaContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        if (oTipoUsuarioCLS.iidTipoUsuario == 0)
                        {
                            TipoUsuario oTipoUsuario = new TipoUsuario();

                            oTipoUsuario.Nombre = oTipoUsuarioCLS.nombre;
                            oTipoUsuario.Descripcion = oTipoUsuarioCLS.descripcion;
                            oTipoUsuario.Bhabilitado = 1;

                            bd.TipoUsuario.Add(oTipoUsuario);
                            bd.SaveChanges();

                            int iidTipoUsuario = oTipoUsuario.Iidtipousuario;

                            if (oTipoUsuarioCLS.arrayId.Count > 0)
                            {
                                foreach (int num in oTipoUsuarioCLS.arrayId)
                                {
                                    PaginaTipoUsuario oPaginaTipoUsuario = new PaginaTipoUsuario();

                                    oPaginaTipoUsuario.Iidpagina = num;
                                    oPaginaTipoUsuario.Iidtipousuario = iidTipoUsuario;
                                    oPaginaTipoUsuario.Bhabilitado = 1;

                                    bd.PaginaTipoUsuario.Add(oPaginaTipoUsuario);
                                    bd.SaveChanges();
                                }
                            }

                            transaccion.Complete();
                            respuesta = 1;
                        }
                        else
                        {
                            TipoUsuario oTipoUsuario = bd.TipoUsuario.Where(p => p.Iidtipousuario == oTipoUsuarioCLS.iidTipoUsuario).First();

                            oTipoUsuario.Nombre = oTipoUsuarioCLS.nombre;
                            oTipoUsuario.Descripcion = oTipoUsuarioCLS.descripcion;
                            bd.SaveChanges();

                            //Primero deshabilitamos bhabilitado=0 todas las páginas asociadas al tipo de usuario
                            List<PaginaTipoUsuario> listaPagUsuario = (from paginaTipoUsuario in bd.PaginaTipoUsuario
                                                                       where paginaTipoUsuario.Iidtipousuario == oTipoUsuarioCLS.iidTipoUsuario
                                                                       select paginaTipoUsuario).ToList();
                            if(listaPagUsuario.Count>0 && listaPagUsuario != null)
                            {
                                foreach(PaginaTipoUsuario item in listaPagUsuario)
                                {
                                    item.Bhabilitado = 0;
                                    bd.SaveChanges();
                                }
                            }
                            
                            //Aquí habilitamos las páginas que nos vengan en oTipoUsuarioCLS.arrayID
                            if (oTipoUsuarioCLS.arrayId.Count > 0)
                            {
                                foreach (int num in oTipoUsuarioCLS.arrayId)
                                {
                                    int numVeces = bd.PaginaTipoUsuario.Where(p => p.Iidtipousuario == oTipoUsuarioCLS.iidTipoUsuario && p.Iidpagina == num).Count();
                                    if(numVeces == 0)
                                    {
                                        PaginaTipoUsuario oPaginaTipoUsuario = new PaginaTipoUsuario();

                                        oPaginaTipoUsuario.Iidtipousuario = oTipoUsuarioCLS.iidTipoUsuario;
                                        oPaginaTipoUsuario.Iidpagina = num;
                                        oPaginaTipoUsuario.Bhabilitado = 1;

                                        bd.PaginaTipoUsuario.Add(oPaginaTipoUsuario);
                                        bd.SaveChanges();
                                        
                                    }
                                    else
                                    {
                                        PaginaTipoUsuario oPaginaTipoUsuario = bd.PaginaTipoUsuario.
                                            Where(p => p.Iidtipousuario == oTipoUsuarioCLS.iidTipoUsuario && p.Iidpagina == num).First();
                                        oPaginaTipoUsuario.Bhabilitado = 1;

                                        bd.SaveChanges();
                                    }
                                }
                            }

                            transaccion.Complete();
                            respuesta = 1;
                        }
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
        [Route("api/TipoUsuario/RecuperarDatos/{iidTipoUsuario}")]
        public TipoUsuarioCLS RecuperarDatos(int iidTipoUsuario)
        {
            TipoUsuarioCLS oTipoUsuarioCLS = new TipoUsuarioCLS();

            using (var bd = new BDBibliotecaContext())
            {
                

                oTipoUsuarioCLS = bd.TipoUsuario.Where(p => p.Iidtipousuario == iidTipoUsuario).Select(p => new TipoUsuarioCLS
                {
                    iidTipoUsuario = p.Iidtipousuario,
                    nombre = p.Nombre,
                    descripcion = p.Descripcion

                }).First();

                List<int> listaIDS = bd.PaginaTipoUsuario.Where(p => p.Iidtipousuario == iidTipoUsuario && p.Bhabilitado == 1)
                    .Select(p => p.Iidpagina).Cast<int>().ToList();

                oTipoUsuarioCLS.arrayId = listaIDS;
            }

            return oTipoUsuarioCLS;
        }

    }
}
