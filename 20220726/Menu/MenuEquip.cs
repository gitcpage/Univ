using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml; // VerticalAlignment, HorizontalAlignment
using Windows.UI.Xaml.Controls; // TextBlock
using Windows.UI.Xaml.Media; // FontFamily

namespace Univ.NsMenu
{
  internal class MenuEquip
  {
    Grid parent_;
    MenuStatus menuStatus_;
    Data.StatusWritable[] charsWritable_;

    Brush currentBrush_; // fgBrush_
    Brush selectedBrush_;// bgBrush_
    Brush erasedBrush_;  // groupBgBrush_

    TextBlock[] tbTopArrows_;
    int topArrowSelect_;

    TextBlock[] tbSecArrows_;
    int secArrowSelect_;

    TextBlock[] tbItemArrows_;
    int itemArrowSelect_;
    int[] viewPositionToEquipId_ = { };

    Data.Status[] statuses_;

    StackPanel equipPanel_;
    Grid view_;

    public MenuEquip(Grid parent, MenuStatus menuStatus, Data.StatusWritable[] charsWritable)
    {
      parent_ = parent;
      menuStatus_ = menuStatus;
      charsWritable_ = charsWritable;

      currentBrush_ = UnivLib.GetBrush(14, 77, 108);
      selectedBrush_ = UnivLib.GetBrush(157, 181, 183);
      erasedBrush_ = UnivLib.GetBrush(0xf3, 0xe4, 0xd5);
      secArrowSelect_ = topArrowSelect_ = 0;
      itemArrowSelect_ = -1;

      statuses_ = Data.Status.Instances;
    }
    void NotifyTop(int id)
    {
      if (id != topArrowSelect_)
        menuStatus_.EquipChange(id);
      topArrowSelect_ = id;
      tbSecArrows_[secArrowSelect_].Foreground = selectedBrush_;
      if (itemArrowSelect_ >= 0)
      {
        if (itemArrowSelect_ > tbItemArrows_.Length)
          tbItemArrows_[itemArrowSelect_].Foreground = erasedBrush_;
      }
      UpdateView();
      menuStatus_.EquipChange(topArrowSelect_);
    }
    void NotifySec(int id)
    {
      tbTopArrows_[topArrowSelect_].Foreground = selectedBrush_;
      if (itemArrowSelect_ >= 0)
      {
        if (itemArrowSelect_ > tbItemArrows_.Length)
          tbItemArrows_[itemArrowSelect_].Foreground = erasedBrush_;
      }
      UpdateView();
      menuStatus_.EquipChange(topArrowSelect_);
    }
    void NotifyItem(int viewItemId)
    {
      tbTopArrows_[topArrowSelect_].Foreground = selectedBrush_;
      tbSecArrows_[secArrowSelect_].Foreground = selectedBrush_;
      menuStatus_.EquipChangeCalc(topArrowSelect_, secArrowSelect_, viewItemId-1);
    }
    // 戻り値：装備ウィンドウを終了する場合は true を返す。
    public bool Notify(NotifyCode notifyCode)
    {
      switch (notifyCode)
      {
        case NotifyCode.Ok:
          if (tbTopArrows_[topArrowSelect_].Foreground == currentBrush_)
          {
            tbTopArrows_[topArrowSelect_].Foreground = selectedBrush_;
            tbSecArrows_[secArrowSelect_].Foreground = currentBrush_;
          }
          else if (tbSecArrows_[secArrowSelect_].Foreground == currentBrush_)
          {
            tbSecArrows_[secArrowSelect_].Foreground = selectedBrush_;
            itemArrowSelect_ = 0;
            tbItemArrows_[itemArrowSelect_].Foreground = currentBrush_;
          }
          else
          {
            int equiped = statuses_[topArrowSelect_].GetEquipId((Data.EquipCategory)secArrowSelect_);
            int arrowEqId = viewPositionToEquipId_[itemArrowSelect_];// itemArrowSelect_ - 1;
            // はずすを含む装備する条件は、キャラ装備と選択装備が異なる。
            if (equiped != arrowEqId)
            {
              // ■装備
              int before = charsWritable_[topArrowSelect_].Equip(
                (Data.EquipCategory)secArrowSelect_, arrowEqId);
              charsWritable_[topArrowSelect_].ResetStatus();
              Data.Bag bag = Data.Bag.Instance;
              if (before >= 0)
              {
                bag.equipPlus((Data.EquipCategory)secArrowSelect_, before);
              }
              if (arrowEqId >= 0)
              {
                bag.equipPlus((Data.EquipCategory)secArrowSelect_, arrowEqId, -1);
              }
              menuStatus_.EquipChange(topArrowSelect_);
              UpdateView();
              tbSecArrows_[secArrowSelect_].Foreground = currentBrush_;
            }
          }
          break;
        case NotifyCode.Cancel:
          if (itemArrowSelect_ >= 0)
          {
            tbSecArrows_[secArrowSelect_].Foreground = currentBrush_;
            tbItemArrows_[itemArrowSelect_].Foreground = erasedBrush_;
            itemArrowSelect_ = -1;
          }
          else if (tbSecArrows_[secArrowSelect_].Foreground == currentBrush_)
          {
            tbTopArrows_[topArrowSelect_].Foreground = currentBrush_;
            tbSecArrows_[secArrowSelect_].Foreground = selectedBrush_;
          }
          else
          {
            // ■戻る
            return true;
          }
          break;
      }
      return false;
    }
    StackPanel GetStackPanel(Panel parent, int margin, int marginTop, Brush border, 
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
    void CreateGroup(StackPanel parent, string[] items, TextBlock[] arrows, bool isTop = true)
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
        tbArrow.Foreground = erasedBrush_;
        tbArrow.FontSize = 16;
        parent.Children.Add(tbArrow);
        arrows[i] = tbArrow;
        tbs[i * 2] = tbArrow;

        TextBlock tbChar = new TextBlock();
        tbChar.VerticalAlignment = VerticalAlignment.Center;
        tbChar.Text = items[i] + "　";
        tbChar.Padding = new Thickness(0, 0, 10, 0);
        tbChar.Foreground = currentBrush_;
        tbChar.FontSize = 16;
        Border bdrChar = UnivLib.WrapBorder(tbChar, parent);
        //parent.Children.Add(bdrChar);
        tbs[i * 2 + 1] = tbChar;
        int hold = i;
        bdrChar.Tapped += (Object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) =>
        {
          if (isTop)
          {
            arrows[topArrowSelect_].Foreground = erasedBrush_;
            arrows[hold].Foreground = currentBrush_;
            NotifyTop(hold);
          }
          else
          {
            arrows[secArrowSelect_].Foreground = erasedBrush_;
            arrows[hold].Foreground = currentBrush_;
            secArrowSelect_ = hold;
            NotifySec(hold);
          }
        };
      }
      UnivLib.MeasureWidth(tbs, parent);
      arrows[isTop ? topArrowSelect_ : secArrowSelect_].Foreground = currentBrush_;
    }
    void UpdateView()
    {
      void SetTapEvent(Border bdrEvent, int viewItemId)
      {
        bdrEvent.Tapped += (Object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) =>
        {
          if (itemArrowSelect_ >= 0)
          {
            if (itemArrowSelect_ < tbItemArrows_.Length)
              tbItemArrows_[itemArrowSelect_].Foreground = erasedBrush_;
          }
          tbItemArrows_[viewItemId].Foreground = currentBrush_;
          itemArrowSelect_ = viewItemId;
          NotifyItem(viewItemId);
        };
      }

      view_.Children.Clear();
      Data.Loader loader = Data.Loader.Instance;
      Data.ConstStatus[] eqs = loader.EquipArray((Data.EquipCategory)secArrowSelect_);

      // ▲▲▲はずす▲▲▲
      tbItemArrows_ = new TextBlock[1];
      tbItemArrows_[0] = MenuUI.RunLavel(view_, 5, 0, "➤");
      tbItemArrows_[0].Foreground = erasedBrush_;
      Border bdr = UnivLib.WrapBorder(MenuUI.RunLavel(null, 30, 0, "はずす"), view_, 155, 0, 0);
      SetTapEvent(bdr, 0);
      MenuUI.RunLavelRightAligned(view_, 190, 0, 33, "-".ToString());
      // ▼▼▼はずす▼▼▼

      int half = 515 / 2;
      Data.Bag bag = Data.Bag.Instance;
      int eqId = statuses_[topArrowSelect_].GetEquipId((Data.EquipCategory)secArrowSelect_);
      Array.Resize(ref this.viewPositionToEquipId_, 1);
      viewPositionToEquipId_[0] = -1; // はずす分
      int viewPos = 1;
      for (int i = 0; i < eqs.Length; i++)
      {
        string num = eqId == i ? "E" : bag.equip((Data.EquipCategory)secArrowSelect_, i).ToString();
        if (num == "0") continue;
        int placeY = viewPos / 2;
        int arrowLeft = 5;
        int nameLeft = 30;
        int numLeft = 190;
        if (viewPos % 2 != 0)
        {
          arrowLeft += half;
          nameLeft += half;
          numLeft += half;
        }
        Array.Resize(ref tbItemArrows_, tbItemArrows_.Length + 1);
        tbItemArrows_[viewPos] = MenuUI.RunLavel(view_, arrowLeft, placeY * 25, "➤");
        tbItemArrows_[viewPos].Foreground = erasedBrush_;
        bdr = UnivLib.WrapBorder(MenuUI.RunLavel(null, 0, 0, eqs[i].name), view_, 155, nameLeft, placeY * 25);
        MenuUI.RunLavelRightAligned(view_, numLeft, placeY * 25, 33, num);
        SetTapEvent(bdr, viewPos);
        Array.Resize(ref this.viewPositionToEquipId_, this.viewPositionToEquipId_.Length + 1);
        this.viewPositionToEquipId_[viewPos] = i;
        viewPos++;
      }
    }
    Grid CreateView()
    {
      Grid view = new Grid();
      view.HorizontalAlignment = HorizontalAlignment.Left;
      view.VerticalAlignment = VerticalAlignment.Top;
      view.Margin = new Thickness(5, 10, 5, 5);
      view.Width = 515;
      view.Height = 329;
      view.Background = erasedBrush_;
      return view;
    }
    public void Create(int charSelected03)
    {
      equipPanel_ = GetStackPanel(parent_, 0, 0, UnivLib.GetBrush(5, 50, 70),
        529, 519, selectedBrush_);

      //▲▲▲トップ（キャラ選択）▲▲▲
      StackPanel top = GetStackPanel(equipPanel_, 5, 15, UnivLib.GetBrush(0x63, 0x42, 0x42),
        515, 29, erasedBrush_);
      string[] names = new string[statuses_.Length];
      for (int i = 0; i < statuses_.Length; i++)
      {
        names[i] = statuses_[i].name();
      }
      tbTopArrows_ = new TextBlock[statuses_.Length];
      topArrowSelect_ = charSelected03;
      CreateGroup(top, names, tbTopArrows_);
      //▼▼▼トップ（キャラ選択）▼▼▼

      //▲▲▲セカンド（装備種類選択）▲▲▲
      StackPanel sec = GetStackPanel(equipPanel_, 5, 10, UnivLib.GetBrush(0x63, 0x42, 0x42),
        515, 29, erasedBrush_);
      string[] secNames = { "武器", "体", "頭", "腕", "外装", "装飾" };
      tbSecArrows_ = new TextBlock[secNames.Length];
      CreateGroup(sec, secNames, tbSecArrows_, false);

      //NotifyTop(topArrowSelect_);
      //▼▼▼セカンド（装備種類選択）▼▼▼

      //▲▲▲ビュー（装備選択）▲▲▲
      view_ = CreateView();
      NotifyTop(topArrowSelect_); //UpdateView();
      equipPanel_.Children.Add(view_);
      //▼▼▼ビュー（装備選択）▼▼▼

      menuStatus_.SetTitle("現", "新");
      menuStatus_.EquipChange(topArrowSelect_);
    }

    // 戻り値：選択しているキャラクター番号を返す。
    public int Destroy()
    {
      parent_.Children.Remove(equipPanel_);
      return topArrowSelect_;
    }
  }
}
