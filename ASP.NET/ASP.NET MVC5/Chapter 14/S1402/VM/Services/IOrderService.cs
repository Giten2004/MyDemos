using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VM.Models;
using VM;

namespace VM.Services
{
public interface IOrderService
{
    void SubmitOrder(Order order);
}
}
