// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel;

namespace ServiceModelEx
{
   class AddressNode : MexNode 
   {
      EndpointAddress m_Address;

      public AddressNode(ExplorerForm form,EndpointAddress address,string text,int imageIndex,int selectedImageIndex) : base(form,text,imageIndex,selectedImageIndex)
      {
         m_Address = address;
      }
      public override void DisplayControl()
      {
         m_Form.DisplayAddressConrol(m_Address);
      }
   }
}
