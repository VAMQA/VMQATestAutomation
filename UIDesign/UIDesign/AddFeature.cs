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
    public partial class AddFeature : Form
    {
        private Label lbl_pagetitle = new Label();
        private Label lbl_featurename = new Label();
        private Label lbl_exfeaturenames = new Label();
        public TextBox txt_featurename = new TextBox();
        public Button btn_add = new Button();


        private PictureBox pic_gecko = new PictureBox();
        private DataGridView grid_featurelist = new DataGridView();
        private BindingSource bindingSrc = new BindingSource();
        private FlowLayoutPanel flp_feature = new FlowLayoutPanel();
        private TableLayoutPanel tlp_feature = new TableLayoutPanel();

        FuncLib objLib = new FuncLib();

        public AddFeature()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            this.Size = new Size(400,330);
            this.AutoScroll = true;
            this.Text = "Add Feature";

            //Flow Layout Panel Settings            
            flp_feature.FlowDirection = FlowDirection.LeftToRight;
            flp_feature.SetFlowBreak(pic_gecko, true);
            flp_feature.Dock = DockStyle.Top;
            flp_feature.AutoSize = true;

            //Table Layout Panel Settings                                    
            tlp_feature.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_feature.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_feature.AutoSize = true;
            tlp_feature.Location = new Point(30, 80);

            //Gecko Picture Box Settings            
            pic_gecko.Height = 170;
            pic_gecko.Width = 220;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            //Data Gridview Settings
            grid_featurelist.ReadOnly = false;
            grid_featurelist.Dock = DockStyle.None;
            grid_featurelist.AutoGenerateColumns = true;
            grid_featurelist.Size = new Size(500, 550);
            grid_featurelist.DataSource = bindingSrc;
            grid_featurelist.DefaultCellStyle.Font = new Font("Calibri", 10);
            grid_featurelist.AlternatingRowsDefaultCellStyle.BackColor = Color.LightYellow;
            grid_featurelist.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8.9F, FontStyle.Bold);
            grid_featurelist.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;            
            grid_featurelist.AllowUserToAddRows = false;

            lbl_pagetitle.Height = 30;
            lbl_pagetitle.Width = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width / 4 - 150;
            lbl_pagetitle.Text = "ADD FEATURE";            
            lbl_pagetitle.TextAlign = ContentAlignment.MiddleCenter;
            lbl_pagetitle.Font = new Font("Calibri", 20F, FontStyle.Bold);
            lbl_pagetitle.ForeColor = Color.Blue;

            lbl_featurename.Text = "Feature Name :*";            
            lbl_featurename.TextAlign = ContentAlignment.BottomLeft;
            lbl_featurename.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_featurename.Height = 20;
            lbl_featurename.Width = 200;

            lbl_exfeaturenames.Text = "Existing Features :";
            lbl_exfeaturenames.TextAlign = ContentAlignment.BottomLeft;
            lbl_exfeaturenames.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_exfeaturenames.Height = 20;
            lbl_exfeaturenames.Width = 200;

            btn_add.Text = "ADD";
            btn_add.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_add.Height = 30;
            btn_add.Width = 100;            

            txt_featurename.Text = "";            
            txt_featurename.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_featurename.Height = 30;
            txt_featurename.Width = 250;
            txt_featurename.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_featurename.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            AutoCompleteStringCollection coll = new AutoCompleteStringCollection();
            coll.AddRange(objLib.GetFeatures().OrderBy(x => x.Value).Select(x => x.Value).Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
            txt_featurename.AutoCompleteCustomSource = coll;


            //tlp_feature.Controls.Add(lbl_pagetitle, 0, 1);
            //tlp_feature.SetColumnSpan(lbl_pagetitle, 10);

            tlp_feature.Controls.Add(lbl_featurename, 1, 2);
            tlp_feature.Controls.Add(txt_featurename, 1, 3);
            tlp_feature.Controls.Add(btn_add, 1, 4);
            //tlp_feature.Controls.Add(lbl_exfeaturenames, 1, 5);
            //tlp_feature.Controls.Add(grid_featurelist, 1, 8);
            

            //Adding Controls to Flow Layout Panel
            flp_feature.Controls.AddRange(new Control[] { pic_gecko, tlp_feature });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_feature });
            btn_add.Click += new System.EventHandler(btn_addfeature_Click);
        }

        private void AddFeature_Load(object sender, EventArgs e)
        {
            //var strQuery = "SELECT Name FROM Features WHERE PROJECTID=" + SignIn.projectId + " ORDER BY NAME";
            //DataTable dt = new DataTable();
            //dt = objLib.binddataTable(strQuery);
            ////dt.Columns.Add("New TestCaseID");
            //bindingSrc.DataSource = dt;
            //grid_featurelist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //grid_featurelist.Columns[0].Width = 50;
            //grid_featurelist.Visible = true;            
        }

        private void btn_addfeature_Click(object sender, System.EventArgs e)
        {
            if (btn_add.Text != "Save")
            {
                try
                {
                    if (!objLib.GetFeatures().Values.Any(x => x.Trim().ToLower() == txt_featurename.Text.Trim().ToLower()))
                    {
                        string strQuery = "INSERT INTO FEATURES(NAME,PROJECTID,CreatedBy,CreatedDate) VALUES('" + txt_featurename.Text.Trim() + "'," + SignIn.projectId + ", '"+ SignIn.userId +"', '" + DateTime.Now + "')";
                        objLib.RunQuery(strQuery);
                        MessageBox.Show("Feature Added Successfully", "Add Feature", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Duplicate Feature Name (OR) Feature Name Can't be Empty.", "Add Feature", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERROR : " + ex.Message, "Add Feature", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                //string strQuery = "UPDATE PAGENAMES SET TAGS='" + txt_tags.Text.Trim() + "' WHERE PAGENAME='" + txt_pagename.Text.Trim() + "' AND PROJECTID=" + SignIn.projectId;
                //objLib.RunQuery(strQuery);
                //MessageBox.Show("PAGE Tags Updated Successfully", "Edit Page", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.Close();
            }
        }
    }
}
