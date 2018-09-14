namespace Nerl
{
  [System.AttributeUsage(System.AttributeTargets.Class)]
  public class NerlRouterAttribute : System.Attribute
  {
    public string Prefix;

    public NerlRouterAttribute(string prefix)
    {
      Prefix = prefix;
    }
  }
}