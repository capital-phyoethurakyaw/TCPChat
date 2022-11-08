using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TCPServer
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var thread1 = new Thread(SetText1);
            var thread2 = new Thread(SetText2);
            thread1.Start();
            thread2.Start();
        }
        private void SetText1()
        {
            textBox1.Text = "Test";
        }

        private void SetText2()
        {
            textBox1.Invoke(new Action(() => textBox1.Text = "Test"));
        }
    }
}
