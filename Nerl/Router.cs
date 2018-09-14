using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Nerl
{
  internal class Router
  {
    private readonly Dictionary<string, List<Tuple<HttpMethod, string, MethodInfo>>> _routing;
    public delegate void RouterDelegate(HttpListenerContext context);
    public Router(Assembly callingAssembly)
    {
      // Static classes are both abstract and sealed
      _routing = callingAssembly.GetTypes()
        .Where(t => t.IsClass && t.IsAbstract && t.IsSealed && t.GetCustomAttribute(typeof(NerlRouterAttribute)) != null)
        .ToDictionary(GetTypePrefix, GetTypeMappings);
    }

    private string GetTypePrefix(Type nerlRouterType) {
      NerlRouterAttribute attribute = nerlRouterType.GetCustomAttribute(typeof(NerlRouterAttribute)) as NerlRouterAttribute;
      return attribute.Prefix;
    }
    private List<Tuple<HttpMethod, string, MethodInfo>> GetTypeMappings(Type nerlRouterType)
    {
      return nerlRouterType.GetMethods()
        .Where(m => m.IsStatic && m.GetCustomAttribute(typeof(INerlMapping)) != null)
        .Select(m => {
          INerlMapping mappingAttribute = m.GetCustomAttribute(typeof(INerlMapping)) as INerlMapping;
          return new Tuple<HttpMethod, string, MethodInfo>(mappingAttribute.GetMethod(), mappingAttribute.GetPath(), m);
        })
        .ToList();
    }

    internal Task Handle(HttpListenerContext context)
    {
      return Task.Factory.StartNew(() => {
        string controllerPrefix = context.Request.Url.Segments[1].Replace("/", "");
        string path = "/" + string.Join("/", context.Request.Url.Segments.Skip(2).Select(s => s.Replace("/", "")).ToList());
        List<Tuple<HttpMethod, string, MethodInfo>> possibleRoutes;
        if (_routing.TryGetValue(controllerPrefix, out possibleRoutes)) {
          MethodInfo routeMethod = possibleRoutes.Find(t =>
            t.Item1.ToString().ToLower() == context.Request.HttpMethod.ToLower() &&
            t.Item2 == path).Item3;
          Delegate routeDelegate = Delegate.CreateDelegate(typeof(RouterDelegate), routeMethod, false);
          if (routeDelegate != null) {
            routeDelegate.DynamicInvoke(context);
          }
        }
      });
    }
  }
}