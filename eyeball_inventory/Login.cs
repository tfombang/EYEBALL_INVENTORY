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
using System.Threading;

namespace EYEBALL_INVENTORY
{
    public partial class Login : XtraForm
    {
        //string username;
        //string password;
        //string accountType;

        MySqlConnection cn = new MySqlConnection();
        MySqlCommand cmd = new MySqlCommand();
        MyConnection conObject = new MyConnection();

        public Login()
        {
            //CREATING THE THREAD TO RUN THE SPLASH SCREEN
            Thread splashThread = new Thread(new ThreadStart(SplashStart));
            splashThread.Start();
            //Set the time within which the thread executing the SplashScreen() method stays on before it is aborted
            Thread.Sleep(5900); 

            InitializeComponent();
            
            splashThread.Abort();
            DevExpress.Skins.SkinManager.EnableFormSkins();

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "McSkin";

            
            //Environment.Exit(0);//Can exit the entire appliaction at this stage
        }

        /******************Method to start the Splash Screen********************************/
        public void SplashStart()
        {
            //Application.Run(new SplashScreen());
        }
/*******************************************************************************************/

        private void Login_Load(object sender, EventArgs e)
        {
            txtUsername.ForeColor = Color.Gray;
            txtPassword.ForeColor = Color.Gray;
            txtPassword.Properties.UseSystemPasswordChar = false;
            txtUsername.Focus();
        }

        private void txtUsername_Click(object sender, EventArgs e)
        {
            Reset_UsernameInput();
        }

        private void txtPassword_Click(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
            txtPassword.ForeColor = Color.Black;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtPassword.Properties.UseSystemPasswordChar = true;
        }

        private void txtUsername_Enter(object sender, EventArgs e)
        {
            Reset_UsernameInput();
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            Reset_PasswordInput();
        }

        void Reset_UsernameInput()
        {
            txtUsername.SelectAll();
            txtUsername.ForeColor = Color.Black;
        }

        void Reset_PasswordInput()
        {
            txtPassword.SelectAll();
            txtPassword.ForeColor = Color.Black;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //TODO authentication code here

            Accounts user = new Accounts();
            user.username = txtUsername.Text.ToString();
            user.username.Trim();
            user.password = txtPassword.Text.ToString();
            user.password.Trim();

            //XtraMessageBox.Show(Color.LightGoldenrodYellow.ToString(), "\t\t\t\t Testing!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //checkDetails(user.username.ToString(), user.password.ToString(), user.accountType.ToString());
            try
            {
                Authenticate(user.username.ToString(), user.password.ToString());
            }
            catch (Exception myEx)
            {
                XtraMessageBox.Show("Oops!! " + myEx.Message + "\nPlease contact your Database Administrator", "EyeBInventory", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }            

        }

        void Authenticate(string username, string password)
        {
            MySqlConnection cn = new MySqlConnection(conObject.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();

            cmd.CommandText = "SELECT * FROM users WHERE username = '" + username + "' AND password = '" + password + "'";
            cmd.Connection = cn;
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Close();

                cmd.CommandText = "SELECT * FROM users WHERE userName = '" + username + "' AND password = '" + password + "'";
                cmd.Connection = cn;
                MySqlDataReader reader2;
                reader2 = cmd.ExecuteReader();
                while (reader2.Read())
                {
                    string accountType = reader2[2].ToString();

                    //Select Account Type and Log Current User
                    switch (accountType)
                    {
                        case "SALES": SALES salesForm = new SALES();

                            //Log the user who has sucessfullky logged in
                            LogUser(username);

                            this.Hide();
                            salesForm.Show();
                            break;

                        case "STOCK": STOCK_MENU stock_Menu_Form = new STOCK_MENU();

                            //Log the user who has sucessfullky logged in
                            LogUser(username);

                            this.Hide();
                            stock_Menu_Form.Show();
                            break;

                        case "ADMINISTRATOR": //XtraMessageBox.Show("Welcome to Admin!!\t UNDER DEVELOPMENT");
                            ADMIN_MENU AdminMenu = new ADMIN_MENU();
                            this.Hide();
                            AdminMenu.Show();

                            //Log the user who has sucessfullky logged in
                            LogUser(username);

                            break;
                        default: return;
                    }
                }
            }
            else
            {
                XtraMessageBox.Show("Invalid username and/or password provided", "iBInventory v1.0", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        void LogUser(string username)
        {
            //This method will log to the file "utility" the current user who i logged to the system.
            System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.StartupPath + "/UTILITY/user_Log.txt");
            writer.Write(username.ToString());
            writer.Close();
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

    }
}
