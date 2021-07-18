using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B76711_B72933_ESPIC.Models
{
    public class AlergiaModel
    {
        public int cedula { get; set; }

        public string nombre { get; set; }

        public string centro { get; set; }

        public string medicina { get; set; }

        public string descripcion { get; set; }

        public string fecha { get; set; }

        public PacienteModel paciente { get; set; }

        public MedicoModel medico { get; set; }

        public AlergiaModel()
        {
            this.cedula = 0;
            this.nombre = "";
            this.centro = "";
            this.medicina = "";
            this.descripcion = "";
            this.fecha = "";
            this.paciente = null;
            this.medico = null;
        }

        public AlergiaModel(int cedula, string nombre ,string centro,string medicina,string fecha, PacienteModel paciente, MedicoModel medico)
        {
            this.cedula = cedula;
            this.nombre = nombre;
            this.centro = centro;
            this.medicina = medicina;
            this.fecha = fecha;
            this.paciente = paciente;
            this.medico = medico;
        }
    }
}
