using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Billing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       

        private void SAP_Click(object sender, EventArgs e)
        {
            SAP to = new SAP();
            to.Show();
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Imed to = new Imed();
            to.Show();
            
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Check to = new Check();
            to.Show();
            
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Scan to = new Scan();
            to.Show();
            
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
