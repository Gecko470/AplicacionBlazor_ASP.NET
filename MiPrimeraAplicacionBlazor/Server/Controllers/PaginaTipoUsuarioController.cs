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
    public class PaginaTipoUsuarioController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("api/PaginaTipoUsuario/Listar")]
        public List<PaginaTipoUsuarioCLS> Listar()
        {
            List<PaginaTipoUsuarioCLS> lista = new List<PaginaTipoUsuarioCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                lista = (from pagTipoUsuario in bd.PaginaTipoUsuario
                         join pagina in bd.Pagina
                         on pagTipoUsuario.Iidpagina equals pagina.Iidpagina
                         join tipoUsuario in bd.TipoUsuario
                         on pagTipoUsuario.Iidtipousuario equals tipoUsuario.Iidtipousuario
                         where pagTipoUsuario.Bhabilitado == 1
                         select new PaginaTipoUsuarioCLS
                         {
                             iidPaginaTipoUsuario = pagTipoUsuario.Iidpaginatipousuario,
                             nombrePagina = pagina.Mensaje,
                             nombreTipoUsuario = tipoUsuario.Nombre

                         }).ToList();
            }

            return lista;
        }

        [HttpGet]
        [Route("api/PaginaTipoUsuario/Filtrar/{iidTipoUsuario?}")]
        public List<PaginaTipoUsuarioCLS> Filtrar(string iidTipoUsuario)
        {
            List<PaginaTipoUsuarioCLS> lista = new List<PaginaTipoUsuarioCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                if (iidTipoUsuario == null || iidTipoUsuario == "Selecciona")
                {
                    lista = (from pagTipoUsuario in bd.PaginaTipoUsuario
                             join pagina in bd.Pagina
                             on pagTipoUsuario.Iidpagina equals pagina.Iidpagina
                             join tipoUsuario in bd.TipoUsuario
                             on pagTipoUsuario.Iidtipousuario equals tipoUsuario.Iidtipousuario
                             where pagTipoUsuario.Bhabilitado == 1
                             select new PaginaTipoUsuarioCLS
                             {
                                 iidPaginaTipoUsuario = pagTipoUsuario.Iidpaginatipousuario,
                                 nombrePagina = pagina.Mensaje,
                                 nombreTipoUsuario = tipoUsuario.Nombre

                             }).ToList();
                }
                else
                {
                    lista = (from pagTipoUsuario in bd.PaginaTipoUsuario
                             join pagina in bd.Pagina
                             on pagTipoUsuario.Iidpagina equals pagina.Iidpagina
                             join tipoUsuario in bd.TipoUsuario
                             on pagTipoUsuario.Iidtipousuario equals tipoUsuario.Iidtipousuario
                             where pagTipoUsuario.Bhabilitado == 1 && pagTipoUsuario.Iidtipousuario == int.Parse(iidTipoUsuario)
                             select new PaginaTipoUsuarioCLS
                             {
                                 iidPaginaTipoUsuario = pagTipoUsuario.Iidpaginatipousuario,
                                 nombrePagina = pagina.Mensaje,
                                 nombreTipoUsuario = tipoUsuario.Nombre

                             }).ToList();
                }

            }

            return lista;
        }

        [HttpGet]
        [Route("api/PaginaTipoUsuario/listarBotones")]
        public List<BotonesCLS> listarBotones()
        {
            List<BotonesCLS> listaBotones = new List<BotonesCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                listaBotones = (from boton in bd.Button
                                where boton.Bhabilitado == 1
                                select new BotonesCLS
                                {
                                    iidButton = boton.Iidbutton,
                                    nombreButton = boton.Nombrebutton

                                }).ToList();
            }

            return listaBotones;
        }

        [HttpGet]
        [Route("api/PaginaTipoUsuario/RecuperarDatos/{iidPaginaTipoUsuario}")]
        public PaginaTipoUsuarioCLS RecuperarDatos(int iidPaginaTipoUsuario)
        {
            PaginaTipoUsuarioCLS oPaginaTipoUsuarioCLS = new PaginaTipoUsuarioCLS();

            using (var bd = new BDBibliotecaContext())
            {
                oPaginaTipoUsuarioCLS.iidPaginaTipoUsuario = iidPaginaTipoUsuario;

                List<int> listaIDSbotones = bd.PaginaTipoUsuButton.Where(p => p.Iidpaginatipousuario == iidPaginaTipoUsuario && p.Bhabilitado == 1)
                    .Select(p => p.Iidbutton).Cast<int>().ToList();

                oPaginaTipoUsuarioCLS.idsButtons = listaIDSbotones;
            }

            return oPaginaTipoUsuarioCLS;
        }

        [HttpPost]
        [Route("api/PaginaTipoUsuario/Guardar")]
        public int Guardar([FromBody] PaginaTipoUsuarioCLS oPaginaTipoUsuarioCLS)
        {
            int respuesta = 0;
            try
            {
                using (var bd = new BDBibliotecaContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        if (oPaginaTipoUsuarioCLS.iidPaginaTipoUsuario != 0)
                        {

                            //Primero deshabilitamos bhabilitado=0 todos los botones asocidados a la PaginaTipoUsuario de la base de datos
                            List<PaginaTipoUsuButton> listaPagTipoUsuButton = (from pagTipoUsuButton in bd.PaginaTipoUsuButton
                                                                               where pagTipoUsuButton.Iidpaginatipousuario == oPaginaTipoUsuarioCLS.iidPaginaTipoUsuario
                                                                               select pagTipoUsuButton).ToList();
                            if (listaPagTipoUsuButton.Count > 0 && listaPagTipoUsuButton != null)
                            {
                                foreach (PaginaTipoUsuButton item in listaPagTipoUsuButton)
                                {
                                    item.Bhabilitado = 0;
                                    bd.SaveChanges();
                                }
                            }

                            //Aquí habilitamos los botones que nos vengan en oPaginaTipoUsuarioCLS.idsButtons
                            if (oPaginaTipoUsuarioCLS.idsButtons.Count > 0)
                            {
                                foreach (int num in oPaginaTipoUsuarioCLS.idsButtons)
                                {
                                    int numVeces = bd.PaginaTipoUsuButton.Where(p => p.Iidpaginatipousuario == oPaginaTipoUsuarioCLS.iidPaginaTipoUsuario && p.Iidbutton == num).Count();
                                    if (numVeces == 0)
                                    {
                                        PaginaTipoUsuButton oPaginaTipoUsuButton = new PaginaTipoUsuButton();

                                        oPaginaTipoUsuButton.Iidpaginatipousuario = oPaginaTipoUsuarioCLS.iidPaginaTipoUsuario;
                                        oPaginaTipoUsuButton.Iidbutton = num;
                                        oPaginaTipoUsuButton.Bhabilitado = 1;

                                        bd.PaginaTipoUsuButton.Add(oPaginaTipoUsuButton);
                                        bd.SaveChanges();

                                    }
                                    else
                                    {
                                        PaginaTipoUsuButton oPaginaTipoUsuButton = bd.PaginaTipoUsuButton.
                                            Where(p => p.Iidpaginatipousuario == oPaginaTipoUsuarioCLS.iidPaginaTipoUsuario && p.Iidbutton == num).First();
                                        oPaginaTipoUsuButton.Bhabilitado = 1;

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
        [Route("api/PaginaTipoUsuario/listarBotones/{iidUsuario}/{iidPagina}")]
        public List<int> listarBotones(int iidUsuario, int iidPagina)
        {
            List<int> listaBotones = new List<int>();//Devuelvo una lista de enteros porque solo quiero los id de los botones
            try
            {
                using (var bd = new BDBibliotecaContext())
                {
                    int iidTipoUsuario = (int)bd.Usuario.Where(p => p.Iidusuario == iidUsuario).First().Iidtipousuario;
                    int iidPaginaTipoUsuario = bd.PaginaTipoUsuario.Where(p => p.Iidtipousuario == iidTipoUsuario && p.Iidpagina == iidPagina).First().Iidpaginatipousuario;
                    List<PaginaTipoUsuButton> listaPagUsuButton = bd.PaginaTipoUsuButton.Where(p => p.Iidpaginatipousuario == iidPaginaTipoUsuario && p.Bhabilitado == 1).ToList();

                    if (listaPagUsuButton.Count > 0)
                    {
                        listaBotones = new List<int>();
                        listaBotones = listaPagUsuButton.Select(p => p.Iidbutton).Cast<int>().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                listaBotones = new List<int>();//Aquí, en caso de que se caiga lo instanciamos, NO devolvemos NULL, porque si no Blazor se cae
            }

            return listaBotones;
        }
    }
}
