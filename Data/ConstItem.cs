using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class ConstItem
  {
    public readonly int Id;
    public readonly string Name;
    public readonly int Price;
    public readonly int Value1;
    public readonly int Value2;
    public readonly string Description;

    public ConstItem(string text, int id)
    {
      string[] items = text.Split("\t", StringSplitOptions.RemoveEmptyEntries);
      JsTrans.Assert(items.Length == 5, "text of 'ConstItem(string text)' split tab is not 20 items.");
      this.Name = items[0];
      this.Price = int.Parse(items[1]);
      this.Value1 = int.Parse(items[2]);
      this.Value2 = int.Parse(items[3]);
      this.Description = items[4];

      this.Id = id;
    }

    //△△△表示用配列を作成△△△
    class AbcComparer : IComparer
    {
      public int Compare(object x, object y)
      {
        ConstItem str1 = (ConstItem)x;
        ConstItem str2 = (ConstItem)y;

        return str1.Name.CompareTo(str2.Name);
      }
    }
    public static ConstItem[] ArrayForView(bool isAbcSort)
    {
      Bag bag = DataSC.Bag();
      ConstItem[] items = DataSC.Items();
      int num = 0;
      for (int i = 0; i < items.Length; i++)
      {
        if (bag.item(i) > 0) num++;
      }
      ConstItem[] filtered = new ConstItem[num];
      int j = 0;
      for (int i = 0; i < items.Length; i++)
      {
        if (bag.item(i) > 0)
        {
          filtered[j++] = items[i];
        }
      }

      if (isAbcSort)
      {
        IComparer abcComp = new AbcComparer();
        Array.Sort(filtered, abcComp);
      }
      return filtered;
    }
    //▽▽▽表示用配列を作成▽▽▽
  }
}
