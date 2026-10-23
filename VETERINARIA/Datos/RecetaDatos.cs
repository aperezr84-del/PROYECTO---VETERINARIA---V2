using System.Data.SQLite;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Datos
{
    // CRUD de la tabla Recetas
    public class RecetaDatos
    {
        // CREATE
        public bool Insertar(Receta r)
        {
            string sql = "INSERT INTO Recetas (Fecha, Medicamento, Dosis, Frecuencia, Duracion, Indicaciones, IdConsulta) " +
                         "VALUES (@fecha, @medicamento, @dosis, @frecuencia, @duracion, @indicaciones, @idConsulta)";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@fecha", BaseDatos.FechaATexto(r.Fecha));
                    cmd.Parameters.AddWithValue("@medicamento", r.Medicamento);
                    cmd.Parameters.AddWithValue("@dosis", r.Dosis);
                    cmd.Parameters.AddWithValue("@frecuencia", r.Frecuencia);
                    cmd.Parameters.AddWithValue("@duracion", r.Duracion);
                    cmd.Parameters.AddWithValue("@indicaciones", r.Indicaciones);
                    cmd.Parameters.AddWithValue("@idConsulta", r.IdConsulta);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // READ (todos)
        public List<Receta> ObtenerTodos()
        {
            List<Receta> lista = new List<Receta>();
            string sql = "SELECT IdReceta, Fecha, Medicamento, Dosis, Frecuencia, Duracion, Indicaciones, IdConsulta " +
                         "FROM Recetas ORDER BY Fecha DESC";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Receta r = new Receta(
                            Convert.ToInt32(dr["IdReceta"]),
                            BaseDatos.TextoAFecha(dr["Fecha"]),
                            dr["Medicamento"].ToString(),
                            dr["Dosis"].ToString(),
                            dr["Frecuencia"].ToString(),
                            dr["Duracion"].ToString(),
                            dr["Indicaciones"].ToString(),
                            Convert.ToInt32(dr["IdConsulta"]));
                        lista.Add(r);
                    }
                }
            }
            return lista;
        }

        // READ (uno solo por id). Devuelve null si no existe
        public Receta BuscarPorId(int id)
        {
            foreach (Receta r in ObtenerTodos())
            {
                if (r.IdReceta == id)
                {
                    return r;
                }
            }
            return null;
        }

        // UPDATE
        public bool Modificar(Receta r)
        {
            string sql = "UPDATE Recetas SET Fecha = @fecha, Medicamento = @medicamento, Dosis = @dosis, " +
                         "Frecuencia = @frecuencia, Duracion = @duracion, Indicaciones = @indicaciones, " +
                         "IdConsulta = @idConsulta WHERE IdReceta = @id";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@fecha", BaseDatos.FechaATexto(r.Fecha));
                    cmd.Parameters.AddWithValue("@medicamento", r.Medicamento);
                    cmd.Parameters.AddWithValue("@dosis", r.Dosis);
                    cmd.Parameters.AddWithValue("@frecuencia", r.Frecuencia);
                    cmd.Parameters.AddWithValue("@duracion", r.Duracion);
                    cmd.Parameters.AddWithValue("@indicaciones", r.Indicaciones);
                    cmd.Parameters.AddWithValue("@idConsulta", r.IdConsulta);
                    cmd.Parameters.AddWithValue("@id", r.IdReceta);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // DELETE
        public bool Eliminar(int id)
        {
            string sql = "DELETE FROM Recetas WHERE IdReceta = @id";

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
