using System.Data.SQLite;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Datos
{
    // CRUD de la tabla Clientes
    public class ClienteDatos
    {
        // CREATE
        public bool Insertar(Cliente c)
        {
            string sql = "INSERT INTO Clientes (Nombre, Telefono, Direccion, Correo) " +
                         "VALUES (@nombre, @telefono, @direccion, @correo)";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", c.Nombre);
                    cmd.Parameters.AddWithValue("@telefono", c.Telefono);
                    cmd.Parameters.AddWithValue("@direccion", c.Direccion);
                    cmd.Parameters.AddWithValue("@correo", c.Correo);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // READ (todos)
        public List<Cliente> ObtenerTodos()
        {
            List<Cliente> lista = new List<Cliente>();
            string sql = "SELECT IdCliente, Nombre, Telefono, Direccion, Correo FROM Clientes ORDER BY IdCliente";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                using (SQLiteDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Cliente c = new Cliente(
                            Convert.ToInt32(dr["IdCliente"]),
                            dr["Nombre"].ToString(),
                            dr["Telefono"].ToString(),
                            dr["Direccion"].ToString(),
                            dr["Correo"].ToString());
                        lista.Add(c);
                    }
                }
            }
            return lista;
        }

        // READ (uno solo por id). Devuelve null si no existe
        public Cliente BuscarPorId(int id)
        {
            foreach (Cliente c in ObtenerTodos())
            {
                if (c.IdCliente == id)
                {
                    return c;
                }
            }
            return null;
        }

        // UPDATE
        public bool Modificar(Cliente c)
        {
            string sql = "UPDATE Clientes SET Nombre = @nombre, Telefono = @telefono, " +
                         "Direccion = @direccion, Correo = @correo WHERE IdCliente = @id";

            using (SQLiteConnection con = BaseDatos.ObtenerConexion())
            {
                con.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@nombre", c.Nombre);
                    cmd.Parameters.AddWithValue("@telefono", c.Telefono);
                    cmd.Parameters.AddWithValue("@direccion", c.Direccion);
                    cmd.Parameters.AddWithValue("@correo", c.Correo);
                    cmd.Parameters.AddWithValue("@id", c.IdCliente);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // DELETE
        public bool Eliminar(int id)
        {
            string sql = "DELETE FROM Clientes WHERE IdCliente = @id";

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
