namespace SistemaControlClinicoVeterinario.Models
{
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Correo { get; set; }

        public Cliente(int idCliente, string nombre, string telefono, string direccion, string correo)
        {
            IdCliente = idCliente;
            Nombre = nombre;
            Telefono = telefono;
            Direccion = direccion;
            Correo = correo;
        }
    }
}
