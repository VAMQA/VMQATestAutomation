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
    public partial class TestCase : Form
    {
        private Label lbl_testcaseid = new Label();
        private Label lbl_testcasetitle = new Label();
        private Label lbl_testcategory = new Label();
        private Label lbl_testcasefun = new Label();
        private Label lbl_release = new Label();
        private Label lbl_testcasetags = new Label();
        private Label lbl_designedby = new Label();
        private Label lbl_assignedto = new Label();
        private Label lbl_designstatus = new Label();
        private Label lbl_testcasesummary = new Label();
        private Label lbl_TCReferenceId = new Label();
        
        private Label lbl_testpriority = new Label();
        private Label lbl_jira = new Label();

        public TextBox txt_testcaseid = new TextBox();
        public TextBox txt_testcasetitle = new TextBox();
        public ComboBox cmb_testcategory = new ComboBox();
        public TextBox txt_testcasefun = new TextBox();
        public TextBox txt_testcasetags = new TextBox();
        public TextBox txt_release = new TextBox();
        public ComboBox cmb_designedby = new ComboBox();
        public ComboBox cmb_assignedto = new ComboBox();
        public ComboBox cmb_designstatus = new ComboBox();
        public ComboBox cmb_priority = new ComboBox();
        public TextBox txt_testcasesummary = new TextBox();
        public TextBox txt_jira = new TextBox();
        public TextBox txt_TCReferenceId = new TextBox();

        public ComboBox cmb_testcasefun = new ComboBox();

        public Button btn_createtestcase = new Button();
        public Button btn_createtestdata = new Button();
        public Button btn_savetestcase = new Button();
        public LinkLabel lnk_edittestdata = new LinkLabel();
        public LinkLabel lnk_populatetags = new LinkLabel();

        public DataGridViewComboBoxColumn cmb_pagename = new DataGridViewComboBoxColumn();
        private List<string> deleteflag = new List<string>();
        private List<string> editflag = new List<string>();

        private PictureBox pic_gecko = new PictureBox();
        public DataGridView grid_testcase = new DataGridView();        
        private FlowLayoutPanel flp_testcase = new FlowLayoutPanel();
        private TableLayoutPanel tlp_stub = new TableLayoutPanel();
        private TableLayoutPanel tlp_testcase = new TableLayoutPanel();

        public Button btn_Resettestdataseq = new Button();

        FuncLib objLib = new FuncLib();
        static int testcaseId = 0;

        public TestCase()
        {
            InitializeComponent();

            #region testcase_design

            //Form Settings
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            //this.Size = new Size(1350, 750);
            this.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width -150, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 130);
            this.AutoScroll = true;
            this.Text = "Test Case";
            this.MaximizeBox = true;
            this.MinimizeBox = false;

            //Flow Layout Panel Settings            
            flp_testcase.FlowDirection = FlowDirection.LeftToRight;
            flp_testcase.SetFlowBreak(tlp_testcase, true);
            flp_testcase.Dock = DockStyle.Top;
            flp_testcase.AutoSize = true;

            //Table Layout Panel Settings                                    
            tlp_testcase.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_testcase.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_testcase.AutoSize = true;
            tlp_testcase.Location = new Point(30, 80);

            //Table Layout Panel Settings                                    
            tlp_stub.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_stub.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_stub.AutoSize = true;
            tlp_stub.Location = new Point(30, 80);

            //Gecko Picture Box Settings            
            pic_gecko.Height = 170;
            pic_gecko.Width = 220;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            //Data Gridview Settings
            grid_testcase.ReadOnly = false;
            grid_testcase.Dock = DockStyle.None;
            grid_testcase.AutoGenerateColumns = true;
            grid_testcase.Size = new Size(770, 280);            
            grid_testcase.DefaultCellStyle.Font = new Font("Calibri", 10);
            grid_testcase.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
            grid_testcase.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8.9F, FontStyle.Bold);
            grid_testcase.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewLinkColumn lnk_deleterow = new DataGridViewLinkColumn();
            lnk_deleterow.HeaderText = "";
            lnk_deleterow.Text = "Del";
            lnk_deleterow.Name = "btnClickMeforDelete";
            lnk_deleterow.UseColumnTextForLinkValue = true;
            grid_testcase.Columns.Add(lnk_deleterow);            

            grid_testcase.Columns.Add("colActionFlowID", "ActionFlow_id");
            grid_testcase.Columns["colActionFlowID"].Visible = false;

            cmb_pagename.HeaderText = "PageName";
            cmb_pagename.Name = "cmb_pagename";            
            cmb_pagename.FlatStyle = FlatStyle.Flat;            
            grid_testcase.Columns.Add(cmb_pagename);
            cmb_pagename.Items.AddRange((from t in objLib.GetPageTitles() orderby t.Value select t.Value).ToArray());

            grid_testcase.Columns.Add("colActionFlow", "Action");
            grid_testcase.Columns.Add("colFlowIdentifier", "FlowIdentifier");
            grid_testcase.Columns.Add("colSeqNumber", "SeqNumber");
            grid_testcase.Columns["colSeqNumber"].ValueType = typeof(float);
            

            //TestCaseID Label settings            
            lbl_testcaseid.Text = "Test Case ID :";
            lbl_testcaseid.Name = "lbl_testcaseid";
            lbl_testcaseid.TextAlign = ContentAlignment.MiddleLeft;
            lbl_testcaseid.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_testcaseid.Height = 24;
            lbl_testcaseid.Width = 110;

            //TestCaseTitle Label settings            
            lbl_testcasetitle.Text = "Title :";
            lbl_testcasetitle.Name = "lbl_testcasetitle ";
            lbl_testcasetitle.TextAlign = ContentAlignment.MiddleLeft;
            lbl_testcasetitle.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_testcasetitle.Height = 24;
            lbl_testcasetitle.Width = 110;

            //DesignedBy Label settings            
            lbl_designedby.Text = "Designed By :";
            lbl_designedby.Name = "lbl_designedby";
            lbl_designedby.TextAlign = ContentAlignment.MiddleLeft;
            lbl_designedby.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_designedby.Height = 24;
            lbl_designedby.Width = 110;

            //DesignedBy Label settings            
            lbl_testpriority.Text = "Priority :*";            
            lbl_testpriority.TextAlign = ContentAlignment.MiddleLeft;
            lbl_testpriority.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_testpriority.Height = 24;
            lbl_testpriority.Width = 110;

            //DesignedBy Label settings            
            lbl_jira.Text = "    JIRA # :";            
            lbl_jira.TextAlign = ContentAlignment.MiddleLeft;
            lbl_jira.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_jira.Height = 24;
            lbl_jira.Width = 110;

            //AssignedTo Label settings            
            lbl_assignedto.Text = "    Assigned To :*";
            lbl_assignedto.Name = "lbl_assignedto";
            lbl_assignedto.TextAlign = ContentAlignment.MiddleLeft;
            lbl_assignedto.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_assignedto.Height = 24;
            lbl_assignedto.Width = 150;

            //TestCategories Label settings            
            lbl_testcategory.Text = "Test Category :*";
            lbl_testcategory.Name = "lbl_testcategory";
            lbl_testcategory.TextAlign = ContentAlignment.MiddleLeft;
            lbl_testcategory.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_testcategory.Height = 24;
            lbl_testcategory.Width = 150;

            //Functionality Label settings            
            lbl_testcasefun.Text = "    Functionality :*";
            lbl_testcasefun.Name = "lbl_testcasefun";
            lbl_testcasefun.TextAlign = ContentAlignment.MiddleLeft;
            lbl_testcasefun.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_testcasefun.Height = 24;
            lbl_testcasefun.Width = 150;

            //Secondary Functionality Label settings            
            lbl_testcasetags.Text = "    Tags :";
            lbl_testcasetags.Name = "lbl_tags";
            lbl_testcasetags.TextAlign = ContentAlignment.MiddleLeft;
            lbl_testcasetags.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_testcasetags.Height = 24;
            lbl_testcasetags.Width = 150;

            //Release Label settings            
            lbl_release.Text = "    Release :";
            lbl_release.Name = "lbl_release";
            lbl_release.TextAlign = ContentAlignment.MiddleLeft;
            lbl_release.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_release.Height = 24;
            lbl_release.Width = 150;

            //DesignStatus Label settings            
            lbl_designstatus.Text = "State :*";
            lbl_designstatus.Name = "lbl_designstatus";
            lbl_designstatus.TextAlign = ContentAlignment.MiddleLeft;
            lbl_designstatus.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_designstatus.Height = 24;
            lbl_designstatus.Width = 110;

            //TestCaseSummary Label settings            
            lbl_testcasesummary.Text = "Summary :";
            lbl_testcasesummary.Name = "lbl_testcasesummary";
            lbl_testcasesummary.TextAlign = ContentAlignment.MiddleLeft;
            lbl_testcasesummary.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_testcasesummary.Height = 24;
            lbl_testcasesummary.Width = 110;

            //TestCaseSummary Label settings            
            lbl_TCReferenceId.Text = "Test Case Ref:";
            lbl_TCReferenceId.Name = "lbl_TCReferenceId";
            lbl_TCReferenceId.TextAlign = ContentAlignment.MiddleLeft;
            lbl_TCReferenceId.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_TCReferenceId.Height = 24;
            lbl_TCReferenceId.Width = 200;

            

            //TestCaseID textbox settings            
            txt_testcaseid.Text = "";
            txt_testcaseid.Name = "txt_testcaseid";
            txt_testcaseid.Enabled = false;
            txt_testcaseid.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_testcaseid.Height = 24;
            txt_testcaseid.Width = 200;

            //TestCaseTitle textbox settings            
            txt_testcasetitle.Text = "";
            txt_testcasetitle.Name = "txt_testcasetitle";
            txt_testcasetitle.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_testcasetitle.Height = 24;
            txt_testcasetitle.Width = 800;

            cmb_designedby.Name = "cmb_designedby";
            cmb_designedby.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_designedby.Height = 24;
            cmb_designedby.Width = 320;
            cmb_designedby.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_designedby.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_designedby.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;            
            cmb_designedby.Items.AddRange((from t in objLib.GetUserInfo() orderby t.Value select t.Value).ToArray());
            cmb_designedby.SelectedItem = objLib.GetUserInfo()[SignIn.userId.ToUpper()].ToString();
            cmb_designedby.Enabled = false;

            //AssignedTo combobox settings            
            cmb_assignedto.Name = "cmb_assignedto";
            cmb_assignedto.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_assignedto.Height = 24;
            cmb_assignedto.Width = 320;
            cmb_assignedto.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_assignedto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_assignedto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_assignedto.Items.AddRange((from t in objLib.GetUserInfo() orderby t.Value select t.Value).ToArray());

            //Functionality combobox settings            
            cmb_testcasefun.Name = "cmb_testcasefun";
            cmb_testcasefun.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_testcasefun.Height = 24;
            cmb_testcasefun.Width = 320;
            cmb_testcasefun.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_testcasefun.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_testcasefun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_testcasefun.Items.AddRange((from t in objLib.GetFeatures() orderby t.Value select t.Value).ToArray());

            //TestCategories combobox settings            
            cmb_testcategory.Name = "cmb_testcategory";
            cmb_testcategory.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_testcategory.Height = 24;
            cmb_testcategory.Width = 320;
            cmb_testcategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_testcategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_testcategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_testcategory.Items.AddRange(new string[] { "Functional", "Regression", "Smoke Test", "Tesla Switchover Project" });

            //TestCaseFunctionality textbox settings            
            txt_testcasefun.Text = "";
            txt_testcasefun.Name = "txt_testcasefun";
            txt_testcasefun.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_testcasefun.Height = 24;
            txt_testcasefun.Width = 320;
            txt_testcasefun.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_testcasefun.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
            coll.AddRange((from t in objLib.GetFunctionalities() orderby t.Value select t.Value).ToArray());
            txt_testcasefun.AutoCompleteCustomSource = coll;
                       
            txt_jira.Text = "";
            txt_jira.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_jira.Height = 24;
            txt_jira.Width = 320;

            txt_TCReferenceId.Text = "";
            txt_TCReferenceId.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_TCReferenceId.Height = 24;
            txt_TCReferenceId.Width = 320; 

            //TestCase Secondary Functionality textbox settings                        
            txt_testcasetags.Name = "txt_testcasetags";
            txt_testcasetags.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_testcasetags.Height = 24;
            txt_testcasetags.Width = 320;

            //Release textbox settings            
            txt_release.Text = "";
            txt_release.Name = "txt_testcasefun";
            txt_release.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_release.Height = 24;
            txt_release.Width = 320;
            txt_release.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_release.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AutoCompleteStringCollection releasecoll = new AutoCompleteStringCollection();
            releasecoll.AddRange((from t in objLib.GetReleases() orderby t.Value select t.Value).ToArray());
            txt_release.AutoCompleteCustomSource = releasecoll;

            //cmbDesignStatus combobox settings            
            cmb_designstatus.Name = "cmb_designstatus";
            cmb_designstatus.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_designstatus.Height = 24;
            cmb_designstatus.Width = 320;
            cmb_designstatus.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_designstatus.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_designstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_designstatus.Items.AddRange(new string[] { "Design", "Ready", "Closed" });

                
            
            cmb_priority.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_priority.Height = 24;
            cmb_priority.Width = 320;
            cmb_priority.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_priority.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_priority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_priority.Items.AddRange(new string[] { "High", "Medium", "Low" });

            //TestCaseSummary textbox settings            
            txt_testcasesummary.Text = "";
            txt_testcasesummary.Name = "txt_testcasesummary";
            txt_testcasesummary.Multiline = true;
            txt_testcasesummary.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_testcasesummary.Height = 70;
            txt_testcasesummary.Width = 800;
            txt_testcasesummary.ScrollBars = ScrollBars.Vertical;

            //Create button settings            
            btn_createtestcase.Text = "Create";
            btn_createtestcase.Name = "btn_createtestcase";
            btn_createtestcase.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_createtestcase.Height = 30;
            btn_createtestcase.Width = 100;

            //Save button settings            
            btn_savetestcase.Text = "Save";
            btn_savetestcase.Name = "btn_savetestcase";
            btn_savetestcase.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_savetestcase.Height = 30;
            btn_savetestcase.Width = 100;

            //Reset Seq button settings            
            btn_Resettestdataseq.Text = "Reset Seq";
            btn_Resettestdataseq.Name = "btn_Resettestdataseq";
            btn_Resettestdataseq.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_Resettestdataseq.Height = 30;
            btn_Resettestdataseq.Width = 100;
            btn_Resettestdataseq.TextAlign = ContentAlignment.MiddleCenter;

            //Test Data button settings            
            btn_createtestdata.Text = "Test Data";
            btn_createtestdata.Name = "btn_createtestcase";
            btn_createtestdata.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_createtestdata.Height = 30;
            btn_createtestdata.Width = 100;
            btn_createtestdata.Visible = false;

            //Test Data Link settings            
            lnk_edittestdata.Text = "Test Data";
            lnk_edittestdata.Name = "lnk_edittestdata";
            lnk_edittestdata.Font = new Font("Calibri", 10F, FontStyle.Bold);
            lnk_edittestdata.Height = 20;
            lnk_edittestdata.Width = 100;
            lnk_edittestdata.Visible = false;

            lnk_populatetags.Text = "Auto Populate";
            lnk_populatetags.Font = new Font("Calibri", 9F, FontStyle.Regular);
            lnk_populatetags.Height = 20;
            lnk_populatetags.Width = 100;
            lnk_populatetags.TextAlign = ContentAlignment.BottomLeft;
            //lnk_populatetags.Visible = false;

            //Adding Controls to Table Layout Panel
            tlp_testcase.Controls.Add(lbl_testcaseid, 0, 1);
            tlp_testcase.Controls.Add(txt_testcaseid, 1, 1);
            tlp_testcase.Controls.Add(lbl_testcasetags, 4, 1);
            tlp_testcase.Controls.Add(txt_testcasetags, 5, 1);
            tlp_testcase.Controls.Add(lnk_populatetags, 6, 1);
            tlp_testcase.Controls.Add(lbl_testcasetitle, 0, 2);
            tlp_testcase.Controls.Add(txt_testcasetitle, 1, 2);
            tlp_testcase.SetColumnSpan(txt_testcasetitle, 10);
            tlp_testcase.Controls.Add(lbl_designedby, 0, 3);
            tlp_testcase.Controls.Add(cmb_designedby, 1, 3);
            tlp_testcase.Controls.Add(lbl_assignedto, 4, 3);
            tlp_testcase.Controls.Add(cmb_assignedto, 5, 3);
            tlp_testcase.Controls.Add(lbl_testcategory, 0, 4);
            tlp_testcase.Controls.Add(cmb_testcategory, 1, 4);
            tlp_testcase.Controls.Add(lbl_testcasefun, 4, 4);
//            tlp_testcase.Controls.Add(txt_testcasefun, 5, 4);
            tlp_testcase.Controls.Add(cmb_testcasefun, 5, 4);
            tlp_testcase.Controls.Add(lbl_designstatus, 0, 5);
            tlp_testcase.Controls.Add(cmb_designstatus, 1, 5);
            tlp_testcase.Controls.Add(lbl_release, 4, 5);
            tlp_testcase.Controls.Add(txt_release, 5, 5);

            tlp_testcase.Controls.Add(lbl_testpriority, 0, 6);
            tlp_testcase.Controls.Add(cmb_priority, 1, 6);
            tlp_testcase.Controls.Add(lbl_jira, 4, 6);
            tlp_testcase.Controls.Add(txt_jira, 5, 6);
            tlp_testcase.Controls.Add(lbl_TCReferenceId, 0, 7);
            tlp_testcase.Controls.Add(txt_TCReferenceId, 1, 7);
            tlp_testcase.Controls.Add(lbl_testcasesummary, 0,8);
            tlp_testcase.Controls.Add(txt_testcasesummary, 1, 8);
            tlp_testcase.SetColumnSpan(txt_testcasesummary, 10);

            

            tlp_stub.Controls.Add(grid_testcase, 0, 1);
            tlp_stub.Controls.Add(lnk_edittestdata, 0, 2);
            tlp_stub.Controls.Add(btn_savetestcase, 0, 3);
            tlp_stub.Controls.Add(btn_createtestcase, 0, 3);
            tlp_stub.Controls.Add(btn_createtestdata, 0, 3);
            tlp_stub.Controls.Add(btn_Resettestdataseq,1,3);

            //Adding Controls to Flow Layout Panel
            flp_testcase.Controls.AddRange(new Control[] { pic_gecko, tlp_testcase, tlp_stub });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_testcase });
            this.Load += new System.EventHandler(TestCase_Load);

            #endregion

            #region testcase_methods
            btn_createtestcase.Click += new System.EventHandler(btn_createtestcase_Click);
            btn_savetestcase.Click += new System.EventHandler(btn_savetestcase_Click);
            btn_Resettestdataseq.Click += new System.EventHandler(btn_ResetTestdataseq_Click);
            btn_createtestdata.Click += new System.EventHandler(btn_createtestdata_Click);
            lnk_edittestdata.Click += new System.EventHandler(lnk_edittestdata_Click);
            lnk_populatetags.Click += new System.EventHandler(lnk_populatetags_Click);
            grid_testcase.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(grid_testcase_cellClick);
            grid_testcase.SortCompare += new DataGridViewSortCompareEventHandler(grid_testcase_sortcompare);
            grid_testcase.MouseClick += new MouseEventHandler(grid_testcase_MouseClick);
            #endregion
        }
        private void TestCase_Load(object sender, EventArgs e)
        {
            grid_testcase.Columns["btnClickMeforDelete"].Width = 40;
        }
        private void lnk_populatetags_Click(object sender, EventArgs e)
        {
            List<string> pageIds = new List<string>();
            if (DialogResult.Yes == MessageBox.Show("Auto Populate will clear the existing tags. \n\n Do You Want To Proceed?", "Auto Populate", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                var dgv_rows = grid_testcase.Rows;
                foreach (DataGridViewRow row in dgv_rows)
                {
                    pageIds.Add((objLib.GetPageTitles().First(x => x.Value == Convert.ToString(row.Cells["cmb_pagename"].Value)).Key));
                }
                var querystriing = string.Join(",", pageIds.ToArray());
                var dt = objLib.GetTags("SELECT PAGEID,TAGS FROM PAGENAMES WHERE PROJECTID=" + SignIn.projectId + " AND PAGEID IN(" + querystriing + ")");
                var tags = dt.Values.ToArray();

                txt_testcasetags.Text = string.Join(",", tags.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray());
            }
            else
            {

            }
        }
        private void grid_testcase_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Insert Step", MenuItem_Insert_Click));
                m.MenuItems.Add(new MenuItem("Copy", MenuItem_Copy_Click));
                //m.MenuItems.Add(new MenuItem("Cut"));
                m.MenuItems.Add(new MenuItem("Paste", MenuItem_Paste_Click));                
                int currentMouseOverRow = grid_testcase.HitTest(e.X, e.Y).RowIndex;
                if (currentMouseOverRow >= 0)
                {
                 //   m.MenuItems.Add(new MenuItem(string.Format("Do something to row {0}", currentMouseOverRow.ToString())));
                 //   grid_testcase.Rows.Add(currentMouseOverRow);
                }
                m.Show(grid_testcase, new Point(e.X, e.Y));
            }
        }
        private void MenuItem_Insert_Click(Object sender, System.EventArgs e)
        {
            this.grid_testcase.Rows.Insert(grid_testcase.CurrentCell.RowIndex);
        }
        private void MenuItem_Copy_Click(Object sender, System.EventArgs e)
        {
            //string clipboard = "";
            //foreach (DataGridViewRow Row in grid_testcase.SelectedRows)
            //{
            //    foreach (DataGridViewColumn Column in grid_testcase.Columns)
            //        clipboard += grid_testcase.Rows[Row.Index].Cells[Column.Index].FormattedValue.ToString() + " ";
            //    clipboard += "\n";
            //}

            //if (this.grid_testcase.SelectedRows.Count>0)
            //{
            //    StringBuilder ClipboardBuillder = new StringBuilder();
            //    foreach (DataGridViewRow Row in grid_testcase.SelectedRows)
            //    {
            //        foreach (DataGridViewColumn Column in grid_testcase.Columns)
            //        {
            //            ClipboardBuillder.Append(Row.Cells[Column.Index].FormattedValue.ToString() + " ");
            //        }
            //        ClipboardBuillder.AppendLine();
            //    }
            //}

            //--grid_testcase.SelectionMode = DataGridViewSelectionMode.FullRowSelect;      

            //DataGridViewSelectedRowCollection tmpRows = this.grid_testcase.SelectedRows;
            //Clipboard.SetDataObject(tmpRows);            
            Clipboard.SetDataObject(grid_testcase.GetClipboardContent(), true);
        }
        private void MenuItem_Paste_Click(Object sender, System.EventArgs e)
        {
            //if (this.grid_testcase.SelectedRows.Count > 0)
                //grid_testcase.SelectionMode = DataGridViewSelectionMode.FullRowSelect;            
                if(!string.IsNullOrEmpty(Clipboard.GetText()))
                {
                    try
                    {
                        char[] rowSplitter = { '\r', '\n' };
                        char[] columnSplitter = { '\t' };
                        string[] copiedrows = Clipboard.GetText().Split(rowSplitter, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var row in copiedrows)
                        {
                            string[] values = row.Split(columnSplitter);
                            //bool res=values.All(x => x== string.Empty) ? true : false; 
                            if (!values.All(x => x == string.Empty))
                            {
                                grid_testcase.Rows.Insert(grid_testcase.CurrentRow.Index);
                                grid_testcase.Rows[grid_testcase.CurrentRow.Index - 1].Cells["cmb_pagename"].Value = values[2];
                                grid_testcase.Rows[grid_testcase.CurrentRow.Index - 1].Cells["colActionFlow"].Value = values[3];
                                grid_testcase.Rows[grid_testcase.CurrentRow.Index - 1].Cells["colFlowIdentifier"].Value = values[4];
                                //grid_testcase.Rows[grid_testcase.CurrentRow.Index - 1].Cells["colSeqNumber"].Value = values[5];
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Invalid Data Selection");
                        grid_testcase.Rows.RemoveAt(grid_testcase.CurrentRow.Index-1);
                    }
                    
                }
        }
        private void grid_testcase_sortcompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ((e.Column.Name == "colSeqNumber"))
            {
                float a = float.Parse(e.CellValue1.ToString()), b = float.Parse(e.CellValue2.ToString());
                e.SortResult = a.CompareTo(b);
                e.Handled = true;
            }
        }
        private void grid_testcase_cellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 0) && (Convert.ToString(grid_testcase.Rows[e.RowIndex].Cells[0].Value) == "Del"))
            {
                DialogResult confirmDelete = MessageBox.Show("Delete Row # : "+ (e.RowIndex+Int32.Parse("1")) +" ?", "Delete Test Step", MessageBoxButtons.YesNo);
                if (confirmDelete == DialogResult.Yes)
                {
                    if (grid_testcase.Rows[e.RowIndex].Cells["colActionFlowID"].Value != null)
                    {
                        deleteflag.Add(grid_testcase.Rows[e.RowIndex].Cells["colActionFlowID"].Value.ToString());
                    }
                    grid_testcase.Rows.RemoveAt(e.RowIndex);
                }
            }
        }
        private void btn_createtestcase_Click(object sender, EventArgs e)
        {
            string querytestcaseInfo = string.Empty;
            string querytestcaseFlow = string.Empty;            
            try
            {
                querytestcaseInfo = "INSERT INTO TestCaseInfo(Tags,TestCaseTitle,TestCaseSummary,DesignedBy, AssignedTo, State,TestCategory,Functionality,Priority,Jira,Release,ProjectID,CreatedBy,CreatedDate,TCReferenceId) OUTPUT INSERTED.TESTCASEID Values('"
                    + txt_testcasetags.Text.ToString().Replace("'", "''") + "','"
                    + txt_testcasetitle.Text.ToString().Replace("'", "''") + "','"
                    + txt_testcasesummary.Text.ToString().Replace("'", "''") + "','"
                    + cmb_designedby.SelectedItem.ToString() + "','"
                    + cmb_assignedto.SelectedItem.ToString() + "','"
                    + cmb_designstatus.SelectedItem.ToString() + "','"
                    + cmb_testcategory.SelectedItem.ToString() + "','"
                    + cmb_testcasefun.SelectedItem.ToString() + "','"
                    //+ txt_testcasefun.Text.ToString().Replace("'", "''") + "','"
                    + cmb_priority.SelectedItem.ToString() + "','"
                    + txt_jira.Text.ToString().Replace("'", "''") + "','"
                    + txt_release.Text.ToString().Replace("'", "''") + "',"
                    + SignIn.projectId + ",'"
                    + SignIn.userId + "','"
                    + DateTime.Now + "','" 
                    + txt_TCReferenceId.Text + "')";

                for (int i = 0; i < grid_testcase.Rows.Count - 1; i++)
                {
                    querytestcaseFlow = "INSERT INTO ACTIONFLOW(TestCaseId,PageID,ActionFlow,FlowIdentifier,SeqNumber,ProjectID,CreatedBy,CreatedDate) VALUES("
                        + testcaseId + ","
                        + Int32.Parse(objLib.GetPageTitles().First(x => x.Value == Convert.ToString(grid_testcase.Rows[i].Cells["cmb_pagename"].Value)).Key) + ",'"
                        + grid_testcase.Rows[i].Cells["colActionFlow"].Value.ToString() + "',"
                        + (string.IsNullOrEmpty(Convert.ToString(grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value)) ? "NULL" : grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value.ToString()) + ","
                        + (string.IsNullOrEmpty(Convert.ToString(grid_testcase.Rows[i].Cells["colSeqNumber"].Value)) ? "NULL" : grid_testcase.Rows[i].Cells["colSeqNumber"].Value.ToString()) + ","
                        + SignIn.projectId + ",'"
                        + SignIn.userId + "','"
                        + DateTime.Now + "')";

                }

                testcaseId = Int32.Parse(objLib.ExecuteScalar(querytestcaseInfo).ToString());
                txt_testcaseid.Text = testcaseId.ToString();
                tlp_testcase.Enabled = false;
                for (int i = 0; i < grid_testcase.Rows.Count - 1; i++)
                {
                    try
                    {
                        querytestcaseFlow = "INSERT INTO ACTIONFLOW(TestCaseId,PageID,ActionFlow,FlowIdentifier,SeqNumber,ProjectID,CreatedBy,CreatedDate) VALUES("
                            + testcaseId + ","
                            + Int32.Parse(objLib.GetPageTitles().First(x => x.Value == Convert.ToString(grid_testcase.Rows[i].Cells["cmb_pagename"].Value)).Key) + ",'"
                            + grid_testcase.Rows[i].Cells["colActionFlow"].Value.ToString() + "',"
                            + (string.IsNullOrEmpty(Convert.ToString(grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value)) ? "NULL" : grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value.ToString()) + ","
                            + (string.IsNullOrEmpty(Convert.ToString(grid_testcase.Rows[i].Cells["colSeqNumber"].Value)) ? "NULL" : grid_testcase.Rows[i].Cells["colSeqNumber"].Value.ToString()) + ","
                            + SignIn.projectId + ",'"
                            + SignIn.userId + "','"
                            + DateTime.Now + "')";
                        objLib.RunQuery(querytestcaseFlow);                        
                    }
                    catch
                    {
                        MessageBox.Show("ERROR @ Test Case Flow - Row # : " + i,"Create Test Case");
                    }                    
                }
                MessageBox.Show("Test Case Created Successfully", "Create Test Case", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btn_createtestcase.Visible = false;
                btn_createtestdata.Visible = true;
                grid_testcase.ReadOnly = true;
            }
            catch
            {
                MessageBox.Show("ERROR : Invalid Inputs. Please Check." , "Create Test Case", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_savetestcase_Click(object sender, EventArgs e)
        {
            string querytestcaseInfo = string.Empty;
            string querytestcaseFlow = string.Empty;
            try
            {
                foreach (var actionflowid in deleteflag)
                {
                    objLib.RunQuery("DELETE FROM ACTIONFLOW WHERE ACTIONFLOW_ID=" + actionflowid + " AND PROJECTID=" + SignIn.projectId);
                }
                for (int i = 0; i < grid_testcase.Rows.Count - 1; i++)
                {
                    if (grid_testcase.Rows[i].Cells["colActionFlowID"].Value == null)
                    {
                        querytestcaseFlow = "INSERT INTO ACTIONFLOW(TestCaseId,PageID,ActionFlow,FlowIdentifier,SeqNumber,ProjectID,CreatedBy,CreatedDate) VALUES("
                        + txt_testcaseid.Text + ","
                        + Int32.Parse(objLib.GetPageTitles().First(x => x.Value == Convert.ToString(grid_testcase.Rows[i].Cells["cmb_pagename"].Value)).Key) + ",'"
                        + grid_testcase.Rows[i].Cells["colActionFlow"].Value.ToString() + "',"
                        + (string.IsNullOrEmpty(Convert.ToString(grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value)) ? "NULL" : grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value.ToString()) + ","
                        + (string.IsNullOrEmpty(Convert.ToString(grid_testcase.Rows[i].Cells["colSeqNumber"].Value)) ? "NULL" : grid_testcase.Rows[i].Cells["colSeqNumber"].Value.ToString()) + ","
                        + SignIn.projectId + ",'"
                        + SignIn.userId + "','"
                        + DateTime.Now + "')";
                    }
                    else
                    {
                        querytestcaseFlow = "UPDATE ACTIONFLOW SET PageID=" + Int32.Parse(objLib.GetPageTitles().First(x => x.Value == Convert.ToString(grid_testcase.Rows[i].Cells["cmb_pagename"].Value)).Key) +
                                                  ",ActionFlow='" + grid_testcase.Rows[i].Cells["colActionFlow"].Value.ToString() +
                                                  "',FlowIdentifier=" + (Convert.ToString(grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value) == string.Empty ? "NULL" : grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value.ToString()) +
                                                  ",SeqNumber=" + (grid_testcase.Rows[i].Cells["colSeqNumber"].Value.ToString() == string.Empty ? "NULL" : grid_testcase.Rows[i].Cells["colSeqNumber"].Value.ToString()) +
                                                  ",ModifiedBy='" + SignIn.userId + 
                                                  "',ModifiedDate='" + DateTime.Now +  
                                                  "' where ActionFlow_id=" + grid_testcase.Rows[i].Cells["colActionFlowID"].Value.ToString() + " AND PROJECTID=" + SignIn.projectId;
                    }                    
                }
                        querytestcaseInfo = "UPDATE TestCaseInfo SET TestCaseTitle='" + txt_testcasetitle.Text.ToString().Replace("'", "''") +
                                                 "',Tags='" + txt_testcasetags.Text.ToString().Replace("'", "''") +
                                                 "',TestCaseSummary='" + txt_testcasesummary.Text.ToString().Replace("'", "''") +
                                                 "',DesignedBy='" + cmb_designedby.SelectedItem.ToString() +
                                                 "',AssignedTo='" + cmb_assignedto.SelectedItem.ToString() +
                                                 "',State='" + cmb_designstatus.SelectedItem.ToString() +
                                                 "',Functionality='" + cmb_testcasefun.SelectedItem.ToString() +
                                                 "',TestCategory='" + cmb_testcategory.SelectedItem.ToString() +
                                                 "',Priority='" + cmb_priority.SelectedItem.ToString() +
                                                 "',Jira='" + txt_jira.Text.ToString().Replace("'", "''") +
                                                 "',Release='" + txt_release.Text.ToString().Replace("'", "''") +
                                                 "',ModifiedBy='" + SignIn.userId + 
                                                 "',ModifiedDate='" + DateTime.Now + 
                                                 "',TCReferenceId='" +txt_TCReferenceId.Text + 
                                                 "' where TestCaseID=" + txt_testcaseid.Text + " AND PROJECTID=" + SignIn.projectId;
                for (int i = 0; i < grid_testcase.Rows.Count - 1; i++)
                {
                    if (grid_testcase.Rows[i].Cells["colActionFlowID"].Value == null)
                    {
                        querytestcaseFlow = "INSERT INTO ACTIONFLOW(TestCaseId,PageID,ActionFlow,FlowIdentifier,SeqNumber,ProjectID,CreatedBy,CreatedDate) VALUES("
                        + txt_testcaseid.Text + ","
                        + Int32.Parse(objLib.GetPageTitles().First(x => x.Value == Convert.ToString(grid_testcase.Rows[i].Cells["cmb_pagename"].Value)).Key) + ",'"
                        + grid_testcase.Rows[i].Cells["colActionFlow"].Value.ToString() + "',"
                        + (string.IsNullOrEmpty(Convert.ToString(grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value)) ? "NULL" : grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value.ToString()) + ","
                        + (string.IsNullOrEmpty(Convert.ToString(grid_testcase.Rows[i].Cells["colSeqNumber"].Value)) ? "NULL" : grid_testcase.Rows[i].Cells["colSeqNumber"].Value.ToString()) + ","
                        + SignIn.projectId + ",'"
                        + SignIn.userId + "','"
                        + DateTime.Now + "')";
                    }
                    else
                    {
                        querytestcaseFlow = "UPDATE ACTIONFLOW SET PageID=" + Int32.Parse(objLib.GetPageTitles().First(x => x.Value == Convert.ToString(grid_testcase.Rows[i].Cells["cmb_pagename"].Value)).Key) +
                                                  ",ActionFlow='" + grid_testcase.Rows[i].Cells["colActionFlow"].Value.ToString() +
                                                  "',FlowIdentifier=" + (Convert.ToString(grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value) == string.Empty ? "NULL" : grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value.ToString()) +
                                                  ",SeqNumber=" + (grid_testcase.Rows[i].Cells["colSeqNumber"].Value.ToString() == string.Empty ? "NULL" : grid_testcase.Rows[i].Cells["colSeqNumber"].Value.ToString()) +
                                                  ",ModifiedBy='" + SignIn.userId +
                                                  "',ModifiedDate='" + DateTime.Now +
                                                  "' where ActionFlow_id=" + grid_testcase.Rows[i].Cells["colActionFlowID"].Value.ToString() + " AND PROJECTID=" + SignIn.projectId;
                    }
                    objLib.RunQuery(querytestcaseFlow);
                }
                        querytestcaseInfo = "UPDATE TestCaseInfo SET TestCaseTitle='" + txt_testcasetitle.Text.ToString().Replace("'", "''") +
                                                 "',Tags='" + txt_testcasetags.Text.ToString().Replace("'", "''") +
                                                 "',TestCaseSummary='" + txt_testcasesummary.Text.ToString().Replace("'", "''") +
                                                 "',DesignedBy='" + cmb_designedby.SelectedItem.ToString() +
                                                 "',AssignedTo='" + cmb_assignedto.SelectedItem.ToString() +
                                                 "',State='" + cmb_designstatus.SelectedItem.ToString() +
                                                 "',Functionality='" + cmb_testcasefun.SelectedItem.ToString() +
                                                 "',TestCategory='" + cmb_testcategory.SelectedItem.ToString() +
                                                 "',Priority='" + cmb_priority.SelectedItem.ToString() +
                                                 "',Jira='" + txt_jira.Text.ToString().Replace("'", "''") +
                                                 "',Release='" + txt_release.Text.ToString().Replace("'", "''") +
                                                 "',ModifiedBy='" + SignIn.userId +
                                                 "',ModifiedDate='" + DateTime.Now +
                                                 "',TCReferenceId='" + txt_TCReferenceId.Text + 
                                                 "' where TestCaseID=" + txt_testcaseid.Text + " AND PROJECTID=" + SignIn.projectId;
                        objLib.RunQuery(querytestcaseInfo);
                MessageBox.Show("Test Case Updated Successfully", "Edit Test Case", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch
            {
                MessageBox.Show("ERROR : Invalid Inputs.Please Check.", "Edit Test Case", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        private void btn_createtestdata_Click(object sender, EventArgs e)
        {
            TestData objTestData = new TestData();
            objTestData.tlp_testdata.Enabled = false;
            objTestData.btn_savetestdata.Visible = false;
            objTestData.txt_testcaseid.Enabled = false;
            objTestData.txt_testcaseid.Text = testcaseId.ToString();
            objTestData.txt_testcasetitle.Text = txt_testcasetitle.Text;
            objTestData.txt_testcasesummary.Text = txt_testcasesummary.Text;
            objTestData.cmb_testcasefun.SelectedItem = cmb_testcasefun.SelectedItem.ToString();
            objTestData.txt_release.Text = txt_release.Text;
            objTestData.cmb_assignedto.SelectedItem = cmb_assignedto.SelectedItem.ToString();
            objTestData.cmb_designedby.SelectedItem = cmb_designedby.SelectedItem.ToString();
            objTestData.cmb_designstatus.SelectedItem = cmb_designstatus.SelectedItem.ToString();
            objTestData.cmb_testcategory.SelectedItem = cmb_testcategory.SelectedItem.ToString();
            this.Close();
            objTestData.ShowDialog();
        }
        private void lnk_edittestdata_Click(object sender, EventArgs e)
        {
            TestData td = new TestData();
            td.btn_createtestdata.Visible = false;
            td.tlp_testdata.Enabled = false;
            DataTable dtTestInfo = new DataTable();
            dtTestInfo = objLib.binddataTable("SELECT * FROM TESTCASEINFO WHERE TESTCASEID=" + txt_testcaseid.Text.Trim() + " AND PROJECTID=" + SignIn.projectId);
            td.txt_testcaseid.Text = dtTestInfo.Rows[0]["TestCaseID"].ToString();
            td.txt_testcasetitle.Text = dtTestInfo.Rows[0]["TestCaseTitle"].ToString();
            td.txt_testcasesummary.Text = dtTestInfo.Rows[0]["TestCaseSummary"].ToString();
            td.cmb_testcasefun.SelectedItem = dtTestInfo.Rows[0]["Functionality"].ToString();
            td.txt_release.Text = dtTestInfo.Rows[0]["Release"].ToString();
            td.cmb_assignedto.SelectedItem = dtTestInfo.Rows[0]["AssignedTo"].ToString();
            td.cmb_designedby.SelectedItem = dtTestInfo.Rows[0]["DesignedBy"].ToString();
            td.cmb_designstatus.SelectedItem = dtTestInfo.Rows[0]["State"].ToString();
            td.cmb_testcategory.SelectedItem = dtTestInfo.Rows[0]["TestCategory"].ToString();

            DataTable dtTestData = new DataTable();
            dtTestData = objLib.binddataTable("SELECT * FROM TESTDATAVIEW WHERE TESTCASEID=" + txt_testcaseid.Text.Trim() + " AND PROJECTID=" + SignIn.projectId + " ORDER BY FLOWIDENTIFIER,DATAIDENTIFIER,SEQNUMBER ASC");          
            Dictionary<string, string> labelColl = new Dictionary<string, string>();
            Dictionary<string, string> pageColl = new Dictionary<string, string>();
            pageColl=objLib.GetPageTitles();
            labelColl = objLib.GetLabels();
            for (int i = 0; i < dtTestData.Rows.Count; i++)
            {
                td.grid_testdata.Rows.Add();
                var pageId = pageColl.First(x => x.Value == dtTestData.Rows[i]["PageName"].ToString()).Key;
                var pageLabels = labelColl.Where(x => x.Key.Split('_')[0].ToString()==pageId).Select(x => x.Value).ToArray();

                DataGridViewComboBoxCell labelCell = (DataGridViewComboBoxCell)(td.grid_testdata.Rows[i].Cells["cmb_label"]);
                labelCell.DataSource = null;
                labelCell.Items.Clear();
                labelCell.Items.AddRange(pageLabels);
                
                td.grid_testdata.Rows[i].Cells["colActionFlowID"].Value = dtTestData.Rows[i]["ActionFlow_id"].ToString();
                td.grid_testdata.Rows[i].Cells["cmb_pagename"].Value = dtTestData.Rows[i]["PageName"].ToString();
                td.grid_testdata.Rows[i].Cells["colFlowIdentifier"].Value = dtTestData.Rows[i]["FlowIdentifier"].ToString();
                td.grid_testdata.Rows[i].Cells["colDataIdentifier"].Value = dtTestData.Rows[i]["DataIdentifier"].ToString();
                td.grid_testdata.Rows[i].Cells["cmb_indicator"].Value = dtTestData.Rows[i]["Keyword"].ToString();
                td.grid_testdata.Rows[i].Cells["cmb_label"].Value = dtTestData.Rows[i]["Label"].ToString();
                td.grid_testdata.Rows[i].Cells["colActionORData"].Value = dtTestData.Rows[i]["ActionORData"].ToString();
                td.grid_testdata.Rows[i].Cells["colSeqNumber"].Value = dtTestData.Rows[i]["SeqNumber"].ToString();
                td.grid_testdata.Rows[i].Cells["cmb_execute"].Value = dtTestData.Rows[i]["Execute"].ToString();
            }
            //td.grid_testdata.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //td.grid_testdata.Columns["cmb_execute"].Width = 80;
            //td.grid_testdata.Columns["cmb_pagename"].Width = 150;
            //td.grid_testdata.Columns["colFlowIdentifier"].Width = 80;
            //td.grid_testdata.Columns["colDataIdentifier"].Width = 80;
            //td.grid_testdata.Columns["cmb_indicator"].Width = 150;
            //td.grid_testdata.Columns["cmb_label"].Width = 250;
            //td.grid_testdata.Columns["colActionORData"].Width = 150;
            //td.grid_testdata.Columns["colSeqNumber"].Width = 80;

            td.grid_testdata.Columns["btnClickMeforDelete"].Width = 40;
            td.grid_testdata.Columns["cmb_execute"].Width = 80;
            td.grid_testdata.Columns["cmb_pagename"].Width = 130;
            td.grid_testdata.Columns["colFlowIdentifier"].Width = 60;
            td.grid_testdata.Columns["colDataIdentifier"].Width = 60;
            td.grid_testdata.Columns["cmb_indicator"].Width = 150;
            td.grid_testdata.Columns["cmb_label"].Width = 180;
            td.grid_testdata.Columns["colActionORData"].Width = 160;
            td.grid_testdata.Columns["colSeqNumber"].Width = 80;
            //grid_testdata.Columns["btnClickMeforDelete"].Width = 40;
            td.ShowDialog();
        }
        private void btn_ResetTestdataseq_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grid_testcase.Rows.Count - 1; i++)
            {
                grid_testcase.Rows[i].Cells["colSeqNumber"].Value = i + 1;
            }
            MessageBox.Show("Sequence Number Updated Successfully", "Test Case", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
