// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace ServiceModelEx
{
   class ContractNode : MexNode 
   {
      ContractDescription m_Contract;

      public ContractNode(ExplorerForm form,ContractDescription contract,string text,int imageIndex,int selectedImageIndex) : base(form,text,imageIndex,selectedImageIndex)
      {
         m_Contract = contract;
      }
      public override void DisplayControl()
      {
         m_Form.DisplayContractConrol(m_Contract);
      }
   }
}
