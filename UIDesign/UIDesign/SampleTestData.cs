using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIDesign
{
    public partial class SampleTestData : Form
    {
        public static string[] copiedrows;
        public SampleTestData()
        {
            InitializeComponent();
        }
        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < sampleTestDataGrid.RowCount; i++)
            {
                sampleTestDataGrid[0, i].Value = ((CheckBox)sampleTestDataGrid.Controls.Find("checkboxHeader", true)[0]).Checked;
            }
            sampleTestDataGrid.EndEdit();
        }
        private void SampleTestData_Load(object sender, EventArgs e)
        {
            sampleTestDataGrid.ReadOnly = false;
            sampleTestDataGrid.Dock = DockStyle.None;
            //grid_testdata.AutoGenerateColumns = true;
            sampleTestDataGrid.Size = new Size(1000, 300);
            sampleTestDataGrid.DefaultCellStyle.Font = new Font("Calibri", 10);
            sampleTestDataGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
            sampleTestDataGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8.9F, FontStyle.Bold);

            DataTable dt = LoadTestData(TestData.TemplateTCId, TestData.PageId);
            if (dt.Rows.Count > 0)
                sampleTestDataGrid.DataSource = dt;
            else
                MessageBox.Show("No Record found");

        }
        private DataTable LoadTestData(string TestCaseId, string pageId)
        {
            DataTable dt = null;
            FuncLib objLib = new FuncLib();
            return dt=objLib.GetSampleTestData(TestCaseId, pageId);
        }

        private void MenuItem_Copy_Click(Object sender, System.EventArgs e)
        {
            Clipboard.SetDataObject(sampleTestDataGrid.GetClipboardContent(), true);
        }
        private void sampleTestDataGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Copy", MenuItem_Copy_Click, Shortcut.CtrlC));
                int currentMouseOverRow = sampleTestDataGrid.HitTest(e.X, e.Y).RowIndex;
                m.Show(sampleTestDataGrid, new Point(e.X, e.Y));
            }
        }
        private void cmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
