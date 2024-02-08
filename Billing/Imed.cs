using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Configuration;

namespace Billing
{
    public partial class Imed : Form
    {

        OdbcConnection con = new OdbcConnection("DSN=imed_bpk");


        OdbcDataAdapter dat;
        DataSet ds;
        OdbcCommand cmd;
        int id = 0;

        public Imed()
        {
            InitializeComponent();
        }

        private void Imed_Load(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
           
            try
            {
                
                ds = new DataSet();
                dat = new OdbcDataAdapter("SELECT receipt.receipt_id " +
                    ", format_hn(visit.hn)::VARCHAR(255) AS hn" +
                    ", (CASE WHEN receipt.fix_visit_type_id = '1' THEN format_an(visit.an) ELSE  format_vn(visit.vn) END)::VARCHAR(255) AS vn_an, format_receipt(receipt.receipt_number, receipt.fix_receipt_type_id, receipt.fix_receipt_status_id, receipt.credit_number)::VARCHAR(20) AS receipt_num" +
                    ", imed_get_patient_name(visit.patient_id)::VARCHAR(255) AS pt_name" +
                    ", receipt.receive_date::DATE AS receive_date" +
                    ", receipt.receive_time AS receive_time" +
                    ", receipt.cost::FLOAT AS total" +
                    ", (receipt.discount::FLOAT + receipt.decimal_discount::FLOAT) AS discount" +
                    ", (receipt.special_discount::FLOAT) AS special_discount" +
                    ", receipt.paid::FLOAT AS net" +
                    ", imed_get_employee_name(receipt.receive_eid)::VARCHAR(255) AS emp_name" +
                    ", receipt.fix_receipt_type_id" +
                    ", receipt.fix_receipt_status_id" +
                    ", receipt.fix_visit_type_id" +
                    ", plan.base_plan_group_id" +
                    ", plan.description::VARCHAR(255) AS plan_name" +
                    ", receipt.template_discount_name" +
                    ", plan.payer_id::VARCHAR(255)" +
                    ", payer.description::VARCHAR(255) AS payer_name" +
                    ", base_plan_group.base_plan_group_id" +
                    ", base_plan_group.description::VARCHAR(255) AS plan_group" +
                    ",case when visit.patient_number <> '' then imed_get_staff_name(visit.patient_number)else '' end::varchar(255) as Welfare_Name " +
                    "FROM receipt" +
                    " INNER JOIN visit ON visit.visit_id = receipt.visit_id" +
                    " INNER JOIN plan ON plan.plan_id = receipt.plan_id" +
                    " LEFT JOIN payer ON payer.payer_id = plan.payer_id" +
                    " LEFT JOIN base_plan_group ON plan.base_plan_group_id = base_plan_group.base_plan_group_id" +
                    " WHERE(receipt.receipt_number <> '' AND receipt.receipt_number IS NOT NULL)" +
                    "AND receipt.fix_receipt_type_id IN('7')" +
                    "AND receipt.fix_receipt_status_id = '2'" +
                    "AND receipt.receive_date::DATE BETWEEN '" + txt_date.Text + "' AND '" + txt_date2.Text + "'" +
                    "AND imed_get_employee_name(receipt.receive_eid) IN('" + txt_dname_emp.Text + "')" +
                    ///"AND(CASE WHEN '{?PayerName}' = '0' THEN '1' ELSE plan.payer_id = '{?PayerName}' END)"+
                    " ORDER BY payer_name" +
                        ", receipt_num" +
                        ", receipt.receive_date" +
                        ", receipt.receive_time", con);
                con.Open();
                
                con.Close();
                dat.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

            }
            catch (OdbcException ex)
            {
                con.Close();
                MessageBox.Show(ex.Message);
            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ///save 
            string mainconn = ConfigurationManager.ConnectionStrings["Myconnection"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(mainconn);
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string sqlquery = string.Format(@"IF NOT EXISTS (SELECT * FROM Imed WHERE Receipt_No = @receipt_num  ) 
                    BEGIN
            insert into [dbo].[Imed] ([Receive_Date]
           ,[Receipt_No]
           ,[HN]
           ,[Name]
           ,[Net]
           ,[payer_name]
           ,[regis_name]
           ,[welfare_name]) values (@receive_date,@receipt_num,@hn,@pt_name,@net,@payer_name,@emp_name,@welfare_name)

            END");
                
                SqlCommand sqlcom = new SqlCommand(sqlquery, sqlcon);

                try
                {
                    sqlcom.Parameters.AddWithValue("@receive_date", Convert.ToDateTime(dataGridView1.Rows[i].Cells[5].Value.ToString()));
                    sqlcom.Parameters.AddWithValue("@receipt_num", dataGridView1.Rows[i].Cells[3].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@hn", dataGridView1.Rows[i].Cells[1].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@pt_name", dataGridView1.Rows[i].Cells[4].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@net", dataGridView1.Rows[i].Cells[10].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@payer_name", dataGridView1.Rows[i].Cells[19].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@emp_name", dataGridView1.Rows[i].Cells[11].Value ?? DBNull.Value);
                    sqlcom.Parameters.AddWithValue("@welfare_name", dataGridView1.Rows[i].Cells[22].Value ?? DBNull.Value);

                    sqlcon.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlcon.Close();
                    label3.Text = "Inserted SuccessFully !!!";
                }
                catch (Exception ex)
                { 
                    sqlcon.Close();
                }
                }
            


            foreach (DataGridViewRow dr in dataGridView1.Rows)
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
