using VM.WebServices.TestAutomationFramework;
using VM.WebServices.TestAutomationFramework.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;

namespace UIDesign
{
    public partial class APITest : Form
    {
        string selectedXMLTemplateFile = null;
        string testExecutionStatus = null;
        public APITest()
        {
            InitializeComponent();

            // Upload Tab Form
            textBoxXMLTemplate.Enabled = false;
            textBoxTestSuiteXL.Enabled = false;
            btnXMLTemplateBrowse.Enabled = false;
            btnTestSuiteXLBrowse.Enabled = false;

            // Download Tab Form
            comboBoxXMLTemplate.Enabled = false;
            comboBoxTestSuiteXL.Enabled = false;
            textBoxSelectXMLTmptPath.Enabled = false;
            textBoxTestSuiteFilePath.Enabled = false;
            btnXMLTmpltBrowse.Enabled = false;
            btnTestSuitXLBrowse.Enabled = false;
            btnXMLTemplateDownload.Enabled = false;
            btnTestSuiteDownload.Enabled = false;

            //Execute Test Cases Tab Form
            btnExecuteTestCases.Enabled = false;
        }

        private void cBXMLTemplate_CheckedChanged(object sender, EventArgs e)
        {
            if (cBXMLTemplate.Checked)
            {
                textBoxXMLTemplate.Text = "";
                textBoxXMLTemplate.Enabled = true;
                btnXMLTemplateBrowse.Enabled = true;
                btnXMLTemplateBrowse.Enabled = true;
                btnTestSuiteXLBrowse.Enabled = true;
            }
            else
            {
                textBoxXMLTemplate.Enabled = false;
                btnXMLTemplateBrowse.Enabled = false;
                btnXMLTemplateBrowse.Enabled = false;
                btnTestSuiteXLBrowse.Enabled = false;
            }
        }

        private void cBTestSuite_CheckedChanged(object sender, EventArgs e)
        {
            if (cBTestSuite.Checked)
            {
                textBoxTestSuiteXL.Text = "";
                textBoxTestSuiteXL.Enabled = true;
                btnTestSuiteXLBrowse.Enabled = true;
            }
            else
            {
                textBoxTestSuiteXL.Enabled = false;
                btnTestSuiteXLBrowse.Enabled = false;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (cBXMLTemplate.Checked && textBoxXMLTemplate.Text == "")
                MessageBox.Show("Choose the XML Template and Click on Upload Button");
            else if (cBTestSuite.Checked && textBoxTestSuiteXL.Text == "")
                MessageBox.Show("Choose Test Suite File and Click on Upload Button");
            else
            {
                //if (textBoxXMLTemplate.Text != "") File.Copy(textBoxXMLTemplate.Text, Library.GetSolutionPath() + @"\VM.WebServices.TestAutomationFramework\TestCases\NonTimeTravelTestCases\XMLTemplates\Billing.Tmplt.Rq.xml", true);
                //if (btnTestSuiteXLBrowse.Text != "") File.Copy(btnTestSuiteXLBrowse.Text, Library.GetSolutionPath() + @"\VM.WebServices.TestAutomationFramework\TestCases\NonTimeTravelTestCases\PolicyBilling-TestCases.xlsx", true);

                if (textBoxXMLTemplate.Text != "")
                {
                    string fileName = Path.GetFileName(textBoxXMLTemplate.Text);

                    //File.Copy(textBoxXMLTemplate.Text, Library.GetSolutionPath() + @"\TestCases\NonTimeTravelTestCases\XMLTemplates\" + fileName, true);
                    //File.Copy(textBoxXMLTemplate.Text, Library.GetSolutionPath() + @"\UIDesign\UIDesign\TestCases\NonTimeTravelTestCases\XMLTemplates\" + fileName, true);
                    //MessageBox.Show(Directory.GetCurrentDirectory() + @"\TestCases\NonTimeTravelTestCases\XMLTemplates\" + fileName);
                    File.Copy(textBoxXMLTemplate.Text, Directory.GetCurrentDirectory() + @"\TestCases\NonTimeTravelTestCases\XMLTemplates\" + fileName, true);
                    MessageBox.Show("The File " + fileName + "Uploaded successfully");
                }
                if (textBoxTestSuiteXL.Text != "")
                {
                    string fileName = Path.GetFileName(textBoxTestSuiteXL.Text);
                    File.Copy(textBoxTestSuiteXL.Text, Directory.GetCurrentDirectory() + @"\TestCases\NonTimeTravelTestCases\" + fileName, true);
                    MessageBox.Show("The File " + fileName + "Uploaded successfully");
                }
            }
        }

        private void btnXMLTemplateBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            textBoxXMLTemplate.Text = fileDialog.FileName;
        }

        private void btnTestSuiteXLBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.ShowDialog();
            // btnTestSuiteXLBrowse.Text = fileDialog.FileName;
            textBoxTestSuiteXL.Text = fileDialog.FileName;
        }

        private void cBDXMLTemplate_CheckedChanged(object sender, EventArgs e)
        {
            // string sourcePath = Library.GetSolutionPath() + @"\VM.WebServices.TestAutomationFramework\TestCases\NonTimeTravelTestCases\XMLTemplates";
            string sourcePath = Directory.GetCurrentDirectory() + @"\TestCases\NonTimeTravelTestCases\XMLTemplates";
            if (cBDXMLTemplate.Checked)
            {

                comboBoxXMLTemplate.Enabled = true;
                textBoxSelectXMLTmptPath.Enabled = true;
                btnXMLTmpltBrowse.Enabled = true;
                btnXMLTemplateDownload.Enabled = true;
            }
            string[] files = Directory.GetFiles(sourcePath);
            foreach (var file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                comboBoxXMLTemplate.Items.Add(fileInfo.Name);
            }
        }

        private void checkBoxTestSuiteXL_CheckedChanged(object sender, EventArgs e)
        {
            //string sourcePath = Library.GetSolutionPath() + @"\VM.WebServices.TestAutomationFramework\TestCases\NonTimeTravelTestCases";
            string sourcePath = Directory.GetCurrentDirectory() + @"\TestCases\NonTimeTravelTestCases";
            if (checkBoxTestSuiteXL.Checked)
            {

                comboBoxTestSuiteXL.Enabled = true;
                textBoxTestSuiteFilePath.Enabled = true;
                btnTestSuitXLBrowse.Enabled = true;
                btnTestSuiteDownload.Enabled = true;
            }
            string[] files = Directory.GetFiles(sourcePath, "*.xlsx");
            foreach (var file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                comboBoxTestSuiteXL.Items.Add(fileInfo.Name);
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (comboBoxXMLTemplate.SelectedItem.ToString() != "" && textBoxSelectXMLTmptPath.Text != "")
            {
                string filePath = Path.Combine(textBoxSelectXMLTmptPath.Text.ToString(), comboBoxXMLTemplate.SelectedItem.ToString());
                string fPaht = Library.GetPath();
                //string xmlTmpltFilePath = Library.GetSolutionPath() + @"\VM.WebServices.TestAutomationFramework\TestCases\NonTimeTravelTestCases\XMLTemplates\";
                string xmlTmpltFilePath = Directory.GetCurrentDirectory() + @"\TestCases\NonTimeTravelTestCases\XMLTemplates\";
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                    File.Copy(xmlTmpltFilePath + comboBoxXMLTemplate.SelectedItem.ToString(), textBoxSelectXMLTmptPath.Text.ToString() + "\\" + comboBoxXMLTemplate.SelectedItem.ToString(), true);
                    MessageBox.Show("The File " + comboBoxXMLTemplate.SelectedItem.ToString() + "got downloaded successfully");
                }
                else
                {
                    File.Copy(xmlTmpltFilePath + comboBoxXMLTemplate.SelectedItem.ToString(), textBoxSelectXMLTmptPath.Text.ToString() + "\\" + comboBoxXMLTemplate.SelectedItem.ToString(), true);
                    MessageBox.Show("The File " + comboBoxXMLTemplate.SelectedItem.ToString() + "got downloaded successfully");
                }
            }
        }

        private void btnTestSuiteDownload_Click(object sender, EventArgs e)
        {
            if (comboBoxTestSuiteXL.SelectedItem.ToString() != "" && textBoxTestSuiteFilePath.Text != "")
            {
                string filePath = Path.Combine(textBoxTestSuiteFilePath.Text.ToString(), comboBoxTestSuiteXL.SelectedItem.ToString());
                //string testSuiteFilePath = Library.GetSolutionPath() + @"\VM.WebServices.TestAutomationFramework\TestCases\NonTimeTravelTestCases\";
                string testSuiteFilePath = Directory.GetCurrentDirectory() + @"\TestCases\NonTimeTravelTestCases\";
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Dispose();
                    File.Copy(testSuiteFilePath + comboBoxTestSuiteXL.SelectedItem.ToString(), textBoxTestSuiteFilePath.Text.ToString() + "\\" + comboBoxTestSuiteXL.SelectedItem.ToString(), true);
                    MessageBox.Show("The File " + comboBoxTestSuiteXL.SelectedItem.ToString() + "got downloaded successfully");
                }
                else
                {
                    File.Copy(testSuiteFilePath + comboBoxTestSuiteXL.SelectedItem.ToString(), textBoxTestSuiteFilePath.Text.ToString() + "\\" + comboBoxTestSuiteXL.SelectedItem.ToString(), true);
                    MessageBox.Show("The File " + comboBoxTestSuiteXL.SelectedItem.ToString() + "got downloaded successfully");
                }
            }
        }

        private void comboBoxTestSuiteXL_SelectedIndexChanged(object sender, EventArgs e)
        {
            string temp = comboBoxTestSuiteXL.SelectedItem.ToString();
        }

        private void comboBoxXMLTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedXMLTemplateFile = comboBoxXMLTemplate.SelectedItem.ToString();
        }

        private void btnXMLTmpltBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBowserdlg = new FolderBrowserDialog();
            DialogResult result = folderBowserdlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxSelectXMLTmptPath.Text = folderBowserdlg.SelectedPath;
            }
        }

        private void btnTestSuitXLBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBowserdlg = new FolderBrowserDialog();
            DialogResult result = folderBowserdlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBoxTestSuiteFilePath.Text = folderBowserdlg.SelectedPath;
            }
        }

        private void btnExecuteTestCases_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBoxEnvironment.Text) && listBoxSelectTCs.Items.Count > 0)
            {
                foreach (var item in listBoxSelectTCs.Items)
                {
                    WebServicesFrameworkDriver.FrameworkDriver(item.ToString(), comboBoxEnvironment.SelectedItem.ToString());
                }
                testExecutionStatus = "Completed";
                MessageBox.Show("Execution is completed. Check the results in the Test Results tab.");
            }
            else
            {
                if (comboBoxEnvironment.SelectedItem.ToString() == null) MessageBox.Show("Please select Environment.");
                if (listBoxSelectTCs.Items.Count == 0) MessageBox.Show("Please select Test Cases to Execute.");
            }
        }


        private void APITestsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((sender as TabControl).SelectedIndex)
            {
                case 2:
                    string testCasesExcelFile = Directory.GetCurrentDirectory() + Constants.TESTCASEEXCELFILE;
                    DataSet All = Library.ReadExcel(testCasesExcelFile);
                    DataTable dtTestSuite = All.Tables[Constants.TESTSUITE];
                    DataTable filteredTable = new DataView(dtTestSuite).ToTable(true, new string[] { "Test Case" });
                    foreach (DataRow dtRow in filteredTable.Rows)
                    {
                        if (dtRow.ItemArray[0].ToString() != "") listBoxAvailableTCs.Items.Add(dtRow.ItemArray[0]);
                    }
                    break;

                case 3:
                    if (testExecutionStatus == "Completed")
                    {
                        chartTestCases.Series.Clear();

                        lblTotalCaseNbr.Text = "2";
                        lblPassCasesNbr.Text = "2";
                        lblFailCaseNbr.Text = "0";

                        //Post Results to Graph
                        Series totTestCasesExecuted = new Series("# TestCases");
                        Series totTestCasesPassed = new Series("# Passed");
                        Series totTestCasesFailed = new Series("# Failed");
                        totTestCasesExecuted.Points.Add(listBoxSelectTCs.Items.Count);
                        totTestCasesPassed.Points.Add(2);
                        totTestCasesFailed.Points.Add(0);
                        chartTestCases.Titles.Add("Test Execution Status - Test Cases");
                        chartTestCases.Series.Add(totTestCasesExecuted);
                        chartTestCases.Series.Add(totTestCasesPassed);
                        chartTestCases.Series.Add(totTestCasesFailed);

                        //Post Results to Grid
                        dataGridViewTRs.ColumnCount = 2;
                        dataGridViewTRs.Columns[0].Name = "Test Case ID";
                        dataGridViewTRs.Columns[1].Name = "HTML Result File";

                        //DirectoryInfo htmlFileDir = new DirectoryInfo(Library.GetPath() + @"\Reports");  
                        DirectoryInfo htmlFileDir = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Reports");
                        foreach (var item in listBoxSelectTCs.Items)
                        {
                            List<string> htmlTRFiles = htmlFileDir.GetFiles("*.html")
                                          .Where(file => file.Name.EndsWith(".html") &&
                                                         file.Name.Contains(item.ToString()))
                                          .Select(file => file.Name).ToList();

                            dataGridViewTRs.Rows.Add(item.ToString(), htmlTRFiles[0].ToString());
                        }
                        string[] htmlResultFiles = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Reports");
                        lblDtlReportsLocation.Text = Directory.GetCurrentDirectory() + @"\Reports";
                        //   string[] htmlResultFiles = Directory.GetFiles(Library.GetPath() + @"\Reports");
                        // lblDtlReportsLocation.Text = Library.GetPath() + @"\Reports";
                    }
                    break;
            }
        }

        private void btnMoveAllCases_Click(object sender, EventArgs e)
        {
            foreach (var item in listBoxAvailableTCs.Items)
            {
                listBoxSelectTCs.Items.Add(item.ToString());
            }
            listBoxAvailableTCs.Items.Clear();
            if (!string.IsNullOrEmpty(comboBoxEnvironment.Text) && listBoxSelectTCs.Items.Count >= 0) btnExecuteTestCases.Enabled = true;
            else MessageBox.Show("Please select Environment");
        }

        private void comboBoxEnvironment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBoxEnvironment.Text) && listBoxSelectTCs.Items.Count >= 0) btnExecuteTestCases.Enabled = true;
        }

        private void dataGridViewTRs_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
