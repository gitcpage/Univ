using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class ConstStatus
  {
    public  string name;
    public readonly int hp;
    public readonly int mp;
    public readonly int atk;
    public readonly int def;
    public readonly int matk;
    public readonly int mdef;
    public readonly int spd;
    public readonly int fire;
    public readonly int water;
    public readonly int wind;
    public readonly int earth;
    public readonly int light;
    public readonly int dark;
    public ConstStatus(string s)
    {
      string[] dem = new string[1];
      dem[0] = "\t";
      string[] items = s.Split(dem, StringSplitOptions.RemoveEmptyEntries);
      JsTrans.Assert(items.Length == 14, "s of 'ConstStatus(string s)' split tab is not 14 items.");
      this.name = items[0];
      this.hp = int.Parse(items[1]);
      this.mp = int.Parse(items[2]);
      this.atk = int.Parse(items[3]);
      this.def = int.Parse(items[4]);
      this.matk = int.Parse(items[5]);
      this.mdef = int.Parse(items[6]);
      this.spd = int.Parse(items[7]);
      this.fire = int.Parse(items[8]);
      this.water = int.Parse(items[9]);
      this.wind = int.Parse(items[10]);
      this.earth = int.Parse(items[11]);
      this.light = int.Parse(items[12]);
      this.dark = int.Parse(items[13]);
    }
    public ConstStatus(string name, int hp, int mp, int atk, int def, int matk, int mdef, int spd,
      int fire = 0, int water = 0, int wind = 0, int earth = 0, int light = 0, int dark = 0)
    {
      this.name = name;
      this.hp = hp;
      this.mp = mp;
      this.atk = atk;
      this.def = def;
      this.matk = matk;
      this.mdef = mdef;
      this.spd = spd;
      this.fire = fire;
      this.water = water;
      this.wind = wind;
      this.earth = earth;
      this.light = light;
      this.dark = dark;
    }
  }
}
