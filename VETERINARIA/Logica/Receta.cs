namespace SistemaControlClinicoVeterinario.Models
{
    public class Receta
    {
        public int IdReceta { get; set; }
        public DateTime Fecha { get; set; }
        public string Medicamento { get; set; }
        public string Dosis { get; set; }
        public string Frecuencia { get; set; }
        public string Duracion { get; set; }
        public string Indicaciones { get; set; }
        public int IdConsulta { get; set; }

        public Receta(int idReceta, DateTime fecha, string medicamento, string dosis,
                      string frecuencia, string duracion, string indicaciones, int idConsulta)
        {
            IdReceta = idReceta;
            Fecha = fecha;
            Medicamento = medicamento;
            Dosis = dosis;
            Frecuencia = frecuencia;
            Duracion = duracion;
            Indicaciones = indicaciones;
            IdConsulta = idConsulta;
        }
    }
}
