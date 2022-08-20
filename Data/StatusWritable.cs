using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class StatusWritable : Status
  {
    // 書き込みアクセサ
    public void name(string name) { name_ = name; }
    public void hp(int hp) { hp_ = hp; }
    public void PlusHp(int hp) { hp_ += hp; }
    public void mp(int mp) { mp_ = mp; }
    public void PlusMp(int mp) { mp_ += mp; }
    public void atk(int atk) { atk_ = atk; }
    public void PlusAtk(int atk) { atk_ += atk; }
    public void def(int def) { def_ = def; }
    public void PlusDef(int def) { def_ += def; }
    public void matk(int matk) { matk_ = matk; }
    public void PlusMAtk(int matk) { matk_ += matk; }
    public void mdef(int mdef) { mdef_ = mdef; }
    public void PlusMDef(int mdef) { mdef_ += mdef; }
    public void spd(int spd) { spd_ = spd; }
    public void PlusSpd(int spd) { spd_ += spd; }
    public void fire(int fire) { fire_ = fire; }
    public void PlusFire(int fire) { fire_ += fire; }
    public void water(int water) { water_ = water; }
    public void PlusWater(int water) { water_ += water; }
    public void wind(int wind) { wind_ = wind; }
    public void PlusWind(int wind) { wind_ += wind; }
    public void earth(int earth) { earth_ = earth; }
    public void PlusEarth(int earth) { earth_ += earth; }
    public void light(int light) { light_ = light; }
    public void PlusLight(int light) { light_ += light; }
    public void dark(int dark) { dark_ = dark; }
    public void PlusDark(int dark) { dark_ += dark; }
    public void NowHp(int hp)
    { nowHp_ = hp; }
    public void PlusNowHp(int hp)
    {
      nowHp_ += hp;
      if (nowHp_ > hp_) nowHp_ = hp_;
    }
    public void NowMp(int mp)
    { nowMp_ = mp; }
    public void PlusNowMp(int mp)
    {
      nowMp_ += mp;
      if (nowMp_ > mp_) nowMp_ = mp_;
    }
    public void Heal(int hp, int mp = 0)
    {
      PlusNowHp(hp);
      PlusNowMp(mp);
    }
    public void Heal()
    {
      Heal(99999, 99999);
    }

    public int experiencePlus(int exp) 
    {
      experience_ += exp;
      int ret = 0;
      int oldHp = hp_;
      int oldMp = mp_;
      while (NeedEexperienceUntilNextLevel() <= 0)
      {
        level_++;
        ret++;
      }
      ResetStatus();
      PlusNowHp(hp_ - oldHp);
      PlusNowMp(mp_ - oldMp);
      return ret;
    }
    public void Level(int level)
    {
      level_ = level;
      int l = level + 2;
      hp_ = l * this.unique.hp() / 101;
      mp_ = l * this.unique.mp() / 101;
      atk_ = l * this.unique.atk() / 101;
      def_ = l * this.unique.def() / 101;
      matk_ = l * this.unique.matk() / 101;
      mdef_ = l * this.unique.mdef() / 101;
      spd_ = l * this.unique.spd() / 101;
      fire_ = l * this.unique.fire() / 101;
      water_ = l * this.unique.water() / 101;
      wind_ = l * this.unique.wind() / 101;
      earth_ = l * this.unique.earth() / 101;
      light_ = l * this.unique.light() / 101;
      dark_ = l * this.unique.dark() / 101;
    }
    public void ResetStatus()
    {
      Level(this.level_);

      void eq(ConstStatus cs)
      {
        PlusHp(cs.hp());
        PlusMp(cs.mp());
        PlusAtk(cs.atk());
        PlusDef(cs.def());
        PlusMAtk(cs.matk());
        PlusMDef(cs.mdef());
        PlusSpd(cs.spd());
        PlusFire(cs.fire());
        PlusWater(cs.water());
        PlusWind(cs.wind());
        PlusEarth(cs.earth());
        PlusLight(cs.light());
        PlusDark(cs.dark());
      }

      Loader l = Loader.Instance;
      if (this.idWeapon != -1) eq(l.weapons[idWeapon]);
      if (this.idBody != -1) eq(l.body[idBody]);
      if (this.idHead != -1) eq(l.head[idHead]);
      if (this.idArm != -1) eq(l.arm[idArm]);
      if (this.idExterior != -1) eq(l.exterior[idExterior]);
      if (this.idAccessory != -1) eq(l.accessory[idAccessory]);
    }
    public void experience(int experience) { experience_ = experience; }

    // ▲▲▲装備▲▲▲
    public int Equip(EquipCategory category, int equipId)
    {
      int ret = -1;
      switch (category)
      {
        case EquipCategory.Weapon:
          ret = idWeapon;
          idWeapon = equipId;
          break;
        case EquipCategory.Body:
          ret = idBody;
          idBody = equipId;
          break;
        case EquipCategory.Head:
          ret = idHead;
          idHead = equipId;
          break;
        case EquipCategory.Arm:
          ret = idArm;
          idArm = equipId;
          break;
        case EquipCategory.Exterior:
          ret = idExterior;
          idExterior = equipId;
          break;
        case EquipCategory.Accessory:
          ret = idAccessory;
          idAccessory = equipId;
          break;
        default:
          JsTrans.Assert("Error: StatusEquippedClass::GetEquippedIdByCategoryId(categoryId)\n"+
            "CategoryId argment " + category.ToString() + " is invalid.");
          break;
      }
      ResetStatus();
      return ret;
    }
    // ▼▼▼装備▼▼▼

    public string TextOfSave()
    {
      return level_.ToString() + "," + experience_.ToString() + "," + idWeapon.ToString() + "," +
        idBody.ToString() + "," + idHead.ToString() + "," + idArm.ToString() + "," +
        idExterior.ToString() + "," + idAccessory.ToString() + "," +
        nowHp_.ToString() + "," + nowMp_.ToString();
    }
    public void Load(string text)
    {
      string[] ss = text.Split(",");
      level_ = int.Parse(ss[0]);
      experience_ = int.Parse(ss[1]);
      idWeapon = int.Parse(ss[2]);
      idBody = int.Parse(ss[3]);
      idHead = int.Parse(ss[4]);
      idArm = int.Parse(ss[5]);
      idExterior = int.Parse(ss[6]);
      idAccessory = int.Parse(ss[7]);
      nowHp_ = int.Parse(ss[8]);
      nowMp_ = int.Parse(ss[9]);
    }



    // ▲▲▲派生クラス シングルトンパターン▲▲▲
    private StatusWritable(ConstStatus unique)
    {
      this.unique = unique;
    }

    static readonly object lockObject_ = new object();
    public static new StatusWritable[] Instances(ConstStatus[] uniques)
    {
      int instanceNumber = uniques.Length;
      StatusWritable[] insts;
      lock (lockObject_)
      {
        JsTrans.Assert(s_instances == null,
          typeof(StatusWritable).Name + " がインスタンスが既にインスタンス化されています。");
        s_num = instanceNumber;
        s_instances = insts = new StatusWritable[instanceNumber];
        for (int i = 0; i < instanceNumber; i++)
        {
          StatusWritable sw = new StatusWritable(uniques[i]);
          sw.name(uniques[i].name());
          s_instances[i] = sw;
        }
      }
      return insts;
    }
    // ▼▼▼派生クラス シングルトンパターン▼▼▼
  }
}
