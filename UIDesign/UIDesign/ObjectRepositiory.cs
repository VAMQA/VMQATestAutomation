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
    public partial class ObjectRepositiory : Form
    {
        private Label lbl_pagename = new Label();
        private Label lbl_label = new Label();
        private Label lbl_controltype = new Label();
        private Label lbl_tagname = new Label();
        private Label lbl_controlid = new Label();
        private Label lbl_labelfor = new Label();
        private Label lbl_xpath = new Label();
        private Label lbl_innertext = new Label();
        private Label lbl_class = new Label();
        private Label lbl_type = new Label();
        private Label lbl_parentcontrol = new Label();
        private Label lbl_friendlyname = new Label();
        private Label lbl_valueattribute = new Label();
        private Label lbl_taginstance = new Label();
        private Label lbl_classname = new Label();
        private Label lbl_ctrldefinition = new Label();
        private Label lbl_errmsg = new Label();
        public Label lbl_pagetitle = new Label();
        public Label lbl_repositiory = new Label();

        public ComboBox cmb_pagename = new ComboBox();
        public TextBox txt_pageid = new TextBox();
        public TextBox txt_masterorid = new TextBox();
        public TextBox txt_label = new TextBox();
        public TextBox txt_controltype = new TextBox();
        public TextBox txt_tagname = new TextBox();
        public TextBox txt_controlid = new TextBox();
        public TextBox txt_labelfor = new TextBox();
        public TextBox txt_xpath = new TextBox();
        public TextBox txt_innertext = new TextBox();
        public TextBox txt_class = new TextBox();
        public TextBox txt_type = new TextBox();
        public TextBox txt_parentcontrol = new TextBox();
        public TextBox txt_friendlyname = new TextBox();
        public TextBox txt_valueattribute = new TextBox();
        public TextBox txt_taginstance = new TextBox();
        public TextBox txt_classname = new TextBox();
        public TextBox txt_ctrldefinition = new TextBox();
        public TextBox txt_repositiory = new TextBox();

        public Button btn_saveorentry = new Button();
        public Button btn_createorentry = new Button();

        private PictureBox pic_gecko = new PictureBox();
        private FlowLayoutPanel flp_repositiory = new FlowLayoutPanel();
        private TableLayoutPanel tlp_repositiory = new TableLayoutPanel();
        
        FuncLib objLib = new FuncLib();

        public ObjectRepositiory()
        {
            InitializeComponent();

            #region or_design

            //Form Settings
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            //this.Size = new Size(1010, 630);
            this.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width-300, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 170);
            this.AutoScroll = true;
            this.Text = "Object Repository";
            this.MaximizeBox = true;
            this.MinimizeBox = false;

            //Flow Layout Panel Settings            
            flp_repositiory.FlowDirection = FlowDirection.LeftToRight;
            //flp_repositiory.SetFlowBreak(pic_gecko, true);
            flp_repositiory.Dock = DockStyle.Top;
            flp_repositiory.AutoSize = true;

            //Table Layout Panel Settings                                    
            tlp_repositiory.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_repositiory.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_repositiory.AutoSize = true;
            tlp_repositiory.Location = new Point(30, 80);
            //tlp_repositiory.AutoScroll = true;

            //Gecko Picture Box Settings            
            pic_gecko.Height = 150;
            pic_gecko.Width = 200;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            //Page Title Label settings           
            lbl_pagetitle.Height = 20;
            lbl_pagetitle.Width = 1800;
            lbl_pagetitle.Text = "";
            lbl_pagetitle.Name = "lbl_pagetitle";
            lbl_pagetitle.TextAlign = ContentAlignment.MiddleCenter;
            lbl_pagetitle.Font = new Font("Calibri", 16F, FontStyle.Underline);
            lbl_pagetitle.ForeColor = Color.Blue;

            //Page Name Label settings            
            lbl_pagename.Text = "Page Name :*";
            lbl_pagename.Name = "lbl_pagename";
            lbl_pagename.TextAlign = ContentAlignment.MiddleLeft;
            lbl_pagename.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_pagename.Height = 24;
            lbl_pagename.Width = 110;

            //Label settings            
            lbl_label.Text = "Label :*";
            lbl_label.Name = "lbl_label";
            lbl_label.TextAlign = ContentAlignment.MiddleLeft;
            lbl_label.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_label.Height = 24;
            lbl_label.Width = 110;

            //ControlType Label settings            
            lbl_controltype.Text = "Control Type :";
            lbl_controltype.Name = "lbl_controltype ";
            lbl_controltype.TextAlign = ContentAlignment.MiddleLeft;
            lbl_controltype.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_controltype.Height = 24;
            lbl_controltype.Width = 110;

            //TagName Name Label settings            
            lbl_tagname.Text = "Tag Name :*";
            lbl_tagname.Name = "lbl_tagname";
            lbl_tagname.TextAlign = ContentAlignment.MiddleLeft;
            lbl_tagname.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_tagname.Height = 24;
            lbl_tagname.Width = 110;

            //ControlID Label settings            
            lbl_controlid.Text = "Control ID :*";
            lbl_controlid.Name = "lbl_controlid";
            lbl_controlid.TextAlign = ContentAlignment.MiddleLeft;
            lbl_controlid.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_controlid.Height = 24;
            lbl_controlid.Width = 110;

            //LabelFor Label settings            
            lbl_labelfor.Text = "Label For :*";
            lbl_labelfor.Name = "lbl_labelfor ";
            lbl_labelfor.TextAlign = ContentAlignment.MiddleLeft;
            lbl_labelfor.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_labelfor.Height = 24;
            lbl_labelfor.Width = 110;

            //Xpath Label settings            
            lbl_xpath.Text = "Xpath :*";
            lbl_xpath.Name = "lbl_xpath";
            lbl_xpath.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_xpath.Height = 24;
            lbl_xpath.Width = 110;

            //InnerText settings            
            lbl_innertext.Text = "Inner Text :*";
            lbl_innertext.Name = "lbl_innertext";
            lbl_innertext.TextAlign = ContentAlignment.MiddleLeft;
            lbl_innertext.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_innertext.Height = 24;
            lbl_innertext.Width = 110;

            //Class Label settings            
            lbl_class.Text = "Class :";
            lbl_class.Name = "lbl_class ";
            lbl_class.TextAlign = ContentAlignment.MiddleLeft;
            lbl_class.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_class.Height = 24;
            lbl_class.Width = 110;

            //Type Label settings            
            lbl_type.Text = "Type :";
            lbl_type.Name = "lbl_type";
            lbl_type.TextAlign = ContentAlignment.MiddleLeft;
            lbl_type.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_type.Height = 24;
            lbl_type.Width = 110;

            //ParentControl Label settings            
            lbl_parentcontrol.Text = "Parent Control :";
            lbl_parentcontrol.Name = "lbl_parentcontrol";
            lbl_parentcontrol.TextAlign = ContentAlignment.MiddleLeft;
            lbl_parentcontrol.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_parentcontrol.Height = 24;
            lbl_parentcontrol.Width = 110;

            //FriendlyName Label settings            
            lbl_friendlyname.Text = "Friendly Name :";
            lbl_friendlyname.Name = "lbl_friendlyname ";
            lbl_friendlyname.TextAlign = ContentAlignment.MiddleLeft;
            lbl_friendlyname.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_friendlyname.Height = 24;
            lbl_friendlyname.Width = 110;

            //ValueAttribute Label settings            
            lbl_valueattribute.Text = "Value Attribute :";
            lbl_valueattribute.Name = "lbl_valueattribute";
            lbl_valueattribute.TextAlign = ContentAlignment.MiddleLeft;
            lbl_valueattribute.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_valueattribute.Height = 24;
            lbl_valueattribute.Width = 110;

            //TagInstance settings            
            lbl_taginstance.Text = "Tag Instance :";
            lbl_taginstance.Name = "lbl_taginstance";
            lbl_taginstance.TextAlign = ContentAlignment.MiddleLeft;
            lbl_taginstance.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_taginstance.Height = 24;
            lbl_taginstance.Width = 110;

            //ClassName Label settings            
            lbl_classname.Text = "Class Name :";
            lbl_classname.Name = "lbl_classname ";
            lbl_classname.TextAlign = ContentAlignment.MiddleLeft;
            lbl_classname.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_classname.Height = 24;
            lbl_classname.Width = 110;

            //CtrlDefinition Label settings            
            lbl_ctrldefinition.Text = "Control Definition :";
            lbl_ctrldefinition.Name = "lbl_ctrldefinition";
            lbl_ctrldefinition.TextAlign = ContentAlignment.MiddleLeft;
            lbl_ctrldefinition.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_ctrldefinition.Height = 24;
            lbl_ctrldefinition.Width = 135;

            //CtrlDefinition Label settings            
            lbl_repositiory.Text = "Repository :*";
            lbl_repositiory.Name = "lbl_repositiory";
            lbl_repositiory.TextAlign = ContentAlignment.MiddleLeft;
            lbl_repositiory.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_repositiory.Height = 24;
            lbl_repositiory.Width = 135;

            //Error Message Label settings           
            lbl_errmsg.Height = 50;
            lbl_errmsg.Width = 600;
            lbl_errmsg.Name = "lbl_errmsg";
            lbl_errmsg.TextAlign = ContentAlignment.BottomLeft;
            lbl_errmsg.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_errmsg.ForeColor = Color.Red;
            lbl_errmsg.Text = "";

            //Page Name combobox settings                                  
            cmb_pagename.Items.AddRange((from t in objLib.GetPageTitles() orderby t.Value select t.Value).ToArray());
            cmb_pagename.Name = "cmb_pagename";
            cmb_pagename.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_pagename.Height = 24;
            cmb_pagename.Width = 300;
            cmb_pagename.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_pagename.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_pagename.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;


            //Label combobox settings            
            txt_label.Text = DBNull.Value.ToString();
            txt_label.Name = "txt_label";
            txt_label.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_label.Height = 24;
            txt_label.Width = 300;

            //ControlType textbox settings            
            txt_controltype.Text = null;
            txt_controltype.Name = "txt_controltype";
            txt_controltype.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_controltype.Height = 24;
            txt_controltype.Width = 300;

            //TagName textbox settings            
            txt_tagname.Text = null;
            txt_tagname.Name = "txt_tagname";
            txt_tagname.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_tagname.Height = 24;
            txt_tagname.Width = 300;

            //ControlID textbox settings            
            txt_controlid.Text = null;
            txt_controlid.Name = "txt_controlid";
            txt_controlid.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_controlid.Height = 24;
            txt_controlid.Width = 300;

            //LabelFor textbox settings            
            txt_labelfor.Text = null;
            txt_labelfor.Name = "txt_labelfor";
            txt_labelfor.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_labelfor.Height = 24;
            txt_labelfor.Width = 300;

            //Xpath textbox settings            
            txt_xpath.Text = null;
            txt_xpath.Name = "txt_xpath";
            txt_xpath.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_xpath.Height = 24;
            txt_xpath.Width = 300;

            //InnerText textbox settings            
            txt_innertext.Text = null;
            txt_innertext.Name = "txt_innertext";
            txt_innertext.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_innertext.Height = 24;
            txt_innertext.Width = 300;

            //Class textbox settings            
            txt_class.Text = null;
            txt_class.Name = "txt_class";
            txt_class.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_class.Height = 24;
            txt_class.Width = 300;

            //Type textbox settings            
            txt_type.Text = null;
            txt_type.Name = "txt_type";
            txt_type.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_type.Height = 24;
            txt_type.Width = 300;

            //Parent Control textbox settings            
            txt_parentcontrol.Text = null;
            txt_parentcontrol.Name = "txt_parentcontrol";
            txt_parentcontrol.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_parentcontrol.Height = 24;
            txt_parentcontrol.Width = 300;

            //FriendlyName textbox settings            
            txt_friendlyname.Text = null;
            txt_friendlyname.Name = "txt_friendlyname";
            txt_friendlyname.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_friendlyname.Height = 24;
            txt_friendlyname.Width = 300;

            //ValueAttribute textbox settings            
            txt_valueattribute.Text = null;
            txt_valueattribute.Name = "txt_valueattribute";
            txt_valueattribute.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_valueattribute.Height = 24;
            txt_valueattribute.Width = 300;

            //TagInstance textbox settings            
            txt_taginstance.Text = null;
            txt_taginstance.Name = "txt_taginstance";
            txt_taginstance.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_taginstance.Height = 24;
            txt_taginstance.Width = 300;

            //ClassName textbox settings            
            txt_classname.Text = null;
            txt_classname.Name = "txt_classname";
            txt_classname.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_classname.Height = 24;
            txt_classname.Width = 300;

            //CtrlDefinition Control textbox settings            
            txt_ctrldefinition.Text = null;
            txt_ctrldefinition.Name = "txt_ctrldefinition";
            txt_ctrldefinition.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_ctrldefinition.Height = 24;
            txt_ctrldefinition.Width = 300;


            //CtrlDefinition Control textbox settings            
            //txt_repositiory.Text = "Master";
            txt_repositiory.Name = "txt_repositiory";
            txt_repositiory.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_repositiory.Height = 24;
            txt_repositiory.Width = 300;

            //Save button settings            
            btn_saveorentry.Text = "Save";
            btn_saveorentry.Name = "btn_saveorentry";
            btn_saveorentry.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_saveorentry.Height = 30;
            btn_saveorentry.Width = 100;

            //Create button settings            
            btn_createorentry.Text = "Create";
            btn_createorentry.Name = "btn_createorentry";
            btn_createorentry.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_createorentry.Height = 30;
            btn_createorentry.Width = 100;

            //Adding Controls to Table Layout Panel            
            //tlp_repositiory.Controls.Add(lbl_pagetitle, 0, 2);
            //tlp_repositiory.SetColumnSpan(lbl_pagetitle, 5);
            tlp_repositiory.Controls.Add(lbl_pagename, 0, 3);
            tlp_repositiory.Controls.Add(cmb_pagename, 1, 3);
            tlp_repositiory.Controls.Add(lbl_label, 3, 3);
            tlp_repositiory.Controls.Add(txt_label, 4, 3);
            tlp_repositiory.Controls.Add(lbl_tagname, 0, 4);
            tlp_repositiory.Controls.Add(txt_tagname, 1, 4);
            tlp_repositiory.Controls.Add(lbl_controlid, 3, 4);
            tlp_repositiory.Controls.Add(txt_controlid, 4, 4);
            tlp_repositiory.Controls.Add(lbl_labelfor, 0, 5);
            tlp_repositiory.Controls.Add(txt_labelfor, 1, 5);
            tlp_repositiory.Controls.Add(lbl_xpath, 3, 5);
            tlp_repositiory.Controls.Add(txt_xpath, 4, 5);
            tlp_repositiory.Controls.Add(lbl_innertext, 0, 6);
            tlp_repositiory.Controls.Add(txt_innertext, 1, 6);
            tlp_repositiory.Controls.Add(lbl_class, 3, 6);
            tlp_repositiory.Controls.Add(txt_class, 4, 6);
            tlp_repositiory.Controls.Add(lbl_type, 0, 7);
            tlp_repositiory.Controls.Add(txt_type, 1, 7);
            tlp_repositiory.Controls.Add(lbl_parentcontrol, 3, 7);
            tlp_repositiory.Controls.Add(txt_parentcontrol, 4, 7);
            tlp_repositiory.Controls.Add(lbl_classname, 0, 8);
            tlp_repositiory.Controls.Add(txt_classname, 1, 8);
            tlp_repositiory.Controls.Add(lbl_friendlyname, 3, 8);
            tlp_repositiory.Controls.Add(txt_friendlyname, 4, 8);
            tlp_repositiory.Controls.Add(lbl_valueattribute, 0, 9);
            tlp_repositiory.Controls.Add(txt_valueattribute, 1, 9);
            tlp_repositiory.Controls.Add(lbl_taginstance, 3, 9);
            tlp_repositiory.Controls.Add(txt_taginstance, 4, 9);
            tlp_repositiory.Controls.Add(lbl_controltype, 0, 10);
            tlp_repositiory.Controls.Add(txt_controltype, 1, 10);
            tlp_repositiory.Controls.Add(lbl_ctrldefinition, 3, 10);
            tlp_repositiory.Controls.Add(txt_ctrldefinition, 4, 10);
            tlp_repositiory.Controls.Add(lbl_repositiory, 0, 11);
            tlp_repositiory.Controls.Add(txt_repositiory, 1, 11);
            tlp_repositiory.Controls.Add(lbl_errmsg, 0, 12);
            tlp_repositiory.SetColumnSpan(lbl_errmsg, 5);
            tlp_repositiory.Controls.Add(btn_saveorentry, 2, 13);
            tlp_repositiory.Controls.Add(btn_createorentry, 2, 13);

            //Adding Controls to Flow Layout Panel
            flp_repositiory.Controls.AddRange(new Control[] { pic_gecko, tlp_repositiory });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_repositiory });

            #endregion

            #region or_methods
            btn_createorentry.Click += new System.EventHandler(btn_createorentry_Click);
            btn_saveorentry.Click += new System.EventHandler(btn_saveorentry_Click);
            cmb_pagename.SelectedIndexChanged += new System.EventHandler(cmb_pagenamechanged_Select);
            #endregion
        }
        private void ObjectRepositiory_Load(object sender, EventArgs e)
        {
            lbl_pagetitle.Width = this.Width;            
        }
        private bool CheckForDuplicateEntries()
        {
            var pageID = Int32.Parse(txt_pageid.Text);
            var objLabel = txt_label.Text;
            var controlID = txt_controlid.Text;
            var labelFor = txt_labelfor.Text;
            var xPath = txt_xpath.Text.Replace("'", "''");
            var innerText = txt_innertext.Text.Replace("'", "''");
            var repositoryVersion = txt_repositiory.Text;            
            bool duplicateflag = false;

            string[] properties = { "objLabel", "controlID", "labelFor", "xPath", "innerText" };

            foreach(var prop in properties)
            {
                string query = string.Empty;
                switch (prop)
                {
                    case "objLabel":
                        if(!string.IsNullOrEmpty(objLabel))
                        query = "SELECT COUNT(*) FROM MASTEROR WHERE PROJECTID=" + SignIn.projectId + " AND PAGEID=" + pageID + " AND LABEL='" + objLabel + "' AND [VERSION]='" + repositoryVersion + "'";
                        break;
                    case "controlID":
                        if (!string.IsNullOrEmpty(controlID))
                        query = "SELECT COUNT(*) FROM MASTEROR WHERE PROJECTID=" + SignIn.projectId + " AND PAGEID=" + pageID + " AND CONTROLID='" + controlID + "' AND [VERSION]='" + repositoryVersion + "'";
                        break;
                    case "labelFor":
                        if (!string.IsNullOrEmpty(labelFor))
                        query = "SELECT COUNT(*) FROM MASTEROR WHERE PROJECTID=" + SignIn.projectId + " AND PAGEID=" + pageID + " AND LABELFOR='" + labelFor + "' AND [VERSION]='" + repositoryVersion + "'";
                        break;
                    case "xPath":
                        if (!string.IsNullOrEmpty(xPath))
                        query = "SELECT COUNT(*) FROM MASTEROR WHERE PROJECTID=" + SignIn.projectId + " AND PAGEID=" + pageID + " AND XPATH='" + xPath + "' AND [VERSION]='" + repositoryVersion + "'";
                        break;
                    case "innerText":
                        if (!string.IsNullOrEmpty(innerText))
                        query = "SELECT COUNT(*) FROM MASTEROR WHERE PROJECTID=" + SignIn.projectId + " AND PAGEID=" + pageID + " AND INNERTEXT='" + innerText + "' AND [VERSION]='" + repositoryVersion + "'";
                        break;
                    default:
                        break;
                }
                if ((!string.IsNullOrEmpty(query)) && (Convert.ToInt32(objLib.ExecuteScalar(query)) != 0) &&(btn_createorentry.Text=="Create"))
                {
                    duplicateflag = true;
                    break;
                }                
            }
            return duplicateflag;
        }
        private bool CheckForDuplicateEntriesOnSave()
        {
            var pageID = Int32.Parse(txt_pageid.Text);
            var objLabel = txt_label.Text;
            var controlID = txt_controlid.Text;
            var labelFor = txt_labelfor.Text;
            var xPath = txt_xpath.Text.Replace("'", "''");
            var innerText = txt_innertext.Text.Replace("'", "''");
            var repositoryVersion = txt_repositiory.Text;
            var masterORID = txt_masterorid.Text;
            bool duplicateflag = false;

            string[] properties = { "objLabel", "controlID", "labelFor", "xPath", "innerText" };

            foreach (var prop in properties)
            {
                string query = string.Empty;
                switch (prop)
                {
                    case "objLabel":
                        if (!string.IsNullOrEmpty(objLabel))
                            query = "SELECT COUNT(*) FROM MASTEROR WHERE PROJECTID=" + SignIn.projectId + " AND PAGEID=" + pageID + " AND LABEL='" + objLabel + "' AND [VERSION]='" + repositoryVersion + "' AND MASTERORID!="+masterORID;
                        break;
                    case "controlID":
                        if (!string.IsNullOrEmpty(controlID))
                            query = "SELECT COUNT(*) FROM MASTEROR WHERE PROJECTID=" + SignIn.projectId + " AND PAGEID=" + pageID + " AND CONTROLID='" + controlID + "' AND [VERSION]='" + repositoryVersion + "' AND MASTERORID!="+masterORID;
                        break;
                    case "labelFor":
                        if (!string.IsNullOrEmpty(labelFor))
                            query = "SELECT COUNT(*) FROM MASTEROR WHERE PROJECTID=" + SignIn.projectId + " AND PAGEID=" + pageID + " AND LABELFOR='" + labelFor + "' AND [VERSION]='" + repositoryVersion + "' AND MASTERORID!="+masterORID;
                        break;
                    case "xPath":
                        if (!string.IsNullOrEmpty(xPath))
                            query = "SELECT COUNT(*) FROM MASTEROR WHERE PROJECTID=" + SignIn.projectId + " AND PAGEID=" + pageID + " AND XPATH='" + xPath + "' AND [VERSION]='" + repositoryVersion + "' AND MASTERORID!="+masterORID;
                        break;
                    case "innerText":
                        if (!string.IsNullOrEmpty(innerText))
                            query = "SELECT COUNT(*) FROM MASTEROR WHERE PROJECTID=" + SignIn.projectId + " AND PAGEID=" + pageID + " AND INNERTEXT='" + innerText + "' AND [VERSION]='" + repositoryVersion + "' AND MASTERORID!="+masterORID;
                        break;
                    default:
                        break;
                }
                if ((!string.IsNullOrEmpty(query)) && (Convert.ToInt32(objLib.ExecuteScalar(query)) != 0) && (btn_saveorentry.Text == "Save"))
                {
                    duplicateflag = true;
                    break;
                }
            }
            return duplicateflag;
        }
        private void btn_createorentry_Click(object sender, System.EventArgs e)
        {
            try
            {
                if(!CheckForDuplicateEntries())
                {
                    if ((!objLib.IsNullOrEmpty(cmb_pagename.SelectedItem.ToString()) &&
                        !objLib.IsNullOrEmpty(txt_label.Text.Trim()) &&
                        !objLib.IsNullOrEmpty(txt_repositiory.Text.Trim()) &&
                        //(SqlHelper.GetLabels.Select(x => x.Value).Where(x => x.ToLower() == txt_label.Text.Trim().ToLower()).Count() == 0) &&
                        !objLib.IsNullOrEmpty(txt_tagname.Text.Trim()) &&
                        (!objLib.IsNullOrEmpty(txt_controlid.Text.Trim()) ||
                        !objLib.IsNullOrEmpty(txt_labelfor.Text.Trim()) ||
                        !objLib.IsNullOrEmpty(txt_xpath.Text.Trim()) ||                     
                        !objLib.IsNullOrEmpty(txt_innertext.Text.Trim()))))
                    {
                        string strCommand = "INSERT INTO MASTEROR(PageID,Label,TagName,ControlID,LabelFor,Xpath,InnerText,ParentControl,Class,Type,FriendlyName,ValueAttribute,TagInstance,ClassName,ControlType,ControlDefinition,[Version],ProjectID,CreatedBy,CreatedDate) VALUES("
                            + Int32.Parse(txt_pageid.Text) + ",'"
                            + txt_label.Text + "','"
                            + txt_tagname.Text + "','"
                            + txt_controlid.Text + "','"
                            + txt_labelfor.Text + "','"
                            + txt_xpath.Text.Replace("'", "''") + "','"
                            + txt_innertext.Text.Replace("'", "''") + "','"
                            + txt_parentcontrol.Text + "','"
                            + txt_class.Text + "','"
                            + txt_type.Text + "','"
                            + txt_friendlyname.Text.Replace("'", "''") + "','"
                            + txt_valueattribute.Text.Replace("'", "''") + "','"
                            + txt_taginstance.Text + "','"
                            + txt_classname.Text + "','"
                            + txt_controltype.Text + "','"
                            + txt_ctrldefinition.Text.Replace("'", "''") + "','"
                            + txt_repositiory.Text.Replace("'", "''") + "',"
                            + SignIn.projectId + ",'"
                            + SignIn.userId + "','"
                            + DateTime.Now + "')";
                        objLib.RunQuery(strCommand);
                        lbl_errmsg.Text = "";
                        MessageBox.Show("Control Added to Object Repository Successfully", "Create OR Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        lbl_errmsg.Text = "Please provide Inputs for Mandatory fields :\n1. PageName 2. Label 3. TagName 4. Repository 5. ControlID (Or) LabelFor (Or) Xpath (Or) InnerText.";
                    }
                }
                else
                {
                    lbl_errmsg.Text = "Duplicate Control Exists.";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("ERROR : " + ex.Message, "Create OR Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }
        private void btn_saveorentry_Click(object sender, System.EventArgs e)
        {
            if (!CheckForDuplicateEntriesOnSave())
            {
                if (!objLib.IsNullOrEmpty(cmb_pagename.SelectedItem.ToString()) &&
                    !objLib.IsNullOrEmpty(txt_label.Text.Trim()) &&
                    !objLib.IsNullOrEmpty(txt_tagname.Text.Trim()) &&
                    !objLib.IsNullOrEmpty(txt_repositiory.Text.Trim()) &&
                    (!objLib.IsNullOrEmpty(txt_controlid.Text.Trim()) ||
                    !objLib.IsNullOrEmpty(txt_labelfor.Text.Trim()) ||
                    !objLib.IsNullOrEmpty(txt_xpath.Text.Trim()) ||
                    !objLib.IsNullOrEmpty(txt_innertext.Text.Trim())))
                {
                    try
                    {
                        string strCommand = "UPDATE MASTEROR SET PageID=" +
                            Int32.Parse(txt_pageid.Text) +
                            ",Label='" + txt_label.Text +
                            "',TagName='" + txt_tagname.Text +
                            "',ControlID='" + txt_controlid.Text +
                            "',LabelFor='" + txt_labelfor.Text +
                            "',Xpath='" + txt_xpath.Text.Replace("'", "''") +
                            "',InnerText='" + txt_innertext.Text.Replace("'", "''") +
                            "',ParentControl='" + txt_parentcontrol.Text +
                            "',Class='" + txt_class.Text +
                            "',Type='" + txt_type.Text +
                            "',FriendlyName='" + txt_friendlyname.Text.Replace("'", "''") +
                            "',ValueAttribute='" + txt_valueattribute.Text.Replace("'", "''") +
                            "',TagInstance='" + txt_taginstance.Text +
                            "',ClassName='" + txt_classname.Text +
                            "',ControlType='" + txt_controltype.Text +
                            "',ControlDefinition='" + txt_ctrldefinition.Text.Replace("'", "''") +
                            "',[Version]='" + txt_repositiory.Text.Replace("'", "''") +
                            "',[ModifiedBy]='" + SignIn.userId +
                            "',[ModifiedDate]='" + DateTime.Now.ToString() + 
                            "' WHERE MASTERORID=" + txt_masterorid.Text + " AND ProjectID=" + SignIn.projectId;
                        objLib.RunQuery(strCommand);
                        lbl_errmsg.Text = "";
                        MessageBox.Show("Control Updated in Object Repository Successfully", "Edit OR Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR : " + ex.Message, "Edit OR Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    lbl_errmsg.Text = "Please provide Inputs for Mandatory fields :\n1. PageName 2. Label 3. TagName 4. Repository 5. ControlID (Or) LabelFor (Or) Xpath (Or) InnerText.";
                }                
            }
            else
            {
                lbl_errmsg.Text = "Duplicate Control Exists.";
            }                      
        }
        private void cmb_pagenamechanged_Select(object sender, System.EventArgs e)
        {
            txt_pageid.Text = objLib.GetPageTitles().FirstOrDefault(x => x.Value == cmb_pagename.SelectedItem.ToString()).Key;
        }
    }
}
