using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class Chars
  {
    // データ要素と読み込みアクセサ
    protected string name_;
    public string name() { return name_; }
    static public string[] names()
    {
      string[] names = new string[s_num];
      for (int i = 0; i < names.Length; i++)
        names[i] = s_instances[i].name_;
      return names;
    }
    protected int hp_;
    public int hp() { return hp_; }
    public string hpStr() { return hp_.ToString(); }

    protected Chars() { }
    static protected Chars[] s_instances;
    static protected int s_num = 0;
    static public int Num { get { return s_num; } }
    static public Chars[] Instances { get { return s_instances; } }
  }
}
