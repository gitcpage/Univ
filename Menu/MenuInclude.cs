using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml; // VerticalAlignment, HorizontalAlignment
using Windows.UI.Xaml.Controls; // TextBlock
using Windows.UI.Xaml.Media; // FontFamily

namespace Univ.NsMenu
{
  public enum NotifyCode
  {
    Left, Up, Right, Down, Top, Bottom, Escape, Next, Ok, Cancel,
    Skill, Equip, Item, Alignment, Strategy, Status, Custom, Save, Load,
    Char0, Char1, Char2, Char3,
    None
  }
  internal class MenuInclude
  {
    protected const int kMainLeftStringsNum = 9;
    protected readonly string[] kMainLeftStrings = 
      { "特技", "装備", "ｱｲﾃﾑ", "隊列", "作戦", "ｽﾃｰﾀｽ", "ｶｽﾀﾑ", "ｾｰﾌﾞ", "ﾛｰﾄﾞ" };
    protected readonly NotifyCode[] kMainLeftNotifies =
        { NotifyCode.Skill, NotifyCode.Equip, NotifyCode.Item, NotifyCode.Alignment, NotifyCode.Strategy,
        NotifyCode.Status, NotifyCode.Custom, NotifyCode.Save, NotifyCode.Load };
    protected const int kMainCenterNum = 4;

    static public Border GetBorder(int width)
    {
      Border bdr = new Border();
      bdr.HorizontalAlignment = HorizontalAlignment.Left;
      bdr.VerticalAlignment = VerticalAlignment.Top;
      bdr.Width = width;
      return bdr;
    }
    static public TextBlock GetTextBlock(string text, double fontSize = 19)
    {
      TextBlock txt = new TextBlock();
      txt.Text = text;
      txt.VerticalAlignment = VerticalAlignment.Center;
      txt.HorizontalAlignment = HorizontalAlignment.Left;
      txt.FontSize = fontSize;
      txt.FontFamily = new FontFamily("メイリオ");
      txt.Foreground = MenuUI.kCursorBrush;
      return txt;
    }
  }
}
