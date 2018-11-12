// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CredentialsManagerClient.Properties;
using System.Diagnostics;
using System.Collections;
using System.Threading;
using System.Web.Services.Protocols;

namespace CredentialsManagerClient
{
   partial class CredentialsManagerForm : Form
   {
      public CredentialsManagerForm()
      {
         SplashScreen splashScreen = new SplashScreen(Resources.Splash);

         Thread.CurrentThread.Name = " Main UI Thread";
         m_DownloadCompletedEvent = new ManualResetEvent(false);

         InitializeComponent();
         m_ApplicationListView.SmallImageList = new ImageList();
         m_ApplicationListView.SmallImageList.Images.Add(Resources.Application);

         m_UsersListView.SmallImageList = new ImageList();
         m_UsersListView.SmallImageList.Images.Add(Resources.User);

         m_UsersToAssignListView.SmallImageList = new ImageList();
         m_UsersToAssignListView.SmallImageList.Images.Add(Resources.User);

         m_RolesListView.SmallImageList = new ImageList();
         m_RolesListView.SmallImageList.Images.Add(Resources.Role);

         m_UsersInRoleComboBox.SetImage(Resources.User);
         m_RolesForUserComboBox.SetImage(Resources.Role);

         ServiceAddress = Settings.Default.AspNetSqlProviderService;
         m_AddressTextbox.Text = ServiceAddress;
         RefreshServicePage();
         RefreshApplicationsPage();

         splashScreen.Close();
      }    
      void OnDeleteApplication(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to delete the " + ApplicationName + " application? This will remove all users and roles already defined.","Credentials Manager",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);

         if(result == DialogResult.OK)
         {
            using(ApplicationManagerProxy applicationManager = new ApplicationManagerProxy(ServiceAddress))
            {
               applicationManager.DeleteApplication(ApplicationName);

               m_ApplicationListView.RemoveItem(ApplicationName);

               SelectedApplicationChanged();
               RefreshApplicationButtons();
            }
         }
      }
      void OnCreateApplication(object sender,EventArgs e)
      {
         CreateApplicationDialog createApplicationDialog = new CreateApplicationDialog(ServiceAddress,Applications);
         createApplicationDialog.ShowDialog(this);

         string[] applications = createApplicationDialog.Applications;
         m_ApplicationListView.AddItems(applications,false);
         SelectedApplicationChanged();
         RefreshApplicationButtons();
      }
      void OnCreateUser(object sender,EventArgs e)
      {
         CreateUserDialog newUserDialog = new CreateUserDialog(ServiceAddress,ApplicationName);
         newUserDialog.ShowDialog();
         string[] users = newUserDialog.Users;
         if(users.Length == 0)
         {
            return;
         }
         m_UsersListView.AddItems(users,false);
         RefreshUserStatus();
         RefreshUsersPageButtonsAndMenuItems();

         //Upade the roles page
         string currentUserToAssign = UserToAssign;
         m_UsersToAssignListView.AddItems(users,false);

         if(currentUserToAssign != String.Empty)
         {
            m_UsersToAssignListView.FindItem(currentUserToAssign).Selected = true;
            m_UsersToAssignListView.Select();
         }
         RefreshRolesForUserComboBox();
         RefreshRolePageButtons();
      }
      void OnDeleteUser(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to delete the user " + UserName + " ?","Credentials Manager",MessageBoxButtons.OKCancel);
         if(result == DialogResult.OK)
         {
            using(MembershipManagerProxy membershipManager = new MembershipManagerProxy(ServiceAddress))
            {
               bool deleted = membershipManager.DeleteUser(ApplicationName,UserName,m_RelatedDataCheckBox.Checked);
               if(deleted == false)
               {
                  MessageBox.Show("Encountered an error trying to delete user " + UserName,"Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Error);
               }
               else
               {
                  //Upade the roles page
                  m_UsersToAssignListView.RemoveItem(UserName);
                  RefreshUsersForRoleComboBox();
                  RefreshRolesForUserComboBox();
                  RefreshRolePageButtons();

                  //Update the users list
                  m_UsersListView.RemoveItem(UserName);
                  RefreshUserStatus();
                  RefreshUsersPageButtonsAndMenuItems();
               }
            }
         }
      }
      void OnUpdateUser(object sender,EventArgs e)
      {
         UpdateUserDialog updateUserDialog = new UpdateUserDialog(ServiceAddress,ApplicationName,UserName);
         updateUserDialog.ShowDialog();
      }

      void OnUserStatusRefresh(object sender,EventArgs e)
      {
         RefreshUserStatus();
      }
      void OnDeleteRole(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to delete the role " + RoleName + " ?","Credentials Manager",MessageBoxButtons.OKCancel);
         if(result == DialogResult.OK)
         {
            using(RoleManagerProxy roleManager = new RoleManagerProxy(ServiceAddress))
            {
               try
               {
                  bool deleted = roleManager.DeleteRole(ApplicationName,RoleName,m_ThrowIfPopulatedCheckBox.Checked);
                  if(deleted == false)
                  {
                     MessageBox.Show("Encountered an error trying to delete role " + m_RolesListView.CurrentListViewItem,"Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Error);
                  }
                  else
                  {
                     m_RolesListView.RemoveItem(RoleName);
                     RefreshUsersForRoleComboBox();
                     RefreshRolesForUserComboBox();
                     RefreshRolePageButtons();
                  }
               }
               catch(SoapException exception)
               {
                  if(exception.Message.Contains("This role cannot be deleted because there are users present in it"))
                  {
                     MessageBox.Show("Failed to delete role " + m_RolesListView.CurrentListViewItem + " because it was populated.","Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Error);
                  }
                  else
                  {
                     throw;
                  }
               }
            }
         }
      }
      void OnCreateRole(object sender,EventArgs e)
      {
         CreateRoleDialog newRoleDialog = new CreateRoleDialog(ServiceAddress,ApplicationName);
         newRoleDialog.ShowDialog();
         string[] roles = newRoleDialog.Roles;
         if(roles.Length == 0)
         {
            return;
         }
         m_RolesListView.AddItems(roles,true);
         RefreshUsersForRoleComboBox();
         RefreshRolePageButtons();
      }
      void OnAssignUserToRole(object sender,EventArgs e)
      {
         using(RoleManagerProxy roleManager = new RoleManagerProxy(ServiceAddress))
         {
            using(UserManagerProxy userManager = new UserManagerProxy(ServiceAddress))
            {
               Debug.Assert(userManager != null);

               string role = m_RolesListView.CurrentListViewItem;
               Debug.Assert(role != String.Empty);

               if(userManager.IsInRole(ApplicationName,UserToAssign,role))
               {
                  MessageBox.Show("The user " + UserToAssign + " is already a member of the role " +  m_RolesListView.CurrentListViewItem,"Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                  return;
               }
               else
               {
                  roleManager.AddUserToRole(ApplicationName,UserToAssign,m_RolesListView.CurrentListViewItem);
                  RefreshRolesForUserComboBox();
                  RefreshUsersForRoleComboBox();
                  RefreshRolePageButtons();
               }
            }
         }
      }
      void OnRemoveUserFromRole(object sender,EventArgs e)
      {
         string role = m_RolesListView.CurrentListViewItem;
         Debug.Assert(! String.IsNullOrEmpty(role));
         using(RoleManagerProxy roleManager = new RoleManagerProxy(ServiceAddress))
         {
            using(UserManagerProxy userManager = new UserManagerProxy(ServiceAddress))
            {
               Debug.Assert(userManager != null);

               if(userManager.IsInRole(ApplicationName,UserToAssign,role))
               {
                  DialogResult result = MessageBox.Show("Are you sure you want to remove the user " + UserToAssign + " from the role " +  m_RolesListView.CurrentListViewItem + " ?","Credentials Manager",MessageBoxButtons.OKCancel);
                  if(result == DialogResult.OK)
                  {
                     roleManager.RemoveUserFromRole(ApplicationName,UserToAssign,role);
                     RefreshRolesForUserComboBox();
                     RefreshUsersForRoleComboBox();
                     RefreshRolePageButtons();
                  }
               }
               else
               {
                  MessageBox.Show("The user " + UserToAssign + " is not a member of the role " +  m_RolesListView.CurrentListViewItem,"Credentials Manager",MessageBoxButtons.OKCancel,MessageBoxIcon.Error);
               }
            }
         }
      }
      void OnGeneratePassword(object sender,EventArgs e)
      {
         int length = Convert.ToInt32(m_LengthTextBox.Text);
         int nonAlphanumeric = Convert.ToInt32(m_NonAlphanumericTextBox.Text);

         using(PasswordManagerProxy passwordManager = new PasswordManagerProxy(ServiceAddress))
         {
            string password = passwordManager.GeneratePassword(ServiceAddress,length,nonAlphanumeric);
            Clipboard.SetText(password);
      
            MessageBox.Show("Generated password: " + password + " " + Environment.NewLine + "The password is avaiable on the clipboard as well.","Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Information);
         }
      }
      void OnViewService(object sender,EventArgs e)
      {
         m_ViewMenuItem.Enabled = m_ViewButton.Enabled = false;
         m_WebBrowser.DocumentText = "<body bgcolor=\"#808080\">";
         Application.DoEvents();
         m_DownloadCompletedEvent.Reset();
         m_WebBrowser.Navigate(m_AddressTextbox.Text);
         while(true)
         {
            Application.DoEvents();
            bool downloadComplete = m_DownloadCompletedEvent.WaitOne(TimeSpan.Zero,false);
            if(downloadComplete)
            {
               m_ViewMenuItem.Enabled = m_ViewButton.Enabled = true;
               return;
            }
         }
      }

      void OnClosing(object sender,FormClosingEventArgs e)
      {
         if(ValidAddress)
         {
            using(ApplicationManagerProxy applicationManager = new ApplicationManagerProxy(ServiceAddress))
            {
               string[] applicationsOnServer = applicationManager.GetApplications();
               List<string> applicationsOnServerList = new List<string>(applicationsOnServer);

               Predicate<string> contain = (str)=>
                                           {
                                              return applicationsOnServerList.Contains(str);
                                           };
               bool unsavedApps = !Array.TrueForAll(Applications,contain);
               if(unsavedApps)
               {
                  DialogResult result = MessageBox.Show("One or more applications have no users or roles defined. Closing the Credentials Manager application will delete those applications. Click OK to close or Cancel to continute using Credentials Manager.","Credentials Manager",MessageBoxButtons.OKCancel,MessageBoxIcon.Exclamation);
                  if(result == DialogResult.Cancel)
                  {
                     e.Cancel = true;
                     return;
                  }
               }
            }
         }
      }
      void OnDeleteAllUsers(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to delete all the users?","Credentials Manager",MessageBoxButtons.OKCancel);
         if(result == DialogResult.OK)
         {
            using(MembershipManagerProxy membershipManager = new MembershipManagerProxy(ServiceAddress))
            {
               membershipManager.DeleteAllUsers(ApplicationName,m_RelatedDataCheckBox.Checked);

               //Update the users page
               m_UsersListView.ClearItems();
               RefreshUserStatus();
               RefreshUsersPageButtonsAndMenuItems();

               //Upade the roles page
               RefreshUsersToAssignListView();
               RefreshUsersForRoleComboBox();
               RefreshRolesForUserComboBox();
               RefreshRolePageButtons();
            }
         }
      }

      void OnRemoveUsersFromAllRoles(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to remove the user " + UserToAssign + " from all its roles?","Credentials Manager",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
         if(result == DialogResult.OK)
         {
            using(RoleManagerProxy roleManager = new RoleManagerProxy(ServiceAddress))
            {
               string[] roles = roleManager.GetRolesForUser(ApplicationName,UserToAssign);
               if(roles.Length > 0)
               {
                  roleManager.RemoveUserFromRoles(ApplicationName,UserToAssign,roles);
                  RefreshRolesForUserComboBox();
                  RefreshUsersForRoleComboBox();
                  RefreshRolePageButtons();
               }
            }
         }
      }

      void OnDeleteAllApplications(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to delete all applications?","Credentials Manager",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);

         if(result == DialogResult.OK)
         {
            using(ApplicationManagerProxy applicationManager = new ApplicationManagerProxy(ServiceAddress))
            {
               applicationManager.DeleteAllApplications();
               m_ApplicationListView.ClearItems();
            }
            SelectedApplicationChanged();
         }
         RefreshApplicationButtons();
      }

      void OnDeleteAllRoles(object sender,EventArgs e)
      {
         DialogResult result = MessageBox.Show("Are you sure you want to delete all roles from the application?","Credentials Manager",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning);
         if(result == DialogResult.OK)
         {
            using(RoleManagerProxy roleManager = new RoleManagerProxy(ServiceAddress))
            {
               roleManager.DeleteAllRoles(ApplicationName,m_ThrowIfPopulatedCheckBox.Checked);
            }
            m_RolesListView.ClearItems();
            RefreshUsersForRoleComboBox();
            RefreshRolesForUserComboBox();
            RefreshRolePageButtons();
         }
      }

      void OnClosed(object sender,FormClosedEventArgs e)
      {
         m_WebBrowser.Dispose();
         m_DownloadCompletedEvent.Close();
      }

      void OnAuthenticate(object sender,EventArgs e)
      {
         LoginDialog loginDialog = new LoginDialog(ServiceAddress,ApplicationName,UserName);
         loginDialog.ShowDialog();
      }
      void OnAuthorize(object sender,EventArgs e)
      {
         AuthorizationDialog authorizationDialog = new AuthorizationDialog(ServiceAddress,ApplicationName,UserName);
         authorizationDialog.ShowDialog();
      }
      void OnSelectedApplicationChanged(object sender,ListViewItemSelectionChangedEventArgs e)
      {
         if(e.IsSelected)
         {
            SelectedApplicationChanged();
         }
      }
      void SelectedApplicationChanged()
      {
         RefreshUsersPage();
         RefreshRolesPage();
         RefreshPasswordsPage();
      }
      void OnSelectedUserAssignChanged(object sender,ListViewItemSelectionChangedEventArgs e)
      {
         RefreshRolesForUserComboBox();
         RefreshRolePageButtons();
      }
      void OnSelectedRoleChanged(object sender,ListViewItemSelectionChangedEventArgs e)
      {
         RefreshUsersForRoleComboBox();
         RefreshRolePageButtons();
      }

      void OnChangePassword(object sender,EventArgs e)
      {
         Debug.Assert(EnablePasswordRetrieval);
         ChangePasswordDialog dialog = new ChangePasswordDialog(ServiceAddress,ApplicationName,UserName);
         dialog.ShowDialog();
      }  

      void OnResetPassword(object sender,EventArgs e)
      {
         Debug.Assert(EnablePasswordReset);

         if(RequiresQuestionAndAnswer)
         {
            ResetWithQuestionDialog dialog = new ResetWithQuestionDialog(ServiceAddress,ApplicationName,UserName);
            dialog.ShowDialog();
         }
         else
         {
            using(PasswordManagerProxy passwordManager = new PasswordManagerProxy(ServiceAddress))
            {
               string newPassword = null;
               try
               {
                  newPassword = passwordManager.ResetPassword(ApplicationName,UserName);
                  Clipboard.SetText(newPassword);
               }
               catch(SoapException exception)
               {
                  if(exception.Message.Contains("The user account has been locked out"))
                  {
                     MessageBox.Show("The user account has been locked out","Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Error);
                     return;
                  }
                  throw;
               }
               MessageBox.Show("Generated password: " + newPassword + " " + Environment.NewLine + "The password is avaiable on the clipboard as well.","Credentials Manager",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
         }
      }
      void OnAbout(object sender,EventArgs e)
      {
         AboutBox aboutBox = new AboutBox();
         aboutBox.ShowDialog();
      }

      void OnLoad(object sender,EventArgs e)
      {
         Activate();
      }
      void OnSelectService(object sender,EventArgs e)
      {
         ServiceAddress = m_AddressTextbox.Text;
         RefreshApplicationsPage();
      }
      void OnDownloadCompleted(object sender,WebBrowserDocumentCompletedEventArgs e)
      {
         m_DownloadCompletedEvent.Set();
         RefreshSelectButton(m_AddressTextbox.Text);
      }

      void Content(object sender,EventArgs e)
      {
         try
         {
            Process.Start("Help.pdf");
         }
         catch
         {}
      }
   }
}