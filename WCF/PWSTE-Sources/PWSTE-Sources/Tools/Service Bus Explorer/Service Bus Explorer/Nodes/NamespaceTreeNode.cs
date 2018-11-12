// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System.Windows.Forms;
using System.ServiceModel.Description;
using System.Diagnostics;

namespace ServiceModelEx.ServiceBus
{
   class NamespaceTreeNode : ServiceBusTreeNode 
   {
      public NamespaceTreeNode(ExplorerForm form) : base(form,null,"Unspecified Namespace",ExplorerForm.UnspecifiedNamespaceIndex)
      {} 
      public NamespaceTreeNode(ExplorerForm form,string serviceNamespace) : this(form,serviceNamespace,ExplorerForm.NamespaceIndex)
      {}
      public NamespaceTreeNode(ExplorerForm form,string serviceNamespace,int imageIndex) : base(form,null,serviceNamespace,imageIndex)
      {}

      public override void DisplayControl()
      {
         if(Text == "Unspecified Namespace")
         {
            Form.DisplayBlankControl();
            Form.SelectNamespaceTextBox();
         }
         Form.DisplayNamespaceControl(Text);
      }
   }
}
