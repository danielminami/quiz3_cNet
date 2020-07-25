using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace quiz3
{
    public partial class PurchaseForm : Form
    {
        private const string STORE_PRODUCT_DB = @"Data Source=LAPTOP-C9LG4BD2; initial catalog=STORE_PRODUCTS; integrated security=true";
        public PurchaseForm()
        {
            InitializeComponent();
        }

        private void PurchaseForm_Load(object sender, EventArgs e)
        {

            //SqlConnection conn = new SqlConnection();
            //conn.ConnectionString = @"Data Source=LAPTOP-C9LG4BD2; initial catalog=STORE_PRODUCTS; integrated security=true";
            //conn.Open();
            //string sqlStatement = "spGetAllProducts";
            //SqlDataAdapter cmd = new SqlDataAdapter(sqlStatement, conn);
            //DataTable dt = new DataTable();
            //cmd.Fill(dt);
            //dataGridView1.DataSource = dt;

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
                string sqlStatement = "GetProduct";

                //Call method to search and pass parameter
                SqlConnection cn = new SqlConnection(STORE_PRODUCT_DB);

                SqlDataAdapter cmd = new SqlDataAdapter(sqlStatement, cn);
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.Add("@prod_name", SqlDbType.VarChar).Value = txtSearchByName.Text;
                cmd.SelectCommand.Parameters.Add("@prod_sale", SqlDbType.Bit).Value = checkAvailable.Checked;
                DataTable dt = new DataTable();

                try
                {
                    cn.Open();
                    cmd.Fill(dt);
                    cn.Close();
                }
                catch (SqlException)
                {
                    MessageBox.Show("There was a problem.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    throw;
                }

                GridSearchResults.DataSource = dt;
        }

        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            int quantity;
            if (GridSearchResults.SelectedRows.Count > 0 && (int.TryParse(txtQuantity.Text, out quantity)) && quantity > 0)
            {
                DataTable dtCart = new DataTable();
                dtCart.Columns.Add("Quantity");
                dtCart.Columns.Add("Product Name");
                dtCart.Columns.Add("Price");

                foreach (DataGridViewRow row in GridSearchResults.SelectedRows)
                {
                    DataRow dr = dtCart.NewRow();
                    dr["Quantity"] = txtQuantity.Text;
                    dr["Product Name"] = row.Cells[1].Value;
                    dr["Price"] = row.Cells[4].Value;
                    dtCart.Rows.Add(dr);
                }

                if (GridCart.Rows.Count > 0)
                {
                    DataTable dtCurrentCart = (DataTable)GridCart.DataSource;
                    dtCurrentCart.Merge(dtCart);
                    GridCart.DataSource =  dtCurrentCart;
                }else
                {
                    GridCart.DataSource = dtCart;
                }
            }
            else
            {
                MessageBox.Show("You must select at least one item from the list \n and/or a valid quantity.");

            }
        }

        private void BtnPurchase_Click(object sender, EventArgs e)
        {
            if (GridCart.Rows.Count > 0)
            {
                double totalPurchase = 0;
                foreach (DataGridViewRow row in GridCart.Rows)
                {
                    totalPurchase += (Convert.ToDouble(row.Cells["Price"].Value)) * (Convert.ToInt32(row.Cells["Quantity"].Value));
                }

                MessageBox.Show($"The TOTAL amount for this purchase is $ {totalPurchase}.");
            }
            else
            {
                MessageBox.Show("You must select at least one item from the list.");
            }
        }
    }
}
