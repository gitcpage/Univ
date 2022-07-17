using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class Status
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
    static public string[] hpStrings()
    {
      string[] hps = new string[s_num];
      for (int i = 0; i < hps.Length; i++)
        hps[i] = s_instances[i].hp_.ToString();
      return hps;
    }

    protected int mp_;
    public int mp() { return mp_; }
    protected int atk_;
    public int atk() { return atk_; }
    protected int def_;
    public int def() { return def_; }
    protected int matk_;
    public int matk() { return matk_; }
    protected int mdef_;
    public int mdef() { return mdef_; }
    protected int spd_;
    public int spd() { return spd_; }
    protected int fire_;
    public int fire() { return fire_; }
    protected int water_;
    public int water() { return water_; }
    protected int wind_;
    public int wind() { return wind_; }
    protected int earth_;
    public int earth() { return earth_; }
    protected int light_;
    public int light() { return light_; }
    protected int dark_;
    public int dark() { return dark_; }

    protected int level_;
    public int level() { return level_; }
    protected int experience_;
    public int experience() { return experience_; }
    public int NeedEexperienceUntilNextLevel()
    { // file:///D:/dospara/%E8%87%AA%E5%AE%85%E3%82%B5%E3%83%BC%E3%83%90/html/d7/menudata/st1bare.js
      return 5 * level_ * (5 * level_ - 2) - experience_;
    }



    protected Status() { }
    static protected Status[] s_instances;
    static protected int s_num = 0;
    static public int Num { get { return s_num; } }
    static public Status[] Instances { get { return s_instances; } }
  }
}
