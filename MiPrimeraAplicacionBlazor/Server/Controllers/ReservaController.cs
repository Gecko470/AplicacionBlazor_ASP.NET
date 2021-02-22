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
    public class ReservaController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        [Route("api/Reserva/GuardarReserva")]
        public int GuardarReserva([FromBody] ReservaCLS oReservaCLS)
        {
            int respuesta = 0;

            try
            {
                using (var transaccion = new TransactionScope())
                {
                    using (var bd = new BDBibliotecaContext())
                    {
                        Reserva oReserva = new Reserva();

                        oReserva.Iidlibro = oReservaCLS.iidLibro;
                        oReserva.Iidusuario = int.Parse(oReservaCLS.iidUsuario);
                        oReserva.Numlibros = oReservaCLS.cantidad;
                        oReserva.Dfechareserva = DateTime.Now;
                        oReserva.Dfechainicioreserva = oReservaCLS.fechaInicio;
                        oReserva.Dfechafinreserva = oReservaCLS.fechaFin;
                        oReserva.Iidestadoreserva = 1;
                        oReserva.Bhabilitado = 1;

                        bd.Reserva.Add(oReserva);
                        bd.SaveChanges();

                        Libro oLibro = bd.Libro.Where(p => p.Iidlibro == oReservaCLS.iidLibro).First();

                        oLibro.Stock = oLibro.Stock - oReservaCLS.cantidad;

                        bd.SaveChanges();

                        ReservaHistorial oReservaHistorial = new ReservaHistorial();

                        oReservaHistorial.Iidreserva = oReserva.Iidreserva;
                        oReservaHistorial.Iidestado = 1;
                        oReservaHistorial.Iidusuario = oReservaCLS.iidUsuarioRegistra;
                        oReservaHistorial.Vobservacion = "Reserva registrada";
                        oReservaHistorial.Bhabilitado = 1;

                        bd.ReservaHistorial.Add(oReservaHistorial);
                        bd.SaveChanges();

                        transaccion.Complete();
                    }

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
        [Route("api/Reserva/ListarReservas/{iidEstadoReserva?}")]
        public List<ReservaCLS> ListarReservas(string iidEstadoReserva)
        {
            List<ReservaCLS> lista = new List<ReservaCLS>();

            using(var bd = new BDBibliotecaContext())
            {
                if(iidEstadoReserva == null)
                {
                    lista = (from reserva in bd.Reserva
                             join libro in bd.Libro
                             on reserva.Iidlibro equals libro.Iidlibro
                             join usuario in bd.Usuario
                             on reserva.Iidusuario equals usuario.Iidusuario
                             join persona in bd.Persona
                             on usuario.Iidpersona equals persona.Iidpersona
                             where reserva.Bhabilitado == 1
                             select new ReservaCLS
                             {
                                 iidReserva = reserva.Iidreserva,
                                 nombreLibro = libro.Titulo,
                                 cantidad = reserva.Numlibros,
                                 fechaInicioCadena = ((DateTime)reserva.Dfechainicioreserva).ToShortDateString(),
                                 fechaFinCadena = ((DateTime)reserva.Dfechafinreserva).ToShortDateString(),
                                 nombreUsuario = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno

                             }).ToList();
                }
                else
                {
                    lista = (from reserva in bd.Reserva
                             join libro in bd.Libro
                             on reserva.Iidlibro equals libro.Iidlibro
                             join usuario in bd.Usuario
                             on reserva.Iidusuario equals usuario.Iidusuario
                             join persona in bd.Persona
                             on usuario.Iidpersona equals persona.Iidpersona
                             where reserva.Bhabilitado == 1 && reserva.Iidestadoreserva == int.Parse(iidEstadoReserva)
                             select new ReservaCLS
                             {
                                 iidReserva = reserva.Iidreserva,
                                 nombreLibro = libro.Titulo,
                                 cantidad = reserva.Numlibros,
                                 fechaInicioCadena = ((DateTime)reserva.Dfechainicioreserva).ToShortDateString(),
                                 fechaFinCadena = ((DateTime)reserva.Dfechafinreserva).ToShortDateString(),
                                 nombreUsuario = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno

                             }).ToList();
                }
                
            }

            return lista;
        }

        [HttpGet]
        [Route("api/Reserva/EliminarReserva/{iidReserva}")]
        public int EliminarReserva(int iidReserva)
        {
            int respuesta = 0;
            try
            {
                using(var transaccion = new TransactionScope())
                {
                    using (var bd = new BDBibliotecaContext())
                    {
                        Reserva oReserva = bd.Reserva.Where(p => p.Iidreserva == iidReserva).First();
                        oReserva.Bhabilitado = 0;

                        bd.SaveChanges();

                        Libro oLibro = bd.Libro.Where(p => p.Iidlibro == oReserva.Iidlibro).First();
                        oLibro.Stock = oLibro.Stock + oReserva.Numlibros;

                        bd.SaveChanges();

                        transaccion.Complete();

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
        [Route("api/Reserva/ListarMisReservas/{iidUsuario?}")]
        public List<ReservaCLS> ListarMisReservas(int iidUsuario)
        {
            List<ReservaCLS> lista = new List<ReservaCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                    lista = (from reserva in bd.Reserva
                             join libro in bd.Libro
                             on reserva.Iidlibro equals libro.Iidlibro
                             join usuario in bd.Usuario
                             on reserva.Iidusuario equals usuario.Iidusuario
                             join persona in bd.Persona
                             on usuario.Iidpersona equals persona.Iidpersona
                             where reserva.Bhabilitado == 1 && reserva.Iidestadoreserva == 2 && reserva.Iidusuario == iidUsuario
                             select new ReservaCLS
                             {
                                 iidReserva = reserva.Iidreserva,
                                 nombreLibro = libro.Titulo,
                                 cantidad = reserva.Numlibros,
                                 fechaInicioCadena = ((DateTime)reserva.Dfechainicioreserva).ToShortDateString(),
                                 fechaFinCadena = ((DateTime)reserva.Dfechafinreserva).ToShortDateString(),
                                 nombreUsuario = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno

                             }).ToList();
            }

            return lista;
        }

        [HttpGet]
        [Route("api/Reserva/EnviarReserva/{iidReserva?}/{iidUsuarioRegistra?}")]
        public int EnviarReserva(int iidReserva, int iidUsuarioRegistra)
        {
            int respuesta = 0;

            try
            {
                using (var transaccion = new TransactionScope())
                {
                    using (var bd = new BDBibliotecaContext())
                    {
                        Reserva oReserva = bd.Reserva.Where(p => p.Iidreserva == iidReserva && p.Iidusuario == iidUsuarioRegistra).First();

                        oReserva.Iidestadoreserva = 2;

                        bd.SaveChanges();

                        ReservaHistorial oReservaHistorial = new ReservaHistorial();

                        oReservaHistorial.Iidreserva = oReserva.Iidreserva;
                        oReservaHistorial.Iidestado = 2;
                        oReservaHistorial.Iidusuario = iidUsuarioRegistra;
                        oReservaHistorial.Vobservacion = "Reserva pendiente";
                        oReservaHistorial.Bhabilitado = 1;

                        bd.ReservaHistorial.Add(oReservaHistorial);
                        bd.SaveChanges();

                        transaccion.Complete();
                    }

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
        [Route("api/Reserva/ObservarReserva/{iidReserva?}/{iidUsuarioRegistra?}")]
        public int ObservarReserva(int iidReserva, int iidUsuarioRegistra)
        {
            int respuesta = 0;

            try
            {
                using (var transaccion = new TransactionScope())
                {
                    using (var bd = new BDBibliotecaContext())
                    {
                        Reserva oReserva = bd.Reserva.Where(p => p.Iidreserva == iidReserva && p.Iidusuario == iidUsuarioRegistra).First();

                        oReserva.Iidestadoreserva = 3;

                        bd.SaveChanges();

                        ReservaHistorial oReservaHistorial = new ReservaHistorial();

                        oReservaHistorial.Iidreserva = oReserva.Iidreserva;
                        oReservaHistorial.Iidestado = 3;
                        oReservaHistorial.Iidusuario = iidUsuarioRegistra;
                        oReservaHistorial.Vobservacion = "Reserva observada";
                        oReservaHistorial.Bhabilitado = 1;

                        bd.ReservaHistorial.Add(oReservaHistorial);
                        bd.SaveChanges();

                        transaccion.Complete();
                    }

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
        [Route("api/Reserva/AceptarReserva/{iidReserva?}/{iidUsuarioRegistra?}")]
        public int AceptarReserva(int iidReserva, int iidUsuarioRegistra)
        {
            int respuesta = 0;

            try
            {
                using (var transaccion = new TransactionScope())
                {
                    using (var bd = new BDBibliotecaContext())
                    {
                        Reserva oReserva = bd.Reserva.Where(p => p.Iidreserva == iidReserva && p.Iidusuario == iidUsuarioRegistra).First();

                        oReserva.Iidestadoreserva = 6;

                        bd.SaveChanges();

                        ReservaHistorial oReservaHistorial = new ReservaHistorial();

                        oReservaHistorial.Iidreserva = oReserva.Iidreserva;
                        oReservaHistorial.Iidestado = 6;
                        oReservaHistorial.Iidusuario = iidUsuarioRegistra;
                        oReservaHistorial.Vobservacion = "Reserva aceptada";
                        oReservaHistorial.Bhabilitado = 1;

                        bd.ReservaHistorial.Add(oReservaHistorial);
                        bd.SaveChanges();

                        transaccion.Complete();
                    }

                    respuesta = 1;
                }
            }
            catch (Exception ex)
            {
                respuesta = 0;
            }

            return respuesta;
        }
    }
}
