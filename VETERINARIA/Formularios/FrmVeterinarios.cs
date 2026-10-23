using SistemaControlClinicoVeterinario.Datos;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Formularios
{
    public class FrmVeterinarios : Form
    {
        private VeterinarioDatos datos = new VeterinarioDatos();
        private int idSeleccionado = 0;   // 0 = no hay ningun registro seleccionado

        private TextBox txtNombre;
        private TextBox txtEspecialidad;
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvVeterinarios;

        public FrmVeterinarios()
        {
            Text = "Veterinarios";
            ClientSize = new Size(880, 560);
            StartPosition = FormStartPosition.CenterScreen;

            Ayuda.CrearEtiqueta(this, "Nombre:", 20, 20);
            txtNombre = Ayuda.CrearTexto(this, 120, 20, 250);

            Ayuda.CrearEtiqueta(this, "Especialidad:", 450, 20);
            txtEspecialidad = Ayuda.CrearTexto(this, 560, 20, 300);

            btnGuardar = Ayuda.CrearBoton(this, "Guardar", 20, 75);
            btnModificar = Ayuda.CrearBoton(this, "Modificar", 130, 75);
            btnEliminar = Ayuda.CrearBoton(this, "Eliminar", 240, 75);
            btnLimpiar = Ayuda.CrearBoton(this, "Limpiar", 350, 75);

            dgvVeterinarios = Ayuda.CrearTabla(this, 20, 125, 840, 415);

            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            dgvVeterinarios.CellClick += DgvVeterinarios_CellClick;

            CargarTabla();
        }

        private void CargarTabla()
        {
            dgvVeterinarios.DataSource = null;
            dgvVeterinarios.DataSource = datos.ObtenerTodos();
            dgvVeterinarios.ClearSelection();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtEspecialidad.Clear();
            idSeleccionado = 0;
        }

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
                Veterinario v = new Veterinario(0, txtNombre.Text.Trim(), txtEspecialidad.Text.Trim());
                datos.Insertar(v);
                Ayuda.MostrarMensaje("Veterinario guardado correctamente.");
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
                Ayuda.MostrarError("Seleccione un veterinario de la tabla.");
                return;
            }
            if (!DatosValidos())
            {
                return;
            }

            try
            {
                Veterinario v = new Veterinario(idSeleccionado, txtNombre.Text.Trim(), txtEspecialidad.Text.Trim());
                datos.Modificar(v);
                Ayuda.MostrarMensaje("Veterinario modificado correctamente.");
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
                Ayuda.MostrarError("Seleccione un veterinario de la tabla.");
                return;
            }
            if (!Ayuda.Confirmar("¿Seguro que desea eliminar este veterinario?"))
            {
                return;
            }

            try
            {
                datos.Eliminar(idSeleccionado);
                Ayuda.MostrarMensaje("Veterinario eliminado.");
                LimpiarCampos();
                CargarTabla();
            }
            catch (Exception)
            {
                // Pasa cuando el veterinario ya tiene citas o consultas
                Ayuda.MostrarError("No se pudo eliminar. Es posible que el veterinario tenga citas o consultas registradas.");
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            dgvVeterinarios.ClearSelection();
        }

        private void DgvVeterinarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            Veterinario v = (Veterinario)dgvVeterinarios.Rows[e.RowIndex].DataBoundItem;
            idSeleccionado = v.IdVeterinario;
            txtNombre.Text = v.Nombre;
            txtEspecialidad.Text = v.Especialidad;
        }
    }
}
