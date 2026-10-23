namespace SistemaControlClinicoVeterinario.Models
{
    public class Cita
    {
        public int IdCita { get; set; }
        public DateTime Fecha { get; set; }
        public string Motivo { get; set; }
        public string Estado { get; set; }
        public int IdMascota { get; set; }
        public int IdVeterinario { get; set; }

        public Cita(int idCita, DateTime fecha, string motivo, string estado,
                    int idMascota, int idVeterinario)
        {
            IdCita = idCita;
            Fecha = fecha;
            Motivo = motivo;
            Estado = estado;
            IdMascota = idMascota;
            IdVeterinario = idVeterinario;
        }
    }
}
