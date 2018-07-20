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
    public partial class Stock_Report : XtraForm
    {
        public Stock_Report()
        {
            InitializeComponent();

            DevExpress.Skins.SkinManager.EnableFormSkins();

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "McSkin";
        }
    }
}
