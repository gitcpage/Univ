using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class SeveralData
  {
    // データ要素。
    protected string name_;
    // 読み込みアクセサ。
    public string name() { return name_; }
    // 全要素配列にする。
    static public string[] names()
    {
      string[] names = new string[s_num];
      for (int i = 0; i < names.Length; i++)
        names[i] = s_instances[i].name_;
      return names;
    }

    protected SeveralData() { }
    static protected SeveralData[] s_instances;
    static protected int s_num = 0;
    static public int Num { get { return s_num; } }
    static public SeveralData[] Instances { get { return s_instances; } }
  }
}
