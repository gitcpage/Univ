using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Univ.Data
{
  internal class SecurityToken
  {
    string name_;
    int random_;
    static Dictionary<string, int> s_map = new Dictionary<string, int>();
    public SecurityToken(string name)
    {
      name_ = name;
      Random random = new Random();
      random_ = random.Next(int.MaxValue);
      s_map.Add(name, random_);
    }
    public bool Authorize(SecurityToken securityToken)
    {
      if (securityToken.name_ == name_ &&
        securityToken.random_ == random_)
      {
        return true;
      }
      return false;
    }
  }
}
