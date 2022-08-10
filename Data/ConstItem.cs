using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class ConstItem
  {
    public readonly string Name;
    public readonly int Price;
    public readonly int Value1;
    public readonly int Value2;
    public readonly string Description;

    public ConstItem(string text)
    {
      string[] items = text.Split("\t", StringSplitOptions.RemoveEmptyEntries);
      JsTrans.Assert(items.Length == 5, "text of 'ConstItem(string text)' split tab is not 20 items.");
      this.Name = items[0];
      this.Price = int.Parse(items[1]);
      this.Value1 = int.Parse(items[2]);
      this.Value2 = int.Parse(items[3]);
      this.Description = items[4];
    }
  }
}
