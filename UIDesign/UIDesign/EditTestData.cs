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
    public partial class EditTestData : Form
    {
        private Label lbl_execute = new Label();
        private Label lbl_pagename = new Label();        
        private Label lbl_flowidentifier = new Label();
        private Label lbl_dataidentifier = new Label();
        private Label lbl_indicator = new Label();
        private Label lbl_label = new Label();        
        private Label lbl_actionordata = new Label();
        private Label lbl_seqnumber = new Label();        
        private Label lbl_errmsg = new Label();
        private Label lbl_pagetitle = new Label();

        public ComboBox cmb_execute = new ComboBox();
        public ComboBox cmb_pagename = new ComboBox();
        public TextBox txt_flowid = new TextBox();
        public TextBox txt_dataid = new TextBox();
        public ComboBox cmb_indicator = new ComboBox();
        public ComboBox cmb_label = new ComboBox();
        public TextBox txt_actionordata = new TextBox();
        public TextBox txt_seqnumber = new TextBox();
        public TextBox txt_testcaseid = new TextBox();
        public TextBox txt_actionflowid = new TextBox();

        public Button btn_savetestdata = new Button();
        private PictureBox pic_gecko = new PictureBox();
        private FlowLayoutPanel flp_editdata = new FlowLayoutPanel();
        private TableLayoutPanel tlp_editdata = new TableLayoutPanel();

        FuncLib objLib = new FuncLib();

        public EditTestData()
        {
            InitializeComponent();

            #region editdata_design

            //Form Settings
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            //this.Size = new Size(1010, 630);
            this.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 2 + 50, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 600);
            this.AutoScroll = true;
            this.Text = "Edit Test Data";
            this.MaximizeBox = false;
            this.MinimizeBox = false;


            //Flow Layout Panel Settings            
            flp_editdata.FlowDirection = FlowDirection.LeftToRight;
            flp_editdata.SetFlowBreak(pic_gecko, true);
            flp_editdata.Dock = DockStyle.Top;
            flp_editdata.AutoSize = true;

            //Table Layout Panel Settings                                    
            tlp_editdata.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_editdata.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_editdata.AutoSize = true;
            tlp_editdata.Location = new Point(30, 80);

            //Gecko Picture Box Settings            
            pic_gecko.Height = 200;
            pic_gecko.Width = 300;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            //Execute Label settings            
            lbl_execute.Text = "Execute";
            lbl_execute.Name = "lbl_execute";
            lbl_execute.TextAlign = ContentAlignment.MiddleLeft;
            lbl_execute.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_execute.Height = 24;
            lbl_execute.Width = 110;

            //Page Name Label settings            
            lbl_pagename.Text = "Page Name";
            lbl_pagename.Name = "lbl_pagename";
            lbl_pagename.TextAlign = ContentAlignment.MiddleLeft;
            lbl_pagename.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_pagename.Height = 24;
            lbl_pagename.Width = 110;

            //FlowIdentifier Label settings            
            lbl_flowidentifier.Text = "FlowIdentifier";
            lbl_flowidentifier.Name = "lbl_flowidentifier";
            lbl_flowidentifier.TextAlign = ContentAlignment.MiddleLeft;
            lbl_flowidentifier.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_flowidentifier.Height = 24;
            lbl_flowidentifier.Width = 110;

            //DataIdentifier Label settings            
            lbl_dataidentifier.Text = "DataIdentifier";
            lbl_dataidentifier.Name = "lbl_dataidentifier";
            lbl_dataidentifier.TextAlign = ContentAlignment.MiddleLeft;
            lbl_dataidentifier.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_dataidentifier.Height = 24;
            lbl_dataidentifier.Width = 110;

            //Indicator Label settings            
            lbl_indicator.Text = "Indicator";
            lbl_indicator.Name = "lbl_indicator";
            lbl_indicator.TextAlign = ContentAlignment.MiddleLeft;
            lbl_indicator.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_indicator.Height = 24;
            lbl_indicator.Width = 110;

            //Label settings            
            lbl_label.Text = "Label";
            lbl_label.Name = "lbl_label";
            lbl_label.TextAlign = ContentAlignment.MiddleLeft;
            lbl_label.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_label.Height = 24;
            lbl_label.Width = 110;

            //actionordata Label settings            
            lbl_actionordata.Text = "ActionORData";
            lbl_actionordata.Name = "lbl_actionordata";
            lbl_actionordata.TextAlign = ContentAlignment.MiddleLeft;
            lbl_actionordata.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_actionordata.Height = 24;
            lbl_actionordata.Width = 110;

            //SeqNumber settings            
            lbl_seqnumber.Text = "SeqNumber";
            lbl_seqnumber.Name = "lbl_seqnumber";
            lbl_seqnumber.TextAlign = ContentAlignment.MiddleLeft;
            lbl_seqnumber.Font = new Font("Calibri", 12F, FontStyle.Bold);
            lbl_seqnumber.Height = 24;
            lbl_seqnumber.Width = 110;

            //Execute combobox settings                                  
            //cmb_execute.Items.AddRange((from t in objLib.GetPageTitles() orderby t.Value select t.Value).ToArray());
            cmb_execute.Name = "cmb_execute";
            cmb_execute.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_execute.Height = 24;
            cmb_execute.Width = 300;
            cmb_execute.Items.Add("Yes");
            cmb_execute.Items.Add("No");
            cmb_execute.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_execute.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_execute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            //Page Name combobox settings                                  
            cmb_pagename.Items.AddRange((from t in objLib.GetPageTitles() orderby t.Value select t.Value).ToArray());
            cmb_pagename.Name = "cmb_pagename";
            cmb_pagename.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_pagename.Height = 24;
            cmb_pagename.Width = 300;
            cmb_pagename.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_pagename.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_pagename.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            //FlowId textbox settings            
            txt_flowid.Text = null;
            txt_flowid.Name = "txt_flowid";
            txt_flowid.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_flowid.Height = 24;
            txt_flowid.Width = 300;

            //DataID textbox settings            
            txt_dataid.Text = null;
            txt_dataid.Name = "txt_dataid";
            txt_dataid.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_dataid.Height = 24;
            txt_dataid.Width = 300;


            //Indicator combobox settings                                  
            cmb_indicator.Items.AddRange((from t in objLib.GetKeywords() orderby t.Value select t.Value).ToArray());
            cmb_indicator.Name = "cmb_indicator";
            cmb_indicator.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_indicator.Height = 24;
            cmb_indicator.Width = 300;
            cmb_indicator.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_indicator.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_indicator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            //Label combobox settings                                  
            cmb_label.Items.AddRange((from t in objLib.GetORLables() orderby t.Value select t.Value).ToArray());
            cmb_label.Name = "cmb_indicator";
            cmb_label.Font = new Font("Calibri", 12F, FontStyle.Regular);
            cmb_label.Height = 24;
            cmb_label.Width = 300;
            cmb_label.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_label.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_label.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            //ActionOrData textbox settings            
            txt_actionordata.Text = null;
            txt_actionordata.Name = "txt_actionordata";
            txt_actionordata.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_actionordata.Height = 24;
            txt_actionordata.Width = 300;

            //SeqNumber textbox settings            
            txt_seqnumber.Text = null;
            txt_seqnumber.Name = "txt_seqnumber";
            txt_seqnumber.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_seqnumber.Height = 24;
            txt_seqnumber.Width = 300;

            //Save button settings            
            btn_savetestdata.Text = "Save";
            btn_savetestdata.Name = "btn_savetestdata";
            btn_savetestdata.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_savetestdata.Height = 30;
            btn_savetestdata.Width = 100;

            tlp_editdata.Controls.Add(lbl_execute, 0, 1);
            tlp_editdata.Controls.Add(cmb_execute, 1, 1);
            tlp_editdata.Controls.Add(lbl_pagename, 3, 1);
            tlp_editdata.Controls.Add(cmb_pagename, 4, 1);
            tlp_editdata.Controls.Add(lbl_label, 0, 2);
            tlp_editdata.Controls.Add(cmb_label, 1, 2);
            tlp_editdata.Controls.Add(lbl_indicator, 3, 2);
            tlp_editdata.Controls.Add(cmb_indicator, 4, 2);
            tlp_editdata.Controls.Add(lbl_flowidentifier, 0, 3);
            tlp_editdata.Controls.Add(txt_flowid, 1,3);
            tlp_editdata.Controls.Add(lbl_dataidentifier, 3, 3);
            tlp_editdata.Controls.Add(txt_dataid, 4, 3);
            tlp_editdata.Controls.Add(lbl_actionordata, 0, 4);
            tlp_editdata.Controls.Add(txt_actionordata, 1, 4);
            tlp_editdata.Controls.Add(lbl_seqnumber, 3, 4);
            tlp_editdata.Controls.Add(txt_seqnumber, 4, 4);
            tlp_editdata.Controls.Add(lbl_errmsg, 0, 5);
            tlp_editdata.SetColumnSpan(lbl_errmsg, 5);
            tlp_editdata.Controls.Add(btn_savetestdata, 2, 7);

            //Adding Controls to Flow Layout Panel
            flp_editdata.Controls.AddRange(new Control[] { pic_gecko, tlp_editdata });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_editdata });
            
            

            #endregion

            cmb_pagename.SelectedIndexChanged += new System.EventHandler(cmb_pagenamechanged_Select);
            btn_savetestdata.Click += new System.EventHandler(btn_savetestdata_Click);
        }

        private void cmb_pagenamechanged_Select(object sender, System.EventArgs e)
        {            
            var pageId = objLib.GetPageTitles().First(x => x.Value == cmb_pagename.SelectedItem.ToString()).Key;
            cmb_label.Items.Clear();
            cmb_label.ResetText();
            cmb_label.Items.AddRange((from t in objLib.GetORLables(int.Parse(pageId)) orderby t.Value select t.Value).ToArray());            
        }
        private void btn_savetestdata_Click(object sender, System.EventArgs e)
        {
            string querytestdataInfo = string.Empty;
            if (!string.IsNullOrEmpty(txt_actionflowid.Text.ToString()))
            {
                int pageId = Int32.Parse(objLib.GetPageTitles().First(x => x.Value == cmb_pagename.SelectedItem.ToString()).Key);
                querytestdataInfo = "UPDATE TESTDATA SET PageID=" + pageId +
                                                      ",Indicator='" + objLib.GetKeywords().First(x => x.Value == cmb_indicator.SelectedItem.ToString()).Key +
                                                      "',MasterORID=" + objLib.GetORLables(pageId).FirstOrDefault(x => x.Value == cmb_label.SelectedItem.ToString()).Key +
                                                      ",ActionORData='" + txt_actionordata.Text.ToString().Replace("'", "''") +
                                                      "',[Execute]='" + cmb_execute.SelectedItem.ToString() +
                                                      "',FlowIdentifier=" + txt_flowid.Text.ToString() +
                                                      ",DataIdentifier=" + txt_dataid.Text.ToString() +
                                                      ",SeqNumber=" + txt_seqnumber.Text.ToString() +
                                                      ",TestCaseID=" + Int32.Parse(txt_testcaseid.Text.Trim()) +
                                                      " where ActionFlow_id=" + txt_actionflowid.Text.ToString() + " AND ProjectID=" + SignIn.projectId;
                objLib.RunQuery(querytestdataInfo);
                this.Close();  
            }
                         
        }
    }
}
