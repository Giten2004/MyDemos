using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VM;

namespace VM.Repositories
{
public interface IOrderRepository
{
    void AddOrder(Order order);
}
}