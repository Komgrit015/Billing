using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace Billing
{
    public partial class Check : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=BPK_IT_TRAIN03\SQLEXPRESS;Initial Catalog =Billing; Integrated Security=True; ");
        DataSet ds;
        SqlDataAdapter dat;
        DataTable dt;

        public Check()
        {
            InitializeComponent();
        }

        private void Check_Load(object sender, EventArgs e)
        {

        }
      
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try {
               
                con.Open();
                ds = new DataSet();
                dat = new SqlDataAdapter("select [Receive_Date]" +
                    " ,[HN]" +
                    " ,[Receipt_No]" +
                    " ,[Name]" +
                    " ,[Net]" +
                    " ,[payer_name]" +
                    " ,[regis_name]" +
                    " ,[welfare_name]" +
                    " ,CASE  WHEN [SAP_HN]<>'' THEN 'Pass' ELSE 'Pending' END AS 'Check' " +
                    " FROM chk_bill" +
                    " WHERE [Receive_Date] BETWEEN '" + date_search1.Text + "' AND '"+ date_search2.Text + "'" , con);
                dt = new DataTable();
                dat.Fill(dt);
                dataGridView1.DataSource = dt;
                label1.Text = "Check SuccessFully !!!";
                con.Close();
            }
            catch (Exception ex) {
                con.Close();
            }
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Btn_back_Click(object sender, EventArgs e)
        {
            Form1 to = new Form1();
            to.Show();
            this.Close();

        }
    }
}
