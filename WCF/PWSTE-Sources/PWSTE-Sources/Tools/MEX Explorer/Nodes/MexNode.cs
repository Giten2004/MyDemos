// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System.Windows.Forms;
using System.ServiceModel.Description;

namespace ServiceModelEx
{
   abstract class MexNode : TreeNode
   {
      protected ExplorerForm m_Form;

      public MexNode(ExplorerForm form,string text,int imageIndex,int selectedImageIndex) : base(text,imageIndex,selectedImageIndex)
      {
         m_Form = form;
      }
      abstract public void DisplayControl();
   }
}
