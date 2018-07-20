using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using DevExpress.XtraEditors;
using System.Data;

namespace EYEBALL_INVENTORY
{
    class Database
    {
        public static string connString = "server=localhost; uid=root;password=; database=EyeBInventory";

        public static void connection()
        {
            MySqlConnection cn = new MySqlConnection(connString);
            try
            {
                if (cn.State == ConnectionState.Closed)
                    cn.Open();
            }
            catch (MySqlException MyEx)
            {
                XtraMessageBox.Show("Connection Error!!" + MyEx.Message, "Db Connection Error");
            }
        }

        public class ConnParams
        {
            public MySqlConnection cn = new MySqlConnection(connString);
            public MySqlCommand cmd = new MySqlCommand();
            public DataTable dt = new DataTable();
            public MySqlDataAdapter da = new MySqlDataAdapter();
            public DataSet ds = new DataSet();

            //Method to run MySQl Queries
            public void RunQuery(string sqlCommand)
            {
                cn.Open();
                cmd.Connection = cn;
                cmd.CommandText = sqlCommand;
                cmd.ExecuteNonQuery();

                XtraMessageBox.Show("Done!!", "EyBall IMS 1.0");
            }
        }
    }
}
