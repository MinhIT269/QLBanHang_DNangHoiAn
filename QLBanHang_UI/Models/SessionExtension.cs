using System.Text.Json;

namespace QLBanHang_UI.Models
{
    public static class SessionExtension
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            var json = JsonSerializer.Serialize(value);
            session.SetString(key, json);
        }
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static bool IsCartExist(ISession session)
        {
            var value = session.GetString("Cart");
            return !string.IsNullOrEmpty(value);
        }
    }
}
