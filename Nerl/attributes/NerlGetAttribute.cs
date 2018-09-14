using System.Net.Http;

namespace Nerl
{
  [System.AttributeUsage(System.AttributeTargets.Method)]
  public class NerlGetAttribute : INerlMapping
  {
    private string _path;

    public NerlGetAttribute(string path)
    {
      _path = path;
    }

    internal override HttpMethod GetMethod() => HttpMethod.Get;

    internal override string GetPath() => _path;
  }
}