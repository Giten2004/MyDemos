using VM.Models;
using VM.Repositories;
using Microsoft.Practices.Unity.Utility;
using System;
using VM;
using VM.Mvc.Extensions;

namespace VM.Services
{
    public class OrderService : ServiceBase, IOrderService, IDisposable
    {
        //Repository和ProductService都采用构造器注入的方式进行初始化
        public IOrderRepository OrderRepository { get; private set; }
        public IProductService ProductService { get; private set; }
        public OrderService(IOrderRepository orderRepository, IProductService productService)
        {
            this.OrderRepository = orderRepository;
            this.ProductService = productService;

            this.AddDisposableObject(orderRepository);
            this.AddDisposableObject(productService);
        }

        [TransactionCallHandler]
        public void SubmitOrder(Order order)
        {
            Guard.ArgumentNotNull(order, "order");
            CheckStock(order);
            this.OrderRepository.AddOrder(order);
        }
        private void CheckStock(Order order)
        {
            foreach (var line in order.OrderLines)
            {
                if (this.ProductService.GetStock(line.ProductId) < line.Quantity)
                {
                    throw new OutOfStockException("Out of stock...");
                }
            }
        }
    }
}