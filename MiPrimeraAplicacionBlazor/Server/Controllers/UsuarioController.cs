using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiPrimeraAplicacionBlazor.Shared;
using MiPrimeraAplicacionBlazor.Server.Models;
using System.Security.Cryptography;
using System.Text;

namespace MiPrimeraAplicacionBlazor.Server.Controllers
{
    [ApiController]
    public class UsuarioController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpPost]
        [Route("api/Usuario/Login")]
        public int Login([FromBody] UsuarioCLS oUsuarioCLS)
        {
            int respuesta = 0;

            try
            {
                using(var bd = new BDBibliotecaContext())
                {
                    string clave = oUsuarioCLS.password;
                    SHA256Managed sha = new SHA256Managed();
                    byte[] buffer = Encoding.Default.GetBytes(clave);
                    byte[] claveCifrada = sha.ComputeHash(buffer);
                    string claveCifradaString = BitConverter.ToString(claveCifrada).Replace("-", "");

                    int numVeces = bd.Usuario.Where(p => p.Nombreusuario == oUsuarioCLS.nombre && p.Contra == claveCifradaString && p.Token == null).Count();

                    if(numVeces == 0)
                    {
                        int numCant = bd.Usuario.Where(p => p.Nombreusuario == oUsuarioCLS.nombre && p.Contra == claveCifradaString && p.Token != null).Count();
                        if(numCant > 0)
                        {
                            return respuesta = -1;
                        }
                        respuesta = 0;

                    }
                    else
                    {
                        respuesta = bd.Usuario.Where(p => p.Nombreusuario == oUsuarioCLS.nombre && p.Contra == claveCifradaString && p.Token == null).First().Iidusuario;
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
        [Route("api/Usuario/listarPagPorTipoUsuario/{iidUsuario}")]
        public List<PaginaCLS> listarPagPorTipoUsuario(int iidUsuario)
        {
            List<PaginaCLS> listaPaginas = new List<PaginaCLS>();

            try
            {
                using(var bd = new BDBibliotecaContext())
                {
                    //Necesitamos tener el iidTipoUsuario a partir de nuestro iidUsuario
                    int iidTipoUsuario = (int)bd.Usuario.Where(p => p.Iidusuario == iidUsuario).First().Iidtipousuario;

                    listaPaginas = (from paginaTipoUsuar in bd.PaginaTipoUsuario
                                    join pagina in bd.Pagina
                                    on paginaTipoUsuar.Iidpagina equals pagina.Iidpagina
                                    where paginaTipoUsuar.Iidtipousuario == iidTipoUsuario && pagina.Bhabilitado == 1 
                                    && paginaTipoUsuar.Bhabilitado == 1 && pagina.Bvisible ==1
                                    select new PaginaCLS
                                    {
                                        iidPagina = pagina.Iidpagina,
                                        mensaje = pagina.Mensaje,
                                        accion = pagina.Accion + "/" + pagina.Iidpagina

                                    }).ToList();
                                    
                }

            }
            catch(Exception ex)
            {
                listaPaginas = null;
            }

            return listaPaginas;
        }

        [HttpGet]
        [Route("api/Usuario/Token/{token}")]
        public int Token(string token)
        {
            int respuesta = 0;

            try
            {
                using(var bd = new BDBibliotecaContext())
                {
                    int num = bd.Usuario.Where(p => p.Token == token).Count();
                    if(num > 0)
                    {
                        Usuario oUsuario = bd.Usuario.Where(p => p.Token == token).First();
                        oUsuario.Token = null;

                        bd.SaveChanges();

                        respuesta = 1;
                    }
                    else
                    {
                        respuesta = 0;
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
        [Route("api/Usuario/ListarUsuarios")]
        public List<UsuarioCLS> ListarUsuarios()
        {
            List<UsuarioCLS> lista = new List<UsuarioCLS>();

            try
            {
                using(var bd = new BDBibliotecaContext())
                {
                    lista = (from usuario in bd.Usuario
                             join persona in bd.Persona
                             on usuario.Iidpersona equals persona.Iidpersona
                             where usuario.Bhabilitado == 1 && persona.Bhabilitado == 1
                             select new UsuarioCLS
                             {
                                 iidUsuario = usuario.Iidusuario,
                                 nombre = persona.Nombre + " " + persona.Appaterno + " " + persona.Apmaterno

                             }).ToList();
                }

            }
            catch(Exception ex)
            {
                lista = null;
            }

            return lista;
        }
    }
}
