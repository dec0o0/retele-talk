using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class welcome : Form
    {
        public welcome()
        {
            InitializeComponent();
        }

        private void welcome_Load(object sender, EventArgs e)
        {
            textBox1.PerformLayout();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                //KeyChar == (char)13)
            {
                String aux = textBox1.Text.Trim().ToString();
                try
                {
                    if (aux.Length == 0)
                        errorProvider1.SetError(textBox1, "Nein uzername");
                    else
                    {
                        this.Hide();
                        var a = new MainForm();
                        a.Tag = aux;
                        a.ShowDialog();
                        this.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
