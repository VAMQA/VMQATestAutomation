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
    public partial class MassUpdate : Form
    {
        private Label lbl_field = new Label();
        private Label lbl_equal = new Label();
        private Label lbl_value = new Label();
        private Label lbl_where = new Label();
        private Label lbl_operator = new Label();
        private Label lbl_errmsg = new Label();
        private Label lbl_pagetitle = new Label();

        private TextBox txt_colvalue = new TextBox();
        public TextBox txt_condval_2 = new TextBox();
        public TextBox txt_condval_1 = new TextBox();
        public TextBox txt_condval_0 = new TextBox();

        public ComboBox cmb_andor_1 = new ComboBox();
        private CheckBox chk_condbox_0 = new CheckBox();
        private CheckBox chk_condbox_1 = new CheckBox();
        private CheckBox chk_condbox_2 = new CheckBox();

        public ComboBox cmb_colnames = new ComboBox();
        public ComboBox cmb_condcol_0 = new ComboBox();
        public ComboBox cmb_condcol_1 = new ComboBox();
        public ComboBox cmb_operator_2 = new ComboBox();
        public ComboBox cmb_operator_1 = new ComboBox();
        public ComboBox cmb_operator_0 = new ComboBox();
        public static bool flag = true;

        public ComboBox cmb_andor_2 = new ComboBox();
        public ComboBox cmb_condcol_2 = new ComboBox();
        //public ComboBox cmb_condval_2 = new ComboBox();

        public Button btn_runquery = new Button();

        private PictureBox pic_gecko = new PictureBox();
        private FlowLayoutPanel flp_massupdate = new FlowLayoutPanel();
        private TableLayoutPanel tlp_massupdate = new TableLayoutPanel();

        FuncLib objLib = new FuncLib();

        public MassUpdate()
        {
            InitializeComponent();

            #region massupdate_design

            //Form Settings
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            //this.Size = new Size(800, 470);
            this.Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width - 500, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height - 300);
            this.AutoScroll = true;
            this.Text = "Mass Update";
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            //Flow Layout Panel Settings            
            flp_massupdate.FlowDirection = FlowDirection.LeftToRight;
            flp_massupdate.SetFlowBreak(pic_gecko, true);
            flp_massupdate.Dock = DockStyle.Top;
            flp_massupdate.AutoSize = true;

            //Table Layout Panel Settings                                    
            tlp_massupdate.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_massupdate.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_massupdate.AutoSize = true;
            tlp_massupdate.Location = new Point(30, 80);

            //Gecko Picture Box Settings            
            pic_gecko.Height = 170;
            pic_gecko.Width = 220;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            //SET Label settings            
            lbl_field.Text = "Field ";
            lbl_field.Name = "lbl_field";
            lbl_field.TextAlign = ContentAlignment.BottomLeft;
            lbl_field.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_field.Height = 15;
            lbl_field.Width = 110;

            //SET Label settings            
            lbl_operator.Text = "Operator ";
            lbl_operator.Name = "lbl_operator";
            lbl_operator.TextAlign = ContentAlignment.BottomCenter;
            lbl_operator.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_operator.Height = 15;
            lbl_operator.Width = 110;

            //SET Value Label settings            
            lbl_value.Text = "Value ";
            lbl_value.Name = "lbl_value";
            lbl_value.TextAlign = ContentAlignment.BottomLeft;
            lbl_value.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_value.Height = 15;
            lbl_value.Width = 110;

            //ColumnNames combobox settings            
            cmb_colnames.Name = "cmb_colnames";
            cmb_colnames.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_colnames.Height = 24;
            cmb_colnames.Width = 150;
            cmb_colnames.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_colnames.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_colnames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_colnames.Items.AddRange(new[] { "ActionORData", "Execute", "Keyword", "Label", "PageName" });

            //Equal Label settings            
            lbl_equal.Text = "=";
            lbl_equal.Name = "lbl_equal";
            lbl_equal.TextAlign = ContentAlignment.BottomRight;
            lbl_equal.Font = new Font("Calibri", 10F, FontStyle.Bold);
            lbl_equal.Height = 15;
            lbl_equal.Width = 50;


            //Value textbox settings                        
            txt_colvalue.Name = "txt_colvalue";
            txt_colvalue.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_colvalue.Height = 24;
            txt_colvalue.Width = 350;

            //WHERE Label settings            
            lbl_where.Text = "Where";
            lbl_where.Name = "lbl_where";
            lbl_where.TextAlign = ContentAlignment.MiddleRight;
            lbl_where.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_where.Height = 15;
            lbl_where.Width = 110;

            //Error Message Label settings           
            lbl_errmsg.Height = 24;
            lbl_errmsg.Width = 1000;
            lbl_errmsg.Name = "lbl_errmsg";
            lbl_errmsg.TextAlign = ContentAlignment.BottomLeft;
            lbl_errmsg.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_errmsg.ForeColor = Color.Red;
            lbl_errmsg.Visible = false;

            //ColumnNames combobox settings            
            cmb_condcol_0.Name = "cmb_condcol_0";
            cmb_condcol_0.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_condcol_0.Height = 24;
            cmb_condcol_0.Width = 150;
            cmb_condcol_0.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_condcol_0.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_condcol_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            //ColumnValues combobox settings            
            txt_condval_0.Name = "txt_condval_0";
            txt_condval_0.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_condval_0.Height = 24;
            txt_condval_0.Width = 350;
            //cmb_condval_0.AutoCompleteSource = AutoCompleteSource.ListItems;
            //cmb_condval_0.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_condval_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            //ColumnNames combobox settings            
            cmb_operator_0.Name = "cmb_operator_0";
            cmb_operator_0.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_operator_0.Height = 24;
            cmb_operator_0.Width = 100;
            cmb_operator_0.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_operator_0.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_operator_0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_operator_0.Items.AddRange(new string[] { "=", "!=", "<", ">", "<=", ">=", "In" });


            //CheckBox textbox settings                        
            chk_condbox_1.Name = "chk_condbox_1";
            chk_condbox_1.Height = 24;
            chk_condbox_1.Width = 20;

            //ColumnNames combobox settings            
            cmb_andor_1.Name = "cmb_andor_1";
            cmb_andor_1.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_andor_1.Height = 24;
            cmb_andor_1.Width = 110;
            cmb_andor_1.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_andor_1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_andor_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_andor_1.Items.AddRange(new string[] { "And", "Or" });

            //ColumnNames combobox settings            
            cmb_condcol_1.Name = "cmb_condcol_1";
            cmb_condcol_1.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_condcol_1.Height = 24;
            cmb_condcol_1.Width = 150;
            cmb_condcol_1.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_condcol_1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_condcol_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            //ColumnValues combobox settings            
            txt_condval_1.Name = "txt_condval_1";
            txt_condval_1.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_condval_1.Height = 24;
            txt_condval_1.Width = 350;
            //cmb_condval_1.AutoCompleteSource = AutoCompleteSource.ListItems;
            //cmb_condval_1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_condval_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            //ColumnNames combobox settings            
            cmb_operator_1.Name = "cmb_operator_1";
            cmb_operator_1.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_operator_1.Height = 24;
            cmb_operator_1.Width = 100;
            cmb_operator_1.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_operator_1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_operator_1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_operator_1.Items.AddRange(new string[] { "=", "!=", "<", ">", "<=", ">=", "In" });


            //CheckBox textbox settings                        
            chk_condbox_0.Name = "chk_condbox_0";
            chk_condbox_0.Height = 24;
            chk_condbox_0.Width = 20;


            //CheckBox textbox settings                        
            chk_condbox_2.Name = "chk_condbox_2";
            chk_condbox_2.Height = 24;
            chk_condbox_2.Width = 20;

            //ColumnNames combobox settings            
            cmb_andor_2.Name = "cmb_andor_2";
            cmb_andor_2.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_andor_2.Height = 24;
            cmb_andor_2.Width = 110;
            cmb_andor_2.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_andor_2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_andor_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_andor_2.Items.AddRange(new string[] { "And", "Or" });

            //ColumnNames combobox settings            
            cmb_condcol_2.Name = "cmb_condcol_2";
            cmb_condcol_2.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_condcol_2.Height = 24;
            cmb_condcol_2.Width = 150;
            cmb_condcol_2.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_condcol_2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_condcol_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            //ColumnValues combobox settings            
            txt_condval_2.Name = "txt_condval_2";
            txt_condval_2.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_condval_2.Height = 24;
            txt_condval_2.Width = 350;
            //cmb_condval_2.AutoCompleteSource = AutoCompleteSource.ListItems;
            //cmb_condval_2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmb_condval_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;

            //ColumnNames combobox settings            
            cmb_operator_2.Name = "cmb_operator_2";
            cmb_operator_2.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_operator_2.Height = 24;
            cmb_operator_2.Width = 100;
            cmb_operator_2.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_operator_2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_operator_2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_operator_2.Items.AddRange(new string[] { "=", "!=", "<", ">", "<=", ">=", "In" });

            //Update button settings            
            btn_runquery.Text = "Run";
            btn_runquery.Name = "btn_runquery";
            btn_runquery.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_runquery.Height = 30;
            btn_runquery.Width = 100;


            //Userid Label settings           
            lbl_pagetitle.Height = 30;
            lbl_pagetitle.Width = 800;
            lbl_pagetitle.Text = "MASS UPDATE";
            lbl_pagetitle.Name = "lbl_pagetitle";
            lbl_pagetitle.TextAlign = ContentAlignment.MiddleCenter;
            lbl_pagetitle.Font = new Font("Calibri", 20F, FontStyle.Bold);
            lbl_pagetitle.ForeColor = Color.Blue;

            //Adding Controls to Table Layout Panel            
            tlp_massupdate.Controls.Add(lbl_pagetitle, 0, 1);
            tlp_massupdate.SetColumnSpan(lbl_pagetitle, 10);
            tlp_massupdate.Controls.Add(lbl_field, 2, 2);
            tlp_massupdate.Controls.Add(lbl_operator, 3, 2);
            tlp_massupdate.Controls.Add(lbl_value, 4, 2);
            tlp_massupdate.Controls.Add(cmb_colnames, 2, 3);
            tlp_massupdate.Controls.Add(lbl_equal, 3, 3);
            tlp_massupdate.Controls.Add(txt_colvalue, 4, 3);
            tlp_massupdate.Controls.Add(chk_condbox_0, 0, 5);
            tlp_massupdate.Controls.Add(lbl_where, 1, 5);
            tlp_massupdate.Controls.Add(cmb_condcol_0, 2, 5);
            tlp_massupdate.Controls.Add(cmb_operator_0, 3, 5);
            tlp_massupdate.Controls.Add(txt_condval_0, 4, 5);
            tlp_massupdate.Controls.Add(chk_condbox_1, 0, 6);
            tlp_massupdate.Controls.Add(cmb_andor_1, 1, 6);
            tlp_massupdate.Controls.Add(cmb_condcol_1, 2, 6);
            tlp_massupdate.Controls.Add(cmb_operator_1, 3, 6);
            tlp_massupdate.Controls.Add(txt_condval_1, 4, 6);
            tlp_massupdate.Controls.Add(chk_condbox_2, 0, 7);
            tlp_massupdate.Controls.Add(cmb_andor_2, 1, 7);
            tlp_massupdate.Controls.Add(cmb_condcol_2, 2, 7);
            tlp_massupdate.Controls.Add(cmb_operator_2, 3, 7);
            tlp_massupdate.Controls.Add(txt_condval_2, 4, 7);
            tlp_massupdate.Controls.Add(lbl_errmsg, 1, 8);
            tlp_massupdate.SetColumnSpan(lbl_errmsg, 4);
            tlp_massupdate.Controls.Add(btn_runquery, 1, 9);

            //Adding Controls to Flow Layout Panel
            flp_massupdate.Controls.AddRange(new Control[] { pic_gecko, tlp_massupdate });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_massupdate });
            //this.Load += new System.EventHandler(MassUpdate_Load);

            #endregion

            #region massupdate_methods

            btn_runquery.Click += new System.EventHandler(btn_runquery_Click);

            #endregion
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
                    if (objLib.IsEqual(columnName, "PageName"))
                    {
                        columnName = "PageID";
                        if (columnValue.Contains(','))
                        {
                            var pagenames = columnValue.Split(',');
                            foreach (var item in pagenames)
                            {
                                try
                                {
                                    var pagename = objLib.GetPageTitles().First(x => x.Value == item).Key;
                                    columnValue = columnValue.Replace(item, pagename);
                                }
                                catch
                                {
                                    throw new Exception("Invalid Page Name : " + item);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                columnValue = objLib.GetPageTitles().First(x => x.Value == columnValue).Key;
                            }
                            catch
                            {
                                throw new Exception("Invalid Page Name : " + columnValue);
                            }
                        }
                    }
                    else if (objLib.IsEqual(columnName, "Label"))
                    {
                        columnName = "MasterORID";
                        if (columnValue.Contains(','))
                        {
                            var labels = columnValue.Split(',');
                            foreach (var item in labels)
                            {
                                try
                                {
                                    var label = objLib.GetORLables().First(x => x.Value == item).Key;
                                    columnValue = columnValue.Replace(item, label);
                                }
                                catch
                                {
                                    throw new Exception("Invalid Label : " + item);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                columnValue = objLib.GetORLables().First(x => x.Value == columnValue).Key;
                            }

                            catch
                            {
                                throw new Exception("Invalid Label : " + columnValue);
                            }
                        }
                    }
                    else if (objLib.IsEqual(columnName, "Keyword"))
                    {
                        columnName = "Indicator";
                        if (columnValue.Contains(','))
                        {
                            var keywords = columnValue.Split(',');
                            foreach (var item in keywords)
                            {
                                try
                                {
                                    var keyword = objLib.GetKeywords().First(x => x.Value == item).Key;
                                    columnValue = columnValue.Replace(item, keyword);
                                }
                                catch
                                {
                                    throw new Exception("Invalid Keyword : " + item);
                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                columnValue = objLib.GetKeywords().First(x => x.Value == columnValue).Key;
                            }
                            catch
                            {
                                throw new Exception("Invalid Keyword : " + columnValue);
                            }
                        }
                    }
                    else if (objLib.IsEqual(columnName, "TestCaseId"))
                    {
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
                                columnValue = objLib.GetTestDataIDs().First(x => x.Value == columnValue).Key;
                            }
                            catch
                            {
                                throw new Exception("Invalid TestCaseID : " + columnValue);
                            }
                        }
                    }
                    if (objLib.IsEqual(operatorValue, "In"))
                        replacedclause = "[" + columnName + "] " + operatorValue + "(" + columnValue + ")";
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
        private void MassUpdate_Load(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = objLib.binddataTable("SELECT * FROM TESTDATA WHERE PROJECTID=" + SignIn.projectId);
            dt.Columns.Remove("ActionFlow_ID");
            dt.Columns.Remove("ProjectID");
            var tdColNames = dt.Columns.Cast<DataColumn>().OrderBy(x => x.ColumnName).Select(x => x.ColumnName).ToArray();
            cmb_condcol_0.Items.AddRange(tdColNames);
            cmb_condcol_1.Items.AddRange(tdColNames);
            cmb_condcol_2.Items.AddRange(tdColNames);
        }
        private void btn_runquery_Click(object sender, EventArgs e)
        {
            try
            {
                lbl_errmsg.Visible = false;
                string executeQuery = buildQuery();
                if ((flag) && (!objLib.IsNullOrEmpty(executeQuery)))
                {
                    DialogResult confirmrunQuery = MessageBox.Show("Want to Execute this Query ? ", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirmrunQuery == DialogResult.Yes)
                    {
                        try
                        {
                            int impactedRows = objLib.ExecuteQuery(executeQuery);
                            MessageBox.Show("Query Executed Successfully.\n\n No.Of TestData Rows Impacted : " + impactedRows, "Mass Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception exce)
                        {
                            throw exce;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("ERROR : " + exc.Message);
            }
        }
        private string buildQuery()
        {
            try
            {
                string updateQuery = string.Empty;
                flag = true;
                string setClause = buildClause(Convert.ToString(cmb_colnames.SelectedItem), lbl_equal.Text, txt_colvalue.Text);
                if (!objLib.IsNullOrEmpty(setClause))
                {
                    updateQuery = "UPDATE TESTDATA SET " + setClause +" WHERE PROJECTID='"+SignIn.projectId+"'";
                    if (chk_condbox_0.Checked)
                    {
                        string condClause_1 = buildClause(Convert.ToString(cmb_condcol_0.SelectedItem), Convert.ToString(cmb_operator_0.SelectedItem), txt_condval_0.Text);
                        if (!objLib.IsNullOrEmpty(condClause_1))
                        {
                            updateQuery = updateQuery + " AND " + condClause_1;
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
                }
                else
                {
                    flag = false;
                    lbl_errmsg.Visible = true;
                    lbl_errmsg.Text = "Invalid Inputs..at Set Clause";
                }
                return updateQuery;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
