using System.Collections.Generic;
using System.Web;

namespace PictlHelpers
{
    public static class HttpBodyParser
    {
        public static Dictionary<string, string> GetParameters(this string rawInfo, params string[] parameterNames)
        {
            var parameters = new Dictionary<string, string>();

            foreach (var parameterName in parameterNames)
            {
                var parameterValue = HttpUtility.ParseQueryString(rawInfo).Get(parameterName);
                parameters.Add(parameterName, parameterValue);
            }

            return parameters;
        }
    }
}
