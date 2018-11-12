// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace ServiceModelEx
{
   class BindingNode : MexNode 
   {
      Binding m_Binding;

      public BindingNode(ExplorerForm form,Binding binding,string text,int imageIndex,int selectedImageIndex) : base(form,text,imageIndex,selectedImageIndex)
      {
         m_Binding = binding;
      }
      public override void DisplayControl()
      {
         m_Form.DisplayBindingConrol(m_Binding);
      }
   }
}
