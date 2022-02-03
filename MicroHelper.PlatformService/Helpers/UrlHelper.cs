using System.Linq;

namespace MicroHelper.PlatformService.Helpers
{
    public class UrlHelper
    {
        public static string Combine(params string[] items)
        {
            if (items?.Any() != true)
            {
                return string.Empty;
            }

            return string.Join("/", items.Where(u => !string.IsNullOrWhiteSpace(u)).Select(u => u.Trim('/', '\\')));
        }
    }
}
