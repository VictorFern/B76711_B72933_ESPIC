using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B76711_B72933_ESPIC.Models
{
    public class CitasModel
    {
        public int cedula { get; set; }
        public string centro { get; set; }

        public string fecha { get; set; }

        public string hora { get; set; }

        public string especialidad { get; set; }

        public string detalle { get; set; }

        public PacienteModel paciente { get; set; }

        public MedicoModel medico { get; set; }

        public CitasModel()
        {
            this.cedula = 0;
            this.centro = "";
            this.fecha = "";
            this.hora = "";
            this.especialidad = "";
            this.detalle = "";
            this.paciente = null;
            this.medico = null;
        }

        public CitasModel(int cedula, string centro, string fecha, string especialidad, string detalle, PacienteModel paciente, MedicoModel medico)
        {
            this.cedula = cedula;
            this.centro = centro;
            this.fecha = fecha;
            this.especialidad = especialidad;
            this.detalle = detalle;
            this.paciente = paciente;
            this.medico = medico;
        }
    }
}
