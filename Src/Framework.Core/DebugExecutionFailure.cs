using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VM.Platform.TestAutomationFramework.Core
{
    public partial class DebugExecutionFailure : Form
    {
        public string resumeMode;

        public DebugExecutionFailure(string seqNumber, string errorMessage)
        {
            InitializeComponent();
            resumeMode = null;
            this.textBox1.Text = "Testcase Failed at Sequence No - " + seqNumber +
                "\r\n\r\nError Stack : \r\n\r\n" + errorMessage;
            this.ShowDialog();
        }

        private void DebugExecutionFailure_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            resumeMode = "resume";
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resumeMode = "abort";
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
