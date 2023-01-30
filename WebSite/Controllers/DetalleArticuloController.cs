using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class DetalleArticuloController : Controller
    {
        // GET: DetalleArticulo
        public async Task<ActionResult> consultarDetalle(int id)
        {
            try
            {
                List<Detalle> articulos = new List<Detalle>();
                Servicio serviceObj = new Servicio();
                HttpResponseMessage response = serviceObj.GetResponse("api/articulo/consultarDetalle?idArticulo=" + id.ToString());
                //response.EnsureSuccessStatusCode();
                //Leer la respuesta de la consulta de la api de articulo
                var consuArticulos = await response.Content.ReadAsStringAsync();
                List<Detalle> resultado = JsonConvert.DeserializeObject<List<Detalle>>(consuArticulos);

                ViewBag.Title = "Todo los detalles";
                return View(resultado);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No hay datos para mostrar!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return RedirectToAction("obtenerArticulo");
            }
        }

        public async Task<ActionResult> eliminarDetalle(int id)
        {
                       
                Servicio serviceObj = new Servicio();
                HttpResponseMessage response = serviceObj.DeleteResponse("api/articulo/eliminarDetalle?idDetalle=" + id.ToString());
                //response.EnsureSuccessStatusCode();
                //Leer la respuesta de la consulta de la api de articulo
                int resEliminar = int.Parse(await response.Content.ReadAsStringAsync());
                
                if (resEliminar == 1)
                {
                //Se crea un objeto anónimo llamado "data" con dos propiedades y con el argumento "JsonRequestBehavior.AllowGet" para permitir
                //solicitudes HTTP GET a ser hechas al método de acción que devuelve estos datos JSON
                var datos = new { message = resEliminar, idDetalle = id };
                    return Json(datos, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar el registro!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return RedirectToAction("consultarDetalle");
                }

            
        }
    }
}