// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System.Windows.Forms;
using System.ServiceModel.Description;

namespace ServiceModelEx
{
   class ServiceNode : MexNode 
   {
      public string MexAddress
      {get;private set;}

      public ServiceNode(string mexAddress,ExplorerForm form,string text,int imageIndex,int selectedImageIndex) : base(form,text,imageIndex,selectedImageIndex)
      {
         MexAddress = mexAddress;
      }
      public override void DisplayControl()
      {
         m_Form.CurrentNode = this;
         m_Form.SetMexAddress(MexAddress);
         m_Form.DisplayServiceControl();
      }
   }
}
