using appWebEjemplo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Diagnostics;


namespace appWebEjemplo.Controllers
{
    public class AlumnoController : Controller
    {
        string cadenaConexion =
            "server=DESKTOP-I7M9PKF;"+
            "database=DSW_MVC;"+
            "Trusted_Connection=True;"+
            "Encrypt=False;"+
            "MultipleActiveResultSets=True;"+
            "TrustServerCertificate=False;";


        public IEnumerable<Carrera> listadoCarrera()
        {
            List<Carrera> lista = new List<Carrera>();
            SqlConnection con = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_carrera_select", con);
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                SqlDataReader reader = cmd.ExecuteReader();
                Carrera objAlumno;
                while (reader.Read())
                {
                    objAlumno = new Carrera()
                    {
                        codigoCarrera = reader.GetInt32(0),
                        nombreCarrera = reader.GetString(1)
                    };
                    lista.Add(objAlumno);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return lista;
        }

        public IEnumerable<Alumno> listadoTotal(string indicador)
        {
            List<Alumno> lista = new List<Alumno>();
            SqlConnection con = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_alumno_crud", con);
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@indicador", indicador);
                cmd.Parameters.AddWithValue("@codigo", 0);
                cmd.Parameters.AddWithValue("@nombres", "");
                cmd.Parameters.AddWithValue("@apellidos", "");
                cmd.Parameters.AddWithValue("@documento", "");
                SqlDataReader reader = cmd.ExecuteReader();
                Alumno objAlumno;
                while (reader.Read())
                {
                    objAlumno = new Alumno()
                    {
                        codigo = reader.GetInt32(0),
                        nombres = reader.GetString(1),
                        apellidos = reader.GetString(2),
                        documento= reader.GetString(3)
                    };
                    lista.Add(objAlumno);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return lista;
        }

        public IEnumerable<Alumno> listadoTotal(string indicador, string apellidos)
        {
            List<Alumno> lista = new List<Alumno>();
            SqlConnection con = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_alumno_crud", con);
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@indicador", indicador);
                cmd.Parameters.AddWithValue("@codigo", 0);
                cmd.Parameters.AddWithValue("@nombres", "");
                cmd.Parameters.AddWithValue("@apellidos", apellidos + "%");
                cmd.Parameters.AddWithValue("@documento", "");
                SqlDataReader reader = cmd.ExecuteReader();
                Alumno objAlumno;
                while (reader.Read())
                {
                    objAlumno = new Alumno()
                    {
                        codigo = reader.GetInt32(0),
                        nombres = reader.GetString(1),
                        apellidos = reader.GetString(2),
                        documento = reader.GetString(3)
                    };
                    lista.Add(objAlumno);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return lista;
        }

        public IEnumerable<Alumno> listadoTotal(string indicador, int fecha)
        {
            List<Alumno> lista = new List<Alumno>();
            SqlConnection con = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_alumno_crud", con);
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@indicador", indicador);
                cmd.Parameters.AddWithValue("@codigo", fecha);
                cmd.Parameters.AddWithValue("@nombres", "");
                cmd.Parameters.AddWithValue("@apellidos","");
                cmd.Parameters.AddWithValue("@documento", "");
                SqlDataReader reader = cmd.ExecuteReader();
                Alumno objAlumno;
                while (reader.Read())
                {
                    objAlumno = new Alumno()
                    {
                        codigo = reader.GetInt32(0),
                        nombres = reader.GetString(1),
                        apellidos = reader.GetString(2),
                        documento = reader.GetString(3)
                    };
                    lista.Add(objAlumno);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return lista;
        }

        public int registrarAlumno(Alumno alumno)
        {
            int procesar = 0;
            SqlConnection con = new SqlConnection(cadenaConexion);
            SqlCommand cmd;
            try
            {
                con.Open();
                cmd = new SqlCommand("usp_alumno_crud", con);
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@indicador","registrar");
                cmd.Parameters.AddWithValue("@codigo", 0);
                cmd.Parameters.AddWithValue("@nombres", alumno.nombres);
                cmd.Parameters.AddWithValue("@apellidos", alumno.apellidos);
                cmd.Parameters.AddWithValue("@documento", alumno.documento);
                cmd.Parameters.AddWithValue("@carrera", alumno.codigoCarrera);
                procesar= Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error : " +ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return procesar;
        }

        // Esta vista nos muestra el listado total de alumnos
        public IActionResult Index()
        {
            return View(listadoTotal("listarTodos"));
        }

        // Esta vista debe mostrar un cuadro de texto donde se ingrese el apellido, y nos devuelva
        // la coincidencia con esa palabra

        public IActionResult Create()
        {
            ViewBag.carreras = new SelectList(listadoCarrera(), "codigoCarrera", "nombreCarrera");
            return View(new Alumno());
        }

        [HttpPost]
        public IActionResult Create(Alumno registro)
        {
            //Codigo de conexion a base datos 
            Debug.WriteLine("Nombre : " + registro.nombres);
            Debug.WriteLine("Apellidos : " + registro.apellidos);
            Debug.WriteLine("Carrera : " + registro.codigoCarrera);

            int procesar = registrarAlumno(registro);
            if (procesar == 1)
            {
                ViewBag.mensajeValidacion = "Registro Ingresado Correctamente.";
            }
            else
            {
                ViewBag.mensajeValidacion = "Hubo un error al ingresar.";
            }
            ViewBag.carreras = new SelectList(listadoCarrera(), "codigoCarrera", "nombreCarrera");
            return View(registro);
        }


        public IActionResult busquedaApellido(string apellido="si")
        {
            return View(listadoTotal("busquedaPorApellido", apellido));
        }

        public IActionResult busquedaPorFecha(int fecha = 2020)
        {
            return View(listadoTotal("busquedaPorFecha", fecha));
        }

        public IActionResult listadoPaginacion(int fecha, int paginaActual=0)
        {
            int contadorTotal = listadoTotal("listarTodos").Count();
            int registrosPorHoja = 3;

            int numPaginas = contadorTotal % registrosPorHoja == 0
                            ? contadorTotal / registrosPorHoja
                            : (contadorTotal / registrosPorHoja) + 1;

            ViewBag.paginaActual = paginaActual;
            ViewBag.numPaginas= numPaginas;
            ViewBag.fecha = 2020;

            return View(
                listadoTotal("listarTodos").Skip(paginaActual* registrosPorHoja).Take(registrosPorHoja)
                );
        }
























        public IActionResult Home()
        {

            return View();
        }



    }
}
