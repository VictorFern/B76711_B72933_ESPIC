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
    public class VacunasController : Controller
    {
        public IConfiguration Configuration { get; }

        public VacunasController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IActionResult Index()
        {
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
            ViewBag.Paciente = paciente;
            return View();
        }
        [HttpPost]
        public IActionResult Registrar(VacunasModel vacunasModel)
        {
            int valor = (int)HttpContext.Session.GetInt32("variableInt");
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);
                string sqlQuery = $"exec sp_INSERTAR_VACUNAS @param_CEDULA_DR='{valor}', @param_CEDULA='{vacunasModel.cedula}', @param_NOMBRE='{vacunasModel.nombre}',@param_CENTRO='{vacunasModel.centro}' ,@param_DESCRIPCION='{vacunasModel.descripcion}', @param_FECHA='{vacunasModel.fecha}',@param_FECHAS='{vacunasModel.fechasiquiente}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteReader();
                    connection.Close();
                }
            }
            return RedirectToAction("Registrar");
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
        public IActionResult listaVacunas(PacienteModel paciente)
        {
            List<object> lista = new List<object>();
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec sp_vacunas @param_CEDULA= '{paciente.cedula}'";
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
                            VacunasModel vacunasModel = new VacunasModel(Int32.Parse(pacienteReader["ID_VACUNA"].ToString())
                                                                   , pacienteReader["VACUNA"].ToString()
                                                                   ,pacienteReader["CENTRO_SALUD"].ToString()
                                                                 , pacienteReader["FECHA_VACUNA"].ToString()
                                                                , pacienteReader["FECHA_SIQUIENTE"].ToString()
                                                                , pacienteModel, medicoTem);

                            lista.Add(vacunasModel);
                        }// while
                        connection.Close();
                    }

                }

            }
            ViewBag.Lista = lista;
            return View();
        }

        public IActionResult actualizar(int id)
        {
            List<object> lista = new List<object>();
            PacienteModel pacienteModel = new PacienteModel();
            VacunasModel vacunasModel = new VacunasModel();
            MedicoModel medicoTem = new MedicoModel();
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec sp_vacunas_id @param_ID= '{id}'";
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
                            vacunasModel.cedula = Int32.Parse(pacienteReader["ID_VACUNA"].ToString());
                            vacunasModel.nombre = pacienteReader["VACUNA"].ToString();
                            vacunasModel.centro = pacienteReader["CENTRO_SALUD"].ToString();
                            vacunasModel.descripcion = pacienteReader["DESCRIPCION"].ToString();
                            vacunasModel.fecha = pacienteReader["FECHA_VACUNA"].ToString();
                            vacunasModel.fechasiquiente = pacienteReader["FECHA_SIQUIENTE"].ToString();
                            vacunasModel.paciente = pacienteModel;
                            vacunasModel.medico = medicoTem;

                            //lista.Add(vacunasModel);
                        }// while
                        connection.Close();
                    }

                }

            }
            //ViewData["lista"] = lista;
            return View(vacunasModel);
        }
        [HttpPost]
        public IActionResult actualizar(VacunasModel vacunasModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);
                string sqlQuery = $"exec sp_actualizar_vacunas @param_ID='{vacunasModel.cedula}', @param_NOMBRE='{vacunasModel.nombre}', @param_DESCRIPCION='{vacunasModel.descripcion}', @param_FECHA='{vacunasModel.fecha}',@param_FECHAS='{vacunasModel.fechasiquiente}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteReader();
                    connection.Close();
                }
            }
            return RedirectToAction("lista");
        }

        public IActionResult detalleEliminar(int id)
        {
            List<object> lista = new List<object>();
            PacienteModel pacienteModel = new PacienteModel();
            VacunasModel vacunasModel = new VacunasModel();
            MedicoModel medicoTem = new MedicoModel();
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec sp_vacunas_id @param_ID= '{id}'";
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
                            vacunasModel.cedula = Int32.Parse(pacienteReader["ID_VACUNA"].ToString());
                            vacunasModel.nombre = pacienteReader["VACUNA"].ToString();
                            vacunasModel.centro = pacienteReader["CENTRO_SALUD"].ToString();
                            vacunasModel.descripcion = pacienteReader["DESCRIPCION"].ToString();
                            vacunasModel.fecha = pacienteReader["FECHA_VACUNA"].ToString();
                            vacunasModel.fechasiquiente = pacienteReader["FECHA_SIQUIENTE"].ToString();
                            vacunasModel.paciente = pacienteModel;
                            vacunasModel.medico = medicoTem;

                            //lista.Add(vacunasModel);
                        }// while
                        connection.Close();
                    }

                }

            }
            //ViewData["lista"] = lista;
            return View(vacunasModel);
        }

        [HttpPost]
        public IActionResult Eliminar(VacunasModel vacunasModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);
                string sqlQuery = $"exec sp_eliminar_vacunas @param_ID='{vacunasModel.cedula}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteReader();
                    connection.Close();
                }
            }
            return RedirectToAction("lista");
        }
    }
}
