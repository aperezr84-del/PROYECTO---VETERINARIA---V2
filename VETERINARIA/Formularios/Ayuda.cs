using System.Globalization;

namespace SistemaControlClinicoVeterinario.Formularios
{
    // Metodos auxiliares para no repetir el mismo codigo en todos los formularios
    public static class Ayuda
    {
        public static Label CrearEtiqueta(Control padre, string texto, int x, int y)
        {
            Label lbl = new Label();
            lbl.Text = texto;
            lbl.Left = x;
            lbl.Top = y + 4;
            lbl.AutoSize = true;
            padre.Controls.Add(lbl);
            return lbl;
        }

        public static TextBox CrearTexto(Control padre, int x, int y, int ancho)
        {
            TextBox txt = new TextBox();
            txt.Left = x;
            txt.Top = y;
            txt.Width = ancho;
            padre.Controls.Add(txt);
            return txt;
        }

        public static ComboBox CrearCombo(Control padre, int x, int y, int ancho)
        {
            ComboBox cmb = new ComboBox();
            cmb.Left = x;
            cmb.Top = y;
            cmb.Width = ancho;
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            padre.Controls.Add(cmb);
            return cmb;
        }

        public static DateTimePicker CrearFecha(Control padre, int x, int y, int ancho, bool conHora)
        {
            DateTimePicker dtp = new DateTimePicker();
            dtp.Left = x;
            dtp.Top = y;
            dtp.Width = ancho;

            if (conHora)
            {
                dtp.Format = DateTimePickerFormat.Custom;
                dtp.CustomFormat = "dd/MM/yyyy HH:mm";
            }
            else
            {
                dtp.Format = DateTimePickerFormat.Short;
            }

            padre.Controls.Add(dtp);
            return dtp;
        }

        public static Button CrearBoton(Control padre, string texto, int x, int y)
        {
            Button btn = new Button();
            btn.Text = texto;
            btn.Left = x;
            btn.Top = y;
            btn.Width = 100;
            btn.Height = 30;
            padre.Controls.Add(btn);
            return btn;
        }

        public static DataGridView CrearTabla(Control padre, int x, int y, int ancho, int alto)
        {
            DataGridView dgv = new DataGridView();
            dgv.Left = x;
            dgv.Top = y;
            dgv.Width = ancho;
            dgv.Height = alto;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            padre.Controls.Add(dgv);
            return dgv;
        }

        // Llena un ComboBox con pares (id, texto a mostrar)
        public static void LlenarCombo(ComboBox cmb, List<KeyValuePair<int, string>> lista)
        {
            cmb.DisplayMember = "Value";
            cmb.ValueMember = "Key";
            cmb.DataSource = lista;
        }

        // Convierte el texto de una caja a numero. Acepta punto o coma como decimal
        public static bool LeerNumero(string texto, out double numero)
        {
            texto = texto.Trim().Replace(',', '.');
            return double.TryParse(texto, NumberStyles.Float, CultureInfo.InvariantCulture, out numero);
        }

        public static void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "Clínica Veterinaria", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Clínica Veterinaria", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static bool Confirmar(string mensaje)
        {
            DialogResult respuesta = MessageBox.Show(mensaje, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return respuesta == DialogResult.Yes;
        }
    }
}
