using System.Data.SQLite;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Datos
{
    // CRUD de la tabla Consultas
    public class ConsultaDatos
    {
        // CREATE
        public bool Insertar(Consulta c)
        {
            string sql = "INSERT INTO Consultas (Fecha, Motivo, Sintomas, Diagnostico, Tratamiento, Peso, IdMascota, IdVeterinario) " +
                         "VALUES (@fecha, @motivo, @sintomas, @diagnostico, @tratamiento, @peso, @idMascota, @idVeterinario)";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@fecha", BaseDatos.FechaATexto(c.Fecha));
                    cmd.Parameters.AddWithValue("@motivo", c.Motivo);
                    cmd.Parameters.AddWithValue("@sintomas", c.Sintomas);
                    cmd.Parameters.AddWithValue("@diagnostico", c.Diagnostico);
                    cmd.Parameters.AddWithValue("@tratamiento", c.Tratamiento);
                    cmd.Parameters.AddWithValue("@peso", c.Peso);
                    cmd.Parameters.AddWithValue("@idMascota", c.IdMascota);
                    cmd.Parameters.AddWithValue("@idVeterinario", c.IdVeterinario);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // READ (todos)
        public List<Consulta> ObtenerTodos()
        {
            List<Consulta> lista = new List<Consulta>();
            string sql = "SELECT IdConsulta, Fecha, Motivo, Sintomas, Diagnostico, Tratamiento, Peso, IdMascota, IdVeterinario " +
                         "FROM Consultas ORDER BY Fecha DESC";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Consulta c = new Consulta(
                            Convert.ToInt32(dr["IdConsulta"]),
                            BaseDatos.TextoAFecha(dr["Fecha"]),
                            dr["Motivo"].ToString(),
                            dr["Sintomas"].ToString(),
                            dr["Diagnostico"].ToString(),
                            dr["Tratamiento"].ToString(),
                            Convert.ToDouble(dr["Peso"]),
                            Convert.ToInt32(dr["IdMascota"]),
                            Convert.ToInt32(dr["IdVeterinario"]));
                        lista.Add(c);
                    }
                }
            }
            return lista;
        }

        // READ (uno solo por id). Devuelve null si no existe
        public Consulta BuscarPorId(int id)
        {
            foreach (Consulta c in ObtenerTodos())
            {
                if (c.IdConsulta == id)
                {
                    return c;
                }
            }
            return null;
        }

        // UPDATE
        public bool Modificar(Consulta c)
        {
            string sql = "UPDATE Consultas SET Fecha = @fecha, Motivo = @motivo, Sintomas = @sintomas, " +
                         "Diagnostico = @diagnostico, Tratamiento = @tratamiento, Peso = @peso, " +
                         "IdMascota = @idMascota, IdVeterinario = @idVeterinario WHERE IdConsulta = @id";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@fecha", BaseDatos.FechaATexto(c.Fecha));
                    cmd.Parameters.AddWithValue("@motivo", c.Motivo);
                    cmd.Parameters.AddWithValue("@sintomas", c.Sintomas);
                    cmd.Parameters.AddWithValue("@diagnostico", c.Diagnostico);
                    cmd.Parameters.AddWithValue("@tratamiento", c.Tratamiento);
                    cmd.Parameters.AddWithValue("@peso", c.Peso);
                    cmd.Parameters.AddWithValue("@idMascota", c.IdMascota);
                    cmd.Parameters.AddWithValue("@idVeterinario", c.IdVeterinario);
                    cmd.Parameters.AddWithValue("@id", c.IdConsulta);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // DELETE
        public bool Eliminar(int id)
        {
            string sql = "DELETE FROM Consultas WHERE IdConsulta = @id";

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
