using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UIDesign
{
    public partial class SignIn : Form
    {
        public static string userId;
        public static string password;
        public static string projectName;
        public static int projectId;

        private Label lbl_userid = new Label();
        private Label lbl_password = new Label();
        private Label lbl_projname = new Label();
        private Label lbl_errmsg = new Label();
        

        private TextBox txt_userid = new TextBox();
        private TextBox txt_password = new TextBox();
        private ComboBox cmb_projname = new ComboBox();
        private CheckBox chk_winauthentication = new CheckBox();
        private Button btn_signin = new Button();
        private LinkLabel lnk_register = new LinkLabel();
        

        private PictureBox pic_gecko = new PictureBox();
        private FlowLayoutPanel flp_logon = new FlowLayoutPanel();
        private TableLayoutPanel tlp_logon = new TableLayoutPanel();

        FuncLib objLib = new FuncLib();

        public SignIn()
        {
            InitializeComponent();

            #region signin_design

            //Form Settings
            this.Size = new Size(550, 350);
            this.WindowState = FormWindowState.Normal;
            this.BackColor = SystemColors.Window;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Sign In - v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            //FlowLayoutPanel - flp_homepage Settings
            flp_logon.FlowDirection = FlowDirection.LeftToRight;            
            flp_logon.Dock = DockStyle.Top;
            flp_logon.AutoSize = true;

            //TableLayoutPanel - tlp_homepage Settings
            tlp_logon.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_logon.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_logon.AutoSize = true;

            //Gecko Picture Box Settings
            pic_gecko.Height = 200;
            pic_gecko.Width = 200;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            //Userid Label settings           
            lbl_userid.Height = 24;
            lbl_userid.Width = 100;
            lbl_userid.Text = "User ID :*";
            lbl_userid.Name = "lbl_userid";
            lbl_userid.TextAlign = ContentAlignment.BottomRight;
            lbl_userid.Font = new Font("Calibri", 10F, FontStyle.Regular);

            //User ID textbox settings           
            txt_userid.Text = "";
            txt_userid.Name = "txt_userid";
            txt_userid.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_userid.Height = 25;
            txt_userid.Width = 200;

            //Password Label settings           
            lbl_password.Height = 24;
            lbl_password.Width = 100;
            lbl_password.Text = "Password :*";            
            lbl_password.TextAlign = ContentAlignment.BottomRight;
            lbl_password.Font = new Font("Calibri", 10F, FontStyle.Regular);

            //Password textbox settings                       
            txt_password.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_password.Height = 25;
            txt_password.Width = 200;
            txt_password.PasswordChar = '*';
            txt_password.Enabled = false;

            chk_winauthentication.Text = "Windows Authentication";
            chk_winauthentication.Font = new Font("Calibri", 9F, FontStyle.Regular);
            chk_winauthentication.Height = 25;
            chk_winauthentication.Width = 250;
            chk_winauthentication.Checked = true;

            //Project Label settings           
            lbl_projname.Height = 24;
            lbl_projname.Width = 100;
            lbl_projname.Text = "Project :*";
            lbl_projname.Name = "lbl_projname";
            lbl_projname.TextAlign = ContentAlignment.BottomRight;
            lbl_projname.Font = new Font("Calibri", 10F, FontStyle.Regular);

            cmb_projname.Name = "cmb_projname";
            cmb_projname.Font = new Font("Calibri", 11F, FontStyle.Regular);
            cmb_projname.Height = 28;
            cmb_projname.Width = 200;
            cmb_projname.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb_projname.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmb_projname.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cmb_projname.Items.AddRange(objLib.GetProjInfo().OrderBy(x => x.Value).Select(x => x.Value).ToArray());

            //LogOn button settings
            btn_signin.Text = "Submit";
            btn_signin.Font = new Font("Calibri", 10F, FontStyle.Regular);
            btn_signin.Height = 25;
            btn_signin.Width = 90;

            //Error Message Label settings           
            lbl_errmsg.Height = 24;
            lbl_errmsg.Width = 300;
            lbl_errmsg.Name = "lbl_errmsg";
            lbl_errmsg.TextAlign = ContentAlignment.BottomCenter;
            lbl_errmsg.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_errmsg.ForeColor = Color.Red;            
            lbl_errmsg.Visible = false;

            //Register Link Label settings           
            lnk_register.Height = 24;
            lnk_register.Width = 100;
            lnk_register.Text = "New User?";
            lnk_register.Name = "lnk_register";
            lnk_register.TextAlign = ContentAlignment.BottomRight;
            lnk_register.Font = new Font("Calibri", 10F, FontStyle.Regular);

            //Adding Controls to Table Layout Panel
            tlp_logon.Controls.Add(pic_gecko, 0, 1);
            tlp_logon.SetRowSpan(pic_gecko, 6);
            tlp_logon.Controls.Add(lbl_userid, 1, 2);
            tlp_logon.Controls.Add(txt_userid, 2, 2);
            tlp_logon.SetColumnSpan(txt_userid, 2);

            tlp_logon.Controls.Add(lbl_password, 1, 3);
            tlp_logon.Controls.Add(txt_password, 2, 3);
            tlp_logon.SetColumnSpan(txt_password, 2);

            tlp_logon.Controls.Add(lbl_projname, 1, 4);
            tlp_logon.Controls.Add(cmb_projname, 2, 4);
            tlp_logon.SetColumnSpan(cmb_projname, 2);

            tlp_logon.Controls.Add(chk_winauthentication, 2, 5);
            tlp_logon.SetColumnSpan(chk_winauthentication, 2);

            tlp_logon.Controls.Add(btn_signin, 3, 6);
            tlp_logon.Controls.Add(lbl_errmsg, 1, 7);
            tlp_logon.SetColumnSpan(lbl_errmsg, 3);
            tlp_logon.Controls.Add(lnk_register, 3, 8);

            //Adding Controls to Flow Layout Panel           
            flp_logon.Controls.AddRange(new Control[] { tlp_logon });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_logon });

            #endregion

            #region signin_methods
            lnk_register.Click += new System.EventHandler(lnk_register_Click);
            btn_signin.Click += new System.EventHandler(btn_signin_Click);
            chk_winauthentication.CheckedChanged += new System.EventHandler(chk_winauthentication_Changed);
            #endregion
        }
        private void SignIn_Load(object sender, EventArgs e)
        {
            
        }
        private void chk_winauthentication_Changed(object sender, EventArgs e)
        {
            txt_password.Text = ""; ;
            if (chk_winauthentication.Checked)
                txt_password.Enabled = false;
            else
                txt_password.Enabled = true;
        }
        private void lnk_register_Click(object sender, EventArgs e)
        {            
            Register objRegister = new Register();
            objRegister.ShowDialog();            
        }
        private void btn_signin_Click(object sender, EventArgs e)
        {
            try
            {
                userId = txt_userid.Text.Trim().ToUpper();
                password = txt_password.Text;

                var encode = System.Text.Encoding.UTF8.GetBytes(password);
                var encodedpassword=System.Convert.ToBase64String(encode);

                //var decode = System.Convert.FromBase64String(encodedpassword);
                //var decodedpassword = System.Text.Encoding.UTF8.GetString(decode);
                projectName = Convert.ToString(cmb_projname.SelectedItem);

                if(chk_winauthentication.Checked)
                {
                    if (!objLib.IsNullOrEmpty(userId) && !objLib.IsNullOrEmpty(projectName))
                    {
                        if (objLib.IsEqual(System.Environment.UserName, userId))
                        {
                            if (objLib.GetProjMaps(projectName).ContainsKey(userId.ToUpper()))
                            {
                                lbl_errmsg.Visible = false;
                                projectId = Convert.ToInt32(objLib.GetProjInfo().Single(x => x.Value == projectName).Key);
                                HomePage objHomePage = new HomePage();
                                objHomePage.ShowDialog();
                            }
                            else
                            {
                                lbl_errmsg.Visible = true;
                                lbl_errmsg.Text = "Access Denied to Project : " + projectName;
                            }
                        }
                        else
                        {
                            lbl_errmsg.Visible = true;
                            lbl_errmsg.Text = "Access Denied to User : " + userId;
                        }
                    }
                    else
                    {
                        lbl_errmsg.Visible = true;
                        lbl_errmsg.Text = "User ID , Project... Can't be Empty.";
                    }
                }
                else
                {
                    if (!objLib.IsNullOrEmpty(userId) && !objLib.IsNullOrEmpty(password) && !objLib.IsNullOrEmpty(projectName))
                    {
                        projectId = Convert.ToInt32(objLib.GetProjInfo().Single(x => x.Value == projectName).Key);
                        if (Convert.ToInt32(objLib.ExecuteScalar("SELECT COUNT(*) FROM USERS WHERE USERID='"+userId+"' AND Password='"+encodedpassword+"' AND PROJECTID="+projectId))!=0)
                        {
                            if (objLib.GetProjMaps(projectName).ContainsKey(userId.ToUpper()))
                            {
                                lbl_errmsg.Visible = false;
                                //projectId = Convert.ToInt32(objLib.GetProjInfo().Single(x => x.Value == projectName).Key);
                                HomePage objHomePage = new HomePage();
                                objHomePage.ShowDialog();
                            }
                            else
                            {
                                lbl_errmsg.Visible = true;
                                lbl_errmsg.Text = "Access Denied to Project : " + projectName;
                            }
                        }
                        else
                        {
                            lbl_errmsg.Visible = true;
                            lbl_errmsg.Text = "Access Denied to User : " + userId;
                        }
                    }
                    else
                    {
                        lbl_errmsg.Visible = true;
                        lbl_errmsg.Text = "User ID , Password , Project... Can't be Empty.";
                    }
                }                
            }
            catch(Exception ex)
            {
                MessageBox.Show("ERROR : " + ex.Message);
            }            
        }
    }
}
