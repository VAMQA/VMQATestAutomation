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
using System.Xml.Linq;
using System.Xml.XPath;


namespace UIDesign
{
    public partial class Favorites : Form
    {

        private Label lbl_FavoriteName = new Label();
        private TextBox txt_FavoriteName = new TextBox();
        private Button btn_AddFavorites = new Button();
        private PictureBox pic_gecko = new PictureBox();
        private FlowLayoutPanel flp_Favorites = new FlowLayoutPanel();
        private TableLayoutPanel tlp_Favorites = new TableLayoutPanel();

        FuncLib objLib = new FuncLib();
        int count = 0;
        public string favQuery;
        public Favorites()
        {
            //InitializeComponent();

            #region FavoritesDesign


            //Form Settings
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = SystemColors.Window;
            this.AutoScroll = true;
            this.Text = "Favourites";
            this.Size = new Size(550, 380);//380


            //Flow Layout Panel Settings            
            flp_Favorites.FlowDirection = FlowDirection.LeftToRight;
            flp_Favorites.SetFlowBreak(pic_gecko, true);
            flp_Favorites.Dock = DockStyle.Top;
            flp_Favorites.AutoSize = true;

            //Table Layout Panel Settings                                    
            tlp_Favorites.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            tlp_Favorites.Font = new Font("Calibri", 9.8F, FontStyle.Regular);
            tlp_Favorites.AutoSize = true;
            tlp_Favorites.Location = new Point(20, 40);

            //Gecko Picture Box Settings            
            pic_gecko.Height = 170;
            pic_gecko.Width = 220;
            pic_gecko.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_gecko.Image = Properties.Resources.VM;

            //Add Favorite Name Label settings            
            lbl_FavoriteName.Text = "Favourite Name";
            lbl_FavoriteName.Name = "lbl_FavoriteName";
            lbl_FavoriteName.TextAlign = ContentAlignment.BottomRight;
            lbl_FavoriteName.Font = new Font("Calibri", 11F, FontStyle.Bold);
            lbl_FavoriteName.Height = 20;
            lbl_FavoriteName.Width = 200;

            //Add Favorite Name textbox settings            
            txt_FavoriteName.Text = "";
            txt_FavoriteName.Name = "txt_FavoriteName";
            txt_FavoriteName.Font = new Font("Calibri", 11F, FontStyle.Regular);
            txt_FavoriteName.Height = 30;
            txt_FavoriteName.Width = 250;
            txt_FavoriteName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txt_FavoriteName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            //Add Favorite button settings            
            btn_AddFavorites.Text = "Save";
            btn_AddFavorites.Name = "btn_AddFavorites";
            btn_AddFavorites.Font = new Font("Calibri", 12F, FontStyle.Bold);
            btn_AddFavorites.Height = 30;
            btn_AddFavorites.Width = 90;
            btn_AddFavorites.Visible = true;
            //txt_Favorite Name
            txt_FavoriteName.Text = "";
            txt_FavoriteName.Name = "txt_FavoriteName";
            txt_FavoriteName.Font = new Font("Calibri", 12F, FontStyle.Regular);
            txt_FavoriteName.Height = 30;
            txt_FavoriteName.Width = 250;


            //frame Favorites
            tlp_Favorites.Controls.Add(lbl_FavoriteName, 0, 1);
            tlp_Favorites.Controls.Add(txt_FavoriteName, 1, 1);
            tlp_Favorites.Controls.Add(btn_AddFavorites, 1, 2);


            //Adding Controls to Flow Layout Panel
            flp_Favorites.Controls.AddRange(new Control[] { pic_gecko, tlp_Favorites });

            //Adding Controls to Form
            this.Controls.AddRange(new Control[] { flp_Favorites });
            this.Load += new System.EventHandler(Favorites_Load);

            #endregion

            #region Favorites_methods
            btn_AddFavorites.Click += new System.EventHandler(btn_AddFavorites_Click);


            #endregion

        }

        private void Favorites_Load(object sender, EventArgs e)
        {

        }

        private void btn_AddFavorites_Click(object sender, EventArgs e)
        {
            string desiredQuery = "select count(*) from Favourites where FavouriteName='" + txt_FavoriteName.Text.ToString().Trim() + "' and UserId = '" + objLib.GetUserInfo()[SignIn.userId.ToUpper()].ToString() + "'";
            int existingEntryCount = Int32.Parse(objLib.ExecuteScalar(desiredQuery).ToString());


            if (existingEntryCount > 0)
            {
                MessageBox.Show("Favourite Name Already Exist ", "Create Favourite", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.favQuery = this.favQuery.Replace('\'', '"');

                if (!string.IsNullOrEmpty(txt_FavoriteName.Text))
                {

                    try
                    {
                        string strQuery = "INSERT INTO Favourites (FavouriteName,Query,ProjectID,UserID) VALUES('"
                                       + txt_FavoriteName.Text.ToString().Trim() + "','"
                                       + this.favQuery + "',"
                                       + SignIn.projectId + ",'"
                                       + objLib.GetUserInfo()[SignIn.userId.ToUpper()].ToString() + "')";
                        objLib.RunQuery(strQuery);

                        MessageBox.Show("Favourite '" + txt_FavoriteName.Text.ToString().Trim() + "' Created Successfully", "Create Favourites", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();



                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Not able to Create Favourite ", "Create Favourite", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }


                else
                {
                    MessageBox.Show("Please provide a Favourite Name ", "Create Favourite", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }

}
