using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B76711_B72933_ESPIC.Models
{
    public class VacunasModel
    {
        public int cedula { get; set; }

        public string nombre { get; set; }

        public string centro { get; set; }

        public string descripcion { get; set; }

        public string fecha { get; set; }

        public string fechasiquiente { get; set; }

        public PacienteModel paciente { get; set; }

        public MedicoModel medico { get; set; }

        public VacunasModel()
        {
            this.cedula = 0;
            this.nombre = "";
            this.descripcion = "";
            this.centro = "";
            this.fecha = "";
            this.fechasiquiente = "";
            this.paciente = null;
            this.medico = null;
        }

        public VacunasModel(int cedula,string nombre, string centro, string fecha, string fechasiquiente, PacienteModel paciente, MedicoModel medico)
        {
            this.cedula = cedula;
            this.nombre = nombre;
            this.centro = centro;
            this.fecha = fecha;
            this.fechasiquiente = fechasiquiente;
            this.paciente = paciente;
            this.medico = medico;
        }
    }
}
