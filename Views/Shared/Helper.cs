using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CoreEmptyProject1.Views.Shared
{
    public class Helper
    {
        private static readonly JsonSerializerOptions options = new() { WriteIndented = true };

        // Browser ma dekhadva (dd jevu)
        public static IActionResult DD(Controller c, object data)
        {
            return c.Content(JsonSerializer.Serialize(data, options), "application/json");
        }

        // Console / Output ma print karva (execution chalu rehse)
        public static void Dump(object data)
        {
            var json = JsonSerializer.Serialize(data, options);
            Console.WriteLine(json);
            System.Diagnostics.Debug.WriteLine(json);
        }
    }
}
