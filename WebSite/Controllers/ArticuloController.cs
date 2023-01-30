using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using WebSite.Models;

namespace WebSite.Controllers
{
    public class ArticuloController : Controller
    {
        // GET: Articulo
        public async Task<ActionResult> obtenerArticulo()
        {
            try
            {
                List<Articulo> articulos = new List<Articulo>();
                Servicio serviceObj = new Servicio();
                HttpResponseMessage response = serviceObj.GetResponse("api/articulo/obtenerArticulo");
                //response.EnsureSuccessStatusCode();
                //Leer la respuesta de la consulta de la api de articulo
                var consuArticulos = await response.Content.ReadAsStringAsync();
                List<Articulo> resultado =  JsonConvert.DeserializeObject<List<Articulo>>(consuArticulos);
                
                ViewBag.Title = "Todo los articulos";
                return View(resultado);
            }
            catch (Exception)
            {
                MessageBox.Show("No hay datos para mostrar!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> modificarArticulo(int id)
        {
            try
            {            
                Servicio serviceObj = new Servicio();
                HttpResponseMessage response = serviceObj.GetResponse("api/articulo/obtenerArticuloXid?idArticulo=" + id.ToString());
                //response.EnsureSuccessStatusCode();
                //Leer la respuesta de la consulta de la api de articulo
                var consuArticulos = await response.Content.ReadAsStringAsync();
                Articulo resultado = JsonConvert.DeserializeObject<Articulo>(consuArticulos);

                ViewBag.Title = "Todo los articulos";
                return View(resultado);
            }
            catch (Exception)
            {
                MessageBox.Show("No hay datos para mostrar!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return RedirectToAction("obtenerArticulo");
            }
        }

        //Metodo para modicar los datos de un articulo en sql server
        public async Task<ActionResult> editarArticulo(Articulo articulo)
        {
            Servicio serviceObj = new Servicio();
            //para enviar un objeto en formato JSON y pasarlo en HttpContent
            var json = JsonConvert.SerializeObject(articulo);
            //Se usa el StringContent para crear un contenido a partir de la cadena JSON que se desea enviar en la solicitud PutAsync.
            //El primer parámetro es la cadena que se desea enviar como contenido, el segundo parámetro es el encoding que se desea utilizar
            //para la cadena y el tercer parámetro es el tipo de contenido que se está enviando, aquí se está indicando que se está
            //enviando un contenido de tipo "application/json"
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = serviceObj.PutResponse("api/articulo/actualizarArticulo", content);
            //Leer la respuesta de la consulta de la api de articulo
            var consuEditar = await response.Content.ReadAsStringAsync();
            var resultado = JsonConvert.DeserializeObject<String>(consuEditar);
            if (resultado == "Actualizado")
            {
                MessageBox.Show("Se ha actualizado el registro!", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return RedirectToAction("obtenerArticulo");
            }
            else
            {
                MessageBox.Show("No se llenaron los campos requeridos!", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return RedirectToAction("modificarArticulo", new { id = articulo.id });
            }
            
        }
    }
}