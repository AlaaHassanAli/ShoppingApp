using ShoppingApp.Models;
using ShoppingApp.ViewModels;

namespace ShoppingApp.Services
{
    public interface IOrderServices
    {
        public Order? AddItemToOrder(ProductOrder item,Order order); 
        IEnumerable<ProductOrder>? GetCurrentOrderItems(string userId) ;
        Order? GetCurrentOrder(string userId); 
        Order CreateOrder(string userId); 

        bool DeleteItem(int productId, Order order);

        bool CheckOut(Order order);
        public IEnumerable<Order>? GetHistoryPerUser(string userId);

    }
}
