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
using ExcelDataReader;
using System.IO;
using System.Configuration;

namespace Billing
{
    public partial class SAP : Form
    {
        public SAP()
        {
            InitializeComponent();
        }
        System.Globalization.CultureInfo cultureinfo =
            new System.Globalization.CultureInfo("en-US");

        private void SAP_Load(object sender, EventArgs e)
        {

        }
        DataSet result;
        private void BtnOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Excel file | *.xls;* . *all file;*", ValidateNames = true })
            {
                if (ofd.ShowDialog() == DialogResult.OK)

                {
                    try
                    {
                        FileStream fs = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read);

                        IExcelDataReader reader = ExcelReaderFactory.CreateReader(fs);

                        result = reader.AsDataSet();
                        cboSheet.Items.Clear();
                        try
                        {
                            foreach (DataTable dt in result.Tables)
                                cboSheet.Items.Add(dt.TableName);
                            reader.Close();
                        }
                        catch (Exception)
                        {
                            reader.Close();
                            throw;
                        }

                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }

        private void CboSheet_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView.DataSource = result.Tables[cboSheet.SelectedIndex];
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(mainconn);
            for (int i = 1; i < dataGridView.Rows.Count; i++)
            {
                string sqlquery = string.Format(@" IF NOT EXISTS (SELECT * FROM SAP WHERE Ref_Document = @Ref_Document  ) 
                    BEGIN
            insert into [dbo].[SAP] ([Billing_Date]
           ,[Bill_To_Party]
           ,[Bill_To_Party_Description]
           ,[Ref_Document]
           ,[HN]
           ,[Patient_Name]
           ,[Net_revenue]
           ,[Cashier_name]
           ,[Collector_Name]
           ,[Cost_center]) values (@Billing_Date,@Bill_To_Party,@Bill_To_Party_Description,@Ref_Document,@HN,@Patient_Name,@Net_revenue,@Cashier_name,@Collector_Name,@Cost_center)

            END");
                SqlCommand sqlcom = new SqlCommand(sqlquery, sqlcon);

                try
                {

                    DateTime dDate;
                    if (DateTime.TryParse(dataGridView.Rows[i].Cells[0].Value.ToString(), out dDate))
                    {
                        sqlcom.Parameters.AddWithValue("@Billing_Date", Convert.ToDateTime(dataGridView.Rows[i].Cells[0].Value.ToString()).ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        sqlcom.Parameters.AddWithValue("@Billing_Date", DBNull.Value);
                    }

                    sqlcom.Parameters.AddWithValue("@Bill_To_Party", dataGridView.Rows[i].Cells[1].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@Bill_To_Party_Description", dataGridView.Rows[i].Cells[2].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@Ref_Document", dataGridView.Rows[i].Cells[3].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@HN", dataGridView.Rows[i].Cells[4].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@Patient_Name", dataGridView.Rows[i].Cells[5].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@Net_revenue", dataGridView.Rows[i].Cells[6].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@Cashier_name", dataGridView.Rows[i].Cells[7].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@Collector_Name", dataGridView.Rows[i].Cells[8].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@Cost_center", dataGridView.Rows[i].Cells[9].Value ?? DBNull.Value);

                    sqlcon.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlcon.Close();
                    label4.Text = "Inserted SuccessFully !!!";

                }
                catch (Exception ex)
                {
                    sqlcon.Close();
                }
            }
            foreach (DataGridViewRow dr in dataGridView.Rows)
            {


            }
        }

        private void Btn_back_Click(object sender, EventArgs e)
        {
            Form1 to = new Form1();
            to.Show();
            this.Close();

        }
    }
}
