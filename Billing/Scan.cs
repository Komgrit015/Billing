using MessagingToolkit.QRCode.Codec.Data;
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
    public partial class Scan : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=BPK_IT_TRAIN03\SQLEXPRESS;Initial Catalog =Billing; Integrated Security=True; ");
        DataSet ds;
        SqlDataAdapter dat;
        SqlCommand cmd;
        DataTable dt = new DataTable();
        int sumscan = 0 ;
        int sumcheck = 0;

        public Scan()
        {
            InitializeComponent();
        }

        private void Btnback_Click(object sender, EventArgs e)
        {
            Form1 to = new Form1();
            to.Show();
            this.Close();
        }

        private void Scan_Load(object sender, EventArgs e)
        {
            sumcheck = 0;
            sumscan = 0;
            this.ShowData();
            this.ShowSum(sumscan);
            this.Showcheck(sumcheck);

        }

        private void TxtReceipt_No_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                try
                {
                    string result = txtReceipt_No.Text.Substring(56);
                    cmd = new SqlCommand("UPDATE [dbo].[Imed] SET [AR] ='" + txtAR.Text + "' " +
                          ",[Pending_delivery] = 'true'" +
                          ",[Date_Scan] = GETDATE()" +
                          " WHERE[Receipt_No] = '" + result + "' ", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@AR", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Pending_delivery", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Date_Scan", DBNull.Value);

                    cmd.ExecuteNonQuery();

                    con.Close();
                    

                }
                catch (Exception ex)
                {
                    con.Close();
                }

                this.ShowData();
                ///Clear Textbox
                txtReceipt_No.Text = "";

                this.ShowSum(sumscan);
                sumscan = 0;
                this.Showcheck(sumcheck);
                sumcheck = 0;
            }
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex != dataGridView1.Columns["Pending_delivery"].Index) 
                return;
            if (e.Value.ToString().ToLower() == "true")
            {
                e.CellStyle.BackColor = Color.Green;
                
            
            }
            //if (e.ColumnIndex != dataGridView1.Columns["Check"].Index)
            //    return;
            //if (e.Value.ToString().ToLower() == "pass")
            //{
            //    e.CellStyle.BackColor = Color.Green;
            //}
            //else
            //{
            //    e.CellStyle.BackColor = Color.Red;
            //}
        }

        public void ShowSum(int scan=0) {

            label3.Text = "Scanned   " + dataGridView1.Rows.Count.ToString() + "/" + sumscan.ToString();

        }

        public void Showcheck(int check=0) {

            label4.Text = "Checking   " + dataGridView1.Rows.Count.ToString() + "/" + sumcheck.ToString();
                }

        public void ShowData() {
            
            try
            {
                sumcheck = 0;
                sumscan = 0;
                this.ShowSum(sumscan);
                this.Showcheck(sumcheck);
                con.Open();
                dat = new SqlDataAdapter("select [Receive_Date]" +
                    " ,[HN]" +
                    " ,[Receipt_No]" +
                    " ,[Name]" +
                    " ,[Net]" +
                    " ,[payer_name]" +
                    " ,[regis_name]" +
                    " ,[welfare_name]" +
                    " ,[AR]" +
                    " ,[Pending_delivery]" +
                    " ,CASE  WHEN [SAP_HN]<>'' THEN 'Pass' ELSE 'Pending' END AS 'Check' " +
                    " ,[Date_Scan]" +

                    " FROM chk_bill" +
                   " WHERE  [Receive_Date] = '"+dateTimeScan.Text+"'" +
                    " ORDER BY [Date_Scan] desc", con);
                dt = new DataTable();
                dat.Fill(dt);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                con.Close();

            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show(ex.Message);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.ShowData();
        }

        private void TxtReceipt_No_TextChanged(object sender, EventArgs e)
        {

        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string result = dataGridView1.Rows[e.RowIndex].Cells["Receipt_No"].Value.ToString();
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().ToLower() == "false")
                {
                    cmd = new SqlCommand("UPDATE [dbo].[Imed] SET [AR] =NULL " +
                      ",[Pending_delivery] = NULL" +
                      ",[Date_Scan] =NULL" +
                      " WHERE[Receipt_No] = '" + result + "' ", con);
                }
                else
                {
                    cmd = new SqlCommand("UPDATE [dbo].[Imed] SET [AR] ='" + txtAR.Text + "' " +
                     ",[Pending_delivery] = 1 " +
                     ",[Date_Scan] = GETDATE() " +
                     " WHERE[Receipt_No] = '" + result + "' ", con);
                }
                
                con.Open();
                
                cmd.ExecuteNonQuery();
                con.Close();
                //this.ShowData();
            }
            catch (Exception ex)
            {
                con.Close();
            }
        }

        private void DataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Pending_delivery"].Value.ToString().ToLower() == "true")
                {
                    sumscan++;
                }

            }
            this.ShowSum(sumscan);

            foreach(DataGridViewRow ch in dataGridView1.Rows)
            {
                if (ch.Cells["Check"].Value.ToString().ToLower() == "pass")
                {
                    sumcheck++;
                }

            }
            this.Showcheck(sumcheck);

        }

        private void DataGridView1_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
