// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System.Windows.Forms;
using System.ServiceModel.Description;

namespace ServiceModelEx.ServiceBus
{
   class BufferTreeNode : ServiceBusTreeNode 
   {
      public BufferTreeNode(ExplorerForm form,ServiceBusNode serviceBusNode,string name) : base(form,serviceBusNode,name,ExplorerForm.BufferIndex)
      {}

      public override void DisplayControl()
      {
         Form.DisplayBufferControl(ServiceBusNode,Credential);
      }
   }
}
