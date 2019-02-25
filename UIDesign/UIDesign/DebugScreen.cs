using VM.Platform.TestAutomationFramework.Adapters.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace UIDesign
{
    public partial class DebugScreen : Form
    {
        //Testexecutin feature in debug
        private Label lbl_planned = new Label();
        private Label lbl_completed = new Label();
        private Label lbl_testlist = new Label();
        private Label lbl_where = new Label();
        private Label lbl_exestatus = new Label();
        private Label lbl_inprogress = new Label();

        private Label lbl_testcaseids = new Label();


        private Label lbl_searchby = new Label();
        private Label lbl_operator = new Label();
        private Label lbl_value = new Label();
        private Label lbl_errmsg = new Label();
        private Label lbl_exeMode = new Label();
        private Label lbl_groupby = new Label();


        List<Thread> threads = new List<Thread>();

        public ComboBox cmb_andor_1 = new ComboBox();
        public ComboBox cmb_andor_2 = new ComboBox();


        private TextBox txt_dummy = new TextBox();
        private TreeView trv_testcaseids = new TreeView();

        private ComboBox cmb_groupby = new ComboBox();
        private ComboBox cmb_Favorites = new ComboBox();

        public ComboBox cmb_condcol_0 = new ComboBox();
        public ComboBox cmb_operator_0 = new ComboBox();
        public TextBox txt_condval_0 = new TextBox();

        public ComboBox cmb_condcol_1 = new ComboBox();
        public ComboBox cmb_operator_1 = new ComboBox();
        public TextBox txt_condval_1 = new TextBox();

        public ComboBox cmb_condcol_2 = new ComboBox();
        public ComboBox cmb_operator_2 = new ComboBox();
        public TextBox txt_condval_2 = new TextBox();
        public Button btn_search1 = new Button();


        private CheckBox chk_condbox_0 = new CheckBox();
        private CheckBox chk_condbox_1 = new CheckBox();
        private CheckBox chk_condbox_2 = new CheckBox();


        private FlowLayoutPanel flp_testexecution = new FlowLayoutPanel();
        private TableLayoutPanel tlp_testexecution = new TableLayoutPanel();
        private TableLayoutPanel tlp_test = new TableLayoutPanel();

        public Dictionary<int, string> testcases = new Dictionary<int, string>();
        public Dictionary<int, string> testcasesummary = new Dictionary<int, string>();
        public Dictionary<int, string> functionality = new Dictionary<int, string>();
        public ListBox lst_testcaseorder = new ListBox();
        public DataGridView dgv_testcaseorder = new DataGridView();
        DataGridViewComboBoxColumn dgv_cmb_browser = new DataGridViewComboBoxColumn();
        string[] searchparameters = { "Assigned To", "Functionality", "Release", "Tags", "TestCaseID", "TestCategory", "Title", "Application", "Module", "ScrumTeamName", "RTMName" };

        TreeNode tree_node;
        public static bool flag = true;
        //test exectution feature in debug
        private Label lbl_browser = new Label();
        private Label lbl_uri = new Label();
        private Label lbl_url = new Label();
        private Label lbl_environment = new Label();
       // private Label lbl_application = new Label();
        private Label lbl_repositiory = new Label();
        private Label lbl_Blank = new Label();

        private ComboBox cmb_browser = new ComboBox();
       // private ComboBox cmb_application = new ComboBox();
        private ComboBox cmb_environment = new ComboBox();
        private ComboBox cmb_repositiory = new ComboBox();
        private Label lbl_TestData = new Label();
        private Label lbl_pagetitle = new Label();
        private Label lbl_TestCase = new Label();
        private TextBox txt_TestCase = new TextBox();
        private Button btn_Debug = new Button();
        private Button btn_Search = new Button();
        private Label lbl_Status = new Label();
        public DataGridView grid_testdata = new DataGridView();
        public DataGridViewCheckBoxColumn chk_selection1 = new DataGridViewCheckBoxColumn();
        Dictionary<int, string> dictTestCaseIDs = new Dictionary<int, string>();
        Dictionary<string, string> dict_Environments = new Dictionary<string, string>();
        Dictionary<string, string> dict_application = new Dictionary<string, string>();

        private PictureBox pic_gecko = new PictureBox();
        private BindingSource bindingSrc = new BindingSource();
        private FlowLayoutPanel flp_testcloner = new FlowLayoutPanel();
        private TableLayoutPanel tlp_testcloner = new TableLayoutPanel();
        FuncLib objLib = new FuncLib();
        UIScrapperCode uicode = new UIScrapperCode();
        public string testc_id;
        private DataTable dtglb;
        public string debugSeqNum;
        public bool debugFlag = false;
        TestExecution ExeObj = new TestExecution();

        public DebugScreen()
        {
            InitializeComponent();

            #region testdebug_design

            //Form Settings
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            this.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 100, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 100);
            this.AutoScroll = true;
            this.Text = "Debug Window";

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
            //same as execution
            //Flow Layout Panel Settings            
            flp_testexecution.FlowDirection = FlowDirection.LeftToRight;
            //flp_testexecution.SetFlowBreak(lbl_pagetitle, true);
            flp_testexecution.SetFlowBreak(tlp_test, true);
            flp_testexecution.Dock = DockStyle.Top;
            flp_testexecution.AutoSize = true;

            //Table Layout Panel Settings                                    
            tlp_testexecution.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_testexecution.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_testexecution.AutoSize = true;
            tlp_testexecution.Location = new Point(30, 80);

            //Table Layout Panel Settings                                    
            tlp_test.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_test.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            //tlp_test.BackColor = Color.LightCyan;
            tlp_test.AutoSize = true;
            //GroupBy
            cmb_groupby.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_groupby.Height = 30;
            cmb_groupby.Width = 250;
            //cmb_groupby.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_groupby.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_groupby.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_groupby.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_groupby.Items.AddRange(new[] { "Default View", "Functionality" });
            cmb_groupby.SelectedItem = "Default View";
            //SET Label settings            
            lbl_searchby.Text = "Search By ";
            lbl_searchby.Name = "lbl_searchby";
            lbl_searchby.TextAlign = ContentAlignment.BottomLeft;
            lbl_searchby.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_searchby.Height = 15;
            lbl_searchby.Width = 110;
            lbl_groupby.Text = "Group By";
            lbl_groupby.TextAlign = ContentAlignment.BottomRight;
            lbl_groupby.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_groupby.Height = 24;
            lbl_groupby.Width = 60;
            //testcaseids
            lbl_testcaseids.Text = "Test Cases :";
            lbl_testcaseids.Name = "lbl_testcaseids";
            lbl_testcaseids.TextAlign = ContentAlignment.BottomLeft;
            lbl_testcaseids.Font = new Font("Calibri", 10F, FontStyle.Bold);
            lbl_testcaseids.Height = 15;
            lbl_testcaseids.Width = 100;
            //SET Label settings            
            lbl_operator.Text = "Operator ";
            lbl_operator.Name = "lbl_operator";
            lbl_operator.TextAlign = ContentAlignment.BottomCenter;
            lbl_operator.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_operator.Height = 15;
            lbl_operator.Width = 60;

            //SET Value Label settings            
            lbl_value.Text = "Value ";
            lbl_value.Name = "lbl_value";
            lbl_value.TextAlign = ContentAlignment.BottomLeft;
            lbl_value.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_value.Height = 15;
            lbl_value.Width = 110;

            //ColumnNames combobox settings            
            cmb_condcol_0.Name = "cmb_condcol_0";
            cmb_condcol_0.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_condcol_0.Height = 24;
            cmb_condcol_0.Width = 250;
            cmb_condcol_0.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_condcol_0.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_condcol_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_condcol_0.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_condcol_0.Items.AddRange(searchparameters);

            //ColumnValues combobox settings            
            txt_condval_0.Name = "txt_condval_0";
            txt_condval_0.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_condval_0.Height = 24;
            txt_condval_0.Width = 250;

            //ColumnNames combobox settings            
            cmb_operator_0.Name = "cmb_operator_0";
            cmb_operator_0.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_operator_0.Height = 24;
            cmb_operator_0.Width = 60;
            cmb_operator_0.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_operator_0.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_operator_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_operator_0.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_operator_0.Items.AddRange(new string[] { "=", "!=", "<", ">", "<=", ">=", "In", "Contains" });

            //ColumnNames combobox settings            
            cmb_condcol_1.Name = "cmb_condcol_1";
            cmb_condcol_1.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_condcol_1.Height = 24;
            cmb_condcol_1.Width = 250;
            cmb_condcol_1.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_condcol_1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_condcol_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_condcol_1.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_condcol_1.Items.AddRange(searchparameters);

            //ColumnValues combobox settings            
            txt_condval_1.Name = "txt_condval_1";
            txt_condval_1.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_condval_1.Height = 24;
            txt_condval_1.Width = 250;

            //ColumnNames combobox settings            
            cmb_operator_1.Name = "cmb_operator_1";
            cmb_operator_1.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_operator_1.Height = 24;
            cmb_operator_1.Width = 60;
            cmb_operator_1.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_operator_1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            // cmb_operator_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_operator_1.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_operator_1.Items.AddRange(new string[] { "=", "!=", "<", ">", "<=", ">=", "In", "Contains" });

            //ColumnNames combobox settings            
            cmb_andor_1.Name = "cmb_andor_1";
            cmb_andor_1.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_andor_1.Height = 24;
            cmb_andor_1.Width = 60;
            cmb_andor_1.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_andor_1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_andor_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_andor_1.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_andor_1.Items.AddRange(new string[] { "And", "Or" });

            //ColumnNames combobox settings            
            cmb_andor_2.Name = "cmb_andor_2";
            cmb_andor_2.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_andor_2.Height = 24;
            cmb_andor_2.Width = 60;
            cmb_andor_2.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_andor_2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_andor_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_andor_2.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_andor_2.Items.AddRange(new string[] { "And", "Or" });

            //ColumnNames combobox settings            
            cmb_condcol_2.Name = "cmb_condcol_2";
            cmb_condcol_2.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_condcol_2.Height = 24;
            cmb_condcol_2.Width = 250;
            cmb_condcol_2.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_condcol_2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_condcol_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_condcol_2.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_condcol_2.Items.AddRange(searchparameters);

            //ColumnValues combobox settings            
            txt_condval_2.Name = "txt_condval_2";
            txt_condval_2.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_condval_2.Height = 24;
            txt_condval_2.Width = 250;

            //ColumnNames combobox settings            
            cmb_operator_2.Name = "cmb_operator_2";
            cmb_operator_2.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_operator_2.Height = 24;
            cmb_operator_2.Width = 60;
            cmb_operator_2.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_operator_2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_operator_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_operator_2.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_operator_2.Items.AddRange(new string[] { "=", "!=", "<", ">", "<=", ">=", "In", "Contains" });

            //Update button settings            
            btn_search1.Text = "Search";
            btn_search1.Name = "btn_search1";
            btn_search1.Font = new Font("Calibri", 10F, FontStyle.Regular);
            btn_search1.Height = 24;
            btn_search1.Width = 90;
            btn_search1.BackColor = SystemColors.Window;

            //CheckBox textbox settings                        
            chk_condbox_0.Name = "chk_condbox_0";
            chk_condbox_0.Height = 24;
            chk_condbox_0.Width = 20;

            //CheckBox textbox settings                        
            chk_condbox_1.Name = "chk_condbox_1";
            chk_condbox_1.Height = 24;
            chk_condbox_1.Width = 20;

            //CheckBox textbox settings                        
            chk_condbox_2.Name = "chk_condbox_2";
            chk_condbox_2.Height = 24;
            chk_condbox_2.Width = 20;
            //Tree View Settings
            trv_testcaseids.Name = "trv_testcaseids";
            trv_testcaseids.Font = new Font("Calibri", 11F, FontStyle.Regular);
            trv_testcaseids.CheckBoxes = true;
            trv_testcaseids.Height = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 550;
            trv_testcaseids.Width = 600;

            tlp_test.Controls.Add(lbl_pagetitle, 1, 1);
            tlp_test.SetColumnSpan(lbl_pagetitle, 8);
            tlp_test.Controls.Add(lbl_searchby, 2, 2);
            tlp_test.Controls.Add(lbl_operator, 3, 2);
            tlp_test.Controls.Add(lbl_value, 4, 2);
            tlp_test.Controls.Add(chk_condbox_0, 0, 3);
            tlp_test.Controls.Add(lbl_where, 1, 3);
            tlp_test.Controls.Add(cmb_condcol_0, 2, 3);
            tlp_test.Controls.Add(cmb_operator_0, 3, 3);
            tlp_test.Controls.Add(txt_condval_0, 4, 3);
            tlp_test.Controls.Add(chk_condbox_1, 0, 4);
            tlp_test.Controls.Add(cmb_andor_1, 1, 4);
            tlp_test.Controls.Add(cmb_condcol_1, 2, 4);
            tlp_test.Controls.Add(cmb_operator_1, 3, 4);
            tlp_test.Controls.Add(txt_condval_1, 4, 4);
            tlp_test.Controls.Add(chk_condbox_2, 0, 5);
            tlp_test.Controls.Add(cmb_andor_2, 1, 5);
            tlp_test.Controls.Add(cmb_condcol_2, 2, 5);
            tlp_test.Controls.Add(cmb_operator_2, 3, 5);
            tlp_test.Controls.Add(txt_condval_2, 4, 5);
            tlp_test.Controls.Add(lbl_groupby, 1, 6);
            //tlp_test.SetColumnSpan(lbl_groupby, 2);
            tlp_test.Controls.Add(cmb_groupby, 2, 6);
            // tlp_test.SetColumnSpan(cmb_groupby, 3);
            tlp_test.Controls.Add(lbl_Blank, 1, 8);
            tlp_test.Controls.Add(btn_search1, 2, 8);
            tlp_test.SetColumnSpan(btn_search1, 2);
            tlp_test.Controls.Add(lbl_errmsg, 3, 7);

            tlp_test.SetColumnSpan(lbl_errmsg, 2);


            tlp_test.Controls.Add(lbl_Blank, 9, 3);
            tlp_test.Controls.Add(lbl_browser, 10, 3);
            tlp_test.Controls.Add(cmb_browser, 11, 3);

           // tlp_test.Controls.Add(lbl_application, 10, 4);
           // tlp_test.Controls.Add(cmb_application, 11, 4);
            tlp_test.Controls.Add(lbl_environment, 10, 5);
            tlp_test.Controls.Add(cmb_environment, 11, 5);
            tlp_test.Controls.Add(lbl_url, 10, 6);
            tlp_test.Controls.Add(lbl_uri, 11, 6);
            tlp_test.Controls.Add(lbl_repositiory, 10, 7);
            tlp_test.Controls.Add(cmb_repositiory, 11, 7);
            tlp_test.Controls.Add(lbl_Blank, 10, 8);
            tlp_test.Controls.Add(btn_Debug, 11, 8);

            tlp_testexecution.Controls.Add(lbl_testcaseids, 0, 1);
            tlp_testexecution.Controls.Add(trv_testcaseids, 0, 2);
            tlp_testexecution.SetRowSpan(trv_testcaseids, 2);
            tlp_testexecution.Controls.Add(lbl_Status, 1, 12);
            tlp_testexecution.SetColumnSpan(lbl_Status, 10);

            tlp_testexecution.Controls.Add(lbl_TestData, 10, 1);
            tlp_testexecution.Controls.Add(grid_testdata, 10, 2);


            flp_testexecution.Controls.AddRange(new Control[] { tlp_test, tlp_testexecution });
            this.Controls.AddRange(new Control[] { flp_testexecution });

            //same as execution

            //Gecko Picture Box Settings            
            pic_gecko.Height = 130;
            pic_gecko.Width = 175;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;




            lbl_TestCase.Text = "TestCase ID : ";
            lbl_TestCase.Name = "lbl_TestCase";
            lbl_TestCase.TextAlign = ContentAlignment.MiddleLeft;
            lbl_TestCase.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_TestCase.Height = 20;
            lbl_TestCase.Width = 200;

            //Clone To textbox settings            
            txt_TestCase.Text = "";
            txt_TestCase.Name = "txt_TestCase";
            txt_TestCase.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_TestCase.Height = 30;
            txt_TestCase.Width = 150;
            txt_TestCase.ReadOnly = false;


            //Clone button settings            
            btn_Debug.Text = "Debug";
            btn_Debug.Name = "btn_Debug";
            btn_Debug.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_Debug.Height = 30;
            btn_Debug.Width = 100;
            btn_Debug.Enabled = false;

            //Clone button settings            
            btn_Search.Text = "Search";
            btn_Search.Name = "btn_Search";
            btn_Search.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_Search.Height = 30;
            btn_Search.Width = 100;


            grid_testdata.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 700, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 550);
            grid_testdata.DefaultCellStyle.Font = new Font("Calibri", 10);
            grid_testdata.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
            grid_testdata.ScrollBars = ScrollBars.Both;
            grid_testdata.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8F, FontStyle.Bold);
            grid_testdata.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            grid_testdata.AllowUserToAddRows = false;

            chk_selection1.Width = 30;
            chk_selection1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid_testdata.Columns.Insert(0, chk_selection1);



            //Inprogress Label settings                        
            lbl_Status.Name = "lbl_Status";
            lbl_Status.TextAlign = ContentAlignment.BottomLeft;
            lbl_Status.Font = new Font("Calibri", 10F, FontStyle.Bold);
            lbl_Status.ForeColor = Color.Red;
            lbl_Status.Height = 20;
            lbl_Status.Width = 1200;
            lbl_Status.AutoSize = true;

            lbl_Blank.Name = "lbl_Blank";
            lbl_Blank.TextAlign = ContentAlignment.BottomLeft;
            lbl_Blank.Font = new Font("Calibri", 10F, FontStyle.Bold);
            lbl_Blank.ForeColor = Color.Red;
            lbl_Blank.Height = 20;
            lbl_Blank.Width = 100;
            //lbl_Blank.AutoSize = true;
            lbl_Blank.Text = "";

            lbl_TestData.Text = "Test Data (Select step to insert Breakpoint) : ";
            lbl_TestData.Name = "lbl_TestData";
            lbl_TestData.TextAlign = ContentAlignment.BottomLeft;
            lbl_TestData.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_TestData.Height = 20;
            lbl_TestData.Width = 400;



            //Userid Label settings           
            lbl_pagetitle.Height = 50;
            lbl_pagetitle.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 4 - 150;
            lbl_pagetitle.Text = "DEBUG WINDOW";
            lbl_pagetitle.Name = "lbl_pagetitle";
            lbl_pagetitle.TextAlign = ContentAlignment.TopLeft;
            lbl_pagetitle.Font = new Font("Calibri", 20F, FontStyle.Bold);
            lbl_pagetitle.ForeColor = Color.Blue;

            //Browser Label settings            
            lbl_browser.Text = "Browser Type :*";
            lbl_browser.Name = "lbl_browser";
            lbl_browser.TextAlign = ContentAlignment.BottomLeft;
            lbl_browser.Font = new Font("Calibri", 11F, FontStyle.Regular);
            lbl_browser.Height = 20;
            lbl_browser.Width = 150;


            lbl_url.Text = "URL :*";
            lbl_url.TextAlign = ContentAlignment.BottomLeft;
            lbl_url.Font = new Font("Calibri", 11F, FontStyle.Regular);
            lbl_url.Height = 20;
            lbl_url.Width = 150;
            //Environment Value Label settings            
            lbl_environment.Text = "Environment :*";
            lbl_environment.Name = "lbl_environment";
            lbl_environment.TextAlign = ContentAlignment.BottomLeft;
            lbl_environment.Font = new Font("Calibri", 11F, FontStyle.Regular);
            lbl_environment.Height = 20;
            lbl_environment.Width = 150;


            ////Environment Value Label settings            
            //lbl_application.Text = "Application :*";
            //lbl_application.Name = "lbl_application";
            //lbl_application.TextAlign = ContentAlignment.BottomLeft;
            //lbl_application.Font = new Font("Calibri", 11F, FontStyle.Regular);
            //lbl_application.Height = 20;
            //lbl_application.Width = 200;

            //Environment Value Label settings            
            lbl_repositiory.Text = "Repository :*";
            lbl_repositiory.Name = "lbl_repositiory";
            lbl_repositiory.TextAlign = ContentAlignment.BottomLeft;
            lbl_repositiory.Font = new Font("Calibri", 11F, FontStyle.Regular);
            lbl_repositiory.Height = 20;
            lbl_repositiory.Width = 150;

            //URI Label settings                        
            lbl_uri.Name = "lbl_uri";
            lbl_uri.ForeColor = Color.Blue;
            lbl_uri.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_uri.Width = 500;
            lbl_uri.Height = 30;
            lbl_uri.TextAlign = ContentAlignment.MiddleLeft;
            lbl_uri.Text = "";

            //ColumnNames combobox settings            
            cmb_browser.Name = "cmb_browser";
            cmb_browser.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_browser.Height = 30;
            cmb_browser.Width = 250;
            cmb_browser.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_browser.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_browser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_browser.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_browser.Items.AddRange(new[] { "Chrome", "Ie" });
            //ColumnNames combobox settings            
            //cmb_application.Name = "cmb_application";
            //cmb_application.Font = new Font("Calibri", 11F, FontStyle.Regular);
            //cmb_application.Height = 30;
            //cmb_application.Width = 250;
            ////cmb_application.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            //cmb_application.DropDownStyle = ComboBoxStyle.DropDownList;
            //cmb_application.AutoCompleteSource = AutoCompleteSource.ListItems;
            //cmb_application.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //ColumnNames combobox settings            
            cmb_environment.Name = "cmb_environment";
            cmb_environment.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_environment.Height = 30;
            cmb_environment.Width = 250;
            // cmb_environment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_environment.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_environment.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_environment.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //ColumnNames combobox settings            
            cmb_repositiory.Name = "cmb_repositiory";
            cmb_repositiory.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_repositiory.Height = 30;
            cmb_repositiory.Width = 250;
            //cmb_repositiory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_repositiory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_repositiory.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_repositiory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;


            //Adding Controls to Flow Layout Panel
            flp_testcloner.Controls.AddRange(new Control[] { pic_gecko, tlp_testcloner, flp_testexecution });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_testcloner });
            this.Load += new System.EventHandler(DebugScreen_Load);

            #endregion

            #region testcloner_methods
            //cmb_application.SelectedIndexChanged += new System.EventHandler(cmb_application_selectionchanged);
            cmb_environment.SelectedIndexChanged += new System.EventHandler(cmb_environment_selectionchanged);
            // btn_Search.Click += new System.EventHandler(btn_Search_Click);
            trv_testcaseids.AfterCheck += new TreeViewEventHandler(treeView_AfterCheck);

            grid_testdata.CellContentClick += new DataGridViewCellEventHandler(grid_testdata_CellContentClick);
            btn_Debug.Click += new System.EventHandler(btn_Debug_Click);
            btn_search1.Click += new System.EventHandler(btn_search1_click);

            #endregion
        }


        private string buildQuery()
        {
            try
            {
                string updateQuery = string.Empty;
                flag = true;

                if (chk_condbox_0.Checked)
                {

                    updateQuery = "SELECT TESTCASEID,TESTCASETITLE,TESTCASESUMMARY,FUNCTIONALITY FROM TESTCASEINFO WHERE ISDELETED IS NULL AND PROJECTID= " + SignIn.projectId;
                    string condClause_1 = buildClause(Convert.ToString(cmb_condcol_0.SelectedItem), Convert.ToString(cmb_operator_0.SelectedItem), txt_condval_0.Text);
                    if (!objLib.IsNullOrEmpty(condClause_1))
                    {

                        updateQuery = updateQuery + " AND (" + condClause_1;
                        if (chk_condbox_1.Checked)
                        {
                            if (!objLib.IsNullOrEmpty(Convert.ToString(cmb_andor_1.SelectedItem)))
                            {
                                string condClause_2 = buildClause(Convert.ToString(cmb_condcol_1.SelectedItem), Convert.ToString(cmb_operator_1.SelectedItem), txt_condval_1.Text);
                                if (!objLib.IsNullOrEmpty(condClause_2))
                                {
                                    updateQuery = updateQuery + " " + Convert.ToString(cmb_andor_1.SelectedItem) + " " + condClause_2;
                                    if (chk_condbox_2.Checked)
                                    {
                                        if (!objLib.IsNullOrEmpty(Convert.ToString(cmb_andor_2.SelectedItem)))
                                        {
                                            string condClause_3 = buildClause(Convert.ToString(cmb_condcol_2.SelectedItem), Convert.ToString(cmb_operator_2.SelectedItem), txt_condval_2.Text);
                                            if (!objLib.IsNullOrEmpty(condClause_3))
                                            {
                                                updateQuery = updateQuery + " " + Convert.ToString(cmb_andor_2.SelectedItem) + " " + condClause_3;
                                            }
                                            else
                                            {
                                                flag = false;
                                                lbl_errmsg.Visible = true;
                                                lbl_errmsg.Text = "Invalid Inputs..at Condition # : 3";
                                            }
                                        }
                                        else
                                        {
                                            flag = false;
                                            lbl_errmsg.Visible = true;
                                            lbl_errmsg.Text = "Invalid Inputs...And/Or at Condition # : 3";
                                        }

                                    }
                                }
                                else
                                {
                                    flag = false;
                                    lbl_errmsg.Visible = true;
                                    lbl_errmsg.Text = "Invalid Inputs..at Condition # : 2";
                                }
                            }
                            else
                            {
                                flag = false;
                                lbl_errmsg.Visible = true;
                                lbl_errmsg.Text = "Invalid Inputs...And/Or at Condition # : 2";
                            }
                        }
                    }

                    else
                    {
                        flag = false;
                        lbl_errmsg.Visible = true;
                        lbl_errmsg.Text = "Invalid Inputs..at Condition # : 1";
                    }
                }
                if (updateQuery != "")
                    return updateQuery + ") ORDER BY TESTCASEID ASC";
                else
                    return "";
                // return updateQuery + "AND ISDELETED IS NULL ORDER BY TESTCASEID ASC";
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private void btn_search1_click(object sender, EventArgs e)
        {
            try
            {

                lbl_errmsg.Visible = false;
                string executeQuery;


                executeQuery = buildQuery();

                if ((flag) && (!objLib.IsNullOrEmpty(executeQuery)))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        dt = objLib.binddataTable(executeQuery);
                        trv_testcaseids.Nodes.Clear();
                        testcases.Clear();
                        testcasesummary.Clear();
                        functionality.Clear();
                        if (dt.Rows.Count != 0)
                        {
                            if (Convert.ToString(cmb_groupby.SelectedItem) == "Default View")
                            {
                                TreeNode trNode = new TreeNode(SignIn.projectName);
                                trNode.Name = SignIn.projectName;
                                trv_testcaseids.Nodes.Add(trNode);
                                foreach (DataRow item in dt.Rows)
                                {
                                    string nodeText = item[0].ToString() + String.Concat(Enumerable.Repeat(" ", 7 - (item[0].ToString().Length))) + " : " + item[1].ToString();
                                    trNode.Nodes.Add(nodeText, nodeText);
                                    //trv_testcaseids.Nodes.Add(new TreeNode() { Text = item[0].ToString()});
                                    testcases.Add(Int32.Parse(item[0].ToString()), item[1].ToString());
                                    testcasesummary.Add(Int32.Parse(item[0].ToString()), item[2].ToString());
                                    functionality.Add(Int32.Parse(item[0].ToString()), item[3].ToString());
                                }
                            }



                            else if (Convert.ToString(cmb_groupby.SelectedItem) == "Functionality")
                            {
                                trv_testcaseids.Nodes.Clear();
                                foreach (DataRow r in dt.Rows)
                                {
                                    TreeNode trNode = new TreeNode(r["Functionality"].ToString());
                                    trNode.Name = r["Functionality"].ToString();
                                    if (!trv_testcaseids.Nodes.ContainsKey(trNode.Name))
                                        trv_testcaseids.Nodes.Add(trNode);

                                    var rows = from row in dt.AsEnumerable()
                                               where row.Field<string>("Functionality").Trim() == trNode.Name
                                               select row;
                                    foreach (var rw in rows)
                                    {
                                        string nodeText = rw["TestCaseID"].ToString() + String.Concat(Enumerable.Repeat(" ", 7 - (rw["TestCaseID"].ToString().Length))) + " : " + rw["TestCaseTitle"].ToString();
                                        trNode.Nodes.Add(nodeText, nodeText);
                                        //trNode.Nodes.Add(new TreeNode(rw["TestCaseID"].ToString() + String.Concat(Enumerable.Repeat(" ", 7 - (rw["TestCaseID"].ToString().Length))) + " : " + rw["TestCaseTitle"].ToString()));
                                    }
                                }
                            }

                        }
                        else
                        {
                            lbl_errmsg.Visible = true;
                            lbl_errmsg.Text = "NO DATA FOUND";
                        }


                    }
                    catch (Exception exce)
                    {
                        throw exce;
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Search Criteria");
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("ERROR : " + exc.Message, "Test Debug");
            }
        }
        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                // e.Node.TreeView.BeginUpdate();
                string selectedNodeText = null;
                var parentNode = e.Node;
                var nodes = e.Node.Nodes;
                //if parent checked
                if (e.Node.Nodes.Count > 0)
                {
                    if (parentNode.Checked && e.Node.Nodes.Count > 1)
                    {
                        MessageBox.Show("Please expand treeview to select any ONE testcase for debug.", "Debug Window");
                    }
                    else
                    {
                        CheckedOrUnCheckedNodes(parentNode, nodes);
                        displayGridEntries(e.Node);


                    }
                }

                else
                {
                    if (e.Node.Checked)
                    {

                        int nodeIndex = e.Node.Index;

                        foreach (TreeNode tn in e.Node.Parent.Nodes)
                        {

                            if (tn.Checked == true && tn.Index != nodeIndex)
                                tn.Checked = false;
                        }
                    }
                    displayGridEntries(e.Node);
                }




            }
            catch
            {

            }
            finally
            {
                lbl_testlist.Text = "Test Order (Test Case Count: " + dgv_testcaseorder.Rows.Count.ToString() + ")";
                e.Node.TreeView.EndUpdate();
            }

            //try
            //{
            //    e.Node.TreeView.BeginUpdate();


            //    if (e.Node.Nodes.Count > 0)
            //    {
            //        var parentNode = e.Node;
            //        var nodes = e.Node.Nodes;
            //        CheckedOrUnCheckedNodes(parentNode, nodes);
            //    }
            //    else
            //    {
            //        if ((e.Node.Checked) && (!lst_testcaseorder.Items.Contains(e.Node.Text)))
            //        {
            //            lst_testcaseorder.Items.Add(e.Node.Text);
            //            int idx = dgv_testcaseorder.Rows.Add();
            //            dgv_testcaseorder.Rows[idx].Cells["colTestCaseTitle"].Value = e.Node.Text;
            //            if (!Convert.ToString(cmb_browser.SelectedItem).Equals(""))
            //                this.dgv_testcaseorder.Rows[idx].Cells["cmb_browser"].Value = cmb_browser.SelectedItem.ToString();
            //        }
            //        else if ((!e.Node.Checked) && (lst_testcaseorder.Items.Contains(e.Node.Text)))
            //        {
            //            lst_testcaseorder.Items.Remove(e.Node.Text);
            //            int rowIndex = -1;

            //            DataGridViewRow row = dgv_testcaseorder.Rows
            //                .Cast<DataGridViewRow>()
            //                .Where(r => r.Cells["colTestCaseTitle"].Value.ToString().Equals(e.Node.Text))
            //                .First();

            //            rowIndex = row.Index;

            //            dgv_testcaseorder.Rows.RemoveAt(rowIndex);

            //        }

            //    }
            //}
            //finally
            //{
            //    lbl_testlist.Text = "Test Order (Test Case Count: " + dgv_testcaseorder.Rows.Count.ToString() + ")";
            //    e.Node.TreeView.EndUpdate();
            //}
        }

        private void displayGridEntries(TreeNode item)
        {
            if (!item.Text.Equals("CLAIMS"))
            {
                testc_id = item.Text.Split(':')[0].Trim();
                btn_Debug.Enabled = false;
                ExeObj.testcases.Clear();
                ExeObj.testcasesummary.Clear();
                ExeObj.functionality.Clear();
                string[] testCaseDetails = null;
                if (!objLib.IsNullOrEmpty(testc_id))
                {
                    string strQuery = "SELECT TESTCASETITLE,TESTCASESUMMARY,FUNCTIONALITY FROM testCaseInfo WHERE ISDELETED IS NULL AND TestCaseID = " + Int32.Parse(testc_id) + " AND Projectid = " + SignIn.projectId;
                    using (SqlConnection con = new SqlConnection(GetDBConnectionString()))
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand(strQuery, con))
                        {
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                testCaseDetails = new string[] { reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString() };

                            }
                        }
                    }
                    ExeObj.testcases.Add(Int32.Parse(testc_id), testCaseDetails[0]);
                    ExeObj.testcasesummary.Add(Int32.Parse(testc_id), testCaseDetails[1]);
                    ExeObj.functionality.Add(Int32.Parse(testc_id), testCaseDetails[2]);

                    //poupulate testdata grid
                    string testDataQuery = "select P.PageName,T.FlowIdentifier,T.DataIdentifier,A.ActionName as Indicator, O.Label, T.ActionORData, T.SeqNumber from TestData T, MasterOR O,PageNames P, ActionKeyword A where TestCaseID = " + Int32.Parse(testc_id) + " and T.PageID = P.PageID and T.[Execute]='Yes' and O.MasterORID = T.MasterORID and T.Indicator= A.ActionKeyword_ID  order by T.SeqNumber Asc";

                    //string testDataQuery = "select P.PageName,T.FlowIdentifier,T.DataIdentifier,O.Label, T.SeqNumber, T.ActionORData from TestData T,MasterOR O,PageNames P where TestCaseID = " + txt_TestCase.Text.ToString().Trim() + " and T.PageID = P.PageID and O.MasterORID = T.MasterORID order by T.SeqNumber Asc";
                    DataTable dt = new DataTable();
                    dt = objLib.binddataTable(testDataQuery);
                    grid_testdata.DataSource = dt;
                    grid_testdata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    grid_testdata.Columns[0].Width = 30;
                    grid_testdata.Visible = true;
                }
            }
        }

        private void CheckedOrUnCheckedNodes(TreeNode parentNode, TreeNodeCollection nodes)
        {
            if (nodes.Count > 0)
            {
                foreach (TreeNode node in nodes)
                {
                    node.Checked = parentNode.Checked;
                    CheckedOrUnCheckedNodes(parentNode, node.Nodes);
                }
            }
        }
        private string buildClause(string columnName, string operatorValue, string columnValue)
        {
            string clause = string.Empty;
            string replacedclause = string.Empty;
            try
            {
                if ((!objLib.IsNullOrEmpty(columnName)) && (!objLib.IsNullOrEmpty(operatorValue)) && (!objLib.IsNullOrEmpty(columnValue)))
                {
                    clause = columnName + operatorValue + "'" + columnValue + "'";
                    if (objLib.IsEqual(columnName, "TestCategory"))
                    {
                        columnName = "TestCategory";
                        if (columnValue.Contains(','))
                        {
                            var categories = columnValue.Split(',');
                            foreach (var item in categories)
                            {
                                try
                                {
                                    var category = objLib.GetTestCategories().First(x => x.Value == item.ToUpper()).Key;
                                    columnValue = columnValue.Replace(item, "'" + category + "'");
                                }
                                catch
                                {
                                    throw new Exception("Invalid Test Category : " + item);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                if (string.Compare(operatorValue, "Contains", StringComparison.OrdinalIgnoreCase) != 0)
                                    columnValue = objLib.GetTestCategories().First(x => x.Value == columnValue.ToUpper()).Key;
                                else if (!objLib.GetTestCategories().Values.Any(x => x.ToLower().Contains(columnValue.ToLower())))
                                    throw new Exception("Invalid Test Category : " + columnValue);
                            }

                            catch
                            {
                                throw new Exception("Invalid Test Category : " + columnValue);
                            }
                        }
                    }
                    else if (objLib.IsEqual(columnName, "Tags"))
                    {
                        columnName = "Tags";
                        var tctags = objLib.GetTags("SELECT TESTCASEID,TAGS FROM TESTCASEINFO WHERE PROJECTID=" + SignIn.projectId).Values.ToArray();
                        var alltags = string.Join(",", (tctags.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToArray())).Split(',');
                        if (columnValue.Contains(','))
                        {
                            var tags = columnValue.Split(',');
                            foreach (var item in tags)
                            {
                                try
                                {
                                    //var pagetags = objLib.GetTags("SELECT PAGEID,TAGS FROM PAGENAMES WHERE PROJECTID=" + SignIn.projectId).Values.ToArray();
                                    //var alltags = string.Join(",", pagetags);
                                    if (alltags.Any(x => x.ToString().ToLower().Equals(item.ToLower())))
                                    {
                                        columnValue = columnValue.Replace(item, "'" + item + "'");
                                    }
                                    else
                                    {
                                        throw new Exception("Invalid Tag : " + item);
                                    }
                                }
                                catch
                                {
                                    throw new Exception("Invalid Tag : " + item);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                if (string.Compare(operatorValue, "Contains", StringComparison.OrdinalIgnoreCase) != 0)
                                {
                                    if (alltags.Any(x => x.ToString().ToLower().Equals(columnValue.ToLower())))
                                    {
                                        columnValue = columnValue.Replace(columnValue, columnValue);
                                        //columnValue = columnValue.Replace(columnValue, "'" + columnValue + "'");
                                    }
                                }
                                else if (!alltags.Any(x => x.ToString().ToLower().Contains(columnValue.ToLower())))
                                {
                                    throw new Exception("Invalid Tag : " + columnValue);
                                }
                            }
                            catch
                            {
                                throw new Exception("Invalid Tag : " + columnValue);
                            }
                        }
                    }

                    else if (objLib.IsEqual(columnName, "Assigned To"))
                    {
                        columnName = "AssignedTo";
                        if (columnValue.Contains(','))
                        {
                            var users = columnValue.Split(',');
                            foreach (var item in users)
                            {
                                try
                                {
                                    var user = objLib.GetUserInfo().First(x => x.Value == item.ToUpper()).Key;
                                    columnValue = columnValue.Replace(item, "'" + item + "'");
                                }
                                catch
                                {
                                    throw new Exception("Invalid User Name : " + item);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                if (string.Compare(operatorValue, "Contains", StringComparison.OrdinalIgnoreCase) != 0)
                                    columnValue = objLib.GetUserInfo().First(x => x.Value == columnValue.ToUpper()).Value;
                                else if (!objLib.GetUserInfo().Values.Any(x => x.ToLower().Contains(columnValue.ToLower())))
                                    throw new Exception("Invalid User Name : " + columnValue);
                            }

                            catch
                            {
                                throw new Exception("Invalid User Name : " + columnValue);
                            }
                        }
                    }
                    else if (objLib.IsEqual(columnName, "Title"))
                    {
                        columnName = "TestCaseTitle";
                        if (columnValue.Contains(','))
                        {
                            var titles = columnValue.Split(',');
                            foreach (var item in titles)
                            {
                                try
                                {
                                    var title = objLib.GetTestCaseTitles().First(x => x.Value.ToUpper() == item.ToUpper()).Key;
                                    columnValue = columnValue.Replace(item, "'" + item + "'");
                                }
                                catch
                                {
                                    throw new Exception("Invalid Test Case Title : " + item);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                if (string.Compare(operatorValue, "Contains", StringComparison.OrdinalIgnoreCase) != 0)
                                    columnValue = objLib.GetTestCaseTitles().First(x => x.Value.ToUpper() == columnValue.ToUpper()).Value;
                                else if (!objLib.GetTestCaseTitles().Values.Any(x => x.ToLower().Contains(columnValue.ToLower())))
                                    throw new Exception("Invalid Test Case Title : " + columnValue);
                            }
                            catch
                            {
                                throw new Exception("Invalid Test Case Title : " + columnValue);
                            }
                        }
                    }
                    else if (objLib.IsEqual(columnName, "Release"))
                    {
                        columnName = "Release";
                        if (columnValue.Contains(','))
                        {
                            var releases = columnValue.Split(',');
                            foreach (var item in releases)
                            {
                                try
                                {
                                    var release = objLib.GetReleases().First(x => x.Value.ToUpper() == item.ToUpper()).Key;
                                    columnValue = columnValue.Replace(item, "'" + item + "'");
                                }
                                catch
                                {
                                    throw new Exception("Invalid Release Name : " + item);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                if (string.Compare(operatorValue, "Contains", StringComparison.OrdinalIgnoreCase) != 0)
                                    columnValue = objLib.GetReleases().First(x => x.Value.ToUpper() == columnValue.ToUpper()).Value;
                                else if (!objLib.GetReleases().Values.Any(x => x.ToLower().Contains(columnValue.ToLower())))
                                    throw new Exception("Invalid Release Name : " + columnValue);
                            }

                            catch
                            {
                                throw new Exception("Invalid Release Name : " + columnValue);
                            }
                        }
                    }

                    else if (objLib.IsEqual(columnName, "Functionality"))
                    {
                        columnName = "Functionality";
                        if (columnValue.Contains(','))
                        {
                            var functionalities = columnValue.Split(',');
                            foreach (var item in functionalities)
                            {
                                try
                                {
                                    var functionality = objLib.GetFunctionalities().First(x => x.Value.ToUpper() == item.ToUpper()).Value;
                                    columnValue = columnValue.Replace(item, "'" + functionality + "'");
                                }
                                catch
                                {
                                    throw new Exception("Invalid Functionality : " + item);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                if (string.Compare(operatorValue, "Contains", StringComparison.OrdinalIgnoreCase) != 0)
                                    columnValue = objLib.GetFunctionalities().First(x => x.Value.ToUpper() == columnValue.ToUpper()).Value;
                                else if (!objLib.GetFunctionalities().Values.Any(x => x.ToLower().Contains(columnValue.ToLower())))
                                    throw new Exception("Invalid Functionality : " + columnValue);
                            }

                            catch
                            {
                                throw new Exception("Invalid Functionality : " + columnValue);
                            }
                        }

                    }
                    else if (objLib.IsEqual(columnName, "TestCaseId"))
                    {
                        //testcaseIDs = columnValue.Split(',');
                        if (columnValue.Contains(','))
                        {
                            var tcids = columnValue.Split(',');
                            foreach (var item in tcids)
                            {
                                try
                                {
                                    var tcid = objLib.GetTestDataIDs().First(x => x.Value == item).Key;
                                }
                                catch
                                {
                                    throw new Exception("Invalid Test Case ID : " + item);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                if (string.Compare(operatorValue, "Contains", StringComparison.OrdinalIgnoreCase) != 0)
                                    columnValue = objLib.GetTestDataIDs().First(x => x.Value == columnValue).Key;
                                else if (!objLib.GetTestDataIDs().Values.Any(x => x.ToLower().Contains(columnValue.ToLower())))
                                    throw new Exception("Invalid TestCaseID : " + columnValue);
                            }
                            catch
                            {
                                throw new Exception("Invalid TestCaseID : " + columnValue);
                            }
                        }
                    }
                    if (objLib.IsEqual(operatorValue, "In"))
                        replacedclause = "[" + columnName + "] " + operatorValue + "(" + columnValue + ")";
                    //else if(string.Compare(operatorValue,"contains",StringComparison.OrdinalIgnoreCase)==0)
                    else if (string.Compare(operatorValue, "contains", StringComparison.OrdinalIgnoreCase) == 0)
                        replacedclause = "[" + columnName + "] LIKE " + "'%" + columnValue + "%'";
                    else
                        replacedclause = "[" + columnName + "]" + operatorValue + "'" + columnValue + "'";
                }
                return replacedclause;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void DebugScreen_Load(object sender, EventArgs e)
        {
            var configFile = XDocument.Load(@"Config\Config.xml");
            dict_Environments = configFile.XPathSelectElements("/TestAutomationFramework/Environments/*")
                                                                   .Select(x => new { key = x.Name.ToString(), value = x.Value })
                                                                   .OrderBy(x => x.key).ToDictionary(x => x.key, x => x.value);
            cmb_environment.Items.AddRange(dict_Environments.Select(x => x.Key).ToArray());
            cmb_repositiory.Items.AddRange(objLib.GetRepoVersion().Select(x => x.Key).ToArray());


            //cmb_application.Items.Clear();
            //cmb_repositiory.Items.Clear();
            //var configFile = XDocument.Load(@"Config\Config.xml");
            //dict_application = configFile.XPathSelectElements("/TestAutomationFramework/Environments/*")
            //                                                       .Select(x => new { key = x.Name.ToString(), value = x.Value })
            //                                                       .OrderBy(x => x.key).ToDictionary(x => x.key, x => x.value);
            //cmb_application.Items.AddRange(dict_application.Select(x => x.Key).ToArray());
            //cmb_repositiory.Items.AddRange(objLib.GetRepoVersion().Select(x => x.Key).ToArray());

        }

        private void cmb_application_selectionchanged(object sender, EventArgs e)
        {
            //cmb_environment.Items.Clear();
            //cmb_environment.Text = "";

            //var configFile = XDocument.Load(@"Config\Config.xml");
            //var app = cmb_application.SelectedItem.ToString();
            //dict_Environments = configFile.XPathSelectElements("/TestAutomationFramework/Environments/" + app + "/*")
            //                                                       .Select(x => new { key = x.Name.ToString(), value = x.Value })
            //                                                       .OrderBy(x => x.key).ToDictionary(x => x.key, x => x.value);
            //cmb_environment.Items.AddRange(dict_Environments.Select(x => x.Key).ToArray());
          //  lbl_uri.Text = dict_Environments[cmb_environment.SelectedItem.ToString()];
        }

        private void cmb_environment_selectionchanged(object sender, EventArgs e)
        {
            lbl_uri.Text = dict_Environments[cmb_environment.SelectedItem.ToString()];
        }
        private void grid_testdata_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            //foreach (DataGridViewRow row in grid_testdata.Rows)
            //{
            //    row.Cells[0].Value = false;
            //}

            //check select row
            grid_testdata.CurrentRow.Cells[0].Value = true;

            btn_Debug.Enabled = true;


        }

        private bool validateErrorMessage()
        {
            bool output = false;
            string outtext = "";
            Dictionary<string, string> val = new Dictionary<string, string>()
            {
                {"Browser Type", Convert.ToString(cmb_browser.SelectedItem) },
               // {"Application", Convert.ToString(cmb_application.SelectedItem) },
                {"Environment", Convert.ToString(cmb_environment.SelectedItem) },
                {"Repository", Convert.ToString(cmb_repositiory.SelectedItem) },

            };

            foreach (string key in val.Keys)
            {
                if (val[key] == "")
                    outtext += "-" + key + "\n";

            }
            if (outtext.Trim() != "")
            {
                MessageBox.Show("Please Provide inputs for : \n" + outtext, "Debug", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                output = true;
            }
            return output;

        }
        private void btn_Search_Click(object sender, EventArgs e)
        {
            try
            {
                //initiate test case params
                btn_Debug.Enabled = false;
                ExeObj.testcases.Clear();
                ExeObj.testcasesummary.Clear();
                ExeObj.functionality.Clear();
                string[] testCaseDetails = null;
                if (!objLib.IsNullOrEmpty(txt_TestCase.Text.ToString().Trim()))
                {
                    string strQuery = "SELECT TESTCASETITLE,TESTCASESUMMARY,FUNCTIONALITY FROM testCaseInfo WHERE ISDELETED IS NULL AND TestCaseID = " + Int32.Parse(txt_TestCase.Text.ToString().Trim()) + " AND Projectid = " + SignIn.projectId;
                    using (SqlConnection con = new SqlConnection(GetDBConnectionString()))
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand(strQuery, con))
                        {
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                testCaseDetails = new string[] { reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString() };

                            }
                        }
                    }
                    ExeObj.testcases.Add(Int32.Parse(txt_TestCase.Text.ToString().Trim()), testCaseDetails[0]);
                    ExeObj.testcasesummary.Add(Int32.Parse(txt_TestCase.Text.ToString().Trim()), testCaseDetails[1]);
                    ExeObj.functionality.Add(Int32.Parse(txt_TestCase.Text.ToString().Trim()), testCaseDetails[2]);

                    //poupulate testdata grid
                    string testDataQuery = "select P.PageName,T.FlowIdentifier,T.DataIdentifier,A.ActionName as Indicator, O.Label, T.ActionORData, T.SeqNumber from TestData T, MasterOR O,PageNames P, ActionKeyword A where TestCaseID = " + txt_TestCase.Text.ToString().Trim() + " and T.PageID = P.PageID and T.[Execute]='Yes' and O.MasterORID = T.MasterORID and T.Indicator= A.ActionKeyword_ID  order by T.SeqNumber Asc";

                    //string testDataQuery = "select P.PageName,T.FlowIdentifier,T.DataIdentifier,O.Label, T.SeqNumber, T.ActionORData from TestData T,MasterOR O,PageNames P where TestCaseID = " + txt_TestCase.Text.ToString().Trim() + " and T.PageID = P.PageID and O.MasterORID = T.MasterORID order by T.SeqNumber Asc";
                    DataTable dt = new DataTable();
                    dt = objLib.binddataTable(testDataQuery);
                    grid_testdata.DataSource = dt;
                    grid_testdata.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    grid_testdata.Columns[0].Width = 30;
                    grid_testdata.Visible = true;

                }
                else
                {
                    MessageBox.Show("Please provide valid TestCase ID", "Debug Screen", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                MessageBox.Show("Unable to Search the provided TestCase", "Debug Screen", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_Debug_Click(object sender, EventArgs e)
        {
            if (validateErrorMessage() == false)
            {
                ExeObj.debugFlag = true;
                ExeObj.debugSeqNum = "";
                foreach (DataGridViewRow row in grid_testdata.Rows)
                {
                    if (row.Cells[0].Value != null && (bool)row.Cells[0].Value)
                        ExeObj.debugSeqNum += row.Cells[7].Value.ToString().Trim() + ",";

                }
                ExeObj.debugSeqNum = ExeObj.debugSeqNum.TrimEnd(',');
                //ExeObj.debugSeqNum = grid_testdata.CurrentRow.Cells[7].Value.ToString().Trim();
                string browser = Convert.ToString(cmb_browser.SelectedItem);
                string env = Convert.ToString(cmb_environment.SelectedItem);
                string repositiory = Convert.ToString(cmb_repositiory.SelectedItem);
                //int application =Convert.ToInt16(cmb_application.SelectedItem);
                int application = 0;

                string projid = Convert.ToString(SignIn.projectId);
                dictTestCaseIDs.Clear();
                dictTestCaseIDs.Add(Int32.Parse(testc_id), cmb_browser.SelectedItem.ToString().ToLower());
                ExeObj.dictTestCaseIterations.Clear();
                ExeObj.dictTestCaseIterations.Add(Int32.Parse(testc_id), 1);
                Thread t1 = new Thread(() =>
                {

                    ExeObj.Initialize(browser, projid, dictTestCaseIDs, env, repositiory, application);
                    ChangeExecutionStatus();
                });
                t1.Start();
                lbl_Status.Text = "Please wait...Execution in debug mode is In-Progress...";

            }
        }

        public void ChangeExecutionStatus()
        {
            Invoke((Action)(() =>
            {
                //MessageBox.Show("Test Execution COMPLETED.");
                lbl_Status.Text = "Test Execution COMPLETED.";
                this.Activate();
            }));
        }

        public string GetDBConnectionString()
        {
            var configurationFile = XDocument.Load(@"Config\Config.xml");
            string connstr = configurationFile.XPathSelectElement("/TestAutomationFramework/DBExecution/DBConnectionString").ToString().Split('>')[1].Split('<')[0];
            return connstr;
        }

    }
}
