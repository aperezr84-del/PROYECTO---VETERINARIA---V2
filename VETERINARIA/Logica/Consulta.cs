namespace SistemaControlClinicoVeterinario.Models
{
    public class Consulta
    {
        public int IdConsulta { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }
        public string Sintomas { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamiento { get; set; }
        public double Peso { get; set; }
        public int IdMascota { get; set; }
        public int IdVeterinario { get; set; }

        public Consulta(int idConsulta, DateTime fecha, string motivo, string sintomas,
                        string diagnostico, string tratamiento, double peso,
                        int idMascota, int idVeterinario)
        {
            IdConsulta = idConsulta;
            Fecha = fecha;
            Motivo = motivo;
            Sintomas = sintomas;
            Diagnostico = diagnostico;
            Tratamiento = tratamiento;
            Peso = peso;
            IdMascota = idMascota;
            IdVeterinario = idVeterinario;
        }
    }
}
