namespace SistemaControlClinicoVeterinario.Models
{
    public class Vacuna
    {
        public int IdVacuna { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaAplicacion { get; set; }
        public DateTime ProximaFecha { get; set; }
        public string Observaciones { get; set; }
        public int IdMascota { get; set; }

        public Vacuna(int idVacuna, string nombre, DateTime fechaAplicacion,
                      DateTime proximaFecha, string observaciones, int idMascota)
        {
            IdVacuna = idVacuna;
            Nombre = nombre;
            FechaAplicacion = fechaAplicacion;
            ProximaFecha = proximaFecha;
            Observaciones = observaciones;
            IdMascota = idMascota;
        }
    }
}
