using SistemaControlClinicoVeterinario.Datos;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Formularios
{
    public class FrmConsultas : Form
    {
        private ConsultaDatos datos = new ConsultaDatos();
        private MascotaDatos mascotaDatos = new MascotaDatos();
        private VeterinarioDatos veterinarioDatos = new VeterinarioDatos();
        private int idSeleccionado = 0;   // 0 = no hay ningun registro seleccionado

        private DateTimePicker dtpFecha;
        private TextBox txtMotivo;
        private TextBox txtSintomas;
        private TextBox txtDiagnostico;
        private TextBox txtTratamiento;
        private TextBox txtPeso;
        private ComboBox cmbMascota;
        private ComboBox cmbVeterinario;
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvConsultas;

        public FrmConsultas()
        {
            Text = "Consultas";
            ClientSize = new Size(900, 600);
            StartPosition = FormStartPosition.CenterScreen;

            Ayuda.CrearEtiqueta(this, "Fecha y hora:", 20, 20);
            dtpFecha = Ayuda.CrearFecha(this, 130, 20, 250, true);

            Ayuda.CrearEtiqueta(this, "Motivo:", 20, 55);
            txtMotivo = Ayuda.CrearTexto(this, 130, 55, 250);

            Ayuda.CrearEtiqueta(this, "Síntomas:", 20, 90);
            txtSintomas = Ayuda.CrearTexto(this, 130, 90, 250);

            Ayuda.CrearEtiqueta(this, "Diagnóstico:", 20, 125);
            txtDiagnostico = Ayuda.CrearTexto(this, 130, 125, 250);

            Ayuda.CrearEtiqueta(this, "Tratamiento:", 460, 20);
            txtTratamiento = Ayuda.CrearTexto(this, 570, 20, 310);

            Ayuda.CrearEtiqueta(this, "Peso (kg):", 460, 55);
            txtPeso = Ayuda.CrearTexto(this, 570, 55, 310);

            Ayuda.CrearEtiqueta(this, "Mascota:", 460, 90);
            cmbMascota = Ayuda.CrearCombo(this, 570, 90, 310);

            Ayuda.CrearEtiqueta(this, "Veterinario:", 460, 125);
            cmbVeterinario = Ayuda.CrearCombo(this, 570, 125, 310);

            btnGuardar = Ayuda.CrearBoton(this, "Guardar", 20, 175);
            btnModificar = Ayuda.CrearBoton(this, "Modificar", 130, 175);
            btnEliminar = Ayuda.CrearBoton(this, "Eliminar", 240, 175);
            btnLimpiar = Ayuda.CrearBoton(this, "Limpiar", 350, 175);

            dgvConsultas = Ayuda.CrearTabla(this, 20, 225, 860, 355);

            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            dgvConsultas.CellClick += DgvConsultas_CellClick;

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
            dgvConsultas.DataSource = null;
            dgvConsultas.DataSource = datos.ObtenerTodos();
            dgvConsultas.ClearSelection();
        }

        private void LimpiarCampos()
        {
            txtMotivo.Clear();
            txtSintomas.Clear();
            txtDiagnostico.Clear();
            txtTratamiento.Clear();
            txtPeso.Clear();
            dtpFecha.Value = DateTime.Now;
            idSeleccionado = 0;
        }

        // Valida los datos y, si estan bien, devuelve la consulta armada. Si no, devuelve null
        private Consulta ArmarConsulta(int id)
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

            double peso;
            if (!Ayuda.LeerNumero(txtPeso.Text, out peso) || peso <= 0)
            {
                Ayuda.MostrarError("El peso debe ser un número mayor que 0.");
                return null;
            }

            int idMascota = (int)cmbMascota.SelectedValue;
            int idVeterinario = (int)cmbVeterinario.SelectedValue;

            return new Consulta(id, dtpFecha.Value, txtMotivo.Text.Trim(), txtSintomas.Text.Trim(),
                                txtDiagnostico.Text.Trim(), txtTratamiento.Text.Trim(), peso,
                                idMascota, idVeterinario);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            Consulta c = ArmarConsulta(0);
            if (c == null)
            {
                return;
            }

            try
            {
                datos.Insertar(c);
                Ayuda.MostrarMensaje("Consulta guardada correctamente.");
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
                Ayuda.MostrarError("Seleccione una consulta de la tabla.");
                return;
            }

            Consulta c = ArmarConsulta(idSeleccionado);
            if (c == null)
            {
                return;
            }

            try
            {
                datos.Modificar(c);
                Ayuda.MostrarMensaje("Consulta modificada correctamente.");
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
                Ayuda.MostrarError("Seleccione una consulta de la tabla.");
                return;
            }
            if (!Ayuda.Confirmar("¿Seguro que desea eliminar esta consulta?"))
            {
                return;
            }

            try
            {
                datos.Eliminar(idSeleccionado);
                Ayuda.MostrarMensaje("Consulta eliminada.");
                LimpiarCampos();
                CargarTabla();
            }
            catch (Exception)
            {
                // Pasa cuando la consulta ya tiene recetas
                Ayuda.MostrarError("No se pudo eliminar. Es posible que la consulta tenga recetas registradas.");
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            dgvConsultas.ClearSelection();
        }

        private void DgvConsultas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            Consulta c = (Consulta)dgvConsultas.Rows[e.RowIndex].DataBoundItem;
            idSeleccionado = c.IdConsulta;
            dtpFecha.Value = c.Fecha;
            txtMotivo.Text = c.Motivo;
            txtSintomas.Text = c.Sintomas;
            txtDiagnostico.Text = c.Diagnostico;
            txtTratamiento.Text = c.Tratamiento;
            txtPeso.Text = c.Peso.ToString();
            cmbMascota.SelectedValue = c.IdMascota;
            cmbVeterinario.SelectedValue = c.IdVeterinario;
        }
    }
}
