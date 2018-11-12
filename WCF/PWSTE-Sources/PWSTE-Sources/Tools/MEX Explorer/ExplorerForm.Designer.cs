// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net

using System.Drawing;
namespace ServiceModelEx
{
   partial class ExplorerForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise,false.</param>
      protected override void Dispose(bool disposing)
      {
         if(disposing &&(components != null))
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
         System.Windows.Forms.Label mexAddressLabel;
         System.Windows.Forms.MenuStrip mainMenu;
         System.Windows.Forms.ToolStripMenuItem metadataMenu;
         System.Windows.Forms.ToolStripMenuItem exploreMenuItem;
         System.Windows.Forms.ToolStripMenuItem proxyMenuItem;
         System.Windows.Forms.ToolStripMenuItem helpMenu;
         System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerForm));
         this.discoverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.serviceBusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.logInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.discoveryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         this.m_MexTree = new System.Windows.Forms.TreeView();
         this.m_ExploreButton = new System.Windows.Forms.Button();
         this.m_MexAddressTextBox = new System.Windows.Forms.TextBox();
         this.proxyButton = new System.Windows.Forms.Button();
         this.m_DiscoverButton = new System.Windows.Forms.Button();
         this.m_BlankViewControl = new ServiceModelEx.NodeViewControl();
         this.m_EndpointViewControl = new ServiceModelEx.EndpointViewControl();
         this.m_OperationViewControl = new ServiceModelEx.OperationViewControl();
         this.m_BindingViewControl = new ServiceModelEx.BindingViewControl();
         this.m_ServiceViewControl = new ServiceModelEx.ServiceViewControl();
         this.m_AddressViewControl = new ServiceModelEx.AddressViewControl();
         this.m_ContractViewControl = new ServiceModelEx.ContractViewControl();
         mexAddressLabel = new System.Windows.Forms.Label();
         mainMenu = new System.Windows.Forms.MenuStrip();
         metadataMenu = new System.Windows.Forms.ToolStripMenuItem();
         exploreMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         proxyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         helpMenu = new System.Windows.Forms.ToolStripMenuItem();
         aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
         mainMenu.SuspendLayout();
         this.SuspendLayout();
         // 
         // mexAddressLabel
         // 
         mexAddressLabel.AutoSize = true;
         mexAddressLabel.Location = new System.Drawing.Point(12,35);
         mexAddressLabel.Name = "mexAddressLabel";
         mexAddressLabel.Size = new System.Drawing.Size(147,13);
         mexAddressLabel.TabIndex = 3;
         mexAddressLabel.Text = "Metadata Exchange Address:";
         // 
         // mainMenu
         // 
         mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            metadataMenu,
            this.serviceBusToolStripMenuItem,
            helpMenu});
         mainMenu.Location = new System.Drawing.Point(0,0);
         mainMenu.Name = "mainMenu";
         mainMenu.Size = new System.Drawing.Size(666,24);
         mainMenu.TabIndex = 17;
         mainMenu.Text = "menuStrip1";
         // 
         // metadataMenu
         // 
         metadataMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            exploreMenuItem,
            this.discoverToolStripMenuItem,
            proxyMenuItem});
         metadataMenu.Name = "metadataMenu";
         metadataMenu.Size = new System.Drawing.Size(49,20);
         metadataMenu.Text = "Action";
         // 
         // exploreMenuItem
         // 
         exploreMenuItem.Image = global::ServiceModelEx.Properties.Resources.Explore;
         exploreMenuItem.Name = "exploreMenuItem";
         exploreMenuItem.Size = new System.Drawing.Size(152,22);
         exploreMenuItem.Text = "Explore";
         exploreMenuItem.Click += new System.EventHandler(this.OnExplore);
         // 
         // discoverToolStripMenuItem
         // 
         this.discoverToolStripMenuItem.Image = global::ServiceModelEx.Properties.Resources.Discover;
         this.discoverToolStripMenuItem.Name = "discoverToolStripMenuItem";
         this.discoverToolStripMenuItem.Size = new System.Drawing.Size(152,22);
         this.discoverToolStripMenuItem.Text = "Discover";
         this.discoverToolStripMenuItem.Click += new System.EventHandler(this.OnDiscover);
         // 
         // proxyMenuItem
         // 
         proxyMenuItem.Image = global::ServiceModelEx.Properties.Resources.Proxy;
         proxyMenuItem.Name = "proxyMenuItem";
         proxyMenuItem.Size = new System.Drawing.Size(152,22);
         proxyMenuItem.Text = "Generate Proxy";
         proxyMenuItem.Click += new System.EventHandler(this.OnGenerateProxy);
         // 
         // serviceBusToolStripMenuItem
         // 
         this.serviceBusToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logInToolStripMenuItem,
            this.discoveryToolStripMenuItem});
         this.serviceBusToolStripMenuItem.Name = "serviceBusToolStripMenuItem";
         this.serviceBusToolStripMenuItem.Size = new System.Drawing.Size(76,20);
         this.serviceBusToolStripMenuItem.Text = "Service Bus";
         // 
         // logInToolStripMenuItem
         // 
         this.logInToolStripMenuItem.Image = global::ServiceModelEx.Properties.Resources.Logon;
         this.logInToolStripMenuItem.Name = "logInToolStripMenuItem";
         this.logInToolStripMenuItem.Size = new System.Drawing.Size(152,22);
         this.logInToolStripMenuItem.Text = "Log in...";
         this.logInToolStripMenuItem.Click += new System.EventHandler(this.OnServiceBusLogOn);
         // 
         // discoveryToolStripMenuItem
         // 
         this.discoveryToolStripMenuItem.Image = global::ServiceModelEx.Properties.Resources.Discover;
         this.discoveryToolStripMenuItem.Name = "discoveryToolStripMenuItem";
         this.discoveryToolStripMenuItem.Size = new System.Drawing.Size(152,22);
         this.discoveryToolStripMenuItem.Text = "Discovery...";
         this.discoveryToolStripMenuItem.Click += new System.EventHandler(this.OnConfigureDiscovery);
         // 
         // helpMenu
         // 
         helpMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            aboutMenuItem});
         helpMenu.Name = "helpMenu";
         helpMenu.Size = new System.Drawing.Size(41,20);
         helpMenu.Text = "Help";
         // 
         // aboutMenuItem
         // 
         aboutMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutMenuItem.Image")));
         aboutMenuItem.Name = "aboutMenuItem";
         aboutMenuItem.Size = new System.Drawing.Size(105,22);
         aboutMenuItem.Text = "About";
         aboutMenuItem.Click += new System.EventHandler(this.OnAbout);
         // 
         // m_MexTree
         // 
         this.m_MexTree.ImeMode = System.Windows.Forms.ImeMode.NoControl;
         this.m_MexTree.Location = new System.Drawing.Point(12,82);
         this.m_MexTree.Name = "m_MexTree";
         this.m_MexTree.Size = new System.Drawing.Size(242,339);
         this.m_MexTree.TabIndex = 0;
         this.m_MexTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnItemSelected);
         // 
         // m_ExploreButton
         // 
         this.m_ExploreButton.Location = new System.Drawing.Point(418,53);
         this.m_ExploreButton.Name = "m_ExploreButton";
         this.m_ExploreButton.Size = new System.Drawing.Size(75,23);
         this.m_ExploreButton.TabIndex = 2;
         this.m_ExploreButton.Text = "Explore";
         this.m_ExploreButton.UseVisualStyleBackColor = true;
         this.m_ExploreButton.Click += new System.EventHandler(this.OnExplore);
         // 
         // m_MexAddressTextBox
         // 
         this.m_MexAddressTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
         this.m_MexAddressTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
         this.m_MexAddressTextBox.Location = new System.Drawing.Point(12,53);
         this.m_MexAddressTextBox.Name = "m_MexAddressTextBox";
         this.m_MexAddressTextBox.Size = new System.Drawing.Size(400,20);
         this.m_MexAddressTextBox.TabIndex = 4;
         this.m_MexAddressTextBox.Text = "http://localhost:8000/";
         // 
         // proxyButton
         // 
         this.proxyButton.Location = new System.Drawing.Point(580,53);
         this.proxyButton.Name = "proxyButton";
         this.proxyButton.Size = new System.Drawing.Size(75,23);
         this.proxyButton.TabIndex = 2;
         this.proxyButton.Text = "Proxy";
         this.proxyButton.UseVisualStyleBackColor = true;
         this.proxyButton.Click += new System.EventHandler(this.OnGenerateProxy);
         // 
         // m_DiscoverButton
         // 
         this.m_DiscoverButton.Location = new System.Drawing.Point(499,53);
         this.m_DiscoverButton.Name = "m_DiscoverButton";
         this.m_DiscoverButton.Size = new System.Drawing.Size(75,23);
         this.m_DiscoverButton.TabIndex = 18;
         this.m_DiscoverButton.Text = "Discover";
         this.m_DiscoverButton.UseVisualStyleBackColor = true;
         this.m_DiscoverButton.Click += new System.EventHandler(this.OnDiscover);
         // 
         // m_BlankViewControl
         // 
         this.m_BlankViewControl.BackColor = System.Drawing.SystemColors.ControlDark;
         this.m_BlankViewControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.m_BlankViewControl.Location = new System.Drawing.Point(270,82);
         this.m_BlankViewControl.Name = "m_BlankViewControl";
         this.m_BlankViewControl.Size = new System.Drawing.Size(385,337);
         this.m_BlankViewControl.TabIndex = 16;
         this.m_BlankViewControl.Visible = false;
         // 
         // m_EndpointViewControl
         // 
         this.m_EndpointViewControl.BackColor = System.Drawing.SystemColors.Control;
         this.m_EndpointViewControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.m_EndpointViewControl.Location = new System.Drawing.Point(270,82);
         this.m_EndpointViewControl.Name = "m_EndpointViewControl";
         this.m_EndpointViewControl.Size = new System.Drawing.Size(385,337);
         this.m_EndpointViewControl.TabIndex = 15;
         this.m_EndpointViewControl.Visible = false;
         // 
         // m_OperationViewControl
         // 
         this.m_OperationViewControl.BackColor = System.Drawing.SystemColors.Control;
         this.m_OperationViewControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.m_OperationViewControl.Location = new System.Drawing.Point(270,82);
         this.m_OperationViewControl.Name = "m_OperationViewControl";
         this.m_OperationViewControl.Size = new System.Drawing.Size(385,337);
         this.m_OperationViewControl.TabIndex = 14;
         this.m_OperationViewControl.Visible = false;
         // 
         // m_BindingViewControl
         // 
         this.m_BindingViewControl.BackColor = System.Drawing.SystemColors.Control;
         this.m_BindingViewControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.m_BindingViewControl.Location = new System.Drawing.Point(270,82);
         this.m_BindingViewControl.Name = "m_BindingViewControl";
         this.m_BindingViewControl.Size = new System.Drawing.Size(385,337);
         this.m_BindingViewControl.TabIndex = 13;
         this.m_BindingViewControl.Visible = false;
         // 
         // m_ServiceViewControl
         // 
         this.m_ServiceViewControl.BackColor = System.Drawing.SystemColors.Control;
         this.m_ServiceViewControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.m_ServiceViewControl.Location = new System.Drawing.Point(270,82);
         this.m_ServiceViewControl.Name = "m_ServiceViewControl";
         this.m_ServiceViewControl.Size = new System.Drawing.Size(385,337);
         this.m_ServiceViewControl.TabIndex = 12;
         this.m_ServiceViewControl.Visible = false;
         // 
         // m_AddressViewControl
         // 
         this.m_AddressViewControl.BackColor = System.Drawing.SystemColors.Control;
         this.m_AddressViewControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.m_AddressViewControl.Location = new System.Drawing.Point(270,82);
         this.m_AddressViewControl.Name = "m_AddressViewControl";
         this.m_AddressViewControl.Size = new System.Drawing.Size(385,337);
         this.m_AddressViewControl.TabIndex = 11;
         this.m_AddressViewControl.Visible = false;
         // 
         // m_ContractViewControl
         // 
         this.m_ContractViewControl.BackColor = System.Drawing.SystemColors.Control;
         this.m_ContractViewControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.m_ContractViewControl.Location = new System.Drawing.Point(270,82);
         this.m_ContractViewControl.Name = "m_ContractViewControl";
         this.m_ContractViewControl.Size = new System.Drawing.Size(385,337);
         this.m_ContractViewControl.TabIndex = 7;
         this.m_ContractViewControl.Visible = false;
         // 
         // ExplorerForm
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F,13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(666,433);
         this.Controls.Add(this.m_DiscoverButton);
         this.Controls.Add(this.m_BlankViewControl);
         this.Controls.Add(this.m_EndpointViewControl);
         this.Controls.Add(this.m_OperationViewControl);
         this.Controls.Add(this.m_BindingViewControl);
         this.Controls.Add(this.m_ServiceViewControl);
         this.Controls.Add(this.m_AddressViewControl);
         this.Controls.Add(this.m_ContractViewControl);
         this.Controls.Add(this.m_MexAddressTextBox);
         this.Controls.Add(mexAddressLabel);
         this.Controls.Add(this.proxyButton);
         this.Controls.Add(this.m_ExploreButton);
         this.Controls.Add(this.m_MexTree);
         this.Controls.Add(mainMenu);
         this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.MainMenuStrip = mainMenu;
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "ExplorerForm";
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.Text = " Metadata Explorer";
         this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnClosed);
         mainMenu.ResumeLayout(false);
         mainMenu.PerformLayout();
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.TreeView m_MexTree;
      private System.Windows.Forms.Button m_ExploreButton;
      System.Windows.Forms.TextBox m_MexAddressTextBox;

      private ServiceViewControl m_ServiceViewControl;
      private AddressViewControl m_AddressViewControl;
      private ContractViewControl m_ContractViewControl;
      private BindingViewControl m_BindingViewControl;
      private OperationViewControl m_OperationViewControl;
      private EndpointViewControl m_EndpointViewControl;
      private NodeViewControl m_BlankViewControl;
      private System.Windows.Forms.Button proxyButton;
      private System.Windows.Forms.Button m_DiscoverButton;
      private System.Windows.Forms.ToolStripMenuItem discoverToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem serviceBusToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem logInToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem discoveryToolStripMenuItem;
   }
}

