
namespace Univ.Data
{
  internal class Status : ConstStatus
  {
    protected int nowHp_;
    protected int nowMp_;
    public int NowHp()
      { return nowHp_; }
    public int NowMp()
      { return nowMp_; }

    public string hpStr() { return hp_.ToString(); }

    protected int level_;
    public int level() { return level_; }
    protected int experience_;
    public int experience() { return experience_; }
    public int NeedEexperienceUntilNextLevel()
    { // file:///D:/dospara/%E8%87%AA%E5%AE%85%E3%82%B5%E3%83%BC%E3%83%90/html/d7/menudata/st1bare.js
      return 5 * level_ * (5 * level_ - 2) - experience_;
    }
    public int Value(int i)
    {
      switch(i)
      {
        case 0: return hp_;
        case 1: return mp_;
        case 2: return atk_;
        case 3: return def_;
        case 4: return matk_;
        case 5: return mdef_;
        case 6: return spd_;
        case 7: return fire_;
        case 8: return water_;
        case 9: return wind_;
        case 10: return earth_;
        case 11: return light_;
        case 12: return dark_;
        default:
          JsTrans.Assert("Status.cs Value i=" + i);
          return 0;
      }
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
    public int[] Hp_Spd(ConstStatus plus)
    {
      int[] ss = new int[7];
      ss[0] = hp() + plus.hp();
      ss[1] = mp() + plus.mp();
      ss[2] = atk() + plus.atk();
      ss[3] = def() + plus.def();
      ss[4] = matk() + plus.matk();
      ss[5] = mdef() + plus.mdef();
      ss[6] = spd() + plus.spd();
      return ss;
    }
    public int[] Hp_SpdWithMinus(ConstStatus minus)
    {
      int[] ss = new int[7];
      ss[0] = hp() - minus.hp();
      ss[1] = mp() - minus.mp();
      ss[2] = atk() - minus.atk();
      ss[3] = def() - minus.def();
      ss[4] = matk() - minus.matk();
      ss[5] = mdef() - minus.mdef();
      ss[6] = spd() - minus.spd();
      return ss;
    }
    public int[] Hp_Spd(ConstStatus plus, ConstStatus minus)
    {
      int[] ss = new int[7];
      ss[0] = hp() + plus.hp() - minus.hp();
      ss[1] = mp() + plus.mp() - minus.mp();
      ss[2] = atk() + plus.atk() - minus.atk();
      ss[3] = def() + plus.def() - minus.def();
      ss[4] = matk() + plus.matk() - minus.matk();
      ss[5] = mdef() + plus.mdef() - minus.mdef();
      ss[6] = spd() + plus.spd() - minus.spd();
      return ss;
    }
    public string[] Hp_SpdBareStrings()
    {
      string[] ss = new string[7];
      int l = level_ + 2;
      ss[0] = (l * this.unique.hp() / 101).ToString();
      ss[1] = (l * this.unique.mp() / 101).ToString();
      ss[2] = (l * this.unique.atk() / 101).ToString();
      ss[3] = (l * this.unique.def() / 101).ToString();
      ss[4] = (l * this.unique.matk() / 101).ToString();
      ss[5] = (l * this.unique.mdef() / 101).ToString();
      ss[6] = (l * this.unique.spd() / 101).ToString();
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
    public int[] Fire_Dark(ConstStatus plus)
    {
      int[] ss = new int[6];
      ss[0] = fire() + plus.fire();
      ss[1] = water() + plus.water();
      ss[2] = wind() + plus.wind();
      ss[3] = earth() + plus.earth();
      ss[4] = light() + plus.light();
      ss[5] = dark() + plus.dark();
      return ss;
    }
    public int[] Fire_DarkWithMinus(ConstStatus minus)
    {
      int[] ss = new int[6];
      ss[0] = fire() - minus.fire();
      ss[1] = water() - minus.water();
      ss[2] = wind() - minus.wind();
      ss[3] = earth() - minus.earth();
      ss[4] = light() - minus.light();
      ss[5] = dark() - minus.dark();
      return ss;
    }
    public int[] Fire_Dark(ConstStatus plus, ConstStatus minus)
    {
      int[] ss = new int[6];
      ss[0] = fire() + plus.fire() - minus.fire();
      ss[1] = water() + plus.water() - minus.water();
      ss[2] = wind() + plus.wind() - minus.wind();
      ss[3] = earth() + plus.earth() - minus.earth();
      ss[4] = light() + plus.light() - minus.light();
      ss[5] = dark() + plus.dark() - minus.dark();
      return ss;
    }
    public string[] Fire_DarkBareStrings()
    {
      string[] ss = new string[6];
      int l = level_ + 2;
      ss[0] = (l * this.unique.fire() / 101).ToString();
      ss[1] = (l * this.unique.water() / 101).ToString();
      ss[2] = (l * this.unique.wind() / 101).ToString();
      ss[3] = (l * this.unique.earth() / 101).ToString();
      ss[4] = (l * this.unique.light() / 101).ToString();
      ss[5] = (l * this.unique.dark() / 101).ToString();
      return ss;
    }

    // △△△装備△△△
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

      if (idWeapon != -1) ss[0] = l.weapons[idWeapon].name();
      if (idBody != -1) ss[1] = l.body[idBody].name();
      if (idHead != -1) ss[2] = l.head[idHead].name();
      if (idArm != -1) ss[3] = l.arm[idArm].name();
      if (idExterior != -1) ss[4] = l.exterior[idExterior].name();
      if (idAccessory != -1) ss[5] = l.accessory[idAccessory].name();

      return ss;
    }
    public int GetEquipId(EquipCategory equipCategory)
    {
      switch(equipCategory)
      {
        case EquipCategory.Weapon: return idWeapon;
        case EquipCategory.Body: return idBody;
        case EquipCategory.Head: return idHead;
        case EquipCategory.Arm: return idArm;
        case EquipCategory.Exterior: return idExterior;
        case EquipCategory.Accessory: return idAccessory;
      }
      JsTrans.Assert("Status.cs EquipId");
      return -1;
    }
    // ▽▽▽装備▽▽▽

    // △△△基底クラス シングルトンパターン△△△
    protected Status() { }
    static protected Status[] s_instances;
    static protected int s_num = 0;
    static public int Num { get { return s_num; } }
    static public Status[] Instances { get { return s_instances; } }
    // ▽▽▽基底クラス シングルトンパターン▽▽▽

    // △△△string[]を生成する静的メソッド△△△
    static public string[] names()
    {
      string[] names = new string[s_num];
      for (int i = 0; i < names.Length; i++)
        names[i] = s_instances[i].name_;
      return names;
    }
    static public string[] hpStrings()
    {
      string[] hps = new string[s_num];
      for (int i = 0; i < hps.Length; i++)
        hps[i] = s_instances[i].hp_.ToString();
      return hps;
    }
    static public string[] NowHpStrings()
    {
      string[] hps = new string[s_num];
      for (int i = 0; i < hps.Length; i++)
        hps[i] = s_instances[i].nowHp_.ToString();
      return hps;
    }
    // ▽▽▽string[]を生成する静的メソッド▽▽▽
  }
}
