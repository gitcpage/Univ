using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;

namespace Univ.NsMenu
{
  internal abstract class Menu23Stack
  {
    protected readonly Brush kCurrentBrush_ = UnivLib.GetBrush(14, 77, 108);
    protected readonly Brush kSelectedBrush_ = UnivLib.GetBrush(157, 181, 183);
    protected readonly Brush kErasedBrush_ = UnivLib.GetBrush(0xf3, 0xe4, 0xd5);

    protected TextBlock[] tbTopArrows_;
    protected int topArrowSelect_ = 0;

    protected TextBlock[] tbSecArrows_;
    protected int secArrowSelect_ = 0;

    protected TextBlock[] tbItemArrows_;
    protected int itemArrowSelect_ = -1;

    protected Grid view_;

    protected void NotifyTop(int id)
    {
      //if (id != topArrowSelect_)
      //  menuStatus_.EquipChange(id);
      topArrowSelect_ = id;
      tbSecArrows_[secArrowSelect_].Foreground = kSelectedBrush_;
      if (itemArrowSelect_ >= 0)
      {
        if (itemArrowSelect_ > tbItemArrows_.Length)
          tbItemArrows_[itemArrowSelect_].Foreground = kErasedBrush_;
      }
      UpdateView();
      //menuStatus_.EquipChange(topArrowSelect_);
    }
    void NotifySec(int id)
    {
      tbTopArrows_[topArrowSelect_].Foreground = kSelectedBrush_;
      if (itemArrowSelect_ >= 0)
      {
        if (itemArrowSelect_ > tbItemArrows_.Length)
          tbItemArrows_[itemArrowSelect_].Foreground = kErasedBrush_;
      }
      UpdateView();
      //menuStatus_.EquipChange(topArrowSelect_);
    }
    protected StackPanel GetStackPanel(Panel parent, int margin, int marginTop, Brush border,
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
    protected void CreateGroup(StackPanel parent, string[] items, TextBlock[] arrows, bool isTop = true)
    {
      parent.Orientation = Orientation.Horizontal;
      parent.HorizontalAlignment = HorizontalAlignment.Center;
      parent.VerticalAlignment = VerticalAlignment.Center;

      TextBlock[] tbs = new TextBlock[items.Length * 2];
      for (int i = 0; i < items.Length; i++)
      {
        TextBlock tbArrow = new TextBlock();
        tbArrow.VerticalAlignment = VerticalAlignment.Center;
        tbArrow.Text = "➤";
        tbArrow.Foreground = kErasedBrush_;
        tbArrow.FontSize = 16;
        parent.Children.Add(tbArrow);
        arrows[i] = tbArrow;
        tbs[i * 2] = tbArrow;

        TextBlock tbChar = new TextBlock();
        tbChar.VerticalAlignment = VerticalAlignment.Center;
        tbChar.Text = items[i] + "　";
        tbChar.Padding = new Thickness(0, 0, 10, 0);
        tbChar.Foreground = kCurrentBrush_;
        tbChar.FontSize = 16;
        Border bdrChar = UnivLib.WrapBorder(tbChar, parent);
        //parent.Children.Add(bdrChar);
        tbs[i * 2 + 1] = tbChar;
        int hold = i;
        bdrChar.Tapped += (Object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) =>
        {
          if (isTop)
          {
            arrows[topArrowSelect_].Foreground = kErasedBrush_;
            arrows[hold].Foreground = kCurrentBrush_;
            NotifyTop(hold);
          }
          else
          {
            arrows[secArrowSelect_].Foreground = kErasedBrush_;
            arrows[hold].Foreground = kCurrentBrush_;
            secArrowSelect_ = hold;
            NotifySec(hold);
          }
        };
      }
      UnivLib.MeasureWidth(tbs, parent);
      arrows[isTop ? topArrowSelect_ : secArrowSelect_].Foreground = kCurrentBrush_;
    }
    protected abstract void UpdateView();
    protected Grid CreateView()
    {
      Grid view = new Grid();
      view.HorizontalAlignment = HorizontalAlignment.Left;
      view.VerticalAlignment = VerticalAlignment.Top;
      view.Margin = new Thickness(5, 10, 5, 5);
      view.Width = 515;
      view.Height = 329;
      view.Background = kErasedBrush_;
      return view;
    }
  }
}
