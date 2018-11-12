using System;
using System.Windows.Forms;
using Library;

namespace MVP
{
    /// <summary>
    /// Summary description for UserView.
    /// </summary>
    public class UserView : Form, IUserView
    {
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public UserView()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            // TODO: Add any initialization after the InitializeComponent call

        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(96, 16);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(144, 20);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.Text = "";
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // lblUserName
            // 
            this.lblUserName.Location = new System.Drawing.Point(0, 16);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(72, 16);
            this.lblUserName.TabIndex = 1;
            this.lblUserName.Text = "User name:";
            // 
            // lblEmail
            // 
            this.lblEmail.Location = new System.Drawing.Point(0, 46);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(80, 16);
            this.lblEmail.TabIndex = 2;
            this.lblEmail.Text = "Email Address:";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(96, 40);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(144, 20);
            this.txtEmail.TabIndex = 3;
            this.txtEmail.Text = "";
            this.txtEmail.TextChanged += new System.EventHandler(this.txtEmail_TextChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(88, 72);
            this.btnSave.Name = "btnSave";
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(168, 72);
            this.btnClose.Name = "btnClose";
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // UserView
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(248, 109);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.lblUserName);
            this.Controls.Add(this.txtUserName);
            this.Name = "UserView";
            this.ResumeLayout(false);

        }
        #endregion

        private void txtUserName_TextChanged(object sender, System.EventArgs e)
        {
            if (this.DataChanged != null)
            {
                this.DataChanged(sender, e);
            }
        }

        private void txtEmail_TextChanged(object sender, System.EventArgs e)
        {
            if (this.DataChanged != null)
            {
                this.DataChanged(sender, e);
            }
        }

        private void btnSave_Click(object sender, System.EventArgs e)
        {
            if (this.Save != null)
            {
                this.Save(sender, e);
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        public event EventHandler DataChanged;
        public event EventHandler Save;

        public string UserName
        {
            get { return this.txtUserName.Text; }
            set { this.txtUserName.Text = value; }
        }

        public string Email
        {
            get { return this.txtEmail.Text; }
            set { this.txtEmail.Text = value; }
        }
    }
}
