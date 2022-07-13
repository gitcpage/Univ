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
  delegate void BattleNotify(Battle.NotifyCode a);
  internal class BattleUI
  {
    Grid monitor_;   // 描画用
    BattleNotify notify_;

    Grid[] charsGrid_;
    TextBlock[] charsArrow_;
    TextBlock[] charsActive_;
    Image[] chars_;

    public BattleUI(Grid monitor, BattleNotify battleNotify)
    {
      notify_ = battleNotify;

      monitor_ = monitor;
      charsGrid_ = new Grid[5];
      charsArrow_ = new TextBlock[5];
      charsActive_ = new TextBlock[5];
      chars_ = new Image[5];
    }
    public void ShowCharsArrow(int index = -1)
    {
      JsTrans.Assert(index, -1, 4, "BattleUI.ShowCharsArrow index: " + index);
      if (index >= 0)
      {
        charsArrow_[index].Text = "";
      }
    }
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

      charsActive_[index] = new TextBlock();
      charsActive_[index].Foreground = UnivLib.GetBrush(Colors.White);
      charsActive_[index].Text = "▼";
      charsActive_[index].Padding = new Thickness(38, 0, 0, 0);
      charsGrid_[index].Children.Add(charsActive_[index]);

      chars_[index] = UnivLib.ImageInstance(0, 0, "char/char"+ str + "p10.png", "char" + str, "char");
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
    public Grid RunUnderGrid(int left, int width)
    {
      Grid g = new Grid();
      g.Width = width;
      g.Height = 146;
      g.Margin = new Thickness(left, 7, 800 -left - width, 7);
      g.Background = UnivLib.GetBrush(Colors.White);
      g.BorderBrush = UnivLib.GetBrush(Colors.Gray);
      g.BorderThickness = new Thickness(2, 2, 2, 2);
      return g;
    }
    public TextBlock RunTextBlockInstance(string str, TextAlignment alignment = TextAlignment.Left)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.Text = str;
      textBlock.Height = 25;
      textBlock.FontSize = 20;
      textBlock.TextAlignment = alignment;
      return textBlock;
    }
    public StackPanel RunStringPanel(string[] strings, int left = 10)
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Padding = new Thickness(left, 0, 0, 0);
      TextBlock tbUnit1 = RunTextBlockInstance(strings[0]);
      tbUnit1.Margin = new Thickness(0, 7, 0, 0);
      stackPanel.Children.Add(tbUnit1);
      for (int i = 1; i <= 4; i++)
      {
        stackPanel.Children.Add(RunTextBlockInstance(strings[i]));
      }
      return stackPanel;
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
    public StackPanel RunSliderPanel()
    {
      StackPanel stackPanel = new StackPanel();
      stackPanel.Padding = new Thickness(270, 0, 0, 0);
      Slider slider1 = new Slider();
      slider1.Width = 80;
      slider1.Height = 27;
      slider1.Value = 90.5;
      slider1.Margin = new Thickness(0, 9, 0, 0);
      stackPanel.Children.Add(slider1);
      Slider slider2 = new Slider();
      slider2.Width = 80;
      slider2.Height = 27;
      slider2.Value = 90.5;
      slider2.Margin = new Thickness(0, 0, 0, 0);
      stackPanel.Children.Add(slider2);
      Slider slider3 = new Slider();
      slider3.Width = 80;
      slider3.Height = 27;
      slider3.Value = 90.5;
      slider3.Margin = new Thickness(0, 0, 0, 0);
      stackPanel.Children.Add(slider3);
      Slider slider4 = new Slider();
      slider4.Width = 80;
      slider4.Height = 27;
      slider4.Value = 90.5;
      slider4.Margin = new Thickness(0, 0, 0, 0);
      stackPanel.Children.Add(slider4);
      Slider slider5 = new Slider();
      slider5.Width = 80;
      slider5.Height = 27;
      slider5.Value = 90.5;
      slider5.Margin = new Thickness(0, 0, 0, 0);
      stackPanel.Children.Add(slider5);
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
  }
}
