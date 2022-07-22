using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System; // VirtualKey
using Windows.UI.Xaml.Controls; // Grid
using Windows.UI;
using Windows.UI.Xaml; // Thickness
using Windows.UI.Xaml.Media; // Stretch
using Windows.UI.Xaml.Input;
using Windows.UI.Text;
using Univ.NsMenu;

namespace Univ.NsMenu
{
  internal class MenuStatus
  {
    TextBlock tbName_;
    TextBlock tbLv_; // "Lv " + st.level()
    TextBlock tbExperience_;

    TextBlock[] tbHp_SpdBefore_ = new TextBlock[7];
    TextBlock[] tbHp_SpdAfter_ = new TextBlock[7];
    TextBlock[] tbFire_DarkBefore_ = new TextBlock[6];
    TextBlock[] tbFire_DarkAfter_ = new TextBlock[6];
    TextBlock[] tbEquips_ = new TextBlock[6];

    TextBlock titleBaseLeft_;
    TextBlock titleBaseRight_;
    TextBlock titleAttributeLeft_;
    TextBlock titleAttributeRight_;

    public MenuStatus()
    {

    }
    public void Create(Grid parent)
    {
      Data.Status[] sts = Data.Status.Instances;
      Data.Status st = sts[0];

      tbName_ = MenuUI.RunLavel(parent, 0, 0, "ホーン", fontSize: 16);
      tbLv_ = MenuUI.RunLavel(parent, 95, 0, "Lv " + st.level(), fontSize: 16);
      MenuUI.RunLavel(parent, 0, 25, "経験値", fontSize: 16);
      tbExperience_ = MenuUI.RunLavel(parent, 95, 25, st.experience().ToString(), fontSize: 16);

      MenuUI.RunLavel(parent, 3, 55, "装備", fontSize: 16, useBold: true);

      titleBaseLeft_ = MenuUI.RunLavel(parent, 55, 55, "前", fontSize: 16, useBold: true);
      titleBaseRight_ = MenuUI.RunLavel(parent, 98, 55, "後", fontSize: 16, useBold: true);
      string[] stHp_SpdItemStrings = { "HP", "MP", "攻撃", "防御", "魔攻", "魔防", "早さ" };
      string[] stHp_SpdBareStrings = st.Hp_SpdBareStrings();
      string[] stHp_SpdEquipStrings = st.Hp_SpdStrings();
      for (int i = 0; i < stHp_SpdItemStrings.Length; i++)
      {
        MenuUI.RunLavelRightAligned(parent, 0, 80 + i * 25, 33, stHp_SpdItemStrings[i], 16);
        tbHp_SpdBefore_[i] =
          MenuUI.RunLavelRightAligned(parent, 38, 80 + i * 25, 33, stHp_SpdBareStrings[i], 16);
        tbHp_SpdAfter_[i] =
          MenuUI.RunLavelRightAligned(parent, 76, 80 + i * 25, 33, stHp_SpdEquipStrings[i], 16);
      }

      titleAttributeLeft_ = MenuUI.RunLavel(parent, 175, 55, "前", fontSize: 16, useBold: true);
      titleAttributeRight_ = MenuUI.RunLavel(parent, 218, 55, "後", fontSize: 16, useBold: true);
      string[] stFire_DarkItemStrings = { "火耐", "水耐", "風耐", "地耐", "光耐", "闇耐" };
      string[] stFire_DarkSBaretrings = st.Fire_DarkBareStrings();
      string[] stFire_DarkEquipStrings = st.Fire_DarkStrings();
      for (int i = 0; i < stFire_DarkItemStrings.Length; i++)
      {
        MenuUI.RunLavelRightAligned(parent, 130, 80 + i * 25, 33, stFire_DarkItemStrings[i], 16);
        tbFire_DarkBefore_[i] =
          MenuUI.RunLavelRightAligned(parent, 158, 80 + i * 25, 33, stFire_DarkSBaretrings[i], 16);
        tbFire_DarkAfter_[i] =
          MenuUI.RunLavelRightAligned(parent, 196, 80 + i * 25, 33, stFire_DarkEquipStrings[i], 16);
      }

      string[] stWepon_AccesoryItemStrings = { "武器", "体", "頭", "腕", "外装", "装飾" };
      string[] stEquipStrings = st.EquipStrings();
      for (int i = 0; i < stWepon_AccesoryItemStrings.Length; i++)
      {
        MenuUI.RunLavelCenterAligned(parent, 0, 260 + i * 25, 33, stWepon_AccesoryItemStrings[i], 16, true);
        tbEquips_[i] =
          MenuUI.RunLavel(parent, 38, 260 + i * 25, stEquipStrings[i], fontSize: 16);
      }
    }

    public void Change(int charId)
    {
      Data.Status[] sts = Data.Status.Instances;
      Data.Status st = sts[charId];

      tbName_.Text = st.name();
      tbLv_.Text = "Lv " + st.level();
      tbExperience_.Text = st.experience().ToString();

      string[] h_sBare = st.Hp_SpdBareStrings();
      string[] h_s = st.Hp_SpdStrings();
      for (int i = 0; i < h_sBare.Length; i++)
      {
        tbHp_SpdBefore_[i].Text = h_sBare[i];
        tbHp_SpdAfter_[i].Text = h_s[i];
      }

      string[] f_dBare = st.Fire_DarkBareStrings();
      string[] f_d = st.Fire_DarkStrings();
      for (int i = 0; i < f_dBare.Length; i++)
      {
        tbFire_DarkBefore_[i].Text = f_dBare[i];
        tbFire_DarkAfter_[i].Text = f_d[i];
      }

      string[] stEquipStrings = st.EquipStrings();
      for (int i = 0; i < stEquipStrings.Length; i++)
      {
        tbEquips_[i].Text = stEquipStrings[i];
      }
    }
    public void Title(string left = "前", string right = "後")
    {
      titleBaseLeft_.Text = left;
      titleBaseRight_.Text = right;
      titleAttributeLeft_.Text = left;
      titleAttributeRight_.Text = right;
    }
    public void EquipChange(int charId)
    {
      Data.Status[] sts = Data.Status.Instances;
      Data.Status st = sts[charId];

      tbName_.Text = st.name();
      tbLv_.Text = "Lv " + st.level();
      tbExperience_.Text = st.experience().ToString();

      Brush normal = UnivLib.GetBrush(14, 77, 108);
      FontWeight nBold = UnivLib.FontWeightBold(false);

      string[] h_sBare = st.Hp_SpdBareStrings();
      string[] h_s = st.Hp_SpdStrings();
      for (int i = 0; i < h_sBare.Length; i++)
      {
        tbHp_SpdBefore_[i].Text = h_s[i];
        tbHp_SpdAfter_[i].Text = "-";
        tbHp_SpdAfter_[i].Foreground = normal;
        tbHp_SpdAfter_[i].FontWeight = nBold;
      }

      string[] f_dBare = st.Fire_DarkBareStrings();
      string[] f_d = st.Fire_DarkStrings();
      for (int i = 0; i < f_dBare.Length; i++)
      {
        tbFire_DarkBefore_[i].Text = f_d[i];
        tbFire_DarkAfter_[i].Text = "-";
        tbFire_DarkAfter_[i].Foreground = normal;
        tbFire_DarkAfter_[i].FontWeight = nBold;
      }

      string[] stEquipStrings = st.EquipStrings();
      for (int i = 0; i < stEquipStrings.Length; i++)
      {
        tbEquips_[i].Text = stEquipStrings[i];
      }
    }
    public void EquipChangeCalc(int charId, int categoryId, int eqId)
    {

      Data.Loader l = Data.Loader.Instance;
      Data.ConstStatus eq = null;
      if (eqId >= 0) eq = l.EquipArray((Data.EquipCategory)categoryId)[eqId];

      Data.Status[] sts = Data.Status.Instances;
      Data.Status charStatus = sts[charId];
      int equipedId = charStatus.GetEquipId((Data.EquipCategory)categoryId);
      Data.ConstStatus equiped = null;
      if (equipedId >= 0) equiped = l.EquipArray((Data.EquipCategory)categoryId)[equipedId];


      int[] h_s, f_d;
      if (eqId == -1)
      {
        if (equipedId == -1)
        {
          EquipChange(charId);
          return;
        }
        else
        {
          h_s = charStatus.Hp_SpdWithMinus(equiped);
          f_d = charStatus.Fire_DarkWithMinus(equiped);
        }
      } 
      else
      {
        if (equipedId == -1)
        {
          h_s = charStatus.Hp_Spd(eq);
          f_d = charStatus.Fire_Dark(eq);
        }
        else
        {
          h_s = charStatus.Hp_Spd(eq, equiped);
          f_d = charStatus.Fire_Dark(eq, equiped);
        }
      }

      Brush blue = UnivLib.GetBrush(Colors.Blue);
      Brush red = UnivLib.GetBrush(Colors.Red);
      Brush normal = UnivLib.GetBrush(14, 77, 108);
      FontWeight bold = UnivLib.FontWeightBold();
      FontWeight nBold = UnivLib.FontWeightBold(false);
      for (int i = 0; i < h_s.Length; i++)
      {
        tbHp_SpdAfter_[i].Text = h_s[i].ToString();
        int prev = charStatus.Value(i);
        if (prev > h_s[i])
        {
          tbHp_SpdAfter_[i].Foreground = blue;
          tbHp_SpdAfter_[i].FontWeight = bold;
        }
        else if (prev < h_s[i])
        {
          tbHp_SpdAfter_[i].Foreground = red;
          tbHp_SpdAfter_[i].FontWeight = bold;
        }
        else
        {
          tbHp_SpdAfter_[i].Foreground = normal;
          tbHp_SpdAfter_[i].FontWeight = nBold;
        }
      }

      for (int i = 0; i < f_d.Length; i++)
      {
        tbFire_DarkAfter_[i].Text = f_d[i].ToString();
        int prev = charStatus.Value(i + h_s.Length);
        if (prev > f_d[i])
        {
          tbFire_DarkAfter_[i].Foreground = blue;
          tbFire_DarkAfter_[i].FontWeight = bold;
        }
        else if (prev < f_d[i])
        {
          tbFire_DarkAfter_[i].Foreground = red;
          tbFire_DarkAfter_[i].FontWeight = bold;
        }
        else
        {
          tbFire_DarkAfter_[i].Foreground = normal;
          tbFire_DarkAfter_[i].FontWeight = nBold;
        }
      }
    }
  }
}
