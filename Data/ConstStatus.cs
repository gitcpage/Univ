using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  // ConstStatus には名前に「Const」とついているが、
  // 継承することによりフィールドを変更することができる。
  // Statusクラスに継承し、コードの重複を減らす。
  internal class ConstStatus
  {
    protected string name_;
    protected int hp_;
    protected int mp_;
    protected int atk_;
    protected int def_;
    protected int matk_;
    protected int mdef_;
    protected int spd_;
    protected int fire_;
    protected int water_;
    protected int wind_;
    protected int earth_;
    protected int light_;
    protected int dark_;

    public string name() { return name_; }
    public int hp() { return hp_; }
    public int mp() { return mp_; }
    public int atk() { return atk_; }
    public int def() { return def_; }
    public int matk() { return matk_; }
    public int mdef() { return mdef_; }
    public int spd() { return spd_; }
    public int fire() { return fire_; }
    public int water() { return water_; }
    public int wind() { return wind_; }
    public int earth() { return earth_; }
    public int light() { return light_; }
    public int dark() { return dark_; }


    public ConstStatus(string text)
    {
      string[] items = text.Split("\t", StringSplitOptions.RemoveEmptyEntries);
      JsTrans.Assert(items.Length == 14, "s of 'ConstStatus(string s)' split tab is not 14 items.");
      this.name_ = items[0];
      this.hp_ = int.Parse(items[1]);
      this.mp_ = int.Parse(items[2]);
      this.atk_ = int.Parse(items[3]);
      this.def_ = int.Parse(items[4]);
      this.matk_ = int.Parse(items[5]);
      this.mdef_ = int.Parse(items[6]);
      this.spd_ = int.Parse(items[7]);
      this.fire_ = int.Parse(items[8]);
      this.water_ = int.Parse(items[9]);
      this.wind_ = int.Parse(items[10]);
      this.earth_ = int.Parse(items[11]);
      this.light_ = int.Parse(items[12]);
      this.dark_ = int.Parse(items[13]);
    }
    // ConstStatusMonsクラスの継承元として使われたときのコンストラクタ。
    protected ConstStatus(string text, int protectedUse)
    {
      string[] items = text.Split("\t", StringSplitOptions.RemoveEmptyEntries);
      JsTrans.Assert(items.Length >= 14, "s of 'ConstStatus(string s)' split tab is less 14 items.");
      this.name_ = items[0];
      this.hp_ = int.Parse(items[1]);
      this.mp_ = int.Parse(items[2]);
      this.atk_ = int.Parse(items[3]);
      this.def_ = int.Parse(items[4]);
      this.matk_ = int.Parse(items[5]);
      this.mdef_ = int.Parse(items[6]);
      this.spd_ = int.Parse(items[7]);
      this.fire_ = int.Parse(items[8]);
      this.water_ = int.Parse(items[9]);
      this.wind_ = int.Parse(items[10]);
      this.earth_ = int.Parse(items[11]);
      this.light_ = int.Parse(items[12]);
      this.dark_ = int.Parse(items[13]);
    }
    // Statusクラスの継承元として使われたときのコンストラクタ。
    protected ConstStatus() { }
  }
}
