// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System.Windows.Forms;
using System.ServiceModel.Description;

namespace ServiceModelEx.ServiceBus
{
   class EndpointTreeNode : ServiceBusTreeNode 
   {
      public EndpointTreeNode(ExplorerForm form,ServiceBusNode serviceBusNode,string name) : base(form,serviceBusNode,name,ExplorerForm.EndpointIndex)
      {}

      public override void DisplayControl()
      {
         Form.DisplayEndpointControl(ServiceBusNode,Credential);
      }
   }
}
