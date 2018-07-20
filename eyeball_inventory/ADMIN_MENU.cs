using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace EYEBALL_INVENTORY
{
    public partial class ADMIN_MENU : XtraForm
    {
        public ADMIN_MENU()
        {
            InitializeComponent();

            DevExpress.Skins.SkinManager.EnableFormSkins();

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "McSkin";
        }

        private void ADMIN_MENU_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            Login loginForm = new Login();
            loginForm.Show();
        }

        private void btn_ManageUsers_ItemClick(object sender, TileItemEventArgs e)
        {
            this.Hide();
            Manage_Users Users = new Manage_Users();
            Users.Show();
        }

        private void Manage_Database_ItemClick(object sender, TileItemEventArgs e)
        {
            this.Hide();
            Manage_Database DatabaseMgt = new Manage_Database();
            DatabaseMgt.Show();
        }

        private void Logs_ItemClick(object sender, TileItemEventArgs e)
        {
            this.Hide();
            Sales_Log SalesLog = new Sales_Log();
            SalesLog.Show();
        }

    }
}
