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
    public partial class PurchaseForm : Form {
        public PurchaseForm() {
            InitializeComponent();
        }

        private void PurchaseForm_Load(object sender, EventArgs e) {

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = @"Data Source=localhost\SQLEXPRESS; initial catalog=STORE_PRODUCTS; integrated security=true";
            conn.Open();
            string sqlStatement = "spGetAllProducts";
            SqlDataAdapter cmd = new SqlDataAdapter(sqlStatement, conn);
            DataTable dt = new DataTable();
            cmd.Fill(dt);
            dataGridView1.DataSource = dt;

        }
    }
}
