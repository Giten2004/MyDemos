// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System.Windows.Forms;
using System.ServiceModel.Description;
using ServiceModelEx.ServiceBus;

namespace ServiceModelEx
{
   class RouterTreeNode : ServiceBusTreeNode 
   {
      public RouterTreeNode(ExplorerForm form,ServiceBusNode serviceBusNode,string name) : base(form,serviceBusNode,name,ExplorerForm.RouterIndex)
      {}

      public override void DisplayControl()
      {
         Form.DisplayRouterControl(ServiceBusNode,Credential);
      }
   }
}
