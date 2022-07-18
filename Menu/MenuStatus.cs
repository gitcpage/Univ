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
    public void Create(Grid parent)
    {
      Data.Status[] sts = Data.Status.Instances;
      Data.Status st = sts[0];

      MenuUI.RunLavel(parent, 0, 0, "ホーン", fontSize: 16);
      MenuUI.RunLavel(parent, 95, 0, "Lv " + st.level(), fontSize: 16);
      MenuUI.RunLavel(parent, 0, 25, "経験値", fontSize: 16);
      MenuUI.RunLavel(parent, 95, 25, st.experience().ToString(), fontSize: 16);

      MenuUI.RunLavel(parent, 3, 55, "装備", fontSize: 16, useBold: true);

      MenuUI.RunLavel(parent, 55, 55, "前", fontSize: 16, useBold: true);
      MenuUI.RunLavel(parent, 98, 55, "後", fontSize: 16, useBold: true);
      string[] stHp_SpdItemStrings = { "HP", "MP", "攻撃", "防御", "魔攻", "魔防", "早さ" };
      string[] stHp_SpdBareStrings = st.Hp_SpdBareStrings();
      string[] stHp_SpdEquipStrings = st.Hp_SpdStrings();
      for (int i = 0; i < stHp_SpdItemStrings.Length; i++)
      {
        MenuUI.RunLavelRightAligned(parent, 0, 80 + i * 25, 33, stHp_SpdItemStrings[i], fontSize: 16);
        MenuUI.RunLavelRightAligned(parent, 38, 80 + i * 25, 33, stHp_SpdBareStrings[i], fontSize: 16);
        MenuUI.RunLavelRightAligned(parent, 76, 80 + i * 25, 33, stHp_SpdEquipStrings[i], fontSize: 16);
      }

      MenuUI.RunLavel(parent, 175, 55, "前", fontSize: 16, useBold: true);
      MenuUI.RunLavel(parent, 218, 55, "後", fontSize: 16, useBold: true);
      string[] stFire_DarkItemStrings = { "火耐", "水耐", "風耐", "地耐", "光耐", "闇耐" };
      string[] stFire_DarkSBaretrings = st.Fire_DarkBareStrings();
      string[] stFire_DarkEquipStrings = st.Fire_DarkStrings();
      for (int i = 0; i < stFire_DarkItemStrings.Length; i++)
      {
        MenuUI.RunLavelRightAligned(parent, 130, 80 + i * 25, 33, stFire_DarkItemStrings[i], fontSize: 16);
        MenuUI.RunLavelRightAligned(parent, 158, 80 + i * 25, 33, stFire_DarkSBaretrings[i], fontSize: 16);
        MenuUI.RunLavelRightAligned(parent, 196, 80 + i * 25, 33, stFire_DarkEquipStrings[i], fontSize: 16);
      }

      string[] stWepon_AccesoryItemStrings = { "武器", "体", "頭", "腕", "外装", "装飾" };
      string[] stEquipStrings = st.EquipStrings();
      for (int i = 0; i < stWepon_AccesoryItemStrings.Length; i++)
      {
        MenuUI.RunLavelCenterAligned(parent, 0, 260 + i * 25, 33, stWepon_AccesoryItemStrings[i],
          fontSize: 16, useBold: true);
        MenuUI.RunLavel(parent, 38, 260 + i * 25, stEquipStrings[i], fontSize: 16);
      }
    }
  }
}
