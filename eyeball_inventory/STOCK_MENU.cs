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
    public partial class STOCK_MENU : XtraForm
    {
        public STOCK_MENU()
        {
            InitializeComponent();
            DevExpress.Skins.SkinManager.EnableFormSkins();

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "McSkin";
        }

        private void Manage_Categories_Button_Click(object sender, DevExpress.XtraEditors.TileItemEventArgs e)
        {
            Categories categoriesForm = new Categories();
            this.Hide();
            categoriesForm.Show();
        }

        private void STOCK_MENU_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
            Login loginForm = new Login();
            loginForm.Show();
        }

        private void Manage_Items_Click(object sender, TileItemEventArgs e)
        {
            STOCK stockForm = new STOCK();
            this.Hide();
            stockForm.Show();
        }

        private void Stock_Report_Click(object sender, TileItemEventArgs e)
        {
            //TODO Code here to open the STOCK REPORT Form
        }
    }
}
