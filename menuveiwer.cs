using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static JojoPos.Form1;

namespace JojoPos
{
    public partial class menuveiwer : Form
    {
        public menuveiwer()
        {
            InitializeComponent();
        }
        List<items> listOfItem = new List<items>();
        List<Mods> ListofMods = new List<Mods>();
        menu _MENU = new menu();
        string path = Application.StartupPath.ToString() + "\\files\\Menu\\Menu.xml";
        items TempITEMS;

        private void menuveiwer_Load(object sender, EventArgs e)
        {

            comboBox1.Items.Clear();
            textBox1.Clear();
            _MENU.Name = "JOJO MENU";
          
           _MENU = _MENU.LoadMenu(path);
         

            //Get all the cat in .file
            List<string> listofcats = new List<string>();
            foreach (var item in _MENU.ListofItems) 
            {
                
                bool There = false;
                foreach (var cat in listofcats)
                {
                    if(item.Cat == cat)
                    {
                        There = true;
                    }
                }
                if(There == false)
                {
                    listofcats.Add(item.Cat.ToString());
                }
            }
            foreach (var item in listofcats)
            {
                comboBox1.Items.Add(item);
                comboBox2.Items.Add(item);
            }
            comboBox2.Text = "";
            comboBox1.Text = comboBox1.Items[0].ToString();
           comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

          
            

        }
     

        public class menu
        {
            private string _Name;

            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }

            private List<items> _ListofItems;

            public List<items> ListofItems
            {
                get => _ListofItems; set { _ListofItems = value; }
            }

            public void SaveMenu(string fileName)
            {

                using (var stream = new FileStream(fileName, FileMode.Create))
                {
                    var Xml = new XmlSerializer(typeof(menu));
                    Xml.Serialize(stream, this);
                    stream.Close();
                }

            }
            public menu LoadMenu(string fileName)
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    var Xml = new XmlSerializer(typeof(menu));
                    var test = (menu)Xml.Deserialize(stream);
                    return test;


                }

            }

        }

       
        private void button1_Click(object sender, EventArgs e)
        {

             Mods mod = new Mods();
          items item = new items();
          mod.Name = "mod";
          mod.Price = "-0.00";
          mod.Type = "No";
          ListofMods.Add(mod);
          item.Name = "null";
          item.Mods = ListofMods;
          item.LucnhPrice = "0.00";
          item.DinnerPrice = "0.00";
          item.Cat = textBox1.Text.ToString();
            listOfItem.Clear();
          listOfItem.Add(item);
          _MENU.ListofItems.AddRange(listOfItem);

          _MENU.SaveMenu(path);
            EventArgs es = new EventArgs();
            menuveiwer_Load(this,es);


        }

        

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string SelectedName,selectedCat,SelectedLP,SelectedDP;
            string Change;
          


        }

        private void button2_Click(object sender, EventArgs e)
        {
            var selectedName = comboBox1.Text.ToString();
            foreach (var item in _MENU.ListofItems)
            {
                if(item.Cat == selectedName)
                {
                    _MENU.ListofItems.Remove(item);
                    break;
                }
            }
            _MENU.SaveMenu(path);

            EventArgs es = new EventArgs();
            menuveiwer_Load(this, es);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            comboBox2.Text = "";
            button5.Enabled = false;
            foreach (var item in _MENU.ListofItems)
            {
                if(item.Cat == comboBox1.SelectedItem.ToString())
                {
                    listBox1.Items.Add(item.Name);

                }
            }
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem.ToString() != null)
            {
                foreach (var item in _MENU.ListofItems)
                {
                    if (item.Name == listBox1.SelectedItem.ToString())
                    {
                        textBox2.Text = item.Name;
                        comboBox2.Text = item.Cat;
                        textBox3.Text = item.LucnhPrice;
                        textBox4.Text = item.DinnerPrice;
                        TempITEMS = item;
                        button5.Enabled = true;
                    }
                }
            }
           

        }

      
        private void button3_Click(object sender, EventArgs e)
        {
            var selected = comboBox1.SelectedIndex;
            items temptempitem = TempITEMS;
            TempITEMS.Name = textBox2.Text;
            TempITEMS.Cat = comboBox2.Text;
            TempITEMS.LucnhPrice = textBox3.Text;
            TempITEMS.DinnerPrice = textBox4.Text;
            TempITEMS.DinnerPrice = textBox4.Text;
            //need mods
            TempITEMS = TempITEMS;
            int count = 0;
            foreach (var item in _MENU.ListofItems)
            {

                if (item == temptempitem)
                {
                    _MENU.ListofItems[count] = TempITEMS;
                    _MENU.SaveMenu(path);
                    EventArgs es = new EventArgs();
                    menuveiwer_Load(this, es);
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    comboBox2.Text = "";
                    comboBox1.SelectedIndex = selected;
                    MessageBox.Show("Save Changed");

                    break;
                }
                else
                {
                    count += 1;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var selected = comboBox1.SelectedIndex;
            Mods mod = new Mods();
            items item = new items();
            mod.Name = "mod";
            mod.Price = "-0.00";
            mod.Type = "No";
            ListofMods.Add(mod);
            item.Name = "newItem";
            item.Mods = ListofMods;
            item.LucnhPrice = "0.00";
            item.DinnerPrice = "0.00";
            item.Cat = comboBox1.Text.ToString();
            listOfItem.Clear();
            listOfItem.Add(item);
            _MENU.ListofItems.AddRange(listOfItem);

            _MENU.SaveMenu(path);
            EventArgs es = new EventArgs();
            menuveiwer_Load(this, es);
            comboBox1.SelectedIndex = selected;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            items temptempitem = TempITEMS;
            TempITEMS.Name = textBox2.Text;
            TempITEMS.Cat = comboBox2.Text;
            TempITEMS.LucnhPrice = textBox3.Text;
            TempITEMS.DinnerPrice = textBox4.Text;
            TempITEMS.DinnerPrice = textBox4.Text;
            //need mods
            TempITEMS = TempITEMS;
            int count = 0;
            foreach (var item in _MENU.ListofItems)
            {

                if (item == temptempitem)
                {
                    _MENU.ListofItems.Remove(item);
                    _MENU.SaveMenu(path);
                    EventArgs es = new EventArgs();
                    menuveiwer_Load(this, es);
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    comboBox2.Text = "";
                    MessageBox.Show("Removed");

                    break;
                }
                else
                {
                    count += 1;
                }
            }
        }
    }
}

