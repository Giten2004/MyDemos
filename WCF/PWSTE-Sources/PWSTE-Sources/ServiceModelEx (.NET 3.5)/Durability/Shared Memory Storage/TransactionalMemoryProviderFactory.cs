// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net


using System;
using System.ServiceModel.Persistence;

namespace ServiceModelEx
{
   public class TransactionalMemoryProviderFactory : MemoryProviderFactory
   {
      public override PersistenceProvider CreateProvider(Guid id)
      {
         return new TransactionalMemoryProvider(id);
      }
   }
}