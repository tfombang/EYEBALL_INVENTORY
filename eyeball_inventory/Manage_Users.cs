using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DevExpress.XtraEditors;

namespace EYEBALL_INVENTORY
{
    public partial class Manage_Users : XtraForm
    {
        Security security = new Security();

        MyConnection connectionObject = new MyConnection();
        MySqlCommand cmd = new MySqlCommand();
        MySqlDataReader reader;
        string id, password, accounttype;


        public Manage_Users()
        {
            InitializeComponent();

            DevExpress.Skins.SkinManager.EnableFormSkins();

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "McSkin";
        }


        private void Users_Load(object sender, EventArgs e)
        {
            MySqlConnection cn = new MySqlConnection(connectionObject.connString);
            btnnew.Text = "New User";
            txtname.Enabled = false;
            txtpassword.Enabled = false;
            cbxAccounttype.Enabled = false;
            btnsave.Enabled = false;
            btnsave.Enabled = false;
            cn.Open();
            fill_list();


        }

        
        void fill_list()
        {
            MySqlConnection cn = new MySqlConnection(connectionObject.connString);
            cn.Open();
            listView1.Items.Clear();
            cmd.CommandText = "SELECT * FROM users";
            cmd.Connection = cn;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ListViewItem list = new ListViewItem(reader[0].ToString());
                list.SubItems.Add(reader[1].ToString());
                list.SubItems.Add(reader[2].ToString());
                listView1.Items.AddRange(new ListViewItem[] { list });
            }
            reader.Close();
            reader.Dispose();
        }


        private void btnnew_Click(object sender, EventArgs e)
        {
            MySqlConnection cn = new MySqlConnection(connectionObject.connString);
            cn.Open();
            if (btnnew.Text == "New User")
            {
                txtname.Enabled = true;
                txtname.Clear();
                txtpassword.Enabled = true;
                txtpassword.Clear();
                btnsave.Enabled = true;
                cbxAccounttype.Enabled = true;
                btnedit.Text = "Cancel";
                btnsave.Text = "Save";
                btnedit.Enabled = true;
                txtname.Focus();
                this.Width = 313;
                this.Height = 231;
            }
            else
            {
                DialogResult dr = new System.Windows.Forms.DialogResult();
                dr = XtraMessageBox.Show("Are you sure??", "iBallInventory  -  Deletes", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    cmd.CommandText = "DELETE FROM users WHERE username='" + txtname.Text + "'";
                    cmd.Connection = cn;
                    cmd.ExecuteNonQuery();
                    fill_list();
                    btnnew.Text = "New User";
                    XtraMessageBox.Show("Account Deleted", "iBallInventory  -  Deletes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnsave.Enabled = false;
                    txtname.ResetText();
                    txtname.Enabled = false;
                    txtpassword.ResetText();
                    txtpassword.Enabled = false;
                    cbxAccounttype.ResetText();
                    cbxAccounttype.Enabled = false;
                }
                else
                {
                    return;
                }
                
            }
            cn.Clone();   
        }


        private void btnedit_Click(object sender, EventArgs e)
        {
            if (btnedit.Text == "Edit Users")
            {
                this.Width = 593; 
                this.Height = 231;
                btnedit.Text = "Cancel";
                btnsave.Text = "Update";
                txtname.Enabled = true;
                txtpassword.Enabled = true;
                cbxAccounttype.Enabled = true;
            }
            else
            {
                this.Width = 313;
                this.Height = 231;
                btnedit.Text = "Edit Users";
                btnnew.Enabled = true;
                btnsave.Enabled = false;
                txtname.Clear();
                txtname.Enabled = false;
                txtpassword.Clear();
                txtpassword.Enabled = false;
                cbxAccounttype.ResetText();
                cbxAccounttype.Enabled = false;
                btnnew.Text = "New User";
            }
            
        }
    

        private void btnsave_Click(object sender, EventArgs e)
        {            
            MySqlConnection cn = new MySqlConnection(connectionObject.connString);
            cn.Open();

            string dbUserID = txtname.Text.ToString();
            string dbPassword = txtpassword.Text.ToString();

            //Convert the Input Password to MD5 Hash string
            string hashPassword = security.ConvertToMD5(dbPassword);
            if (btnsave.Text=="Save")
            {
                if (txtname.Text == "" || txtpassword.Text == "" || cbxAccounttype.Text == "")
                {
                    XtraMessageBox.Show("Please Fill in the required fields for creating the user account","iBallInventory  -  Updates",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    txtname.Focus();
                }
                else
                {
                    try
                    {
                        cmd.CommandText = "INSERT INTO users (username, password, accounttype) VALUES('" + txtname.Text + "', '" + hashPassword.ToString() + "', '" + cbxAccounttype.Text + "')";
                        cmd.Connection = cn;
                        cmd.ExecuteNonQuery();
                        XtraMessageBox.Show("User Created", "iBallInventory  -  Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        fill_list();
                        btnnew.Enabled = true;
                        btnsave.Enabled = false;
                        btnedit.Enabled = true;
                        btnedit.Text = "Edit Users";
                        txtname.Clear();
                        txtname.Enabled = false;
                        txtpassword.Clear();
                        txtpassword.Enabled = false;
                        cbxAccounttype.ResetText();
                        cbxAccounttype.Enabled = false;
                    }
                    catch (MySqlException ex)
                    {
                        XtraMessageBox.Show(ex.Message);
                    }
                    
                }
                
            }
            else
            {
                try
                {
                    if (hashPassword == password && id == txtname.Text)
                    {

                        XtraMessageBox.Show("Account Successfully Updated with no Change"+"\ndbPwd= "+dbPassword+"\nPwd= "+password, "iBallInventory   -   Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        fill_list();
                        this.Width = 313;
                        this.Height = 231;
                        btnedit.Show();
                        btnsave.Enabled = false;
                        btnnew.Text = "New User";
                        btnedit.Text = "Edit Users";
                        txtname.ResetText();
                        txtname.Enabled = false;
                        txtpassword.ResetText();
                        txtpassword.Enabled = false;
                        cbxAccounttype.ResetText();
                        cbxAccounttype.Enabled = false;
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE users SET username='" + txtname.Text + "', password='" + hashPassword + "', accounttype='" + cbxAccounttype.Text + "' WHERE username='" + id + "'";
                        cmd.Connection = cn;
                        cmd.ExecuteNonQuery();

                        XtraMessageBox.Show("Account Successfully Updated with some Change" + "\ndbPwd= " + dbPassword + "\nPwd= " + password, "iBallInventory   -   Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //XtraMessageBox.Show("Account Successfully Updated with Change", "iBallInventory   -   Updates", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        fill_list();
                        this.Width = 313;
                        this.Height = 231;
                        btnedit.Show();
                        btnsave.Enabled = false;
                        btnnew.Text = "New User";
                        btnedit.Text = "Edit Users";
                        txtname.ResetText();
                        txtname.Enabled = false;
                        txtpassword.ResetText();
                        txtpassword.Enabled = false;
                        cbxAccounttype.ResetText();
                        cbxAccounttype.Enabled = false;
                    }

                    
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "iBallInventory   -   Updates,", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtname.Text = listView1.Items[listView1.FocusedItem.Index].SubItems[0].Text;
            id = txtname.Text;
            txtpassword.Text = listView1.Items[listView1.FocusedItem.Index].SubItems[1].Text;
            password = txtpassword.Text;
            cbxAccounttype.Text = listView1.Items[listView1.FocusedItem.Index].SubItems[2].Text;
            accounttype = listView1.Items[listView1.FocusedItem.Index].SubItems[2].Text.ToString();
            btnnew.Text = "Delete User";
            btnnew.Enabled = true;
            btnsave.Enabled = true;
            cbxAccounttype.Enabled = true;
        }

        private void Adminstrator_FormClosing(object sender, FormClosingEventArgs e)
        {
            ADMIN_MENU AdminMenu = new ADMIN_MENU();
            AdminMenu.Show();
        }
    }
}
