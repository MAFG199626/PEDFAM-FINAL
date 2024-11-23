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
    public class MedicosController : Controller
    {
        IFirebaseClient cliente;
        // GET: Pacientes
        public MedicosController()
        {
            IFirebaseConfig config = new FirebaseConfig()
            {
                AuthSecret = "AIzaSyBmfkceRMScmXmxPIeMapnT3tt4NNLSjx0",
                BasePath = "https://pedfam-f5e91-default-rtdb.firebaseio.com/"
            };
            cliente = new FirebaseClient(config);

        }

        public ActionResult Inicio()
        {
            Dictionary<string, Medicos> lista = new Dictionary<string, Medicos>();
            FirebaseResponse response = cliente.Get("medicos");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                lista = JsonConvert.DeserializeObject<Dictionary<string, Medicos>>(response.Body);

            List<Medicos> listaMed = new List<Medicos>();

            foreach (KeyValuePair<string, Medicos> elemento in lista)
            {
                listaMed.Add(new Medicos()
                {
                    id = elemento.Value.id,
                    mai = elemento.Value.mai,
                    nombreCompleto = elemento.Value.nombreCompleto,
                    pass = elemento.Value.pass,
                    telefono = elemento.Value.telefono,
                    urlFirmaMedica = elemento.Value.urlFirmaMedica,
                    urlFt = elemento.Value.urlFt,
                    userId = elemento.Value.userId

                });
            }

            return View(listaMed);
        }
        public ActionResult Crear()
        {
            return View();
        }

        public ActionResult Editar(string medC)
        {
            FirebaseResponse response = cliente.Get("medicos/" + medC);
            Medicos oMed = response.ResultAs<Medicos>();
            oMed.userId = medC;

            return View(oMed);
        }
        public ActionResult Eliminar(string medC)
        {
            FirebaseResponse response = cliente.Delete("medicos/" + medC);
            return RedirectToAction("Inicio", "Medicos");
        }

        [HttpPost]
        public ActionResult Crear(Medicos medC)
        {
            string IdGenerado = Guid.NewGuid().ToString("N");
            medC.userId = IdGenerado;

            medC.id = Guid.NewGuid().ToString("N");
            SetResponse response = cliente.Set("medicos/" + IdGenerado, medC);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return View();
            else
                return View();
        }

        [HttpPost]
        public ActionResult Editar(Medicos medC)
        {
            string idUser = medC.userId;
            medC.id = null;
            FirebaseResponse response = cliente.Update("medicos/" + idUser, medC);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return RedirectToAction("Inicio", "Medicos");
            else
                return View();
        }
    }
}