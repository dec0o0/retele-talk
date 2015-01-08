using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;


namespace Client
{
    public partial class MainForm : Form, IClient
    {
        private Proxy proxy;
        string uzer;
        private List<string> users = new List<string>();
        

        public MainForm()
        {
            InitializeComponent();
        }

        private void refresh()
        {
            listView1.Items.Clear();
            listView1.Items.Add(new ListViewItem("TUTUROR"));
            foreach (string s in users)
            {
                listView1.Items.Add(new ListViewItem(s));
            }
            listView1.Items[0].Selected = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.Columns.Add("userz aktivi ( - eu)", 180);
            listView1.MultiSelect = false;
            uzer = this.Tag.ToString();
            proxy = new Proxy(this);
            proxy.Register(uzer);
            this.Text = "Guten tag " + uzer.ToUpper();
            listBox.Items.Add("This is your chat area. Enjoy!");
            listBox.Items.Add("\n");
            listView1.CheckBoxes = true;
        }

        public void OnReceive(string message, string user, bool mass)
        {
            if (!mass)
                listBox.Items.Add("(PM) - " + user + " : " + message);
            else
                listBox.Items.Add("(PUBLIC) - " + user + " : " + message);
        }

        public void OnRegister(string user)
        {
            users.Add(user);
            listBox.Items.Add("Uzer " + user + " s-a logat.");
            refresh();
        }

        public void OnUnregister(string user)
        {
            users.Remove(user);
            listBox.Items.Add("Uzer " + user + " s-a delogat."); 
            refresh();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            proxy.Unregister();
            MessageBox.Show("Sesiune incheiata");
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
       {
            if (e.KeyCode == Keys.Enter && textBox.TextLength > 2)
            {
                if (listView1.SelectedItems.Count != 1)
                    MessageBox.Show("Selektati destinatarul");
                else
                {
                    bool mas = false;
                    string user = listView1.SelectedItems[0].Text.ToString();
                    if (user.Equals("TUTUROR"))
                        mas = true;
                    proxy.Send(textBox.Text, user, mas);
                    // listBox.Items.Add("(YU) catre " + user + " : " + textBox.Text);
                    textBox.Text = "";
                }
            }
        }


        //unfinished
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            label2.Text = "sending to " + listView1.SelectedItems[0].Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.CheckedItems.Count > 1)
            {
                List<String> aux = new List<string>();
                foreach (ListViewItem li in listView1.CheckedItems)
                    aux.Add(li.Text);
                button1.Text = "Requesting code ...";
                int cod = proxy.AddConf(aux);
                if (cod == -1)
                {
                    MessageBox.Show("Failed!");
                    return;
                }
                Conferinta conf = new Conferinta(true);
                conf.Tag = cod;
                conferinte.Add(cod, conf);
                button1.Text = "Creeaza camera virtuala" ;
                conf.Show(this);
            }
            else
                MessageBox.Show("Useri insuficienti");
        }

        Dictionary<int, Conferinta> conferinte = new Dictionary<int, Conferinta>();

        public void stergeConf(int cod)
        {
            proxy.StergeConf(cod);
            if (conferinte.ContainsKey(cod))
                conferinte.Remove(cod);
        }

        public void SendToConf(int cod, string message)
        {
            proxy.SendToConf(cod, message);
        }

        public void ReceiveFromConf(int cod, string user, string message)
        {
            if (conferinte.ContainsKey(cod))
            {
                Conferinta c = conferinte[cod];
                if (c.Visible == false)
                    c.Visible = true;
                c.ReceiveMsg(user, message);
            }
            else
            {
                Conferinta c = new Conferinta(false);
                c.Tag = cod;
                conferinte.Add(cod, c);
                c.Show(this);
                c.ReceiveMsg(user, message);
            }
        }

        public string RequstUsersOfConf(int cod){
            return proxy.RequstUsersOfConf(cod);
        }

    }
}
