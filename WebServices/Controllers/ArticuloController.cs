using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using WebServices.Models;

namespace WebServices.Controllers
{
    public class ArticuloController : ApiController
    {

        List<ArticuloClass> articulos = new List<ArticuloClass>();
        List<DetalleClass> detalleArticulo = new List<DetalleClass>();
        string conexion = "data source=DESKTOP-T2UP07I;initial catalog=Prueba;integrated security=True";

        [HttpGet]
        [Route("api/articulo/obtenerArticulo")]
        // GET: Articulo
        public JsonResult<List<ArticuloClass>> obtenerArticulo()
        {
            using (SqlConnection sqlCon = new SqlConnection(conexion))
            {
                //Se crea un objeto con la consulta SQL para seleccionar los datos de la tabla
                //El bloque using asegura que el objeto cmd se liberará correctamente una vez que se complete la ejecución de la consulta
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM articulos",sqlCon))
                {
                    //Abrir una coneión con la base de datos
                    sqlCon.Open();
                    //SqlDataReader se utiliza para leer los datos de la base de datos de forma secuencial
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Read devuelve una fila de resultados hasta que se alcance el final de los resultados
                        while (sdr.Read())
                        {
                            //Se agrega un nuevo objeto(ArticuloClass) a la lista(articulo)
                            articulos.Add(new ArticuloClass
                            {
                                //Se convierte el valor del objeto al valor específico de los campos de la clase
                                id = Convert.ToInt32(sdr["id"]),
                                nombre = Convert.ToString(sdr["nombre"]),
                                descripcion = Convert.ToString(sdr["descripcion"]),
                                precio = Convert.ToDecimal(sdr["precio"]),
                                codigo = Convert.ToInt32(sdr["codigo"])
                            });
                        }
                    }
                    //Se cierra la conexión a la base de datos
                    sqlCon.Close();
                }
            }
            //Retornar un Json con la lista de registro de la tabla articulo
            return Json<List<ArticuloClass>>(articulos);    
        }

        [HttpGet]
        [Route("api/articulo/obtenerArticuloXid")]
        // GET: Articulo
        public JsonResult<ArticuloClass> obtenerArticuloXid(int idArticulo)
        {
            ArticuloClass articulo = new ArticuloClass();
            using (SqlConnection sqlCon = new SqlConnection(conexion))
            {
                //Se crea un objeto con la consulta SQL para seleccionar los datos de la tabla
                //El bloque using asegura que el objeto cmd se liberará correctamente una vez que se complete la ejecución de la consulta
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM articulos where id = " + idArticulo, sqlCon))
                {
                    //Abrir una coneión con la base de datos
                    sqlCon.Open();
                    //SqlDataReader se utiliza para leer los datos de la base de datos de forma secuencial
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Read devuelve una fila de resultados hasta que se alcance el final de los resultados
                        while (sdr.Read())
                        {
                            //Se convierte el valor del objeto al valor específico de los campos de la clase
                            articulo.id  = Convert.ToInt32(sdr["id"]);
                            articulo.nombre = Convert.ToString(sdr["nombre"]);
                            articulo.descripcion = Convert.ToString(sdr["descripcion"]);
                            articulo.precio = Convert.ToDecimal(sdr["precio"]);
                            articulo.codigo = Convert.ToInt32(sdr["codigo"]);                           
                        }
                    }
                    //Se cierra la conexión a la base de datos
                    sqlCon.Close();
                }
            }
            //Retornar un Json con el registro de un articulo en específicio
            return Json<ArticuloClass>(articulo);
        }

        [HttpPut]
        [Route("api/articulo/actualizarArticulo")]
        //El parametro [FromBody] para indicar que se espera recibir un objeto ExampleObject en el cuerpo del mensaje HTTP
        public IHttpActionResult actualizarArticulo([FromBody] ArticuloClass articulo)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conexion))
                {
                    //Abrir una coneión con la base de datos
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand("actualizacion_articulo", sqlCon);
                    //Indicar que se está llamando un procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    //Agrega los valores a los parámetros necesarios para actualizar
                    cmd.Parameters.AddWithValue("@id", articulo.id);
                    cmd.Parameters.AddWithValue("@nombre", articulo.nombre);
                    cmd.Parameters.AddWithValue("@descri", articulo.descripcion);
                    cmd.Parameters.AddWithValue("@precio", articulo.precio);
                    cmd.Parameters.AddWithValue("@codigo", articulo.codigo);
                    //se llama al método ExecuteNonQuery para ejecutar el procedimiento almacenado y realizar la modificación en la base de datos,
                    //también este método devuelve el número de filas afectadas por la consulta.
                    cmd.ExecuteNonQuery();
                    //Se cierra la conexión a la base de datos
                    sqlCon.Close();
                }
                //Si se realiza la actualización se envía Actualizado
                return Ok("Actualizado");

            }catch (Exception)
            {
                return NotFound();
            }
        }

        //----------------------------------- Area de detalle articulo -----------------------------------------------------------

        [HttpGet]
        [Route("api/articulo/consultarDetalle")]
        // GET: Articulo
        public JsonResult<List<DetalleClass>> consultarDetalle(int idArticulo)
        {
            using (SqlConnection sqlCon = new SqlConnection(conexion))
            {
                //Se crea un objeto con la consulta SQL para seleccionar los datos de la tabla
                //El bloque using asegura que el objeto cmd se liberará correctamente una vez que se complete la ejecución de la consulta
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM detalle_articulos where id_articulo = " + idArticulo))
                {
                    cmd.Connection = sqlCon;
                    //Abrir una coneión con la base de datos
                    sqlCon.Open();
                    //SqlDataReader se utiliza para leer los datos de la base de datos de forma secuencial
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        //Read devuelve una fila de resultados hasta que se alcance el final de los resultados
                        while (sdr.Read())
                        {
                            //Se agrega un nuevo objeto(DetalleClass) a la lista(detalleArticulo)
                            detalleArticulo.Add(new DetalleClass
                            {
                                //Se convierte el valor del objeto al valor específico de los campos de la clase
                                id = Convert.ToInt32(sdr["id"]),
                                id_articulo = Convert.ToInt32(sdr["id_articulo"]),
                                cantidad = Convert.ToInt32(sdr["cantidad"]),
                                precio = Convert.ToDecimal(sdr["precio"])
                            });
                        }
                    }
                    //Cerrar la conexión de la base de datos
                    sqlCon.Close();
                }
            }
            //Retornar un Json con el registro del detalle_articulo en específicio
            return Json<List<DetalleClass>>(detalleArticulo);
        }

        [HttpDelete]
        [Route("api/articulo/eliminarDetalle")]
        public IHttpActionResult eliminarDetalle(int idDetalle)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(conexion))
                {
                    //Abrir una coneión con la base de datos
                    sqlCon.Open();
                    SqlCommand cmd = new SqlCommand("eliminar_detalle", sqlCon);
                    //Indicar que se está llamando un procedimiento almacenado
                    cmd.CommandType = CommandType.StoredProcedure;
                    //Agrega los valores a los parámetros necesarios para actualizar
                    cmd.Parameters.AddWithValue("@id", idDetalle);
                    //se llama al método ExecuteNonQuery para ejecutar el procedimiento almacenado y realizar la modificación en la base de datos,
                    //también este método devuelve el número de filas afectadas por la consulta.
                    cmd.ExecuteNonQuery();
                    //Cerrar la conexión de la base de datos
                    sqlCon.Close();
                }
                //Devuelve 1 cuando se ejecuto el procedimiento almacenado sin error
                return Ok(1);
            }
            catch (Exception)
            {
                return Ok(0);
            }
        }
    }
}
