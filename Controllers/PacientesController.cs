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
    public class PacientesController : Controller
    {
        IFirebaseClient cliente;
        // GET: Pacientes
        public PacientesController()
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
            Dictionary<string, Pacientes> lista = new Dictionary<string, Pacientes>();
            FirebaseResponse response = cliente.Get("pacientes");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                lista = JsonConvert.DeserializeObject<Dictionary<string, Pacientes>>(response.Body);

            List<Pacientes> listaPac = new List<Pacientes>();

            foreach (KeyValuePair<string, Pacientes> elemento in lista)
            {
                listaPac.Add(new Pacientes()
                {
                    id = elemento.Value.id,
                    nombrePac = elemento.Value.nombrePac,
                    edad = elemento.Value.edad,
                    tutor = elemento.Value.tutor,
                    alergias = elemento.Value.alergias,
                    enfCronicas = elemento.Value.enfCronicas,
                    hospitazacion = elemento.Value.hospitazacion,
                    operaciones = elemento.Value.operaciones,
                    mai = elemento.Value.mai

                });
            }

            return View(listaPac);
        }
        public ActionResult Crear()
        {
            return View();
        }
       
        public ActionResult Editar(string pacC)
        {
            FirebaseResponse response = cliente.Get("pacientes/" + pacC);
            Pacientes oPac = response.ResultAs<Pacientes>();
            oPac.userId = pacC;

            return View(oPac);
        }
        public ActionResult Eliminar(string pacC)
        {
            FirebaseResponse response = cliente.Delete("pacientes/" + pacC);
            return RedirectToAction("Inicio", "Pacientes");
        }

        [HttpPost]
        public ActionResult Crear(Pacientes pacC)
        {
            string IdGenerado = Guid.NewGuid().ToString("N");
            pacC.userId = IdGenerado;
            pacC.id = Guid.NewGuid().ToString("N");
            SetResponse response = cliente.Set("pacientes/" + IdGenerado, pacC);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return View();
            else
                return View();
        }

        [HttpPost]
        public ActionResult Editar(Pacientes pacC)
        {
            string idUser = pacC.userId;
            pacC.id = null;
            FirebaseResponse response = cliente.Update("pacientes/" + idUser, pacC);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return RedirectToAction("Inicio", "Pacientes");
            else
                return View();
        }
    }
}