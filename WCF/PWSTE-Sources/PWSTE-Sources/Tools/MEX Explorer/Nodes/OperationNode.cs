// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace ServiceModelEx
{
   class OperationNode : MexNode 
   {
      OperationDescription m_Operation;

      public OperationNode(ExplorerForm form,OperationDescription operation,string text,int imageIndex,int selectedImageIndex) : base(form,text,imageIndex,selectedImageIndex)
      {
         m_Operation = operation;
      }
      public override void DisplayControl()
      {
         m_Form.DisplayOperationConrol(m_Operation);
      }
   }
}
