using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Globalization;
using static JojoPos.menuveiwer;

namespace JojoPos
{
    public partial class Form1 : Form
    {
        StreamWriter sw;
        StreamReader sr;
        XmlDocument xdoc;
        client GOD = new client();
        string eatinpath, takeoutpath;
        string fpath;
        menu _MENU = new menu();
        string root = Application.StartupPath.ToString();
        string Menupath = Application.StartupPath.ToString() + "\\files\\Menu\\Menu.xml";
        DirectoryInfo di;
        FileInfo fi;
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            //Load Day Combo
            //This is a test
            string path = root + "\\files\\Archive\\";

            di = new DirectoryInfo(path);
            comboBox1.Items.Add("Today");
            foreach (var item in di.GetFiles())
            {
                comboBox1.Items.Add(item);
            }
            comboBox1.SelectedIndex = 0;

            _MENU.Name = "JOJO MENU";

            _MENU = _MENU.LoadMenu(Menupath);

            //Get all the cat in .file
            List<string> listofcats = new List<string>();
            foreach (var item in _MENU.ListofItems)
            {

                bool There = false;
                foreach (var cat in listofcats)
                {
                    if (item.Cat == cat)
                    {
                        There = true;
                    }
                }
                if (There == false)
                {
                    listofcats.Add(item.Cat.ToString());
                }
            }
            foreach (var item in listofcats)
            {
             
                comboBox2.Items.Add(item);
            }
            comboBox2.Text = comboBox1.Items[0].ToString();
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.SelectedIndex = 0;
        }

        private void LoadDay(string SelectedDay)
        {
            //START HERE

            // Load TakeOut
            if(SelectedDay != "Today")
            {
                string Path = root + "\\files\\Archive\\";
               
            }
            else
            {
                string Path = root + "\\files\\Today\\";
                di = new DirectoryInfo(Path);
                foreach (var item in di.GetDirectories())
                {
                    
                    if(item.ToString() == "EatIn")
                    {
                        
                        Path = root + "\\files\\Today\\EatIn";
                        eatinpath = Path;
                        di = new DirectoryInfo(Path);
                        foreach (var item2 in di.GetFiles())
                        {
                            listBox3.Items.Add(item2.ToString().Replace(".xml", ""));
                        }


                    }
                    //load SitDOwn
                    else if (item.ToString() == "TakeOut")
                    {
                       
                        Path = root + "\\files\\Today\\TakeOut";
                        takeoutpath = Path;
                        di = new DirectoryInfo(Path);
                        foreach (var item2 in di.GetFiles())
                        {
                            listBox1.Items.Add(item2.ToString().Replace(".xml",""));
                        }
                    }
                   
                }
               
            }
        }
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            
           LoadDay( comboBox1.Text);
        }

        private void listBox3_Click(object sender, EventArgs e)
        {
            listBox1.ClearSelected();
            loadCust(listBox3.SelectedItem.ToString() + ".xml");
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            listBox3.ClearSelected();
            loadCust(listBox1.SelectedItem.ToString()+".xml");

        }

        private void loadCust(string Cust)
        {
            string whatDay = comboBox1.Text;
            string path;
            //add file dir based off day or archive
            if( whatDay == "Today")
            {
                 path = root + "\\files\\Today\\";
            }
            else
            {
                path = root + "\\files\\Archive\\";
            }


            //if the selected custmer from either listbox dinein/dineout
            // if it table add to dir for table or takeout
            if (Cust.Contains("Table"))
            {

                path += "EatIn\\";
                di = new DirectoryInfo(path);

                foreach (var item in di.GetFiles())
                {
                    if (item.Name == Cust)
                    {

                        fi = new FileInfo(path + item.Name);
                        break;
                    }
                }

                fi = fi;
            }
            else
            {
                path += "TakeOut\\";

                di = new DirectoryInfo(path);

                foreach (var item in di.GetFiles())
                {
                    if (item.Name == Cust)
                    {

                        fi = new FileInfo(path + item.Name);
                        break;
                    }
                }
            }

            // has full file dir and should have right file loaded
            //load in to cloent class wich reads/writes cust files and saves them
                string f = fi.FullName.ToString();                               
                GOD = GOD.LoadClient(f);
            fpath = f;

            //puts clients info onto form1 and builds recipt

                LblName.Text = GOD.Name.ToString();
                List<string> Recipt = new List<string>();
                decimal price = 0m;
                listBox2.Items.Clear();
            //loads clients listofitems
                foreach (var item in GOD.ListofItems)
                {
                    var a = item;
                    //TEST CoDe
                    //end TEST CoDE
                    Recipt.Add("|" + a.Name.ToString() + " \t\t\t\t  " + a.LucnhPrice.ToString());
                    var t = a.LucnhPrice.ToString();
                    t = t.Replace("$", "");
                    price += Convert.ToDecimal(t);
                    if (a.Mods.Count != 0)
                    {
                        foreach (var mod in a.Mods)
                        {

                            if (mod.Price.Contains("-") || mod.Price.Contains("+"))
                            {
                                if (mod.Price.Contains("-"))
                                {
                                    string s = mod.Price;
                                    s = s.Replace("-", "");

                                    price -= Convert.ToDecimal(s);

                                    Recipt.Add("|--MOD: " + mod.Type + " " + mod.Name + " \t\t\t  $-" + s);

                                }
                                else
                                {
                                    string s = mod.Price;
                                    s = s.Replace("+", "");

                                    price += Convert.ToDecimal(s);
                                    Recipt.Add("|--MOD: " + mod.Type + " " + mod.Name + " \t\t\t  $+" + s);

                                }
                            }

                        }
                                           }
                Recipt.Add("---------------------------------------------------------------------------------------------");
            }
            CultureInfo culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                            culture.NumberFormat.CurrencyNegativePattern = 1;
                            
            // IM HERE TRYING TO GET THE PRICE FORMATE RIGHT AND ACCORIT
            Recipt.Add("\t\t\tSubTotal $" + string.Format(culture,"{0:C}", price));

                var r = price * 0.07m;
                price += r;
                Recipt.Add("\t\t\t     TAX " + string.Format(culture,"{0:C}", r));
                Recipt.Add("\t\t\t     Total " + string.Format(culture,"{0:C}", price));
                Recipt.Add("---------------------------------------------------------------------------------------------");

                foreach (var item in Recipt)
                {
                    listBox2.Items.Add(item);
                }
                /*  items tempitem = new items();
                  Mods TempMod = new Mods,
                  List<items> TempList = new List<items>();
                  List<Mods> TempModList = new List<Mods>();
                  tempitem.Name = "stake";
                  tempitem.Cat = "Starters";
                  tempitem.LucnhPrice = "$40.00";
                  tempitem.DinnerPrice = "$60.00";


                  TempMod.Name = "Rice";
                  TempMod.Price = "-3.00";
                  TempMod.Type = "No";
                  TempModList.Add(TempMod);

                 TempMod.Name = "Stake";
                 TempMod.Price = "+4.00";
                 TempMod.Type = "Extra";
                 TempModList.Add(TempMod);

                 TempMod.Name = "Salt";
                 TempMod.Price = "-0.10";
                 TempMod.Type = "No";
                 TempModList.Add(TempMod);

                 tempitem.Mods = TempModList;
                 TempList.Add(tempitem);

                  GOD.Name = Cust;
                 GOD.ListofItems.AddRange(TempList);

                  GOD.SaveClient(f);*/

                GOD = GOD;
    }
     
    public class client
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

            public void SaveClient(string fileName)
            {

                using (var stream = new FileStream(fileName, FileMode.Create))
                {
                    var Xml = new XmlSerializer(typeof(client));
                    Xml.Serialize(stream, this);
                    stream.Close();
                }

            }
            public client LoadClient(string fileName)
            {
                using (var stream = new FileStream(fileName, FileMode.Open))
                {
                    var Xml = new XmlSerializer(typeof(client));
                    var test = (client)Xml.Deserialize(stream);
                    return test ;                  
                }
            }
        }

    public class items
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _LunchPrice;

        public string LucnhPrice
        {
            get { return _LunchPrice; }
            set { _LunchPrice = value; }
        }
        private string _DinnerPrice ;

        public string DinnerPrice
        {
            get { return _DinnerPrice; }
            set { _DinnerPrice = value; }
        }

        private string _Cat;

        public string Cat
        {
            get { return _Cat; }
            set { _Cat = value; }
        }
        private List<Mods> _Mods;

        public List<Mods> Mods
        {
            get { return _Mods; }
            set { _Mods = value; }
        }
    }

    public class Mods
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private string _Type;

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        private string _Price;

        public string Price
        {
            get { return _Price; }
            set { _Price = value; }
        }
    }
        private void listBox2_Click(object sender, EventArgs e)
        {
            button5.Enabled = false;
            button4.Enabled = false;
            button6.Enabled = false;
            // this is to select item from recipt know what the fuck it is 
            // Toggle buttons that r needed for editing or deleteing or adding
            // make sure that the recipt load back best to do a foruc save then have loadCut reload

            //    MessageBox.Show("THIS IS WHAT U WANTED DUMBASS");
            string selecteditem = listBox2.SelectedItem.ToString();
            if (selecteditem.Contains("MOD"))
            {              
                button5.Enabled = true;
            }
            else if(selecteditem.Contains("----------"))
            {
                MessageBox.Show("---------------------------------------");
            }
            else if (!selecteditem.Contains("MOD") && selecteditem.Contains("|"))
            {          
                button4.Enabled = true;
                button6.Enabled = true;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            GOD = new client();
            GOD.Name = "Temp";
            LoadDay(comboBox1.Text.ToString());
            
            GOD.SaveClient(takeoutpath+"\\"  + GOD.Name + ".xml");
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
        private void button5_Click(object sender, EventArgs e)
        {
            GOD = GOD;
            foreach (var item in GOD.ListofItems)
            {
               
                    foreach (var item2 in item.Mods)
                    {
                        if(listBox2.SelectedItem !=null)
                      {
                    if (listBox2.SelectedItem.ToString().Contains(item2.Name) && listBox2.SelectedItem.ToString().Contains(item2.Type))
                        {
                         
                            item.Mods.Remove(item2);
                            fpath = fpath;
                            GOD.SaveClient(fpath);
                            listBox2.Items.Clear();
                            loadCust(GOD.Name + ".xml");
                            button5.Enabled = false;

                            break;

                        }
                    }
                        
                    }
                
                
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            menuveiwer menuve = new menuveiwer();
            menuve.Show();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string price;
            listBox4.Items.Clear();
            foreach (var item in _MENU.ListofItems)
            {
                if(item.Cat == comboBox2.Text)
                {
                    if(DateTime.Now.Hour >=16)
                    {
                        price = item.DinnerPrice;
                    }
                    else
                    {
                        price = item.LucnhPrice;
                    }
                    listBox4.Items.Add(item.Name);
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToShortTimeString();
        }
        private void listBox4_DoubleClick(object sender, EventArgs e)
        {
            
            foreach (var item in _MENU.ListofItems)
            {
                if(item.Name == listBox4.SelectedItem.ToString())
                {
                    item.Mods.Clear(); ;
                    GOD.ListofItems.Add(item);
                    GOD.SaveClient(fpath);
                    listBox2.Items.Clear();
                    loadCust(GOD.Name + ".xml");
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            GOD = GOD;
            int i = 0;

            List<items> TEmp = new List<items>();
            items tesmp2 = new items();
            foreach (var line in listBox2.Items)
                {
                if (line.ToString().Contains("|") && line.ToString().Contains("\t"))
                {
                    var _line = line.ToString();
                    var _Name = _line.Substring(1, _line.IndexOf("\t")-3);
                    _line = _line.Remove(0, _line.IndexOf("\t"));
                    _line = _line.Remove(0, _line.IndexOf(" ") +1);
                    var _Price = _line;
                    tesmp2.Name = _Name;
                    tesmp2.LucnhPrice = _Price;
                   
                }
                else if (line.ToString().Contains("|-MOD"))
                {
                    var _line = line.ToString();
                }
                else if (line.ToString().Contains("-------------"))
                {
                    TEmp.Add(tesmp2);
                   
                }
                


            }           
                      /*  GOD.ListofItems.Remove(item);
                        // NEED TO FIND HOW ITEM AND REMOVE THE FUCKERE
                       
                            fpath = fpath;
                            GOD.SaveClient(fpath);
                            listBox2.Items.Clear();
                            loadCust(GOD.Name + ".xml");
                            button5.Enabled = false;

                           */     
        }
        private void button6_Click(object sender, EventArgs e)
        {
            //NEED POPUP
            AddModPop pop = new AddModPop();
            Mods newMOd = new Mods();
            pop.ShowDialog();
           newMOd = pop.ADD();

            foreach (var item in GOD.ListofItems)
            {
                if (listBox2.SelectedItem != null)
                {
                    if (listBox2.SelectedItem.ToString().Contains(item.Name))
                    {
                        item.Mods.Add(newMOd);
                        GOD = GOD;
                        fpath = fpath;
                        GOD.SaveClient(fpath);
                        listBox2.Items.Clear();
                        loadCust(GOD.Name + ".xml");
                        button6.Enabled = false;

                    }                 
                }
            }
        }
    }
}
