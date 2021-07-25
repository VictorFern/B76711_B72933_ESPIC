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
    public class AlergiaController : Controller
    {
        public IConfiguration Configuration { get; }

        public AlergiaController(IConfiguration configuration)
        {
            Configuration = configuration;
        } // constructor

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
        public IActionResult Registrar(AlergiaModel alergiaModel)
        {
            int valor = (int)HttpContext.Session.GetInt32("variableInt");
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);
                string sqlQuery = $"exec sp_INSERTAR_ALERGIAS @param_CEDULA_DR='{valor}',@param_CEDULA='{alergiaModel.cedula}',@param_NOMBRE='{alergiaModel.nombre}',@param_CENTRO='{alergiaModel.centro}',@param_MEDICAMENTO='{alergiaModel.medicina}',@param_DESCRIPCION='{alergiaModel.descripcion}',@param_FECHA='{alergiaModel.fecha}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteReader();
                    connection.Close();
                }
                TempData["Success"] = "Se registro corectamente.";
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
        public IActionResult listaAlergia(PacienteModel paciente)
        {
            List<object> lista = new List<object>();
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec sp_lista_alergia @param_CEDULA= '{paciente.cedula}'";
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
                            AlergiaModel alergiaTem = new AlergiaModel(Int32.Parse(pacienteReader["ID_ALERGIA"].ToString())
                                                                 , pacienteReader["ALERGIA"].ToString()
                                                                 , pacienteReader["CENTRO_SALUD"].ToString()
                                                                 ,pacienteReader["MEDICAMENTO"].ToString()
                                                                 , pacienteReader["FECHA"].ToString()
                                                                , pacienteModel, medicoTem);

                            lista.Add(alergiaTem);
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
            AlergiaModel alergiaModel = new AlergiaModel();
            MedicoModel medicoTem = new MedicoModel();

            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString)) 
                { 

                    string sqlQuery = $"exec sp_alergia @param_ID= '{id}'";
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
                            alergiaModel.cedula = Int32.Parse(pacienteReader["ID_ALERGIA"].ToString());
                            alergiaModel.nombre = pacienteReader["ALERGIA"].ToString();
                            alergiaModel.centro = pacienteReader["CENTRO_SALUD"].ToString();
                            alergiaModel.descripcion = pacienteReader["DESCRIPCION"].ToString();
                            alergiaModel.fecha = pacienteReader["FECHA"].ToString();
                            alergiaModel.paciente = pacienteModel;
                            alergiaModel.medico = medicoTem;

                        }
                        connection.Close();
                    }
                }
            }
           return View(alergiaModel);
        }

        [HttpPost]
        public IActionResult actualizar(AlergiaModel alergiaModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);
                string sqlQuery = $"exec sp_actualizar_alergia @param_ID='{alergiaModel.cedula}', @param_NOMBRE='{alergiaModel.nombre}',@param_MEDICAMENTO='{alergiaModel.medicina}' ,@param_DESCRIPCION='{alergiaModel.descripcion}', @param_FECHA='{alergiaModel.fecha}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteReader();
                    connection.Close();
                }
                TempData["Success"] = "Se actualizo corectamente.";
                return RedirectToAction("lista");
            }
            return View();
        }

        public IActionResult detalleEliminar(int id)
        {
            List<object> lista = new List<object>();
            PacienteModel pacienteModel = new PacienteModel();
            AlergiaModel vacunasModel = new AlergiaModel();
            MedicoModel medicoTem = new MedicoModel();
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlQuery = $"exec sp_alergia @param_ID= '{id}'";
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
                            vacunasModel.cedula = Int32.Parse(pacienteReader["ID_ALERGIA"].ToString());
                            vacunasModel.nombre = pacienteReader["ALERGIA"].ToString();
                            vacunasModel.centro = pacienteReader["CENTRO_SALUD"].ToString();
                            vacunasModel.medicina = pacienteReader["MEDICAMENTO"].ToString();
                            vacunasModel.descripcion = pacienteReader["DESCRIPCION"].ToString();
                            vacunasModel.fecha = pacienteReader["FECHA"].ToString();
                            vacunasModel.paciente = pacienteModel;
                            vacunasModel.medico = medicoTem;

                            //lista.Add(vacunasModel);
                        }// while
                        connection.Close();
                    }
                }
            }
            return View(vacunasModel);
        }

        [HttpPost]
        public IActionResult Eliminar(AlergiaModel alergiaModel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DB_Connection"];
                var connection = new SqlConnection(connectionString);
                string sqlQuery = $"exec sp_eliminar_alergia @param_ID='{alergiaModel.cedula}'";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    command.ExecuteReader();
                    connection.Close();
                }
                TempData["Success"] = "Se elimino corectamente.";
                return RedirectToAction("lista");
            }
            return View();
        }
    }
}
