using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace WebSite.Models
{
    public class Servicio
    {
        public HttpClient Client { get; set; }
        public Servicio()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(ConfigurationManager.AppSettings["ServiceUrl"].ToString());
        }
        public HttpResponseMessage GetResponse(string url)
        {
            return Client.GetAsync(url).Result;
        }
        //Metodo que envia la solicitud de put a la api establecida
        public HttpResponseMessage PutResponse(string url, HttpContent model)
        {
            return Client.PutAsync(url, model).Result;
        }
        /*public HttpResponseMessage PostResponse(string url, object model)
        {
            return Client.PostAsJsonAsync(url, model).Result;
        }*/
        public HttpResponseMessage DeleteResponse(string url)
        {
            return Client.DeleteAsync(url).Result;
        }
    }
}