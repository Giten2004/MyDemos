using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using System.Transactions;

namespace VM.Mvc.Extensions
{
    public class TransactionCallHandler: ICallHandler
    {
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    IMethodReturn methodReturn = getNext()(input, getNext);
                    transactionScope.Complete();
                    return methodReturn;
                }
            }
            catch (Exception ex)
            {
                return input.CreateExceptionMethodReturn(ex);
            }
        }

        public int Order { get; set; }
    }
}
