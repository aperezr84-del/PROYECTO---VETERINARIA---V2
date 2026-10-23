using SistemaControlClinicoVeterinario.Datos;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Formularios
{
    public class FrmCitas : Form
    {
        private CitaDatos datos = new CitaDatos();
        private MascotaDatos mascotaDatos = new MascotaDatos();
        private VeterinarioDatos veterinarioDatos = new VeterinarioDatos();
        private int idSeleccionado = 0;   // 0 = no hay ningun registro seleccionado

        private DateTimePicker dtpFecha;
        private TextBox txtMotivo;
        private ComboBox cmbEstado;
        private ComboBox cmbMascota;
        private ComboBox cmbVeterinario;
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvCitas;

        public FrmCitas()
        {
            Text = "Citas";
            ClientSize = new Size(880, 560);
            StartPosition = FormStartPosition.CenterScreen;

            Ayuda.CrearEtiqueta(this, "Fecha y hora:", 20, 20);
            dtpFecha = Ayuda.CrearFecha(this, 140, 20, 250, true);

            Ayuda.CrearEtiqueta(this, "Motivo:", 20, 55);
            txtMotivo = Ayuda.CrearTexto(this, 140, 55, 250);

            Ayuda.CrearEtiqueta(this, "Estado:", 20, 90);
            cmbEstado = Ayuda.CrearCombo(this, 140, 90, 250);
            cmbEstado.Items.Add("Pendiente");
            cmbEstado.Items.Add("Atendida");
            cmbEstado.Items.Add("Cancelada");
            cmbEstado.SelectedIndex = 0;

            Ayuda.CrearEtiqueta(this, "Mascota:", 450, 20);
            cmbMascota = Ayuda.CrearCombo(this, 560, 20, 290);

            Ayuda.CrearEtiqueta(this, "Veterinario:", 450, 55);
            cmbVeterinario = Ayuda.CrearCombo(this, 560, 55, 290);

            btnGuardar = Ayuda.CrearBoton(this, "Guardar", 20, 140);
            btnModificar = Ayuda.CrearBoton(this, "Modificar", 130, 140);
            btnEliminar = Ayuda.CrearBoton(this, "Eliminar", 240, 140);
            btnLimpiar = Ayuda.CrearBoton(this, "Limpiar", 350, 140);

            dgvCitas = Ayuda.CrearTabla(this, 20, 190, 840, 350);

            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            dgvCitas.CellClick += DgvCitas_CellClick;

            CargarCombos();
            CargarTabla();
        }

        private void CargarCombos()
        {
            List<KeyValuePair<int, string>> mascotas = new List<KeyValuePair<int, string>>();
            foreach (Mascota m in mascotaDatos.ObtenerTodos())
            {
                mascotas.Add(new KeyValuePair<int, string>(m.IdMascota, m.IdMascota + " - " + m.Nombre));
            }
            Ayuda.LlenarCombo(cmbMascota, mascotas);

            List<KeyValuePair<int, string>> veterinarios = new List<KeyValuePair<int, string>>();
            foreach (Veterinario v in veterinarioDatos.ObtenerTodos())
            {
                veterinarios.Add(new KeyValuePair<int, string>(v.IdVeterinario, v.IdVeterinario + " - " + v.Nombre));
            }
            Ayuda.LlenarCombo(cmbVeterinario, veterinarios);
        }

        private void CargarTabla()
        {
            dgvCitas.DataSource = null;
            dgvCitas.DataSource = datos.ObtenerTodos();
            dgvCitas.ClearSelection();
        }

        private void LimpiarCampos()
        {
            txtMotivo.Clear();
            cmbEstado.SelectedIndex = 0;
            dtpFecha.Value = DateTime.Now;
            idSeleccionado = 0;
        }

        // Valida los datos y, si estan bien, devuelve la cita armada. Si no, devuelve null
        private Cita ArmarCita(int id)
        {
            if (txtMotivo.Text.Trim() == "")
            {
                Ayuda.MostrarError("El motivo es obligatorio.");
                return null;
            }
            if (cmbMascota.SelectedValue == null)
            {
                Ayuda.MostrarError("Primero debe registrar una mascota.");
                return null;
            }
            if (cmbVeterinario.SelectedValue == null)
            {
                Ayuda.MostrarError("Primero debe registrar un veterinario.");
                return null;
            }

            int idMascota = (int)cmbMascota.SelectedValue;
            int idVeterinario = (int)cmbVeterinario.SelectedValue;

            return new Cita(id, dtpFecha.Value, txtMotivo.Text.Trim(), cmbEstado.Text, idMascota, idVeterinario);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            Cita c = ArmarCita(0);
            if (c == null)
            {
                return;
            }

            try
            {
                datos.Insertar(c);
                Ayuda.MostrarMensaje("Cita guardada correctamente.");
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
                Ayuda.MostrarError("Seleccione una cita de la tabla.");
                return;
            }

            Cita c = ArmarCita(idSeleccionado);
            if (c == null)
            {
                return;
            }

            try
            {
                datos.Modificar(c);
                Ayuda.MostrarMensaje("Cita modificada correctamente.");
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
                Ayuda.MostrarError("Seleccione una cita de la tabla.");
                return;
            }
            if (!Ayuda.Confirmar("¿Seguro que desea eliminar esta cita?"))
            {
                return;
            }

            try
            {
                datos.Eliminar(idSeleccionado);
                Ayuda.MostrarMensaje("Cita eliminada.");
                LimpiarCampos();
                CargarTabla();
            }
            catch (Exception ex)
            {
                Ayuda.MostrarError("No se pudo eliminar: " + ex.Message);
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            dgvCitas.ClearSelection();
        }

        private void DgvCitas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            Cita c = (Cita)dgvCitas.Rows[e.RowIndex].DataBoundItem;
            idSeleccionado = c.IdCita;
            dtpFecha.Value = c.Fecha;
            txtMotivo.Text = c.Motivo;
            cmbEstado.Text = c.Estado;
            cmbMascota.SelectedValue = c.IdMascota;
            cmbVeterinario.SelectedValue = c.IdVeterinario;
        }
    }
}
