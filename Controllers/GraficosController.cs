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
    using System.Threading.Tasks;
    using System.Web.UI.WebControls;

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

            var topPacientes = listaCit
               .GroupBy(cita => cita.pacienteId)
               .Select(grupo => new
               {
                   PacienteId = grupo.Key,
                   CantidadCitas = grupo.Count()
               })
               .OrderByDescending(p => p.CantidadCitas)
               .Take(5)
               .ToList();
            ViewBag.TopPacientes = topPacientes;

            return View(listaCit);
        }
        [HttpGet]
        public async Task<JsonResult> ContarPacientes()
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
                    //nombrePac = elemento.Value.nombrePac,
                    //edad = elemento.Value.edad,
                    //tutor = elemento.Value.tutor,
                    //alergias = elemento.Value.alergias,
                    //enfCronicas = elemento.Value.enfCronicas,
                    //hospitazacion = elemento.Value.hospitazacion,
                    //operaciones = elemento.Value.operaciones,
                    //mai = elemento.Value.mai
                });
            }
            int cantidadPacientes = listaPac.Count;
            return Json(cantidadPacientes, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> ContarCitasM()
        {
            Dictionary<string, Citas> lista = new Dictionary<string, Citas>();
            FirebaseResponse response = cliente.Get("citas");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                lista = JsonConvert.DeserializeObject<Dictionary<string, Citas>>(response.Body);
            List<Citas> listaCM = new List<Citas>();
            foreach (KeyValuePair<string, Citas> elemento in lista)
            {
                listaCM.Add(new Citas()
                {
                    horaProgramada = elemento.Value.horaProgramada,
                    id = elemento.Value.id,
                    pacienteId = elemento.Value.pacienteId,
                });
            }

            int cantidadCitas = listaCM.Count;
            return Json(cantidadCitas, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> ContarSO()
        {
            Dictionary<string, Citas> lista = new Dictionary<string, Citas>();
            FirebaseResponse response = cliente.Get("citas");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                lista = JsonConvert.DeserializeObject<Dictionary<string, Citas>>(response.Body);
            List<Citas> listaCitM = new List<Citas>();
            foreach (KeyValuePair<string, Citas> elemento in lista)
            {
                listaCitM.Add(new Citas()
                {
                    id = elemento.Value.id,
                    servicioOfrecido = elemento.Value.servicioOfrecido,
                });
            }
            int contarSO = listaCitM.Count;
            return Json(contarSO, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> TopPacientes()
        {
            Dictionary<string, Citas> lista = new Dictionary<string, Citas>();
            FirebaseResponse response = cliente.Get("citas");
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                lista = JsonConvert.DeserializeObject<Dictionary<string, Citas>>(response.Body);
            List<Citas> listaT = new List<Citas>();

            foreach (KeyValuePair<string, Citas> elemento in lista)
            {
                listaT.Add(new Citas()
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
            var listaTop = listaT.GroupBy(x => x.pacienteId).Select(grupo => new
            {
                pacienteID = grupo.Key,
                CountCitas = grupo.Count(),

            }).OrderByDescending(p=>p.CountCitas).Take(5).ToList();
            DateTime now = DateTime.Now;
            DateTime meses6 = now.AddMonths(-6);
            listaT = listaT.Where(c => DateTime.Parse(c.fechaProgramada).Month >= now.Month && DateTime.Parse(c.fechaProgramada).Month <=now.Month).ToList();

            return Json(listaTop, JsonRequestBehavior.AllowGet);
        }
    }
}