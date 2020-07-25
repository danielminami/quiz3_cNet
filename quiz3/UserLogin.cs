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

namespace quiz3 {
    public partial class UserLogin : Form {
        public UserLogin() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {

            SqlConnection conn = new SqlConnection();
            //conn.ConnectionString = @"Data Source=localhost\SQLEXPRESS; initial catalog=STORE_PRODUCTS; integrated security=true";
            conn.ConnectionString = @"Data Source=LAPTOP-C9LG4BD2; initial catalog=STORE_PRODUCTS; integrated security=true";
            conn.Open();

            SqlCommand SQLcmd = new SqlCommand("spLogin", conn);

            SQLcmd.CommandType = CommandType.StoredProcedure;

            SQLcmd.Parameters.AddWithValue("@userid", txtUserName.Text);

            SqlDataReader rd = SQLcmd.ExecuteReader();

            if (rd.FieldCount > 0) {
                string pass = "";
                while (rd.Read()) {
                    pass = Convert.ToString(rd["userpassword"]).ToUpper();
                }
                if (pass == txtPassword.Text.ToUpper()) {
                    PurchaseForm purchaseForm = new PurchaseForm();
                    purchaseForm.Show();
                    this.Hide();
                } else {
                    MessageBox.Show("Invalid Password. Please try again.");
                }
            } else {
                MessageBox.Show("Invalid User. Please try again.");
            }
        }

    }
}
