using Microsoft.EntityFrameworkCore;
using ShoppingApp.Data;
using ShoppingApp.Models;

namespace ShoppingApp.Services
{
    public class OrderServices : IOrderServices
    {
        private readonly ApplicationDbContext _context;

        public OrderServices(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
        }
        public Order? AddItemToOrder(ProductOrder item, Order order)
        {
            if (item != null && order != null)
            {
                var OldItem = _context.ProductOrders
                    .Include(x => x.Product)
                    .FirstOrDefault(x => (x.OrderId == order.Id) && (x.ProductId == item.ProductId));

                if (OldItem is null)//check if product not already in the cart (new item)
                {
                    order.Product_Orders.Add(item);
                    order.NumberOfItems++;
                }
                else // (old item)
                {
                    OldItem.Quantity += item.Quantity;
                }
                var product = _context.Products.Find(item.ProductId);
                product.Quantity -= item.Quantity;
                order.TotalPrice += item.Quantity * product.Price;


                _context.SaveChanges();

            }
            return order;
        }

        public bool CheckOut(Order order)
        {
            if(order == null)
            {
                return false;
            }
            var OrderFound = _context.Orders.Find(order.Id);
            if (OrderFound != null)
            {
                OrderFound.Current = false;
                var historyPerUser = _context.OrdersHistories
                     .Include(h => h.Orders_History)
                     .FirstOrDefault(h => h.UserId.Equals(OrderFound.UserId));

                if (historyPerUser == null)  //first order added to history
                {
                    historyPerUser = new OrdersHistory()
                    {
                        UserId = OrderFound.UserId,
                    };
                    _context.OrdersHistories.Add(historyPerUser);//history for one user
                }
                historyPerUser.Orders_History.Add(order);  // list of orders per user
                _context.SaveChanges();
                return true;
            }
            return false;
                

        }

        public Order CreateOrder(string userId)
        {
            Order order = new Order()
            {
                UserId = userId,
                Current = true

            };
            _context.Orders.Add(order);
            _context.SaveChanges();

            return order;
        }

        public bool DeleteItem(int productId, Order order)
        {
            if ( order != null && productId !=0)
            {
                var OldItem = _context.ProductOrders
                    .FirstOrDefault( x=> (x.OrderId == order.Id) && (x.ProductId == productId));
                order.NumberOfItems--;
                var product = _context.Products.Find(OldItem.ProductId);
                product.Quantity += OldItem.Quantity;
                order.TotalPrice -= OldItem.Quantity * product.Price;
                order.Product_Orders.Remove(OldItem);

                _context.SaveChanges();
            }
            return false;
        }

        public IEnumerable<ProductOrder>? GetCurrentOrderItems(string userId)
        {
            var order = _context.Orders
                .Include(p => p.Product_Orders)
           .FirstOrDefault(p => (p.UserId.Equals(userId)) && (p.Current==true));
           
            if (order == null || order.Product_Orders.Count()==0)
            {
                return null;
            }
            var productOrders = _context.ProductOrders
                .Include(po => po.Product)
                .Where(po => po.OrderId == order.Id)
                .ToList();//////


            return productOrders;
        }

        public Order? GetCurrentOrder(string userId)
        {
            Order? order = _context.Orders
           .FirstOrDefault(o => (o.UserId.Equals(userId)) && o.Current);
            if (order == null) return null;
           
            return order;

        }

        public IEnumerable<Order>? GetHistoryPerUser(string userId)
        {
            var UserHistory = _context.OrdersHistories
                   .Include(h => h.Orders_History)
                   .FirstOrDefault(h => h.UserId.Equals(userId));
            if (UserHistory == null) 
                return null;
            else
                return UserHistory.Orders_History.ToList();
        }



    }
}
