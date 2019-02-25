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
    public partial class Register : Form
    {
        string userId;
        string userName;
        string password;
        string projectName;
        

        private Label lbl_userid = new Label();
        private Label lbl_password = new Label();
        private Label lbl_username = new Label();        
        private Label lbl_projname = new Label();
        private Label lbl_errmsg = new Label();


        private TextBox txt_userid = new TextBox();
        private TextBox txt_password = new TextBox();
        private TextBox txt_username = new TextBox();        
        private ComboBox cmb_projname = new ComboBox();
        private Button btn_register = new Button();
        private LinkLabel lnk_regduser = new LinkLabel();


        private PictureBox pic_gecko = new PictureBox();
        private FlowLayoutPanel flp_register = new FlowLayoutPanel();
        private TableLayoutPanel tlp_register = new TableLayoutPanel();

        FuncLib objLib = new FuncLib();

        public Register()
        {
            InitializeComponent();

            #region register_design
            
            //Form Settings
            this.Size = new Size(550, 350);
            this.WindowState = FormWindowState.Normal;
            this.BackColor = SystemColors.Window;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "New User";
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            //FlowLayoutPanel - flp_homepage Settings
            flp_register.FlowDirection = FlowDirection.LeftToRight;            
            flp_register.Dock = DockStyle.Top;
            flp_register.AutoSize = true;

            //TableLayoutPanel - tlp_homepage Settings
            tlp_register.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_register.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_register.AutoSize = true;

            //Gecko Picture Box Settings
            pic_gecko.Height = 200;
            pic_gecko.Width = 200;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            //UserID Label settings           
            lbl_userid.Height = 24;
            lbl_userid.Width = 100;
            lbl_userid.Text = "User ID :*";
            lbl_userid.Name = "lbl_userid";
            lbl_userid.TextAlign = ContentAlignment.BottomRight;
            lbl_userid.Font = new Font("Calibri", 10F, FontStyle.Regular);

            //UserID textbox settings           
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

            //Userid Label settings           
            lbl_username.Height = 24;
            lbl_username.Width = 100;
            lbl_username.Text = "User Name :*";
            lbl_username.Name = "lbl_userid";
            lbl_username.TextAlign = ContentAlignment.BottomRight;
            lbl_username.Font = new Font("Calibri", 10F, FontStyle.Regular);

            //UserName textbox settings           
            txt_username.Text = "";
            txt_username.Name = "txt_username";
            txt_username.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_username.Height = 25;
            txt_username.Width = 200;

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

            //Register button settings
            btn_register.Text = "Create";
            btn_register.Font = new Font("Calibri", 10F, FontStyle.Regular);
            btn_register.Height = 25;
            btn_register.Width = 90;

            //Error Message Label settings           
            lbl_errmsg.Height = 24;
            lbl_errmsg.Width = 400;
            lbl_errmsg.Name = "lbl_errmsg";
            lbl_errmsg.TextAlign = ContentAlignment.BottomLeft;
            lbl_errmsg.Font = new Font("Calibri", 10F, FontStyle.Regular);
            lbl_errmsg.ForeColor = Color.Red;            
            lbl_errmsg.Visible = false;

            //Register Link Label settings           
            lnk_regduser.Height = 24;
            lnk_regduser.Width = 100;
            lnk_regduser.Text = "Registred User?";
            lnk_regduser.Name = "lnk_existinguser";
            lnk_regduser.TextAlign = ContentAlignment.BottomRight;
            lnk_regduser.Font = new Font("Calibri", 10F, FontStyle.Regular);

            //Adding Controls to Table Layout Panel
            tlp_register.Controls.Add(pic_gecko, 0, 1);
            tlp_register.SetRowSpan(pic_gecko, 6);
            tlp_register.Controls.Add(lbl_userid, 1, 2);
            tlp_register.Controls.Add(txt_userid, 2, 2);
            tlp_register.SetColumnSpan(txt_userid, 2);
            tlp_register.Controls.Add(lbl_username, 1, 3);
            tlp_register.Controls.Add(txt_username, 2, 3);
            tlp_register.SetColumnSpan(txt_username, 2);
            tlp_register.Controls.Add(lbl_password, 1, 4);
            tlp_register.Controls.Add(txt_password, 2, 4);
            tlp_register.SetColumnSpan(txt_password, 2);
            tlp_register.Controls.Add(lbl_projname, 1, 5);
            tlp_register.Controls.Add(cmb_projname, 2, 5);
            tlp_register.SetColumnSpan(cmb_projname, 2);
            tlp_register.Controls.Add(btn_register, 3, 7);
            tlp_register.Controls.Add(lbl_errmsg, 1, 6);
            tlp_register.SetColumnSpan(lbl_errmsg, 3);
            tlp_register.Controls.Add(lnk_regduser, 3, 8);

            //Adding Controls to Flow Layout Panel           
            flp_register.Controls.AddRange(new Control[] { tlp_register });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_register });

            #endregion

            #region register_methods
            btn_register.Click += new System.EventHandler(btn_create_Click);
            lnk_regduser.Click += new System.EventHandler(lnk_regduser_Click);
            #endregion
        }
        private void Register_Load(object sender, EventArgs e)
        {

        }
        private void btn_create_Click(object sender, EventArgs e)
        {
            userId = txt_userid.Text.Trim().ToUpper();
            userName= txt_username.Text.Trim().ToUpper();
            password = txt_password.Text;
            var encode = System.Text.Encoding.UTF8.GetBytes(password);
            var encodedpassword = System.Convert.ToBase64String(encode);
            projectName = Convert.ToString(cmb_projname.SelectedItem);

            if (!objLib.IsNullOrEmpty(userId) && !objLib.IsNullOrEmpty(userName) && !objLib.IsNullOrEmpty(password) && !objLib.IsNullOrEmpty(projectName))
            {
                if (!objLib.GetProjMaps(projectName).ContainsKey(userId.ToUpper()))
                {
                    objLib.RunQuery("INSERT INTO Users(UserId,UserName,[Password],IsAdmin,ProjectID,CreatedOn) VALUES('" + userId.ToUpper() + "','" + userName + "','"+encodedpassword+"','No','" + objLib.GetProjInfo().Single(x => x.Value == projectName).Key + "',GETDATE())");
                    MessageBox.Show("User ID : " + userId + " assigned to project : " + projectName + " Successfully.", "Sign-Up", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    lbl_errmsg.Visible = true;
                    lbl_errmsg.Text = "User ID : " + userId + " already assigned to project : " + projectName;
                }
            }
            else
            {
                lbl_errmsg.Visible = true;
                lbl_errmsg.Text = "Please provide User ID,Username and ProjectName";
            }
        }
        private void lnk_regduser_Click(object sender, EventArgs e)
        {            
            this.Close();
        }
    }
}
