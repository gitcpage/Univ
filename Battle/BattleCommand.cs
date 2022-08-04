using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls; // Grid
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml; // Visibilityl
using Windows.UI.Xaml.Input; //TappedRoutedEventArgs

namespace Univ.NsBattle
{
  delegate void BattleCommandNotify(int cmd1);

  internal class BattleCommand
  {
    Grid grid_;

    TextBlock[] tbArrows = new TextBlock[5];
    TextBlock[] tbTexts = new TextBlock[5];

    readonly Brush kCurrentBrush_ = UnivLib.GetBrush(Colors.Black);
    readonly Brush kSelectedBrush = UnivLib.GetBrush(128, 0, 0, 0);
    readonly Brush kErasedBrush = UnivLib.GetBrush(255, 127, 201, 203);

    int selected = 0;
    public bool IsShowing { get { return grid_.Visibility == Visibility.Visible; } }
    BattleCommandNotify commandNotify_;

    void Arrow(int idx)
    {
      for (int j = 0; j < tbArrows.Length; j++)
      {
        if (j == idx)
          tbArrows[j].Foreground = kCurrentBrush_;
        else
          tbArrows[j].Foreground = kErasedBrush;
      }
      selected = idx;
    }
    public BattleCommand(Grid underGrid, BattleCommandNotify commandNotify)
    {
      commandNotify_ = commandNotify;
      grid_ = BattleUI.RunUnderGrid(170, 130);
      grid_.Visibility = Visibility.Collapsed;
      grid_.Background = UnivLib.GetBrush(127, 201, 203);
      string[] charArrows = new string[5] {
        "➤", "➤", "➤", "➤", "➤" };
      StackPanel spArrow = BattleUI.RunStringPanel(charArrows, tbs: tbArrows);
      grid_.Children.Add(spArrow);

      string[] commandsString = new string[5] {
        "たたかう", "とくぎ", "まほう", "アイテム", "そうび" };
      StackPanel spText = BattleUI.RunStringPanel(commandsString, 35, tbTexts);
      for (int i = 0; i < tbTexts.Length; i++)
      {
        int cmdIdx = i;
        spText.Children[i].Tapped += (Object sender, TappedRoutedEventArgs e) =>
        {
          if (cmdIdx == selected)
          {
            tbArrows[cmdIdx].Foreground = kSelectedBrush;
            commandNotify_(cmdIdx);
          }
          else
          {
            Arrow(cmdIdx);
            commandNotify_(-1);
          }
        };
      }
      grid_.Children.Add(spText);
      underGrid.Children.Add(grid_);
      Arrow(0);
    }

    public void Show(bool isShow = true)
    {
      if (isShow)
      {
        JsTrans.Assert(grid_.Visibility == Visibility.Collapsed,
          "BattleCommand.cs Show grid_.Visibility==Visibility.Collapsed");
        Arrow(0);
        grid_.Visibility = Visibility.Visible;
      }
      else
      {
        JsTrans.Assert(grid_.Visibility == Visibility.Visible,
          "BattleCommand.cs Show grid_.Visibility==Visibility.Visible");
        grid_.Visibility = Visibility.Collapsed;
      }
    }
  }
}
