using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Test_Solution1.Common.Helper
{
    public static class ResourceHelper
    {
        public static string GetString(string key)
        {
            CultureInfo ci = new CultureInfo("en-GB");
            ResourceManager resourceManager = new ResourceManager("Web.Resources.Message", Assembly.GetExecutingAssembly());
            return resourceManager.GetString(key, ci);
        }
    }
}
