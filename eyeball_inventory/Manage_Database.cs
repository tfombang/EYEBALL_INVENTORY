using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using DevExpress.XtraEditors;

namespace EYEBALL_INVENTORY
{
    public partial class Manage_Database : XtraForm
    {
        string action;
        MyConnection conOnject = new MyConnection();
        MySqlCommand cmd = new MySqlCommand();
        MySqlDataReader dr;

        public Manage_Database()
        {
            InitializeComponent();

            DevExpress.Skins.SkinManager.EnableFormSkins();

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "McSkin";
        }


        private void Managedatabase_Form_Load(object sender, EventArgs e)
        {
            action = cbxMode.Text.ToString();
            btnBackup.Text = "";
            refreshListBox();
            MySqlConnection cn = new MySqlConnection(conOnject.connString);
            cmd.CommandText = "SELECT * FROM backup";
            cmd.Connection = cn;

            try
            {
                cn.Open();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
            }   
        }

        private void cbxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxMode.Text == "BACKUP")
            {
                btnBackup.Text = "BACKUP";
                label3.Text = "Backup to:";
            }
            else
            {
                btnBackup.Text = "RESTORE";
                label3.Text = "Restore from:";
            }
        }
        
        private void btnBackup_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Backup Location";
            sfd.Filter = "SQL File |*.sql|CSV File |*.csv|XML File|*.xml";
            sfd.OverwritePrompt = false;

            if (cbxMode.Text == "BACKUP")
            {
                sfd.FileName = "Backup_of_" + DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Year.ToString();

                DialogResult result = sfd.ShowDialog();
                if (result != DialogResult.Cancel)
                {
                    txtLoc.Text = sfd.FileName;
                }
            }
            else
                if (cbxMode.Text == "RESTORE")
                {
                    sfd.FileName = "";

                    DialogResult result = sfd.ShowDialog();
                    if (result != DialogResult.Cancel)
                    {
                        txtLoc.Text = sfd.FileName;

                    }
                }
                else
                {
                    XtraMessageBox.Show("Please select either Backup or Restore Action", "Backup - iBallInventory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            
        }


/**************************** BACKUP METHOD **********************************************/
        void backup()
        {
            DialogResult answer = XtraMessageBox.Show("Backup database into the following directory.\n" + txtLoc.Text + "\nThis may take a while depending on the size of the database" + "\nProceed with this action??\n", "Backup - iBallInventory", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes)
            {
                //XtraMessageBox.Show("About to Backup", "Backup - iBallInventory", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //Set the Backup Directory
                System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.StartupPath + "/PLUGINS/BACKUP.BAT");
                writer.WriteLine(" ");
                writer.WriteLine("mysqldump -u root -p --database sisbase>" + txtLoc.Text.Trim().ToString());
                writer.Close();

                Process proc = new Process();
                ProcessStartInfo procInfo = new ProcessStartInfo();
                procInfo.FileName = Application.StartupPath + "/PLUGINS/BACKUP.BAT";
                procInfo.WindowStyle = ProcessWindowStyle.Normal;
                proc.StartInfo = procInfo;
                
                try
                {
                    proc.Start();
                    //make a LOG of the backup date and time
                    string startTime = proc.StartTime.ToString();
                    //XtraMessageBox.Show("Process was started at exactly: " + startTime);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "Backup - iBallInventory", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                trackRecord();  //Register the Backup activity in the Database
                refreshListBox();   //Refresh the ListBox of Backup Records
                XtraMessageBox.Show("Backup process completed successfully", "iBallInventory   -   Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                return;
            }
            
        }

/******************************** RESTORE METHOD ****************************************/
        void restore()
        {
            MySqlConnection cn = new MySqlConnection(conOnject.connString);
            cmd.CommandText = "CREATE DATABASE IF NOT EXISTS sisbase";
            cmd.Connection = cn;
            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                //XtraMessageBox.Show("Database successfully created on the database system");

                DialogResult answer = XtraMessageBox.Show("Restore database from the following directory.\n" + txtLoc.Text + "\nThis may take a while depending on the size of the database" + "\nProceed with this action??\n", "Backup - iBallInventory", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == DialogResult.Yes)
                {
                    //XtraMessageBox.Show("About to Restore", "Backup - iBallInventory", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Set the Backup Directory
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.StartupPath + "/PLUGINS/BACKUP.BAT", true);
                    //writer.WriteLine("mysql -u root create database sisbase");
                    writer.WriteLine("mysqldump -u root sisbase<" + txtLoc.Text.Trim().ToString());
                    writer.Close();

                    Process proc = new Process();
                    ProcessStartInfo procInfo = new ProcessStartInfo();
                    procInfo.FileName = Application.StartupPath + "/PLUGINS/BACKUP.BAT";
                    procInfo.WindowStyle = ProcessWindowStyle.Normal;
                    proc.StartInfo = procInfo;
                    try
                    {
                        proc.Start();

                        XtraMessageBox.Show("Database successfully Restored !!", "Backup - iBallInventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message, "Backup - iBallInventory", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    //trackRecord();  //Register the Backup activity in the Database
                    refreshListBox();   //Refresh the ListBox of Backup Records
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Backup - iBallInventory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
            }
            return;
            
        }
/************************* STORE THE NAME OF THE BACKUP IN DATABASE *******************/
        private void trackRecord()
        {
            string today = DateTime.Today.Day.ToString() + "-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Month.ToString();

            MySqlConnection cn = new MySqlConnection(conOnject.connString);
            cmd.CommandText = "SELECT COUNT(num) FROM backup"; 
            cmd.Connection = cn; 
            cn.Open();
            Int16 count = Convert.ToInt16(cmd.ExecuteScalar());
            count++;
            //XtraMessageBox.Show("Count gives us: " + count);

            cmd.CommandText = "INSERT INTO backup  (backupDate) VALUES('BACKUP_OF" + today + " -> OPERATION NUMBER " + count.ToString() + "')";
            cmd.Connection = cn;
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Backup - iBallInventory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
                //XtraMessageBox.Show("Backup process completed successfully", "iBallInventory   -   Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        void refreshListBox()
        {
            listBox1.Items.Clear();
            MySqlConnection cn=new MySqlConnection(conOnject.connString);
            cmd.CommandText = "SELECT * FROM backup";
            cmd.Connection = cn;
            try
            {
                cn.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (dr.HasRows)
                    {
                        listBox1.Items.Add(dr["backupDate"].ToString().ToUpper());
                    }
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Backup - iBallInventory", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cn.Close();
            }
        }

        private void btnBackup_Click_1(object sender, EventArgs e)
        {
            if (txtLoc.Text != "")
            {
                action = cbxMode.Text.ToString();

                if (action == "BACKUP" || action == "RESTORE")
                {
                    switch (action)
                    {
                        case "BACKUP":
                            {
                                backup();
                                break;
                            }
                        case "RESTORE":
                            {
                                restore();
                                break;
                            }
                        default: return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                XtraMessageBox.Show("Please select a file location", "iBallInventory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }


        private void ManageDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            ADMIN_MENU AdminMenu = new ADMIN_MENU();
            AdminMenu.Show();
        }
        
    }
}
