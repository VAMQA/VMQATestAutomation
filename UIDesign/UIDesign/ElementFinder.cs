using VM.Platform.TestAutomationFramework.Adapters.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace UIDesign
{
    public partial class ElementFinder : Form
    {

        private Label lbl_browser = new Label();
        private Label lbl_uri = new Label();
        private Label lbl_url = new Label();
        private Label lbl_environment = new Label();
        //private Label lbl_application = new Label();

        private Label lbl_pagetitle = new Label();
        private Label lbl_URL = new Label();
        private TextBox txt_URL = new TextBox();
        private Button btn_Navigate = new Button();
        private Button btn_getElement = new Button();
        private Button btn_addToOR = new Button();
        private Button btn_highlight = new Button();
        private Button btn_editOR = new Button();
        private Button btn_TestObjects = new Button();
        private Label lbl_inprogress = new Label();
        private ComboBox cmb_browser = new ComboBox();
       // private ComboBox cmb_application = new ComboBox();
        private ComboBox cmb_environment = new ComboBox();
        private Label lbl_blank = new Label();
        private Label lbl_selectProperty = new Label();
        private TabControl tab_SelectTab = new TabControl();
        private TabPage tabPage1 = new TabPage();
        private TabPage tabPage2 = new TabPage();
        private ComboBox cmb_columnvalues = new ComboBox();
        private ComboBox cmb_columnnames = new ComboBox();
        private DataGridView grid_displayResult = new DataGridView();
        private Label lbl_mismatch = new Label();


        private PictureBox pic_gecko = new PictureBox();
        public DataGridView grid_cloner = new DataGridView();
        private BindingSource bindingSrc = new BindingSource();
        private FlowLayoutPanel flp_testcloner = new FlowLayoutPanel();
        private TableLayoutPanel tlp_testcloner = new TableLayoutPanel();
        private TableLayoutPanel tlp_testcloner_tab = new TableLayoutPanel();
        Dictionary<string, string> dict_Environments = new Dictionary<string, string>();
        Dictionary<string, string> dict_application = new Dictionary<string, string>();
        public DataGridView grid_SelectProperty = new DataGridView();
        FuncLib objLib = new FuncLib();
        UIScrapperCode uicode = new UIScrapperCode();
        int tcid = 0;
        private DataTable dtglb;
        string[] properties = new string[] { "Default(<INPUT>/<A>/<BUTTON>/<SELECT>)", "Elements With Label", "Editbox(<INPUT>)", "Link/Buttons(<A>/<BUTTON>)", "Combo(<SELECT>)", "Label/Text(<SPAN>/<LABEL>)", "Images(<IMG>)", "Block(<DIV>)", "TABLE(<TABLE>)", "Table Rows/Columns(<TD>/<TR>)", "Frame(<FRAME>)" };
        SqlDataAdapter da = new SqlDataAdapter();
        string[] datefilterOptions = new string[] { "All", "Today", "Last 24 hours", "Last 48 hours", "Last 7 days", "Last 14 days", "Last 28 days", "Last 90 days" };
        string[] comboItems = new string[] { "PageName", "Label", "ControlType", "ControlID", "Class", "ParentControl", "TagName", "FriendlyName", "ValueAttribute", "TagInstance", "Type", "ClassName", "InnerText", "ControlDefinition", "LebelFor", "Xpath", "Version", "CreatedBy", "CreatedDate", "LastUpdatedBy", "UpdatedDate" };
        //FuncLib objLib = new FuncLib();
        public string tabName;


        public ElementFinder()
        {
            InitializeComponent();

            #region testcloner_design

            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            this.Width = 1360;
            this.Height = 720;
            this.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 50, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 100);
            //this.AutoScroll = true;            
            this.Text = "Object Spy";

            grid_displayResult.ReadOnly = false;
            //grid_displayResult.AutoSize = true;
            grid_displayResult.DataSource = bindingSrc;
            grid_displayResult.Dock = DockStyle.Fill;
            grid_displayResult.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 175, 200);
            grid_displayResult.AutoGenerateColumns = true;
            grid_displayResult.DefaultCellStyle.Font = new Font("Calibri", 10);
            grid_displayResult.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
            grid_displayResult.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8.9F, FontStyle.Bold);
            grid_displayResult.Visible = false;
            grid_displayResult.ScrollBars = ScrollBars.Both;
            grid_displayResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.None;
            DataGridViewCheckBoxColumn chk_selection_result = new DataGridViewCheckBoxColumn();
            chk_selection_result.Width = 30;
            chk_selection_result.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid_displayResult.Columns.Insert(0, chk_selection_result);
            tab_SelectTab.Name = "tab_SelectTab";
            tab_SelectTab.Font = new Font("Calibri", 12F, FontStyle.Regular);
            tab_SelectTab.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 950;
            tab_SelectTab.Height = 30;
            tab_SelectTab.Dock = DockStyle.Left;
            //tab_SelectTab.set(ControlStyles.SupportsTransparentBackColor, true);
            tab_SelectTab.BackColor = Color.White;




            tabPage1.Name = "tabPage1";
            tabPage1.Text = "Objects Found in the Page";
            tabPage1.Font = new Font("Calibri", 12F, FontStyle.Bold);
            //tabPage1.Width = 100;
            tab_SelectTab.TabPages.Add(tabPage1);
            // Add TabPage2

            tabPage2.Name = "tabPage2";
            tabPage2.Text = " Existing Objects in Repository";
            tabPage2.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            //tabPage2.Width = 100;
            tab_SelectTab.TabPages.Add(tabPage2);
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
            //Table Layout Panel Settings                                    
            tlp_testcloner_tab.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_testcloner_tab.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_testcloner_tab.AutoSize = true;
            tlp_testcloner_tab.Location = new Point(30, 80);

            //Gecko Picture Box Settings            
            pic_gecko.Height = 110;
            pic_gecko.Width = 170;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            lbl_selectProperty.Text = "Object Type :*";
            lbl_selectProperty.Name = "lbl_selectProperty";
            lbl_selectProperty.TextAlign = ContentAlignment.BottomLeft;
            lbl_selectProperty.Font = new Font("Calibri", 11F, FontStyle.Regular);
            lbl_selectProperty.Height = 20;
            lbl_selectProperty.Width = 150;

            //Browser Label settings            
            lbl_browser.Text = "Browser Type :*";
            lbl_browser.Name = "lbl_browser";
            lbl_browser.TextAlign = ContentAlignment.BottomLeft;
            lbl_browser.Font = new Font("Calibri", 11F, FontStyle.Regular);
            lbl_browser.Height = 20;
            lbl_browser.Width = 150;

            cmb_browser.Name = "cmb_browser";
            cmb_browser.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_browser.Height = 30;
            cmb_browser.Width = 250;
            cmb_browser.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_browser.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_browser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_browser.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_browser.Items.AddRange(new[] { "Chrome", "Ie" });

            lbl_url.Text = "URL :*";
            lbl_url.TextAlign = ContentAlignment.BottomLeft;
            lbl_url.Font = new Font("Calibri", 11F, FontStyle.Regular);
            lbl_url.Height = 20;
            lbl_url.Width = 150;

            //URI Label settings                        
            lbl_uri.Name = "lbl_uri";
            lbl_uri.ForeColor = Color.Blue;
            lbl_uri.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_uri.Width = 500;
            lbl_uri.Height = 30;
            lbl_uri.TextAlign = ContentAlignment.MiddleLeft;
            lbl_uri.Text = "";

            //Environment Value Label settings            
            lbl_environment.Text = "Environment :*";
            lbl_environment.Name = "lbl_environment";
            lbl_environment.TextAlign = ContentAlignment.BottomLeft;
            lbl_environment.Font = new Font("Calibri", 11F, FontStyle.Regular);
            lbl_environment.Height = 20;
            lbl_environment.Width = 150;

            //Environment Value Label settings            
            //lbl_application.Text = "Application :*";
            //lbl_application.Name = "lbl_application";
            //lbl_application.TextAlign = ContentAlignment.BottomLeft;
            //lbl_application.Font = new Font("Calibri", 11F, FontStyle.Regular);
            //lbl_application.Height = 20;
            //lbl_application.Width = 200;

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

            //Data Gridview Settings
            grid_cloner.ReadOnly = false;
            grid_cloner.Dock = DockStyle.None;
            grid_cloner.AutoGenerateColumns = true;
            grid_cloner.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 100, 200);
            grid_cloner.DataSource = bindingSrc;
            grid_cloner.DefaultCellStyle.Font = new Font("Calibri", 10);
            grid_cloner.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;

            grid_cloner.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8.9F, FontStyle.Bold);
            grid_cloner.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            grid_cloner.Visible = true;
            grid_cloner.AllowUserToAddRows = true;

            // customize dataviewgrid, add checkbox column
            DataGridViewCheckBoxColumn chk_selection = new DataGridViewCheckBoxColumn();
            chk_selection.Width = 30;
            chk_selection.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid_cloner.Columns.Insert(0, chk_selection);

            grid_SelectProperty.Size = new Size(400, 100);
            grid_SelectProperty.DefaultCellStyle.Font = new Font("Calibri", 10);
            grid_SelectProperty.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;

            grid_SelectProperty.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8F, FontStyle.Bold);
            grid_SelectProperty.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            grid_SelectProperty.AllowUserToAddRows = false;
            DataGridViewCheckBoxColumn chk_selection1 = new DataGridViewCheckBoxColumn();
            chk_selection1.Width = 30;
            chk_selection1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid_SelectProperty.Columns.Insert(0, chk_selection1);

            //grid_SelectProperty.Columns.Add("colSelection", "");
            //grid_SelectProperty.Columns["colSelection"].Width = 50;
            //grid_SelectProperty.Columns.Add("colProperties", "Properties");
            //grid_SelectProperty.Columns["colProperties"].ReadOnly = true;
            //grid_SelectProperty.Columns["colProperties"].Width = 350;


            lbl_URL.Text = "Application URL:";
            lbl_URL.Name = "lbl_URL";
            lbl_URL.TextAlign = ContentAlignment.MiddleLeft;
            lbl_URL.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_URL.Height = 20;
            lbl_URL.Width = 200;

            //Clone To textbox settings            
            txt_URL.Text = "";
            txt_URL.Name = "txt_URL";
            txt_URL.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_URL.Height = 30;
            txt_URL.Width = 450;
            txt_URL.ReadOnly = false;
            txt_URL.Text = "http://atlas1.int.ibu.VM.net:1075/cc/ClaimCenter.do";

            //Clone button settings            
            btn_Navigate.Text = "Navigate";
            btn_Navigate.Name = "btn_Navigate";
            btn_Navigate.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_Navigate.Height = 30;
            btn_Navigate.Width = 130;
            btn_Navigate.FlatStyle = FlatStyle.Flat;
            btn_Navigate.Anchor = AnchorStyles.Left;


            lbl_blank.Text = "";
            lbl_blank.Name = "lbl_blank";
            lbl_blank.TextAlign = ContentAlignment.MiddleLeft;
            lbl_blank.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_blank.Height = 20;
            lbl_blank.Width = 40;

            btn_getElement.Text = "Get Objects";
            btn_getElement.Name = "btn_getElement";
            btn_getElement.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_getElement.Height = 30;
            btn_getElement.Width = 130;
            //btn_getElement.Visible = false;
            btn_getElement.FlatStyle = FlatStyle.Flat;
            btn_getElement.Anchor = AnchorStyles.Left;
            btn_getElement.Enabled = false;

            btn_addToOR.Text = "Add To OR";
            btn_addToOR.Name = "btn_addToOR";
            btn_addToOR.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_addToOR.Height = 30;
            btn_addToOR.Width = 130;
            //btn_addToOR.Visible = false;
            btn_addToOR.FlatStyle = FlatStyle.Flat;
            btn_addToOR.Anchor = AnchorStyles.Left;
            btn_addToOR.Enabled = false;
            btn_editOR.Text = "Edit Object";
            btn_editOR.Name = "btn_editOR";
            btn_editOR.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_editOR.Height = 30;
            btn_editOR.Width = 130;
            btn_editOR.Visible = false;
            btn_editOR.FlatStyle = FlatStyle.Flat;
            btn_editOR.Anchor = AnchorStyles.Left;
            btn_editOR.Enabled = false;


            btn_TestObjects.Text = "Validate Objects";
            btn_TestObjects.Name = "btn_TestObjects";
            btn_TestObjects.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_TestObjects.Height = 30;
            btn_TestObjects.Width = 130;
            btn_TestObjects.Visible = false;
            btn_TestObjects.FlatStyle = FlatStyle.Flat;
            btn_TestObjects.Anchor = AnchorStyles.Left;
            btn_TestObjects.Enabled = false;

            btn_highlight.Text = "Highlight";
            btn_highlight.Name = "btn_highlight";
            btn_highlight.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_highlight.Height = 30;
            btn_highlight.Width = 130;
            // btn_highlight.Visible = false;
            btn_highlight.FlatStyle = FlatStyle.Flat;
            btn_highlight.Anchor = AnchorStyles.Left;
            btn_highlight.Enabled = false;


            //Inprogress Label settings                        
            lbl_inprogress.Name = "lbl_inprogress";
            lbl_inprogress.TextAlign = ContentAlignment.BottomLeft;
            lbl_inprogress.Font = new Font("Calibri", 10F, FontStyle.Bold);
            lbl_inprogress.ForeColor = Color.Blue;
            lbl_inprogress.Height = 30;
            lbl_inprogress.Width = 1200;
            lbl_inprogress.AutoSize = true;
            lbl_inprogress.Visible = false;
            lbl_inprogress.Text = "";

            lbl_mismatch.Name = "lbl_mismatch";
            lbl_mismatch.TextAlign = ContentAlignment.BottomLeft;
            lbl_mismatch.Font = new Font("Calibri", 10F, FontStyle.Bold);
            lbl_mismatch.ForeColor = Color.Red;
            lbl_mismatch.Height = 30;
            lbl_mismatch.Width = 1200;
            lbl_mismatch.AutoSize = true;
            lbl_mismatch.Visible = false;
            lbl_mismatch.Text = "";
            //Userid Label settings           
            lbl_pagetitle.Height = 60;
            lbl_pagetitle.Width = 1100;
            //lbl_pagetitle.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 4 - 150;
            lbl_pagetitle.Text = "OBJECT SPY";
            lbl_pagetitle.Name = "lbl_pagetitle";
            lbl_pagetitle.TextAlign = ContentAlignment.TopLeft;
            lbl_pagetitle.Font = new Font("Calibri", 20F, FontStyle.Bold);
            lbl_pagetitle.ForeColor = Color.Blue;

            cmb_columnnames.Height = 24;
            cmb_columnnames.Width = 200;
            cmb_columnnames.Font = new Font("Calibri", 11, FontStyle.Regular);
            //cmb_columnnames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_columnnames.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_columnnames.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_columnnames.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_columnnames.Visible = false;
            cmb_columnnames.Items.AddRange(comboItems);


            cmb_columnvalues.Height = 24;
            cmb_columnvalues.Width = 300;
            cmb_columnvalues.Font = new Font("Calibri", 11, FontStyle.Regular);
            cmb_columnvalues.Enabled = false;
            cmb_columnvalues.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            //cmb_columnvalues.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_columnvalues.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_columnvalues.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_columnvalues.Visible = false;
            tlp_testcloner.Controls.Add(lbl_pagetitle, 1, 1);
            tlp_testcloner.SetColumnSpan(lbl_pagetitle, 7);
            tlp_testcloner.Controls.Add(lbl_browser, 1, 2);
            tlp_testcloner.Controls.Add(cmb_browser, 2, 2);
            tlp_testcloner.SetColumnSpan(cmb_browser, 2);

           // tlp_testcloner.Controls.Add(lbl_application, 1, 3);
            //tlp_testcloner.Controls.Add(cmb_application, 2, 3);
            //tlp_testcloner.SetColumnSpan(cmb_application, 2);
            tlp_testcloner.Controls.Add(lbl_environment, 1, 4);
            tlp_testcloner.Controls.Add(cmb_environment, 2, 4);
            tlp_testcloner.SetColumnSpan(cmb_environment, 2);
            tlp_testcloner.Controls.Add(lbl_url, 1, 5);
            tlp_testcloner.Controls.Add(lbl_uri, 2, 5);
            tlp_testcloner.SetColumnSpan(lbl_uri, 2);

            // tlp_testcloner.Controls.Add(lbl_URL, 4, 2);
            // tlp_testcloner.Controls.Add(txt_URL, 4, 3);
            tlp_testcloner.Controls.Add(lbl_selectProperty, 3, 2);
            tlp_testcloner.Controls.Add(grid_SelectProperty, 4, 2);
            tlp_testcloner.SetColumnSpan(grid_SelectProperty, 3);
            tlp_testcloner.SetRowSpan(grid_SelectProperty, 4);

            tlp_testcloner.Controls.Add(btn_Navigate, 3, 6);
            tlp_testcloner.Controls.Add(btn_getElement, 7, 6);
            tlp_testcloner.Controls.Add(tab_SelectTab, 1, 7);
            tlp_testcloner.SetColumnSpan(tab_SelectTab, 7);
            tlp_testcloner.Controls.Add(btn_highlight, 6, 8);
            tlp_testcloner.Controls.Add(btn_addToOR, 7, 8);
            tlp_testcloner.Controls.Add(cmb_columnnames, 1, 8);
            //tlp_testcloner.SetColumnSpan(cmb_columnnames, 2);
            tlp_testcloner.Controls.Add(cmb_columnvalues, 2, 8);
            tlp_testcloner.SetColumnSpan(cmb_columnvalues, 2);
            tlp_testcloner.Controls.Add(btn_editOR, 5, 8);
            tlp_testcloner.Controls.Add(btn_TestObjects, 6, 8);
            tlp_testcloner.Controls.Add(grid_displayResult, 1, 9);
            tlp_testcloner.SetColumnSpan(grid_displayResult, 7);
            tlp_testcloner.Controls.Add(grid_cloner, 1, 9);
            tlp_testcloner.SetColumnSpan(grid_cloner, 7);
            tlp_testcloner.Controls.Add(lbl_inprogress, 1, 10);
            tlp_testcloner.SetColumnSpan(lbl_inprogress, 7);
            tlp_testcloner.Controls.Add(lbl_mismatch, 1, 10);
            tlp_testcloner.SetColumnSpan(lbl_mismatch, 7);




            //Adding Controls to Flow Layout Panel
            flp_testcloner.Controls.AddRange(new Control[] { pic_gecko, tlp_testcloner });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_testcloner });
            this.Load += new System.EventHandler(ElementFinder_Load);

            #endregion

            #region testcloner_methods
            //cmb_application.SelectedIndexChanged += new System.EventHandler(cmb_application_selectionchanged);
            cmb_environment.SelectedIndexChanged += new System.EventHandler(cmb_environment_selectionchanged);
            btn_Navigate.Click += new System.EventHandler(btn_navigate_Click);
            btn_getElement.Click += new System.EventHandler(btn_getElement_Click);
            grid_cloner.CellContentClick += new DataGridViewCellEventHandler(grid_cloner_CellContentClick);
            grid_displayResult.CellContentClick += new DataGridViewCellEventHandler(grid_displayResult_CellContentClick);
            btn_addToOR.Click += new System.EventHandler(btn_addtoOR_Click);
            btn_editOR.Click += new System.EventHandler(btn_editOR_Click);
            btn_highlight.Click += new System.EventHandler(btn_highlight_Click);
            btn_TestObjects.Click += new System.EventHandler(btn_testObjects_Click);
            cmb_columnnames.SelectedIndexChanged += new System.EventHandler(columnname_selectionChanged);
            cmb_columnvalues.SelectedIndexChanged += new System.EventHandler(columnvalue_selectionChanged);
            tab_SelectTab.SelectedIndexChanged += new System.EventHandler(tab_select_changed);


            #endregion
        }
        private void tab_select_changed(object sender, EventArgs e)
        {
            if (tab_SelectTab.SelectedTab == tab_SelectTab.TabPages["tabPage2"])
            {
                if (grid_cloner.RowCount > 1)
                {
                    da.SelectCommand = new SqlCommand("SELECT [MasterORID],[PageID],[Label],[ControlType],[ControlID],[Class],[ParentControl],[TagName],[FriendlyName],[ValueAttribute],[TagInstance],[Type],[ClassName],[InnerText],[ControlDefinition],[LabelFor],[Xpath],[Version],[CreatedBy],[CreatedDate],[LastUpdatedBy],[UpdatedDate] FROM MASTEROR WHERE PROJECTID=" + SignIn.projectId + " ORDER BY MASTERORID DESC");
                    grid_cloner.Visible = false;
                    cmb_columnnames.Visible = true;
                    cmb_columnvalues.Visible = true;
                    grid_displayResult.Visible = true;
                    btn_addToOR.Visible = false;
                    btn_editOR.Visible = true;
                    btn_TestObjects.Visible = true;
                    lbl_mismatch.Visible = true;
                    tabName = "Existing Objects in Repository";
                }
                else
                {
                    MessageBox.Show("Please select input parameters and GetElelments on the page first.");
                    tab_SelectTab.SelectedTab = tab_SelectTab.TabPages["tabPage1"];
                    cmb_columnnames.Visible = false;
                    cmb_columnvalues.Visible = false;
                    grid_displayResult.Visible = false;
                    grid_cloner.Visible = true;
                    btn_editOR.Visible = false;
                    btn_addToOR.Visible = true;
                    btn_TestObjects.Visible = false;
                    lbl_mismatch.Visible = false;
                    tabName = "Objects Found in the Page";
                }

            }
            else
            {

                cmb_columnnames.Visible = false;
                cmb_columnvalues.Visible = false;
                grid_displayResult.Visible = false;
                grid_cloner.Visible = true;
                btn_editOR.Visible = false;
                btn_addToOR.Visible = true;
                btn_TestObjects.Visible = false;
                lbl_mismatch.Visible = false;
                tabName = "Objects Found in the Page";

            }
        }
        private void ElementFinder_Load(object sender, EventArgs e)
        {
            //populate environment variable
            tabName = "Objects Found in the Page";
            //cmb_application.Items.Clear();
            var configFile = XDocument.Load(@"Config\Config.xml");
            dict_Environments = configFile.XPathSelectElements("/TestAutomationFramework/Environments/*")
                                                                   .Select(x => new { key = x.Name.ToString(), value = x.Value })
                                                                   .OrderBy(x => x.key).ToDictionary(x => x.key, x => x.value);
            //cmb_application.Items.AddRange(dict_application.Select(x => x.Key).ToArray());
            cmb_environment.Items.AddRange(dict_Environments.Select(x => x.Key).ToArray());

            //populate properties values
            DataTable dtloc = new DataTable();
            dtloc.Columns.Add("Properties", typeof(string));
            for (int i = 0; i <= properties.Length - 1; i++)
            {
                dtloc.NewRow();
                dtloc.Rows.Add(properties[i]);
            }
            grid_SelectProperty.DataSource = dtloc;
            grid_SelectProperty.Rows[0].Cells[0].Value = true;
            grid_SelectProperty.Columns[0].Width = 60;
            grid_SelectProperty.Columns[1].Width = 270;

        }
        private void columnname_selectionChanged(object sender, EventArgs e)
        {

            FilterGridResultByColName(da.SelectCommand.CommandText, cmb_columnnames.SelectedItem.ToString());
        }
        private void columnvalue_selectionChanged(object sender, EventArgs e)
        {
            FilterGridResultByColNameWithValue(da.SelectCommand.CommandText, cmb_columnnames.SelectedItem.ToString(), cmb_columnvalues.Text.ToString());
            btn_TestObjects.Enabled = true;
        }

        private void FilterGridResultByColName(string query, string columnName)
        {
            cmb_columnvalues.Enabled = true;
            cmb_columnvalues.Items.Clear();
            cmb_columnvalues.ResetText();
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
                            cmb_columnvalues.Items.AddRange((from t in objLib.GetORLables() orderby t.Value select t.Value).ToArray());
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
                    else if ((objLib.IsEqual(query.Split(' ')[3], "TestCaseInfo")) || (objLib.IsEqual(query.Split(' ')[3], "TestData")))
                        dt = objLib.binddataTable("SELECT DISTINCT [" + columnName + "] FROM " + query.Split(' ')[3] + " WHERE PROJECTID = " + SignIn.projectId + " AND ISDELETED IS NULL");
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

        private void FilterGridResultByColNameWithValue(string strQuery, string strColName, string strColValue)
        {
            string customquery = string.Empty;
            //txt_tags.Text = "";
            int days = 0;
            try
            {
                switch (strColName)
                {
                    case "PageName":
                        strColName = "PageID";
                        strColValue = objLib.GetPageTitles().FirstOrDefault(x => x.Value == strColValue).Key;
                        break;
                    case "Label":

                        if (da.SelectCommand.CommandText.Split(' ')[3].ToUpper() == "MASTEROR")
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
                        strColValue = objLib.GetUserInfo().FirstOrDefault(x => x.Value == strColValue).Key;
                        break;
                    case "Functionality":
                        // lbl_tags.Visible = true;
                        // txt_tags.Visible = true;
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
                else if ((objLib.IsEqual(strQuery.Split(' ')[3], "TestCaseInfo")) || (objLib.IsEqual(strQuery.Split(' ')[3], "TestData")))
                {
                    if (!objLib.IsNullOrEmpty(strColValue))
                        customquery = "SELECT * FROM " + strQuery.Split(' ')[3] + " WHERE [" + strColName + "]='" + strColValue.Replace("'", "''") + "' AND ISDELETED IS NULL AND ProjectID=" + SignIn.projectId;
                    else
                        customquery = "SELECT * FROM " + strQuery.Split(' ')[3] + " WHERE [" + strColName + "] IS NULL AND ISDELETED IS NULL AND ProjectID=" + SignIn.projectId;
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
                    //grid_displayResult.Columns[1].Visible = false;                
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
        }

        private void cmb_environment_selectionchanged(object sender, EventArgs e)
        {
            lbl_uri.Text = dict_Environments[cmb_environment.SelectedItem.ToString()];
        }

        public string desiredXPath()
        {
            string xpath = "";
            foreach (DataGridViewRow row in grid_SelectProperty.Rows)
            {


                if ((bool)row.Cells[0].FormattedValue)
                {
                    if (!xpath.Equals(""))
                    {
                        xpath = xpath + " | ";
                    }
                    switch (row.Cells[1].FormattedValue.ToString().Trim())
                    {
                        case "Default(<INPUT>/<A>/<BUTTON>/<SELECT>)":
                            xpath += "//input[not(@type='hidden')] | //a[not(@type='hidden')] | //select[not(@type='hidden')] | //button[not(@type='hidden')]";
                            break;
                        case "Elements With Label":
                            xpath += "//*[not(@type='hidden') and @label and not(@label='')]";
                            break;
                        case "Editbox(<INPUT>)":
                            xpath += "//input[not(@type='hidden')]";
                            break;
                        case "Link/Buttons(<A>/<BUTTON>)":
                            xpath += "//a[not(@type='hidden')] | //button[not(@type='hidden')]";
                            break;
                        case "Combo(<SELECT>)":
                            xpath += "//select[not(@type='hidden')]";
                            break;
                        case "Label/Text(<SPAN>/<LABEL>)":
                            xpath += "//span[not(@type='hidden')] | //label[not(@type='hidden')]";
                            break;
                        case "Images(<IMG>)":
                            xpath += "//img[not(@type='hidden')]";
                            break;
                        case "Block(<DIV>)":
                            xpath += "//div[not(@type='hidden')]";
                            break;
                        case "TABLE(<TABLE>)":
                            xpath += "//table[not(@type='hidden')]";
                            break;
                        case "Table Rows/Columns(<TD>/<TR>)":
                            xpath += "//tr[not(@type='hidden')] | //td[not(@type='hidden')]";
                            break;
                        case "Frame(<FRAME>)":
                            xpath += "//frame[not(@type='hidden')]";
                            break;
                    }
                }
            }
            return xpath;

        }

        private void btn_navigate_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lbl_uri.Text) && !string.IsNullOrEmpty(cmb_browser.SelectedItem.ToString()))
            {
                uicode.navigateToUrl(cmb_browser.SelectedItem.ToString().Trim(), lbl_uri.Text.ToString().Trim());
                //btn_getElement.Visible = true;
                btn_getElement.Enabled = true;


            }
            else
            {
                MessageBox.Show("Please provide inputs for BrowserType, Application & Environment...", "Object Spy", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_getElement_Click(object sender, EventArgs e)
        {

            lbl_inprogress.Visible = true;
            lbl_inprogress.Text = "Please wait.Retrieving Objects from Page...";
            this.Update();
            Dictionary<int, string> elementProperties = uicode.getElements(desiredXPath());
            string[] tagname;
            dtglb = new DataTable();  //here dtglb is a global datatable
            dtglb.Columns.Add("tagname", typeof(string));
            dtglb.Columns.Add("controlID", typeof(string));
            dtglb.Columns.Add("name", typeof(string));
            dtglb.Columns.Add("label", typeof(string));
            dtglb.Columns.Add("abs_xpath", typeof(string));
            dtglb.Columns.Add("href", typeof(string));
            dtglb.Columns.Add("class", typeof(string));
            dtglb.Columns.Add("text", typeof(string));
            dtglb.Columns.Add("rel_xpath", typeof(string));
            dtglb.Columns.Add("frame_name", typeof(string));


            foreach (int key in elementProperties.Keys)
            {
                dtglb.NewRow();
                tagname = elementProperties[key].Split('~');
                dtglb.Rows.Add(tagname[0], tagname[1], tagname[2], tagname[3], tagname[4], tagname[5], tagname[6], tagname[7], tagname[8], tagname[9]);
            }
            grid_cloner.DataSource = dtglb;
            lbl_inprogress.Text = "";
            lbl_inprogress.Visible = false;
            this.Update();
            //btn_addToOR.Visible = true;
            //btn_highlight.Visible = true;
        }


        private void grid_cloner_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btn_addToOR.Enabled = true;
            btn_highlight.Enabled = true;
            //clean al rows
            foreach (DataGridViewRow row in grid_cloner.Rows)
            {
                row.Cells[0].Value = false;
            }

            //check select row
            grid_cloner.CurrentRow.Cells[0].Value = true;
        }
        private void grid_displayResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //btn_addToOR.Visible = false;
            btn_highlight.Enabled = true;
            btn_editOR.Enabled = true;
            //clean al rows
            foreach (DataGridViewRow row in grid_displayResult.Rows)
            {
                row.Cells[0].Value = false;
            }

            //check select row
            grid_cloner.CurrentRow.Cells[0].Value = true;

        }



        public void btn_highlight_Click(object sender, EventArgs e)
        {
            if (tabName.Equals("Objects Found in the Page"))
            {
                HighlighterScreen ohlt = new HighlighterScreen(grid_cloner.CurrentRow.Cells[10].Value.ToString().Trim(), grid_cloner.CurrentRow.Cells[2].Value.ToString().Trim(), grid_cloner.CurrentRow.Cells[3].Value.ToString().Trim(), grid_cloner.CurrentRow.Cells[5].Value.ToString().Trim(), grid_cloner.CurrentRow.Cells[6].Value.ToString().Trim(), grid_cloner.CurrentRow.Cells[7].Value.ToString().Trim(), grid_cloner.CurrentRow.Cells[8].Value.ToString().Trim(), grid_cloner.CurrentRow.Cells[9].Value.ToString().Trim());
                ohlt.ShowDialog();
            }
            else if (tabName.Equals("Existing Objects in Repository"))
            {
                int masterORID = Int32.Parse(grid_displayResult.CurrentRow.Cells[1].Value.ToString().Trim());
                string frameID = "";
                string frameName = "";
                string tempFrameName = "";
                string[] objParams = null;
                string frameQuery = "select ParentControl,PageID,ProjectID from MasterOR where MasterORID=" + masterORID;
                try
                {
                    using (SqlConnection con = new SqlConnection(objLib.dbConnectionString))
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand(frameQuery, con))
                        {
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                                objParams = new string[] { reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString() };
                        }

                        tempFrameName = objParams[0];
                        frameName = tempFrameName;

                        while (!tempFrameName.Equals(""))
                        {
                            string subframeNameQuery = "Select ParentControl from MasterOR where Label = '" + tempFrameName + "' and PageID = " + Int32.Parse(objParams[1]) + " and ProjectId = " + Int32.Parse(objParams[2]);

                            SqlDataReader reader1 = null;
                            SqlCommand command1 = null;
                            using (SqlConnection con1 = new SqlConnection(objLib.dbConnectionString))
                            {
                                con1.Open();
                                using (command1 = new SqlCommand(subframeNameQuery, con1))
                                {
                                    reader1 = command1.ExecuteReader();
                                    while (reader1.Read())
                                        tempFrameName = reader1.GetValue(0).ToString();
                                }
                                reader1.Close();
                                if (!tempFrameName.Equals(""))
                                    frameName += ";" + tempFrameName;
                            }
                        }
                        frameID = "";
                        string tempFrameID = "";
                        string[] framenames = frameName.Split(';');
                        for (int i = 0; i <= framenames.Count() - 1; i++)
                        {
                            string subFrameIDQuery = "select ControlID from MasterOR where Label = '" + framenames[i] + "' and PageID = " + Int32.Parse(objParams[1]) + " and ProjectId = " + Int32.Parse(objParams[2]);
                            SqlDataReader reader2 = null;
                            SqlCommand command2 = null;
                            using (SqlConnection con2 = new SqlConnection(objLib.dbConnectionString))
                            {
                                con2.Open();
                                using (command2 = new SqlCommand(subFrameIDQuery, con2))
                                {
                                    reader2 = command2.ExecuteReader();
                                    while (reader2.Read())
                                        tempFrameID = reader2.GetValue(0).ToString();
                                }
                                reader2.Close();

                                frameID += tempFrameID + ";";
                            }
                        }
                        frameID = frameID.TrimEnd(';');
                        string tmpVar = "";
                        string[] frameIDs = frameID.Split(';');
                        for (int i = frameIDs.Count() - 1; i >= 0; i--)
                        {
                            tmpVar += frameIDs[i] + ";";
                        }
                        frameID = tmpVar.TrimEnd(';');
                    }
                }
                catch
                {

                }




                string sqlquery = "select ControlID,Xpath,ClassName from MasterOR M where MasterORID= " + masterORID;
                string[] values = null;
                try
                {
                    using (SqlConnection con = new SqlConnection(objLib.dbConnectionString))
                    {
                        con.Open();
                        using (SqlCommand command = new SqlCommand(sqlquery, con))
                        {
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                                values = new string[] { reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString() };
                        }
                    }
                }
                catch
                {

                }
                HighlighterScreen ohlt = new HighlighterScreen(frameID, values[0], "", "", "", values[2], "", values[1]);
                ohlt.ShowDialog();
            }

        }

        public void btn_testObjects_Click(object sender, EventArgs e)
        {
            lbl_inprogress.Visible = true;
            lbl_inprogress.Text = "Please wait. Validating Objects on the page displayed...";
            this.Update();
            lbl_mismatch.Visible = false;
            this.Update();
            bool tempVar = true;
            int mismatchCount = 0;
            for (int i = 0; i <= grid_displayResult.RowCount - 2; i++)
            {
                try
                {
                    int masterORID = Int32.Parse(grid_displayResult.Rows[i].Cells[1].Value.ToString().Trim());
                    string frameID = "";
                    string frameName = "";
                    string tempFrameName = "";
                    string[] objParams = null;
                    string frameQuery = "select ParentControl,PageID,ProjectID from MasterOR where MasterORID=" + masterORID;
                    try
                    {
                        using (SqlConnection con = new SqlConnection(objLib.dbConnectionString))
                        {
                            con.Open();
                            using (SqlCommand command = new SqlCommand(frameQuery, con))
                            {
                                SqlDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                    objParams = new string[] { reader.GetValue(0).ToString(), reader.GetValue(1).ToString(), reader.GetValue(2).ToString() };
                            }

                            tempFrameName = objParams[0];
                            frameName = tempFrameName;

                            while (!tempFrameName.Equals(""))
                            {
                                string subframeNameQuery = "Select ParentControl from MasterOR where Label = '" + tempFrameName + "' and PageID = " + Int32.Parse(objParams[1]) + " and ProjectId = " + Int32.Parse(objParams[2]);
                                tempFrameName = "";
                                SqlDataReader reader1 = null;
                                SqlCommand command1 = null;
                                using (SqlConnection con1 = new SqlConnection(objLib.dbConnectionString))
                                {
                                    con1.Open();
                                    using (command1 = new SqlCommand(subframeNameQuery, con1))
                                    {
                                        reader1 = command1.ExecuteReader();
                                        while (reader1.Read())
                                            tempFrameName = reader1.GetValue(0).ToString();
                                    }
                                    reader1.Close();
                                    if (!tempFrameName.Equals(""))
                                        frameName += ";" + tempFrameName;
                                }
                            }
                            frameID = "";
                            string tempFrameID = "";
                            string[] framenames = frameName.Split(';');
                            for (int j = 0; j <= framenames.Count() - 1; j++)
                            {
                                string subFrameIDQuery = "select ControlID from MasterOR where Label = '" + framenames[j] + "' and PageID = " + Int32.Parse(objParams[1]) + " and ProjectId = " + Int32.Parse(objParams[2]);
                                SqlDataReader reader2 = null;
                                SqlCommand command2 = null;
                                using (SqlConnection con2 = new SqlConnection(objLib.dbConnectionString))
                                {
                                    con2.Open();
                                    using (command2 = new SqlCommand(subFrameIDQuery, con2))
                                    {
                                        reader2 = command2.ExecuteReader();
                                        while (reader2.Read())
                                            tempFrameID = reader2.GetValue(0).ToString();
                                    }
                                    reader2.Close();

                                    frameID += tempFrameID + ";";
                                }
                            }
                            frameID = frameID.TrimEnd(';');
                            string tmpVar = "";
                            string[] frameIDs = frameID.Split(';');
                            for (int k = frameIDs.Count() - 1; k >= 0; k--)
                            {
                                tmpVar += frameIDs[k] + ";";
                            }
                            frameID = tmpVar.TrimEnd(';');
                        }
                    }
                    catch
                    {

                    }
                    //int masterORID = Int32.Parse(grid_displayResult.Rows[i].Cells[1].Value.ToString().Trim());
                    //string frameID = null;
                    //string sqlquery = "select (select ControlID from MasterOR where Label=M.ParentControl and PageID=M.PageID and ProjectId=M.ProjectID) as FrameID from MasterOR M where MasterORID= " + masterORID;
                    //using (SqlConnection con = new SqlConnection(objLib.dbConnectionString))
                    //{
                    //    con.Open();
                    //    using (SqlCommand command = new SqlCommand(sqlquery, con))
                    //    {
                    //        SqlDataReader reader = command.ExecuteReader();
                    //        while (reader.Read())
                    //            frameID = reader.GetValue(0).ToString().Trim();
                    //    }
                    //}

                    //get frame name          
                    if (!grid_displayResult.Rows[i].Cells["ControlID"].Value.ToString().Trim().Equals(""))
                        tempVar = uicode.verifyElement(frameID, "id", grid_displayResult.Rows[i].Cells["ControlID"].Value.ToString().Trim());
                    else if (!grid_displayResult.Rows[i].Cells["Xpath"].Value.ToString().Trim().Equals(""))
                        tempVar = uicode.verifyElement(frameID, "xpath", grid_displayResult.Rows[i].Cells["Xpath"].Value.ToString().Trim());

                    if (tempVar == false)
                    {
                        grid_displayResult.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                        mismatchCount += 1;
                        grid_displayResult.Refresh();
                    }
                    //else
                    //{
                    //    grid_displayResult.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                    //}
                }
                catch
                {

                }

            }
            lbl_inprogress.Text = "";
            lbl_inprogress.Visible = false;
            this.Update();
            lbl_mismatch.Visible = true;
            if (mismatchCount > 0)
            {
                lbl_mismatch.ForeColor = Color.Red;
                lbl_mismatch.Text = "Total number of Objects mismatch - " + mismatchCount + " \r\t Please update Object Properties.";
            }
            else
            {
                lbl_mismatch.ForeColor = Color.Blue;
                lbl_mismatch.Text = "All Objects Validated on the Page. NO mismatch found.";
            }
            this.Update();

        }

        //uicode.highlightElement(grid_cloner.CurrentRow.Cells[6].Value.ToString().Trim(),grid_cloner.CurrentRow.Cells[2].Value.ToString().Trim());

        public void btn_addtoOR_Click(object sender, EventArgs e)
        {
            var Admin = objLib.GetAdminUsers().Where(x => ((x.Value.ToLower() == "yes") || (x.Value.ToLower() == "superuser")) && x.Key.ToUpper() == SignIn.userId.ToUpper()).Select(x => x.Key).ToArray();
            if (Admin.Length != 0)
            {

                ObjectRepositiory objOR = new ObjectRepositiory();
                objOR.lbl_pagetitle.Text = "CREATE OR ENTRY";
                objOR.btn_saveorentry.Visible = false;
                //objOR.ShowDialog();
                objOR.Show();
                objOR.txt_tagname.Text = grid_cloner.CurrentRow.Cells[1].Value.ToString().Trim().ToUpper();
                objOR.txt_controltype.Text = grid_cloner.CurrentRow.Cells[7].Value.ToString().Trim().ToUpper();
                objOR.txt_label.Text = grid_cloner.CurrentRow.Cells[4].Value.ToString().Trim();

                // if (!grid_cloner.CurrentRow.Cells[2].Value.ToString().Trim().Equals(""))
                objOR.txt_controlid.Text = grid_cloner.CurrentRow.Cells[2].Value.ToString().Trim();
                // else if (!grid_cloner.CurrentRow.Cells[9].Value.ToString().Trim().Equals(""))
                objOR.txt_xpath.Text = grid_cloner.CurrentRow.Cells[9].Value.ToString().Trim();
                objOR.txt_repositiory.Text = "Master";
                objOR.txt_friendlyname.Text = "NEW";




            }
            else
            {
                MessageBox.Show("Sorry...You don't have rights to Create Object in OR.", "Create OR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        public void btn_editOR_Click(object sender, EventArgs e)
        {
            var Admin = objLib.GetAdminUsers().Where(x => ((x.Value.ToLower() == "yes") || (x.Value.ToLower() == "superuser")) && x.Key.ToUpper() == SignIn.userId.ToUpper()).Select(x => x.Key).ToArray();
            if (Admin.Length != 0)
            {
                ObjectRepositiory or = new ObjectRepositiory();
                or.btn_createorentry.Visible = false;
                or.txt_masterorid.Text = grid_displayResult.CurrentRow.Cells["MasterORID"].Value.ToString();
                or.cmb_pagename.Text = grid_displayResult.CurrentRow.Cells["PageName"].Value.ToString();
                or.txt_label.Text = grid_displayResult.CurrentRow.Cells["Label"].Value.ToString();
                or.txt_controltype.Text = grid_displayResult.CurrentRow.Cells["ControlType"].Value.ToString();
                or.txt_tagname.Text = grid_displayResult.CurrentRow.Cells["TagName"].Value.ToString();
                or.txt_controlid.Text = grid_displayResult.CurrentRow.Cells["ControlID"].Value.ToString();
                or.txt_labelfor.Text = grid_displayResult.CurrentRow.Cells["LabelFor"].Value.ToString();
                or.txt_xpath.Text = grid_displayResult.CurrentRow.Cells["Xpath"].Value.ToString();
                or.txt_classname.Text = grid_displayResult.CurrentRow.Cells["ClassName"].Value.ToString();
                or.txt_innertext.Text = grid_displayResult.CurrentRow.Cells["InnerText"].Value.ToString();
                or.txt_class.Text = grid_displayResult.CurrentRow.Cells["Class"].Value.ToString();
                or.txt_type.Text = grid_displayResult.CurrentRow.Cells["Type"].Value.ToString();
                or.txt_parentcontrol.Text = grid_displayResult.CurrentRow.Cells["ParentControl"].Value.ToString();
                or.txt_friendlyname.Text = grid_displayResult.CurrentRow.Cells["FriendlyName"].Value.ToString();
                or.txt_valueattribute.Text = grid_displayResult.CurrentRow.Cells["ValueAttribute"].Value.ToString();
                or.txt_taginstance.Text = grid_displayResult.CurrentRow.Cells["TagInstance"].Value.ToString();
                or.txt_ctrldefinition.Text = grid_displayResult.CurrentRow.Cells["ControlDefinition"].Value.ToString();
                or.txt_repositiory.Text = grid_displayResult.CurrentRow.Cells["Version"].Value.ToString();
                //or.cmb_CreatedBy.Text = grid_displayResult.CurrentRow.Cells["CreatedBy"].Value.ToString();
                //or.cmb_UpdatedBy.Text = grid_displayResult.CurrentRow.Cells["LastUpdatedBy"].Value.ToString();
                or.Show();
            }
            else
            {
                MessageBox.Show("Sorry...You don't have rights to Edit Object in OR.", "Create OR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }



    }
}
