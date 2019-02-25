namespace UIDesign
{
    partial class SampleTestData
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sampleTestDataGrid = new System.Windows.Forms.DataGridView();
            this.cmdClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sampleTestDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // sampleTestDataGrid
            // 
            this.sampleTestDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.sampleTestDataGrid.Location = new System.Drawing.Point(12, 117);
            this.sampleTestDataGrid.Name = "sampleTestDataGrid";
            this.sampleTestDataGrid.Size = new System.Drawing.Size(1087, 420);
            this.sampleTestDataGrid.TabIndex = 0;
            this.sampleTestDataGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.sampleTestDataGrid_MouseClick);
            // 
            // cmdClose
            // 
            this.cmdClose.Location = new System.Drawing.Point(1011, 573);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(87, 33);
            this.cmdClose.TabIndex = 1;
            this.cmdClose.Text = "&Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // SampleTestData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 638);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.sampleTestDataGrid);
            this.Name = "SampleTestData";
            this.Text = "SampleTestData";
            this.Load += new System.EventHandler(this.SampleTestData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sampleTestDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView sampleTestDataGrid;
        private System.Windows.Forms.Button cmdClose;
    }
}