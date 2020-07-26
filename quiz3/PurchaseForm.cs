using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace quiz3
{
    public partial class PurchaseForm : Form
    {
        private const string STORE_PRODUCT_DB = @"Data Source=localhost\SQLEXPRESS; initial catalog=STORE_PRODUCTS; integrated security=true";
        private const string GET_PRODUCT = "GetProduct";
        private Decimal subtotalPurchase = 0;
        public PurchaseForm()
        {
            InitializeComponent();
            ShowSubtotalCart();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            //Call method to search and pass parameter
            SqlConnection cn = new SqlConnection(STORE_PRODUCT_DB);
            SqlDataAdapter cmd = new SqlDataAdapter(GET_PRODUCT, cn);

            cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
            cmd.SelectCommand.Parameters.Add("@productName", SqlDbType.VarChar).Value = txtSearchName.Text;
            cmd.SelectCommand.Parameters.Add("@isOnSale", SqlDbType.Bit).Value = checkAvailable.Checked;
            cmd.SelectCommand.Parameters.Add("@productCategory", SqlDbType.VarChar).Value = txtSearchCategory.Text;
            cmd.SelectCommand.Parameters.Add("@productDescription", SqlDbType.VarChar).Value = txtSearchDescription.Text;

            DataTable dt = new DataTable();

            try
            {
                cn.Open();
                cmd.Fill(dt);
                cn.Close();
                
            }
            catch (SqlException)
            {
                MessageBox.Show("There was a problem trying to get the results.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            GridSearchResults.DataSource = dt;
        }

        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            if (GridSearchResults.SelectedRows.Count > 0 && numQuantity.Value > 0)
            {
                DataTable dtCart = new DataTable();
                dtCart.Columns.Add("Quantity");
                dtCart.Columns.Add("Product Name");
                dtCart.Columns.Add("Price");

                foreach (DataGridViewRow row in GridSearchResults.SelectedRows)
                {
                    DataRow dr = dtCart.NewRow();
                    dr["Quantity"] = numQuantity.Value;
                    dr["Product Name"] = row.Cells[1].Value;
                    dr["Price"] = row.Cells[4].Value;
                    subtotalPurchase += Convert.ToDecimal(row.Cells[4].Value);
                    dtCart.Rows.Add(dr);
                }

                if (GridCart.Rows.Count > 0)
                {
                    DataTable dtCurrentCart = (DataTable)GridCart.DataSource;
                    dtCurrentCart.Merge(dtCart);
                    GridCart.DataSource = dtCurrentCart;
                }
                else
                {
                    GridCart.DataSource = dtCart;
                }

                ShowSubtotalCart();
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
                Decimal totalPurchase = 0;
                foreach (DataGridViewRow row in GridCart.Rows)
                {
                    totalPurchase += (Convert.ToDecimal(row.Cells["Price"].Value)) * (Convert.ToInt32(row.Cells["Quantity"].Value));
                }
                
                MessageBox.Show(String.Format("The TOTAL amount for this purchase is {0:C}", totalPurchase));
            }
            else
            {
                MessageBox.Show("You must select at least one item from the list.");
            }
        }

        private void BtnClearCart_Click(object sender, EventArgs e)
        {
            GridCart.DataSource = null;
            subtotalPurchase = 0;
            lblSubtotalAmount.Text = string.Empty;
        }

        private void ShowSubtotalCart()
        {
            lblSubtotalAmount.Text = String.Format(" {0:C}", subtotalPurchase);
        }
    }
}
