using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PEDFAM.Controllers
{
    using FireSharp;
    using FireSharp.Config;
    using FireSharp.Interfaces;
    using FireSharp.Response;

    using Newtonsoft.Json;

    using PEDFAM.Models;
    public class GraficosController : Controller
    {
        IFirebaseClient cliente;
        // GET: Pacientes
        public GraficosController()
        {
            IFirebaseConfig config = new FirebaseConfig()
            {
                AuthSecret = "AIzaSyBmfkceRMScmXmxPIeMapnT3tt4NNLSjx0",
                BasePath = "https://pedfam-f5e91-default-rtdb.firebaseio.com/"
            };
            cliente = new FirebaseClient(config);

        }
        // GET: Graficos
         public int Contar()
        {
            List<Citas> listaCit = new List<Citas>();
            int AMed = listaCit.Count(cita => cita.servicioOfrecido == "[Atención Médica]");
            return AMed;
        }
        public ActionResult DashBoard()
        {
            Dictionary<string, Citas> lista = new Dictionary<string, Citas>();
            FirebaseResponse response = cliente.Get("citas");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                lista = JsonConvert.DeserializeObject<Dictionary<string, Citas>>(response.Body);

            List<Citas> listaCit = new List<Citas>();

            foreach (KeyValuePair<string, Citas> elemento in lista)
            {
                listaCit.Add(new Citas()
                {
                    fechaProgramada = elemento.Value.fechaProgramada,
                    horaProgramada = elemento.Value.horaProgramada,
                    id = elemento.Value.id,
                    pacienteId = elemento.Value.pacienteId,
                    servicioOfrecido = elemento.Value.servicioOfrecido,
                    titulo = elemento.Value.titulo,
                }
                );
            }
            //List<Citas> listafiltrada = listaCit.Where(cita => cita.servicioOfrecido == "[Vacunaciones]").ToList();
            //int tConsulta = listaCit.Count(cita => cita.servicioOfrecido == "[Vacunaciones]");
            //int tVacuna = listaCit.Count(cita => cita.servicioOfrecido == "[Atención Médica]");
            return View(listaCit);
            //return View();
        }
    }
}