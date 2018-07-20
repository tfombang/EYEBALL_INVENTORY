using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using DevExpress.XtraEditors;

namespace EYEBALL_INVENTORY
{
    public partial class Sales_Log : XtraForm
    {
        MySqlDataReader reader;
        MyConnection conObj = new MyConnection();
        string sqlCommand = "";
        MySqlCommand cmd;
        string agent;
        string date = "";

        List<string> agents=new List<string>();


        public Sales_Log()
        {
            InitializeComponent();

            DevExpress.Skins.SkinManager.EnableFormSkins();

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "McSkin";
        }

        private void Sales_Log_Load(object sender, EventArgs e)
        {
            MySqlConnection cn = new MySqlConnection(conObj.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            sqlCommand = "SELECT username FROM users WHERE accountType = 'SALES'";
            cmd = new MySqlCommand(sqlCommand, cn);
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Create_Button(reader[0].ToString());
                }
            }
            reader.Close();
            reader.Dispose();

            ClearControls();

            date = DateTime.Today.ToShortDateString().ToString();
            lblDate.Text = date;
            txtDate.Text = date;
        }

        void ClearControls()
        {
            lblAgent.ResetText();
            lblDate.ResetText();
            lblIncome.ResetText();
            //txtDate.ResetText();
            dataGridView1.DataSource = null;
            //txtVoucherNumber.ResetText();
        }


        public void Create_Button(string salesAgent)
        {
            SimpleButton btnAgent = new SimpleButton();
            btnAgent.Name = salesAgent;
            btnAgent.Text = salesAgent;
            btnAgent.Font = new System.Drawing.Font("Agency FB", 20);
            btnAgent.Width = 160;
            btnAgent.Height = 55;
            btnAgent.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            tableLayoutPanel1.Controls.Add(btnAgent, 0,0);

            btnAgent.Click += new EventHandler(btnAgent_Click);
            
        }

        private void btnAgent_Click(object sender, EventArgs e)
        {
            ClearControls();
            SimpleButton btn = (SimpleButton) sender;

            btn.ForeColor = Color.Black;

            agent = btn.Text.ToString();
            lblAgent.Text = agent.ToUpper();
            lblDate.Text = date;

            string state = ChckBoxAllDate.CheckState.ToString();

            if (state == "Checked") //Get All Sales Records of current Agent
            {
                sqlCommand = "SELECT agent AS AGENT, transactionID AS VOUCHER_NUMBER, itemName AS ITEM, unitPrice AS UNINT_PRICE, quantity AS QUANTITY, grossPrice AS PAID_PRICE, transactionDate AS TRANSACTION_DATE FROM sales WHERE agent = '" + agent + "' ORDER BY transactionID DESC";
                LoadSalesRecord(sqlCommand);
                ComputeGrossPersonalSummary();
            }
            else  //Get Sales Records by date for each Agent
            {
                sqlCommand = "SELECT agent AS AGENT, transactionID AS VOUCHER_NUMBER, itemName AS ITEM, unitPrice AS UNINT_PRICE, quantity AS QUANTITY, grossPrice AS PAID_PRICE, transactionDate AS TRANSACTION_DATE FROM sales WHERE agent = '" + agent + "' AND transactionDate = '" + date + "' ORDER BY transactionID DESC";
                LoadSalesRecord(sqlCommand);
                ComputeSalesSummary();
            }

/********************************************/
            //MySqlConnection cn = new MySqlConnection(conObj.connString);
            //if (cn.State != ConnectionState.Open)
            //    cn.Open();
            //sqlCommand = "SELECT agent FROM sales";
            //cmd = new MySqlCommand(sqlCommand, cn);
            //reader = cmd.ExecuteReader();
            //if (reader.HasRows)
            //{
            //    while (reader.Read())
            //    {
            //        //MessageBox.Show(reader["agent"].ToString());
            //        if (btn.Text == reader["agent"].ToString())
            //        {
            //            btn.ForeColor = Color.Blue;
            //        }
            //        else
            //        {
            //            btn.ForeColor = Color.Black;
                        
            //        }
            //    }
            //}
/*******************************************/            
        }

        void LoadSalesRecord(string sqlCmd)
        {
            try
            {
                MySqlConnection cn = new MySqlConnection(conObj.connString);
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sqlCmd, cn);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            
        }

        private void txtVoucherNumber_EditValueChanged(object sender, EventArgs e)
        {
            if (txtVoucherNumber.Text == "" || txtVoucherNumber.Text == "0")
            {
                sqlCommand = "SELECT agent AS AGENT, transactionID AS VOUCHER_NUMBER, itemName AS ITEM, unitPrice AS UNINT_PRICE, quantity AS QUANTITY, grossPrice AS PAID_PRICE, transactionDate AS TRANSACTION_DATE FROM sales WHERE agent = '" + agent + "' ORDER BY transactionID DESC";
                LoadSalesRecord(sqlCommand);
            }
            else
            {
                MySqlConnection cn = new MySqlConnection(conObj.connString);
                if (cn.State != ConnectionState.Open)
                    cn.Open();
                sqlCommand = "SELECT agent AS AGENT, transactionID AS VOUCHER_NUMBER, itemName AS ITEM, unitPrice AS UNINT_PRICE, quantity AS QUANTITY, grossPrice AS PAID_PRICE, transactionDate AS TRANSACTION_DATE FROM sales WHERE transactionID LIKE '%" + txtVoucherNumber.Text.ToString().Trim() + "%' ORDER BY transactionID DESC";
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sqlCommand, cn);
                da.Fill(dt);
                dataGridView1.DataSource = dt;

                try
                {
                    lblAgent.Text = dataGridView1.Rows[0].Cells[0].Value.ToString();
                    lblDate.Text = dataGridView1.Rows[0].Cells[6].Value.ToString();

                    ComputeVoucherSumary(Int32.Parse(dataGridView1.Rows[0].Cells[1].Value.ToString()));
                }
                catch (Exception ex)
                {
                    return;
                }
                
                
            }
            
        }

        private void txtDate_EditValueChanged(object sender, EventArgs e)
        {
            date = txtDate.Text.ToString();
            lblDate.Text = date;

            try
            {
                sqlCommand = "SELECT agent AS AGENT, transactionID AS VOUCHER_NUMBER, itemName AS ITEM, unitPrice AS UNINT_PRICE, quantity AS QUANTITY, grossPrice AS PAID_PRICE, transactionDate AS TRANSACTION_DATE FROM sales WHERE agent = '" + agent + "' AND transactionDate = '" + date + "' ORDER BY transactionID DESC";
                LoadSalesRecord(sqlCommand);
                ComputeSalesSummary();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        void ComputeSalesSummary()
        {
            try
            {
                MySqlConnection cn = new MySqlConnection(conObj.connString);
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                sqlCommand = "SELECT SUM(grossPrice) FROM sales WHERE transactionDate = '" + date + "' AND agent = '" + agent + "'";
                cmd = new MySqlCommand(sqlCommand, cn);
                lblIncome.Text = cmd.ExecuteScalar().ToString() + " F CFA";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            
        }

        void ComputeGrossPersonalSummary()
        {
            try
            {
                lblDate.Text = "All";
                MySqlConnection cn = new MySqlConnection(conObj.connString);
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                sqlCommand = "SELECT SUM(grossPrice) FROM sales WHERE agent = '" + agent + "'";
                cmd = new MySqlCommand(sqlCommand, cn);
                lblIncome.Text = cmd.ExecuteScalar().ToString() + " F CFA";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            
        }

        void ComputePersonalSumaryByDate()
        {
            try
            {
                MySqlConnection cn = new MySqlConnection(conObj.connString);
                if (cn.State != ConnectionState.Open)
                    cn.Open();

                sqlCommand = "SELECT SUM(grossPrice) FROM sales WHERE transactionDate = '" + date + "' AND agent = '" + agent + "'";
                cmd = new MySqlCommand(sqlCommand, cn);
                lblIncome.Text = cmd.ExecuteScalar().ToString() + " F CFA";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            
        }

        void ComputeVoucherSumary(int voucherID)
        {
            MySqlConnection cn = new MySqlConnection(conObj.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();

            sqlCommand = "SELECT SUM(grossPrice) FROM sales WHERE transactionID = '" + voucherID + "'";
            cmd = new MySqlCommand(sqlCommand, cn);
            lblIncome.Text = cmd.ExecuteScalar().ToString() + " F CFA";
        }


        private void Sales_Log_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            ADMIN_MENU AdminMenu = new ADMIN_MENU();
            AdminMenu.Show();
        }


    }
}
