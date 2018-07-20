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
    public partial class STOCK : XtraForm
    {
        StockClass stock;
        int i;

        //Current Item Details
        string currentName;

        MyConnection conObject = new MyConnection();
        
        MySqlCommand cmd;
        string sqlCommand;

        string catName = "";

        public STOCK()
        {
            InitializeComponent();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName = "McSkin";
        }

        private void Stock_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'eyebinventoryDataSet1.categories' table. You can move, or remove it, as needed.
            this.categoriesTableAdapter.Fill(this.eyebinventoryDataSet1.categories);
            
            System.IO.StreamReader fileReader = new System.IO.StreamReader(Application.StartupPath + "/UTILITY/user_Log.txt");
            lblUerLog.Text = fileReader.ReadToEnd().ToString();

            DisableControls_Cancel();
            try
            {
                fill_Categories();
            }
            catch (MySqlException myEx)
            {
                XtraMessageBox.Show("Oop!!\n" + myEx.Message + "\nPlease contact your database administrator");
            }
            
        }


        void fill_Categories()
        {
            MySqlConnection cn = new MySqlConnection(conObject.connString);
            cn.Open();
            lbxCategory.Items.Clear();
            MySqlCommand cmd;
            cmd = new MySqlCommand("SELECT * FROM categories", cn);
            MySqlDataReader reader;
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lbxCategory.Items.Add(reader[0].ToString().ToUpper());
            }
            reader.Close();
            reader.Dispose();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            EnableControls_Input();
            ShowAsterics();
            ///TODO Code here
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            stock = new StockClass();
            //i = dataGridView1.SelectedCells[0].RowIndex;

            if (txtName.Text == "" || cbxCategory.Text == "" || txtUnitPrice.Text == "")
            {
                XtraMessageBox.Show("Please fill in MANDATORY FIELDS marked with Asterics", "iBallInventory", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                ShowAsterics();
                //txtName.Text.Trim().ToString()==""|| cbxCategory.Text.Trim().ToString()==""||txtManufac.Text.Trim().ToString()==""||txtSellingPrice.Text.Trim().ToString()==""||txtStockQty.Text.Trim().ToString()==""||txtDateBought.Text.Trim().ToString()==""||txtExpiryDate.Text.Trim().ToString()==""||txtUnitPrice.Text.Trim().ToString()==""
            }
            else
            {
                try
                {
                    stock.itemName = txtName.Text.Trim().ToString();
                    stock.category = cbxCategory.Text.Trim().ToString();
                    stock.manufacturer = txtManufac.Text.Trim().ToString();
                    stock.grossPrice = txtGrossPrice.Text.Trim().ToString();
                    stock.stockQuantity = txtStockQty.Text.Trim().ToString();
                    stock.dateBought = txtDateBought.Text.Trim().ToString();
                    stock.expiryDate = txtExpiryDate.Text.Trim().ToString();
                    //stock.quantitySold = dataGridView1.Rows[i].Cells[7].Value.ToString();
                    stock.unitPrice = txtUnitPrice.Text.Trim().ToString();
                    //stock.badProduct = dataGridView1.Rows[i].Cells[9].Value.ToString();

                    //Validate inputs provided
                    ValidateInputs(stock.itemName, stock.category, stock.manufacturer, stock.grossPrice, stock.stockQuantity, stock.dateBought, stock.expiryDate, stock.unitPrice);

                }
                catch (MySqlException ex)
                {
                    XtraMessageBox.Show("Oops!!\n" + ex.Message);
                }
            }
            
        }

        void ValidateInputs(string itemName, string category, string manufac, string grossPrice, string stockQuantity, string dateBought, string expiryDate, string unitPrice)
        {
            MySqlConnection cn = new MySqlConnection(conObject.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            MySqlCommand cmd;
            if (btnAdd.Text == "ADD")   
            {
                //Adding New Item to the Category

                cmd = new MySqlCommand("SELECT * FROM stock WHERE itemName = '" + txtName.Text.Trim().ToString() + "' AND category = '" + cbxCategory.Text.ToString() + "'", cn);
                MySqlDataReader reader;
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    XtraMessageBox.Show("The Item *'" + txtName.Text.Trim().ToString() + "'* already exists in the System", "iBInventory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    reader.Close();
                    reader.Dispose();
                }
                else
                {
                    reader.Close();
                    reader.Dispose();

                    sqlCommand = "INSERT INTO `eyebinventory`.`stock` (`itemName`, `category`, `Manufacturer`, `grossPrice`, `availableQuantity`, `dateBought`, `expiryDate`, `unitPrice`) VALUES ('" + itemName + "', '" + category + "', '" + manufac + "', '" + grossPrice + "', '" + stockQuantity + "', '" + dateBought + "', '" + expiryDate + "', '" + unitPrice + "')";
                    cmd = new MySqlCommand(sqlCommand, cn);
                    int my = cmd.ExecuteNonQuery();
                    XtraMessageBox.Show("Record added Successfully", "iBInventory", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    EnableControls_Input();
                    fill_by_Category(catName);
                    HideAsterics();
                }
            }
            else
            {
                //Updating the Current Item in the Category

                sqlCommand = "SELECT * FROM stock WHERE itemName='" + txtName.Text.Trim().ToString() + "' AND category='" + cbxCategory.Text.ToString() + "' AND manufacturer='" + txtManufac.Text.Trim().ToString() + "' AND grossPrice='" + txtGrossPrice.Text.Trim().ToString() + "' AND dateBought='" + txtDateBought.Text.Trim().ToString() + "' AND expiryDate='" + txtExpiryDate.Text.Trim().ToString() + "' AND unitPrice='" + txtUnitPrice.Text.Trim().ToString() + "' AND availableQuantity='" + stockQuantity + "'";
                cmd = new MySqlCommand(sqlCommand, cn);
                MySqlDataReader myReader;
                myReader = cmd.ExecuteReader();
                myReader.Read();
                    if (myReader.HasRows)
                    {
                        XtraMessageBox.Show("No changes made for ITEM ** " + currentName.ToUpper() + " **", "iBallInventory v1.0", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        myReader.Close();
                        myReader.Dispose();
                        sqlCommand = "UPDATE stock SET itemName='" + txtName.Text.Trim().ToString() + "', category='" + cbxCategory.Text.ToString() + "', manufacturer='" + txtManufac.Text.ToString() + "', grossPrice='" + txtGrossPrice.Text.ToString() + "', dateBought='" + txtDateBought.Text.ToString() + "', expiryDate='" + txtExpiryDate.Text.ToString() + "', unitPrice='" + txtUnitPrice.Text.ToString() + "', availableQuantity='" + txtStockQty.Text.ToString() + "' WHERE itemName='" + currentName + "' AND category = '" + catName + "'";
                        cmd = new MySqlCommand(sqlCommand, cn);
                        cmd.ExecuteNonQuery();
                        cmd.ExecuteNonQuery(); //XtraMessageBox.Show("After ExecuteNoneQuery\nItemName: " + itemName.ToString() + "\nRows affected: " + res.ToString());

                        XtraMessageBox.Show("\n Item * '" + currentName.ToUpper() + "' * Updated Successfully", "iBallInventory v1.0", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        DisableControls_Cancel();
                        dataGridView1.DataSource = null;
                        fill_by_Category(catName);
                        HideAsterics();
                    }
            }
            
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            dr = XtraMessageBox.Show("Are you sure??", "iBallInventory v1.0", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
                try
                {
                    DeleteItem();
                    DisableControls_Cancel();
                    fill_by_Category(catName);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("Oops!!\n" + ex.Message, "iBallInventory v1.0", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else
                return;
            
        }

        void DeleteItem()
        {
            sqlCommand = "DELETE FROM stock WHERE itemName = '" + currentName + "'";
            MySqlConnection cn = new MySqlConnection(conObject.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            cmd = new MySqlCommand(sqlCommand, cn);

            int result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                XtraMessageBox.Show("Record Deleted Successfully", "iBInventory v1.0", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //TODO Code
            DisableControls_Cancel();
        }

        public void DisableControls_Cancel()
        {
            cbxCategory.ResetText();
            cbxCategory.Text = "";
            txtGrossPrice.ResetText(); 
            txtDateBought.ResetText(); 
            txtExpiryDate.ResetText(); 
            txtManufac.ResetText(); 
            txtName.ResetText(); 
            txtStockQty.ResetText(); 
            txtUnitPrice.ResetText();

            cbxCategory.Enabled = false;
            txtGrossPrice.Enabled = false;
            txtDateBought.Enabled = false;
            txtExpiryDate.Enabled = false;
            txtManufac.Enabled = false;
            txtName.Enabled = false;
            txtStockQty.Enabled = false;
            txtUnitPrice.Enabled = false;
            

            btnAdd.Enabled = false;
            btnAdd.Text = "ADD";
            btnCancel.Enabled = false;
            btnNew.Enabled = true;
            btnRemove.Enabled = false;

            HideAsterics();
        }

        void EnableControls_Input()
        {
            cbxCategory.ResetText();
            cbxCategory.Text = lblCategory.Text.ToString();
            txtGrossPrice.ResetText();
            txtDateBought.ResetText();
            txtExpiryDate.ResetText();
            txtManufac.ResetText();
            txtName.ResetText();
            txtStockQty.ResetText();
            txtUnitPrice.ResetText();

            cbxCategory.Enabled = true;
            txtGrossPrice.Enabled = true;
            txtDateBought.Enabled = true;
            txtExpiryDate.Enabled = true;
            txtManufac.Enabled = true;
            txtName.Enabled = true;
            txtStockQty.Enabled = true;
            txtUnitPrice.Enabled = true;

            btnAdd.Enabled = true;
            btnAdd.Text = "ADD";
            btnCancel.Enabled = true;
            btnNew.Enabled = true;

            HideAsterics();

        }

        void HideAsterics()
        {
            //Asterics
            lblAsterics1.Visible = false;
            lblAsterics2.Visible = false;
            lblAsterics3.Visible = false;
            lblAsterics4.Visible = false;
            lblAsterics5.Visible = false;
        }

        void ShowAsterics()
        {
            //Asterics
            lblAsterics1.Visible = true;
            lblAsterics2.Visible = true;
            lblAsterics3.Visible = true;
            //lblAsterics4.Visible = true;
            lblAsterics5.Visible = true;
        }

        void EnableControl_Update()
        {
            cbxCategory.Enabled = true;
            txtGrossPrice.Enabled = true;
            txtDateBought.Enabled = true;
            txtExpiryDate.Enabled = true;
            txtManufac.Enabled = true;
            txtName.Enabled = true;
            txtStockQty.Enabled = true;
            txtUnitPrice.Enabled = true;

            btnAdd.Enabled = true;
            btnAdd.Text = "UPDATE";
            btnRemove.Enabled = true;
            btnCancel.Enabled = true;
            btnNew.Enabled = false;

            ShowAsterics();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void STOCK_FormClosed(object sender, FormClosedEventArgs e)
        {
            STOCK_MENU stockMenu = new STOCK_MENU();
            this.Hide();
            stockMenu.Show();
        }

        private void btnSearchCategory_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchCategory();
            }
            catch (Exception)
            {
                return;
            }
            
        }

        private void btnSearchCategory_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                //SearchCategory();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void btnSearchCategory_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
               //SearchCategory(); 
            }
            catch (Exception)
            {
                return;
            } 
        }

        void SearchCategory()
        {
            MySqlConnection cn = new MySqlConnection(conObject.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            string sqlCommand = "SELECT * FROM categories WHERE category LIKE '%" + txtSearchCategory.Text.ToString() + "%'";
            //string sql = string.Format("SELECT * FROM mydata where fullname like section like '%" + txtSearch.Text + "%'");
            cmd = new MySqlCommand(sqlCommand, cn);
            MySqlDataReader dr = cmd.ExecuteReader();

            lbxCategory.Items.Clear();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lbxCategory.Items.Add(dr[0].ToString().ToUpper());
                }
                dr.Close();
                dr.Dispose();
            }
            else
            {
                fill_Categories();
            }
        }

        private void btnSearchItem(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                SearchItem();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void txtSearchItem_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            try
            {
                SearchItem();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void txtSearchItem_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            try
            {
                SearchItem();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void txtSearchItem_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                SearchItem();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void txtSearchItem_Click(object sender, EventArgs e)
        {
            txtSearchItem.SelectAll();
        }

        void SearchItem()
        {
            MySqlConnection cn = new MySqlConnection(conObject.connString);
            if (cn.State != ConnectionState.Open)
                cn.Open();
            string sqlCommand = "SELECT * FROM stock WHERE itemName LIKE '%" + txtSearchItem.Text.ToString() + "%' AND category = '" + lblCategory.Text.ToString() + "'";
            
            cmd = new MySqlCommand(sqlCommand, cn);
                
            MySqlDataAdapter da;
            DataTable dt = new DataTable();

            da = new MySqlDataAdapter(sqlCommand, cn);
            da.Fill(dt);
            dataGridView1.DataSource = dt;            
        }

        private void lbxCategory_Click(object sender, EventArgs e)
        {
            catName = lbxCategory.SelectedItem.ToString();
            lblCategory.Text = catName;
            try
            {
                fill_by_Category(catName);
            }
            catch (MySqlException myEx)
            {
                XtraMessageBox.Show("Oop!!\n" + myEx.Message + "\nPlease contact your database administrator", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtSearchCategory_Click(object sender, EventArgs e)
        {
            txtSearchCategory.SelectAll();
        }

        private void lbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisableControls_Cancel();
            catName = lbxCategory.SelectedItem.ToString();
            //string categoryName = lbxCategory.SelectedItem.ToString();
            
            try
            {
                fill_by_Category(catName);
            }
            catch (MySqlException myEx)
            {
                XtraMessageBox.Show("Oop!!\n" + myEx.Message + "\nPlease contact your database administrator", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            dataGridView1.RowHeadersVisible = false;
            
            LoadGridProperty();
        }

        void LoadGridProperty()
        {
            dataGridView1.Columns[0].HeaderText = "Item Name";
            dataGridView1.Columns[1].HeaderText = "Category";
            dataGridView1.Columns[2].HeaderText = "Manufacturer";
            dataGridView1.Columns[3].HeaderText = "Gross Price";
            dataGridView1.Columns[4].HeaderText = "Stock Qty";
            dataGridView1.Columns[5].HeaderText = "Date Bought";
            dataGridView1.Columns[6].HeaderText = "Exp. Date";
            dataGridView1.Columns[7].HeaderText = "Qty Sold";
            dataGridView1.Columns[8].HeaderText = "Unit Price";
            dataGridView1.Columns[9].HeaderText = "Sale Location";
            dataGridView1.Columns[10].HeaderText = "Bad Product";

            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        void fill_by_Category(string catName)
        {
            MySqlDataAdapter da;
            DataTable dt = new DataTable();
            
            MySqlConnection cn = new MySqlConnection(conObject.connString);
            if (cn.State != ConnectionState.Closed)
                cn.Open();
            sqlCommand = "SELECT * FROM stock WHERE category = '" + catName + "'";
            cmd = new MySqlCommand(sqlCommand, cn);
            
            da = new MySqlDataAdapter(sqlCommand, cn);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            stock = new StockClass();
            i = dataGridView1.SelectedCells[0].RowIndex;

            //XtraMessageBox.Show(dataGridView1.Rows[i].Cells[0].Value.ToString());
            try
            {
                stock.itemName = dataGridView1.Rows[i].Cells[0].Value.ToString();
                currentName = dataGridView1.Rows[i].Cells[0].Value.ToString();
                stock.category = dataGridView1.Rows[i].Cells[1].Value.ToString();
                stock.manufacturer = dataGridView1.Rows[i].Cells[2].Value.ToString();
                stock.grossPrice = dataGridView1.Rows[i].Cells[3].Value.ToString();
                stock.stockQuantity = dataGridView1.Rows[i].Cells[4].Value.ToString();
                stock.dateBought = dataGridView1.Rows[i].Cells[5].Value.ToString();
                stock.expiryDate = dataGridView1.Rows[i].Cells[6].Value.ToString();
                stock.quantitySold = dataGridView1.Rows[i].Cells[7].Value.ToString();
                stock.unitPrice = dataGridView1.Rows[i].Cells[8].Value.ToString();
                stock.badProduct = dataGridView1.Rows[i].Cells[9].Value.ToString();

                //XtraMessageBox.Show(dataGridView1.Rows[i].Cells[8].Value.ToString());
                
            }
            catch (Exception ex)
            {
                //XtraMessageBox.Show(ex.Message);
                return ;
            }

            setControlsData(stock.itemName, stock.category, stock.manufacturer, stock.grossPrice, stock.stockQuantity, stock.dateBought, stock.expiryDate, stock.quantitySold, stock.unitPrice, stock.badProduct);
        }

        void setControlsData(string itemName, string catecory, string manufacturer, string grossPrice, string stockQty, string dateBought, string expiryDate, string quantitySold, string unitPrice, string badProduct)
        {
            EnableControl_Update();

            txtName.Text = itemName;
            cbxCategory.Text = catecory;
            txtManufac.Text = manufacturer;
            txtGrossPrice.Text = grossPrice.ToString();
            txtUnitPrice.Text = unitPrice.ToString();
            txtDateBought.Text = dateBought.ToString();
            txtExpiryDate.Text = expiryDate.ToString();
            txtStockQty.Text = stockQty.ToString();
        }


        private void STOCK_Activated(object sender, EventArgs e)
        {
            //txtSearchCategory.Text.ToUpper();
            //txtSearchItem.Text.ToUpper();
        }

        private void txtSearchCategory_Leave(object sender, EventArgs e)
        {
            if (txtSearchCategory.Text == "")
            {
                txtSearchCategory.Text = "Search Category";
                fill_Categories();
            }
        }

    }
}
