using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Univ.NsBattle
{
  internal class BattleUIExWindow
  {
    Grid monitor_;   // 描画用
    Grid gTop_;
    TextBlock tbTop_;
    Queue<string> topStrings_;
    public BattleUIExWindow(Grid monitor)
    {
      monitor_ = monitor;

      gTop_ = new Grid();
      gTop_.VerticalAlignment = VerticalAlignment.Top;
      gTop_.Margin = new Thickness(30, 15, 30, 0);
      gTop_.Width = 740;
      gTop_.Height = 55;
      gTop_.BorderBrush = UnivLib.GetBrush(Colors.White);
      gTop_.BorderThickness = new Thickness(2, 2, 2, 2);
      gTop_.Background = UnivLib.GetBrush(57, 81, 253);
      //monitor_.Children.Add(topGrid);

      tbTop_ = new TextBlock();
      tbTop_.Margin = new Thickness(7, 0, 0, 0);
      tbTop_.Text = "たたかいにかった";
      tbTop_.Height = 40;
      tbTop_.FontSize = 30;
      tbTop_.Foreground = UnivLib.GetBrush(Colors.White);
      tbTop_.TextAlignment = TextAlignment.Left;
      gTop_.Children.Add(tbTop_);

      topStrings_ = null;
    }
    public bool IsShowTop()
    {
      return monitor_.Children.IndexOf(gTop_) != -1;
    }
    public void ShowTop(string text, bool isWide = false)
    {
      if (isWide)
      {
        gTop_.Width = 740;
        tbTop_.TextAlignment = TextAlignment.Left;
      }
      else
      {
        gTop_.Width = 300;
        tbTop_.TextAlignment = TextAlignment.Center;
      }
      tbTop_.Text = text;

      if (!IsShowTop())
        monitor_.Children.Add(gTop_);
    }
    public void HiddenTop()
    {
      monitor_.Children.Remove(gTop_);
    }
    public void SetTopStrings(Queue<string> texts)
    {
      topStrings_ = texts;
    }
    public bool NextTopStringsShow()
    {
      JsTrans.Assert(topStrings_!=null, "BattleUIExWingos NextTopStringShow() topStrings_!=null");
      JsTrans.Assert(!IsShowTop(), "BattleUIExWingos NextTopStringShow() !IsShowTop()");
      if (topStrings_.Count > 0)
      {
        ShowTop(topStrings_.Dequeue(), true);
        return true;
      }
      return false;
    }
  }
}
