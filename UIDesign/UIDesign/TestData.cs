
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;

namespace UIDesign
{
    public partial class TestData : Form
    {
        private Label lbl_testcaseid = new Label();
        private Label lbl_testcasetitle = new Label();
        private Label lbl_designedby = new Label();
        private Label lbl_assignedto = new Label();
        private Label lbl_testcategory = new Label();
        private Label lbl_testcasefun = new Label();
        private Label lbl_designstatus = new Label();
        private Label lbl_testcasesummary = new Label();
        private Label lbl_release = new Label();

        private Label lbl_TemplateTCId = new Label();
        public TextBox txt_TemplateTCId = new TextBox();
        public static string TemplateTCId;
        public static string PageId;

        public TextBox txt_testcaseid = new TextBox();
        public TextBox txt_release = new TextBox();
        public TextBox txt_testcasetitle = new TextBox();
        public ComboBox cmb_designedby = new ComboBox();
        public ComboBox cmb_assignedto = new ComboBox();
        public ComboBox cmb_testcategory = new ComboBox();
        public TextBox txt_testcasefun = new TextBox();
        public ComboBox cmb_testcasefun = new ComboBox();
        public ComboBox cmb_designstatus = new ComboBox();
        public TextBox txt_testcasesummary = new TextBox();
        public Button btn_Resettestdataseq = new Button();
        public Button btn_TestCaseFind = new Button();
        public ComboBox cmb_TestPagename = new ComboBox();

        public Button btn_savetestdata = new Button();
        public Button btn_createtestdata = new Button();
        public Button btn_search = new Button();

        private DataGridViewComboBoxColumn cmb_execute = new DataGridViewComboBoxColumn();
        private DataGridViewComboBoxColumn cmb_pagename = new DataGridViewComboBoxColumn();
        private DataGridViewComboBoxColumn cmb_indicator = new DataGridViewComboBoxColumn();
        private DataGridViewComboBoxColumn cmb_label = new DataGridViewComboBoxColumn();   

        private PictureBox pic_gecko = new PictureBox();
        public DataGridView grid_testdata = new DataGridView();
        private FlowLayoutPanel flp_testdata = new FlowLayoutPanel();
        public TableLayoutPanel tlp_testdata = new TableLayoutPanel();
        private TableLayoutPanel tlp_testdatastub = new TableLayoutPanel();
        private TableLayoutPanel tlp_searchStub = new TableLayoutPanel();
        
        private TableLayoutPanel tlp_searchTestCase = new TableLayoutPanel();

        TableLayoutPanel tlp_search = new TableLayoutPanel();



        FuncLib objLib = new FuncLib();
        private List<string> deleteflag = new List<string>();
        private List<string> editflag = new List<string>();
        private List<int> StepsToBeDeleted = new List<int>();

       // private void txt_TemplateTCId_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e);

        public TestData()
        {
            InitializeComponent();

            #region testdata_design

            //Form Settings
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            //this.Size = new Size(1350, 750);
            this.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 100);
            this.AutoScroll = true;
            this.Text = "Test Data";
            this.MaximizeBox = true;
            this.MinimizeBox = false;

            //Flow Layout Panel Settings            
            flp_testdata.FlowDirection = FlowDirection.LeftToRight;
            flp_testdata.SetFlowBreak(tlp_testdata, true);
            flp_testdata.Dock = DockStyle.Top;
            flp_testdata.AutoSize = true;

            btn_search.Text = "Search";
            btn_search.Font = new Font("Calibri", 10F, FontStyle.Bold);
            btn_search.TextAlign = ContentAlignment.MiddleCenter;
            btn_search.Height = 24;
            btn_search.Width = 100;

            tlp_search.Controls.Add(new Label { Name = "lbl1", Height = 20, Width = 120, Text = "Search Criteria :", TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Calibri", 10F, FontStyle.Bold) }, 1, 1);            
            tlp_search.Controls.Add(new CheckBox { Name = "chk2", Height = 20, Width = 120, Text = "PageName", TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Calibri", 9F) }, 1, 3);
            tlp_search.Controls.Add(new TextBox { Name = "txt2", Height = 20, Width = 120, Font = new Font("Calibri", 9F) }, 2, 3);
            tlp_search.Controls.Add(new CheckBox { Name = "chk3", Height = 20, Width = 120, Text = "FlowIdentifier", TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Calibri", 9F) }, 1, 4);
            tlp_search.Controls.Add(new TextBox { Name = "txt3", Height = 20, Width = 120, Font = new Font("Calibri", 9F) }, 2, 4);
            tlp_search.Controls.Add(new CheckBox { Name = "chk4", Height = 20, Width = 120, Text = "DataIdentifier", TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Calibri", 9F) }, 1, 5);
            tlp_search.Controls.Add(new TextBox { Name = "txt4", Height = 20, Width = 120, Font = new Font("Calibri", 9F) }, 2, 5);
            tlp_search.Controls.Add(new CheckBox { Name = "chk5", Height = 20, Width = 120, Text = "Label", TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Calibri", 9F) }, 1, 6);
            tlp_search.Controls.Add(new TextBox { Name = "txt5", Height = 20, Width = 120, Font = new Font("Calibri", 9F) }, 2, 6);
            tlp_search.Controls.Add(new CheckBox { Name = "chk6", Height = 20, Width = 120, Text = "Keyword", TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Calibri", 9F) }, 1, 7);
            tlp_search.Controls.Add(new TextBox { Name = "txt6", Height = 20, Width = 120, Font = new Font("Calibri", 9F) }, 2, 7);
            tlp_search.Controls.Add(new CheckBox { Name = "chk7", Height = 20, Width = 120, Text = "ActionORData", TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Calibri", 9F) }, 1, 8);
            tlp_search.Controls.Add(new TextBox { Name = "txt", Height = 20, Width = 120, Font = new Font("Calibri", 9F) }, 2, 8);
            tlp_search.Controls.Add(new CheckBox { Name = "chk8", Height = 20, Width = 120, Text = "SeqNumber", TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Calibri", 9F) }, 1, 9);
            tlp_search.Controls.Add(new TextBox { Name = "txt8", Height = 20, Width = 120, Font = new Font("Calibri", 9F) }, 2, 9);
            tlp_search.Controls.Add(new CheckBox { Name = "chk9", Height = 20, Width = 120, Text = "Execute", TextAlign = ContentAlignment.MiddleLeft, Font = new Font("Calibri", 9F) }, 1, 10);
            tlp_search.Controls.Add(new TextBox { Name = "txt9", Height = 20, Width = 120, Font = new Font("Calibri", 9F) }, 2, 10);           
            tlp_search.Controls.Add(btn_search, 2, 13);            
            tlp_search.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            //Table Layout Panel Settings                                    
            tlp_testdata.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_testdata.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_testdata.AutoSize = true;
            tlp_testdata.Location = new Point(30, 80);

            //Table layout panel settings for TestCase Search

            tlp_searchTestCase.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_searchTestCase.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_searchTestCase.AutoSize = true;
            tlp_searchTestCase.Location = new Point(50, 80);

            //Table Layout Panel Settings                                    
            tlp_testdatastub.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_testdatastub.Font = new Font("Calibri", 9.8F, FontStyle.Regular);            
            tlp_testdatastub.Size = new Size(1400, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 400);
            tlp_testdatastub.Location = new Point(30, 80);

            //Gecko Picture Box Settings            
            pic_gecko.Height = 150;
            pic_gecko.Width = 200;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            DataGridViewLinkColumn lnk_deleterow = new DataGridViewLinkColumn();
            lnk_deleterow.HeaderText = "";
            lnk_deleterow.Text = "Del";
            lnk_deleterow.Name = "btnClickMeforDelete";
            lnk_deleterow.UseColumnTextForLinkValue = true;
            grid_testdata.Columns.Add(lnk_deleterow);

            //DataGridViewLinkColumn lnk_editrow = new DataGridViewLinkColumn();
            //lnk_editrow.HeaderText = "";
            //lnk_editrow.Text = "Edit";
            //lnk_editrow.Name = "btnClickMeforEdit";
            //lnk_editrow.UseColumnTextForLinkValue = true;
            //grid_testdata.Columns.Add(lnk_editrow);

            //Data Gridview Settings
            grid_testdata.ReadOnly = false;            
            grid_testdata.Dock = DockStyle.None;
            //grid_testdata.AutoGenerateColumns = true;
            grid_testdata.Size = new Size(1000, 300);
            grid_testdata.DefaultCellStyle.Font = new Font("Calibri", 10);
            grid_testdata.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
            grid_testdata.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8.9F, FontStyle.Bold);
            //grid_testdata.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            grid_testdata.Columns.Add("colActionFlowID", "ActionFlow_id");
            grid_testdata.Columns["colActionFlowID"].Visible = false;
            grid_testdata.Columns.Add(cmb_execute);
            cmb_execute.HeaderText = "Execute";
            cmb_execute.Name = "cmb_execute";
            cmb_execute.FlatStyle = FlatStyle.Flat;
            grid_testdata.Columns.Add(cmb_pagename);
            cmb_pagename.HeaderText = "PageName";
            cmb_pagename.Name = "cmb_pagename";
            cmb_pagename.FlatStyle = FlatStyle.Flat;
            grid_testdata.Columns.Add("colFlowIdentifier", "FlowIdentifier");
            grid_testdata.Columns.Add("colDataIdentifier", "DataIdentifier");
            grid_testdata.Columns.Add(cmb_indicator);
            cmb_indicator.HeaderText = "Indicator";
            cmb_indicator.Name = "cmb_indicator";
            cmb_indicator.FlatStyle = FlatStyle.Flat;
            //grid_testdata.Columns.Add("colIndicator", "Indicator");
            //grid_testdata.Columns.Add("colLabel", "Label");
            grid_testdata.Columns.Add(cmb_label);
            cmb_label.HeaderText = "Label";
            cmb_label.Name = "cmb_label";
            cmb_label.FlatStyle = FlatStyle.Flat;
            grid_testdata.Columns.Add("colActionORData", "ActionORData");
            grid_testdata.Columns["colActionORData"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            grid_testdata.Columns.Add("colSeqNumber", "SeqNumber");            
            grid_testdata.Columns["colSeqNumber"].ValueType =typeof(float);
            grid_testdata.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


            grid_testdata.Columns["btnClickMeforDelete"].Width = 40;
            grid_testdata.Columns["cmb_execute"].Width = 80;
            grid_testdata.Columns["cmb_pagename"].Width = 130;
            grid_testdata.Columns["colFlowIdentifier"].Width = 60;
            grid_testdata.Columns["colDataIdentifier"].Width = 60;
            grid_testdata.Columns["cmb_indicator"].Width = 150;
            grid_testdata.Columns["cmb_label"].Width = 180;
            grid_testdata.Columns["colActionORData"].Width = 160;
            grid_testdata.Columns["colSeqNumber"].Width = 80;
            //grid_testdata.Columns["btnClickMeforDelete"].Width = 40;

            tlp_search.Height = grid_testdata.Height;// Screen.PrimaryScreen.WorkingArea.Height - grid_testdata.Height - 380;
            tlp_search.Width = tlp_testdatastub.Width - grid_testdata.Width-50;

            //grid_testdata.Sort(grid_testdata.Columns["colSeqNumber"], ListSortDirection.Ascending);
            //grid_testdata.Columns["colSeqNumber"].HeaderCell.SortGlyphDirection = System.Windows.Forms.SortOrder.Ascending;
            
            cmb_pagename.Items.AddRange((from t in objLib.GetPageTitles() orderby t.Value select t.Value).ToArray());            
            cmb_indicator.Items.AddRange((from t in objLib.GetKeywords() orderby t.Value select t.Value).ToArray());            
            cmb_label.Items.AddRange((from t in objLib.GetLabels() orderby t.Value select t.Value).ToArray());
            cmb_execute.Items.Add("Yes");
            cmb_execute.Items.Add("No");

            //TestCaseID Label settings            
            lbl_testcaseid.Text = "Test Case ID";
            lbl_testcaseid.Name = "lbl_testcaseid";
            lbl_testcaseid.TextAlign = ContentAlignment.MiddleLeft;
            lbl_testcaseid.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_testcaseid.Height = 24;
            lbl_testcaseid.Width = 110;

            //TestCaseID Label settings            
            lbl_TemplateTCId.Text = "Enter Template Test Case ID";
            lbl_TemplateTCId.Name = "lbl_TemplateTCId";
            lbl_TemplateTCId.TextAlign = ContentAlignment.MiddleLeft;
            lbl_TemplateTCId.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_TemplateTCId.Height = 24;
            lbl_TemplateTCId.Width = 200;


            //TestCaseTitle Label settings            
            lbl_testcasetitle.Text = "Title";
            lbl_testcasetitle.Name = "lbl_testcasetitle ";
            lbl_testcasetitle.TextAlign = ContentAlignment.MiddleLeft;
            lbl_testcasetitle.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_testcasetitle.Height = 24;
            lbl_testcasetitle.Width = 110;

            //Release Label settings            
            lbl_release.Text = "     Release";
            lbl_release.Name = "lbl_release";
            lbl_release.TextAlign = ContentAlignment.MiddleLeft;
            lbl_release.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_release.Height = 24;
            lbl_release.Width = 150;

            //DesignedBy Label settings            
            lbl_designedby.Text = "Designed By";
            lbl_designedby.Name = "lbl_designedby";
            lbl_designedby.TextAlign = ContentAlignment.MiddleLeft;
            lbl_designedby.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_designedby.Height = 24;
            lbl_designedby.Width = 110;

            //AssignedTo Label settings            
            lbl_assignedto.Text = "     Assigned To";
            lbl_assignedto.Name = "lbl_assignedto";
            lbl_assignedto.TextAlign = ContentAlignment.MiddleLeft;
            lbl_assignedto.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_assignedto.Height = 24;
            lbl_assignedto.Width = 150;

            //TestCategories Label settings            
            lbl_testcategory.Text = "Test Category";
            lbl_testcategory.Name = "lbl_testcategory";
            lbl_testcategory.TextAlign = ContentAlignment.MiddleLeft;
            lbl_testcategory.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_testcategory.Height = 24;
            lbl_testcategory.Width = 150;

            //TestCategories Label settings            
            lbl_testcasefun.Text = "     Functionality";
            lbl_testcasefun.Name = "lbl_testcasefun";
            lbl_testcasefun.TextAlign = ContentAlignment.MiddleLeft;
            lbl_testcasefun.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_testcasefun.Height = 24;
            lbl_testcasefun.Width = 150;

            //DesignStatus Label settings            
            lbl_designstatus.Text = "State";
            lbl_designstatus.Name = "lbl_designstatus";
            lbl_designstatus.TextAlign = ContentAlignment.MiddleLeft;
            lbl_designstatus.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_designstatus.Height = 24;
            lbl_designstatus.Width = 110;

            //TestCaseSummary Label settings            
            lbl_testcasesummary.Text = "Summary";
            lbl_testcasesummary.Name = "lbl_testcasesummary";
            lbl_testcasesummary.TextAlign = ContentAlignment.MiddleLeft;
            lbl_testcasesummary.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_testcasesummary.Height = 24;
            lbl_testcasesummary.Width = 110;

            //TestCaseID textbox settings                        
            txt_testcaseid.Name = "txt_testcaseid";
            txt_testcaseid.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_testcaseid.Height = 24;
            txt_testcaseid.Width = 200;

            //Template TestCase Id textbox settings                        
            txt_TemplateTCId.Name = "txt_TemplateTCId";
            txt_TemplateTCId.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_TemplateTCId.Height = 24;
            txt_TemplateTCId.Width = 200;

            //TestCaseTitle textbox settings                        
            txt_testcasetitle.Name = "txt_testcasetitle";
            txt_testcasetitle.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_testcasetitle.Height = 24;
            txt_testcasetitle.Width = 800;
            txt_testcasetitle.ReadOnly = true;

            cmb_designedby.Name = "cmb_designedby";
            cmb_designedby.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_designedby.Height = 24;
            cmb_designedby.Width = 320;
            cmb_designedby.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_designedby.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_designedby.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_designedby.Items.AddRange((from t in objLib.GetUserInfo() orderby t.Value select t.Value).ToArray());
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
            cmb_assignedto.Enabled = false;

            //AssignedTo combobox settings            
            cmb_testcasefun.Name = "cmb_testcasefun";
            cmb_testcasefun.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_testcasefun.Height = 24;
            cmb_testcasefun.Width = 320;
            cmb_testcasefun.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_testcasefun.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_testcasefun.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_testcasefun.Items.AddRange((from t in objLib.GetFeatures() orderby t.Value select t.Value).ToArray());
            cmb_testcasefun.Enabled = false;

            //TestCategories combobox settings            
            cmb_testcategory.Name = "cmb_testcategory";
            cmb_testcategory.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_testcategory.Height = 24;
            cmb_testcategory.Width = 320;
            cmb_testcategory.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_testcategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_testcategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_testcategory.Items.AddRange(new string[] { "Functional", "Regression", "Smoke Test" });
            cmb_testcategory.Enabled = false;

            //TestCaseFunctionality textbox settings            
            txt_testcasefun.Text = "";
            txt_testcasefun.Name = "txt_testcasefun";
            txt_testcasefun.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_testcasefun.Height = 24;
            txt_testcasefun.Width = 320;
            txt_testcasefun.ReadOnly = true;

            //Release textbox settings            
            txt_release.Text = "";
            txt_release.Name = "txt_release";
            txt_release.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_release.Height = 24;
            txt_release.Width = 320;
            txt_release.ReadOnly = true;

            //cmbDesignStatus combobox settings            
            cmb_designstatus.Name = "cmb_designstatus";
            cmb_designstatus.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_designstatus.Height = 24;
            cmb_designstatus.Width = 320;
            cmb_designstatus.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_designstatus.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_designstatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_designstatus.Items.AddRange(new string[] { "Design", "Ready", "Closed" });
            cmb_designstatus.Enabled = false;

            //TestCaseSummary textbox settings                        
            txt_testcasesummary.Name = "txt_testcasesummary";
            txt_testcasesummary.Multiline = true;
            txt_testcasesummary.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_testcasesummary.Height = 70;
            txt_testcasesummary.Width = 800;
            txt_testcasesummary.ReadOnly = true;

           

            //Save button settings            
            btn_savetestdata.Text = "Save";
            btn_savetestdata.Name = "btn_savetestdata";
            btn_savetestdata.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_savetestdata.Height = 30;
            btn_savetestdata.Width = 100;

            //Reset Seq button settings            
            btn_Resettestdataseq.Text = "Reset Seq";
            btn_Resettestdataseq.Name = "btn_Resettestdataseq";
            btn_Resettestdataseq.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_Resettestdataseq.Height = 30;
            btn_Resettestdataseq.Width = 150;

            //Reset Seq button settings            
            btn_TestCaseFind.Text = "Find Test Case Template";
            btn_TestCaseFind.Name = "btn_TestCaseFind";
            btn_TestCaseFind.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_TestCaseFind.Height = 30;
            btn_TestCaseFind.Width = 150;

            //Create button settings            
            btn_createtestdata.Text = "Create";
            btn_createtestdata.Name = "btn_createtestdata";
            btn_createtestdata.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_createtestdata.Height = 30;
            btn_createtestdata.Width = 100;

            //DataGridViewLinkColumn lnk_editrow = new DataGridViewLinkColumn();
            //lnk_editrow.HeaderText = "";
            //lnk_editrow.Text = "Edit";
            //lnk_editrow.Name = "btnClickMeforEdit";
            //lnk_editrow.UseColumnTextForLinkValue = true;
            //grid_testdata.Columns.Add(lnk_editrow);

            DataGridViewTextBoxColumn txt_editrow = new DataGridViewTextBoxColumn();
            txt_editrow.HeaderText = "";            
            txt_editrow.Name = "txtflag";            
            grid_testdata.Columns.Add(txt_editrow);
            grid_testdata.Columns["txtflag"].Visible = false;

            //Adding Controls to Table Layout Panel
            tlp_testdata.Controls.Add(lbl_testcaseid, 0, 1);
            tlp_testdata.Controls.Add(txt_testcaseid, 1, 1);
            tlp_testdata.Controls.Add(lbl_testcasetitle, 0, 2);
            tlp_testdata.Controls.Add(txt_testcasetitle, 1, 2);
            tlp_testdata.SetColumnSpan(txt_testcasetitle, 10);
            
            tlp_testdata.Controls.Add(lbl_designedby, 0, 3);
            tlp_testdata.Controls.Add(cmb_designedby, 1, 3);
            tlp_testdata.Controls.Add(lbl_assignedto, 4, 3);
            tlp_testdata.Controls.Add(cmb_assignedto, 5, 3);

            tlp_testdata.Controls.Add(lbl_testcategory, 0, 4);
            tlp_testdata.Controls.Add(cmb_testcategory, 1, 4);
            tlp_testdata.Controls.Add(lbl_testcasefun, 4, 4);
            tlp_testdata.Controls.Add(cmb_testcasefun, 5, 4);
            
            tlp_testdata.Controls.Add(lbl_designstatus, 0, 5);
            tlp_testdata.Controls.Add(cmb_designstatus, 1, 5);
            tlp_testdata.Controls.Add(lbl_release, 4, 5);
            tlp_testdata.Controls.Add(txt_release, 5, 5);
            tlp_testdata.Controls.Add(lbl_testcasesummary, 0, 6);
            tlp_testdata.Controls.Add(txt_testcasesummary, 1, 6);
            tlp_testdata.SetColumnSpan(txt_testcasesummary, 10);

            tlp_testdatastub.Controls.Add(grid_testdata, 0, 1);
            tlp_testdatastub.Controls.Add(tlp_search, 1, 1);
            tlp_testdatastub.Controls.Add(btn_savetestdata, 0, 2);
            tlp_testdatastub.Controls.Add(btn_createtestdata, 0, 2);
            tlp_testdatastub.Controls.Add(btn_Resettestdataseq, 1, 2);


            cmb_TestPagename.Name = "cmb_TestPagename";
            cmb_TestPagename.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_TestPagename.Height = 24;
            cmb_TestPagename.Width = 320;
            cmb_TestPagename.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_TestPagename.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_TestPagename.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            tlp_searchTestCase.Controls.Add(lbl_TemplateTCId, 0, 7);
            tlp_searchTestCase.Controls.Add(txt_TemplateTCId, 1, 7);
            tlp_searchTestCase.Controls.Add(cmb_TestPagename, 2, 7);
            tlp_searchTestCase.Controls.Add(btn_TestCaseFind, 3, 7);

            //tlp_testdata.Enabled = false;

            //Adding Controls to Flow Layout Panel
            flp_testdata.Controls.AddRange(new Control[] { pic_gecko, tlp_testdata, tlp_searchTestCase, tlp_testdatastub });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_testdata });
            this.Load += new System.EventHandler(TestData_Load);

            #endregion
            
            #region testdata_methods

            txt_testcaseid.Leave += new System.EventHandler(txt_testcaseid_Leave);
            btn_createtestdata.Click += new System.EventHandler(btn_createtestdata_Click);
            btn_Resettestdataseq.Click += new System.EventHandler(btn_ResetTestdataseq_Click);
            btn_savetestdata.Click += new System.EventHandler(btn_savetestdata_Click);
            grid_testdata.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(grid_testdata_cellClick);
            //grid_testdata.SelectedRows += new System.Windows.Forms.DataGridViewSelectedRowCollection();
            grid_testdata.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(grid_testdata_EditingControlShowing);
            grid_testdata.DataError += new DataGridViewDataErrorEventHandler(grid_testdata_dataError);
            grid_testdata.CellEndEdit += new DataGridViewCellEventHandler(grid_testdata_cellendedit);
            grid_testdata.CellBeginEdit += new DataGridViewCellCancelEventHandler(grid_testdata_cellbeginedit);
            grid_testdata.SortCompare += new DataGridViewSortCompareEventHandler(grid_testdata_sortcompare);
            grid_testdata.MouseClick += new MouseEventHandler(grid_testdata_MouseClick);
            btn_search.Click += new System.EventHandler(btn_search_Click);
            btn_TestCaseFind.Click += new System.EventHandler(btn_TestCaseFind_Click);
            txt_TemplateTCId.Leave += new System.EventHandler(txt_TemplateTCIdEnter);
            #endregion
        }
        private void txt_TemplateTCIdEnter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_TemplateTCId.Text))
            {
                Dictionary<string, string> dictpageName = new Dictionary<string, string>();
                dictpageName = objLib.GetPageNames(txt_TemplateTCId.Text);
                if (dictpageName.Count > 0)
                {
                    cmb_TestPagename.DataSource = new BindingSource(dictpageName, null);
                    cmb_TestPagename.DisplayMember = "Text";
                    cmb_TestPagename.ValueMember = "Key";
                }
                else
                {
                    MessageBox.Show("No page found.");
                }
            }
        }
        private void btn_TestCaseFind_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txt_TemplateTCId.Text))
            {
                MessageBox.Show("Plese enter the Test Case Id.");
                return;
            }
            if (cmb_TestPagename.SelectedIndex < 0)
            {
                MessageBox.Show("Plese select page name.");
                return;
            }

            string dropDownvalue =((KeyValuePair<string, string>)cmb_TestPagename.SelectedItem).Value;
            if (dropDownvalue != null)
            {
                TemplateTCId = txt_TemplateTCId.Text;
                PageId = ((KeyValuePair<string, string>)cmb_TestPagename.SelectedItem).Value;
                SampleTestData sampleTestDat = new SampleTestData();
                sampleTestDat.ShowDialog();
            }
        }
        private void grid_testdata_sortcompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if ((e.Column.Name == "colSeqNumber") || (e.Column.Name == "colDataIdentifier") || (e.Column.Name == "colFlowIdentifier"))
            {
                float a = float.Parse(e.CellValue1.ToString()), b = float.Parse(e.CellValue2.ToString());
                e.SortResult = a.CompareTo(b);
                e.Handled = true;
            }            
        }
        private void grid_testdata_MouseClick(object sender, MouseEventArgs e)
                {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("I&nsert Step", MenuItem_Insert_Click));
                m.MenuItems.Add(new MenuItem("C&opy", MenuItem_Copy_Click, Shortcut.CtrlC));
                m.MenuItems.Add(new MenuItem("P&aste", MenuItem_Paste_Click,Shortcut.CtrlV));
                m.MenuItems.Add(new MenuItem("Delete", MenuItem_Delete_Click));  
                int currentMouseOverRow = grid_testdata.HitTest(e.X, e.Y).RowIndex;
                if (currentMouseOverRow >= 0)
                {
                    //   m.MenuItems.Add(new MenuItem(string.Format("Do something to row {0}", currentMouseOverRow.ToString())));
                    //   grid_testcase.Rows.Add(currentMouseOverRow);
                }
                m.Show(grid_testdata, new Point(e.X, e.Y));
            }
        }
        private void MenuItem_Insert_Click(Object sender, System.EventArgs e)
        {
            this.grid_testdata.Rows.Insert(grid_testdata.CurrentCell.RowIndex);
        }
        private void MenuItem_Copy_Click(Object sender, System.EventArgs e)
        {               
            Clipboard.SetDataObject(grid_testdata.GetClipboardContent(), true);
        }
        private void MenuItem_Delete_Click(Object sender, System.EventArgs e)
        {
            var rows = this.grid_testdata.SelectedRows;
            if ((rows.Count > 0) &&
                (DialogResult.Yes == MessageBox.Show("Do you want to delete selected rows..?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)))
            {
                foreach (DataGridViewRow row in rows)
                {
                    if (!row.IsNewRow)
                    {                        
                        StepsToBeDeleted.Add(Convert.ToInt32(row.Cells["colActionFlowID"].Value));
                        this.grid_testdata.Rows.RemoveAt(row.Index);
                    }
                    else
                        MessageBox.Show("Default Row...Can't be DELETED.","Delete");
                }
            }
        }
        public  void MenuItem_Paste_Click(Object sender, System.EventArgs e)
        {              
            if (!string.IsNullOrEmpty(Clipboard.GetText()))
            {
                try
                {
                    char[] rowSplitter = { '\r', '\n' };
                    char[] columnSplitter = { '\t' };
                    string[] copiedrows = Clipboard.GetText().Split(rowSplitter, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var row in copiedrows)
                    {
                        string[] values = row.Split(columnSplitter);                        
                        if (!values.All(x => x == string.Empty))
                        {
                            //if (grid_testdata.CurrentRow==null )
                            //    grid_testdata.CurrentRow = 
                            grid_testdata.Rows.Insert(grid_testdata.CurrentRow.Index);
                            grid_testdata.Rows[grid_testdata.CurrentRow.Index - 1].Cells["cmb_execute"].Value = values[2];
                            grid_testdata.Rows[grid_testdata.CurrentRow.Index - 1].Cells["cmb_pagename"].Value = values[3];
                            grid_testdata.Rows[grid_testdata.CurrentRow.Index - 1].Cells["colFlowIdentifier"].Value = values[4];
                            grid_testdata.Rows[grid_testdata.CurrentRow.Index - 1].Cells["colDataIdentifier"].Value = values[5];
                            grid_testdata.Rows[grid_testdata.CurrentRow.Index - 1].Cells["cmb_indicator"].Value = values[6];
                            grid_testdata.Rows[grid_testdata.CurrentRow.Index - 1].Cells["cmb_label"].Value = values[7];
                            grid_testdata.Rows[grid_testdata.CurrentRow.Index - 1].Cells["colActionOrData"].Value = values[8];                            
                            grid_testdata.Rows[grid_testdata.CurrentRow.Index - 1].Cells["colSeqNumber"].Value = values[9];
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Invalid Data Selection");
                    grid_testdata.Rows.RemoveAt(grid_testdata.CurrentRow.Index - 1);
                }

            }
        }
        private void TestData_Load(object sender, EventArgs e)
        {
            grid_testdata.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);
        }
        private void grid_testdata_cellbeginedit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (grid_testdata.Rows[e.RowIndex].Cells["colActionFlowID"].Value != null)
            {
                grid_testdata.Rows[e.RowIndex].Cells["txtflag"].Value = "Yes";
                editflag.Add(grid_testdata.Rows[e.RowIndex].Cells["colActionFlowID"].Value.ToString());
            }
        }
        private void grid_testdata_cellendedit(object sender, DataGridViewCellEventArgs e)
        {
            if (grid_testdata.Rows[e.RowIndex].Cells["colActionFlowID"].Value != null)
            {
                grid_testdata.Rows[e.RowIndex].Cells["txtflag"].Value = "Yes";
                editflag.Add(grid_testdata.Rows[e.RowIndex].Cells["colActionFlowID"].Value.ToString());
            }
        }
        private void grid_testdata_cellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.ColumnIndex == 0) && (Convert.ToString(grid_testdata.Rows[e.RowIndex].Cells[0].Value) == "Del"))
            {
                DialogResult confirmDelete = MessageBox.Show("Delete Test Data Row # : "+(e.RowIndex+1), "Delete Test Data Row", MessageBoxButtons.YesNo);
                if (confirmDelete == DialogResult.Yes)
                {
                    if (grid_testdata.Rows[e.RowIndex].Cells["colActionFlowID"].Value != null)
                    {
                        deleteflag.Add(grid_testdata.Rows[e.RowIndex].Cells["colActionFlowID"].Value.ToString());
                    }
                    grid_testdata.Rows.RemoveAt(e.RowIndex);
                }
            }
            //else if ((e.ColumnIndex == 10) && (Convert.ToString(grid_testdata.Rows[e.RowIndex].Cells[10].Value) == "Edit"))
            //{
            //    EditTestData etd = new EditTestData();
            //    etd.cmb_execute.Text = grid_testdata.Rows[e.RowIndex].Cells["cmb_execute"].Value.ToString();
            //    etd.cmb_pagename.Text = grid_testdata.Rows[e.RowIndex].Cells["cmb_pagename"].Value.ToString();
            //    etd.cmb_label.Text = grid_testdata.Rows[e.RowIndex].Cells["cmb_label"].Value.ToString();
            //    etd.cmb_indicator.Text = grid_testdata.Rows[e.RowIndex].Cells["cmb_indicator"].Value.ToString();
            //    etd.txt_actionordata.Text = grid_testdata.Rows[e.RowIndex].Cells["colActionOrData"].Value.ToString();
            //    etd.txt_seqnumber.Text = grid_testdata.Rows[e.RowIndex].Cells["colSeqNumber"].Value.ToString();
            //    etd.txt_flowid.Text = grid_testdata.Rows[e.RowIndex].Cells["colFlowIdentifier"].Value.ToString();
            //    etd.txt_dataid.Text = grid_testdata.Rows[e.RowIndex].Cells["colDataIdentifier"].Value.ToString();
            //    etd.txt_testcaseid.Text = txt_testcaseid.Text.Trim().ToString();
            //    etd.txt_actionflowid.Text = grid_testdata.Rows[e.RowIndex].Cells["colActionFlowID"].Value.ToString();
            //    etd.ShowDialog();
            //}
        }
        private void grid_testdata_SelectionChanged(object sender, EventArgs e)
        {
            MessageBox.Show(grid_testdata.SelectedRows.Count.ToString());
        }
        private void btn_createtestdata_Click(object sender, EventArgs e)
        {
            string querytestdataInfo = string.Empty;
            try
            {
                int result1 = 0;
                if (objLib.GetTestCaseIDs().ContainsKey(txt_testcaseid.Text.Trim()))
                {
                    if (!(objLib.GetTestDataIDs().ContainsKey(txt_testcaseid.Text.Trim())))
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.AddRange(new DataColumn[16]
                        { 
                        new DataColumn("Actionflow_id", typeof(int)),
                        new DataColumn("TestCaseId", typeof(int)),
                        new DataColumn("PageId", typeof(int)),
                        new DataColumn("FlowIdentifier", typeof(int)),
                        new DataColumn("DataIdentifier", typeof(int)),
                        new DataColumn("MasterORID", typeof(int)),
                        new DataColumn("Indicator", typeof(int)),
                        new DataColumn("ActionORData", typeof(string)),
                        new DataColumn("SeqNumber", typeof(int)),
                        new DataColumn("Execute", typeof(string)),
                        new DataColumn("Projectid", typeof(int)),
                        new DataColumn("IsDeleted",typeof(string)),
                        new DataColumn("CreatedBy",typeof(string)),
                        new DataColumn("CreatedDate",typeof(string)),
                        new DataColumn("ModifiedBy",typeof(string)),
                        new DataColumn("ModifiedDate",typeof(string)),
                        });

                        for (int i = 0; i < grid_testdata.Rows.Count - 1; i++)
                        {

                            int Actionflowid = 0;
                            int TestCaseId = Int32.Parse(txt_testcaseid.Text);
                            int PageId = Int32.Parse(objLib.GetPageTitles().First(x => x.Value == grid_testdata.Rows[i].Cells["cmb_pagename"].Value.ToString()).Key);
                            int FlowIdentifier = Int32.Parse(grid_testdata.Rows[i].Cells["colFlowIdentifier"].Value.ToString());
                            int DataIdentifier = int.Parse(grid_testdata.Rows[i].Cells["colDataIdentifier"].Value.ToString());
                            int MasterORID = int.Parse(objLib.GetORLables(PageId).FirstOrDefault(x => x.Value == grid_testdata.Rows[i].Cells["cmb_label"].Value.ToString()).Key);
                            int Indicator = Int32.Parse(objLib.GetKeywords().First(x => x.Value == grid_testdata.Rows[i].Cells["cmb_indicator"].Value.ToString()).Key);
                            string ActionORData = grid_testdata.Rows[i].Cells["colActionOrData"].Value.ToString();
                            int SeqNumber = int.Parse(grid_testdata.Rows[i].Cells["colSeqNumber"].Value.ToString());
                            string Execute = (string.IsNullOrEmpty(Convert.ToString(grid_testdata.Rows[i].Cells["cmb_execute"].Value)) ? "Yes" : grid_testdata.Rows[i].Cells["cmb_execute"].Value.ToString());
                            int Projectid = SignIn.projectId;
                            string IsDeleted = DBNull.Value.ToString();
                            string CreatedBy = SignIn.userId;
                            string CreatedDate = "";
                            string ModifiedBy = "";
                            string ModifiedDate = "";

                            dt.Rows.Add(Actionflowid, TestCaseId, PageId, FlowIdentifier, DataIdentifier, MasterORID, Indicator, ActionORData, SeqNumber, Execute, Projectid, IsDeleted, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate);
                        }
                        if (dt.Rows.Count > 0)
                        {
                            dt.TableName = "TestData";
                            string result;
                            using (StringWriter sw = new StringWriter())
                            {
                                dt.WriteXml(sw);
                                result = sw.ToString();
                            }
                            string spName = "sp_InsertTestData";
                            result1 = objLib.ExecuteStoredProcedure(result, spName);
                        }
                       if (result1==-1)
                       {
                           MessageBox.Show("Test Data Created Successfully", "Create Test Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           this.Close();
                       }
                    }
                    else
                    {
                        MessageBox.Show("Test Data Already Exists for this Test Case ID : " + Int32.Parse(txt_testcaseid.Text.Trim()), "Create Test Data", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
                else
                {
                    MessageBox.Show("Test Case NOT Found with ID : " + Int32.Parse(txt_testcaseid.Text.Trim()), "Create Test Data", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR : " + ex.Message, "Create Test Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_ResetTestdataseq_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grid_testdata.Rows.Count - 1; i++)
            {
                grid_testdata.Rows[i].Cells["colSeqNumber"].Value = i + 1;
                if (grid_testdata.Rows[i].Cells["colActionFlowID"].Value != null)
                {
                    grid_testdata.Rows[i].Cells["txtflag"].Value = "Yes";
                    editflag.Add(grid_testdata.Rows[i].Cells["colActionFlowID"].Value.ToString());
                }
            }


            MessageBox.Show("Sequence Number Updated Successfully", "Test Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        
        private void btn_savetestdata_Click(object sender, EventArgs e)
        {
            int result1 = 0;
            foreach (var actionflowid in deleteflag)
            {
                objLib.RunQuery("DELETE FROM TESTDATA WHERE ACTIONFLOW_ID=" + actionflowid + " AND ProjectID=" + SignIn.projectId);
                result1 = 1;
            }
            foreach (var step in StepsToBeDeleted)
            {
                objLib.RunQuery("DELETE FROM TESTDATA WHERE ACTIONFLOW_ID=" + step + " AND PROJECTID=" + SignIn.projectId);
                result1 = 1;
            }
            string querytestdataInfo = string.Empty;
            var modifiedrowIndexes = (from r in grid_testdata.Rows.Cast<DataGridViewRow>()
                                      where (Convert.ToString(r.Cells["txtflag"].Value) == "Yes")
                         select r.Index).ToArray();

            var newrowIndexes = (from r in grid_testdata.Rows.Cast<DataGridViewRow>()
                                 where (r.Cells["colActionFlowID"].Value == null && Convert.ToString(r.Cells["btnClickMeforDelete"].Value) == "Del")
                                      select r.Index).ToArray();
            try
            {
                if (newrowIndexes.ToList().Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[16]
                        { 
                        new DataColumn("Actionflow_id", typeof(int)),
                        new DataColumn("TestCaseId", typeof(int)),
                        new DataColumn("PageId", typeof(int)),
                        new DataColumn("FlowIdentifier", typeof(int)),
                        new DataColumn("DataIdentifier", typeof(int)),
                        new DataColumn("MasterORID", typeof(int)),
                        new DataColumn("Indicator", typeof(int)),
                        new DataColumn("ActionORData", typeof(string)),
                        new DataColumn("SeqNumber", typeof(int)),
                        new DataColumn("Execute", typeof(string)),
                        new DataColumn("Projectid", typeof(int)),
                        new DataColumn("IsDeleted",typeof(string)),
                        new DataColumn("CreatedBy",typeof(string)),
                        new DataColumn("CreatedDate",typeof(string)),
                        new DataColumn("ModifiedBy",typeof(string)),
                        new DataColumn("ModifiedDate",typeof(string)),
                        });

                    foreach (var i in newrowIndexes)
                    {
                        int Actionflowid = 0;
                        int TestCaseId = Int32.Parse(txt_testcaseid.Text);
                        int PageId = Int32.Parse(objLib.GetPageTitles().First(x => x.Value == grid_testdata.Rows[i].Cells["cmb_pagename"].Value.ToString()).Key);
                        int FlowIdentifier = Int32.Parse(grid_testdata.Rows[i].Cells["colFlowIdentifier"].Value.ToString());
                        int DataIdentifier = int.Parse(grid_testdata.Rows[i].Cells["colDataIdentifier"].Value.ToString());
                        int MasterORID = int.Parse(objLib.GetORLables(PageId).FirstOrDefault(x => x.Value == grid_testdata.Rows[i].Cells["cmb_label"].Value.ToString()).Key);
                        int Indicator = Int32.Parse(objLib.GetKeywords().First(x => x.Value == grid_testdata.Rows[i].Cells["cmb_indicator"].Value.ToString()).Key);
                        string ActionORData = grid_testdata.Rows[i].Cells["colActionOrData"].Value.ToString();
                        int SeqNumber = int.Parse(grid_testdata.Rows[i].Cells["colSeqNumber"].Value.ToString());
                        string Execute = (string.IsNullOrEmpty(Convert.ToString(grid_testdata.Rows[i].Cells["cmb_execute"].Value)) ? "Yes" : grid_testdata.Rows[i].Cells["cmb_execute"].Value.ToString());
                        int Projectid = SignIn.projectId;
                        string IsDeleted = DBNull.Value.ToString();
                        string CreatedBy = SignIn.userId;
                        string CreatedDate = "";
                        string ModifiedBy = "";
                        string ModifiedDate = "";

                        dt.Rows.Add(Actionflowid, TestCaseId, PageId, FlowIdentifier, DataIdentifier, MasterORID, Indicator, ActionORData, SeqNumber, Execute, Projectid, IsDeleted, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.TableName = "TestData";
                        string result;
                        using (StringWriter sw = new StringWriter())
                        {
                            dt.WriteXml(sw);
                            result = sw.ToString();
                        }
                        string spName = "sp_InsertTestData";
                        result1 = objLib.ExecuteStoredProcedure(result, spName);
                    }
                }
                if (modifiedrowIndexes.ToList().Count > 0)
                {
                    DataTable dtUpdate = new DataTable();
                    dtUpdate.Columns.AddRange(new DataColumn[12]
                        { 
                        new DataColumn("Actionflow_id", typeof(int)),
                        new DataColumn("TestCaseId", typeof(int)),
                        new DataColumn("PageId", typeof(int)),
                        new DataColumn("FlowIdentifier", typeof(int)),
                        new DataColumn("DataIdentifier", typeof(int)),
                        new DataColumn("MasterORID", typeof(int)),
                        new DataColumn("Indicator", typeof(int)),
                        new DataColumn("ActionORData", typeof(string)),
                        new DataColumn("SeqNumber", typeof(int)),
                        new DataColumn("Execute", typeof(string)),
                        new DataColumn("ModifiedBy",typeof(string)),
                        new DataColumn("ModifiedDate",typeof(string)),
                        });
                    foreach (var i in modifiedrowIndexes)
                    {
                        int Actionflowid = Int32.Parse(grid_testdata.Rows[i].Cells["colActionFlowID"].Value.ToString());
                        int TestCaseId = Int32.Parse(txt_testcaseid.Text);
                        int PageId = Int32.Parse(objLib.GetPageTitles().First(x => x.Value == grid_testdata.Rows[i].Cells["cmb_pagename"].Value.ToString()).Key);
                        int FlowIdentifier = Int32.Parse(grid_testdata.Rows[i].Cells["colFlowIdentifier"].Value.ToString());
                        int DataIdentifier = int.Parse(grid_testdata.Rows[i].Cells["colDataIdentifier"].Value.ToString());
                        int MasterORID = int.Parse(objLib.GetORLables(PageId).FirstOrDefault(x => x.Value == grid_testdata.Rows[i].Cells["cmb_label"].Value.ToString()).Key);
                        int Indicator = Int32.Parse(objLib.GetKeywords().First(x => x.Value == grid_testdata.Rows[i].Cells["cmb_indicator"].Value.ToString()).Key);
                        string ActionORData = grid_testdata.Rows[i].Cells["colActionOrData"].Value.ToString();
                        int SeqNumber = int.Parse(grid_testdata.Rows[i].Cells["colSeqNumber"].Value.ToString());
                        string Execute = (string.IsNullOrEmpty(Convert.ToString(grid_testdata.Rows[i].Cells["cmb_execute"].Value)) ? "Yes" : grid_testdata.Rows[i].Cells["cmb_execute"].Value.ToString());
                        string ModifiedBy = SignIn.userId;
                        string ModifiedDate = "";
                        dtUpdate.Rows.Add(Actionflowid, TestCaseId, PageId, FlowIdentifier, DataIdentifier, MasterORID, Indicator, ActionORData, SeqNumber, Execute, ModifiedBy, ModifiedDate);
                    }

                    if (dtUpdate.Rows.Count > 0)
                    {
                        dtUpdate.TableName = "TestData";
                        string result;
                        using (StringWriter sw = new StringWriter())
                        {
                            dtUpdate.WriteXml(sw);
                            result = sw.ToString();
                        }

                        string spName = "sp_UpdateTestData";
                        result1 = objLib.ExecuteStoredProcedure(result, spName);
                        result1 = 2;
                    }
                }
                if (result1 == -1)
                {
                    MessageBox.Show("Test Data Created Successfully", "Create Test Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else if (result1 == 1)
                {
                    MessageBox.Show("Test Data Deleted Successfully", "Deleted Test Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else if (result1 == 2)
                {
                    MessageBox.Show("Test Data Updated  Successfully", "Updated Test Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch
            {
                MessageBox.Show("ERROR : Invalid Inputs", "Edit Test Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void grid_testdata_dataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            //MessageBox.Show("Error happened " + anError.Context.ToString());
        }
        private void grid_testdata_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (grid_testdata.CurrentCell.ColumnIndex == 2 && e.Control is ComboBox)
            {  
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox!=null)
                { 
                    comboBox.SelectedIndex = 0;
                }
            }
            else if (grid_testdata.CurrentCell.ColumnIndex == 3 && e.Control is ComboBox)
            {
                ComboBox comboBox = e.Control as ComboBox;
                comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox.SelectedIndexChanged += LastColumnComboSelectionChanged;
            }
            else if (grid_testdata.CurrentCell.ColumnIndex == 6 && e.Control is ComboBox)
            {
                ComboBox comboBox = e.Control as ComboBox;
                comboBox.DropDownStyle = ComboBoxStyle.DropDown;
            }
            else if (grid_testdata.CurrentCell.ColumnIndex == 7)
            {
                ComboBox comboBox = e.Control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                    comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                }
            }
        }
        private void LastColumnComboSelectionChanged(object sender, EventArgs e)
        {
            if (grid_testdata.CurrentCell.ColumnIndex == 3)
            {
                var currentcell = grid_testdata.CurrentCellAddress;
                var currentrow = grid_testdata.CurrentRow;
                var sendingCB = sender as DataGridViewComboBoxEditingControl;
                sendingCB.DropDown += new EventHandler(combo_DropDown);
                sendingCB.GotFocus += new EventHandler(combo_DropDown);
                DataGridViewComboBoxCell cel = (DataGridViewComboBoxCell)grid_testdata.Rows[currentcell.Y].Cells[3];
                cel.Value = sendingCB.EditingControlFormattedValue.ToString();
                var pageId = objLib.GetPageTitles().First(x => x.Value == cel.Value.ToString()).Key;
                var pageLabels = objLib.GetORLables(int.Parse(pageId));

                DataGridViewComboBoxCell labelCell=(DataGridViewComboBoxCell)grid_testdata.Rows[currentrow.Index].Cells["cmb_label"];
                labelCell.DataSource = null;
                labelCell.Items.Clear();
                labelCell.Items.Add("[Please Select a Label]");
                labelCell.Items.AddRange(pageLabels.Values.ToArray());
            }
        }
        void combo_DropDown(object sender, EventArgs e)
        {
            ((DataGridViewComboBoxEditingControl)sender).BackColor = Color.LightYellow;
        }
        private void txt_testcaseid_Leave(object sender, EventArgs e)
        {
            txt_testcasetitle.Text = "";
            txt_testcasesummary.Text = "";
            cmb_assignedto.SelectedItem = null;
            cmb_designedby.SelectedItem = null;
            cmb_designstatus.SelectedItem = null;
            cmb_testcategory.SelectedItem = null;
            try
            {
                if (!objLib.IsNullOrEmpty(txt_testcaseid.Text.Trim()))
                {
                    if (objLib.GetTestCaseIDs().ContainsKey(txt_testcaseid.Text.Trim()))
                    {
                        if (!(objLib.GetTestDataIDs().ContainsKey(txt_testcaseid.Text.Trim())))
                        {
                            DataTable dt = new DataTable();
                            dt = objLib.binddataTable("SELECT * FROM TESTCASEINFO WHERE TESTCASEID=" + txt_testcaseid.Text.Trim() + " AND PROJECTID=" + SignIn.projectId);

                            txt_testcaseid.Text = dt.Rows[0]["TestCaseID"].ToString();
                            txt_testcasetitle.Text = dt.Rows[0]["TestCaseTitle"].ToString();
                            txt_testcasesummary.Text = dt.Rows[0]["TestCaseSummary"].ToString();
                            txt_release.Text = dt.Rows[0]["Release"].ToString();
                            cmb_testcasefun.SelectedItem = dt.Rows[0]["Functionality"].ToString();
                            cmb_assignedto.SelectedItem = dt.Rows[0]["AssignedTo"].ToString();
                            cmb_designedby.SelectedItem = dt.Rows[0]["DesignedBy"].ToString();
                            cmb_designstatus.SelectedItem = dt.Rows[0]["State"].ToString();
                            cmb_testcategory.SelectedItem = dt.Rows[0]["TestCategory"].ToString();
                            btn_createtestdata.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("TestData Exists For Test Case ID: " + txt_testcaseid.Text.Trim() + "\n\nPlease Re-Check...", "Create Test Data", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            btn_createtestdata.Enabled = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Test Case NOT Exists With Test Case ID: " + txt_testcaseid.Text.Trim() + "\n\nPlease Re-Check...", "Create Test Data", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        btn_createtestdata.Enabled = false;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("ERROR : " + ex.Message);
            }                        
        }
        private void btn_search_Click(object sender, EventArgs e)
        {
            DataTable dtsearch = new DataTable(); 
            var filterClause=FilterGridViewData();
            string query=string.Empty;
            if(!string.IsNullOrEmpty(filterClause))
                query = "SELECT * FROM TESTDATAVIEW WHERE TESTCASEID=" + txt_testcaseid.Text.Trim() + " AND PROJECTID=" + SignIn.projectId + " AND "+filterClause+"  ORDER BY FLOWIDENTIFIER,DATAIDENTIFIER,SEQNUMBER ASC";                        
            else
                query = "SELECT * FROM TESTDATAVIEW WHERE TESTCASEID=" + txt_testcaseid.Text.Trim() + " AND PROJECTID=" + SignIn.projectId + " ORDER BY FLOWIDENTIFIER,DATAIDENTIFIER,SEQNUMBER ASC";                        
            dtsearch = objLib.binddataTable(query);
            Dictionary<string, string> labelColl = new Dictionary<string, string>();
            Dictionary<string, string> pageColl = new Dictionary<string, string>();
            pageColl = objLib.GetPageTitles();
            labelColl = objLib.GetLabels();
            grid_testdata.Rows.Clear();
            for (int i = 0; i < dtsearch.Rows.Count; i++)
            {
                grid_testdata.Rows.Add();
                var pageId = pageColl.First(x => x.Value == dtsearch.Rows[i]["PageName"].ToString()).Key;
                var pageLabels = labelColl.Where(x => x.Key.Split('_')[0].ToString() == pageId).Select(x => x.Value).ToArray();                  
                DataGridViewComboBoxCell labelCell = (DataGridViewComboBoxCell)(grid_testdata.Rows[i].Cells["cmb_label"]);
                labelCell.DataSource = null;
                labelCell.Items.Clear();
                //labelCell.Items.Add("“[Please Select a Label]");
                labelCell.Items.AddRange(pageLabels);
                grid_testdata.Rows[i].Cells["colActionFlowID"].Value = dtsearch.Rows[i]["ActionFlow_id"].ToString();
                grid_testdata.Rows[i].Cells["cmb_pagename"].Value = dtsearch.Rows[i]["PageName"].ToString();
                grid_testdata.Rows[i].Cells["colFlowIdentifier"].Value = dtsearch.Rows[i]["FlowIdentifier"].ToString();
                grid_testdata.Rows[i].Cells["colDataIdentifier"].Value = dtsearch.Rows[i]["DataIdentifier"].ToString();
                grid_testdata.Rows[i].Cells["cmb_indicator"].Value = dtsearch.Rows[i]["Keyword"].ToString();
                grid_testdata.Rows[i].Cells["cmb_label"].Value = dtsearch.Rows[i]["Label"].ToString();
                grid_testdata.Rows[i].Cells["colActionORData"].Value = dtsearch.Rows[i]["ActionORData"].ToString();
                grid_testdata.Rows[i].Cells["colSeqNumber"].Value = dtsearch.Rows[i]["SeqNumber"].ToString();
                grid_testdata.Rows[i].Cells["cmb_execute"].Value = dtsearch.Rows[i]["Execute"].ToString();
            }
            //grid_testdata.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //grid_testdata.Columns["colActionORData"].Width = 150;
        }
        private string FilterGridViewData()
        {
            List<string> query = new List<string>();
            string qString = string.Empty;
            for (int i = 1; i <= tlp_search.Controls.Count - 2; i = i + 2)
            {
                if (((CheckBox)tlp_search.Controls[i]).Checked)
                {
                    var colName = ((CheckBox)tlp_search.Controls[i]).Text;
                    var value = ((TextBox)tlp_search.Controls[i + 1]).Text;
                    if (colName.ToUpper().Contains("LABEL"))
                    {
                        string[] masterorids;
                        if (value.StartsWith("*"))
                            masterorids = objLib.GetORLables().Where(x=>x.Value.ToLower().Contains(value.Substring(1).ToLower())).Select(x=>x.Key).ToArray();                        
                        else
                            masterorids = objLib.GetORLables().Where(x => x.Value.ToLower()==value.ToLower()).Select(x => x.Key).ToArray();
                        if (masterorids.Length != 0)
                            query.Add("[MASTERORID] IN (" + string.Join(",", masterorids) + ")");
                        else
                        {
                            MessageBox.Show("Invalid Label", "Label");
                            query.Clear();
                            break;
                        }
                    }
                    else if (colName.ToUpper().Contains("PAGENAME"))
                    {
                        string[] pageids;
                        if (value.StartsWith("*"))
                        pageids = objLib.GetPageTitles().Where(x => x.Value.ToLower().Contains(value.Substring(1).ToLower())).Select(x => x.Key).ToArray();
                        else
                        pageids = objLib.GetPageTitles().Where(x => x.Value.ToLower()==value.ToLower()).Select(x => x.Key).ToArray();
                        if (pageids.Length != 0)
                            query.Add("[PAGEID] IN (" + string.Join(",", pageids) + ")");
                        else
                        {
                            MessageBox.Show("Invalid PageName", "PageName");
                            query.Clear();
                            break;
                        }                        
                    }
                    else if (colName.ToUpper().Contains("KEYWORD"))
                    {
                        string[] keyids;
                        if(value.StartsWith("*"))
                        keyids = objLib.GetKeywords().Where(x => x.Value.ToLower().Contains(value.Substring(1).ToLower())).Select(x => x.Key).ToArray();
                        else
                        keyids = objLib.GetKeywords().Where(x => x.Value.ToLower()==value.ToLower()).Select(x => x.Key).ToArray();
                        if (keyids.Length != 0)
                            query.Add("[INDICATOR] IN (" + string.Join(",", keyids) + ")");
                        else
                        {
                            MessageBox.Show("Invalid Keyword", "Keyword");
                            query.Clear();
                            break;
                        }                            
                    }
                    else
                    query.Add(value.StartsWith("*") ? (string.Format("[{0}] LIKE '%{1}%'", colName, string.IsNullOrEmpty(value) ? "" : value.Substring(1))) : (string.Format("[{0}]='{1}'", colName, string.IsNullOrEmpty(value) ? "" : value)));
                }
            }
            return (string.Join(" AND ", query));
        }        

    }
}
