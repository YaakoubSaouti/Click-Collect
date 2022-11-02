using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace ClickandCollect.Models
{
    public static class OrderExtensions
    {
        public static void SetOrder(this ISession session, string key, Order order)
        {
            session.SetString(key, JsonSerializer.Serialize(order));
        }

        public static Order GetOrder(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<Order>(value);
        }
    }
}
