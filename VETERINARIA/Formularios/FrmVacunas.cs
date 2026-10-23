using SistemaControlClinicoVeterinario.Datos;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Formularios
{
    public class FrmVacunas : Form
    {
        private VacunaDatos datos = new VacunaDatos();
        private MascotaDatos mascotaDatos = new MascotaDatos();
        private int idSeleccionado = 0;   // 0 = no hay ningun registro seleccionado

        private TextBox txtNombre;
        private DateTimePicker dtpAplicacion;
        private DateTimePicker dtpProxima;
        private TextBox txtObservaciones;
        private ComboBox cmbMascota;
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvVacunas;

        public FrmVacunas()
        {
            Text = "Vacunas";
            ClientSize = new Size(880, 560);
            StartPosition = FormStartPosition.CenterScreen;

            Ayuda.CrearEtiqueta(this, "Nombre vacuna:", 20, 20);
            txtNombre = Ayuda.CrearTexto(this, 140, 20, 250);

            Ayuda.CrearEtiqueta(this, "Fecha aplicación:", 20, 55);
            dtpAplicacion = Ayuda.CrearFecha(this, 140, 55, 250, false);

            Ayuda.CrearEtiqueta(this, "Próxima fecha:", 20, 90);
            dtpProxima = Ayuda.CrearFecha(this, 140, 90, 250, false);

            Ayuda.CrearEtiqueta(this, "Observaciones:", 450, 20);
            txtObservaciones = Ayuda.CrearTexto(this, 560, 20, 300);

            Ayuda.CrearEtiqueta(this, "Mascota:", 450, 55);
            cmbMascota = Ayuda.CrearCombo(this, 560, 55, 300);

            btnGuardar = Ayuda.CrearBoton(this, "Guardar", 20, 140);
            btnModificar = Ayuda.CrearBoton(this, "Modificar", 130, 140);
            btnEliminar = Ayuda.CrearBoton(this, "Eliminar", 240, 140);
            btnLimpiar = Ayuda.CrearBoton(this, "Limpiar", 350, 140);

            dgvVacunas = Ayuda.CrearTabla(this, 20, 190, 840, 350);

            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            dgvVacunas.CellClick += DgvVacunas_CellClick;

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
        }

        private void CargarTabla()
        {
            dgvVacunas.DataSource = null;
            dgvVacunas.DataSource = datos.ObtenerTodos();
            dgvVacunas.ClearSelection();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtObservaciones.Clear();
            dtpAplicacion.Value = DateTime.Now;
            dtpProxima.Value = DateTime.Now;
            idSeleccionado = 0;
        }

        // Valida los datos y, si estan bien, devuelve la vacuna armada. Si no, devuelve null
        private Vacuna ArmarVacuna(int id)
        {
            if (txtNombre.Text.Trim() == "")
            {
                Ayuda.MostrarError("El nombre de la vacuna es obligatorio.");
                return null;
            }
            if (cmbMascota.SelectedValue == null)
            {
                Ayuda.MostrarError("Primero debe registrar una mascota.");
                return null;
            }
            if (dtpProxima.Value.Date < dtpAplicacion.Value.Date)
            {
                Ayuda.MostrarError("La próxima fecha no puede ser antes de la fecha de aplicación.");
                return null;
            }

            int idMascota = (int)cmbMascota.SelectedValue;

            return new Vacuna(id, txtNombre.Text.Trim(), dtpAplicacion.Value, dtpProxima.Value,
                              txtObservaciones.Text.Trim(), idMascota);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            Vacuna v = ArmarVacuna(0);
            if (v == null)
            {
                return;
            }

            try
            {
                datos.Insertar(v);
                Ayuda.MostrarMensaje("Vacuna guardada correctamente.");
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
                Ayuda.MostrarError("Seleccione una vacuna de la tabla.");
                return;
            }

            Vacuna v = ArmarVacuna(idSeleccionado);
            if (v == null)
            {
                return;
            }

            try
            {
                datos.Modificar(v);
                Ayuda.MostrarMensaje("Vacuna modificada correctamente.");
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
                Ayuda.MostrarError("Seleccione una vacuna de la tabla.");
                return;
            }
            if (!Ayuda.Confirmar("¿Seguro que desea eliminar esta vacuna?"))
            {
                return;
            }

            try
            {
                datos.Eliminar(idSeleccionado);
                Ayuda.MostrarMensaje("Vacuna eliminada.");
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
            dgvVacunas.ClearSelection();
        }

        private void DgvVacunas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            Vacuna v = (Vacuna)dgvVacunas.Rows[e.RowIndex].DataBoundItem;
            idSeleccionado = v.IdVacuna;
            txtNombre.Text = v.Nombre;
            dtpAplicacion.Value = v.FechaAplicacion;
            dtpProxima.Value = v.ProximaFecha;
            txtObservaciones.Text = v.Observaciones;
            cmbMascota.SelectedValue = v.IdMascota;
        }
    }
}
