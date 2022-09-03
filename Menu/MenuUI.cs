using System;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml; // Thickness
using Windows.UI.Xaml.Controls; // Grid
using Windows.UI.Xaml.Input; // PointerRoutedEventArgs
using Windows.UI.Xaml.Media; // Stretch

namespace Univ.NsMenu
{
  delegate void MenuNotify(NotifyCode notifyCode);
  internal class MenuUI
  {
    public static Brush kMonitorBgBrush = UnivLib.GetBrush(11, 70, 100);
    public static Brush kMainGridBrush = UnivLib.GetBrush(Colors.Orange);
    public static Brush kPanelBorderBrush = UnivLib.GetBrush(5, 50, 70);
    public static Brush kPanelBgBrush = UnivLib.GetBrush(157, 181, 183);
    public static Brush kPanelBgHoverBrush = UnivLib.GetBrush(187, 211, 213); //メインのフレンドホバー
    public static Brush kPanelBgTransparentBrush = UnivLib.GetBrush(0, 157, 181, 183);
    public static Brush kChildPanelBgBrush = UnivLib.GetBrush(0xf3, 0xe4, 0xd5);
    public static Brush kChildPanelBorderBrush = UnivLib.GetBrush(0x63, 0x42, 0x42);
    public static Brush kCursorBrush = UnivLib.GetBrush(14, 77, 108);
    public static Brush kCursorTranslucentBrush = UnivLib.GetBrush(128, 14, 77, 108);

    //Grid monitor_;   // 描画用
    //MenuNotify notify_;
    private MenuUI()//MenuNotify menuNotify)
    {
      //notify_ = menuNotify;
    }
    static public Grid RunGrid(int left, int top, int width, int height,int paddingLeft, int paddingTop, Grid parent, bool hasBorder = true, Brush bgBrush = null)
    {
      Grid g = new Grid();
      g.HorizontalAlignment = HorizontalAlignment.Left;
      g.VerticalAlignment = VerticalAlignment.Top;
      g.Margin = new Thickness(left, top, 0, 0);
      g.Width = width;
      g.Height = height;
      if (hasBorder)
      {
        g.BorderThickness = new Thickness(2, 2, 2, 2);
        g.BorderBrush = kPanelBorderBrush;
      }
      g.Padding = new Thickness(paddingLeft, paddingTop, 0, 0);
      if (bgBrush == null)
        g.Background = kPanelBgBrush;
      else
        g.Background = bgBrush;

      parent.Children.Add(g);
      return g;
    }
    static public Grid RunMainLeftItem(int top, string str, Grid parent,
      MenuNotify menuNotify, NotifyCode notifyCode)
    {
      Grid btn = new Grid();
      btn.Margin = new Thickness(0, top, 0, 0);
      btn.HorizontalAlignment = HorizontalAlignment.Left;
      btn.VerticalAlignment = VerticalAlignment.Top;
      btn.Background = kPanelBgTransparentBrush;//Tappedイベント検出のため
      btn.Width = 85;
      btn.Height = 55;
      btn.PointerEntered += (Object sender, PointerRoutedEventArgs e) =>
      {
        Grid o = sender as Grid;
        foreach(UIElement uie in o.Children)
        {
          Border bdr = uie as Border;
          if (bdr != null)
          {
            TextBlock tb = bdr.Child as TextBlock;
            if (tb.Text == str)
            {
              tb.TextDecorations = TextDecorations.Underline;
              return;
            }
          }
        }
      };
      btn.PointerExited += (Object sender, PointerRoutedEventArgs e) =>
      {
        Grid g = sender as Grid;
        foreach (UIElement uie in g.Children)
        {
          Border bdr = uie as Border;
          if (bdr != null)
          {
            TextBlock tb = bdr.Child as TextBlock;
            if (tb.Text == str)
            {
              // t.TextDecorations = TextDecorations.None; はバグで効かない。以下回避策。
              bdr.Child = MenuInclude.GetTextBlock(tb.Text);
              return;
            }
          }
        }
      };
      btn.Tapped += (Object sender, TappedRoutedEventArgs e) =>
      {
        menuNotify(notifyCode);
        Grid g = sender as Grid;
        foreach (UIElement uie in g.Children)
        {
          TextBlock tb = uie as TextBlock;
          if (tb != null)
          {
            tb.Text = "➤";
            return;
          }
        }
      };

      btn.Children.Add(MenuInclude.GetTextBlock("　 "));

      Border border = new Border();
      border.Padding = new Thickness(20, 0, 0, 0);
      border.Child = MenuInclude.GetTextBlock(str);
      btn.Children.Add(border);

      parent.Children.Add(btn);
      return btn;
    }
    static public TextBlock RunLavel(Panel parent, int x, int y, string text, 
      bool useMeiryo = false, int fontSize = 19, bool useBold = false)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock.VerticalAlignment = VerticalAlignment.Top;
      textBlock.Text = text;
      textBlock.Margin = new Thickness(x, y, 0, 0);
      textBlock.FontSize = fontSize;
      textBlock.Foreground = kCursorBrush;
      if (useMeiryo) textBlock.FontFamily = new FontFamily("メイリオ");
      if (useBold) textBlock.FontWeight = UnivLib.FontWeightBold();
      if (parent != null) parent.Children.Add(textBlock);
      return textBlock;
    }
    static public TextBlock RunLavelCenterAligned(Panel parent, int x, int y, int width, string text, int fontSize = 19, bool useBold = false)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock.VerticalAlignment = VerticalAlignment.Top;
      textBlock.HorizontalTextAlignment = TextAlignment.Center;
      textBlock.Width = width;
      textBlock.Text = text;
      textBlock.Margin = new Thickness(x, y, 0, 0);
      textBlock.FontSize = fontSize;
      textBlock.Foreground = kCursorBrush;
      if (useBold) textBlock.FontWeight = UnivLib.FontWeightBold();
      parent.Children.Add(textBlock);
      return textBlock;
    }
    static public TextBlock RunLavelRightAligned(Panel parent, int x, int y, int width, string text, int fontSize = 19)
    {
      TextBlock textBlock = new TextBlock();
      textBlock.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock.VerticalAlignment = VerticalAlignment.Top;
      textBlock.HorizontalTextAlignment = TextAlignment.Right;
      textBlock.Width = width;
      textBlock.Text = text;
      textBlock.Margin = new Thickness(x, y, 0, 0);
      textBlock.FontSize = fontSize;
      textBlock.Foreground = kCursorBrush;
      parent.Children.Add(textBlock);
      return textBlock;
    }
    static public Border RunButton(int left, int top, string str, Panel parent,
      MenuNotify menuNotify, NotifyCode notifyCode,
      int width = 100)
    {
      Border btn = new Border();
      btn.Margin = new Thickness(left, top, 0, 0);
      btn.HorizontalAlignment = HorizontalAlignment.Left;
      btn.VerticalAlignment = VerticalAlignment.Top;
      btn.Background = UnivLib.GetBrush(Colors.White);
      btn.BorderThickness = new Thickness(2, 2, 2, 2);
      btn.BorderBrush = UnivLib.GetBrush(Colors.Gray);
      btn.Width = width;
      btn.Height = 30;
      btn.PointerEntered += (Object obj, PointerRoutedEventArgs e) =>
      {
        Border o = obj as Border;
        o.Background = UnivLib.GetBrush(Colors.LightGray);
      };
      btn.PointerExited += (Object obj, PointerRoutedEventArgs e) =>
      {
        Border o = obj as Border;
        o.Background = UnivLib.GetBrush(Colors.White);
      };
      btn.Tapped += (object sender, TappedRoutedEventArgs e) =>
      {
        menuNotify(notifyCode);
      };
      TextBlock tb = new TextBlock();
      tb.Text = str;
      tb.VerticalAlignment = VerticalAlignment.Center;
      tb.HorizontalAlignment = HorizontalAlignment.Center;
      tb.FontSize = 18;
      btn.Child = tb;

      parent.Children.Add(btn);
      return btn;
    }
    public static StackPanel CreateStackPanel(Panel parent, int margin, int marginTop, Brush border,
      int width, int height, Brush bg)
    {
      StackPanel g = new StackPanel();
      g.HorizontalAlignment = HorizontalAlignment.Left;
      g.VerticalAlignment = VerticalAlignment.Top;
      g.Margin = new Thickness(margin, marginTop, margin, margin);
      g.Width = width;
      g.Height = height;
      if (border != null)
      {
        g.BorderThickness = new Thickness(2, 2, 2, 2);
        g.BorderBrush = border;
      }
      g.Background = bg;
      parent.Children.Add(g);
      return g;
    }
  }
}
