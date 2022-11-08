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

namespace TestConnection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string connetionString = null;
                SqlConnection cnn;
                connetionString = textBox1.Text; //"Data Source="";Initial Catalog=DatabaseName;User ID=UserName;Password=Password"
                cnn = new SqlConnection(connetionString);
                try
                {
                    cnn.Open();
                    MessageBox.Show("Connection Open ! ");
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            
        }
    }
}
