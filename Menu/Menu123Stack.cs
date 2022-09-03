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
  internal abstract class Menu123Stack
  {
    protected readonly Grid parent_;

    protected readonly Brush kCurrentCursorBrush_ = MenuUI.kCursorBrush;
    protected readonly Brush kSelectedCursorBrush_ = MenuUI.kPanelBgBrush;
    protected readonly Brush kErasedCursorBrush_ = MenuUI.kChildPanelBgBrush;

    protected TextBlock[] tbTopArrows_;
    protected int topArrowSelect_ = 0;

    protected TextBlock[] tbSecArrows_;
    protected int secArrowSelect_ = 0;

    protected TextBlock[] tbItemArrows_;
    protected int itemArrowSelect_ = -1;

    protected Grid view_;

    protected StackPanel wholePanel_;

    public Menu123Stack(Grid parent)
    {
      parent_ = parent;
      wholePanel_ = MenuUI.CreateStackPanel(parent_, 0, 0, MenuUI.kPanelBorderBrush,
        529, 519, kSelectedCursorBrush_);
    }
    protected virtual void NotifyTop(int id)
    {
      topArrowSelect_ = id;
      tbSecArrows_[secArrowSelect_].Foreground = kSelectedCursorBrush_;
      if (itemArrowSelect_ >= 0)
      {
        if (itemArrowSelect_ > tbItemArrows_.Length)
          tbItemArrows_[itemArrowSelect_].Foreground = kErasedCursorBrush_;
      }
      UpdateView();
    }
    protected virtual void NotifySec(int id)
    {
      tbTopArrows_[topArrowSelect_].Foreground = kSelectedCursorBrush_;
      if (itemArrowSelect_ >= 0)
      {
        if (itemArrowSelect_ > tbItemArrows_.Length)
          tbItemArrows_[itemArrowSelect_].Foreground = kErasedCursorBrush_;
      }
      UpdateView();
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
        tbArrow.Foreground = kErasedCursorBrush_;
        tbArrow.FontSize = 16;
        parent.Children.Add(tbArrow);
        arrows[i] = tbArrow;
        tbs[i * 2] = tbArrow;

        TextBlock tbChar = new TextBlock();
        tbChar.VerticalAlignment = VerticalAlignment.Center;
        tbChar.Text = items[i] + "　";
        tbChar.Padding = new Thickness(0, 0, 10, 0);
        tbChar.Foreground = kCurrentCursorBrush_;
        tbChar.FontSize = 16;
        Border bdrChar = UnivLib.WrapBorder(tbChar, parent);
        //parent.Children.Add(bdrChar);
        tbs[i * 2 + 1] = tbChar;
        int hold = i;
        bdrChar.Tapped += (Object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) =>
        {
          if (isTop)
          {
            arrows[topArrowSelect_].Foreground = kErasedCursorBrush_;
            arrows[hold].Foreground = kCurrentCursorBrush_;
            NotifyTop(hold);
          }
          else
          {
            arrows[secArrowSelect_].Foreground = kErasedCursorBrush_;
            arrows[hold].Foreground = kCurrentCursorBrush_;
            secArrowSelect_ = hold;
            NotifySec(hold);
          }
        };
      }
      UnivLib.MeasureWidth(tbs, parent);
      arrows[isTop ? topArrowSelect_ : secArrowSelect_].Foreground = kCurrentCursorBrush_;
    }
    protected void CreateTopFriends(int friendSelected)
    {
      StackPanel top = MenuUI.CreateStackPanel(wholePanel_, 5, 15, MenuUI.kChildPanelBorderBrush,
        515, 29, kErasedCursorBrush_);
      Data.Status[] friends = Data.DataSC.Friends();
      string[] names = new string[friends.Length];
      for (int i = 0; i < friends.Length; i++)
      {
        names[i] = friends[i].name();
      }
      tbTopArrows_ = new TextBlock[friends.Length];
      topArrowSelect_ = friendSelected;
      CreateGroup(top, names, tbTopArrows_);
    }
    protected void CreateSecondPanel(string[] secNames)
    {
      StackPanel sec = MenuUI.CreateStackPanel(wholePanel_, 5, 10, MenuUI.kChildPanelBorderBrush,
        515, 29, kErasedCursorBrush_);
      tbSecArrows_ = new TextBlock[secNames.Length];
      CreateGroup(sec, secNames, tbSecArrows_, false);
    }
    protected abstract void NotifyItem(int viewItemId);
    public abstract bool Notify(NotifyCode notifyCode);

    //△△△UpdateViewメソッドとその内部から呼び出されるメソッド△△△
    //UpdateView()内で使用される項目をタップ時に設定されるイベント。
    protected void SetTapEvent(Border bdrEvent, int viewItemId)
    {
      bdrEvent.Tapped += (Object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) =>
      {
        if (itemArrowSelect_ >= 0)
        {
          if (itemArrowSelect_ < tbItemArrows_.Length)
            tbItemArrows_[itemArrowSelect_].Foreground = kErasedCursorBrush_;
        }
        tbItemArrows_[viewItemId].Foreground = kCurrentCursorBrush_;
        itemArrowSelect_ = viewItemId;
        NotifyItem(viewItemId);
      };
    }
    protected void AddViewItem(int viewPos, string name, string num)
    {
      int placeY = viewPos / 2;
      int arrowLeft = 5;
      int nameLeft = 30;
      int numLeft = 190;
      if (viewPos % 2 != 0)
      {
        int half = 515 / 2;
        arrowLeft += half;
        nameLeft += half;
        numLeft += half;
      }
      Array.Resize(ref tbItemArrows_, tbItemArrows_.Length + 1);
      tbItemArrows_[viewPos] = MenuUI.RunLavel(view_, arrowLeft, placeY * 25, "➤");
      tbItemArrows_[viewPos].Foreground = kErasedCursorBrush_;
      SetTapEvent(
        UnivLib.WrapBorder(
          MenuUI.RunLavel(null, 0, 0, name),
          view_, 155, nameLeft, placeY * 25),
        viewPos);
      MenuUI.RunLavelRightAligned(view_, numLeft, placeY * 25, 33, num);
    }
    protected abstract void UpdateView();
    //▽▽▽UpdateViewメソッドとその内部から呼び出されるメソッド▽▽▽

    protected Grid CreateView()
    {
      Grid view = new Grid();
      view.HorizontalAlignment = HorizontalAlignment.Left;
      view.VerticalAlignment = VerticalAlignment.Top;
      view.Margin = new Thickness(5, 10, 5, 5);
      view.Width = 515;
      view.Height = 329;
      view.Background = kErasedCursorBrush_;
      return view;
    }
    //戻り値：選択されているキャラクターID
    public abstract int Destroy();
  }
}
