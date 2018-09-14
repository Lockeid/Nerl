using System.Net.Http;

namespace Nerl
{
  [System.AttributeUsage(System.AttributeTargets.Method)]
  public class NerlDeleteAttribute : INerlMapping
  {
    private string _path;

    public NerlDeleteAttribute(string path)
    {
      _path = path;
    }

    internal override HttpMethod GetMethod() => HttpMethod.Delete;

    internal override string GetPath() => _path;
  }
}