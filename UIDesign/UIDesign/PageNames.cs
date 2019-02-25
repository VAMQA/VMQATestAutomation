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
    public partial class PageNames : Form
    {
        private Label lbl_pagename = new Label();
        public TextBox txt_pagename = new TextBox();

        private Label lbl_tags = new Label();
        public TextBox txt_tags = new TextBox();

        private Label lbl_lastupdatedby = new Label();
        public Label lbl_lastupdatedbydateandtime = new Label();

        public Button btn_createpage = new Button();

        private PictureBox pic_gecko = new PictureBox();
        private FlowLayoutPanel flp_pagenames = new FlowLayoutPanel();
        private TableLayoutPanel tlp_pagenames = new TableLayoutPanel();

        FuncLib objLib = new FuncLib();

        public PageNames()
        {
            InitializeComponent();

            #region pagenames_design
            
            //Form Settings
            this.AutoScroll = true;
            this.Size = new Size(400, 330);
            this.BackColor = SystemColors.Window;
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Page Names";
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            //Flow Layout Panel Settings            
            flp_pagenames.AutoSize = true;
            flp_pagenames.Dock = DockStyle.Top;
            flp_pagenames.SetFlowBreak(pic_gecko, true);
            flp_pagenames.FlowDirection = FlowDirection.LeftToRight;

            //Table Layout Panel Settings                                    
            tlp_pagenames.AutoSize = true;
            tlp_pagenames.Location = new Point(30, 80);
            tlp_pagenames.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_pagenames.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;

            //Gecko Picture Box Settings                        
            pic_gecko.Width = 220;
            pic_gecko.Height = 150;
            pic_gecko.Image = Properties.Resources.VM;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;

            //Page Name Label settings            
            lbl_pagename.Height = 24;
            lbl_pagename.Width = 100;
            lbl_pagename.Text = "Page Name:*";
            lbl_pagename.Name = "lblPageName";
            lbl_pagename.TextAlign = ContentAlignment.BottomLeft;
            lbl_pagename.Font = new Font("Calibri", 12F, FontStyle.Bold);

            //Page Name textbox settings            
            txt_pagename.Height = 24;
            txt_pagename.Width = 250;
            txt_pagename.Text = "";
            txt_pagename.Name = "txtPageName";
            txt_pagename.BorderStyle = BorderStyle.FixedSingle;
            txt_pagename.Font = new Font("Calibri", 12F, FontStyle.Regular);


            //Page Name Label settings            
            lbl_tags.Height = 24;
            lbl_tags.Width = 100;
            lbl_tags.Text = "Tags:";            
            lbl_tags.TextAlign = ContentAlignment.BottomLeft;
            lbl_tags.Font = new Font("Calibri", 12F, FontStyle.Bold);
            
            //Page Name Label settings            
            lbl_lastupdatedby.Height = 24;
            lbl_lastupdatedby.Width = 140;
            lbl_lastupdatedby.Text = "Last Updated By:";
            lbl_lastupdatedby.TextAlign = ContentAlignment.TopLeft;
            lbl_lastupdatedby.Font = new Font("Calibri", 11F, FontStyle.Bold);

            //Page Name textbox settings            
            txt_tags.Height = 50;
            txt_tags.Width = 250;
            txt_tags.Multiline = true;
            txt_tags.Text = "";            
            txt_tags.BorderStyle = BorderStyle.FixedSingle;
            txt_tags.Font = new Font("Calibri", 12F, FontStyle.Regular);

            lbl_lastupdatedbydateandtime.Height = 50;
            lbl_lastupdatedbydateandtime.Width = 250;                        
            //lbl_lastupdatedbydateandtime.BorderStyle = BorderStyle.FixedSingle;
            lbl_lastupdatedbydateandtime.Font = new Font("Calibri", 10F, FontStyle.Regular);

            //Create button settings            
            btn_createpage.Height = 30;
            btn_createpage.Width = 80;
            btn_createpage.Text = "Create";
            btn_createpage.Name = "btnCreatePage";
            btn_createpage.Font = new Font("Calibri", 12F, FontStyle.Bold);

            //Adding Controls to Table Layout Panel            
            tlp_pagenames.Controls.Add(lbl_pagename, 0, 2);
            tlp_pagenames.Controls.Add(txt_pagename, 1, 2);
            tlp_pagenames.Controls.Add(lbl_tags, 0, 3);
            tlp_pagenames.Controls.Add(txt_tags, 1, 3);
            //tlp_pagenames.Controls.Add(lbl_lastupdatedby, 0, 4);
            //tlp_pagenames.Controls.Add(lbl_lastupdatedbydateandtime, 1, 4);
            tlp_pagenames.Controls.Add(btn_createpage, 0, 5);

            //Adding Controls to Flow Layout Panel
            flp_pagenames.Controls.AddRange(new Control[] { pic_gecko, tlp_pagenames });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_pagenames });
            #endregion

            #region pagenames_methods                        
            btn_createpage.Click += new System.EventHandler(btn_createpage_Click);
            #endregion
        }
        private void PageNames_Load(object sender, EventArgs e)
        {

        }
        private void btn_createpage_Click(object sender, System.EventArgs e)
        {
            if(btn_createpage.Text!="Save")
            {
                try
                {
                    if (!objLib.GetPageTitles().Values.Any(x => x.Trim().ToLower() == txt_pagename.Text.Trim().ToLower()))
                    {
                        if (Int32.Parse(objLib.ExecuteScalar("SELECT COUNT(*) FROM PAGENAMES WHERE PAGENAME IS NULL AND PROJECTID=" + SignIn.projectId).ToString()) == 0)
                            objLib.RunQuery("INSERT INTO PAGENAMES (PageName,ProjectID,CreatedBy,CreatedDate) VALUES(NULL," + SignIn.projectId + ",'" + SignIn.userId + "',GETDATE())");

                        string strQuery = "INSERT INTO PAGENAMES(PAGENAME,TAGS,CreatedBy,CreatedDate,UpdatedBy,LastUpdateDate,PROJECTID) VALUES('" + txt_pagename.Text.Trim() + "','" + txt_tags.Text.Trim() + "','" + SignIn.userId + "',GETDATE(),'" + SignIn.userId + "',GETDATE()," + SignIn.projectId + ")";
                        objLib.RunQuery(strQuery);
                        MessageBox.Show("PAGE Created Successfully", "Create Page", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Duplicate Page Name (OR) Page Name Can't be Empty.", "Create Page", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR : " + ex.Message, "Create Page", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                string strQuery = "UPDATE PAGENAMES SET TAGS='" + txt_tags.Text.Trim() + "',UpdatedBy='" + SignIn.userId + "',LastUpdateDate=GETDATE() WHERE PAGENAME='" + txt_pagename.Text.Trim() + "' AND PROJECTID=" + SignIn.projectId;
                objLib.RunQuery(strQuery);
                MessageBox.Show("PAGE Tags Updated Successfully", "Edit Page", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }           
        }
    }
}
