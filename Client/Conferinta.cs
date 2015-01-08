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
    public partial class Conferinta : Form
    {
        int codConferinta;
        MainForm owner;

        public Conferinta(bool permisie)
        {
            InitializeComponent();
            if (permisie == true)
                button1.Enabled = true;
        }


        private void Conferinta_Load(object sender, EventArgs e)
        {
            codConferinta = (int)this.Tag;
            owner = (MainForm)this.Owner;
            if (owner == null)
                MessageBox.Show("nooo owner");
            // label1.Text = owner.RequstUsersOfConf(codConferinta);
        }

        internal void ReceiveMsg(string user, string message){
            listBox1.Items.Add(user.ToUpper() + " said: " + message);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (owner != null)
            {
                owner.stergeConf(codConferinta);
                this.Dispose();
            }

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && textBox1.TextLength > 2)
            {
                listBox1.Items.Add("YU said : " + textBox1.Text);
                owner.SendToConf(codConferinta, textBox1.Text);
                textBox1.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }
}
