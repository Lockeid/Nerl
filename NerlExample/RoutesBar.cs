using System;
using System.Net;
using System.Net.Http;
using Nerl;

namespace NerlExample
{
  [NerlRouterAttribute("bar")]
  public static class RoutesBar
  {
    [NerlPost("/test")]
    public static void testRoutePost(HttpListenerContext context) {
      Console.WriteLine("Post on /bar/test");
    }

    [NerlDelete("/bar")]
    public static void barRouteDelete(HttpListenerContext context) {
      Console.WriteLine("Delete on /bar/bar");
    }
  }
}