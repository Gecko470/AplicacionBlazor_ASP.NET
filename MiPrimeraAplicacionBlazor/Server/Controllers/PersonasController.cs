using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiPrimeraAplicacionBlazor.Shared;
using MiPrimeraAplicacionBlazor.Server.Models;
using System.Text;
using System.Security.Cryptography;
using System.Transactions;
using MiPrimeraAplicacionBlazor.Server.Clases;

namespace MiPrimeraAplicacionBlazor.Server.Controllers
{
    [ApiController]//Importante esto, estamos creando servicios
    public class PersonasController : Controller
    {
        //Importante comentar o borrar esto, si no no funciona

        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        [Route("api/Personas/listarPersonas")]
        public List<PersonasCLS> listarPersonas()
        {
            List<PersonasCLS> listaPersonas = new List<PersonasCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                listaPersonas = (from persona in bd.Persona
                                 where persona.Bhabilitado == 1
                                 select new PersonasCLS
                                 {
                                     iidPersona = persona.Iidpersona,
                                     nombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                     email = persona.Correo,
                                     fecha = (persona.Fechanacimiento == null) ? "" : persona.Fechanacimiento.Value.ToShortDateString()

                                 }).ToList();
            }

            return listaPersonas;
        }

        [HttpGet]
        [Route("api/Personas/EliminarPersona/{iidPersona}")]
        public int EliminarPersona(int iidPersona)
        {
            int respuesta = 0;

            try
            {
                using (var bd = new BDBibliotecaContext())
                {
                    Persona oPersona = bd.Persona.Where(p => p.Iidpersona == iidPersona).First();
                    oPersona.Bhabilitado = 0;
                    bd.SaveChanges();
                }
                respuesta = 1;
            }
            catch (Exception ex)
            {
                respuesta = 0;
            }
            return respuesta;
        }

        [HttpGet]
        [Route("api/Personas/FiltrarPersonas/{data?}")]
        public List<PersonasCLS> FiltrarPersonas(string data)
        {
            List<PersonasCLS> listaPersonas = new List<PersonasCLS>();

            using (var bd = new BDBibliotecaContext())
            {
                if (data == null)
                {
                    listaPersonas = (from persona in bd.Persona
                                     where persona.Bhabilitado == 1
                                     select new PersonasCLS
                                     {
                                         iidPersona = persona.Iidpersona,
                                         nombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                         email = persona.Correo,
                                         fecha = (persona.Fechanacimiento == null) ? "" : persona.Fechanacimiento.Value.ToShortDateString()

                                     }).ToList();
                }
                else
                {
                    listaPersonas = (from persona in bd.Persona
                                     where persona.Bhabilitado == 1 && (persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno).Contains(data)
                                     select new PersonasCLS
                                     {
                                         iidPersona = persona.Iidpersona,
                                         nombreCompleto = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno,
                                         email = persona.Correo,
                                         fecha = (persona.Fechanacimiento == null) ? "" : persona.Fechanacimiento.Value.ToShortDateString()

                                     }).ToList();
                }
            }

            return listaPersonas;
        }

        [HttpGet]
        [Route("api/Personas/obtenerPersona/{iidPersona}")]
        public PersonasCLS obtenerPersona(int iidPersona)
        {
            PersonasCLS oPersonasCLS = new PersonasCLS();

            using (var bd = new BDBibliotecaContext())
            {
                oPersonasCLS = (from persona in bd.Persona
                                join usuario in bd.Usuario
                                on persona.Iidpersona equals usuario.Iidpersona
                                where persona.Iidpersona == iidPersona
                                select new PersonasCLS
                                {
                                    iidPersona = persona.Iidpersona,
                                    nombre = persona.Nombre,
                                    aPaterno = persona.Appaterno,
                                    aMaterno = persona.Apmaterno,
                                    telefono = persona.Telefono,
                                    email = persona.Correo,
                                    fechaNto = (DateTime)persona.Fechanacimiento,
                                    nombreUsuario = usuario.Nombreusuario,
                                    iidTipoUsuario = usuario.Iidtipousuario.ToString()

                                }).First();

                return oPersonasCLS;
            }
        }

        [HttpPost]
        [Route("api/Personas/Guardar")]
        public int Guardar([FromBody] PersonasCLS oPersonaCLS)
        {
            int respuesta = 0;

            try
            {
                using (var bd = new BDBibliotecaContext())
                {
                    using (var transaccion = new TransactionScope())
                    {

                        if (oPersonaCLS.iidPersona == 0)//Si el ID que nos viene del formulario es 0 es porque es un registro nuevo
                        {
                            //Agregamos en Persona

                            Persona oPersona = new Persona();

                            oPersona.Nombre = oPersonaCLS.nombre;
                            oPersona.Appaterno = oPersonaCLS.aPaterno;
                            oPersona.Apmaterno = oPersonaCLS.aMaterno;
                            oPersona.Telefono = oPersonaCLS.telefono;
                            oPersona.Correo = oPersonaCLS.email;
                            oPersona.Fechanacimiento = oPersonaCLS.fechaNto;
                            oPersona.Bhabilitado = 1;
                            oPersona.Btieneusuario = 1;

                            bd.Persona.Add(oPersona);
                            bd.SaveChanges();

                            //Agregamos en Usuario

                            string clave = oPersonaCLS.contrasenia;
                            SHA256Managed sha = new SHA256Managed();
                            byte[] buffer = Encoding.Default.GetBytes(clave);
                            byte[] claveCifrada = sha.ComputeHash(buffer);
                            string claveCifradaString = BitConverter.ToString(claveCifrada).Replace("-", "");

                            Usuario oUsuario = new Usuario();

                            oUsuario.Iidpersona = oPersona.Iidpersona;
                            oUsuario.Contra = claveCifradaString;
                            oUsuario.Iidtipousuario = int.Parse(oPersonaCLS.iidTipoUsuario);
                            oUsuario.Nombreusuario = oPersonaCLS.nombreUsuario;
                            oUsuario.Bhabilitado = 1;
                            oUsuario.Token = null;

                            bd.Usuario.Add(oUsuario);
                            bd.SaveChanges();

                            transaccion.Complete();
                            respuesta = 1;
                        }
                        else
                        {
                            //Modificamos Persona

                            Persona oPersona = bd.Persona.Where(p => p.Iidpersona == oPersonaCLS.iidPersona).First();

                            oPersona.Nombre = oPersonaCLS.nombre;
                            oPersona.Appaterno = oPersonaCLS.aPaterno;
                            oPersona.Apmaterno = oPersonaCLS.aMaterno;
                            oPersona.Telefono = oPersonaCLS.telefono;
                            oPersona.Correo = oPersonaCLS.email;
                            oPersona.Fechanacimiento = oPersonaCLS.fechaNto;

                            //Modificamos Usuario
                            Usuario oUsuario = bd.Usuario.Where(p => p.Iidpersona == oPersonaCLS.iidPersona).First();

                            oUsuario.Iidtipousuario = int.Parse(oPersonaCLS.iidTipoUsuario);
                            oUsuario.Nombreusuario = oPersonaCLS.nombreUsuario;

                            bd.SaveChanges();

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
        [Route("api/Personas/obtnerIdPersona/{iidUsuario}")]
        public int obtnerIdPersona(int iidUsuario)
        {
            int iidPersona = 0;

            try
            {
                using (var bd = new BDBibliotecaContext())
                {
                    iidPersona = (int)bd.Usuario.Where(p => p.Iidusuario == iidUsuario).First().Iidpersona;
                }

            }
            catch (Exception ex)
            {
                iidPersona = 0;
            }

            return iidPersona;
        }

        [HttpPost]
        [Route("api/Personas/Registrar")]
        public int Registrar([FromBody] RegistroUsuarioCLS oRegistroUsuarioCLS)
        {
            int respuesta = 0;

            try
            {
                using (var bd = new BDBibliotecaContext())
                {
                    using (var transaccion = new TransactionScope())
                    {
                        //Agregamos en Persona

                        Persona oPersona = new Persona();

                        oPersona.Nombre = oRegistroUsuarioCLS.nombre;
                        oPersona.Appaterno = oRegistroUsuarioCLS.aPaterno;
                        oPersona.Apmaterno = oRegistroUsuarioCLS.aMaterno;
                        oPersona.Telefono = oRegistroUsuarioCLS.telefono;
                        oPersona.Correo = oRegistroUsuarioCLS.email;
                        oPersona.Bhabilitado = 1;
                        oPersona.Btieneusuario = 1;

                        bd.Persona.Add(oPersona);
                        bd.SaveChanges();

                        //Agregamos en Usuario

                        string clave = oRegistroUsuarioCLS.contrasenia;
                        SHA256Managed sha = new SHA256Managed();
                        byte[] buffer = Encoding.Default.GetBytes(clave);
                        byte[] claveCifrada = sha.ComputeHash(buffer);
                        string claveCifradaString = BitConverter.ToString(claveCifrada).Replace("-", "");

                        Usuario oUsuario = new Usuario();

                        oUsuario.Iidpersona = oPersona.Iidpersona;
                        oUsuario.Contra = claveCifradaString;
                        oUsuario.Iidtipousuario = 16;
                        oUsuario.Nombreusuario = oRegistroUsuarioCLS.nombreUsuario;
                        oUsuario.Bhabilitado = 1;

                        string token = "";
                        for (int i = 0; i < 20; i++)
                        {
                            Random r = new Random();
                            int n = r.Next(0, 9);
                            token += n.ToString();
                        }
                        oUsuario.Token = token;

                        Generic.enviarCorreo(oPersona.Correo, "Activa tu cuenta en Aplicación Blazor", " Para activar tu cuenta en Aplicación Blazor haz click <a href='https://localhost:44307/Token/"+token+"'>Aquí</a>");

                        bd.Usuario.Add(oUsuario);
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
    }
}