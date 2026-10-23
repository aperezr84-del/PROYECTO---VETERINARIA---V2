using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Services
{
    public class ClinicaVeterinaria
    {
        public List<Cliente> Clientes { get; } = new List<Cliente>();
        public List<Mascota> Mascotas { get; } = new List<Mascota>();
        public List<Veterinario> Veterinarios { get; } = new List<Veterinario>();
        public List<Cita> Citas { get; } = new List<Cita>();
        public List<Consulta> Consultas { get; } = new List<Consulta>();
        public List<Vacuna> Vacunas { get; } = new List<Vacuna>();
        public List<Receta> Recetas { get; } = new List<Receta>();

        public ClinicaVeterinaria()
        {
            // Veterinarios de ejemplo para poder usar el sistema desde el inicio.
            Veterinarios.Add(new Veterinario(1, "Dra. Ana López", "Medicina general"));
            Veterinarios.Add(new Veterinario(2, "Dr. Carlos Pérez", "Cirugía veterinaria"));
        }

        public Cliente RegistrarCliente(string nombre, string telefono, string direccion, string correo)
        {
            int id = Clientes.Count + 1;
            Cliente cliente = new Cliente(id, nombre, telefono, direccion, correo);
            Clientes.Add(cliente);
            return cliente;
        }

        public Mascota RegistrarMascota(string nombre, string especie, string raza, string sexo,
                                        DateTime fechaNacimiento, double peso, int idCliente)
        {
            int id = Mascotas.Count + 1;
            Mascota mascota = new Mascota(id, nombre, especie, raza, sexo, fechaNacimiento, peso, idCliente);
            Mascotas.Add(mascota);
            return mascota;
        }

        public bool ExisteCliente(int idCliente)
        {
            return Clientes.Any(c => c.IdCliente == idCliente);
        }

        public bool ExisteMascota(int idMascota)
        {
            return Mascotas.Any(m => m.IdMascota == idMascota);
        }

        public bool ExisteVeterinario(int idVeterinario)
        {
            return Veterinarios.Any(v => v.IdVeterinario == idVeterinario);
        }

        public Mascota? BuscarMascota(int idMascota)
        {
            return Mascotas.FirstOrDefault(m => m.IdMascota == idMascota);
        }

        public Cliente? BuscarCliente(int idCliente)
        {
            return Clientes.FirstOrDefault(c => c.IdCliente == idCliente);
        }

        public Veterinario? BuscarVeterinario(int idVeterinario)
        {
            return Veterinarios.FirstOrDefault(v => v.IdVeterinario == idVeterinario);
        }

        public Cita AgendarCita(DateTime fecha, string motivo, int idMascota, int idVeterinario)
        {
            int id = Citas.Count + 1;
            Cita cita = new Cita(id, fecha, motivo, "Pendiente", idMascota, idVeterinario);
            Citas.Add(cita);
            return cita;
        }

        public Consulta RegistrarConsulta(DateTime fecha, string motivo, string sintomas,
                                          string diagnostico, string tratamiento, double peso,
                                          int idMascota, int idVeterinario)
        {
            int id = Consultas.Count + 1;
            Consulta consulta = new Consulta(id, fecha, motivo, sintomas, diagnostico,
                                             tratamiento, peso, idMascota, idVeterinario);
            Consultas.Add(consulta);
            return consulta;
        }

        public Vacuna RegistrarVacuna(string nombre, DateTime fechaAplicacion, DateTime proximaFecha,
                                      string observaciones, int idMascota)
        {
            int id = Vacunas.Count + 1;
            Vacuna vacuna = new Vacuna(id, nombre, fechaAplicacion, proximaFecha, observaciones, idMascota);
            Vacunas.Add(vacuna);
            return vacuna;
        }

        public Receta EmitirReceta(DateTime fecha, string medicamento, string dosis,
                                   string frecuencia, string duracion, string indicaciones,
                                   int idConsulta)
        {
            int id = Recetas.Count + 1;
            Receta receta = new Receta(id, fecha, medicamento, dosis, frecuencia,
                                       duracion, indicaciones, idConsulta);
            Recetas.Add(receta);
            return receta;
        }

        public List<Consulta> ObtenerConsultasMascota(int idMascota)
        {
            return Consultas.Where(c => c.IdMascota == idMascota)
                            .OrderByDescending(c => c.Fecha)
                            .ToList();
        }

        public List<Vacuna> ObtenerVacunasMascota(int idMascota)
        {
            return Vacunas.Where(v => v.IdMascota == idMascota)
                          .OrderByDescending(v => v.FechaAplicacion)
                          .ToList();
        }

        public List<Cita> ObtenerCitasMascota(int idMascota)
        {
            return Citas.Where(c => c.IdMascota == idMascota)
                        .OrderBy(c => c.Fecha)
                        .ToList();
        }
    }
}
