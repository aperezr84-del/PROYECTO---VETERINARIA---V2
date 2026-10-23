namespace SistemaControlClinicoVeterinario.Models
{
    public class Veterinario
    {
        public int IdVeterinario { get; set; }
        public string Nombre { get; set; }
        public string Especialidad { get; set; }

        public Veterinario(int idVeterinario, string nombre, string especialidad)
        {
            IdVeterinario = idVeterinario;
            Nombre = nombre;
            Especialidad = especialidad;
        }
    }
}
