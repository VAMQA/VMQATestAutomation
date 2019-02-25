using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Framework.Tests.Auto;
using System.Threading;
using System.Diagnostics;
using VM.Platform.TestAutomationFramework.Core;
using System.IO;
using System.Data.SqlClient;

namespace UIDesign
{
    public partial class TestExecution : Form
    {
        private Label lbl_uri = new Label();
        private Label lbl_url = new Label();
        private Label lbl_planned = new Label();
        private Label lbl_testlist = new Label();
        private Label lbl_where = new Label();
        private Label lbl_exestatus = new Label();
        private Label lbl_inprogress = new Label();
        private Label lbl_browser = new Label();
        private Label lbl_testcaseids = new Label();
        private Label lbl_environment = new Label();
        private Label lbl_repositiory = new Label();
        private Label lbl_pagetitle = new Label();
        private Label lbl_searchby = new Label();
        private Label lbl_operator = new Label();
        private Label lbl_value = new Label();
        private Label lbl_errmsg = new Label();
        private Label lbl_exeMode = new Label();
        private Label lbl_groupby = new Label();
        private Label lbl_Favorites = new Label();

        List<Thread> threads = new List<Thread>();

        public ComboBox cmb_andor_1 = new ComboBox();
        public ComboBox cmb_andor_2 = new ComboBox();

        private ComboBox rdExecution = new ComboBox();

        private Button btn_execute = new Button();
        private Button btn_cancel = new Button();
        private Button btn_up = new Button();
        private Button btn_down = new Button();
        private Button btn_remove = new Button();
        private Button btn_clear = new Button();
        private Button btn_reset = new Button();
        private TextBox txt_dummy = new TextBox();
        private TreeView trv_testcaseids = new TreeView();
        private ComboBox cmb_browser = new ComboBox();
        private ComboBox cmb_environment = new ComboBox();
        private ComboBox cmb_repositiory = new ComboBox();
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
        public Button btn_search = new Button();
        public Button btn_AddtoFavorites = new Button();
        public Button btn_SearchFavorites = new Button();

        private CheckBox chk_condbox_0 = new CheckBox();
        private CheckBox chk_condbox_1 = new CheckBox();
        private CheckBox chk_condbox_2 = new CheckBox();

        private PictureBox pic_gecko = new PictureBox();
        private FlowLayoutPanel flp_testexecution = new FlowLayoutPanel();
        private TableLayoutPanel tlp_testexecution = new TableLayoutPanel();
        private TableLayoutPanel tlp_test = new TableLayoutPanel();
        Dictionary<string, string> dict_Environments = new Dictionary<string, string>();

        public ListBox lst_testcaseorder = new ListBox();
        public DataGridView dgv_testcaseorder = new DataGridView();
        DataGridViewComboBoxColumn dgv_cmb_browser = new DataGridViewComboBoxColumn();
        public Dictionary<int, int> dictTestCaseIterations = new Dictionary<int, int>();
        public Dictionary<int, string> testcases = new Dictionary<int, string>();
        public Dictionary<int, string> testcasesummary = new Dictionary<int, string>();
        public Dictionary<int, string> functionality = new Dictionary<int, string>();

        string[] searchparameters = { "Assigned To", "Functionality", "Release", "Tags", "TestCaseID", "TestCategory", "Title" };


        FuncLib objLib = new FuncLib();
        public static bool flag = true;
        Thread t1 = null;
        TreeNode tree_node;
        //string[] testcaseIDs;
        public string favInsertQuery;
        public string favRetrieveQuery;
        public bool favOption = false;
        public bool debugFlag = false;
        public string debugSeqNum = null;
        TestAutomation testAutomation = null;

        public TestExecution()
        {
            InitializeComponent();

            #region design

            //Form Settings
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            this.Width = 1360;
            this.Height = 720;
            //this.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 700 , System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 50);
            //this.AutoScroll = true;            
            this.Text = "Test Execution";

            //Flow Layout Panel Settings            
            flp_testexecution.FlowDirection = FlowDirection.LeftToRight;
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
            //tlp_test.Location = new Point(30, 80);

            //Gecko Picture Box Settings            
            pic_gecko.Height = 200;
            pic_gecko.Width = 200;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;


            //Value textbox settings                        
            txt_dummy.Name = "txt_dummy";
            txt_dummy.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_dummy.Height = 24;
            txt_dummy.Width = 50;
            txt_dummy.ReadOnly = true;
            txt_dummy.Visible = false;

            dgv_cmb_browser.HeaderText = "Browser";
            dgv_cmb_browser.Name = "cmb_browser";
            dgv_cmb_browser.FlatStyle = FlatStyle.Flat;
            //dgv_cmb_browser.Visible = false;
            dgv_cmb_browser.Items.AddRange(new[] { "ChromeHeadless", "Chrome", "Firefox", "Ie", "PhantomJS", "Android", "AndroidMobilelabs", "IOSMobilelabs" });

            //Browser Label settings            
            lbl_testcaseids.Text = "Test Cases :";
            lbl_testcaseids.Name = "lbl_testcaseids";
            lbl_testcaseids.TextAlign = ContentAlignment.BottomLeft;
            lbl_testcaseids.Font = new Font("Calibri", 10F, FontStyle.Bold);
            lbl_testcaseids.Height = 15;
            lbl_testcaseids.Width = 100;

            //Browser Label settings            
            lbl_testlist.Text = "Test Order :";
            lbl_testlist.Name = "lbl_testlist";
            lbl_testlist.TextAlign = ContentAlignment.BottomLeft;
            lbl_testlist.Font = new Font("Calibri", 10F, FontStyle.Bold);
            lbl_testlist.Height = 15;
            lbl_testlist.Width = 100;

            lbl_where.Width = 60;

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
            //Label Favorites settings
            lbl_Favorites.Text = "Favourites";
            lbl_Favorites.TextAlign = ContentAlignment.BottomRight;
            lbl_Favorites.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_Favorites.Height = 24;
            lbl_Favorites.Width = 70;

            ////SET Label settings            
            //rdSequential.Text = "Sequential ";
            //rdSequential.TextAlign = ContentAlignment.BottomLeft;
            //rdSequential.Font = new Font("Calibri", 10F, FontStyle.Regular);
            //rdSequential.Height = 20;
            //rdSequential.Width = 110;
            //rdSequential.Checked = true;

            //rdParallel.Text = "Parallel ";
            //rdParallel.TextAlign = ContentAlignment.BottomLeft;
            //rdParallel.Font = new Font("Calibri", 10F, FontStyle.Regular);
            //rdParallel.Height = 20;
            //rdParallel.Width = 110;

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
            cmb_condcol_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
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
            cmb_operator_0.Items.AddRange(new string[] { "=", "!=", "<", ">", "<=", ">=", "In", "Contains" });

            //ColumnNames combobox settings            
            cmb_condcol_1.Name = "cmb_condcol_1";
            cmb_condcol_1.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_condcol_1.Height = 24;
            cmb_condcol_1.Width = 250;
            cmb_condcol_1.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_condcol_1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_condcol_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
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
            cmb_operator_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_operator_1.Items.AddRange(new string[] { "=", "!=", "<", ">", "<=", ">=", "In", "Contains" });

            //ColumnNames combobox settings            
            cmb_andor_1.Name = "cmb_andor_1";
            cmb_andor_1.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_andor_1.Height = 24;
            cmb_andor_1.Width = 60;
            cmb_andor_1.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_andor_1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_andor_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_andor_1.Items.AddRange(new string[] { "And", "Or" });

            //ColumnNames combobox settings            
            cmb_andor_2.Name = "cmb_andor_2";
            cmb_andor_2.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_andor_2.Height = 24;
            cmb_andor_2.Width = 60;
            cmb_andor_2.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_andor_2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_andor_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_andor_2.Items.AddRange(new string[] { "And", "Or" });

            //ColumnNames combobox settings            
            cmb_condcol_2.Name = "cmb_condcol_2";
            cmb_condcol_2.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_condcol_2.Height = 24;
            cmb_condcol_2.Width = 250;
            cmb_condcol_2.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_condcol_2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_condcol_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
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
            cmb_operator_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_operator_2.Items.AddRange(new string[] { "=", "!=", "<", ">", "<=", ">=", "In", "Contains" });

            //Update button settings            
            btn_search.Text = "Search";
            btn_search.Name = "btn_search";
            btn_search.Font = new Font("Calibri", 10F, FontStyle.Regular);
            btn_search.Height = 24;
            btn_search.Width = 90;
            btn_search.BackColor = SystemColors.Window;
            //Search Favarites button settings            
            btn_SearchFavorites.Text = "Search Favourites";
            btn_SearchFavorites.Name = "btn_SearchFavorites";
            btn_SearchFavorites.Font = new Font("Calibri", 10F, FontStyle.Regular);
            btn_SearchFavorites.Height = 24;
            btn_SearchFavorites.Width = 130;
            btn_SearchFavorites.BackColor = SystemColors.Window;

            //Add Favorites button settings            
            btn_AddtoFavorites.Text = "Add to Favourites";
            btn_AddtoFavorites.Name = "btn_AddtoFavorites";
            btn_AddtoFavorites.Font = new Font("Calibri", 10F, FontStyle.Regular);
            btn_AddtoFavorites.Height = 24;
            btn_AddtoFavorites.Width = 130;
            btn_AddtoFavorites.BackColor = SystemColors.Window;
            btn_AddtoFavorites.Enabled = false;

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

            //URI Label settings                        
            lbl_uri.Name = "lbl_uri";
            lbl_uri.ForeColor = Color.Blue;
            lbl_uri.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_uri.Width = 500;
            lbl_uri.Height = 30;
            lbl_uri.TextAlign = ContentAlignment.MiddleLeft;
            lbl_uri.Text = "";

            //Tree View Settings
            trv_testcaseids.Name = "trv_testcaseids";
            trv_testcaseids.Font = new Font("Calibri", 11F, FontStyle.Regular);
            trv_testcaseids.CheckBoxes = true;
            trv_testcaseids.Height = 440;
            trv_testcaseids.Width = 600;


            //Tree View Settings

            lst_testcaseorder.Font = new Font("Calibri", 11F, FontStyle.Regular);
            lst_testcaseorder.Height = 180;
            lst_testcaseorder.Width = 600;
            lst_testcaseorder.SelectionMode = SelectionMode.MultiExtended;
            
            dgv_testcaseorder.Size = new Size(600, 180);
            dgv_testcaseorder.DefaultCellStyle.Font = new Font("Calibri", 10);
            dgv_testcaseorder.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
            dgv_testcaseorder.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8F, FontStyle.Bold);
            dgv_testcaseorder.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgv_testcaseorder.AllowUserToAddRows = false;
            //dgv_testcaseorder.Columns.Add("colTestCaseID", "TestCase ID");
            dgv_testcaseorder.Columns.Add("colTestCaseTitle", "TestCase Title");
            dgv_testcaseorder.Columns.Add(dgv_cmb_browser);
            dgv_testcaseorder.Columns["colTestCaseTitle"].ReadOnly = true;
            dgv_testcaseorder.Columns["colTestCaseTitle"].Width = 450;

            //Browser Label settings            
            lbl_browser.Text = "Browser Type :*";
            lbl_browser.Name = "lbl_browser";
            lbl_browser.TextAlign = ContentAlignment.BottomLeft;
            lbl_browser.Font = new Font("Calibri", 11F, FontStyle.Regular);
            lbl_browser.Height = 20;
            lbl_browser.Width = 150;


            lbl_exeMode.Text = "Execution Mode :*";
            lbl_exeMode.TextAlign = ContentAlignment.BottomLeft;
            lbl_exeMode.Font = new Font("Calibri", 11F, FontStyle.Regular);
            lbl_exeMode.Height = 20;
            lbl_exeMode.Width = 150;


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

            //Environment Value Label settings            
            lbl_repositiory.Text = "Repository :*";
            lbl_repositiory.Name = "lbl_repositiory";
            lbl_repositiory.TextAlign = ContentAlignment.BottomLeft;
            lbl_repositiory.Font = new Font("Calibri", 11F, FontStyle.Regular);
            lbl_repositiory.Height = 20;
            lbl_repositiory.Width = 150;

            //Planned Label settings                        
            lbl_planned.Name = "lbl_planned";
            lbl_planned.TextAlign = ContentAlignment.BottomLeft;
            lbl_planned.Font = new Font("Calibri", 10F, FontStyle.Bold);
            lbl_planned.Height = 20;
            lbl_planned.Width = 30;

            //Completed Label settings                                    
            lbl_exestatus.TextAlign = ContentAlignment.BottomLeft;
            lbl_exestatus.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_exestatus.ForeColor = Color.Red;
            lbl_exestatus.Height = 20;
            lbl_exestatus.Width = 400;
            //lbl_exestatus.Text = "Please Wait...";


            //Inprogress Label settings                        
            lbl_inprogress.Name = "lbl_inprogress";
            lbl_inprogress.TextAlign = ContentAlignment.BottomLeft;
            lbl_inprogress.Font = new Font("Calibri", 10F, FontStyle.Bold);
            lbl_inprogress.Height = 20;
            lbl_inprogress.Width = 200;

            //ColumnNames combobox settings            
            cmb_browser.Name = "cmb_browser";
            cmb_browser.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_browser.Height = 30;
            cmb_browser.Width = 250;
            cmb_browser.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_browser.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_browser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_browser.Items.AddRange(new[] { "ChromeHeadless", "Chrome", "Firefox", "Ie", "PhantomJS", "Remote", "Android", "AndroidMobilelabs", "IOSMobilelabs" });

            //ColumnNames combobox settings            
            cmb_environment.Name = "cmb_environment";
            cmb_environment.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_environment.Height = 30;
            cmb_environment.Width = 250;
            cmb_environment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_environment.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_environment.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //ColumnNames combobox settings            
            cmb_repositiory.Name = "cmb_repositiory";
            cmb_repositiory.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_repositiory.Height = 30;
            cmb_repositiory.Width = 250;
            cmb_repositiory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_repositiory.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_repositiory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //ColumnNames combobox settings            
            rdExecution.Name = "rdExecution";
            rdExecution.Font = new Font("Calibri", 11F, FontStyle.Regular);
            rdExecution.Height = 30;
            rdExecution.Width = 250;
            rdExecution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            rdExecution.AutoCompleteSource = AutoCompleteSource.ListItems;
            rdExecution.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            rdExecution.Items.AddRange(new[] { "Sequential", "Sequential-Retry", "Parallel" });

            cmb_groupby.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_groupby.Height = 30;
            cmb_groupby.Width = 250;
            cmb_groupby.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_groupby.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_groupby.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_groupby.Items.AddRange(new[] { "Default View", "Functionality" });
            cmb_groupby.SelectedItem = "Default View";
            //Favorites combobox settings            
            cmb_Favorites.Name = "cmb_Favorites";
            cmb_Favorites.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_Favorites.Height = 24;
            cmb_Favorites.Width = 250;
            cmb_Favorites.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_Favorites.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_Favorites.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_Favorites.Items.AddRange((from t in objLib.GetFavorites() orderby t.Value select t.Value).ToArray()); 

            //Execute button settings            
            btn_execute.Text = "Run";
            btn_execute.Name = "btn_execute";
            btn_execute.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_execute.Height = 30;
            btn_execute.Width = 80;

            //Reset button settings            
            btn_reset.Text = "Reset";
            btn_reset.Name = "btn_reset";
            btn_reset.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_reset.Height = 30;
            btn_reset.Width = 80;

            btn_up.Text = char.ConvertFromUtf32(0x2191);       
            btn_up.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_up.Height = 30;
            btn_up.Width = 70;

            btn_down.Text = char.ConvertFromUtf32(0x2193);
            btn_down.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_down.Height = 30;
            btn_down.Width = 70;

            btn_remove.Text = "DEL";
            btn_remove.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_remove.Height = 30;
            btn_remove.Width = 70;

            btn_clear.Text = "CLR";
            btn_clear.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_clear.Height = 30;
            btn_clear.Width = 70;



            //Cancel button settings            
            btn_cancel.Text = "Cancel";
            btn_cancel.Name = "btn_cancel";
            btn_cancel.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_cancel.Height = 30;
            btn_cancel.Width = 100;

            //Userid Label settings           
            lbl_pagetitle.Height = 30;
            lbl_pagetitle.Width = 700;
            lbl_pagetitle.Text = "TEST EXECUTION";
            lbl_pagetitle.Name = "lbl_pagetitle";
            lbl_pagetitle.TextAlign = ContentAlignment.MiddleCenter;
            lbl_pagetitle.Font = new Font("Calibri", 20F, FontStyle.Bold);
            lbl_pagetitle.ForeColor = Color.Blue;

            //Error Message Label settings           
            lbl_errmsg.Height = 24;
            lbl_errmsg.Width = 250;
            lbl_errmsg.Name = "lbl_errmsg";
            lbl_errmsg.TextAlign = ContentAlignment.BottomLeft;
            lbl_errmsg.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_errmsg.ForeColor = Color.Red;
            lbl_errmsg.Visible = false;
            lbl_errmsg.Text = "Invalid Inputs...";

            tlp_test.Controls.Add(pic_gecko, 0, 1);
            tlp_test.SetRowSpan(pic_gecko, 8);
            //tlp_test.SetColumnSpan(pic_gecko, 4);
            tlp_test.Controls.Add(lbl_pagetitle, 1, 1);
            tlp_test.SetColumnSpan(lbl_pagetitle, 8);

            tlp_test.Controls.Add(lbl_searchby, 3, 2);
            tlp_test.Controls.Add(lbl_operator, 4, 2);
            tlp_test.Controls.Add(lbl_value, 5, 2);

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
            tlp_test.SetColumnSpan(lbl_groupby, 2);
            tlp_test.Controls.Add(cmb_groupby, 3, 6);
            tlp_test.SetColumnSpan(cmb_groupby, 3);
            tlp_test.Controls.Add(btn_search, 1, 7);
            tlp_test.SetColumnSpan(btn_search, 2);
            tlp_test.Controls.Add(btn_AddtoFavorites, 2, 7);
            tlp_test.SetColumnSpan(btn_AddtoFavorites, 2);
            tlp_test.Controls.Add(lbl_Favorites, 1, 8);
            tlp_test.SetColumnSpan(lbl_Favorites, 2);
            tlp_test.Controls.Add(cmb_Favorites, 2, 8);
            //tlp_test.SetColumnSpan(cmb_Favorites, 3);
            tlp_test.Controls.Add(btn_SearchFavorites, 3, 8);
            //tlp_test.Controls.Add(btn_SearchFavorites, 2, 9);
            tlp_test.SetColumnSpan(btn_SearchFavorites, 2);
            tlp_test.Controls.Add(lbl_errmsg, 3, 7);
            tlp_test.SetColumnSpan(lbl_errmsg, 2);


            tlp_testexecution.Controls.Add(lbl_planned, 1, 1);
            tlp_testexecution.Controls.Add(lbl_testcaseids, 0, 1);
            //tlp_testexecution.SetColumnSpan(lbl_testcaseids, 3);
            tlp_testexecution.Controls.Add(trv_testcaseids, 0, 2);
            tlp_testexecution.SetRowSpan(trv_testcaseids, 12);
            //tlp_testexecution.SetColumnSpan(trv_testcaseids, 5);
            tlp_testexecution.Controls.Add(lbl_testlist, 2, 1);
            //tlp_testexecution.Controls.Add(lst_testcaseorder, 2, 2);
            //tlp_testexecution.SetColumnSpan(lst_testcaseorder, 3);
            //tlp_testexecution.SetRowSpan(lst_testcaseorder, 4);

            tlp_testexecution.Controls.Add(dgv_testcaseorder, 2, 2);
            tlp_testexecution.SetColumnSpan(dgv_testcaseorder, 3);
            tlp_testexecution.SetRowSpan(dgv_testcaseorder, 4);

            tlp_testexecution.Controls.Add(btn_up, 5, 2);
            tlp_testexecution.Controls.Add(btn_down, 5, 3);
            tlp_testexecution.Controls.Add(btn_remove, 5, 4);
            tlp_testexecution.Controls.Add(btn_clear, 5, 5);
            tlp_testexecution.Controls.Add(lbl_browser, 2, 6);
            //tlp_testexecution.SetColumnSpan(lbl_browser, 4);
            tlp_testexecution.Controls.Add(cmb_browser, 3, 6);
            tlp_testexecution.SetColumnSpan(cmb_browser, 2);
            tlp_testexecution.Controls.Add(lbl_environment, 2, 7);
            //tlp_testexecution.SetColumnSpan(lbl_environment, 4);
            tlp_testexecution.Controls.Add(cmb_environment, 3, 7);
            tlp_testexecution.SetColumnSpan(cmb_environment, 2);

            tlp_testexecution.Controls.Add(lbl_url, 2, 8);
            tlp_testexecution.Controls.Add(lbl_uri, 3, 8);
            tlp_testexecution.SetColumnSpan(lbl_uri, 3);
            tlp_testexecution.Controls.Add(lbl_repositiory, 2, 9);
            //tlp_testexecution.SetColumnSpan(lbl_repositiory, 4);
            tlp_testexecution.Controls.Add(cmb_repositiory, 3, 9);
            tlp_testexecution.SetColumnSpan(cmb_repositiory, 2);
            tlp_testexecution.Controls.Add(lbl_exeMode, 2, 10);
            //tlp_testexecution.SetColumnSpan(lbl_exeMode, 4);
            tlp_testexecution.Controls.Add(rdExecution, 3, 10);
            //tlp_testexecution.SetColumnSpan(rdParallel, 4);
            tlp_testexecution.Controls.Add(lbl_exestatus, 2, 11);
            tlp_testexecution.SetColumnSpan(lbl_exestatus, 3);
            tlp_testexecution.Controls.Add(btn_execute, 4, 12);
            tlp_testexecution.Controls.Add(btn_reset, 3, 12);


            //Adding Controls to Flow Layout Panel
            flp_testexecution.Controls.AddRange(new Control[] { tlp_test, tlp_testexecution });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_testexecution });
            //this.Load += new System.EventHandler(TestExecution_Load);
            this.FormClosing += new FormClosingEventHandler(testeexecution_closing);

            #endregion

            #region methods

            //Methods
            cmb_environment.SelectedIndexChanged += new System.EventHandler(cmb_environment_selectionchanged);
            cmb_browser.SelectedIndexChanged += new System.EventHandler(cmb_browsertype_IndexChanged);
            //cmb_groupby.SelectedIndexChanged += new System.EventHandler(cmb_groupby_selectionchanged);
            btn_execute.Click += new System.EventHandler(btn_execute_click);
            btn_reset.Click += new System.EventHandler(btn_reset_click);
            btn_search.Click += new System.EventHandler(btn_search_click);
            btn_AddtoFavorites.Click += new System.EventHandler(btn_AddtoFavorites_click);
            btn_SearchFavorites.Click += new System.EventHandler(btn_SearchFavorites_click);
            btn_clear.Click += new System.EventHandler(btn_clear_click);
            btn_remove.Click += new System.EventHandler(btn_remove_click);
            btn_up.Click += new System.EventHandler(btn_up_click);
            btn_down.Click += new System.EventHandler(btn_down_click);
            //trv_testcaseids.AfterCheck += new TreeViewEventHandler(trv_testcaseids_afterselect);
            trv_testcaseids.AfterCheck += new TreeViewEventHandler(treeView_AfterCheck);
            cmb_Favorites.Click += new System.EventHandler(cmb_Favourite_ISelect);

            #endregion
        }
        private void cmb_Favourite_ISelect(object sender, EventArgs e)
        {
            cmb_Favorites.Items.Clear();
            cmb_Favorites.Items.AddRange((from t in objLib.GetFavorites() orderby t.Value select t.Value).ToArray());
        }

        //button click -"UP ARROW"
        private void btn_up_click(object sender, EventArgs e)
        {
            int selectedIndex = lst_testcaseorder.SelectedIndex;
            if (selectedIndex > 0)
            {
                lst_testcaseorder.Items.Insert(selectedIndex - 1, lst_testcaseorder.Items[selectedIndex]);
                lst_testcaseorder.Items.RemoveAt(selectedIndex + 1);
                lst_testcaseorder.SelectedIndex = selectedIndex - 1;
            }

            DataGridViewRowCollection rows = dgv_testcaseorder.Rows;
            if(rows.Count>0)
            {
                int index = dgv_testcaseorder.SelectedCells[0].OwningRow.Index;
                // remove the previous row and add it behind the selected row.
                if (index != 0)
                {
                    DataGridViewRow prevRow = rows[index - 1];
                    rows.Remove(prevRow);
                    prevRow.Frozen = false;
                    rows.Insert(index, prevRow);
                    dgv_testcaseorder.ClearSelection();
                    dgv_testcaseorder.Rows[index - 1].Selected = true;
                }  
            }                      
        }

        //button click -"DOWN ARROW"
        private void btn_down_click(object sender, EventArgs e)
        {
            int selectedIndex = lst_testcaseorder.SelectedIndex;
            if (selectedIndex < lst_testcaseorder.Items.Count - 1 & selectedIndex != -1)
            {
                lst_testcaseorder.Items.Insert(selectedIndex + 2, lst_testcaseorder.Items[selectedIndex]);
                lst_testcaseorder.Items.RemoveAt(selectedIndex);
                lst_testcaseorder.SelectedIndex = selectedIndex + 1;
            }

            DataGridViewRowCollection rows = dgv_testcaseorder.Rows;
            if(rows.Count>0)
            {
                var rowCount = dgv_testcaseorder.Rows.Count;
                int index = dgv_testcaseorder.SelectedCells[0].OwningRow.Index;

                if (index != rowCount - 1)
                {
                    // remove the next row and add it in front of the selected row.
                    DataGridViewRow nextRow = rows[index + 1];
                    rows.Remove(nextRow);
                    nextRow.Frozen = false;
                    rows.Insert(index, nextRow);
                    dgv_testcaseorder.ClearSelection();
                    dgv_testcaseorder.Rows[index + 1].Selected = true;
                } 
            }                       
        }

        //button click - "CLR"
        private void btn_clear_click(object sender, EventArgs e)
        {            
            uncheckAllListItemsFromTreeView();
            removeAllItemsFromListBox();
            dgv_testcaseorder.Rows.Clear();
        }   
     
        //button click - 'DEL"
        private void btn_remove_click(object sender, EventArgs e)
        {
            removeSelectedItemsFromGridView();
            uncheckSelectedItemsFromTreeView();
            removeSelectedItemsFromListBox();            
        }
        
        private void removeSelectedItemsFromGridView()
        {
            var src = dgv_testcaseorder.SelectedRows;
            foreach(DataGridViewRow r in src)
            {
                lst_testcaseorder.SetSelected(lst_testcaseorder.Items.IndexOf(r.Cells["colTestCaseTitle"].Value), true);                
                //dgv_testcaseorder.Rows.RemoveAt(r.Index);

                foreach (TreeNode node in trv_testcaseids.Nodes)
                {
                    var childNodes = node.Nodes.Find(Convert.ToString(lst_testcaseorder.SelectedItem), false).FirstOrDefault();
                    if (childNodes == null)
                    {
                        DataGridViewRow row = dgv_testcaseorder.Rows
                           .Cast<DataGridViewRow>()
                           .Where(rw => rw.Cells["colTestCaseTitle"].Value.ToString().Equals(Convert.ToString(lst_testcaseorder.SelectedItem)))
                           .First();

                        dgv_testcaseorder.Rows.RemoveAt(row.Index);
                    }
                }
            }            
        }

        //remove selected items from listbox
        private void removeSelectedItemsFromListBox()
        {
            ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(lst_testcaseorder);
            selectedItems = lst_testcaseorder.SelectedItems;

            if (lst_testcaseorder.SelectedIndex != -1)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                    lst_testcaseorder.Items.Remove(selectedItems[i]);
            }
        }

        //remove items from listbox
        private void removeAllItemsFromListBox()
        {
            ListBox.ObjectCollection allItems = new ListBox.ObjectCollection(lst_testcaseorder);
            allItems = lst_testcaseorder.Items;

            //if (lst_testcaseorder.SelectedIndex != -1)
            //{
                for (int i = allItems.Count - 1; i >= 0; i--)
                    lst_testcaseorder.Items.Remove(allItems[i]);
            //}
        }

        //uncheck all list items from treeview
        private void uncheckAllListItemsFromTreeView()
        {
            ListBox.ObjectCollection listItems = new ListBox.ObjectCollection(lst_testcaseorder);
            listItems = lst_testcaseorder.Items;

            for (int i = listItems.Count - 1; i >= 0; i--)
            {
                foreach (TreeNode node in trv_testcaseids.Nodes)
                {
                    var childNodes = node.Nodes.Find(listItems[i].ToString(), false).FirstOrDefault();
                    if (childNodes != null)
                    {
                        childNodes.Checked = false;
                        break;
                    }
                }
            }
        }        

        //uncheck selected items from treeview
        private void uncheckSelectedItemsFromTreeView()
        {
            ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(lst_testcaseorder);
            selectedItems = lst_testcaseorder.SelectedItems;

            for (int i = selectedItems.Count - 1; i >= 0; i--)
            {
                foreach (TreeNode node in trv_testcaseids.Nodes)
                {
                    var childNodes = node.Nodes.Find(selectedItems[i].ToString(), false).FirstOrDefault();
                    if (childNodes != null)
                    {                     
                        childNodes.Checked = false;                     
                        break;
                    }
                }
            }
            tree_node = null;
        }

        //button click - "Search"
        private void btn_search_click(object sender, EventArgs e)
        {
           // testcaseIDs = null;
            try
            {
                lbl_errmsg.Visible = false;
                string executeQuery;
                if (favOption == true)
                {
                    executeQuery = favRetrieveQuery;
                }
                else
                {
                    executeQuery = buildQuery();
                    favInsertQuery = executeQuery;
                }
                if ((flag) && (!objLib.IsNullOrEmpty(executeQuery)))
                {
                    try
                    {
                        DataTable dt = new DataTable();
                        dt = objLib.binddataTable(executeQuery);
                        trv_testcaseids.Nodes.Clear();

                        //====== 08/02/2017======

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
                                    //trNode.Nodes.Add(new TreeNode() { Text = item[0].ToString() + String.Concat(Enumerable.Repeat(" ", 7 - (item[0].ToString().Length))) + " : " + item[1].ToString() });
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
                            btn_AddtoFavorites.Enabled = true;
                        }
                        else
                        {
                            lbl_errmsg.Visible = true;
                            lbl_errmsg.Text = "NO DATA FOUND";
                        }






                        //======


                        //if(testcaseIDs!=null)
                        //{
                        //    for(int i=0;i<testcaseIDs.Length;i++)
                        //    {
                        //        foreach (DataRow item in dt.Rows)
                        //        {
                        //            if(item[0].ToString()==testcaseIDs[i])
                        //            trv_testcaseids.Nodes.Add(new TreeNode() { Text = item[0].ToString() + " : " + item[1].ToString() });
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    foreach (DataRow item in dt.Rows)
                        //    {
                        //        trv_testcaseids.Nodes.Add(new TreeNode() { Text = item[0].ToString() + " : " + item[1].ToString() });
                        //    }
                        //}


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
                MessageBox.Show("ERROR : " + exc.Message, "Test Execution");
            }
        }
        private void btn_AddtoFavorites_click(object sender, System.EventArgs e)
        {
            // tlp_testexecution.Visible = false;
            Favorites frmFavorites = new Favorites();
            frmFavorites.favQuery = this.favInsertQuery;
            frmFavorites.ShowDialog();
        }
        private void btn_SearchFavorites_click(object sender, System.EventArgs e)
        {
            if (!objLib.IsNullOrEmpty(Convert.ToString(cmb_Favorites.SelectedItem)))
            {
                favOption = true;

                string desiredQuery = "SELECT [Query] FROM Favourites where FavouriteName='" + cmb_Favorites.SelectedItem.ToString().Trim() + "' And USERID= '" + objLib.GetUserInfo()[SignIn.userId.ToUpper()].ToString() + "'";
                favRetrieveQuery = objLib.ExecuteScalar(desiredQuery).ToString();

                favRetrieveQuery = favRetrieveQuery.Replace("\"", "'");
                btn_search_click(sender, e);
                favOption = false;
                btn_AddtoFavorites.Enabled = false;
            }
            else
            {
                MessageBox.Show("Please Select Favourite", "Search Favourites", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }

        //generate search query
        private string buildQuery()
        {
            try
            {
                string updateQuery = string.Empty;
                flag = true;

                if (chk_condbox_0.Checked)
                {
                    updateQuery = "SELECT TESTCASEID,TESTCASETITLE,FUNCTIONALITY FROM TESTCASEINFO WHERE ISDELETED IS NULL AND PROJECTID= " + SignIn.projectId;
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
                return updateQuery + ") ORDER BY TESTCASEID ASC";
                //return updateQuery + "AND ISDELETED IS NULL ORDER BY TESTCASEID ASC";
                //return updateQuery + "AND ISDELETED IS NULL";
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        //generate search query clauses
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
                    //==================
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
                    //===================
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

        //formm closing
        private void testeexecution_closing(object sender, FormClosingEventArgs e)
        {
            if (btn_execute.Text != "Run")
                e.Cancel = true;
        }

        //form loading 
        private void TestExecution_Load(object sender, EventArgs e)
        {
            var configFile = XDocument.Load(@"Config\Config.xml");
            dict_Environments = configFile.XPathSelectElements("/TestAutomationFramework/Environments/*")
                                                                   .Select(x => new { key = x.Name.ToString(), value = x.Value })
                                                                   .OrderBy(x => x.key).ToDictionary(x => x.key, x => x.value);
            cmb_environment.Items.AddRange(dict_Environments.Select(x => x.Key).ToArray());
            cmb_repositiory.Items.AddRange(objLib.GetRepoVersion().Select(x => x.Key).ToArray());

            //var treeviewDict = objLib.GetTestCaseIDs()
            //                             .Where(x => objLib.GetTestDataIDs().ContainsKey(x.Key.ToString())).ToDictionary(x => x.Key, x => x.Value)
            //                             .GroupBy(x => x.Value);

            //////--var treeviewDict = SQLQueries.GetTestCaseIDs.Where(x => SQLQueries.GetTestDataIDs.ContainsKey(x.Key.ToString())).Select(t => new { t.Key, t.Value }).ToDictionary(t => t.Key, t => t.Value);

            //foreach (var item in treeviewDict)
            //{
            //    TreeNode trNode = new TreeNode(item.Key);
            //    trNode.Name = item.Key;
            //    trv_testcaseids.Nodes.Add(trNode);

            //    var tcids = (from t in item select t.Key).ToArray();
            //    foreach (var i in tcids)
            //        trNode.Nodes.Add(new TreeNode(i.ToString()));
            //}
        }

        // button click "Run"
        private void btn_execute_click(object sender, EventArgs e)
        {
            string browser = Convert.ToString(cmb_browser.SelectedItem);
            string env = Convert.ToString(cmb_environment.SelectedItem);
            string repositiory = Convert.ToString(cmb_repositiory.SelectedItem);
            string projid = Convert.ToString(SignIn.projectId);
            List<int> testcaseid = new List<int>();
            int retry = 0;
            Dictionary<int, string> dictTestCaseIDs = new Dictionary<int, string>();

            if (btn_execute.Text != "Run")
            {
                btn_execute.Enabled = false;
                btn_reset.Enabled = false;
                KillAllDriversBeforeExecutionStarts();
                if (rdExecution.SelectedItem.ToString() == "Parallel")
                {
                    lbl_exestatus.Text = "Cancelling...";
                    foreach (var tr in threads)
                    {
                        tr.Abort();

                        while ((tr != null) && (tr.IsAlive))
                        {
                            lbl_exestatus.Text = "Cancelling...";
                            tr.Abort();
                        }
                    }
                }
                else if (rdExecution.SelectedItem.ToString() == "Sequential")
                {
                    while ((t1 != null) && (t1.IsAlive))
                    {
                        btn_execute.Enabled = false;

                        lbl_exestatus.Text = "Cancelling...";
                        t1.Abort();
                    }
                }
                else if (rdExecution.SelectedItem.ToString() == "Sequential-Retry")
                {
                    while ((t1 != null) && (t1.IsAlive))
                    {
                        btn_execute.Enabled = false;

                        lbl_exestatus.Text = "Cancelling...";
                        t1.Abort();
                    }
                }
                lbl_exestatus.Text = "Execution Cancelled By User..";
                trv_testcaseids.Enabled = true;
                cmb_browser.Enabled = true;
                cmb_environment.Enabled = true;
                cmb_repositiory.Enabled = true;
                btn_execute.Text = "Run";
                btn_execute.Enabled = true;
                btn_reset.Enabled = true;
            }
            else
            {
                foreach (var item in lst_testcaseorder.Items)
                {
                    var tcid = item.ToString().Split(':')[0].Trim();
                    if (!testcaseid.Contains(Int32.Parse(tcid)))
                    {
                        testcaseid.Add(Int32.Parse(tcid));
                    }                                     
                }
                foreach (DataGridViewRow row in dgv_testcaseorder.Rows)
                {
                    var tcid = row.Cells["colTestCaseTitle"].Value.ToString().Split(':')[0].Trim();
                    var tcbrowser = row.Cells["cmb_browser"].Value ?? string.Empty;
                    if (!dictTestCaseIDs.ContainsKey(Int32.Parse(tcid)))
                    {
                        dictTestCaseIDs.Add(Int32.Parse(tcid), tcbrowser.ToString());
                    }
                }
                if ((!objLib.IsNullOrEmpty(Convert.ToString(cmb_browser.SelectedItem))) && 
                    (!objLib.IsNullOrEmpty(Convert.ToString(cmb_environment.SelectedItem))) &&
                    (!objLib.IsNullOrEmpty(Convert.ToString(cmb_repositiory.SelectedItem))) &&
                    (dictTestCaseIDs.Count != 0))
                {
                    KillAllDriversBeforeExecutionStarts();
                    btn_execute.Text = "Cancel";
                    trv_testcaseids.Enabled = false;
                    cmb_browser.Enabled = false;
                    cmb_environment.Enabled = false;
                    cmb_repositiory.Enabled = false;
                    btn_reset.Enabled = false;
                    if (rdExecution.SelectedItem.ToString() == "Sequential")
                    {

                        t1 = new Thread(() =>
                        {
                            Initialize(browser, projid, dictTestCaseIDs, env, repositiory, retry);
                            runbuttonRename();
                        });
                        t1.Start();
                        lbl_exestatus.Text = "Please Wait..Sequential Execution IN-PROGRESS..";
                    }
                    else if (rdExecution.SelectedItem.ToString() == "Sequential-Retry")
                    {
                        retry = 1;
                        t1 = new Thread(() =>
                        {
                            Initialize(browser, projid, dictTestCaseIDs, env, repositiory, retry);
                            runbuttonRename();
                        });
                        t1.Start();
                        lbl_exestatus.Text = "Please Wait..Sequential Execution IN-PROGRESS..";
                    }
                    else if (rdExecution.SelectedItem.ToString() == "Parallel")
                    {
                        threads.Clear();
                        TimeSpan executionTime = TimeSpan.Zero;

                        //if (File.Exists("Execution.log"))
                        //    File.Delete("Execution.log");
                        Task task = Task.Factory.StartNew(() =>
                        {
                            InitializeParallel(browser, projid, dictTestCaseIDs, env, repositiory, retry, executionTime);
                            //foreach (var tcid in testcaseid)
                            //{
                            //    var testcaseId = tcid;

                            //    Thread t = new Thread(() => ExecuteTestsInParallel(testcaseId, env, browser, repositiory, projid));
                            //    threads.Add(t);
                            //    t.Start();

                            //    Thread.Sleep(2000);
                            //}
                        });
                        lbl_exestatus.Text = "Please Wait..Parallel Execution IN-PROGRESS..";
                        btn_execute.Text = "Cancel";
                        Thread.Sleep(3000);
                        btn_execute.Enabled = true;
                        btn_reset.Enabled = false;

                        AutoCustomerTests autotest = new AutoCustomerTests();
                        
                        //Task verifystatus = Task.Factory.StartNew(() => verifythreadstatus(threads));
                    }
                }
                else
                {
                    MessageBox.Show("Input Required For : \n   - Browser Type \n   - Environement\n   - Repositiory\n   - TestCaseID(s)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_reset_click(object sender, EventArgs e)        
        {
            chk_condbox_0.Checked = false;
            chk_condbox_1.Checked = false;
            chk_condbox_2.Checked = false;

            cmb_condcol_0.ResetText();
            cmb_condcol_1.ResetText();
            cmb_condcol_2.ResetText();

            cmb_andor_1.ResetText();
            cmb_andor_2.ResetText();

            cmb_operator_0.ResetText();
            cmb_operator_1.ResetText();
            cmb_operator_2.ResetText();

            txt_condval_0.Text = "";
            txt_condval_1.Text = "";
            txt_condval_2.Text = "";

            cmb_groupby.ResetText();
            cmb_groupby.SelectedItem = "Default View";
            trv_testcaseids.Nodes.Clear();
            lst_testcaseorder.Items.Clear();
            dgv_testcaseorder.Rows.Clear();

            cmb_browser.ResetText();
            cmb_environment.ResetText();
            cmb_repositiory.ResetText();
            lbl_uri.Text = "";
            rdExecution.ResetText();

            lbl_exestatus.Text = "";
        }

        // kill browser base webdrivers
        private void KillAllDriversBeforeExecutionStarts()
        {
            var processNamesToKill = new List<string> { "chromedriver", "IEDriverServer"};

            Process.GetProcesses().Where(p => processNamesToKill.Contains(p.ProcessName)).ToList()
                                  .ForEach(y => y.Kill());
        }

        //intialize parallel execution
        public void InitializeParallel(string browserType, string projId, Dictionary<int, string> testcaseId, string env, string repositiory, int retry, TimeSpan executionTime)
        {
            //TestAutomation testAutomation = null;
            AutoCustomerTests autotest = new AutoCustomerTests();
            Framework.Tests.ConsolidatedHTMLReport.ListConsolidatedResults.ConsolidatedTestCaseResults.Clear();

            //TimeSpan executionTime = TimeSpan.Zero;
            if (File.Exists("Execution.log"))
            {
                File.Delete("Execution.log");
            }
            foreach (var id in testcaseId)
            {
                Thread t = new Thread(() =>
                {
                    try
                    {
                        const string repositoryLocation = "https://local";
                        const string projectName = null;
                        string testPlan = projId + "_" + SignIn.projectName;
                        string controlMapSource = repositiory;
                        string browser = string.Empty;
                        if (browserType == "Remote")
                            browser = browserType + id.Value;
                        else
                            browser = id.Value;
                        testAutomation = TestAutomation.Init(repositoryLocation, projectName, testPlan, browser, controlMapSource);
                        //if (File.Exists("Execution.log"))
                        //{
                        //    File.Delete("Execution.log");
                        //}
                        autotest.Execute(id.Key, env, testAutomation, retry);

                        //testAutomation.TestRunCleanup();
                        //autotest.Cleanup();
                    }
                    catch
                    {
                        if (testAutomation != null)
                        {
                            //testAutomation.TestRunCleanup();
                        }

                    }
                });
                threads.Add(t);
                t.Start();
                Thread.Sleep(5000);
            }
            Task verifystatus = Task.Factory.StartNew(() => verifythreadstatus(threads));
        }

        //ignore
        private void ExecuteTestsInParallel(int testId, string env, string browser, string repositiory, string projectId, int retry)
        {
            const string repositoryLocation = "https://local";
            const string projectName = null;
            string testPlan = projectId;
            string controlMapSource = repositiory;
            TestAutomation testAutomation = null;

            try
            {
                AutoCustomerTests autotest = new AutoCustomerTests();
                testAutomation = TestAutomation.Init(repositoryLocation, projectName, testPlan, browser, controlMapSource);
                Thread.Sleep(1000);
                autotest.Execute(testId, env, testAutomation, retry);
                testAutomation.TestRunCleanup();
            }
            catch
            {
                if (testAutomation != null)
                    testAutomation.TestRunCleanup();
            }
        }

        //verify threads running status
        private void verifythreadstatus(List<Thread> executionThreads)
        {
            var AllExecutionThreads = executionThreads;

            foreach (var executionThread in AllExecutionThreads.ToList())
            {
                while (executionThread.ThreadState != System.Threading.ThreadState.Stopped)
                {
                    // Do Nothing
                }
            }

            runbuttonRename();
            AutoCustomerTests autotest = new AutoCustomerTests();
            TimeSpan executionTime = TimeSpan.Zero;
            autotest.ConsolidateReport(executionTime);

            if (testAutomation != null)
                testAutomation.TestRunCleanup();

        }

        //intializing sequential execution
        public void Initialize(string browserType, string projId, Dictionary<int, string> testcaseId, string env, string repositiory, int retry)
        {
            TestAutomation testAutomation = null;
            AutoCustomerTests autotest = new AutoCustomerTests();
            Framework.Tests.ConsolidatedHTMLReport.ListConsolidatedResults.ConsolidatedTestCaseResults.Clear();

            TimeSpan executionTime = TimeSpan.Zero;
            foreach (var id in testcaseId)
            {
                try
                {
                    const string repositoryLocation = "https://local";
                    const string projectName = null;
                    string testPlan = projId + "_" + SignIn.projectName;
                    string controlMapSource = repositiory;
                    string driver = id.Value;
                    string browser = string.Empty;
                    if (browserType == "Remote")
                        browser = browserType + id.Value;
                    else
                        browser = id.Value;

                    testAutomation = TestAutomation.Init(repositoryLocation, projectName, testPlan, browser, controlMapSource);
                    if (File.Exists("Execution.log"))
                    {
                        File.Delete("Execution.log");
                    }
                    autotest.Execute(id.Key, env, testAutomation, retry);
                    testAutomation.TestRunCleanup();
                }
                catch
                {
                    if (testAutomation != null)
                        testAutomation.TestRunCleanup();
                }
            }
            autotest.ConsolidateReport(executionTime);
        }

        //populate test xecution status
        public void runbuttonRename()
        {
            Invoke((Action)(() =>
            {
                //MessageBox.Show("Test Execution COMPLETED.");
                lbl_exestatus.Text = "Test Execution COMPLETED.";
                btn_execute.Text = "Run";
                trv_testcaseids.Enabled = true;
                cmb_browser.Enabled = true;
                cmb_environment.Enabled = true;
            }));
        }

        //populate application url
        private void cmb_environment_selectionchanged(object sender, EventArgs e)
        {            
            lbl_uri.Text = dict_Environments[cmb_environment.SelectedItem.ToString()];
        }

        //check or uncheck tree nodes
        private void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                e.Node.TreeView.BeginUpdate();
                if (e.Node.Nodes.Count > 0)
                {
                    var parentNode = e.Node;
                    var nodes = e.Node.Nodes;
                    CheckedOrUnCheckedNodes(parentNode, nodes);
                }
                else
                {
                    if ((e.Node.Checked) && (!lst_testcaseorder.Items.Contains(e.Node.Text)))
                    {
                        lst_testcaseorder.Items.Add(e.Node.Text);
                        int idx = dgv_testcaseorder.Rows.Add();
                        dgv_testcaseorder.Rows[idx].Cells["colTestCaseTitle"].Value = e.Node.Text;                    
                    }                        
                    else if ((!e.Node.Checked) && (lst_testcaseorder.Items.Contains(e.Node.Text)))
                    {
                        lst_testcaseorder.Items.Remove(e.Node.Text);
                        int rowIndex = -1;

                        DataGridViewRow row = dgv_testcaseorder.Rows
                            .Cast<DataGridViewRow>()
                            .Where(r => r.Cells["colTestCaseTitle"].Value.ToString().Equals(e.Node.Text))
                            .First();

                        rowIndex = row.Index;

                        dgv_testcaseorder.Rows.RemoveAt(rowIndex);
                    }
                    //refreshGrid(lst_testcaseorder);
                }
            }
            finally
            {
                e.Node.TreeView.EndUpdate();
            }
        }

        private void refreshGrid(ListBox lst_testcaseorder)
        {
            dgv_testcaseorder.Rows.Clear();
            foreach(var item in lst_testcaseorder.Items)
            {
                int idx=dgv_testcaseorder.Rows.Add();
                dgv_testcaseorder.Rows[idx].Cells["colTestCaseTitle"].Value = item;
            }
        }

        //check or un-check nodes (recursive method)
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

        //ignore
        private void cmb_groupby_selectionchanged(object sender, EventArgs e)
        {
            trv_testcaseids.Nodes.Clear();
            var treeviewDict = objLib.GetTestCaseIDsAndTitles()
                //.Where(x => objLib.GetTestDataIDs().ContainsKey(x.Key.ToString())).ToDictionary(x => x.Key, x => x.Value)
                                         .GroupBy(x => x.Value);

            ////--var treeviewDict = SQLQueries.GetTestCaseIDs.Where(x => SQLQueries.GetTestDataIDs.ContainsKey(x.Key.ToString())).Select(t => new { t.Key, t.Value }).ToDictionary(t => t.Key, t => t.Value);

            foreach (var item in treeviewDict)
            {
                TreeNode trNode = new TreeNode(item.Key);
                trNode.Name = item.Key;
                trv_testcaseids.Nodes.Add(trNode);

                var tcids = (from t in item select t.Key).ToArray();
                foreach (var i in tcids)
                    trNode.Nodes.Add(new TreeNode(i.ToString()));
            }
        }

        private void cmb_browsertype_IndexChanged(object sender, EventArgs e)
        {
            lbl_exestatus.Text = "";
            if (cmb_browser.SelectedItem.ToString()!="Remote")
            {
                for (int i = 0; i < dgv_testcaseorder.Rows.Count; i++)
                    this.dgv_testcaseorder.Rows[i].Cells["cmb_browser"].Value = cmb_browser.SelectedItem.ToString();
            }
            else
            {                
                //for (int i = 0; i < dgv_testcaseorder.Rows.Count; i++)
                //    this.dgv_testcaseorder.Rows[i].Cells["cmb_browser"].Value = "";
                //lbl_exestatus.Visible = true;
                lbl_exestatus.Text = "*Ensure Selenium Hub and Node(s) are Up and Running.";
            }
            
        }
    }
}
