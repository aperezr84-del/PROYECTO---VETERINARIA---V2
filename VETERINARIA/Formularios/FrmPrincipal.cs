namespace SistemaControlClinicoVeterinario.Formularios
{
    // Menu principal: desde aqui se abre cada formulario del sistema
    public class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            Text = "Sistema de Control Clínico Veterinario";
            ClientSize = new Size(420, 400);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label titulo = new Label();
            titulo.Text = "Clínica Veterinaria";
            titulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            titulo.AutoSize = true;
            titulo.Left = 110;
            titulo.Top = 20;
            Controls.Add(titulo);

            Button btnClientes = CrearBotonMenu("Clientes", 20, 70);
            Button btnMascotas = CrearBotonMenu("Mascotas", 220, 70);
            Button btnVeterinarios = CrearBotonMenu("Veterinarios", 20, 130);
            Button btnCitas = CrearBotonMenu("Citas", 220, 130);
            Button btnConsultas = CrearBotonMenu("Consultas", 20, 190);
            Button btnVacunas = CrearBotonMenu("Vacunas", 220, 190);
            Button btnRecetas = CrearBotonMenu("Recetas", 20, 250);
            Button btnSalir = CrearBotonMenu("Salir", 220, 250);

            btnClientes.Click += BtnClientes_Click;
            btnMascotas.Click += BtnMascotas_Click;
            btnVeterinarios.Click += BtnVeterinarios_Click;
            btnCitas.Click += BtnCitas_Click;
            btnConsultas.Click += BtnConsultas_Click;
            btnVacunas.Click += BtnVacunas_Click;
            btnRecetas.Click += BtnRecetas_Click;
            btnSalir.Click += BtnSalir_Click;
        }

        private Button CrearBotonMenu(string texto, int x, int y)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Left = x;
            btn.Top = y;
            btn.Width = 180;
            btn.Height = 45;
            Controls.Add(btn);
            return btn;
        }

        private void BtnClientes_Click(object sender, EventArgs e)
        {
            FrmClientes frm = new FrmClientes();
            frm.ShowDialog();
        }

        private void BtnMascotas_Click(object sender, EventArgs e)
        {
            FrmMascotas frm = new FrmMascotas();
            frm.ShowDialog();
        }

        private void BtnVeterinarios_Click(object sender, EventArgs e)
        {
            FrmVeterinarios frm = new FrmVeterinarios();
            frm.ShowDialog();
        }

        private void BtnCitas_Click(object sender, EventArgs e)
        {
            FrmCitas frm = new FrmCitas();
            frm.ShowDialog();
        }

        private void BtnConsultas_Click(object sender, EventArgs e)
        {
            FrmConsultas frm = new FrmConsultas();
            frm.ShowDialog();
        }

        private void BtnVacunas_Click(object sender, EventArgs e)
        {
            FrmVacunas frm = new FrmVacunas();
            frm.ShowDialog();
        }

        private void BtnRecetas_Click(object sender, EventArgs e)
        {
            FrmRecetas frm = new FrmRecetas();
            frm.ShowDialog();
        }

        private void BtnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
