using System.Data.SQLite;
using System.Globalization;

namespace SistemaControlClinicoVeterinario.Datos
{
    // Esta clase se encarga de la conexion con SQLite y de crear las tablas del sistema.
    public static class BaseDatos
    {
        // El archivo de la base de datos se guarda junto al programa (.exe)
        private static string rutaArchivo = Path.Combine(AppContext.BaseDirectory, "veterinaria.db");

        // Foreign Keys=True activa las llaves foraneas en SQLite
        private static string cadenaConexion = "Data Source=" + rutaArchivo + ";Version=3;Foreign Keys=True;";

        public static SQLiteConnection ObtenerConexion()
        {
            return new SQLiteConnection(cadenaConexion);
        }

        // Crea todas las tablas de soporte del sistema (si no existen)
        public static void CrearBaseDeDatos()
        {
            string tablaClientes =
                "CREATE TABLE IF NOT EXISTS Clientes (" +
                "IdCliente INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "Nombre TEXT NOT NULL, " +
                "Telefono TEXT, " +
                "Direccion TEXT, " +
                "Correo TEXT);";

            string tablaVeterinarios =
                "CREATE TABLE IF NOT EXISTS Veterinarios (" +
                "IdVeterinario INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "Nombre TEXT NOT NULL, " +
                "Especialidad TEXT);";

            string tablaMascotas =
                "CREATE TABLE IF NOT EXISTS Mascotas (" +
                "IdMascota INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "Nombre TEXT NOT NULL, " +
                "Especie TEXT, " +
                "Raza TEXT, " +
                "Sexo TEXT, " +
                "FechaNacimiento TEXT, " +
                "Peso REAL, " +
                "IdCliente INTEGER NOT NULL, " +
                "FOREIGN KEY (IdCliente) REFERENCES Clientes(IdCliente));";

            string tablaCitas =
                "CREATE TABLE IF NOT EXISTS Citas (" +
                "IdCita INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "Fecha TEXT NOT NULL, " +
                "Motivo TEXT, " +
                "Estado TEXT, " +
                "IdMascota INTEGER NOT NULL, " +
                "IdVeterinario INTEGER NOT NULL, " +
                "FOREIGN KEY (IdMascota) REFERENCES Mascotas(IdMascota), " +
                "FOREIGN KEY (IdVeterinario) REFERENCES Veterinarios(IdVeterinario));";

            string tablaConsultas =
                "CREATE TABLE IF NOT EXISTS Consultas (" +
                "IdConsulta INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "Fecha TEXT NOT NULL, " +
                "Motivo TEXT, " +
                "Sintomas TEXT, " +
                "Diagnostico TEXT, " +
                "Tratamiento TEXT, " +
                "Peso REAL, " +
                "IdMascota INTEGER NOT NULL, " +
                "IdVeterinario INTEGER NOT NULL, " +
                "FOREIGN KEY (IdMascota) REFERENCES Mascotas(IdMascota), " +
                "FOREIGN KEY (IdVeterinario) REFERENCES Veterinarios(IdVeterinario));";

            string tablaVacunas =
                "CREATE TABLE IF NOT EXISTS Vacunas (" +
                "IdVacuna INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "Nombre TEXT NOT NULL, " +
                "FechaAplicacion TEXT NOT NULL, " +
                "ProximaFecha TEXT, " +
                "Observaciones TEXT, " +
                "IdMascota INTEGER NOT NULL, " +
                "FOREIGN KEY (IdMascota) REFERENCES Mascotas(IdMascota));";

            string tablaRecetas =
                "CREATE TABLE IF NOT EXISTS Recetas (" +
                "IdReceta INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "Fecha TEXT NOT NULL, " +
                "Medicamento TEXT NOT NULL, " +
                "Dosis TEXT, " +
                "Frecuencia TEXT, " +
                "Duracion TEXT, " +
                "Indicaciones TEXT, " +
                "IdConsulta INTEGER NOT NULL, " +
                "FOREIGN KEY (IdConsulta) REFERENCES Consultas(IdConsulta));";

            // El orden importa: primero las tablas que no dependen de otras
            string[] tablas = { tablaClientes, tablaVeterinarios, tablaMascotas,
                                tablaCitas, tablaConsultas, tablaVacunas, tablaRecetas };

            using (SQLiteConnection con = ObtenerConexion())
            {
                con.Open();

                foreach (string sql in tablas)
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // Si no hay veterinarios, se agregan los dos de ejemplo (igual que en la version 1)
                using (SQLiteCommand contar = new SQLiteCommand("SELECT COUNT(*) FROM Veterinarios", con))
                {
                    int total = Convert.ToInt32(contar.ExecuteScalar());

                    if (total == 0)
                    {
                        string insertar = "INSERT INTO Veterinarios (Nombre, Especialidad) VALUES " +
                                          "('Dra. Ana López', 'Medicina general'), " +
                                          "('Dr. Carlos Pérez', 'Cirugía veterinaria');";
                        using (SQLiteCommand cmd = new SQLiteCommand(insertar, con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        // SQLite no tiene tipo fecha, entonces las guardamos como texto con este formato
        public static string FechaATexto(DateTime fecha)
        {
            return fecha.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }

        public static DateTime TextoAFecha(object valor)
        {
            return DateTime.ParseExact(valor.ToString(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }
    }
}
