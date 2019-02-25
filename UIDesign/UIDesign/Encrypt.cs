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


namespace UIDesign
{
    public partial class Encrypt : Form
    {

        private Label lbl_Inputstring = new Label();
        private Label lbl_Encryptedstring = new Label();
        private TextBox txt_Inputstring = new TextBox();
        private Button btn_Encrypt = new Button();
        private TextBox txt_Encrypt = new TextBox();
        private PictureBox pic_gecko = new PictureBox();

        private FlowLayoutPanel flp_Encrypt = new FlowLayoutPanel();
        private TableLayoutPanel tlp_Encrypt = new TableLayoutPanel();


        FuncLib objLib = new FuncLib();



        public Encrypt()
        {
            //InitializeComponent();

            #region EncryptDesign


            //Form Settings
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            this.AutoScroll = true;
            this.Text = "Encrypt";
            this.Size = new Size(550, 450);//380


            //Flow Layout Panel Settings            
            flp_Encrypt.FlowDirection = FlowDirection.LeftToRight;
            flp_Encrypt.SetFlowBreak(pic_gecko, true);
            flp_Encrypt.Dock = DockStyle.Top;
            flp_Encrypt.AutoSize = true;

            //Table Layout Panel Settings                                    
            tlp_Encrypt.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_Encrypt.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_Encrypt.AutoSize = true;
            tlp_Encrypt.Location = new Point(20, 40);

            //Gecko Picture Box Settings            
            pic_gecko.Height = 170;
            pic_gecko.Width = 220;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            //Add Inputstring Label settings            
            lbl_Inputstring.Text = "Input Text :";
            lbl_Inputstring.Name = "lbl_Inputstring";
            lbl_Inputstring.TextAlign = ContentAlignment.BottomRight;
            lbl_Inputstring.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_Inputstring.Height = 20;
            lbl_Inputstring.Width = 200;

            //Add Encrypt Label settings            
            lbl_Encryptedstring.Text = "Encrypted Text :";
            lbl_Encryptedstring.Name = "lbl_Encryptedstring";
            lbl_Encryptedstring.TextAlign = ContentAlignment.BottomRight;
            lbl_Encryptedstring.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_Encryptedstring.Height = 20;
            lbl_Encryptedstring.Width = 200;

            //Add inputstring textbox settings            
            txt_Inputstring.Text = "";
            txt_Inputstring.Name = "txt_Inputstring";
            txt_Inputstring.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_Inputstring.Height = 30;
            txt_Inputstring.Width = 250;
            txt_Inputstring.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_Inputstring.AutoCompleteMode = AutoCompleteMode.SuggestAppend;


            //Add Encrypt button settings            
            btn_Encrypt.Text = "Encrypt";
            btn_Encrypt.Name = "btn_Encrypt";
            btn_Encrypt.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_Encrypt.Height = 30;
            btn_Encrypt.Width = 100;
            btn_Encrypt.Visible = true;
            //txt_Encrypt
            txt_Encrypt.Text = "";
            txt_Encrypt.Name = "txt_Encrypt";
            //txt_Encrypt.Enabled = false;
            txt_Encrypt.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_Encrypt.Height = 30;
            txt_Encrypt.Width = 250;


            //frame Encrypt
            tlp_Encrypt.Controls.Add(lbl_Inputstring, 0, 1);
            tlp_Encrypt.Controls.Add(txt_Inputstring, 1, 1);
            tlp_Encrypt.Controls.Add(btn_Encrypt, 1, 2);
            tlp_Encrypt.Controls.Add(lbl_Encryptedstring, 0, 3);
            tlp_Encrypt.Controls.Add(txt_Encrypt, 1, 3);

            //Adding Controls to Flow Layout Panel
            flp_Encrypt.Controls.AddRange(new Control[] { pic_gecko, tlp_Encrypt });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_Encrypt });
            this.Load += new System.EventHandler(Encrypt_Load);

            #endregion

            #region Encrypt_methods

            btn_Encrypt.Click += new System.EventHandler(btn_Encrypt_Click);


            #endregion

        }

        private void Encrypt_Load(object sender, EventArgs e)
        {

        }

        private void btn_Encrypt_Click(object sender, EventArgs e)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(txt_Inputstring.Text.ToString());
            string encodedText = Convert.ToBase64String(plainTextBytes);
            txt_Encrypt.Text = encodedText;


        }
    }
}
