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
    public string[] Hp_SpdStrings()
    {
      string[] ss= new string[7];
      ss[0] = hpStr();
      ss[1] = mp().ToString();
      ss[2] = atk().ToString();
      ss[3] = def().ToString();
      ss[4] = matk().ToString();
      ss[5] = mdef().ToString();
      ss[6] = spd().ToString();
      return ss;
    }
    public string[] Hp_SpdBareStrings()
    {
      string[] ss = new string[7];
      int l = level_ + 2;
      ss[0] = (l * this.unique.hp / 101).ToString();
      ss[1] = (l * this.unique.mp / 101).ToString();
      ss[2] = (l * this.unique.atk / 101).ToString();
      ss[3] = (l * this.unique.def / 101).ToString();
      ss[4] = (l * this.unique.matk / 101).ToString();
      ss[5] = (l * this.unique.mdef / 101).ToString();
      ss[6] = (l * this.unique.spd / 101).ToString();
      return ss;
    }
    public string[] Fire_DarkStrings()
    {
      string[] ss = new string[6];
      ss[0] = fire().ToString();
      ss[1] = water().ToString();
      ss[2] = wind().ToString();
      ss[3] = earth().ToString();
      ss[4] = light().ToString();
      ss[5] = dark().ToString();
      return ss;
    }
    public string[] Fire_DarkBareStrings()
    {
      string[] ss = new string[6];
      int l = level_ + 2;
      ss[0] = (l * this.unique.fire / 101).ToString();
      ss[1] = (l * this.unique.water / 101).ToString();
      ss[2] = (l * this.unique.wind / 101).ToString();
      ss[3] = (l * this.unique.earth / 101).ToString();
      ss[4] = (l * this.unique.light / 101).ToString();
      ss[5] = (l * this.unique.dark / 101).ToString();
      return ss;
    }

    // ▲▲▲装備▲▲▲
    // キャラクター固有ステータスは値で保持。
    protected ConstStatus unique;

    // 装備はインデックスで保持。
    // file:///D:/dospara/%E8%87%AA%E5%AE%85%E3%82%B5%E3%83%BC%E3%83%90/html/d7/menudata/st1equip.js
    protected int idWeapon = -1;
    protected int idBody = -1;
    protected int idHead = -1;
    protected int idArm = -1;
    protected int idExterior = -1;
    protected int idAccessory = -1;
    public string[] EquipStrings()
    {
      string[] ss = { "-", "-", "-", "-", "-", "-" };
      Loader l = Loader.Instance;

      if (idWeapon != -1) ss[0] = l.weapons[idWeapon].name;
      if (idBody != -1) ss[1] = l.body[idBody].name;
      if (idHead != -1) ss[2] = l.head[idHead].name;
      if (idArm != -1) ss[3] = l.arm[idArm].name;
      if (idExterior != -1) ss[4] = l.exterior[idExterior].name;
      if (idAccessory != -1) ss[5] = l.accessory[idAccessory].name;

      return ss;
    }
    // ▼▼▼装備▼▼▼

    // ▲▲▲基底クラス シングルトンパターン▲▲▲
    protected Status() { }
    static protected Status[] s_instances;
    static protected int s_num = 0;
    static public int Num { get { return s_num; } }
    static public Status[] Instances { get { return s_instances; } }
    // ▼▼▼基底クラス シングルトンパターン▼▼▼
  }
}
