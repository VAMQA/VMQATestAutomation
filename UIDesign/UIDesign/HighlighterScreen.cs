using VM.Platform.TestAutomationFramework.Adapters.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIDesign
{
    public partial class HighlighterScreen : Form
    {

        private Label lbl_pagetitle = new Label();
        private Label lbl_frame = new Label();
        private CheckBox chk_ID = new CheckBox();
        private Label lbl_ID = new Label();
        private CheckBox chk_xpath = new CheckBox();
        private Label lbl_xpath = new Label();
        private CheckBox chk_customXpath = new CheckBox();
        private Label lbl_customXpath = new Label();
        private TextBox txt_customXpath = new TextBox();
        private Button btn_customXpath = new Button();
        private CheckBox chk_name = new CheckBox();
        private Label lbl_name = new Label();
        private CheckBox chk_label = new CheckBox();
        private Label lbl_label = new Label();
        private CheckBox chk_href = new CheckBox();
        private Label lbl_href = new Label();
        private CheckBox chk_class = new CheckBox();
        private Label lbl_class = new Label();
        private CheckBox chk_text = new CheckBox();
        private Label lbl_text = new Label();
        private CheckBox chk_relativeXpath = new CheckBox();
        private Label lbl_relativeXpath = new Label();
        private Label lbl_blank = new Label();
        public DataGridView grid_SelectProperty = new DataGridView();
        public DataGridViewCheckBoxColumn chk_selection1 = new DataGridViewCheckBoxColumn();
        private Label lbl_message = new Label();

        private PictureBox pic_gecko = new PictureBox();
        private BindingSource bindingSrc = new BindingSource();
        private FlowLayoutPanel flp_testcloner = new FlowLayoutPanel();
        private TableLayoutPanel tlp_testcloner = new TableLayoutPanel();
        FuncLib objLib = new FuncLib();
        UIScrapperCode uicode = new UIScrapperCode();
        int tcid = 0;
        private DataTable dtglb;
        public string frameName;
        public string id;
        public string name;
        public string xpath;
        public string href;
        public string className;
        public string text;
        public string relativeXpath;




        public HighlighterScreen(string frameName, string id, string name, string xpath, string href, string className, string text, string relativeXpath)
        {
            this.frameName = frameName;
            this.id = id;
            this.name = name;
            this.xpath = xpath;
            this.href = href;
            this.className = className;
            this.text = text;
            this.relativeXpath = relativeXpath;

            InitializeComponent();


            #region testcloner_design

            //Form Settings
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            this.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 400, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 100);
            this.AutoScroll = true;
            this.Text = "HighLight Element";

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
            pic_gecko.Height = 150;
            pic_gecko.Width = 220;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            lbl_blank.Text = "";
            lbl_blank.Name = "lbl_blank";
            lbl_blank.TextAlign = ContentAlignment.MiddleLeft;
            lbl_blank.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_blank.Height = 20;
            lbl_blank.Width = 40;


            // CheckBox  settings
            chk_relativeXpath.Name = "chk_relativeXpath";
            chk_relativeXpath.Height = 24;
            chk_relativeXpath.Width = 20;

            lbl_relativeXpath.Text = "RELATIVE XPATH: " + this.relativeXpath;
            lbl_relativeXpath.Name = "lbl_relativeXpath";
            lbl_relativeXpath.TextAlign = ContentAlignment.MiddleLeft;
            lbl_relativeXpath.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_relativeXpath.Height = 20;
            lbl_relativeXpath.Width = 600;
            //lbl_relativeXpath.AutoSize = true;
            lbl_message.Text = "";
            lbl_message.Name = "lbl_message";
            lbl_message.TextAlign = ContentAlignment.MiddleLeft;
            lbl_message.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_message.Height = 20;
            lbl_message.Width = 900;
            // CheckBox  settings
            chk_customXpath.Name = "chk_customXpath";
            chk_customXpath.Height = 24;
            chk_customXpath.Width = 20;

            lbl_customXpath.Text = "Custom XPath : ";
            lbl_customXpath.Name = "lbl_customXpath";
            lbl_customXpath.TextAlign = ContentAlignment.MiddleLeft;
            lbl_customXpath.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_customXpath.Height = 20;
            lbl_customXpath.Width = 600;

            txt_customXpath.Text = "";
            txt_customXpath.Name = "txt_customXpath";
            txt_customXpath.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_customXpath.Height = 30;
            txt_customXpath.Width = 400;
            txt_customXpath.ReadOnly = false;
            txt_customXpath.Enabled = false;

            btn_customXpath.Text = "Highlight";
            btn_customXpath.Name = "btn_customXpath";
            btn_customXpath.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_customXpath.Height = 30;
            btn_customXpath.Width = 100;
            btn_customXpath.FlatStyle = FlatStyle.Flat;
            btn_customXpath.Anchor = AnchorStyles.Left;
            btn_customXpath.Enabled = false;

            //Userid Label settings           
            lbl_pagetitle.Height = 60;
            lbl_pagetitle.Width = 1000;
            lbl_pagetitle.Text = "Highlight Element";
            lbl_pagetitle.Name = "lbl_pagetitle";
            lbl_pagetitle.TextAlign = ContentAlignment.TopLeft;
            lbl_pagetitle.Font = new Font("Calibri", 20F, FontStyle.Bold);
            lbl_pagetitle.ForeColor = Color.Blue;


            grid_SelectProperty.Size = new Size(800, 220);
            grid_SelectProperty.DefaultCellStyle.Font = new Font("Calibri", 10);
            grid_SelectProperty.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
            grid_SelectProperty.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8F, FontStyle.Bold);
            grid_SelectProperty.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            grid_SelectProperty.AllowUserToAddRows = false;

            chk_selection1.Width = 30;
            chk_selection1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid_SelectProperty.Columns.Insert(0, chk_selection1);

            tlp_testcloner.Controls.Add(lbl_pagetitle, 1, 1);
            tlp_testcloner.SetColumnSpan(lbl_pagetitle, 2);


            tlp_testcloner.Controls.Add(grid_SelectProperty, 1, 2);
            tlp_testcloner.SetColumnSpan(grid_SelectProperty, 4);
            //tlp_testcloner.SetRowSpan(grid_SelectProperty, 8);
            tlp_testcloner.Controls.Add(chk_customXpath, 0, 3);
            tlp_testcloner.Controls.Add(lbl_customXpath, 1, 3);
            tlp_testcloner.Controls.Add(txt_customXpath, 1, 4);
            tlp_testcloner.Controls.Add(btn_customXpath, 3, 4);
            tlp_testcloner.Controls.Add(lbl_blank, 1, 5);
            tlp_testcloner.Controls.Add(lbl_message, 0, 6);
            tlp_testcloner.SetColumnSpan(lbl_message, 5);





            //Adding Controls to Flow Layout Panel
            flp_testcloner.Controls.AddRange(new Control[] { pic_gecko, tlp_testcloner });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_testcloner });
            this.Load += new System.EventHandler(HighlighterScreen_Load);

            #endregion

            #region testcloner_methods


            chk_customXpath.Click += new System.EventHandler(chk_CustomXpathCheckBox_Click);
            btn_customXpath.Click += new System.EventHandler(btn_CustomXpathCheckBox_Click);
            grid_SelectProperty.CellContentClick += new DataGridViewCellEventHandler(grid_SelectProperty_CellContentClick);

            #endregion
        }

        private void grid_SelectProperty_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 0 && e.RowIndex != 0)
            {
                foreach (DataGridViewRow row in grid_SelectProperty.Rows)
                {
                    row.Cells[0].Value = false;
                }

                //check select row
                grid_SelectProperty.CurrentRow.Cells[0].Value = true;
                if (uicode.highlightElement(frameName, grid_SelectProperty.CurrentRow.Cells[1].Value.ToString().ToLower(), grid_SelectProperty.CurrentRow.Cells[2].Value.ToString()))
                {
                    //lbl_message.ForeColor = Color.ForestGreen;
                    lbl_message.Text = "Element located successfully on the Application Window";
                    this.Update();
                }
                else
                {
                    lbl_message.Text = "Unable to Locate Object on the Application Window";
                    lbl_message.ForeColor = Color.Red;
                    this.Update();
                }
                this.Activate();
            }
        }

        private void HighlighterScreen_Load(object sender, EventArgs e)
        {

            //populate properties values
            string[] properties = new string[] { "FRAME NAME", "ID", "NAME", "ABSOLUTE XPATH", "HREF", "CLASSNAME", "TEXT", "RELATIVE XPATH" };
            string[] values = new string[] { this.frameName, this.id, this.name, this.xpath, this.href, this.className, this.text, this.relativeXpath };

            DataTable dtloc = new DataTable();
            dtloc.Columns.Add("Properties", typeof(string));
            dtloc.Columns.Add("Values", typeof(string));
            for (int i = 0; i <= properties.Length - 1; i++)
            {
                dtloc.NewRow();
                dtloc.Rows.Add(properties[i], values[i]);
            }
            grid_SelectProperty.DataSource = dtloc;
            //grid_SelectProperty.Rows[0].Cells[0];
            grid_SelectProperty.Columns[0].Width = 40;
            grid_SelectProperty.Columns[1].Width = 100;
            grid_SelectProperty.Columns[2].Width = 500;
            grid_SelectProperty.Rows[0].ReadOnly = true;
            grid_SelectProperty.Rows[0].DefaultCellStyle.BackColor = Color.LightGray;
            lbl_message.Text = "Please select an option to highlight element";
            lbl_message.ForeColor = Color.Blue;
            this.Update();
        }



        private void chk_CustomXpathCheckBox_Click(object sender, EventArgs e)
        {
            if (chk_customXpath.Checked == true)
            {
                chk_ID.Checked = false;
                chk_xpath.Checked = false;
                chk_name.Checked = false;
                chk_href.Checked = false;
                chk_class.Checked = false;
                chk_text.Checked = false;
                chk_relativeXpath.Checked = false;
                txt_customXpath.Enabled = true;
                btn_customXpath.Enabled = true;

            }
        }

        private void btn_CustomXpathCheckBox_Click(object sender, EventArgs e)
        {
            if (!txt_customXpath.Equals(""))
            {
                if (uicode.highlightElement(frameName, "custom_xpath", txt_customXpath.Text.ToString().Trim()))
                {
                    this.Refresh();
                    lbl_message.ForeColor = Color.Blue;
                    lbl_message.Text = "Element located on Application Window";

                }
                else
                {
                    this.Refresh();
                    lbl_message.Text = "Unable to locate element";
                    lbl_message.ForeColor = Color.Red;

                }
                this.Activate();
                this.Update();

                //Form.ActiveForm.Focus();
            }
        }

    }
}
