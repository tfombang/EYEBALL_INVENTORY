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
    public partial class SALES : XtraForm
    {

        MyConnection connObj = new MyConnection();
        MySqlConnection cn;
        MySqlCommand cmd;
        string sqlCommand;
        int quantity;

        int receiptNumber=0000;
        
        public SALES()
        {
            InitializeComponent();
            DevExpress.Skins.SkinManager.EnableFormSkins();

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "McSkin";
            //DevExpress.LookAndFeel.UserLookAndFeel.Default.UseWindowsXPTheme = false;

            //DefaultLookAndFeel defaultSkin = new DefaultLookAndFeel();
            //defaultSkin.LookAndFeel.SetSkinStyle("Seven"); 
        }

        private void SALES_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'itemNameDataSet.stock' table. You can move, or remove it, as needed.
            this.stockTableAdapter.Fill(this.itemNameDataSet.stock);
            System.IO.StreamReader fileReader = new System.IO.StreamReader(Application.StartupPath + "/UTILITY/user_Log.txt");
            lblUerLog.Text = fileReader.ReadToEnd().ToString();
            
            btnAddToList.BackColor = Color.Transparent;
            cbxItem.Text = null;

            ReceiptLetterHead();

            generateReceiptNumber();
            lblVoucher.Text = receiptNumber.ToString();

            dataGridView1.ForeColor = Color.Black;

        }

        private void generateReceiptNumber()
        {
            cn = new MySqlConnection(connObj.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            sqlCommand = "SELECT * from sales WHERE transactionID='" + receiptNumber + "'";
            cmd = new MySqlCommand(sqlCommand, cn);
            
            try
            {
                Random rand = new Random();
                this.receiptNumber = rand.Next(999999999);
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Close();
                    reader.Dispose();
                    generateReceiptNumber();
                }
                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            
        }

        void ReceiptLetterHead()
        {
            lblAgent.Text = lblUerLog.Text.ToString();
            lblDate.Text = DateTime.Today.Date.ToShortDateString();
        }


        private void SALES_FormClosed(object sender, FormClosedEventArgs e)
        {
            Login loginForm = new Login();
            this.Hide();
            loginForm.Show();
        }

        private void cbxItem_TextChanged(object sender, EventArgs e)
        {
            txtQuantity.Reset();
            txtQuantity.Enabled = false;
            lblAvailableQty.ResetText();
            this.quantity = 0;
        }



        private void btnAddToList_Click(object sender, EventArgs e)
        {
            cn = new MySqlConnection(connObj.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            int qtty = Int32.Parse(txtQuantity.Text.ToString());
            if (qtty != 0 && qtty > 0)
            {
                if (qtty <= getAvailableQuantity())
                {
                    sqlCommand = "SELECT unitPrice FROM stock WHERE itemName = '" + cbxItem.Text.ToString() + "'";
                    cmd = new MySqlCommand(sqlCommand, cn);
                    Int32 price = Int32.Parse(cmd.ExecuteScalar().ToString());
                    int unitPrice = price;

                    price *= Int32.Parse(txtQuantity.Text.ToString());

                    int rows = dataGridView1.Rows.Count;
                    dataGridView1.Rows.Add(rows, cbxItem.Text.ToString(), unitPrice, txtQuantity.Text.ToString(), price);

                    //cLEAR INPUT CONTROLS
                    txtQuantity.ResetText();
                    txtQuantity.Enabled = false;
                    lblAvailableQty.ResetText();
                    cbxItem.ResetText();
                    btnAddToList.Enabled = false;
                }
                else
                    XtraMessageBox.Show("Insufficient Stock Quantity", "iBallInventory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                
                
            }
            else
            {
                XtraMessageBox.Show("Invalid Quamtity", "iBallInventory",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                try
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != 5)
            {
                try
                {
                    //XtraMessageBox.Show(dataGridView1.CurrentRow.Cells[1].Value.ToString());
                    //cbxItem.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                    //btnQuantity.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    ////Update the Row upon Click for Add
                }
                catch (Exception)
                {
                    return;
                }
            }
            
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //Clear the previous Table content
            tableLayoutPanel1.Controls.Clear();
            

/*******************************Add the "TOTAL" row *******************************************/
            int rows, total=0;
            rows = dataGridView1.RowCount;
            
            

            for (int i = 0; i < rows-1; i++)
            {
                if (dataGridView1.Rows[i].Cells[1].Value.ToString() == "Total")
                { 
                    dataGridView1.Rows.RemoveAt(i); break; //Remove the Column for total in order not to duplicate the Total column hence doubling the total price
                }
                else
                {
                    total += Int32.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                }
                
            }

            dataGridView1.Rows.Add(dataGridView1.RowCount + 1, "Total", "", "", total);
/*************************************************************************************************/


/***************************Peform the database updates***********************************************/

            MySqlConnection cn = new MySqlConnection(connObj.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();

            string item;
            int quantity=0;
            float unitPrice = 0, payPrice = 0;

                
            for (int i = 0; i < rows - 1; i++)
            {
                
                try
                {
                    item = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    unitPrice = Int32.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    quantity = Int32.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    payPrice = Int32.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());

                    //XtraMessageBox.Show("item: " + item + "\nquantity: " + quantity);
                    updateStock(item, quantity);
                    storeSalesInDatabase(item, unitPrice, quantity, payPrice);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "iBallInventory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //return;
                }
            }

/***********************************************************************************************************/
            LoadDatatoTable();
            XtraMessageBox.Show("Sales Recorded!!", "EyeBInventory", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ///nEW vOUCHER nUMBER gENERATION fOR nEXT tRANSACTION
            ///cLEAR DataGridView1 fOR nEW tRANSACTION
            ReceiptLetterHead();
            generateReceiptNumber();
            lblVoucher.Text = receiptNumber.ToString();

            dataGridView1.Rows.Clear();

            DialogResult dr= XtraMessageBox.Show("Print Receipt??", "EyeBInventory", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                printDocument1.Print();
                tableLayoutPanel1.Controls.Clear();
            }
            else
            {
                tableLayoutPanel1.Controls.Clear();
                return;
            }
/***********************************Print the Receipt*******************************************************/     

        }


        void LoadDatatoTable()
        {

            int rows;
            rows = dataGridView1.RowCount;
            //dataGridView1.Rows.Add();
            for (int i = 0; i < rows - 1; i++)
            {
                
                if (i == rows-3)
                {
                    Label lblItm = new Label();
                    Label lblUPrice = new Label();
                    Label lblQty = new Label();
                    Label lblAmnt = new Label();
                    lblItm.Text="~ ~ ~";
                    lblUPrice.Text = "_ _ _ _ _ _ _ _";
                    lblQty.Text = "_ _ _ _";
                    lblAmnt.Text = "_ _ _";

                    tableLayoutPanel1.Controls.Add(lblItm, i, i);
                    tableLayoutPanel1.Controls.Add(lblUPrice, i,i);
                    tableLayoutPanel1.Controls.Add(lblQty,i,i);
                    tableLayoutPanel1.Controls.Add(lblAmnt,i,i);
                    
                }

                Label lblItem = new Label(); lblItem.AutoSize = true;
                Label lblUnitPrice = new Label();
                Label lblQuantity = new Label();
                Label lblAmount = new Label();
                //Create a lable to store the contents
                
                lblItem.Text = dataGridView1.Rows[i].Cells[1].Value.ToString();
                tableLayoutPanel1.Controls.Add(lblItem, 0, i);

                
                lblUnitPrice.Text = dataGridView1.Rows[i].Cells[2].Value.ToString();
                tableLayoutPanel1.Controls.Add(lblUnitPrice, 1, i);
                
                lblQuantity.Text = dataGridView1.Rows[i].Cells[3].Value.ToString();
                tableLayoutPanel1.Controls.Add(lblQuantity, 2, i);

                lblAmount.Text = dataGridView1.Rows[i].Cells[4].Value.ToString();
                tableLayoutPanel1.Controls.Add(lblAmount, 3, i);
            }
        }

        void updateStock(string item, int quantity)
        {
            int availableQuantity = getAvailableQuantity(item);
            int newQuantity = availableQuantity-quantity;
            int stockQttySold = getDBaseQuantitySold(item) + quantity;

            //Update the Available Quantity
            sqlCommand = "";
            sqlCommand = "UPDATE stock SET availableQuantity='" + newQuantity + "', quantitySold='" + stockQttySold + "' WHERE itemName='" + item + "'";
            cmd = new MySqlCommand(sqlCommand, cn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            
        }

        int getDBaseQuantitySold(string item)
        {
            cn = new MySqlConnection(connObj.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            sqlCommand = "SELECT quantitySold FROM stock WHERE itemName = '" + item.ToString() + "'";
            cmd = new MySqlCommand(sqlCommand, cn);
            int qttySold = Int32.Parse(cmd.ExecuteScalar().ToString());

            return qttySold;
        }


        void storeSalesInDatabase(string item, float unitPrice, int quantity, float payPrice)
        {
            MySqlConnection cn = new MySqlConnection(connObj.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();

            sqlCommand = "INSERT INTO sales (transactionID, itemName, unitPrice, quantity, grossPrice, agent, transactionDate) VALUES ('" + receiptNumber + "','" + item + "','" + unitPrice + "','" + quantity + "','" + payPrice + "','" + lblAgent.Text.ToString() + "','" + DateTime.Today.ToShortDateString() + "')";
            cmd = new MySqlCommand(sqlCommand, cn);
            cmd.ExecuteNonQuery();
        }


        int getAvailableQuantity( string item)
        {
            MySqlConnection cn = new MySqlConnection(connObj.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();

            //Get the availableQuantity in the Database
            sqlCommand = "SELECT availableQuantity FROM stock WHERE itemName='" + item + "'";
            cmd = new MySqlCommand(sqlCommand, cn);
            return Int32.Parse(cmd.ExecuteScalar().ToString());
        }

        int getAvailableQuantity()
        {
            MySqlConnection cn = new MySqlConnection(connObj.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();

            //Get the availableQuantity in the Database
            sqlCommand = "SELECT availableQuantity FROM stock WHERE itemName='" + cbxItem.Text.ToString() + "'";
            cmd = new MySqlCommand(sqlCommand, cn);
            return Int32.Parse(cmd.ExecuteScalar().ToString());
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            STOCK stockform = new STOCK();
            stockform.ShowDialog();
        }


        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap gbxReceiptImage = new Bitmap(this.groupBox2.Width, this.groupBox2.Height);
            //groupBox2.DrawToBitmap(gbxReceiptImage, new Rectangle(20, 20, this.groupBox2.Width, this.groupBox2.Height));
            groupBox2.DrawToBitmap(gbxReceiptImage, new Rectangle(0, 0, this.groupBox2.Width, this.groupBox2.Height));
            e.Graphics.DrawImage(gbxReceiptImage, 0, 0);
        }

        private void cbxItem_MouseClick(object sender, MouseEventArgs e)
        {
            cbxItem.SelectAll();
        }


        private void cbxItem_MouseLeave(object sender, EventArgs e)
        {
            DisplayAvailableQuantity();                      
        }

        private void txtQuantity_EditValueChanged(object sender, EventArgs e)
        {
            DisplayAvailableQuantity(); 
            
            int qtty = Int32.Parse(txtQuantity.Text.ToString());
            if (qtty != 0 && qtty > 0)
            {
                btnAddToList.Enabled = true;
            }

        }

        private void cbxItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayAvailableQuantity();
        }

        void DisplayAvailableQuantity()
        {
            try
            {
                cn = new MySqlConnection(connObj.connString);
                if (cn.State != ConnectionState.Open)
                    cn.Open();
                sqlCommand = "SELECT itemName FROM stock WHERE itemName ='" + cbxItem.Text.ToString() + "'";
                cmd = new MySqlCommand(sqlCommand, cn);
                MySqlDataReader myreader;
                myreader = cmd.ExecuteReader();
                myreader.Read();
                if (myreader.HasRows)
                {
                    int availableQuantity = getAvailableQuantity();
                    lblAvailableQty.Text = "Max Available= " + availableQuantity.ToString();
                    txtQuantity.Enabled = true;
                    txtQuantity.Focus();
                }
                myreader.Close();
                myreader.Dispose();
                
                
            }
            catch (Exception)
            {
                return;
            }
        }
        
        private void txtQuantity_MouseHover(object sender, EventArgs e)
        {
            
        }   

    }
}
