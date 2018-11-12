// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System.Windows.Forms;
using System.ServiceModel.Description;

namespace ServiceModelEx.ServiceBus
{
   class RouterSubscriberTreeNode : ServiceBusTreeNode 
   {
      public RouterSubscriberTreeNode(ExplorerForm form,ServiceBusNode serviceBusNode,string name) : base(form,serviceBusNode,name,ExplorerForm.EventEndpointIndex)
      {}

      public override void DisplayControl()
      {
         Form.DisplayRouterSubscriberControl(ServiceBusNode,Credential);
      }
   }
}
