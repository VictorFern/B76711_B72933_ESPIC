using B76711_B72933_ESPIC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace B76711_B72933_ESPIC.Controllers
{
    public class CitasController : Controller
    {
        public IConfiguration Configuration { get; }

        public CitasController(IConfiguration configuration)
        {
            Configuration = configuration;
        } // constructor

        public IActionResult Index()
        {
            //int valor = (int)HttpContext.Session.GetInt32("variableInt");

            return View();
        }

        public IActionResult Registrar()
        {
            List<PacienteModel> paciente = new List<PacienteModel>();
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec sp_paciente";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        connection.Open();
                        SqlDataReader pacienteReader = command.ExecuteReader();
                        while (pacienteReader.Read())
                        {
                            PacienteModel pacienteTem = new PacienteModel();
                            pacienteTem.cedula = Int32.Parse(pacienteReader["CEDULA"].ToString());
                            pacienteTem.nombre = pacienteReader["NOMBRE"].ToString();

                            paciente.Add(pacienteTem);
                        }// while
                        connection.Close();
                    }

                }
            }
            ViewBag.Paciente= paciente;
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(CitasModel citasModel)
        {
            int valor = (int)HttpContext.Session.GetInt32("variableInt");
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);
                string sqlQuery = $"exec sp_registrar_cita @param_CEDULA_DR='{valor}',@param_CEDULA='{citasModel.cedula}',@param_CENTRO='{citasModel.centro}',@param_FECHA='{citasModel.fecha}',@param_HORA='{citasModel.hora}',@param_ESPECIALIDAD='{citasModel.especialidad}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteReader();
                    connection.Close();
                }
                TempData["Success"] = "Se registró correctamente.";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult lista()
        {
            List<PacienteModel> paciente = new List<PacienteModel>();
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec sp_paciente";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        connection.Open();
                        SqlDataReader pacienteReader = command.ExecuteReader();
                        while (pacienteReader.Read())
                        {
                            PacienteModel pacienteTem = new PacienteModel();
                            pacienteTem.cedula = Int32.Parse(pacienteReader["CEDULA"].ToString());
                            pacienteTem.nombre = pacienteReader["NOMBRE"].ToString();

                            paciente.Add(pacienteTem);
                        }// while
                        connection.Close();
                    }

                }

            }
            ViewBag.Paciente = paciente;
            return View();
        }

        [HttpPost]
        public IActionResult listaPaciente(PacienteModel paciente, string filtro)
        {
            List<object> lista = new List<object>();
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec sp_lista_citas_filto @param_CEDULA= '{paciente.cedula}', @param_OPCION='{filtro}'";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        connection.Open();
                        SqlDataReader pacienteReader = command.ExecuteReader();
                        while (pacienteReader.Read())
                        {
                            PacienteModel pacienteModel = new PacienteModel();
                            MedicoModel medicoTem = new MedicoModel();
                            medicoTem.nombre = pacienteReader["NOMBRE"].ToString();
                            pacienteModel.cedula = Int32.Parse(pacienteReader["CEDULA"].ToString());
                            pacienteModel.nombre = pacienteReader["Paciente"].ToString();
                            CitasModel citaTem = new CitasModel(Int32.Parse(pacienteReader["ID_CITA"].ToString())
                                                                  , pacienteReader["CENTRO_SALUD"].ToString()
                                                                 , pacienteReader["FECHA"].ToString()
                                                                , pacienteReader["ESPECIALIDAD"].ToString()
                                                                , pacienteReader["DETALLE"].ToString()
                                                                ,pacienteModel,medicoTem);

                            lista.Add(citaTem);
                        }// while
                        connection.Close();
                    }

                }

            }
            ViewBag.Lista= lista;
            return View();
        }

        public IActionResult detalleEliminar(int id)
        {
            List<object> lista = new List<object>();
            PacienteModel pacienteModel = new PacienteModel();
            CitasModel citasModel = new CitasModel();
            MedicoModel medicoTem = new MedicoModel();
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec sp_lista_citas_id @param_CEDULA= '{id}'";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        connection.Open();
                        SqlDataReader pacienteReader = command.ExecuteReader();
                        while (pacienteReader.Read())
                        {
                            medicoTem.nombre = pacienteReader["NOMBRE"].ToString();
                            pacienteModel.cedula = Int32.Parse(pacienteReader["CEDULA"].ToString());
                            pacienteModel.nombre = pacienteReader["Paciente"].ToString();
                            citasModel.cedula = Int32.Parse(pacienteReader["ID_CITA"].ToString());
                            citasModel.centro = pacienteReader["CENTRO_SALUD"].ToString();
                            citasModel.especialidad = pacienteReader["ESPECIALIDAD"].ToString();
                            citasModel.fecha = pacienteReader["FECHA"].ToString();
                            citasModel.detalle = pacienteReader["DETALLE"].ToString();
                            citasModel.paciente = pacienteModel;
                            citasModel.medico = medicoTem;

                            //lista.Add(vacunasModel);
                        }// while
                        connection.Close();
                    }
                }
            }
            return View(citasModel);
        }

        [HttpPost]
        public IActionResult Eliminar(CitasModel citasModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);
                string sqlQuery = $"exec sp_eliminar_cita @param_ID='{citasModel.cedula}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteReader();
                    connection.Close();
                }
                TempData["Success"] = "Se eliminó correctamente.";
                return RedirectToAction("lista");
            }
            return View();
        }

        public IActionResult actualizar(int id)
        {
            List<object> lista = new List<object>();
            PacienteModel pacienteModel = new PacienteModel();
            CitasModel citasModel = new CitasModel();
            MedicoModel medicoTem = new MedicoModel();

            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {

                    string sqlQuery = $"exec sp_lista_citas_id @param_CEDULA= '{id}'";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        connection.Open();
                        SqlDataReader pacienteReader = command.ExecuteReader();
                        while (pacienteReader.Read())
                        {
                            medicoTem.nombre = pacienteReader["NOMBRE"].ToString();
                            pacienteModel.cedula = Int32.Parse(pacienteReader["CEDULA"].ToString());
                            pacienteModel.nombre = pacienteReader["Paciente"].ToString();
                            citasModel.cedula = Int32.Parse(pacienteReader["ID_CITA"].ToString());
                            citasModel.centro = pacienteReader["CENTRO_SALUD"].ToString();
                            citasModel.especialidad = pacienteReader["ESPECIALIDAD"].ToString();
                            citasModel.fecha = pacienteReader["FECHA"].ToString();
                            citasModel.paciente = pacienteModel;
                            citasModel.medico = medicoTem;

                        }
                        connection.Close();
                    }
                }
            }
            return View(citasModel);
        }

        [HttpPost]
        public IActionResult actualizar(CitasModel citasModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);
                string sqlQuery = $"exec sp_actualizar_cita @param_ID='{citasModel.cedula}',@param_detalle='{citasModel.detalle}', @param_FECHA='{citasModel.fecha}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteReader();
                    connection.Close();
                }
                TempData["Success"] = "Se actualizó correctamente.";
                return RedirectToAction("lista");
            }
            return View();
        }
    }
}