using System.Data.SQLite;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Datos
{
    // CRUD de la tabla Veterinarios
    public class VeterinarioDatos
    {
        // CREATE
        public bool Insertar(Veterinario v)
        {
            string sql = "INSERT INTO Veterinarios (Nombre, Especialidad) VALUES (@nombre, @especialidad)";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", v.Nombre);
                    cmd.Parameters.AddWithValue("@especialidad", v.Especialidad);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // READ (todos)
        public List<Veterinario> ObtenerTodos()
        {
            List<Veterinario> lista = new List<Veterinario>();
            string sql = "SELECT IdVeterinario, Nombre, Especialidad FROM Veterinarios ORDER BY IdVeterinario";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Veterinario v = new Veterinario(
                            Convert.ToInt32(dr["IdVeterinario"]),
                            dr["Nombre"].ToString(),
                            dr["Especialidad"].ToString());
                        lista.Add(v);
                    }
                }
            }
            return lista;
        }

        // READ (uno solo por id). Devuelve null si no existe
        public Veterinario BuscarPorId(int id)
        {
            foreach (Veterinario v in ObtenerTodos())
            {
                if (v.IdVeterinario == id)
                {
                    return v;
                }
            }
            return null;
        }

        // UPDATE
        public bool Modificar(Veterinario v)
        {
            string sql = "UPDATE Veterinarios SET Nombre = @nombre, Especialidad = @especialidad " +
                         "WHERE IdVeterinario = @id";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", v.Nombre);
                    cmd.Parameters.AddWithValue("@especialidad", v.Especialidad);
                    cmd.Parameters.AddWithValue("@id", v.IdVeterinario);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // DELETE
        public bool Eliminar(int id)
        {
            string sql = "DELETE FROM Veterinarios WHERE IdVeterinario = @id";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
