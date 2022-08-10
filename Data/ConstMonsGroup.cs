using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class ConstMonsArrangement
  {
    public readonly int id;
    public readonly int x;
    public readonly int y;
    public ConstMonsArrangement(int id, int x, int y)
    {
      this.id = id;
      this.x = x;
      this.y = y;
    }
    public string path { get{ return "battle/mon" + (id+1) + ".png"; } }
  }
  internal class ConstMonsGroups
  {
    public readonly ConstMonsArrangement[] group;
    public ConstMonsGroups(string text)
    {
      string[] items = text.Split("\t", StringSplitOptions.RemoveEmptyEntries);
      JsTrans.Assert(items.Length % 3 == 0, "ConstMonsGroup items.Length % 3 == 0");
      int num = items.Length / 3;
      group = new ConstMonsArrangement[num];
      for (int i = 0; i < num; i++)
      {
        int id = int.Parse(items[i * 3]);
        int x = int.Parse(items[i * 3 + 1]);
        int y = int.Parse(items[i * 3 + 2]);
        group[i] = new ConstMonsArrangement(id, x, y);
      }
    }
  }
}
