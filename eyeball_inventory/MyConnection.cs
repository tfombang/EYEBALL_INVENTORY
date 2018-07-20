using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using MySql.Data.MySqlClient;
using DevExpress.XtraEditors;

namespace EYEBALL_INVENTORY
{
    class MyConnection
    {
        public string connString = @"HOST=localhost; uid=root; password=; database=eyebInventory";
 

        public void connect()
        {
            MySqlConnection cn = new MySqlConnection(connString);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
                XtraMessageBox.Show(cn.State.ToString());
            }
            catch (MySqlException myExcp)
            {
                XtraMessageBox.Show("Oops!! " + myExcp.Message + " Please contact Your Datbase Administrator");
            }
        }
    }
}
