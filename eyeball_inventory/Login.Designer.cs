namespace EYEBALL_INVENTORY
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemFrame tileItemFrame1 = new DevExpress.XtraEditors.TileItemFrame();
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemFrame tileItemFrame2 = new DevExpress.XtraEditors.TileItemFrame();
            DevExpress.XtraEditors.TileItemElement tileItemElement4 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.Utils.SuperToolTip superToolTip1 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem1 = new DevExpress.Utils.ToolTipItem();
            DevExpress.Utils.SuperToolTip superToolTip2 = new DevExpress.Utils.SuperToolTip();
            DevExpress.Utils.ToolTipItem toolTipItem2 = new DevExpress.Utils.ToolTipItem();
            this.tileControl1 = new DevExpress.XtraEditors.TileControl();
            this.tileControl2 = new DevExpress.XtraEditors.TileControl();
            this.tileGroup2 = new DevExpress.XtraEditors.TileGroup();
            this.tileItem2 = new DevExpress.XtraEditors.TileItem();
            this.txtUsername = new DevExpress.XtraEditors.TextEdit();
            this.btnLogin = new DevExpress.XtraEditors.SimpleButton();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tileControl1
            // 
            this.tileControl1.AllowItemHover = true;
            this.tileControl1.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.tileControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileControl1.Location = new System.Drawing.Point(0, 0);
            this.tileControl1.Name = "tileControl1";
            this.tileControl1.Size = new System.Drawing.Size(240, 150);
            this.tileControl1.TabIndex = 0;
            // 
            // tileControl2
            // 
            this.tileControl2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tileControl2.Groups.Add(this.tileGroup2);
            this.tileControl2.Location = new System.Drawing.Point(12, 12);
            this.tileControl2.MaxId = 4;
            this.tileControl2.Name = "tileControl2";
            this.tileControl2.Size = new System.Drawing.Size(313, 344);
            this.tileControl2.TabIndex = 0;
            this.tileControl2.Text = "tileControl2";
            // 
            // tileGroup2
            // 
            this.tileGroup2.Items.Add(this.tileItem2);
            this.tileGroup2.Name = "tileGroup2";
            this.tileGroup2.Text = null;
            // 
            // tileItem2
            // 
            this.tileItem2.AppearanceItem.Hovered.ForeColor = System.Drawing.Color.Maroon;
            this.tileItem2.AppearanceItem.Hovered.Options.UseForeColor = true;
            this.tileItem2.AppearanceItem.Normal.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.tileItem2.AppearanceItem.Normal.ForeColor = System.Drawing.Color.Maroon;
            this.tileItem2.AppearanceItem.Normal.Options.UseBackColor = true;
            this.tileItem2.AppearanceItem.Normal.Options.UseForeColor = true;
            this.tileItem2.AppearanceItem.Pressed.ForeColor = System.Drawing.Color.Maroon;
            this.tileItem2.AppearanceItem.Pressed.Options.UseForeColor = true;
            this.tileItem2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tileItem2.BackgroundImage")));
            this.tileItem2.BackgroundImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.tileItem2.BackgroundImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Stretch;
            this.tileItem2.ContentAnimation = DevExpress.XtraEditors.TileItemContentAnimationType.ScrollRight;
            this.tileItem2.CurrentFrameIndex = 1;
            tileItemElement1.Text = "<b>EYEBALL     TECH</b>";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopLeft;
            this.tileItem2.Elements.Add(tileItemElement1);
            tileItemFrame1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tileItemFrame1.BackgroundImage")));
            tileItemElement2.Text = "<b>Reaching your every </br>";
            tileItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopLeft;
            tileItemElement3.Text = "<b> IT needs</b>";
            tileItemElement3.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.BottomRight;
            tileItemFrame1.Elements.Add(tileItemElement2);
            tileItemFrame1.Elements.Add(tileItemElement3);
            tileItemFrame2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tileItemFrame2.BackgroundImage")));
            tileItemElement4.Text = "<b>EYEBALL     TECH</b>";
            tileItemElement4.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.TopLeft;
            tileItemFrame2.Elements.Add(tileItemElement4);
            this.tileItem2.Frames.Add(tileItemFrame1);
            this.tileItem2.Frames.Add(tileItemFrame2);
            this.tileItem2.Id = 3;
            this.tileItem2.ItemSize = DevExpress.XtraEditors.TileItemSize.Large;
            this.tileItem2.Name = "tileItem2";
            this.tileItem2.Tag = "<Null>";
            // 
            // txtUsername
            // 
            this.txtUsername.EditValue = "username";
            this.txtUsername.Location = new System.Drawing.Point(459, 90);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtUsername.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtUsername.Properties.Appearance.Options.UseBackColor = true;
            this.txtUsername.Properties.Appearance.Options.UseFont = true;
            this.txtUsername.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.txtUsername.Size = new System.Drawing.Size(173, 24);
            toolTipItem1.Text = "Username here!!";
            superToolTip1.Items.Add(toolTipItem1);
            this.txtUsername.SuperTip = superToolTip1;
            this.txtUsername.TabIndex = 1;
            this.txtUsername.Click += new System.EventHandler(this.txtUsername_Click);
            this.txtUsername.Enter += new System.EventHandler(this.txtUsername_Enter);
            // 
            // btnLogin
            // 
            this.btnLogin.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnLogin.Appearance.Options.UseFont = true;
            this.btnLogin.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.btnLogin.Location = new System.Drawing.Point(475, 250);
            this.btnLogin.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(131, 46);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "LOGIN";
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.EditValue = "password";
            this.txtPassword.Location = new System.Drawing.Point(459, 153);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtPassword.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.txtPassword.Properties.Appearance.Options.UseBackColor = true;
            this.txtPassword.Properties.Appearance.Options.UseFont = true;
            this.txtPassword.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.txtPassword.Properties.Mask.EditMask = "\\d\\d\\d\\d\\d-\\d?\\d?\\d?\\d?";
            this.txtPassword.Properties.Mask.PlaceHolder = '$';
            this.txtPassword.Properties.UseSystemPasswordChar = true;
            this.txtPassword.Properties.ValidateOnEnterKey = true;
            this.txtPassword.Size = new System.Drawing.Size(173, 24);
            toolTipItem2.Text = "Password here!!";
            superToolTip2.Items.Add(toolTipItem2);
            this.txtPassword.SuperTip = superToolTip2;
            this.txtPassword.TabIndex = 2;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            this.txtPassword.Click += new System.EventHandler(this.txtPassword_Click);
            this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(383, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(383, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Password";
            // 
            // Login
            // 
            this.AcceptButton = this.btnLogin;
            this.Appearance.BackColor = System.Drawing.Color.GhostWhite;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 368);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.tileControl2);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Login_FormClosed);
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txtUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TileControl tileControl1;
        private DevExpress.XtraEditors.TileControl tileControl2;
        private DevExpress.XtraEditors.TileGroup tileGroup2;
        private DevExpress.XtraEditors.TileItem tileItem2;
        private DevExpress.XtraEditors.TextEdit txtUsername;
        private DevExpress.XtraEditors.SimpleButton btnLogin;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}