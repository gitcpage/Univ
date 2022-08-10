using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class ConstStatusMons : ConstStatus
  {
    public readonly int Lv;
    public readonly int Exp;
    public readonly int Gold;
    public readonly int Item;
    public readonly int ItemPer;
    public readonly int Width;

    public ConstStatusMons(string text) : base(text, 0)
    {
      string[] items = text.Split("\t", StringSplitOptions.RemoveEmptyEntries);
      JsTrans.Assert(items.Length == 20, "text of 'ConstStatus(string text)' split tab is not 20 items.");
      this.Lv = int.Parse(items[14]);
      this.Exp = int.Parse(items[15]);
      this.Gold = int.Parse(items[16]);
      this.Item = int.Parse(items[17]);
      this.ItemPer = int.Parse(items[18]);
      this.Width = int.Parse(items[19]);
    }
  }
}
