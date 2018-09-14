using System;
using System.Net;
using System.Net.Http;
using Nerl;

namespace NerlExample
{
  [NerlRouterAttribute("foo")]
  public static class RoutesFoo
  {
    [NerlGet("/test")]
    public static void testRouteGet(HttpListenerContext context) {
      Console.WriteLine("Route /foo/test");
    }

    [NerlGet("/bar")]
    public static void barRouteGet(HttpListenerContext context) {
      Console.WriteLine("Route /foo/bar");
    }
  }
}