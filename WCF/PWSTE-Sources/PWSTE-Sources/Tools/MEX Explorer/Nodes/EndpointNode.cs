// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System.Windows.Forms;
using System.ServiceModel.Description;

namespace ServiceModelEx
{
   class EndpointNode : MexNode 
   {
      ServiceEndpoint m_Endpoint;

      public EndpointNode(ExplorerForm form,ServiceEndpoint endpoint,string text,int imageIndex,int selectedImageIndex) : base(form,text,imageIndex,selectedImageIndex)
      {
         m_Endpoint = endpoint;
      }
      public override void DisplayControl()
      {
         m_Form.DisplayEndpointConrol(m_Endpoint);
      }
   }
}
