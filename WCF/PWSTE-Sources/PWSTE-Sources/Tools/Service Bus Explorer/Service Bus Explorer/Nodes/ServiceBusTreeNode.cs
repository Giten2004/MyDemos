// 2011 IDesign Inc.
//Questions? Comments? go to 
//http://www.idesign.net


using System;
using System.Windows.Forms;
using Microsoft.ServiceBus;

namespace ServiceModelEx.ServiceBus
{
   abstract class ServiceBusTreeNode : TreeNode
   {
      protected readonly ExplorerForm Form;
      protected readonly TransportClientEndpointBehavior Credential;

      public readonly ServiceBusNode ServiceBusNode;
     
      public ServiceBusTreeNode(ExplorerForm form,ServiceBusNode serviceBusNode,string text,int imageIndex) : base(text,imageIndex,imageIndex)
      {
         Form = form;
         ServiceBusNode = serviceBusNode;

         if(serviceBusNode != null)
         {
            string serviceNamespace = ExtractNamespace(new Uri(serviceBusNode.Address));
            Credential = form.Graphs[serviceNamespace.ToLower()].Credential;
         }
      }
      abstract public void DisplayControl();

      
      public static string ExtractNamespace(Uri address)
      {
         return address.Host.Split('.')[0];
      }
   }
}