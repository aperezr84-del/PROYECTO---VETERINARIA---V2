using System.Data.SQLite;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Datos
{
    // CRUD de la tabla Citas
    public class CitaDatos
    {
        // CREATE
        public bool Insertar(Cita c)
        {
            string sql = "INSERT INTO Citas (Fecha, Motivo, Estado, IdMascota, IdVeterinario) " +
                         "VALUES (@fecha, @motivo, @estado, @idMascota, @idVeterinario)";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@fecha", BaseDatos.FechaATexto(c.Fecha));
                    cmd.Parameters.AddWithValue("@motivo", c.Motivo);
                    cmd.Parameters.AddWithValue("@estado", c.Estado);
                    cmd.Parameters.AddWithValue("@idMascota", c.IdMascota);
                    cmd.Parameters.AddWithValue("@idVeterinario", c.IdVeterinario);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // READ (todos)
        public List<Cita> ObtenerTodos()
        {
            List<Cita> lista = new List<Cita>();
            string sql = "SELECT IdCita, Fecha, Motivo, Estado, IdMascota, IdVeterinario " +
                         "FROM Citas ORDER BY Fecha";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Cita c = new Cita(
                            Convert.ToInt32(dr["IdCita"]),
                            BaseDatos.TextoAFecha(dr["Fecha"]),
                            dr["Motivo"].ToString(),
                            dr["Estado"].ToString(),
                            Convert.ToInt32(dr["IdMascota"]),
                            Convert.ToInt32(dr["IdVeterinario"]));
                        lista.Add(c);
                    }
                }
            }
            return lista;
        }

        // READ (uno solo por id). Devuelve null si no existe
        public Cita BuscarPorId(int id)
        {
            foreach (Cita c in ObtenerTodos())
            {
                if (c.IdCita == id)
                {
                    return c;
                }
            }
            return null;
        }

        // UPDATE
        public bool Modificar(Cita c)
        {
            string sql = "UPDATE Citas SET Fecha = @fecha, Motivo = @motivo, Estado = @estado, " +
                         "IdMascota = @idMascota, IdVeterinario = @idVeterinario WHERE IdCita = @id";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@fecha", BaseDatos.FechaATexto(c.Fecha));
                    cmd.Parameters.AddWithValue("@motivo", c.Motivo);
                    cmd.Parameters.AddWithValue("@estado", c.Estado);
                    cmd.Parameters.AddWithValue("@idMascota", c.IdMascota);
                    cmd.Parameters.AddWithValue("@idVeterinario", c.IdVeterinario);
                    cmd.Parameters.AddWithValue("@id", c.IdCita);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // DELETE
        public bool Eliminar(int id)
        {
            string sql = "DELETE FROM Citas WHERE IdCita = @id";

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
