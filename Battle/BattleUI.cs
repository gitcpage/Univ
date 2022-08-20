using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System; // VirtualKey
using Windows.UI.Xaml; // Thickness
using Windows.UI.Xaml.Controls; // Grid
using Windows.UI.Xaml.Media; // Stretch
using Windows.UI;
using Windows.UI.Xaml.Input; // PointerRoutedEventArgs

namespace Univ.NsBattle
{
  delegate void BattleNotify(Battle.NotifyCode notifyCode);
  internal class BattleUI : BattleInclude
  {
    Grid monitor_;   // 描画用
    BattleNotify notify_;

    Grid[] charsGrid_;
    TextBlock[] charsArrow_;
    TextBlock[] charsActiveMark_;
    Image[] chars_;
    ProgressBar[] bars_;
    Brush gaugeChargeBrush_;
    Brush gaugeChargedBrush_;
    Brush gaugeActingBrush_;

    public BattleUI(Grid monitor, BattleNotify battleNotify)
    {
      notify_ = battleNotify;

      monitor_ = monitor;
      charsGrid_ = new Grid[5];
      charsArrow_ = new TextBlock[5];
      charsActiveMark_ = new TextBlock[5];
      chars_ = new Image[5];
      bars_ = new ProgressBar[5];
      gaugeChargeBrush_ = UnivLib.GetBrush(0, 120, 212); // プログレスバーのデフォルト色
      gaugeChargedBrush_ = UnivLib.GetBrush(Colors.Orange);
      gaugeActingBrush_ = UnivLib.GetBrush(Colors.GreenYellow);
    }
    public void ShowCharsArrow(int index = -1)
    {
      JsTrans.Assert(index, -1, 4, "BattleUI.ShowCharsArrow index: " + index);
      if (index >= 0)
      {
        charsArrow_[index].Text = "";
      }
    }
    // △△△初期化△△△
    public void RunCharOne(int index, int top)
    {
      string str = index.ToString();
      charsGrid_[index] = new Grid();
      charsGrid_[index].Margin = new Thickness(600, top-16, 0, 0);

      charsArrow_[index] = new TextBlock();
      charsArrow_[index].Foreground = UnivLib.GetBrush(Colors.White);
      charsArrow_[index].Text = "➤";
      charsArrow_[index].Padding = new Thickness(0, 26, 0, 0);
      charsGrid_[index].Children.Add(charsArrow_[index]);

      charsActiveMark_[index] = new TextBlock();
      charsActiveMark_[index].Foreground = UnivLib.GetBrush(0, 255, 255, 255);
      charsActiveMark_[index].Text = "▼";
      charsActiveMark_[index].Padding = new Thickness(38, 0, 0, 0);
      charsGrid_[index].Children.Add(charsActiveMark_[index]);

      chars_[index] = UnivLib.ImageInstance(0, 0, "char/char"+ str + "p10.png", "char" + str/*, "char"*/);
      chars_[index].Margin = new Thickness(20, 16, 0, 0);
      charsGrid_[index].Children.Add(chars_[index]);

      monitor_.Children.Add(charsGrid_[index]);
    }
    public void RunChars()
    {
      RunCharOne(0, 150);
      RunCharOne(1, 205);
      RunCharOne(2, 260);
      RunCharOne(3, 315);
      RunCharOne(4, 370);
    }
    public StackPanel RunStringPanelRightAlignment(string[] strings, int left, int width)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Padding = new Thickness(left, 0, 0, 0);
      stackPanel.HorizontalAlignment = HorizontalAlignment.Left;
      TextBlock tbUnit1 = RunTextBlockInstance(strings[0], TextAlignment.Right);
      tbUnit1.Margin = new Thickness(0, 7, 0, 0);
      tbUnit1.Width = width;
      stackPanel.Children.Add(tbUnit1);
      for (int i = 1; i <= 4; i++)
      {
        TextBlock tb = RunTextBlockInstance(strings[i], TextAlignment.Right);
        tb.Width = width;
        stackPanel.Children.Add(tb);
      }
      return stackPanel;
    }
    public StackPanel RunSliderPanel(BattleAtb atb)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Padding = new Thickness(270, 0, 0, 0);

      ProgressBar Create(int charId, int MarginTop = 15)
      {
        ProgressBar bar = new ProgressBar();
        bar.Width = 80;
        bar.Height = 10;
        // ■ゲージ初期値の設定
        bar.Maximum = BattleAtb.kGaugeFullValue;
        bar.Value = atb.GetGaugeValue(charId);
        bar.Margin = new Thickness(0, MarginTop, 0, 0);
        stackPanel.Children.Add(bar);
        return bar;
      }
      bars_[0] = Create(0, 26);
      bars_[1] = Create(1);
      bars_[2] = Create(2);
      bars_[3] = Create(3);
      bars_[4] = Create(4);
      return stackPanel;
    }
    public void RunControllerTextBlock(int left, int top, int width, string str, Grid parent, Battle.NotifyCode notifyCode)
    {
      TextBlock tb = new TextBlock();
      tb.Text = str;
      tb.FontSize = 17;
      tb.TextAlignment = TextAlignment.Center;
      tb.VerticalAlignment = VerticalAlignment.Center;
      tb.HorizontalAlignment = HorizontalAlignment.Center;
      tb.HorizontalTextAlignment = TextAlignment.Center;
      tb.PointerEntered += (Object sender, PointerRoutedEventArgs e) =>
      {
        TextBlock t = (TextBlock)sender;
        t.Foreground = UnivLib.GetBrush(7, 37, 54);
        Border bdr = VisualTreeHelper.GetParent(t) as Border;
        bdr.Background = UnivLib.GetBrush(213, 213, 213);
      };
      tb.PointerExited += (Object sender, PointerRoutedEventArgs e) =>
      {
        TextBlock t = (TextBlock)sender;
        t.Foreground = UnivLib.GetBrush(0, 0, 0);
        Border bdr = VisualTreeHelper.GetParent(t) as Border;
        bdr.Background = UnivLib.GetBrush(255, 255, 255);
      };
      tb.Tapped += (Object sender, TappedRoutedEventArgs e) =>
      {
        notify_(notifyCode);
      };

      Border border = new Border();
      border.Width = width;
      border.Height = 25;
      border.Margin = new Thickness(left, top, 115 - left - width, 160 - top - 25 - 20);
      border.VerticalAlignment = VerticalAlignment.Center;
      border.HorizontalAlignment = HorizontalAlignment.Center;
      border.Child = tb;

      parent.Children.Add(border);
    }
    // ▽▽▽初期化▽▽▽

    public void SetGaugeValue(int charId, int value)
    {
      //■ゲージの設定
      bars_[charId].Value = value;
      if (bars_[charId].Value >= kGaugeFullValue)
      {
        bars_[charId].Foreground = gaugeChargedBrush_;
      }
    }
    public void SetActiveMark(int charId, bool isCommand, bool isAction = false)
    {
      if (isCommand)
      {
        charsActiveMark_[charId].Foreground = UnivLib.GetBrush(Colors.White);
        bars_[charId].Foreground = gaugeChargedBrush_;
      }
      else if (isAction)
      {
        charsActiveMark_[charId].Foreground = UnivLib.GetBrush(0, 255, 255, 255);
        bars_[charId].Foreground = gaugeActingBrush_;
      }
      else
      {
        charsActiveMark_[charId].Foreground = UnivLib.GetBrush(0, 255, 255, 255);
        bars_[charId].Foreground = gaugeChargeBrush_;
      }
    }


    static public Grid RunUnderGrid(int left, int width)
    {
      Grid g = new Grid();
      g.Width = width;
      g.Height = 146;
      g.Margin = new Thickness(left, 7, 800 - left - width, 7);
      g.Background = UnivLib.GetBrush(Colors.White);
      g.BorderBrush = UnivLib.GetBrush(Colors.Gray);
      g.BorderThickness = new Thickness(2, 2, 2, 2);
      return g;
    }
    static public TextBlock RunTextBlockInstance(string str, TextAlignment alignment = TextAlignment.Left)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Text = str;
      textBlock.Height = 25;
      textBlock.FontSize = 20;
      textBlock.TextAlignment = alignment;
      return textBlock;
    }
    static public StackPanel RunStringPanel(string[] strings, int left = 10, TextBlock[] tbs = null)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Padding = new Thickness(left, 0, 0, 0);
      TextBlock tb = RunTextBlockInstance(strings[0]);
      if (tbs != null) tbs[0] = tb;
      tb.Margin = new Thickness(0, 7, 0, 0);
      stackPanel.Children.Add(tb);
      for (int i = 1; i <= 4; i++)
      {
        tb = RunTextBlockInstance(strings[i]);
        if (tbs != null) tbs[i] = tb;
        stackPanel.Children.Add(tb);
      }
      return stackPanel;
    }
  }
}
