using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  internal class Bag
  {
    Bag(int weaponsNum, int bodyNum, int headNum, int armNum, int exteriorNum, int accessoryNum)
    {
      s_weapons = new int[weaponsNum];
      s_body = new int[bodyNum];
      s_head = new int[headNum];
      s_arm = new int[armNum];
      s_exterior = new int[exteriorNum];
      s_accessory = new int[accessoryNum];
    }
    static int[] s_weapons;
    static int[] s_body;
    static int[] s_head;
    static int[] s_arm;
    static int[] s_exterior;
    static int[] s_accessory;

    public int wepons(int id) { return s_weapons[id]; }
    public void wepons(int id, int num) { s_weapons[id] = num; }
    public int weponsPlus(int id, int num = 1) { s_weapons[id] += num; JsTrans.Assert(s_weapons[id]>=0,"wepons"); return s_weapons[id]; }
    public int body(int id) { return s_body[id]; }
    public void body(int id, int num) { s_body[id] = num; }
    public int bodyPlus(int id, int num = 1) { s_body[id] += num; JsTrans.Assert(s_body[id] >= 0, "body"); return s_body[id]; }
    public int head(int id) { return s_head[id]; }
    public void head(int id, int num) { s_head[id] = num; }
    public int headPlus(int id, int num = 1) { s_head[id] += num; JsTrans.Assert(s_head[id] >= 0, "head"); return s_head[id]; }
    public int arm(int id) { return s_arm[id]; }
    public void arm(int id, int num) { s_arm[id] = num; }
    public int armPlus(int id, int num = 1) { s_arm[id] += num; JsTrans.Assert(s_arm[id] >= 0, "arm"); return s_arm[id]; }
    public int exterior(int id) { return s_exterior[id]; }
    public void exterior(int id, int num) { s_exterior[id] = num; }
    public int exteriorPlus(int id, int num = 1) { s_exterior[id] += num; JsTrans.Assert(s_exterior[id] >= 0, "exterior"); return s_exterior[id]; }
    public int accessory(int id) { return s_accessory[id]; }
    public void accessory(int id, int num) { s_accessory[id] = num; }
    public int accessoryPlus(int id, int num = 1) { s_accessory[id] += num; JsTrans.Assert(s_accessory[id] >= 0, "accessory"); return s_accessory[id]; }
    public int equip(EquipCategory equipCategory, int id)
    {
      switch (equipCategory)
      {
        case EquipCategory.Weapon: return wepons(id);
        case EquipCategory.Body: return body(id);
        case EquipCategory.Head: return head(id);
        case EquipCategory.Arm: return arm(id);
        case EquipCategory.Exterior: return exterior(id);
        case EquipCategory.Accessory: return accessory(id);
        default:
          JsTrans.Assert("Bag.cs equip");
          return -1;
      }
    }
    public int equipPlus(EquipCategory equipCategory, int id, int num = 1)
    {
      switch (equipCategory)
      {
        case EquipCategory.Weapon: return weponsPlus(id, num);
        case EquipCategory.Body: return bodyPlus(id, num);
        case EquipCategory.Head: return headPlus(id, num);
        case EquipCategory.Arm: return armPlus(id, num);
        case EquipCategory.Exterior: return exteriorPlus(id, num);
        case EquipCategory.Accessory: return accessoryPlus(id, num);
        default:
          JsTrans.Assert("Bag.cs equipPlus");
          return -1;
      }
    }
    public void equipSubstitution(EquipCategory equipCategory, int id, int num = 1)
    {
      switch (equipCategory)
      {
        case EquipCategory.Weapon: s_weapons[id] = num; break;
        case EquipCategory.Body: s_body[id] = num; break;
        case EquipCategory.Head: s_head[id] = num; break;
        case EquipCategory.Arm: s_arm[id] = num; break;
        case EquipCategory.Exterior: s_exterior[id] = num; break;
        case EquipCategory.Accessory: s_accessory[id] = num; break;
        default:
          JsTrans.Assert("Bag.cs equipSubstitution");
          break;
      }
    }
    public int[] GetArrayByCategory(EquipCategory equipCategory)
    {
      switch (equipCategory)
      {
        case EquipCategory.Weapon: return s_weapons;
        case EquipCategory.Body: return s_body;
        case EquipCategory.Head: return s_head;
        case EquipCategory.Arm: return s_arm;
        case EquipCategory.Exterior: return s_exterior;
        case EquipCategory.Accessory: return s_accessory;
        default:
          JsTrans.Assert("Bag.cs GetArrayByCategory");
          return null;
      }
    }

    static private Bag instance = null;
    static public Bag Instance
    {
      get
      {
        if (instance == null)
        {
          JsTrans.window_close("セットアップ前に Bag.Instance が呼び出されました。");
        }
        return instance;
      }
    }
    static public Bag Setup(int weaponsNum, int bodyNum, int headNum, int armNum, int exteriorNum, int accessoryNum)
    {
      if (instance == null)
      {
        return instance = new Bag(weaponsNum, bodyNum, headNum, armNum, exteriorNum, accessoryNum);
      }
      return null;
      //JsTrans.window_close("セットアップ済みなのに Bag.Setup() が呼び出されました。");
    }
  }
}
