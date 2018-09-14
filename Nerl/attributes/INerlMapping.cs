using System.Net.Http;

namespace Nerl
{
  [System.AttributeUsage(System.AttributeTargets.Method)]
  public abstract class INerlMapping : System.Attribute
  {
    internal abstract string GetPath();
    internal abstract HttpMethod GetMethod();
  }
}