using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == UsbAlarm.WM_DEVICECHANGE)
            {
                Debug.WriteLine("Form1: " + DateTime.Now + ": " + m.Msg + ", " + m.WParam.ToInt64() + ", " + m.LParam.ToInt64());
            }

            base.WndProc(ref m);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyForm form = new MyForm();
            form.ShowDialog();
            form.textBox1.Text = "안녕";
        }
    }
}