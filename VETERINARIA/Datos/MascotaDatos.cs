using System.Data.SQLite;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Datos
{
    // CRUD de la tabla Mascotas
    public class MascotaDatos
    {
        // CREATE
        public bool Insertar(Mascota m)
        {
            string sql = "INSERT INTO Mascotas (Nombre, Especie, Raza, Sexo, FechaNacimiento, Peso, IdCliente) " +
                         "VALUES (@nombre, @especie, @raza, @sexo, @fecha, @peso, @idCliente)";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", m.Nombre);
                    cmd.Parameters.AddWithValue("@especie", m.Especie);
                    cmd.Parameters.AddWithValue("@raza", m.Raza);
                    cmd.Parameters.AddWithValue("@sexo", m.Sexo);
                    cmd.Parameters.AddWithValue("@fecha", BaseDatos.FechaATexto(m.FechaNacimiento));
                    cmd.Parameters.AddWithValue("@peso", m.Peso);
                    cmd.Parameters.AddWithValue("@idCliente", m.IdCliente);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // READ (todos)
        public List<Mascota> ObtenerTodos()
        {
            List<Mascota> lista = new List<Mascota>();
            string sql = "SELECT IdMascota, Nombre, Especie, Raza, Sexo, FechaNacimiento, Peso, IdCliente " +
                         "FROM Mascotas ORDER BY IdMascota";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Mascota m = new Mascota(
                            Convert.ToInt32(dr["IdMascota"]),
                            dr["Nombre"].ToString(),
                            dr["Especie"].ToString(),
                            dr["Raza"].ToString(),
                            dr["Sexo"].ToString(),
                            BaseDatos.TextoAFecha(dr["FechaNacimiento"]),
                            Convert.ToDouble(dr["Peso"]),
                            Convert.ToInt32(dr["IdCliente"]));
                        lista.Add(m);
                    }
                }
            }
            return lista;
        }

        // READ (uno solo por id). Devuelve null si no existe
        public Mascota BuscarPorId(int id)
        {
            foreach (Mascota m in ObtenerTodos())
            {
                if (m.IdMascota == id)
                {
                    return m;
                }
            }
            return null;
        }

        // UPDATE
        public bool Modificar(Mascota m)
        {
            string sql = "UPDATE Mascotas SET Nombre = @nombre, Especie = @especie, Raza = @raza, " +
                         "Sexo = @sexo, FechaNacimiento = @fecha, Peso = @peso, IdCliente = @idCliente " +
                         "WHERE IdMascota = @id";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", m.Nombre);
                    cmd.Parameters.AddWithValue("@especie", m.Especie);
                    cmd.Parameters.AddWithValue("@raza", m.Raza);
                    cmd.Parameters.AddWithValue("@sexo", m.Sexo);
                    cmd.Parameters.AddWithValue("@fecha", BaseDatos.FechaATexto(m.FechaNacimiento));
                    cmd.Parameters.AddWithValue("@peso", m.Peso);
                    cmd.Parameters.AddWithValue("@idCliente", m.IdCliente);
                    cmd.Parameters.AddWithValue("@id", m.IdMascota);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // DELETE
        public bool Eliminar(int id)
        {
            string sql = "DELETE FROM Mascotas WHERE IdMascota = @id";

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
