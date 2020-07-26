using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace quiz3
{
    public partial class UserLogin : Form {
        private const string STORE_PRODUCT_DB = @"Data Source=localhost\SQLEXPRESS; initial catalog=STORE_PRODUCTS; integrated security=true";
        private const string SP_LOGIN = "spLogin";
        public UserLogin() {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e) {

            SqlConnection conn = new SqlConnection(STORE_PRODUCT_DB);
            SqlCommand SQLcmd = new SqlCommand(SP_LOGIN, conn);

            SQLcmd.CommandType = CommandType.StoredProcedure;
            SQLcmd.Parameters.AddWithValue("@userid", txtUserName.Text);

            try
            {
                conn.Open();

                SqlDataReader rd = SQLcmd.ExecuteReader();

                if (rd.FieldCount > 0)
                {
                    string pass = string.Empty;
                    while (rd.Read())
                    {
                        pass = Convert.ToString(rd["userpassword"]).ToUpper();
                    }

                    if (pass == txtPassword.Text.ToUpper())
                    {
                        PurchaseForm purchaseForm = new PurchaseForm();
                        purchaseForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Password. Please try again.");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid User. Please try again.");
                }

                conn.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("There was a problem trying to validate your User.\n Please try again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

    }
}
