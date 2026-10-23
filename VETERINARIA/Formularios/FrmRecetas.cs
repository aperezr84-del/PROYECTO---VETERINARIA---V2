using SistemaControlClinicoVeterinario.Datos;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Formularios
{
    public class FrmRecetas : Form
    {
        private RecetaDatos datos = new RecetaDatos();
        private ConsultaDatos consultaDatos = new ConsultaDatos();
        private int idSeleccionado = 0;   // 0 = no hay ningun registro seleccionado

        private DateTimePicker dtpFecha;
        private TextBox txtMedicamento;
        private TextBox txtDosis;
        private TextBox txtFrecuencia;
        private TextBox txtDuracion;
        private TextBox txtIndicaciones;
        private ComboBox cmbConsulta;
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvRecetas;

        public FrmRecetas()
        {
            Text = "Recetas";
            ClientSize = new Size(880, 580);
            StartPosition = FormStartPosition.CenterScreen;

            Ayuda.CrearEtiqueta(this, "Fecha:", 20, 20);
            dtpFecha = Ayuda.CrearFecha(this, 130, 20, 250, false);

            Ayuda.CrearEtiqueta(this, "Medicamento:", 20, 55);
            txtMedicamento = Ayuda.CrearTexto(this, 130, 55, 250);

            Ayuda.CrearEtiqueta(this, "Dosis:", 20, 90);
            txtDosis = Ayuda.CrearTexto(this, 130, 90, 250);

            Ayuda.CrearEtiqueta(this, "Frecuencia:", 450, 20);
            txtFrecuencia = Ayuda.CrearTexto(this, 560, 20, 300);

            Ayuda.CrearEtiqueta(this, "Duración:", 450, 55);
            txtDuracion = Ayuda.CrearTexto(this, 560, 55, 300);

            Ayuda.CrearEtiqueta(this, "Indicaciones:", 450, 90);
            txtIndicaciones = Ayuda.CrearTexto(this, 560, 90, 300);

            Ayuda.CrearEtiqueta(this, "Consulta:", 20, 125);
            cmbConsulta = Ayuda.CrearCombo(this, 130, 125, 380);

            btnGuardar = Ayuda.CrearBoton(this, "Guardar", 20, 170);
            btnModificar = Ayuda.CrearBoton(this, "Modificar", 130, 170);
            btnEliminar = Ayuda.CrearBoton(this, "Eliminar", 240, 170);
            btnLimpiar = Ayuda.CrearBoton(this, "Limpiar", 350, 170);

            dgvRecetas = Ayuda.CrearTabla(this, 20, 220, 840, 340);

            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            dgvRecetas.CellClick += DgvRecetas_CellClick;

            CargarCombos();
            CargarTabla();
        }

        // La receta se asocia a una consulta, por eso se llena el combo con las consultas
        private void CargarCombos()
        {
            List<KeyValuePair<int, string>> consultas = new List<KeyValuePair<int, string>>();
            foreach (Consulta c in consultaDatos.ObtenerTodos())
            {
                string texto = c.IdConsulta + " - " + c.Fecha.ToString("dd/MM/yyyy") + " - " + c.Motivo;
                consultas.Add(new KeyValuePair<int, string>(c.IdConsulta, texto));
            }
            Ayuda.LlenarCombo(cmbConsulta, consultas);
        }

        private void CargarTabla()
        {
            dgvRecetas.DataSource = null;
            dgvRecetas.DataSource = datos.ObtenerTodos();
            dgvRecetas.ClearSelection();
        }

        private void LimpiarCampos()
        {
            txtMedicamento.Clear();
            txtDosis.Clear();
            txtFrecuencia.Clear();
            txtDuracion.Clear();
            txtIndicaciones.Clear();
            dtpFecha.Value = DateTime.Now;
            idSeleccionado = 0;
        }

        // Valida los datos y, si estan bien, devuelve la receta armada. Si no, devuelve null
        private Receta ArmarReceta(int id)
        {
            if (txtMedicamento.Text.Trim() == "")
            {
                Ayuda.MostrarError("El medicamento es obligatorio.");
                return null;
            }
            if (cmbConsulta.SelectedValue == null)
            {
                Ayuda.MostrarError("Primero debe registrar una consulta.");
                return null;
            }

            int idConsulta = (int)cmbConsulta.SelectedValue;

            return new Receta(id, dtpFecha.Value, txtMedicamento.Text.Trim(), txtDosis.Text.Trim(),
                              txtFrecuencia.Text.Trim(), txtDuracion.Text.Trim(),
                              txtIndicaciones.Text.Trim(), idConsulta);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            Receta r = ArmarReceta(0);
            if (r == null)
            {
                return;
            }

            try
            {
                datos.Insertar(r);
                Ayuda.MostrarMensaje("Receta guardada correctamente.");
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
                Ayuda.MostrarError("Seleccione una receta de la tabla.");
                return;
            }

            Receta r = ArmarReceta(idSeleccionado);
            if (r == null)
            {
                return;
            }

            try
            {
                datos.Modificar(r);
                Ayuda.MostrarMensaje("Receta modificada correctamente.");
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
                Ayuda.MostrarError("Seleccione una receta de la tabla.");
                return;
            }
            if (!Ayuda.Confirmar("¿Seguro que desea eliminar esta receta?"))
            {
                return;
            }

            try
            {
                datos.Eliminar(idSeleccionado);
                Ayuda.MostrarMensaje("Receta eliminada.");
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
            dgvRecetas.ClearSelection();
        }

        private void DgvRecetas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            Receta r = (Receta)dgvRecetas.Rows[e.RowIndex].DataBoundItem;
            idSeleccionado = r.IdReceta;
            dtpFecha.Value = r.Fecha;
            txtMedicamento.Text = r.Medicamento;
            txtDosis.Text = r.Dosis;
            txtFrecuencia.Text = r.Frecuencia;
            txtDuracion.Text = r.Duracion;
            txtIndicaciones.Text = r.Indicaciones;
            cmbConsulta.SelectedValue = r.IdConsulta;
        }
    }
}
