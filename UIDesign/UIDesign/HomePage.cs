using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using System.Diagnostics;
using System.Reflection;
using VM.Platform.TestAutomationFramework.Core;
using System.Xml.Linq;
using System.Xml.XPath;
using UIDesign.common;

namespace UIDesign
{
    public partial class HomePage : Form
    {
        public Rectangle screen = Screen.PrimaryScreen.WorkingArea;
        private Label lbl_userid = new Label();
        private Label lbl_Blank = new Label();
        private Label lbl_Blank1 = new Label();
        private Label lbl_username = new Label();
        private Label lbl_projname = new Label();
        private Label lbl_useridvalue = new Label();
        private Label lbl_usernamevalue = new Label();
        private Label lbl_projnamevalue = new Label();
        private Label lbl_tags = new Label();
        private TextBox txt_tags = new TextBox();
        private LinkLabel lbl_help = new LinkLabel();
        private Label lbl_pagetitle = new Label();

        private Button btn_pagenames = new Button();
        private Button btn_Blank = new Button();
        private Button btn_keywords = new Button();
        private Button btn_masteror = new Button();
        private Button btn_testcase = new Button();
        private Button btn_testdata = new Button();
        private Button btn_massupdate = new Button();
        private Button btn_testexecution = new Button();
        private Button btn_testresult = new Button();
        private Button btn_testcloner = new Button();
        private Button btn_debug = new Button();
        private Button btn_v3migration = new Button();
        private Button btn_feature = new Button();
        private Button btn_Encrypt = new Button();
        private Button btn_ElementFinder = new Button();
        private Button btn_APITest = new Button();

        private Button btn_create = new Button();
        private Button btn_refresh = new Button();
        private Button btn_export = new Button();

        private ComboBox cmb_columnvalues = new ComboBox();
        private ComboBox cmb_columnnames = new ComboBox();

        private DataGridView grid_displayResult = new DataGridView();
        private DataGridViewLinkColumn btn_gridedit = new DataGridViewLinkColumn();
        private DataGridViewLinkColumn btn_griddel = new DataGridViewLinkColumn();
        //private DataGridViewLinkColumn btn_gridsnapshot = new DataGridViewLinkColumn();
        private BindingSource bindingSrc = new BindingSource();

        private PictureBox pic_gecko = new PictureBox();
        private TableLayoutPanel tlp_userinfo = new TableLayoutPanel();

        private TableLayoutPanel tlp_buttons = new TableLayoutPanel();
        private FlowLayoutPanel flp_buttons = new FlowLayoutPanel();
        private FlowLayoutPanel flp_homepage = new FlowLayoutPanel();
        private TableLayoutPanel tlp_homepage = new TableLayoutPanel();
        public StatusBar mainStatusBar = new StatusBar();
        public StatusBarPanel statusPanel = new StatusBarPanel();

        SqlDataAdapter da = new SqlDataAdapter();
        FuncLib objLib = new FuncLib();

        string[] datefilterOptions = new string[] { "All", "Today", "Last 24 hours", "Last 48 hours", "Last 7 days", "Last 14 days", "Last 28 days", "Last 90 days" };
        public static string SName;
        public HomePage()
        {
            InitializeComponent();

            #region homepage_design

            //Form Settings
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = SystemColors.Window;
            this.Text = "Home Page - v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.MaximizeBox = true;
            this.MinimizeBox = false;


            //FlowLayoutPanel - flp_homepage Settings
            flp_homepage.FlowDirection = FlowDirection.LeftToRight;
            flp_homepage.SetFlowBreak(tlp_userinfo, true);
            flp_homepage.Dock = DockStyle.Top;
            flp_homepage.AutoSize = true;

            //FlowLayoutPanel - flp_buttons Settings
            flp_buttons.FlowDirection = FlowDirection.LeftToRight;
            flp_buttons.Dock = DockStyle.Top;
            flp_buttons.AutoSize = true;


            //TableLayoutPanel - tlp_homepage Settings
            tlp_userinfo.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_userinfo.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_userinfo.AutoSize = true;

            tlp_buttons.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_buttons.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_buttons.AutoSize = true;
            tlp_buttons.Visible = true;

            //TableLayoutPanel - tlp_homepage Settings
            tlp_homepage.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_homepage.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_homepage.AutoSize = true;
            tlp_homepage.Visible = false;


            //Gecko Picture Box Settings
            pic_gecko.Height = 100;
            pic_gecko.Width = 140;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            //Userid Label settings           
            lbl_userid.Height = 24;
            lbl_userid.Width = 700;
            lbl_userid.Text = "User ID :";
            lbl_userid.Name = "lbl_userid";
            lbl_userid.TextAlign = ContentAlignment.BottomRight;
            lbl_userid.Font = new Font("Calibri", 11F, FontStyle.Bold);

            //Userid Label settings           
            lbl_username.Height = 24;
            lbl_username.Width = 700;
            lbl_username.Text = "User Name :";
            lbl_username.Name = "lbl_username";
            lbl_username.TextAlign = ContentAlignment.BottomRight;
            lbl_username.Font = new Font("Calibri", 11F, FontStyle.Bold);

            lbl_help.Height = 24;
            lbl_help.Width = 110;
            lbl_help.Text = "Help";
            lbl_help.TextAlign = ContentAlignment.BottomLeft;
            lbl_help.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_help.Margin = new Padding(0);

            lbl_tags.Height = 30;
            lbl_tags.Width = 100;
            lbl_tags.Text = "Tags :";
            lbl_tags.TextAlign = ContentAlignment.MiddleCenter;
            lbl_tags.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_tags.Visible = false;



            txt_tags.Height = 30;
            txt_tags.Width = 250;
            txt_tags.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_tags.Visible = false;
            //Project Label settings           
            lbl_projname.Height = 24;
            lbl_projname.Width = 700;
            lbl_projname.Text = "Project :";
            lbl_projname.Name = "lbl_projname";
            lbl_projname.TextAlign = ContentAlignment.BottomRight;
            lbl_projname.Font = new Font("Calibri", 11F, FontStyle.Bold);

            //Userid Label settings           
            lbl_useridvalue.Height = 24;
            lbl_useridvalue.Width = 200;
            lbl_useridvalue.Text = SignIn.userId;
            lbl_useridvalue.Name = "lbl_useridvalue";
            lbl_useridvalue.TextAlign = ContentAlignment.BottomRight;
            lbl_useridvalue.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_useridvalue.ForeColor = Color.Blue;
            //label Blank
            lbl_Blank.Text = "";
            lbl_Blank.Name = "lbl_Blank";
            lbl_Blank.TextAlign = ContentAlignment.MiddleLeft;
            lbl_Blank.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_Blank.Height = 24;
            lbl_Blank.Width = 103;

            //label Blank1
            lbl_Blank1.Text = "";
            lbl_Blank1.Name = "lbl_Blank1";
            lbl_Blank1.TextAlign = ContentAlignment.MiddleLeft;
            lbl_Blank1.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_Blank1.Height = 24;
            lbl_Blank1.Width = 300;
            //Userid Label settings           
            lbl_usernamevalue.Height = 24;
            lbl_usernamevalue.Width = 200;
            lbl_usernamevalue.Text = objLib.GetUserInfo().Single(x => x.Key == SignIn.userId).Value;
            lbl_usernamevalue.Name = "lbl_usernamevalue";
            lbl_usernamevalue.TextAlign = ContentAlignment.BottomRight;
            lbl_usernamevalue.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_usernamevalue.ForeColor = Color.Blue;

            //Project Label settings           
            lbl_projnamevalue.Height = 24;
            lbl_projnamevalue.Width = 200;
            lbl_projnamevalue.Text = SignIn.projectName;
            lbl_projnamevalue.Name = "lbl_projname";
            lbl_projnamevalue.TextAlign = ContentAlignment.BottomRight;
            lbl_projnamevalue.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_projnamevalue.ForeColor = Color.Blue;

            //Page Title Label settings           
            lbl_pagetitle.Height = 30;
            lbl_pagetitle.Width = 363;
            lbl_pagetitle.Text = "";
            lbl_pagetitle.Name = "lbl_pagetitle";
            lbl_pagetitle.TextAlign = ContentAlignment.TopLeft;
            lbl_pagetitle.Font = new Font("Calibri", 20F, FontStyle.Bold);
            lbl_pagetitle.ForeColor = Color.Blue;

            //Page Names button settings
            btn_pagenames.Text = "Page Names";
            btn_pagenames.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_pagenames.TextAlign = ContentAlignment.MiddleCenter;
            btn_pagenames.FlatStyle = FlatStyle.Flat;
            btn_pagenames.FlatAppearance.BorderSize = 1;
            btn_pagenames.Dock = DockStyle.Fill;
            btn_pagenames.BackColor = Color.LightBlue;
            btn_pagenames.Height = 32;
            btn_pagenames.Width = 115;
            btn_pagenames.Margin = new Padding(0);

            //Keywords button settings 
            btn_keywords.Text = "Keywords";
            btn_keywords.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_keywords.TextAlign = ContentAlignment.MiddleCenter;
            btn_keywords.FlatAppearance.BorderSize = 1;
            btn_keywords.FlatStyle = FlatStyle.Flat;
            btn_keywords.Dock = DockStyle.Fill;
            btn_keywords.BackColor = Color.LightBlue;
            btn_keywords.Height = 32;
            btn_keywords.Width = 115;
            btn_keywords.Margin = new Padding(0);

            //Master OR button settings
            btn_masteror.Text = "Object Repo";
            btn_masteror.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_masteror.TextAlign = ContentAlignment.MiddleCenter;
            btn_masteror.FlatAppearance.BorderSize = 1;
            btn_masteror.FlatStyle = FlatStyle.Flat;
            btn_masteror.BackColor = Color.LightBlue;
            btn_masteror.Height = 32;
            btn_masteror.Width = 115;
            btn_masteror.Margin = new Padding(0);

            //Test Case button settings
            btn_testcase.Text = "Test Case";
            btn_testcase.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_testcase.TextAlign = ContentAlignment.MiddleCenter;
            btn_testcase.FlatStyle = FlatStyle.Flat;
            btn_testcase.FlatAppearance.BorderSize = 1;
            btn_testcase.Margin = new Padding(0);
            btn_testcase.BackColor = Color.LightBlue;
            btn_testcase.Height = 32;
            btn_testcase.Width = 115;

            //Test Data button settings
            btn_testdata.Text = "Test Data";
            btn_testdata.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_testdata.TextAlign = ContentAlignment.MiddleCenter;
            btn_testdata.FlatStyle = FlatStyle.Flat;
            btn_testdata.BackColor = Color.LightBlue;
            btn_testdata.FlatAppearance.BorderSize = 1;
            btn_testdata.Margin = new Padding(0);
            btn_testdata.Height = 32;
            btn_testdata.Width = 115;

            //Mass Update button settings
            btn_massupdate.Text = "Mass Update";
            btn_massupdate.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_massupdate.TextAlign = ContentAlignment.MiddleCenter;
            btn_massupdate.FlatStyle = FlatStyle.Flat;
            btn_massupdate.BackColor = Color.LightBlue;
            btn_massupdate.Margin = new Padding(0);
            btn_massupdate.FlatAppearance.BorderSize = 1;
            btn_massupdate.Height = 32;
            btn_massupdate.Width = 115;

            //Mass Update button settings
            btn_testexecution.Text = "Test Execution";
            btn_testexecution.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_testexecution.TextAlign = ContentAlignment.MiddleCenter;
            btn_testexecution.FlatStyle = FlatStyle.Flat;
            btn_testexecution.BackColor = Color.LightBlue;
            btn_testexecution.Margin = new Padding(0);
            btn_testexecution.FlatAppearance.BorderSize = 1;
            btn_testexecution.Height = 32;
            btn_testexecution.Width = 115;
            //Encrypt button settings
            btn_Encrypt.Text = "Encrypt";
            btn_Encrypt.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_Encrypt.TextAlign = ContentAlignment.MiddleCenter;
            btn_Encrypt.FlatStyle = FlatStyle.Flat;
            btn_Encrypt.BackColor = Color.LightBlue;
            btn_Encrypt.Margin = new Padding(0);
            btn_Encrypt.FlatAppearance.BorderSize = 1;
            btn_Encrypt.Height = 32;
            btn_Encrypt.Width = 115;
            //Element Finder button settings
            btn_ElementFinder.Text = "Object Spy";
            btn_ElementFinder.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_ElementFinder.TextAlign = ContentAlignment.MiddleCenter;
            btn_ElementFinder.FlatStyle = FlatStyle.Flat;
            btn_ElementFinder.BackColor = Color.LightBlue;
            btn_ElementFinder.Margin = new Padding(0);
            btn_ElementFinder.FlatAppearance.BorderSize = 1;
            btn_ElementFinder.Height = 32;
            btn_ElementFinder.Width = 115;
            //Test Cloner button settings
            btn_testcloner.Text = "Test Cloner";
            btn_testcloner.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_testcloner.TextAlign = ContentAlignment.MiddleCenter;
            btn_testcloner.FlatStyle = FlatStyle.Flat;
            btn_testcloner.BackColor = Color.LightBlue;
            btn_testcloner.Margin = new Padding(0);
            btn_testcloner.FlatAppearance.BorderSize = 1;
            btn_testcloner.Height = 32;
            btn_testcloner.Width = 115;
            //Debug button settings
            btn_debug.Text = "Debug";
            btn_debug.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_debug.TextAlign = ContentAlignment.MiddleCenter;
            btn_debug.FlatStyle = FlatStyle.Flat;
            btn_debug.BackColor = Color.LightBlue;
            btn_debug.Margin = new Padding(0);
            btn_debug.FlatAppearance.BorderSize = 1;
            btn_debug.Height = 32;
            btn_debug.Width = 115;
            //Test Results button settings
            btn_testresult.Text = "Test Results";
            btn_testresult.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_testresult.TextAlign = ContentAlignment.MiddleCenter;
            btn_testresult.FlatStyle = FlatStyle.Flat;
            btn_testresult.BackColor = Color.LightBlue;
            btn_testresult.Margin = new Padding(0);
            btn_testresult.FlatAppearance.BorderSize = 1;
            btn_testresult.Height = 32;
            btn_testresult.Width = 115;

            //Test Results button settings
            btn_v3migration.Text = "v3 Migration";
            btn_v3migration.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_v3migration.TextAlign = ContentAlignment.MiddleCenter;
            btn_v3migration.FlatStyle = FlatStyle.Flat;
            btn_v3migration.BackColor = Color.LightBlue;
            btn_v3migration.FlatAppearance.BorderSize = 1;
            btn_v3migration.Margin = new Padding(0);
            btn_v3migration.Height = 32;
            btn_v3migration.Width = 115;
            //Features button settings
            btn_feature.Text = "Features";
            btn_feature.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_feature.TextAlign = ContentAlignment.MiddleCenter;
            btn_feature.FlatStyle = FlatStyle.Flat;
            btn_feature.BackColor = Color.LightBlue;
            btn_feature.FlatAppearance.BorderSize = 1;
            btn_feature.Margin = new Padding(0);
            btn_feature.Dock = DockStyle.Fill;
            btn_feature.Height = 32;
            btn_feature.Width = 115;
            //API Test button settings
            btn_APITest.Text = "API Test";
            btn_APITest.Font = new Font("Calibri", 11F, FontStyle.Bold);
            btn_APITest.TextAlign = ContentAlignment.MiddleCenter;
            btn_APITest.FlatStyle = FlatStyle.Flat;
            btn_APITest.BackColor = Color.LightBlue;
            btn_APITest.FlatAppearance.BorderSize = 1;
            btn_APITest.Margin = new Padding(0);
            btn_APITest.Height = 32;
            btn_APITest.Width = 115;

            //Create button settings
            btn_create.Text = "Create";
            btn_create.Font = new Font("Calibri", 9.8F, FontStyle.Bold);
            btn_create.Height = 30;
            btn_create.Width = 110;

            //export button settings
            btn_export.Text = "Export";
            btn_export.Font = new Font("Calibri", 9.8F, FontStyle.Bold);
            btn_export.Height = 30;
            btn_export.Width = 110;

            //Refresh button settings
            btn_refresh.Text = "Refresh";
            btn_refresh.Font = new Font("Calibri", 9.8F, FontStyle.Bold);
            btn_refresh.Height = 30;
            btn_refresh.Width = 110;

            cmb_columnnames.Height = 40;
            cmb_columnnames.Width = 225;
            cmb_columnnames.Font = new Font("Calibri", 11, FontStyle.Regular);
            cmb_columnnames.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_columnnames.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_columnnames.AutoCompleteMode = AutoCompleteMode.SuggestAppend;


            cmb_columnvalues.Height = 40;
            cmb_columnvalues.Width = 300;
            cmb_columnvalues.Font = new Font("Calibri", 11, FontStyle.Regular);
            cmb_columnvalues.Enabled = false;
            cmb_columnvalues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_columnvalues.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_columnvalues.AutoCompleteMode = AutoCompleteMode.SuggestAppend;



            tlp_homepage.Controls.Add(lbl_Blank, 0, 2);
            tlp_homepage.Controls.Add(lbl_pagetitle, 3, 2);
            tlp_homepage.SetColumnSpan(lbl_pagetitle, 2);
            tlp_homepage.Controls.Add(btn_create, 6, 2);
            tlp_homepage.Controls.Add(btn_refresh, 7, 2);
            tlp_homepage.Controls.Add(btn_export, 8, 2);
            tlp_homepage.Controls.Add(cmb_columnnames, 9, 2);
            tlp_homepage.Controls.Add(cmb_columnvalues, 10, 2);
            tlp_homepage.Controls.Add(lbl_tags, 11, 2);
            tlp_homepage.Controls.Add(txt_tags, 12, 2);

            grid_displayResult.ReadOnly = true;
            grid_displayResult.Size = new Size(1240, 485);
            grid_displayResult.DataSource = bindingSrc;
            grid_displayResult.Dock = DockStyle.None;
            grid_displayResult.ScrollBars = ScrollBars.Both;
            grid_displayResult.DefaultCellStyle.Font = new Font("Calibri", 10);
            grid_displayResult.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
            grid_displayResult.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8.9F, FontStyle.Bold);
            grid_displayResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;

            //Adding Edit Column in Data Grid View            
            btn_gridedit.HeaderText = "";
            btn_gridedit.Text = "Edit";
            btn_gridedit.Name = "btnClickMeforEdit";
            btn_gridedit.Width = 40;
            btn_gridedit.UseColumnTextForLinkValue = true;
            grid_displayResult.Columns.Add(btn_gridedit);

            //Adding Edit Column in Data Grid View            
            btn_griddel.HeaderText = "";
            btn_griddel.Text = "Del";
            btn_griddel.Name = "btnClickMeforDelete";
            btn_griddel.Width = 40;
            btn_griddel.UseColumnTextForLinkValue = true;
            grid_displayResult.Columns.Add(btn_griddel);

            //Adding Snapshot Column in Data Grid View            
            //btn_gridsnapshot.HeaderText = "";
            //btn_gridsnapshot.Text = "Snapshot";
            //btn_gridsnapshot.Name = "btnGridSnapshot";
            //btn_gridsnapshot.Width = 80;
            //btn_gridsnapshot.UseColumnTextForLinkValue = true;
            //grid_displayResult.Columns.Add(btn_gridsnapshot);
            // Add StatusBar to home screen
            statusPanel.BorderStyle = StatusBarPanelBorderStyle.Sunken;
            statusPanel.Text = "CONNECTED TO : " + this.GetDBPath().ToUpper();
            statusPanel.AutoSize = StatusBarPanelAutoSize.Spring;
            mainStatusBar.Panels.Add(statusPanel);
            mainStatusBar.ShowPanels = true;
            mainStatusBar.Dock = DockStyle.Bottom;
            mainStatusBar.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            //Adding Controls to Table Layout Panel
            tlp_userinfo.Controls.Add(pic_gecko, 0, 1);
            tlp_userinfo.SetRowSpan(pic_gecko, 30);
            tlp_userinfo.SetColumnSpan(pic_gecko, 1);
            tlp_userinfo.Controls.Add(lbl_Blank1, 2, 10);
            tlp_userinfo.Controls.Add(lbl_projname, 3, 10);
            tlp_userinfo.Controls.Add(lbl_projnamevalue, 4, 10);
            tlp_userinfo.Controls.Add(lbl_userid, 3, 12);
            tlp_userinfo.Controls.Add(lbl_useridvalue, 4, 12);
            tlp_userinfo.Controls.Add(lbl_username, 3, 14);
            tlp_userinfo.Controls.Add(lbl_usernamevalue, 4, 14);

            //Adding Controls to Table Layout Panel--tlp_buttons

            tlp_buttons.Controls.Add(btn_pagenames, 0, 1);
            tlp_buttons.Controls.Add(btn_keywords, 0, 2);
            tlp_buttons.Controls.Add(btn_feature, 0, 3);
            tlp_buttons.Controls.Add(btn_masteror, 0, 4);
            tlp_buttons.Controls.Add(btn_ElementFinder, 0, 5);
            tlp_buttons.Controls.Add(btn_testcase, 0, 6);
            tlp_buttons.Controls.Add(btn_testdata, 0, 7);
            tlp_buttons.Controls.Add(btn_testexecution, 0, 8);
            tlp_buttons.Controls.Add(btn_testresult, 0, 9);
            tlp_buttons.Controls.Add(btn_debug, 0, 10);
            tlp_buttons.Controls.Add(btn_Encrypt, 0, 11);
            tlp_buttons.Controls.Add(btn_massupdate, 0, 12);
            tlp_buttons.Controls.Add(btn_testcloner, 0, 13);
            tlp_buttons.Controls.Add(btn_v3migration, 0, 14);
            tlp_buttons.Controls.Add(btn_APITest, 0, 15);
            tlp_buttons.Controls.Add(lbl_help, 0, 16);
            tlp_buttons.Controls.Add(grid_displayResult, 3, 1);

            tlp_buttons.SetColumnSpan(grid_displayResult, 12);
            tlp_buttons.SetRowSpan(grid_displayResult, 30);
            //Adding Controls to Flow Layout Panel
            flp_homepage.Controls.AddRange(new Control[] { tlp_userinfo, tlp_homepage });

            flp_buttons.Controls.AddRange(new Control[] { tlp_buttons });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_buttons, flp_homepage, mainStatusBar });



            #endregion

            #region homepage_methods
            btn_pagenames.Click += new System.EventHandler(btn_pagenames_Click);
            btn_keywords.Click += new System.EventHandler(btn_keywords_Click);
            btn_masteror.Click += new System.EventHandler(btn_masteror_Click);
            btn_testcase.Click += new System.EventHandler(btn_testcase_Click);
            btn_testdata.Click += new System.EventHandler(btn_testdata_Click);
            btn_testresult.Click += new System.EventHandler(btn_testresult_Click);
            btn_massupdate.Click += new System.EventHandler(btn_massupdate_Click);
            btn_testexecution.Click += new System.EventHandler(btn_testexecution_Click);
            btn_testcloner.Click += new System.EventHandler(btn_testcloner_Click);
            btn_debug.Click += new System.EventHandler(btn_debug_Click);
            btn_v3migration.Click += new System.EventHandler(btn_migration_Click);
            btn_APITest.Click += new System.EventHandler(btn_APITest_Click);
            btn_feature.Click += new System.EventHandler(btn_feature_Click);
            btn_Encrypt.Click += new System.EventHandler(btn_Encrypt_Click);
            btn_ElementFinder.Click += new System.EventHandler(btn_ElementFinder_Click);
            btn_create.Click += new System.EventHandler(btn_create_Click);
            btn_refresh.Click += new System.EventHandler(btn_refresh_Click);
            btn_export.Click += new System.EventHandler(btn_export_Click);
            cmb_columnnames.SelectedIndexChanged += new System.EventHandler(columnname_selectionChanged);
            cmb_columnvalues.SelectedIndexChanged += new System.EventHandler(columnvalue_selectionChanged);
            grid_displayResult.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(displayResult_editClick);
            lbl_help.Click += new System.EventHandler(lbl_help_Click);
            txt_tags.Leave += new System.EventHandler(txt_tags_Leave);
            this.FormClosing += new FormClosingEventHandler(homepage_closing);

            #endregion
        }
        private void homepage_closing(object sender, FormClosingEventArgs e)
        {
            if (Application.OpenForms.OfType<TestExecution>().Count() != 0)
                e.Cancel = true;
        }
        private void HomePage_Load(object sender, EventArgs e)
        {
            SName = ScreenName.PageName.ToString();
            BindPageNames();

            float widthRatio = Screen.PrimaryScreen.Bounds.Width / 1364f;
            float heightRatio = Screen.PrimaryScreen.Bounds.Height / 768f;

            SizeF scale = new SizeF(widthRatio, heightRatio);
            this.Scale(scale);
            foreach (Control control in this.Controls)
            {
                control.Font = new Font("Verdana", control.Font.SizeInPoints * heightRatio * widthRatio);
            }

        }
        private void lbl_help_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, @".\\help\v4 User Manual.chm");
        }
        private void btn_pagenames_Click(object sender, EventArgs e)
        {
            SName = ScreenName.PageName.ToString();
            BindPageNames();
        }
        private void btn_keywords_Click(object sender, System.EventArgs e)
        {
            SName = ScreenName.KeyWords.ToString();
            BindKeywords();
        }
        private void btn_masteror_Click(object sender, System.EventArgs e)
        {
            SName = ScreenName.MasterOr.ToString();
            BindMasterOR();
        }
        private void btn_testcase_Click(object sender, System.EventArgs e)
        {
            SName = ScreenName.TestCase.ToString();
            BindTestCase();
        }
        private void btn_testdata_Click(object sender, System.EventArgs e)
        {
            SName = ScreenName.TestData.ToString();
            BindTestData();
        }
        private void btn_testresult_Click(object sender, System.EventArgs e)
        {
            SName = ScreenName.TestResult.ToString();
            BindTestResults();
        }
        private void btn_feature_Click(object sender, System.EventArgs e)
        {
            var Admin = objLib.GetAdminUsers().Where(x => ((x.Value.ToLower() == "yes") || (x.Value.ToLower() == "superuser")) && x.Key.ToUpper() == SignIn.userId.ToUpper()).Select(x => x.Key).ToArray();
            SName = ScreenName.Features.ToString();
            BindFeatures();
        }
        private void btn_massupdate_Click(object sender, System.EventArgs e)
        {

            var Admin = objLib.GetAdminUsers().Where(x => ((x.Value.ToLower() == "yes")) && x.Key.ToUpper() == SignIn.userId.ToUpper()).Select(x => x.Key).ToArray();

            if (Admin.Length != 0)
            {
                MassUpdate frmMassUpdate = new MassUpdate();
                frmMassUpdate.ShowDialog();
            }
            else
            {
                MessageBox.Show("Sorry...You don't have rights to perform Mass Update.", "Mass Update", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void btn_testexecution_Click(object sender, System.EventArgs e)
        {

            if (Application.OpenForms.OfType<TestExecution>().Count() != 0)
            {
                Application.OpenForms.OfType<TestExecution>().First().WindowState = FormWindowState.Normal;
                Application.OpenForms.OfType<TestExecution>().First().StartPosition = FormStartPosition.CenterScreen;

            }
            else
            {
                TestExecution frmTestExecution = new TestExecution();
                frmTestExecution.StartPosition = FormStartPosition.CenterScreen;
                frmTestExecution.Show();
            }
        }

        private void btn_testcloner_Click(object sender, System.EventArgs e)
        {
            SName = ScreenName.TestClone.ToString();
            TestCloner frmTestCloner = new TestCloner();
            frmTestCloner.ShowDialog();
        }
        public void btn_debug_Click(object sender, System.EventArgs e)
        {
            DebugScreen frmDebug = new DebugScreen();
            frmDebug.Show();
        }
        private void btn_ElementFinder_Click(object sender, System.EventArgs e)
        {
            ElementFinder frmElementFinder = new ElementFinder();
            frmElementFinder.Show();
        }
        private void btn_Encrypt_Click(object sender, System.EventArgs e)
        {
            Encrypt frmEncrypt = new Encrypt();
            frmEncrypt.ShowDialog();
        }
        private void btn_migration_Click(object sender, System.EventArgs e)
        {
            v3Migration frmMigration = new v3Migration();
            frmMigration.ShowDialog();
        }
        private void btn_APITest_Click(object sender, System.EventArgs e)
        {
            APITest apiTest = new APITest();
            apiTest.ShowDialog();
        }
        private void btn_create_Click(object sender, EventArgs e)
        {

            var Admin = objLib.GetAdminUsers().Where(x => ((x.Value.ToLower() == "yes") || (x.Value.ToLower() == "superuser")) && x.Key.ToUpper() == SignIn.userId.ToUpper()).Select(x => x.Key).ToArray();

            switch (SName)
            {
                case "PageName":
                    PageNames objPageNames = new PageNames();
                    //objPageNames.lbl_lastupdatedbydateandtime.Text = objLib.ToCamelCase(objLib.GetUserInfo().Single(x => x.Key == SignIn.userId).Value) + "\n\n" + DateTime.Now;
                    objPageNames.ShowDialog();
                    break;
                case "KeyWords":
                    MessageBox.Show("Sorry...Keywords Can't be CREATED", "Keyword Create", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
                case "MasterOr":
                    if (Admin.Length != 0)
                    {
                        ObjectRepositiory objOR = new ObjectRepositiory();
                        objOR.lbl_pagetitle.Text = "CREATE OR ENTRY";
                        objOR.btn_saveorentry.Visible = false;
                        objOR.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Sorry...You don't have rights to CREATE OR Entries.", "OR Create", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                case "TestCase":
                    TestCase objTestCase = new TestCase();
                    objTestCase.btn_savetestcase.Visible = false;
                    objTestCase.ShowDialog();
                    break;
                case "TestData":
                    TestData frmTestData = new TestData();
                    frmTestData.btn_savetestdata.Visible = false;
                    frmTestData.ShowDialog();
                    break;
                case "TestResult":
                    MessageBox.Show("Sorry...Test Results Can't be CREATED", "Test Results Create", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    break;
                case "Features":
                    if (Admin.Length != 0)
                    {
                        AddFeature frmAddFeature = new AddFeature();
                        frmAddFeature.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Sorry...You don't have rights to create feature.", "Add Feature", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    }
                    break;
                default:
                    break;
            }
        }
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            switch (SName)
            {
                case "PageName":
                    BindPageNames();
                    break;
                case "KeyWords":
                    BindKeywords();
                    break;
                case "MasterOr":
                    BindMasterOR();
                    break;
                case "TestCase":
                    BindTestCase();
                    break;
                case "TestData":
                    BindTestData();
                    break;
                case "TestResult":
                    BindTestResults();
                    break;
                case "Features":
                    BindFeatures();
                    break;
                default:
                    break;
            }
        }
        private void btn_export_Click(object sender, EventArgs e)
        {
            try
            {
                var bindingsrc = (BindingSource)grid_displayResult.DataSource;
                var dtable = (DataTable)bindingsrc.DataSource;
                XLWorkbook wb = new XLWorkbook();
                wb.Worksheets.Add(dtable, "Data");
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                MemoryStream MyMemoryStream = new MemoryStream();
                wb.SaveAs(MyMemoryStream);

                FileStream file = new FileStream("exportresults.xlsx", FileMode.Create, FileAccess.Write);
                MyMemoryStream.WriteTo(file);
                file.Close();
                MyMemoryStream.Close();
                MessageBox.Show("Exporting...COMPLETED", "Export", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR : " + ex.Message, "Export", MessageBoxButtons.OK);
            }
        }
        private void columnname_selectionChanged(object sender, EventArgs e)
        {
            FilterGridResultByColName(da.SelectCommand.CommandText, cmb_columnnames.SelectedItem.ToString());
        }
        private void columnvalue_selectionChanged(object sender, EventArgs e)
        {
            FilterGridResultByColNameWithValue(da.SelectCommand.CommandText, cmb_columnnames.SelectedItem.ToString(), cmb_columnvalues.Text.ToString());
        }
        private void displayResult_editClick(object sender, DataGridViewCellEventArgs e)
        {
            var Admin = objLib.GetAdminUsers().Where(x => ((x.Value.ToLower() == "yes") || (x.Value.ToLower() == "superuser")) && x.Key.ToUpper() == SignIn.userId.ToUpper()).Select(x => x.Key).ToArray();

            if ((e.ColumnIndex == 0) && (Convert.ToString(grid_displayResult.Rows[e.RowIndex].Cells[0].Value) == "Edit"))
            {
                switch (SName)
                {
                    case "PageName":
                        PageNames pg = new PageNames();
                        pg.txt_pagename.Text = grid_displayResult.Rows[e.RowIndex].Cells["PageName"].Value.ToString();
                        pg.txt_tags.Text = grid_displayResult.Rows[e.RowIndex].Cells["Tags"].Value.ToString();
                        pg.txt_pagename.Enabled = false;
                        pg.btn_createpage.Text = "Save";
                        pg.ShowDialog();
                        break;
                    case "Features":
                        MessageBox.Show("Sorry...Feature Name Can't be EDITED.", "Edit Feature", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        break;
                    case "KeyWords":
                        MessageBox.Show("Sorry...Keywords Can't be EDITED.", "Keyword Edit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        break;
                    case "MasterOr":
                        if (Admin.Length != 0)
                        {
                            ObjectRepositiory or = new ObjectRepositiory();
                            or.btn_createorentry.Visible = false;
                            or.txt_masterorid.Text = grid_displayResult.Rows[e.RowIndex].Cells["MasterORID"].Value.ToString();
                            or.cmb_pagename.Text = grid_displayResult.Rows[e.RowIndex].Cells["PageName"].Value.ToString();
                            or.txt_label.Text = grid_displayResult.Rows[e.RowIndex].Cells["Label"].Value.ToString();
                            or.txt_controltype.Text = grid_displayResult.Rows[e.RowIndex].Cells["ControlType"].Value.ToString();
                            or.txt_tagname.Text = grid_displayResult.Rows[e.RowIndex].Cells["TagName"].Value.ToString();
                            or.txt_controlid.Text = grid_displayResult.Rows[e.RowIndex].Cells["ControlID"].Value.ToString();
                            or.txt_labelfor.Text = grid_displayResult.Rows[e.RowIndex].Cells["LabelFor"].Value.ToString();
                            or.txt_xpath.Text = grid_displayResult.Rows[e.RowIndex].Cells["Xpath"].Value.ToString();
                            or.txt_classname.Text = grid_displayResult.Rows[e.RowIndex].Cells["ClassName"].Value.ToString();
                            or.txt_innertext.Text = grid_displayResult.Rows[e.RowIndex].Cells["InnerText"].Value.ToString();
                            or.txt_class.Text = grid_displayResult.Rows[e.RowIndex].Cells["Class"].Value.ToString();
                            or.txt_type.Text = grid_displayResult.Rows[e.RowIndex].Cells["Type"].Value.ToString();
                            or.txt_parentcontrol.Text = grid_displayResult.Rows[e.RowIndex].Cells["ParentControl"].Value.ToString();
                            or.txt_friendlyname.Text = grid_displayResult.Rows[e.RowIndex].Cells["FriendlyName"].Value.ToString();
                            or.txt_valueattribute.Text = grid_displayResult.Rows[e.RowIndex].Cells["ValueAttribute"].Value.ToString();
                            or.txt_taginstance.Text = grid_displayResult.Rows[e.RowIndex].Cells["TagInstance"].Value.ToString();
                            or.txt_ctrldefinition.Text = grid_displayResult.Rows[e.RowIndex].Cells["ControlDefinition"].Value.ToString();
                            or.txt_repositiory.Text = grid_displayResult.Rows[e.RowIndex].Cells["Version"].Value.ToString();
                            or.txt_Imageelement.Text = grid_displayResult.Rows[e.RowIndex].Cells["ImagePath"].Value.ToString();
                            or.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Sorry...You don't have rights to EDIT feature name.", "Edit Feature", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        break;
                    case "TestCase":
                    case "ActionFlow":
                        TestCase tc = new TestCase();
                        tc.btn_createtestcase.Visible = false;
                        tc.lnk_edittestdata.Visible = true;
                        tc.txt_testcaseid.Enabled = false;
                        tc.cmb_designedby.Enabled = false;
                        tc.txt_testcaseid.Text = grid_displayResult.Rows[e.RowIndex].Cells["TestCaseID"].Value.ToString();
                        tc.txt_testcasetags.Text = grid_displayResult.Rows[e.RowIndex].Cells["Tags"].Value.ToString();
                        tc.txt_testcasetitle.Text = grid_displayResult.Rows[e.RowIndex].Cells["TestCaseTitle"].Value.ToString();
                        tc.txt_release.Text = grid_displayResult.Rows[e.RowIndex].Cells["Release"].Value.ToString();
                        tc.txt_testcasesummary.Text = grid_displayResult.Rows[e.RowIndex].Cells["TestCaseSummary"].Value.ToString();
                        //tc.txt_testcasefun.Text = grid_displayResult.Rows[e.RowIndex].Cells["Functionality"].Value.ToString();
                        tc.cmb_testcasefun.SelectedItem = grid_displayResult.Rows[e.RowIndex].Cells["Functionality"].Value.ToString();
                        tc.cmb_assignedto.SelectedItem = grid_displayResult.Rows[e.RowIndex].Cells["AssignedTo"].Value.ToString();
                        tc.cmb_designedby.SelectedItem = grid_displayResult.Rows[e.RowIndex].Cells["DesignedBy"].Value.ToString();
                        tc.cmb_designstatus.SelectedItem = grid_displayResult.Rows[e.RowIndex].Cells["State"].Value.ToString();
                        tc.cmb_testcategory.SelectedItem = grid_displayResult.Rows[e.RowIndex].Cells["TestCategory"].Value.ToString();
                        tc.cmb_priority.SelectedItem = grid_displayResult.Rows[e.RowIndex].Cells["Priority"].Value.ToString();
                        tc.txt_jira.Text = grid_displayResult.Rows[e.RowIndex].Cells["Jira"].Value.ToString();
                        tc.txt_TCReferenceId.Text = grid_displayResult.Rows[e.RowIndex].Cells["TestCase Ref"].Value.ToString();
                        //tc.txt_TCReferenceId.Enabled = false;

                        DataTable dt = new DataTable();
                        //dt = objLib.binddataTable("SELECT * FROM ACTIONFLOW WHERE TESTCASEID=" + tc.txt_testcaseid.Text.Trim() + " AND ProjectID=" + SignIn.projectId + " ORDER BY SEQNUMBER");

                        dt = objLib.binddataTable("SELECT ActionFlow_id,Pg.PageName,ActionFlow,FlowIdentifier,SeqNumber FROM ActionFlow AF inner join PageNames PG on AF.PageID = PG.PageID WHERE TESTCASEID=" + tc.txt_testcaseid.Text.Trim() + " AND AF.ProjectID=" + SignIn.projectId + " ORDER BY SEQNUMBER");

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            tc.grid_testcase.Rows.Add();
                            tc.grid_testcase.Rows[i].Cells["colActionFlowID"].Value = dt.Rows[i]["ActionFlow_id"].ToString();
                            tc.grid_testcase.Rows[i].Cells["cmb_PageName"].Value = dt.Rows[i]["PageName"].ToString();
                            tc.grid_testcase.Rows[i].Cells["colActionFlow"].Value = dt.Rows[i]["ActionFlow"].ToString();
                            tc.grid_testcase.Rows[i].Cells["colFlowIdentifier"].Value = dt.Rows[i]["FlowIdentifier"].ToString();
                            tc.grid_testcase.Rows[i].Cells["colSeqNumber"].Value = dt.Rows[i]["SeqNumber"].ToString();
                        }
                        tc.grid_testcase.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                        tc.ShowDialog();
                        break;
                    case "TestData":
                        TestData td = new TestData();
                        td.btn_createtestdata.Visible = false;
                        td.tlp_testdata.Enabled = false;
                        DataTable dtTestInfo = new DataTable();
                        dtTestInfo = objLib.binddataTable("SELECT * FROM TESTCASEINFO WHERE TESTCASEID=" + grid_displayResult.Rows[e.RowIndex].Cells["TestCaseID"].Value.ToString() + " AND ProjectID=" + SignIn.projectId);
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
                        dtTestData = objLib.binddataTable("SELECT * FROM TESTDATAVIEW WHERE TESTCASEID=" + grid_displayResult.Rows[e.RowIndex].Cells["TestCaseID"].Value.ToString() + " AND ProjectID=" + SignIn.projectId + " ORDER BY SEQNUMBER");
                        for (int i = 0; i < dtTestData.Rows.Count; i++)
                        {
                            td.grid_testdata.Rows.Add();
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
                        td.ShowDialog();
                        break;
                    case "TestResult":
                        MessageBox.Show("Sorry...Test Results Can't be EDITED.", "Test Results Edit", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        break;
                    default:
                        break;
                }
            }
            else if ((e.ColumnIndex == 0) && (Convert.ToString(grid_displayResult.Rows[e.RowIndex].Cells[0].Value) == "View"))
            {
                switch (SName)
                {
                    case "TestResult":
                        try
                        {
                            string serverpath = objLib.testresultsPath;
                            string projectfolder = SignIn.projectName;
                            string testcaseId = grid_displayResult.Rows[e.RowIndex].Cells["TestCaseID"].Value.ToString();
                            //string userId = objLib.GetUserInfo().First(x => x.Value == grid_displayResult.Rows[e.RowIndex].Cells["RunBy"].Value.ToString()).Key;
                            //string dateOfexe = DateTime.Parse(grid_displayResult.Rows[e.RowIndex].Cells["DateOfExe"].Value.ToString()).ToUniversalTime().ToString("yyyy-MM-dd_HH-mm-ss");
                            string dateOfexe = DateTime.Parse(grid_displayResult.Rows[e.RowIndex].Cells["DateOfExe"].Value.ToString()).ToString("yyyy-MM-dd_HH-mm-ss");
                            try
                            {
                                //var resultfilesFromLocal = Directory.GetFiles(@".\\TestResults\\" + projectfolder + "\\" + testcaseId + "\\" + userId + "_" + dateOfexe + "\\");
                                var resultfilesFromLocal = Directory.GetFiles(@".\\TestResults\\" + projectfolder + "\\" + testcaseId + "\\" + "VMQApractice" + "_" + dateOfexe + "\\");
                                if (resultfilesFromLocal.Length != 0)
                                {
                                    foreach (var file in resultfilesFromLocal)
                                    {
                                        Process p = new Process();
                                        ProcessStartInfo pi = new ProcessStartInfo();
                                        pi.UseShellExecute = true;
                                        pi.FileName = file;
                                        p.StartInfo = pi;

                                        try
                                        {
                                            p.Start();
                                        }
                                        catch (Exception Ex)
                                        {
                                            //MessageBox.Show(Ex.Message);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                try
                                {
                                    //var resultfilesFromServer = Directory.GetFiles(serverpath + "\\" + projectfolder + "\\" + testcaseId + "\\" + userId + "_" + dateOfexe + "\\");
                                    var resultfilesFromServer = Directory.GetFiles(serverpath + "\\" + projectfolder + "\\" + testcaseId + "\\" + "VMQApractice" + "_" + dateOfexe + "\\");
                                    //var result1 = System.IO.Path.GetDirectoryName(Application.ExecutablePath + "\\" + "VMQApractice_ConsolidatedReport" + dateOfexe);
                                    //var path = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

                                    //var resultfilesFromServer = Directory.GetFiles(path + "\\" + "VMQApractice_ConsolidatedReport" + dateOfexe);
                                    if (resultfilesFromServer.Length != 0)
                                    {
                                        foreach (var file in resultfilesFromServer)
                                        {
                                            Process p = new Process();
                                            ProcessStartInfo pi = new ProcessStartInfo();
                                            pi.UseShellExecute = true;
                                            pi.FileName = file;
                                            p.StartInfo = pi;

                                            try
                                            {
                                                p.Start();
                                            }
                                            catch (Exception Ex)
                                            {
                                                //MessageBox.Show(Ex.Message);
                                            }
                                        }

                                    }
                                }
                                catch (Exception ex1)
                                {
                                    MessageBox.Show("Test Results NOT found for TestCaseID :" + grid_displayResult.Rows[e.RowIndex].Cells["TestCaseID"].Value.ToString());
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Test Results NOT found for TestCaseID :" + grid_displayResult.Rows[e.RowIndex].Cells["TestCaseID"].Value.ToString());
                        }
                        break;
                    default:
                        break;
                }
            }
            else if ((e.ColumnIndex == 1) && (Convert.ToString(grid_displayResult.Rows[e.RowIndex].Cells[1].Value) == "Del"))
            {
                switch (SName)
                {
                    case "TestCase":
                        try
                        {
                            DialogResult confirmDelete = MessageBox.Show("Do You Want to Delete Test Case # : " + (grid_displayResult.Rows[e.RowIndex].Cells["TestCaseID"].Value.ToString()), "Delete Test Case", MessageBoxButtons.YesNo);
                            if (confirmDelete == DialogResult.Yes)
                            {
                                if (grid_displayResult.Rows[e.RowIndex].Cells["TestCaseID"].Value != null)
                                {
                                    objLib.RunQuery("UPDATE TESTCASEINFO SET ISDELETED='Yes' WHERE TESTCASEID=" + grid_displayResult.Rows[e.RowIndex].Cells["TestCaseID"].Value.ToString());
                                    objLib.RunQuery("UPDATE ACTIONFLOW SET ISDELETED='Yes' WHERE TESTCASEID=" + grid_displayResult.Rows[e.RowIndex].Cells["TestCaseID"].Value.ToString());
                                    objLib.RunQuery("UPDATE TESTDATA SET ISDELETED='Yes' WHERE TESTCASEID=" + grid_displayResult.Rows[e.RowIndex].Cells["TestCaseID"].Value.ToString());
                                    grid_displayResult.Rows.RemoveAt(e.RowIndex);
                                }
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Unable to Delete TestCaseID :" + grid_displayResult.Rows[e.RowIndex].Cells["TestCaseID"].Value.ToString());
                        }
                        break;
                    case "Features":
                        if (Admin.Length != 0)
                        {
                            try
                            {
                                DialogResult confirmDelete = MessageBox.Show("Do You Want to Delete Feature : " + (grid_displayResult.Rows[e.RowIndex].Cells["Name"].Value.ToString()), "Delete Feature", MessageBoxButtons.YesNo);
                                if (confirmDelete == DialogResult.Yes)
                                {
                                    if (grid_displayResult.Rows[e.RowIndex].Cells["Name"].Value != null)
                                    {
                                        objLib.RunQuery("DELETE FROM FEATURES WHERE NAME='" + grid_displayResult.Rows[e.RowIndex].Cells["NAME"].Value.ToString() + "'");
                                        grid_displayResult.Rows.RemoveAt(e.RowIndex);
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Unable to Delete Feature :" + grid_displayResult.Rows[e.RowIndex].Cells["Name"].Value.ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show("Sorry...You don't have rights to DELETE feature name.", "Delete Feature", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        break;
                    case "MasterOr":    //New Enhancement - Delete function in Object Repository
                        if (Admin.Length != 0)
                        {
                            try
                            {
                                DialogResult confirmDelete = MessageBox.Show("Do You Want to Delete this Object : " + (grid_displayResult.Rows[e.RowIndex].Cells["MasterORID"].Value.ToString()), "Delete Object", MessageBoxButtons.YesNo);
                                if (confirmDelete == DialogResult.Yes)
                                {
                                    if (grid_displayResult.Rows[e.RowIndex].Cells["MasterORID"].Value != null)
                                    {
                                        objLib.RunQuery("DELETE FROM MasterOR WHERE MasterORID='" + grid_displayResult.Rows[e.RowIndex].Cells["MasterORID"].Value.ToString() + "'");
                                        grid_displayResult.Rows.RemoveAt(e.RowIndex);
                                    }
                                }
                            }
                            catch
                            {
                                MessageBox.Show("Unable to Delete Object :" + grid_displayResult.Rows[e.RowIndex].Cells["Name"].Value.ToString());
                            }
                        }
                        else
                        {
                            MessageBox.Show("Sorry...You don't have rights to DELETE feature name.", "Delete Feature", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        break;
                    default:
                        break;
                }
            }
            //else if ((e.ColumnIndex == 2) && (Convert.ToString(grid_displayResult.Rows[e.RowIndex].Cells[2].Value) == "Snapshot"))
            //{
            //    switch (da.SelectCommand.CommandText.Split(' ')[3].ToUpper())
            //    {
            //        case "PAGENAMES":
            //            try
            //            {
            //                string path = GetPageSnapshotPath() + grid_displayResult.Rows[e.RowIndex].Cells["PageName"].Value.ToString().Trim() + ".jpg";
            //                Help.ShowHelp(this, path);

            //            }
            //            catch
            //            {
            //                MessageBox.Show("No snapshot exist for :" + grid_displayResult.Rows[e.RowIndex].Cells["PageName"].Value.ToString());
            //            }
            //            break;
            //        default:
            //            break;
            //    }


            //}
        }
        private void BindFeatures()
        {
            lbl_pagetitle.Text = "FEATURES";
            btn_gridedit.Text = "Edit";
            da.SelectCommand = new SqlCommand("SELECT * FROM FEATURES WHERE PROJECTID=" + SignIn.projectId + " ORDER BY NAME");
            retrieveData(da.SelectCommand.CommandText);
        }
        public void BindPageNames()
        {
            grid_displayResult.Visible = true;
            lbl_pagetitle.Text = "PAGE NAMES";
            da.SelectCommand = new SqlCommand("SELECT PageID,PageName,Tags,CreatedBy,CreatedDate,UpdatedBy,LastUpdateDate FROM PAGENAMES WHERE PROJECTID=" + SignIn.projectId + " ORDER BY PAGENAME");
            retrieveData(da.SelectCommand.CommandText);
            grid_displayResult.Columns[2].Visible = true;
            grid_displayResult.Columns[2].Width = 80;
            grid_displayResult.Columns[4].Width = 250;
            grid_displayResult.Columns[5].Width = 250;
            grid_displayResult.Columns[6].Width = 250;
            // grid_displayResult.Columns[8].Width = 250;
        }
        private void BindKeywords()
        {
            grid_displayResult.Visible = true;
            lbl_pagetitle.Text = "KEYWORDS";
            btn_gridedit.Text = "Edit";
            da.SelectCommand = new SqlCommand("SELECT [ActionKeyword_ID],[ActionName],[Description] FROM ACTIONKEYWORD ORDER BY ACTIONNAME");
            retrieveData(da.SelectCommand.CommandText);

            grid_displayResult.Columns[3].Width = 700;
            grid_displayResult.Columns[4].Width = 700;
            // grid_displayResult.Columns[5].Width = 700;
        }
        private void BindMasterOR()
        {
            lbl_pagetitle.Text = "OBJECT REPOSITORY";
            btn_gridedit.Text = "Edit";
            string strSql = "SELECT * FROM MASTERORVIEW WHERE PROJECTID=" + SignIn.projectId + " ORDER BY MASTERORID DESC";
            da.SelectCommand = new SqlCommand(strSql);
            retrieveData(da.SelectCommand.CommandText);
            grid_displayResult.Columns[4].Width = 450;
            grid_displayResult.Columns[5].Width = 450;
            grid_displayResult.Columns[6].Width = 250;
        }
        private void BindTestCase()
        {
            grid_displayResult.Visible = true;
            lbl_pagetitle.Text = "TEST CASES";
            btn_gridedit.Text = "Edit";
            //da.SelectCommand = new SqlCommand("SELECT [ProjectID],[TestCaseID],[TestCaseTitle],[DesignedBy],[CreatedDate],[AssignedTo],[LastUpdatedBy],[UpdatedDate],[TestCategory],[Functionality],[State],[Release],[TestCaseSummary],[Tags],[Priority],[Jira],[Application],[ScrumTeamName],[RTMName],[Module],[ClonedBy],[ClonedDate] FROM TESTCASEINFO WHERE PROJECTID=" + SignIn.projectId + "AND ISDELETED IS NULL ORDER BY TESTCASEID DESC");
            //string strSql = "SELECT [ProjectID],[TestCaseID],[TestCaseTitle],[DesignedBy],[AssignedTo],[TestCategory],[Functionality],[State],[Release],[TestCaseSummary],[Tags],[Priority],[Jira],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate] FROM TESTCASEINFO WHERE PROJECTID=" + SignIn.projectId + "AND ISDELETED IS NULL ORDER BY TESTCASEID DESC";
            string strSql = "SELECT * FROM TESTCASEINFOVIEW WHERE PROJECTID=" + SignIn.projectId + " ORDER BY TESTCASEID DESC";
            da.SelectCommand = new SqlCommand(strSql);
            retrieveData(da.SelectCommand.CommandText);
            grid_displayResult.Columns[3].Width = 50;
            grid_displayResult.Columns[4].Width = 50;
            grid_displayResult.Columns[5].Width = 100;
            grid_displayResult.Columns[6].Width = 100;
        }
        private void BindTestData()
        {
            grid_displayResult.Visible = true;
            lbl_pagetitle.Text = "TEST DATA";
            btn_gridedit.Text = "Edit";
            //da.SelectCommand = new SqlCommand("SELECT [ActionFlow_id],[TestCaseId],[PageID],[FlowIdentifier],[DataIdentifier],[Indicator],[MasterORID],[ActionORData],[SeqNumber],[CreatedBy],[CreatedDate],[LastUpdatedBy],[UpdatedDate],[Execute] FROM TESTDATA WHERE PROJECTID=" + SignIn.projectId + "AND ISDELETED IS NULL ORDER BY TESTCASEID DESC");
            string strSql = "SELECT Top 2000 * FROM TESTDATAVIEW WHERE PROJECTID=" + SignIn.projectId + " ORDER BY TESTCASEID DESC";
            da.SelectCommand = new SqlCommand(strSql);
            retrieveData(da.SelectCommand.CommandText);
            grid_displayResult.Columns[3].Width = 50;
            grid_displayResult.Columns[4].Width = 50;
            grid_displayResult.Columns[5].Width = 100;
            grid_displayResult.Columns[6].Width = 100;
        }
        private void BindTestResults()
        {
            grid_displayResult.Visible = true;
            lbl_pagetitle.Text = "TEST RESULTS";
            btn_gridedit.Text = "View";
            string strSql = "SELECT * FROM TESTRESULTSVIEW  WHERE PROJECTID=" + SignIn.projectId + " ORDER BY DATEOFEXE DESC";
            da.SelectCommand = new SqlCommand(strSql);
            //da.SelectCommand = new SqlCommand("SELECT T.[ProjectID],T.[TestRunID],T.[TestCaseID],T.[Result],T.[CondExecuted],T.[CondPassed],T.[CondFailed],T.[RunBy],T.[DateOfExe],T.[Environment],T.[Browser],T.[ExeTime] FROM TESTRESULTS T,USERS U  WHERE T.PROJECTID=" + SignIn.projectId + " AND T.RunBy = U.UserId ORDER BY T.DATEOFEXE DESC");
            retrieveData(da.SelectCommand.CommandText);
            //grid_displayResult.Columns[10].Width = 550;
            //grid_displayResult.Columns[11].Width = 700;
            //grid_displayResult.Columns[12].Width = 500;
        }
        private void retrieveData(string query)
        {
            tlp_homepage.Visible = true;
            txt_tags.Visible = false;
            lbl_tags.Visible = false;

            cmb_columnnames.Items.Clear();
            cmb_columnnames.ResetText();

            cmb_columnvalues.Enabled = false;
            cmb_columnvalues.Items.Clear();
            cmb_columnvalues.ResetText();

            try
            {
                DataTable dtable = new DataTable();
                dtable = objLib.binddataTable(query);

                bindingSrc.DataSource = dtable;
                grid_displayResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                grid_displayResult.Columns[2].Visible = false;
                grid_displayResult.Columns[0].Width = 40;
                grid_displayResult.Columns[1].Width = 40;


                if (dtable.Rows.Count != 0)
                {
                    var colNames = dtable.Columns.Cast<DataColumn>()
                                        .Select(x => x.ColumnName).ToArray();
                    cmb_columnnames.Items.AddRange(colNames);

                    cmb_columnnames.Items.Remove("PageID");
                    cmb_columnnames.Items.Remove("ProjectID");
                    cmb_columnnames.Items.Remove("ActionKeyword_ID");
                    cmb_columnnames.Items.Remove("MasterORID");
                    cmb_columnnames.Items.Remove("ActionFlow_id");
                }
                else
                {
                    MessageBox.Show("NO DATA FOUND....", "Home Page", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    grid_displayResult.Refresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid DB Connection String.\n\nException: " + ex.Message, "Home Page", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FilterGridResultByColName(string query, string columnName)
        {
            cmb_columnvalues.Enabled = true;
            lbl_tags.Visible = false;
            txt_tags.Visible = false;
            cmb_columnvalues.Items.Clear();
            cmb_columnvalues.ResetText();
            string[] dataspt = query.Split(new[] { "FROM" }, StringSplitOptions.None);
            string sTable = dataspt[1].Split(' ')[1].ToUpper();
            try
            {
                if ((columnName == "PageName") || (columnName == "Keyword") || (columnName == "Label") || (columnName == "RunBy") || (columnName == "DateOfExe"))
                {
                    switch (columnName)
                    {
                        case "PageName":
                            cmb_columnvalues.Items.AddRange((from t in objLib.GetPageTitles() orderby t.Value select t.Value).ToArray());
                            break;
                        case "Label":
                            cmb_columnvalues.Items.AddRange((from t in objLib.GetORLables() orderby t.Value select t.Value).Distinct().ToArray());
                            break;
                        case "Keyword":
                            cmb_columnvalues.Items.AddRange((from t in objLib.GetKeywords() orderby t.Value select t.Value).ToArray());
                            break;
                        case "RunBy":
                            cmb_columnvalues.Items.AddRange((from t in objLib.GetUserInfo() orderby t.Value select t.Value).ToArray());
                            break;
                        case "DateOfExe":
                            cmb_columnvalues.Items.AddRange(datefilterOptions);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    DataTable dt = new DataTable();
                    if (objLib.IsEqual(query.Split(' ')[3], "ActionKeyword"))
                        dt = objLib.binddataTable("SELECT DISTINCT [" + columnName + "] FROM " + query.Split(' ')[3]);
                    else if ((objLib.IsEqual(sTable, "TESTCASEINFO")))
                        dt = objLib.binddataTable("SELECT DISTINCT [" + columnName + "] FROM " + query.Split(' ')[3] + " WHERE PROJECTID = " + SignIn.projectId + " AND ISDELETED IS NULL");
                    else if (objLib.IsEqual(sTable, "TESTDATAVIEW"))
                        dt = objLib.binddataTable("SELECT DISTINCT [" + columnName + "] FROM " + sTable + " WHERE PROJECTID = " + SignIn.projectId + "");
                    else
                        dt = objLib.binddataTable("SELECT DISTINCT [" + columnName + "] FROM " + query.Split(' ')[3] + " WHERE PROJECTID = " + SignIn.projectId);
                    var result = dt.AsEnumerable().Select(s => new
                    {
                        val = s.Field<object>(columnName),
                    });
                    cmb_columnvalues.Items.AddRange((from t in result select t.val ?? "").ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR : " + ex.Message);
            }
        }
        private void txt_tags_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_tags.Text.Trim()))
            {
                var tags = txt_tags.Text.Trim().Split(',');
                List<string> tagslist = new List<string>();

                foreach (var tag in tags)
                    tagslist.Add("tags like '%" + tag + "%'");



                DataTable dt = new DataTable();
                dt = objLib.binddataTable("Select * from testcaseinfo where functionality='" + cmb_columnvalues.SelectedItem.ToString() + "' and (" + string.Join(" or ", tagslist.ToArray()) + ")");
                DataColumnCollection colcoll = dt.Columns;
                if (colcoll.Contains("ProjectID")) dt.Columns.Remove("ProjectID");
                if (colcoll.Contains("IsDeleted")) dt.Columns.Remove("IsDeleted");
                bindingSrc.DataSource = dt;
                if (dt.Rows.Count != 0)
                {
                    grid_displayResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    grid_displayResult.Columns[0].Width = 40;

                }
                else
                {
                    MessageBox.Show("NO DATA FOUND....", "No Data Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    grid_displayResult.Refresh();
                }
            }
        }
        private void FilterGridResultByColNameWithValue(string strQuery, string strColName, string strColValue)
        {
            string customquery = string.Empty;
            txt_tags.Text = "";
            int days = 0;
            string[] dataspt = strQuery.Split(new[] { "FROM" }, StringSplitOptions.None);
            string sTable = dataspt[1].Split(' ')[1].ToUpper();
            try
            {
                switch (strColName)
                {
                    case "PageName":
                        strColName = "PageID";
                        strColValue = objLib.GetPageTitles().FirstOrDefault(x => x.Value == strColValue).Key;
                        break;
                    case "Label":

                        if (da.SelectCommand.CommandText.Split(' ')[3].ToUpper() == "MASTERORVIEW")
                        {
                            strColName = "Label";
                            strColValue = objLib.GetLabels().FirstOrDefault(x => x.Value == strColValue).Key;
                            strColValue = strColValue.Split(new char[] { '_' }, 2)[1].ToString().Trim();
                        }
                        else
                        {
                            strColName = "MasterORID";
                            strColValue = objLib.GetORLables().FirstOrDefault(x => x.Value == strColValue).Key;
                        }

                        break;
                    case "Keyword":
                        strColName = "Indicator";
                        strColValue = objLib.GetKeywords().FirstOrDefault(x => x.Value == strColValue).Key;
                        break;
                    case "RunBy":
                        strColName = "RunBy";
                        //strColValue = objLib.GetUserInfo().FirstOrDefault(x => x.Value == strColValue).Key;
                        break;
                    case "Functionality":
                        lbl_tags.Visible = true;
                        txt_tags.Visible = true;
                        break;
                    case "DateOfExe":
                        switch (strColValue)
                        {
                            case "Today":
                                days = 0;
                                break;
                            case "Last 24 hours":
                                days = -1;
                                break;
                            case "Last 48 hours":
                                days = -2;
                                break;
                            case "Last 7 days":
                                days = -7;
                                break;
                            case "Last 14 days":
                                days = -14;
                                break;
                            case "Last 28 days":
                                days = -28;
                                break;
                            case "Last 90 days":
                                days = -90;
                                break;
                            case "All":
                                days = -365;
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
                if (objLib.IsEqual(strQuery.Split(' ')[3], "ActionKeyword"))
                {
                    if (!objLib.IsNullOrEmpty(strColValue))
                        customquery = "SELECT * FROM " + strQuery.Split(' ')[3] + " WHERE [" + strColName + "]='" + strColValue.Replace("'", "''") + "'";
                    else
                        customquery = "SELECT * FROM " + strQuery.Split(' ')[3] + " WHERE [" + strColName + "] IS NULL";
                }
                else if ((objLib.IsEqual(sTable, "TestCaseInfo")) || (objLib.IsEqual(sTable, "TESTDATAVIEW")))
                {
                    if (!objLib.IsNullOrEmpty(strColValue))
                        customquery = "SELECT * FROM " + sTable + " WHERE [" + strColName + "]='" + strColValue.Replace("'", "''") + "' AND ProjectID=" + SignIn.projectId;
                    else
                        customquery = "SELECT * FROM " + sTable + " WHERE [" + strColName + "] IS NULL AND ISDELETED IS NULL AND ProjectID=" + SignIn.projectId;
                }
                else if (strColName == "DateOfExe")
                {
                    if (days != 0)
                        customquery = "SELECT * FROM " + strQuery.Split(' ')[3] + " WHERE [" + strColName + "]>= DATEADD(DAY, " + days + ", GETDATE()) AND ProjectID=" + SignIn.projectId;
                    else
                        customquery = "SELECT * FROM " + strQuery.Split(' ')[3] + " WHERE CAST(DateOfExe as date) =(CAST(GETDATE() AS DATE)) AND ProjectID=" + SignIn.projectId;
                }
                else
                {
                    if (!objLib.IsNullOrEmpty(strColValue))
                        customquery = "SELECT * FROM " + strQuery.Split(' ')[3] + " WHERE [" + strColName + "]='" + strColValue.Replace("'", "''") + "' AND ProjectID=" + SignIn.projectId;
                    else
                        customquery = "SELECT * FROM " + strQuery.Split(' ')[3] + " WHERE [" + strColName + "] IS NULL AND ProjectID=" + SignIn.projectId;
                }
                DataTable dt = new DataTable();
                dt = objLib.binddataTable(customquery);
                DataColumnCollection colcoll = dt.Columns;
                if (colcoll.Contains("ProjectID")) dt.Columns.Remove("ProjectID");
                if (colcoll.Contains("IsDeleted")) dt.Columns.Remove("IsDeleted");
                bindingSrc.DataSource = dt;
                if (dt.Rows.Count != 0)
                {
                    grid_displayResult.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    grid_displayResult.Columns[0].Width = 40;

                }
                else
                {
                    MessageBox.Show("NO DATA FOUND....", "No Data Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    grid_displayResult.Refresh();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
        }
        public string GetPageSnapshotPath()
        {
            var configurationFile = XDocument.Load(@"Config\Config.xml");
            var connstr = configurationFile.XPathSelectElement("/TestAutomationFramework/PageSnapshotlocation");
            return connstr == null
                ? null
                : connstr.Value;
        }
        public string GetDBPath()
        {
            var configurationFile = XDocument.Load(@"Config\Config.xml");
            var connstr = configurationFile.XPathSelectElement("/TestAutomationFramework/DBExecution/DBConnectionString");
            var strDBName = connstr.Value.ToString().Split(';')[1].Split('=')[1].Trim();
            return connstr == null
                ? null
                : strDBName;
        }
    }
}
