using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity.Utility;
using VM;
using System.Data.Entity;

namespace VM.Repositories
{
    public class OrderRepository : VmRepository<Order>, IOrderRepository
    {
        public OrderRepository(VmDbContext dbContext)
            : base(dbContext)
        { }

        public void AddOrder(Order order)
        {
            Guard.ArgumentNotNull(order, "order");
            this.DbContext.Orders.Attach(order);
            this.DbContext.Entry(order).State = EntityState.Added;

            foreach (OrderLine orderLine in order.OrderLines)
            {
                this.DbContext.Entry(orderLine).State = EntityState.Added;
            }
            this.DbContext.SaveChanges();
        }
    }
}