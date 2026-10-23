using SistemaControlClinicoVeterinario.Datos;
using SistemaControlClinicoVeterinario.Models;

namespace SistemaControlClinicoVeterinario.Formularios
{
    public class FrmMascotas : Form
    {
        private MascotaDatos datos = new MascotaDatos();
        private ClienteDatos clienteDatos = new ClienteDatos();
        private int idSeleccionado = 0;   // 0 = no hay ningun registro seleccionado

        private TextBox txtNombre;
        private TextBox txtEspecie;
        private TextBox txtRaza;
        private ComboBox cmbSexo;
        private DateTimePicker dtpNacimiento;
        private TextBox txtPeso;
        private ComboBox cmbCliente;
        private Button btnGuardar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvMascotas;

        public FrmMascotas()
        {
            Text = "Mascotas";
            ClientSize = new Size(880, 560);
            StartPosition = FormStartPosition.CenterScreen;

            Ayuda.CrearEtiqueta(this, "Nombre:", 20, 20);
            txtNombre = Ayuda.CrearTexto(this, 140, 20, 250);

            Ayuda.CrearEtiqueta(this, "Especie:", 20, 55);
            txtEspecie = Ayuda.CrearTexto(this, 140, 55, 250);

            Ayuda.CrearEtiqueta(this, "Raza:", 20, 90);
            txtRaza = Ayuda.CrearTexto(this, 140, 90, 250);

            Ayuda.CrearEtiqueta(this, "Sexo:", 20, 125);
            cmbSexo = Ayuda.CrearCombo(this, 140, 125, 250);
            cmbSexo.Items.Add("Macho");
            cmbSexo.Items.Add("Hembra");
            cmbSexo.SelectedIndex = 0;

            Ayuda.CrearEtiqueta(this, "Fecha de nacimiento:", 450, 20);
            dtpNacimiento = Ayuda.CrearFecha(this, 600, 20, 250, false);

            Ayuda.CrearEtiqueta(this, "Peso (kg):", 450, 55);
            txtPeso = Ayuda.CrearTexto(this, 600, 55, 250);

            Ayuda.CrearEtiqueta(this, "Dueño (cliente):", 450, 90);
            cmbCliente = Ayuda.CrearCombo(this, 600, 90, 250);

            btnGuardar = Ayuda.CrearBoton(this, "Guardar", 20, 170);
            btnModificar = Ayuda.CrearBoton(this, "Modificar", 130, 170);
            btnEliminar = Ayuda.CrearBoton(this, "Eliminar", 240, 170);
            btnLimpiar = Ayuda.CrearBoton(this, "Limpiar", 350, 170);

            dgvMascotas = Ayuda.CrearTabla(this, 20, 220, 840, 320);

            btnGuardar.Click += BtnGuardar_Click;
            btnModificar.Click += BtnModificar_Click;
            btnEliminar.Click += BtnEliminar_Click;
            btnLimpiar.Click += BtnLimpiar_Click;
            dgvMascotas.CellClick += DgvMascotas_CellClick;

            CargarCombos();
            CargarTabla();
        }

        // Llena el combo con los clientes que existen en la base de datos
        private void CargarCombos()
        {
            List<KeyValuePair<int, string>> lista = new List<KeyValuePair<int, string>>();
            foreach (Cliente c in clienteDatos.ObtenerTodos())
            {
                lista.Add(new KeyValuePair<int, string>(c.IdCliente, c.IdCliente + " - " + c.Nombre));
            }
            Ayuda.LlenarCombo(cmbCliente, lista);
        }

        private void CargarTabla()
        {
            dgvMascotas.DataSource = null;
            dgvMascotas.DataSource = datos.ObtenerTodos();
            dgvMascotas.ClearSelection();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtEspecie.Clear();
            txtRaza.Clear();
            txtPeso.Clear();
            cmbSexo.SelectedIndex = 0;
            dtpNacimiento.Value = DateTime.Now;
            idSeleccionado = 0;
        }

        // Valida los datos y, si estan bien, devuelve la mascota armada. Si no, devuelve null
        private Mascota ArmarMascota(int id)
        {
            if (txtNombre.Text.Trim() == "")
            {
                Ayuda.MostrarError("El nombre es obligatorio.");
                return null;
            }
            if (cmbCliente.SelectedValue == null)
            {
                Ayuda.MostrarError("Primero debe registrar un cliente.");
                return null;
            }

            double peso;
            if (!Ayuda.LeerNumero(txtPeso.Text, out peso) || peso <= 0)
            {
                Ayuda.MostrarError("El peso debe ser un número mayor que 0.");
                return null;
            }

            int idCliente = (int)cmbCliente.SelectedValue;

            return new Mascota(id, txtNombre.Text.Trim(), txtEspecie.Text.Trim(), txtRaza.Text.Trim(),
                               cmbSexo.Text, dtpNacimiento.Value, peso, idCliente);
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            Mascota m = ArmarMascota(0);
            if (m == null)
            {
                return;
            }

            try
            {
                datos.Insertar(m);
                Ayuda.MostrarMensaje("Mascota guardada correctamente.");
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
                Ayuda.MostrarError("Seleccione una mascota de la tabla.");
                return;
            }

            Mascota m = ArmarMascota(idSeleccionado);
            if (m == null)
            {
                return;
            }

            try
            {
                datos.Modificar(m);
                Ayuda.MostrarMensaje("Mascota modificada correctamente.");
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
                Ayuda.MostrarError("Seleccione una mascota de la tabla.");
                return;
            }
            if (!Ayuda.Confirmar("¿Seguro que desea eliminar esta mascota?"))
            {
                return;
            }

            try
            {
                datos.Eliminar(idSeleccionado);
                Ayuda.MostrarMensaje("Mascota eliminada.");
                LimpiarCampos();
                CargarTabla();
            }
            catch (Exception)
            {
                // Pasa cuando la mascota ya tiene citas, consultas o vacunas
                Ayuda.MostrarError("No se pudo eliminar. Es posible que la mascota tenga citas, consultas o vacunas registradas.");
            }
        }

        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
            dgvMascotas.ClearSelection();
        }

        private void DgvMascotas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            Mascota m = (Mascota)dgvMascotas.Rows[e.RowIndex].DataBoundItem;
            idSeleccionado = m.IdMascota;
            txtNombre.Text = m.Nombre;
            txtEspecie.Text = m.Especie;
            txtRaza.Text = m.Raza;
            cmbSexo.Text = m.Sexo;
            dtpNacimiento.Value = m.FechaNacimiento;
            txtPeso.Text = m.Peso.ToString();
            cmbCliente.SelectedValue = m.IdCliente;
        }
    }
}
