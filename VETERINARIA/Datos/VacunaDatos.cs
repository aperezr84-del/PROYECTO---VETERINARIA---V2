using System.Data.SQLite;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Datos
{
    // CRUD de la tabla Vacunas
    public class VacunaDatos
    {
        // CREATE
        public bool Insertar(Vacuna v)
        {
            string sql = "INSERT INTO Vacunas (Nombre, FechaAplicacion, ProximaFecha, Observaciones, IdMascota) " +
                         "VALUES (@nombre, @aplicacion, @proxima, @observaciones, @idMascota)";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", v.Nombre);
                    cmd.Parameters.AddWithValue("@aplicacion", BaseDatos.FechaATexto(v.FechaAplicacion));
                    cmd.Parameters.AddWithValue("@proxima", BaseDatos.FechaATexto(v.ProximaFecha));
                    cmd.Parameters.AddWithValue("@observaciones", v.Observaciones);
                    cmd.Parameters.AddWithValue("@idMascota", v.IdMascota);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // READ (todos)
        public List<Vacuna> ObtenerTodos()
        {
            List<Vacuna> lista = new List<Vacuna>();
            string sql = "SELECT IdVacuna, Nombre, FechaAplicacion, ProximaFecha, Observaciones, IdMascota " +
                         "FROM Vacunas ORDER BY FechaAplicacion DESC";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Vacuna v = new Vacuna(
                            Convert.ToInt32(dr["IdVacuna"]),
                            dr["Nombre"].ToString(),
                            BaseDatos.TextoAFecha(dr["FechaAplicacion"]),
                            BaseDatos.TextoAFecha(dr["ProximaFecha"]),
                            dr["Observaciones"].ToString(),
                            Convert.ToInt32(dr["IdMascota"]));
                        lista.Add(v);
                    }
                }
            }
            return lista;
        }

        // READ (uno solo por id). Devuelve null si no existe
        public Vacuna BuscarPorId(int id)
        {
            foreach (Vacuna v in ObtenerTodos())
            {
                if (v.IdVacuna == id)
                {
                    return v;
                }
            }
            return null;
        }

        // UPDATE
        public bool Modificar(Vacuna v)
        {
            string sql = "UPDATE Vacunas SET Nombre = @nombre, FechaAplicacion = @aplicacion, " +
                         "ProximaFecha = @proxima, Observaciones = @observaciones, IdMascota = @idMascota " +
                         "WHERE IdVacuna = @id";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", v.Nombre);
                    cmd.Parameters.AddWithValue("@aplicacion", BaseDatos.FechaATexto(v.FechaAplicacion));
                    cmd.Parameters.AddWithValue("@proxima", BaseDatos.FechaATexto(v.ProximaFecha));
                    cmd.Parameters.AddWithValue("@observaciones", v.Observaciones);
                    cmd.Parameters.AddWithValue("@idMascota", v.IdMascota);
                    cmd.Parameters.AddWithValue("@id", v.IdVacuna);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // DELETE
        public bool Eliminar(int id)
        {
            string sql = "DELETE FROM Vacunas WHERE IdVacuna = @id";

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
