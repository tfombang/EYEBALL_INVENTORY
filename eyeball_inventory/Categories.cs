using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;

namespace EYEBALL_INVENTORY
{
    public partial class Categories : XtraForm
    {
        MyConnection conObject = new MyConnection();
        MySqlCommand cmd;

        string catname = "";

        public Categories()
        {
            InitializeComponent();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            Database.connection();

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "McSkin";
            
        }

        private void Categories_Load(object sender, EventArgs e)
        {
            // TODO: 

            System.IO.StreamReader fileReader = new System.IO.StreamReader(Application.StartupPath + "/UTILITY/user_Log.txt");
            lblUerLog.Text = fileReader.ReadToEnd().ToString();

            MySqlConnection cn = new MySqlConnection(conObject.connString);
            string sqlCommand="SELECT * FROM categories";
            DataTable dt = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(sqlCommand, cn);
            try
            {
                fill_list();
            }
            catch (MySqlException myEx)
            {
                XtraMessageBox.Show("Oops!! " + myEx.Message);
            }


            Disable_Conrols_Load();
                
        }


        void fill_list()
        {
            MySqlConnection cn = new MySqlConnection(conObject.connString);
            cn.Open();
            lView.Items.Clear();
            MySqlCommand cmd;
            cmd = new MySqlCommand("SELECT * FROM categories", cn);
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ListViewItem list = new ListViewItem(reader[0].ToString().ToUpper());
                lView.Items.AddRange(new ListViewItem[] { list });
                if (list.Index % 2 == 0)
                {
                    list.BackColor = Color.LightGoldenrodYellow;
                }
            }
            reader.Close();
            reader.Dispose();
        }


        void Disable_Conrols_Load()
        {
            btnDelete.Enabled = false;
            btnSave.Enabled = false;
            txtCategoryName.Enabled = false;
            txtCategoryName.ResetText();
            btnNew.Enabled = true;
        }


        public void Enable_Controls_New()
        {
            txtCategoryName.ResetText();
            txtCategoryName.Enabled = true;
            btnSave.Text = "Save";
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            txtCategoryName.Focus();

            fill_list();
        }


        public void Enable_Controls_Update()
        {
            txtCategoryName.ResetText();
            txtCategoryName.Enabled = true;
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
            btnSave.Text = "Update";
            btnSave.Enabled = true;
        }


        private void btnNew_Click(object sender, EventArgs e)
        {
            Enable_Controls_New();
        }
        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteCategory();
            }
            catch (Exception)
            {
                throw;
            }
        }


        void DeleteCategory()
        {
            MySqlConnection cn = new MySqlConnection(conObject.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            string sqlCommand = "DELETE FROM categories WHERE category = '" + catname + "'";
            cmd = new MySqlCommand(sqlCommand, cn);
            cmd.ExecuteNonQuery();
            fill_list();
            XtraMessageBox.Show("Category Deeted Successfully!!");
            Disable_Conrols_Load();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            string decision = btnSave.Text.ToString();
            if (txtCategoryName.Text.ToString() != "" && txtCategoryName.Text.ToString() != null)
            {
                try
                {
                    switch (decision)
                    {
                        case "Save":
                            SaveData();
                            break;

                        case "Update":
                            UpdateData();
                            break;

                        default: return;
                    }
                }
                catch (MySqlException ex)
                {
                    XtraMessageBox.Show("Oops!! \n" + ex.Message);
                }
            }
            
            
        }


        void SaveData()
        {
            MySqlConnection cn = new MySqlConnection(conObject.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            MySqlCommand cmd;
            string sqlCommand = "SELECT * FROM categories WHERE category='" + txtCategoryName.Text.ToString() + "'";
            cmd = new MySqlCommand(sqlCommand, cn);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                XtraMessageBox.Show("Category ''" + txtCategoryName.Text + "'' already Exists in the Database");
                txtCategoryName.Enabled = true;
                btnSave.Enabled = true;
                txtCategoryName.Enabled = true;
                txtCategoryName.SelectAll();
                btnSave.Enabled = true;
                reader.Close();
                reader.Dispose();
            }
            else
            {
                reader.Close();
                reader.Dispose();
                sqlCommand = "INSERT INTO categories VALUES('" + txtCategoryName.Text.Trim().ToString() + "')";
                cmd = new MySqlCommand(sqlCommand, cn);
                int my = cmd.ExecuteNonQuery();
                XtraMessageBox.Show("Category ''" + txtCategoryName.Text.ToString() + "'' Successfully added");
                fill_list();
                Disable_Conrols_Load();
            }
            reader.Close();
            reader.Dispose();
            cn.Close();
        }


        void UpdateData()
        {
            MySqlConnection cn = new MySqlConnection(conObject.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            //Check if the new name already exists in the Database

            string sqlCommand = "SELECT * FROM categories WHERE category='" + txtCategoryName.Text.ToString() + "'";
            cmd = new MySqlCommand(sqlCommand, cn);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                XtraMessageBox.Show("Category *''" + txtCategoryName.Text + "''* already Exists in the Database", "EyeBInventory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCategoryName.Enabled = true;
                btnSave.Enabled = true;
                txtCategoryName.Enabled = true;
                txtCategoryName.SelectAll();
                btnSave.Enabled = true;
            }
            else
            {
                reader.Close();
                reader.Dispose();
                //Execute UPDATE on the 'categories' table
                sqlCommand = "UPDATE categories SET category='" + txtCategoryName.Text.Trim().ToString() + "' WHERE category = '" + catname + "'";
                cmd = new MySqlCommand(sqlCommand, cn);
                cmd.ExecuteNonQuery();

                //Execute UODATE on the 'stock' table
                sqlCommand = "UPDATE stock SET category='" + txtCategoryName.Text.ToString().ToUpper() + "' WHERE category='" + catname + "'";
                cmd = new MySqlCommand(sqlCommand, cn);
                cmd.ExecuteNonQuery();

                XtraMessageBox.Show("Updates Successfull!!", "EyeBInventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fill_list();
            }

            
        }


        private void listView_Click(object sender, ColumnClickEventArgs e)
        {
            

        }


        private void lView_SelectedIndexChanged(object sender, EventArgs e)
        {
            Enable_Controls_Update();
            catname = lView.Items[lView.FocusedItem.Index].SubItems[0].Text;
            catname.Trim();
            txtCategoryName.Text = catname;
        }


        private void txtCategoryName_Enter(object sender, EventArgs e)
        {
            txtCategoryName.SelectAll();
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                searchCategory();
            }
            catch (MySqlException myEx)
            {
                XtraMessageBox.Show("Oops!!\n"+myEx.Message+"\nPlease contact your databases administrator");
            }
        }


        private void txtSearch_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                searchCategory();
            }
            catch (MySqlException myEx)
            {
                XtraMessageBox.Show("Oops!!\n" + myEx.Message + "\nPlease contact your Database Administrator");
            }
        }


        private void txtSearch_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            searchCategory();
        }


        void searchCategory()
        {
            MySqlConnection cn = new MySqlConnection(conObject.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            string sqlCommand = "SELECT * FROM categories WHERE category LIKE '%" + txtSearch.Text.ToString() + "%'";
            //string sql = string.Format("SELECT * FROM mydata where fullname like section like '%" + txtSearch.Text + "%'");
            cmd = new MySqlCommand(sqlCommand, cn);
            MySqlDataReader dr = cmd.ExecuteReader();

            lView.Items.Clear();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    ListViewItem list = new ListViewItem(dr[0].ToString().ToUpper());
                    lView.Items.AddRange(new ListViewItem[] { list });
                    if (list.Index % 2 == 0)
                    {
                        list.BackColor = Color.LightGoldenrodYellow;
                    }
                }
                dr.Close();
                dr.Dispose();
            }
        }


        private void Categories_FormClosed(object sender, FormClosedEventArgs e)
        {
            STOCK_MENU stockMenu = new STOCK_MENU();
            this.Hide();
            stockMenu.Show();
        }


    }
}
