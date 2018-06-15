using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JojoPos
{
    public partial class AddModPop : Form
    {
        

        public AddModPop()
        {
            InitializeComponent();
        }

        public Form1.Mods ADD()
        {
            Form1.Mods mod = new Form1.Mods();
            mod.Name = this.textBox1.Text.ToString();
            mod.Type = this.textBox2.Text.ToString();
            mod.Price = this.textBox3.Text.ToString();


            return mod;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
