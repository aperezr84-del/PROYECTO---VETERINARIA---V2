using SistemaControlClinicoVeterinario.Datos;
using SistemaControlClinicoVeterinario.Formularios;

namespace SistemaControlClinicoVeterinario
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                // Crea el archivo veterinaria.db y las tablas si todavia no existen
                BaseDatos.CrearBaseDeDatos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo crear la base de datos: " + ex.Message);
                return;
            }

            Application.Run(new FrmPrincipal());
        }
    }
}
