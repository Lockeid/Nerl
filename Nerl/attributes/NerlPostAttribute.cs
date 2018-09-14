using System.Net.Http;

namespace Nerl
{
  [System.AttributeUsage(System.AttributeTargets.Method)]
  public class NerlPostAttribute : INerlMapping
  {
    private string _path;

    public NerlPostAttribute(string path)
    {
      _path = path;
    }

    internal override HttpMethod GetMethod() => HttpMethod.Post;

    internal override string GetPath() => _path;
  }
}