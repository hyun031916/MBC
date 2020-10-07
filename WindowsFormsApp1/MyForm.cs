using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MyForm : Form
    {
        private TextBox textBox1;
        UsbAlarm _usbAlarm;

        public MyForm()
        {
            InitializeComponent();
            _usbAlarm = new UsbAlarm(this.Handle);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == UsbAlarm.WM_DEVICECHANGE)
            {
                Debug.WriteLine("MyForm: " + DateTime.Now + ": " + m.Msg + ", " + m.WParam.ToInt64() + ", " + m.LParam.ToInt64());
            }

            base.WndProc(ref m);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_usbAlarm != null)
            {
                _usbAlarm.Dispose();
            }

            base.OnClosing(e);
        }

        internal object label1_Click()
        {
            throw new NotImplementedException();
        }

        void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(66, 66);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(135, 25);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // MyForm
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.textBox1);
            this.Name = "MyForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}