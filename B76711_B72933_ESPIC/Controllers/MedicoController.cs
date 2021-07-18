using B76711_B72933_ESPIC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace B76711_B72933_ESPIC.Controllers
{
    public class MedicoController : Controller
    {

        public IConfiguration Configuration { get; }

        public MedicoController(IConfiguration configuration)
        {
            Configuration = configuration;
        } // constructor

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult login()
        {
            HttpContext.Session.SetString("variableSession", "valor en session");
            HttpContext.Session.SetInt32("variableInt", 0);
            return View();
        }

        
        public IActionResult contrasena()
        {
            return View();
        }

        [HttpPost]
        public IActionResult contrasena(MedicoModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);
                string sqlQuery = $"exec sp_cambiar_contrasenna @param_CEDULA='{model.cedula}', @param_CODIGO='{model.codigo}', @param_CONTRASENNA='{model.contrasenna}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteReader();
                    connection.Close();
                }
            }
            return RedirectToAction("login");
        }

        [HttpPost]
        public IActionResult login(MedicoModel model)
        {
            List<MedicoModel> medico = new List<MedicoModel>();
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec sp_verificar_medico @param_CEDULA='{model.cedula}',@param_CODIGO='{model.codigo}',@param_CONTRASENNA='{model.contrasenna}'";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        connection.Open();
                        SqlDataReader medicoReader = command.ExecuteReader();
                        while (medicoReader.Read())
                        {
                            MedicoModel medicoTem = new MedicoModel();
                            medicoTem.cedula = Int32.Parse(medicoReader["CEDULA"].ToString());
                            medicoTem.nombre = medicoReader["NOMBRE"].ToString();
                            HttpContext.Session.SetInt32("variableInt", medicoTem.cedula);
                            medico.Add(medicoTem);
                        }// while
                        
                        connection.Close();
                    }
                }
            }
            if (medico.Count == 0)
            {
                return RedirectToAction("login");
            }
            else
            {
                medico.Clear();
                return RedirectToAction("Index");
            }
           
        }

        public ActionResult LogOut()
        {

            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }
    }
}
