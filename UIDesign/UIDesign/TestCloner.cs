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
    public partial class TestCloner : Form
    {
        private Label lbl_cloneby = new Label();
        private Label lbl_cloneto = new Label();
        private Label lbl_category = new Label();
        private Label lbl_pagetitle = new Label();

        private TextBox txt_cloneto = new TextBox();
        private ComboBox cmb_cloneby = new ComboBox();
        private ComboBox cmb_category = new ComboBox();

        private Button btn_clone = new Button();

        private PictureBox pic_gecko = new PictureBox();
        private DataGridView grid_cloner = new DataGridView();
        private BindingSource bindingSrc = new BindingSource();
        private FlowLayoutPanel flp_testcloner = new FlowLayoutPanel();
        private TableLayoutPanel tlp_testcloner = new TableLayoutPanel();

        FuncLib objLib = new FuncLib();
        int tcid = 0;

        public TestCloner()
        {
            InitializeComponent();

            #region testcloner_design

            //Form Settings
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            this.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 4 - 50 , System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 50);
            this.AutoScroll = true;
            this.Text = "Test Cloner";

            //Flow Layout Panel Settings            
            flp_testcloner.FlowDirection = FlowDirection.LeftToRight;            
            flp_testcloner.SetFlowBreak(pic_gecko, true);
            flp_testcloner.Dock = DockStyle.Top;
            flp_testcloner.AutoSize = true;

            //Table Layout Panel Settings                                    
            tlp_testcloner.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_testcloner.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_testcloner.AutoSize = true;
            tlp_testcloner.Location = new Point(30, 80);

            //Gecko Picture Box Settings            
            pic_gecko.Height = 170;
            pic_gecko.Width = 220;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            //Data Gridview Settings
            grid_cloner.ReadOnly = false;
            grid_cloner.Dock = DockStyle.None;
            grid_cloner.AutoGenerateColumns = true;
            grid_cloner.Size = new Size(250, 250);
            grid_cloner.DataSource = bindingSrc;
            grid_cloner.DefaultCellStyle.Font = new Font("Calibri", 10);
            grid_cloner.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
            grid_cloner.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8.9F, FontStyle.Bold);
            grid_cloner.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            grid_cloner.Visible = false;
            grid_cloner.AllowUserToAddRows = false;

            // customize dataviewgrid, add checkbox column
            DataGridViewCheckBoxColumn chk_selection = new DataGridViewCheckBoxColumn();
            chk_selection.Width = 30;
            chk_selection.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid_cloner.Columns.Insert(0, chk_selection);

            // add checkbox header
            Rectangle rect = grid_cloner.GetCellDisplayRectangle(0, -1, true);
            // set checkbox header to center of header cell. +1 pixel to position correctly.
            rect.X = rect.Location.X + (rect.Width / 4);

            CheckBox checkboxHeader = new CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            checkboxHeader.Size = new Size(13, 13);
            //checkboxHeader.Location = rect.Location;
            checkboxHeader.Location = new Point(60, 5);
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);

            grid_cloner.Controls.Add(checkboxHeader);

            //CloneTo Label settings            
            lbl_cloneto.Text = "Clone To Release :*";
            lbl_cloneto.Name = "lbl_cloneto";
            lbl_cloneto.TextAlign = ContentAlignment.BottomLeft;
            lbl_cloneto.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_cloneto.Height = 20;
            lbl_cloneto.Width = 200;

            //CloneBy Label settings            
            lbl_cloneby.Text = "Clone By :*";
            lbl_cloneby.Name = "lbl_cloneby";
            lbl_cloneby.TextAlign = ContentAlignment.BottomLeft;
            lbl_cloneby.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_cloneby.Height = 20;
            lbl_cloneby.Width = 200;

            //CloneBy Label settings            
            lbl_category.Text = "Category :";
            lbl_category.Name = "lbl_category";
            lbl_category.TextAlign = ContentAlignment.BottomLeft;
            lbl_category.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_category.Height = 20;
            lbl_category.Width = 200;
            lbl_category.Visible = false;

            //Clone To textbox settings            
            txt_cloneto.Text = "";
            txt_cloneto.Name = "txt_cloneto";
            txt_cloneto.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_cloneto.Height = 30;
            txt_cloneto.Width = 250;
            txt_cloneto.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_cloneto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
            coll.AddRange(objLib.GetReleases().OrderBy(x => x.Value).Select(x => x.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
            txt_cloneto.AutoCompleteCustomSource = coll;

            //CloneBy combobox settings            
            cmb_cloneby.Name = "cmb_cloneby";
            cmb_cloneby.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_cloneby.Height = 30;
            cmb_cloneby.Width = 250;
            //cmb_cloneby.AutoCompleteSource = AutoCompleteSource.ListItems;
            //cmb_cloneby.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_cloneby.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_cloneby.Items.AddRange(new[] {"Functionality","Release", "Test Case ID", "Test Category" });

            //CloneBy combobox settings            
            cmb_category.Name = "cmb_category";
            cmb_category.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_category.Height = 30;
            cmb_category.Width = 250;
            cmb_category.Visible = false;
            //cmb_cloneby.AutoCompleteSource = AutoCompleteSource.ListItems;
            //cmb_cloneby.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_cloneby.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
//            cmb_category.Items.AddRange(new[] { "Functional", "Regression", "Smoke Test" });

            //Clone button settings            
            btn_clone.Text = "CLONE";
            btn_clone.Name = "btn_clone";
            btn_clone.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_clone.Height = 30;
            btn_clone.Width = 100;
            btn_clone.Visible = false;

            //Userid Label settings           
            lbl_pagetitle.Height = 30;
            lbl_pagetitle.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 4 - 150;
            lbl_pagetitle.Text = "TEST CLONER";
            lbl_pagetitle.Name = "lbl_pagetitle";
            lbl_pagetitle.TextAlign = ContentAlignment.MiddleCenter;
            lbl_pagetitle.Font = new Font("Calibri", 20F, FontStyle.Bold);
            lbl_pagetitle.ForeColor = Color.Blue;

            tlp_testcloner.Controls.Add(lbl_pagetitle, 0, 1);
            tlp_testcloner.SetColumnSpan(lbl_pagetitle, 10);

            tlp_testcloner.Controls.Add(lbl_cloneto, 1, 2);
            tlp_testcloner.Controls.Add(txt_cloneto, 1, 3);
            tlp_testcloner.Controls.Add(lbl_cloneby, 1, 4);
            tlp_testcloner.Controls.Add(cmb_cloneby, 1, 5);
            tlp_testcloner.Controls.Add(lbl_category, 1, 6);
            tlp_testcloner.Controls.Add(cmb_category, 1, 7);
            tlp_testcloner.Controls.Add(grid_cloner, 1, 8);
            tlp_testcloner.Controls.Add(btn_clone, 1, 9);

            //Adding Controls to Flow Layout Panel
            flp_testcloner.Controls.AddRange(new Control[] { pic_gecko, tlp_testcloner });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_testcloner });
            this.Load += new System.EventHandler(TestCloner_Load);

            #endregion

            #region testcloner_methods

            cmb_cloneby.SelectedIndexChanged += new System.EventHandler(cmb_cloneby_selectionChanged);
            cmb_category.SelectedIndexChanged += new System.EventHandler(cmb_category_selectionChanged);
            btn_clone.Click += new System.EventHandler(btn_clone_Click);

            #endregion
        }
        private void TestCloner_Load(object sender, EventArgs e)
        {

        }
        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < grid_cloner.RowCount; i++)
            {
                grid_cloner[0, i].Value = ((CheckBox)grid_cloner.Controls.Find("checkboxHeader", true)[0]).Checked;
            }
            grid_cloner.EndEdit();
        }
        private void cmb_cloneby_selectionChanged(object sender, EventArgs e)
        {
            //string strQuery = string.Empty;
            //if (cmb_cloneby.SelectedIndex == 0)
            //{
            //    lbl_category.Visible = false;
            //    cmb_category.Visible = false;
            //    strQuery = "SELECT DISTINCT TestCaseID FROM TestCaseInfo WHERE PROJECTID=" + SignIn.projectId;
            //    DataTable dt = new DataTable();
            //    dt = objLib.binddataTable(strQuery);
            //    //dt.Columns.Add("New TestCaseID");
            //    bindingSrc.DataSource = dt;
            //    grid_cloner.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //    grid_cloner.Columns[0].Width = 50;
            //    grid_cloner.Visible = true;
            //    btn_clone.Visible = true;
            //}

            //if (cmb_cloneby.SelectedIndex == 1)
            //{
            //    grid_cloner.Visible = false;
            //    lbl_category.Visible = true;
            //    cmb_category.Visible = true;
            //    btn_clone.Visible = false;
            //}

            if(cmb_cloneby.SelectedItem.ToString()=="Functionality")
            {
                grid_cloner.Visible = false;
                lbl_category.Visible = true;
                lbl_category.Text = "Functionality";
                cmb_category.Visible = true;
                cmb_category.Items.Clear();
                cmb_category.ResetText();
                cmb_category.Items.AddRange(objLib.GetFunctionalities().OrderBy(x => x.Value).Select(x => x.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
                btn_clone.Visible = false;
            }
            else if(cmb_cloneby.SelectedItem.ToString()=="Release")
            {
                grid_cloner.Visible = false;
                lbl_category.Visible = true;
                lbl_category.Text = "Release";
                cmb_category.Visible = true;
                cmb_category.Items.Clear();
                cmb_category.ResetText();
                cmb_category.Items.AddRange(objLib.GetReleases().OrderBy(x => x.Value).Select(x => x.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
                btn_clone.Visible = false;
            }
            else if (cmb_cloneby.SelectedItem.ToString() == "Test Case ID")
            {
                string strQuery = string.Empty;
                lbl_category.Visible = false;
                cmb_category.Visible = false;
                strQuery = "SELECT DISTINCT TestCaseID FROM TestCaseInfo WHERE PROJECTID=" + SignIn.projectId +" AND ISDELETED IS NULL";
                DataTable dt = new DataTable();
                dt = objLib.binddataTable(strQuery);
                //dt.Columns.Add("New TestCaseID");
                bindingSrc.DataSource = dt;
                grid_cloner.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grid_cloner.Columns[0].Width = 50;
                grid_cloner.Visible = true;
                btn_clone.Visible = true;

            }
            else if (cmb_cloneby.SelectedItem.ToString() == "Test Category")
            {
                grid_cloner.Visible = false;
                lbl_category.Visible = true;
                lbl_category.Text = "Category";
                cmb_category.Visible = true;
                cmb_category.Items.Clear();
                cmb_category.ResetText();
                cmb_category.Items.AddRange(new[] { "Functional", "Regression", "Smoke Test" });
                btn_clone.Visible = false;
            }

        }
        private void cmb_category_selectionChanged(object sender, EventArgs e)
        {
            string strQuery = string.Empty;
            var val = string.IsNullOrEmpty(cmb_category.SelectedItem.ToString()) ? string.Empty : cmb_category.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(cmb_category.SelectedItem.ToString()))
            {
                if (cmb_cloneby.SelectedItem.ToString() == "Functionality")
                {
                    strQuery = "SELECT DISTINCT TestCaseID FROM TestCaseInfo where Functionality='" + cmb_category.SelectedItem.ToString() + "' AND PROJECTID=" + SignIn.projectId + " AND ISDELETED IS NULL";
                }
                else if (cmb_cloneby.SelectedItem.ToString() == "Release")
                {
                    strQuery = "SELECT DISTINCT TestCaseID FROM TestCaseInfo where Release='" + cmb_category.SelectedItem.ToString() + "' AND PROJECTID=" + SignIn.projectId + " AND ISDELETED IS NULL";
                }
                else if (cmb_cloneby.SelectedItem.ToString() == "Test Category")
                {
                    strQuery = "SELECT DISTINCT TestCaseID FROM TestCaseInfo where TestCategory='" + cmb_category.SelectedItem.ToString() + "' AND PROJECTID=" + SignIn.projectId + " AND ISDELETED IS NULL";
                }
            }
            else
            {
                if (cmb_cloneby.SelectedItem.ToString() == "Functionality")
                {
                    strQuery = "SELECT DISTINCT TestCaseID FROM TestCaseInfo where (Functionality IS NULL or Functionality='') AND PROJECTID=" + SignIn.projectId + " AND ISDELETED IS NULL";
                }
                else if (cmb_cloneby.SelectedItem.ToString() == "Release")
                {
                    strQuery = "SELECT DISTINCT TestCaseID FROM TestCaseInfo where (Release IS NULL or Release='') AND PROJECTID=" + SignIn.projectId + " AND ISDELETED IS NULL";
                }
                else if (cmb_cloneby.SelectedItem.ToString() == "Test Category")
                {
                    strQuery = "SELECT DISTINCT TestCaseID FROM TestCaseInfo where (TestCategory IS NULL or TestCategory='') AND PROJECTID=" + SignIn.projectId + " AND ISDELETED IS NULL";
                }
            }            
            
            DataTable dt = new DataTable();
            dt = objLib.binddataTable(strQuery);
            //dt.Columns.Add("New TestCaseID");
            bindingSrc.DataSource = dt;
            grid_cloner.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid_cloner.Columns[0].Width = 50;
            grid_cloner.Visible = true;
            btn_clone.Visible = true;
        }
        private void btn_clone_Click(object sender, EventArgs e)
        {            
            if (!string.IsNullOrEmpty(txt_cloneto.Text))
            {
                btn_clone.Enabled = false;
                try
                {
                    for (int i = 0; i < grid_cloner.Rows.Count; i++)
                    {
                        if ((!objLib.IsNullOrEmpty(Convert.ToString(grid_cloner.Rows[i].Cells[0].Value))))
                        {
                            DataTable dt_tcinfo = new DataTable();
                            dt_tcinfo = objLib.binddataTable("SELECT * FROM TESTCASEINFO WHERE TESTCASEID='" + grid_cloner.Rows[i].Cells[1].Value.ToString() + "' AND PROJECTID=" + SignIn.projectId);
                            for (int j = 0; j < dt_tcinfo.Rows.Count; j++)
                            {
                                string strQuery = "INSERT INTO TestCaseInfo(TestCaseTitle,TestCaseSummary,DesignedBy, AssignedTo,Priority, State,TestCategory,Release,Tags,Functionality,ProjectID,Jira,CreatedBy,CreatedDate,TCReferenceId)OUTPUT INSERTED.TESTCASEID Values('"
                                    //+ Int32.Parse(grid_cloner.Rows[i].Cells[2].Value.ToString().Trim()) + ",'"
                                    + dt_tcinfo.Rows[j]["TestCaseTitle"].ToString().Replace("'", "''") + "','"
                                    + dt_tcinfo.Rows[j]["TestCaseSummary"].ToString().Replace("'", "''") + "','"
                                    + dt_tcinfo.Rows[j]["DesignedBy"].ToString() + "','"
                                    + dt_tcinfo.Rows[j]["AssignedTo"].ToString() + "','"
                                    + dt_tcinfo.Rows[j]["Priority"].ToString() + "','"
                                    + dt_tcinfo.Rows[j]["State"].ToString() + "','"
                                    + dt_tcinfo.Rows[j]["TestCategory"].ToString() + "','"
                                    + txt_cloneto.Text.ToString() + "','"
                                    + dt_tcinfo.Rows[j]["Tags"].ToString() + "','"
                                    + dt_tcinfo.Rows[j]["Functionality"].ToString() + "',"
                                    + SignIn.projectId + ",'"
                                    + dt_tcinfo.Rows[j]["Jira"].ToString() + "','"
                                    + SignIn.userId + "','"
                                    + DateTime.Now + "','"
                                    + dt_tcinfo.Rows[j]["TCReferenceId"] +"')";
                                
                                tcid = Int32.Parse(objLib.ExecuteScalar(strQuery).ToString());
                            }

                            DataTable dt_tcflow = new DataTable();
                            dt_tcflow = objLib.binddataTable("SELECT ActionFlow_id,AF.PageID,ActionFlow,FlowIdentifier,SeqNumber FROM ActionFlow AF inner join PageNames PG on AF.PageID = PG.PageID WHERE TESTCASEID='" + grid_cloner.Rows[i].Cells[1].Value.ToString() + "' AND AF.PROJECTID=" + SignIn.projectId);
                            for (int j = 0; j < dt_tcflow.Rows.Count; j++)
                            {
                                string strQuery = "INSERT INTO ACTIONFLOW(TestCaseId,PageID,ActionFlow,FlowIdentifier,SeqNumber,ProjectID,CreatedBy,CreatedDate) VALUES("
                                    + tcid + ","
                                    + dt_tcflow.Rows[j]["PageID"] + ",'"
                                    + dt_tcflow.Rows[j]["ActionFlow"].ToString().Trim() + "',"
                                    + (objLib.IsNullOrEmpty(Convert.ToString(dt_tcflow.Rows[j]["FlowIdentifier"])) ? "NULL" : dt_tcflow.Rows[j]["FlowIdentifier"].ToString()) + ","
                                    + (objLib.IsNullOrEmpty(Convert.ToString(dt_tcflow.Rows[j]["SeqNumber"])) ? "NULL" : dt_tcflow.Rows[j]["SeqNumber"].ToString()) + ","
                                    + SignIn.projectId + ",'"
                                    + SignIn.userId + "','"
                                    + DateTime.Now + "')";
                                objLib.RunQuery(strQuery);
                            }
                            DataTable dt_tcdata = new DataTable();
                            dt_tcdata = objLib.binddataTable("SELECT * FROM TESTDATAVIEW WHERE TESTCASEID='" + grid_cloner.Rows[i].Cells[1].Value.ToString() + "' AND PROJECTID=" + SignIn.projectId);
                            for (int j = 0; j < dt_tcdata.Rows.Count; j++)
                            {
                                int pageId = int.Parse(dt_tcdata.Rows[j]["PageID"].ToString());
                                string strQuery = "INSERT INTO TESTDATA([Execute],TestCaseId,PageID,FlowIdentifier,DataIdentifier,Indicator,MasterORID,ActionORData,SeqNumber,ProjectID,CreatedBy,CreatedDate) VALUES('"
                                    //+ grid_testdata.Rows[i].Cells["cmb_execute"].Value.ToString() + "',"
                                    + (objLib.IsNullOrEmpty(Convert.ToString(dt_tcdata.Rows[j]["Execute"])) ? "Yes" : dt_tcdata.Rows[j]["Execute"].ToString()) + "',"
                                    + tcid + ","
                                    + pageId + ","
                                    + dt_tcdata.Rows[j]["FlowIdentifier"].ToString() + ","
                                    + dt_tcdata.Rows[j]["DataIdentifier"].ToString() + ",'"
                                    + Int32.Parse(objLib.GetKeywords().FirstOrDefault(x => x.Value == dt_tcdata.Rows[j]["Keyword"].ToString()).Key) + "',"
                                    + objLib.GetORLables(pageId).FirstOrDefault(x => x.Value == dt_tcdata.Rows[j]["Label"].ToString()).Key + ",'"
                                    + dt_tcdata.Rows[j]["ActionOrData"].ToString().Replace("'", "''") + "',"
                                    + dt_tcdata.Rows[j]["SeqNumber"].ToString() + ","
                                    + SignIn.projectId + ",'"
                                    + SignIn.userId + "','"
                                    + DateTime.Now + "')";
                                objLib.RunQuery(strQuery);
                            }
                        }
                    }
                    MessageBox.Show("Test Case Cloned Successfully", "Clone Test Cases", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR : " + ex.Message, "Clone Test Cases", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please provide Clone To Release :","Clone Test Cases", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
