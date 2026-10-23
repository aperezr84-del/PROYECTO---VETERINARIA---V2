using SistemaControlClinicoVeterinario.Datos;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Formularios
{
    public class FrmClientes : Form
    {
        private ClienteDatos datos = new ClienteDatos();
        private int idSeleccionado = 0;   // 0 = no hay ningun registro seleccionado

        private TextBox txtNombre;
        private TextBox txtTelefono;
        private TextBox txtDireccion;
        private TextBox txtCorreo;
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvClientes;

        public FrmClientes()
        {
            Text = "Clientes";
            ClientSize = new Size(880, 560);
            StartPosition = FormStartPosition.CenterScreen;

            Ayuda.CrearEtiqueta(this, "Nombre:", 20, 20);
            txtNombre = Ayuda.CrearTexto(this, 120, 20, 250);

            Ayuda.CrearEtiqueta(this, "Teléfono:", 20, 55);
            txtTelefono = Ayuda.CrearTexto(this, 120, 55, 250);

            Ayuda.CrearEtiqueta(this, "Dirección:", 450, 20);
            txtDireccion = Ayuda.CrearTexto(this, 560, 20, 300);

            Ayuda.CrearEtiqueta(this, "Correo:", 450, 55);
            txtCorreo = Ayuda.CrearTexto(this, 560, 55, 300);

            btnGuardar = Ayuda.CrearBoton(this, "Guardar", 20, 110);
            btnModificar = Ayuda.CrearBoton(this, "Modificar", 130, 110);
            btnEliminar = Ayuda.CrearBoton(this, "Eliminar", 240, 110);
            btnLimpiar = Ayuda.CrearBoton(this, "Limpiar", 350, 110);

            dgvClientes = Ayuda.CrearTabla(this, 20, 160, 840, 380);

            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            dgvClientes.CellClick += DgvClientes_CellClick;

            CargarTabla();
        }

        private void CargarTabla()
        {
            dgvClientes.DataSource = null;
            dgvClientes.DataSource = datos.ObtenerTodos();
            dgvClientes.ClearSelection();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();
            txtCorreo.Clear();
            idSeleccionado = 0;
        }

        // Valida que los datos esten bien antes de guardar
        private bool DatosValidos()
        {
            if (txtNombre.Text.Trim() == "")
            {
                Ayuda.MostrarError("El nombre es obligatorio.");
                return false;
            }
            return true;
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!DatosValidos())
            {
                return;
            }

            try
            {
                Cliente c = new Cliente(0, txtNombre.Text.Trim(), txtTelefono.Text.Trim(),
                                        txtDireccion.Text.Trim(), txtCorreo.Text.Trim());
                datos.Insertar(c);
                Ayuda.MostrarMensaje("Cliente guardado correctamente.");
                LimpiarCampos();
                CargarTabla();
            }
            catch (Exception ex)
            {
                Ayuda.MostrarError("Error al guardar: " + ex.Message);
            }
        }

        private void BtnModificar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                Ayuda.MostrarError("Seleccione un cliente de la tabla.");
                return;
            }
            if (!DatosValidos())
            {
                return;
            }

            try
            {
                Cliente c = new Cliente(idSeleccionado, txtNombre.Text.Trim(), txtTelefono.Text.Trim(),
                                        txtDireccion.Text.Trim(), txtCorreo.Text.Trim());
                datos.Modificar(c);
                Ayuda.MostrarMensaje("Cliente modificado correctamente.");
                LimpiarCampos();
                CargarTabla();
            }
            catch (Exception ex)
            {
                Ayuda.MostrarError("Error al modificar: " + ex.Message);
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                Ayuda.MostrarError("Seleccione un cliente de la tabla.");
                return;
            }
            if (!Ayuda.Confirmar("¿Seguro que desea eliminar este cliente?"))
            {
                return;
            }

            try
            {
                datos.Eliminar(idSeleccionado);
                Ayuda.MostrarMensaje("Cliente eliminado.");
                LimpiarCampos();
                CargarTabla();
            }
            catch (Exception)
            {
                // Pasa cuando el cliente ya tiene mascotas registradas
                Ayuda.MostrarError("No se pudo eliminar. Es posible que el cliente tenga mascotas registradas.");
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            dgvClientes.ClearSelection();
        }

        // Al hacer clic en una fila, los datos pasan a las cajas de texto
        private void DgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            Cliente c = (Cliente)dgvClientes.Rows[e.RowIndex].DataBoundItem;
            idSeleccionado = c.IdCliente;
            txtNombre.Text = c.Nombre;
            txtTelefono.Text = c.Telefono;
            txtDireccion.Text = c.Direccion;
            txtCorreo.Text = c.Correo;
        }
    }
}
