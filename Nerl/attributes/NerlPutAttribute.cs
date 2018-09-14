using System.Net.Http;

namespace Nerl
{
  [System.AttributeUsage(System.AttributeTargets.Method)]
  public class NerlPutAttribute : INerlMapping
  {
    private string _path;

    public NerlPutAttribute(string path)
    {
      _path = path;
    }

    internal override HttpMethod GetMethod() => HttpMethod.Put;

    internal override string GetPath() => _path;
  }
}