using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Univ.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Univ.NsMenu
{
  internal class MenuItem : Menu23Stack
  {
    //topArrowSelect_, secArrowSelect__, itemArrowSelect_ は基底クラスに定義されている。
    //クラスが分かれているため、列挙体は使用しない。
    const int kTopArrowSelectKindOrder = 0;//種類順
    const int kTopArrowSelectAbcOrder  = 1;//あいうえお順

    Grid parent_;
    MenuStatus menuStatus_;

    Data.Status[] friends_ = Data.DataSC.Friends();
    Data.StatusWritable[] friendsWritable_;

    int itemArrowSelect_ = -1;

    StackPanel stackPanel_;

    public MenuItem(Grid parent, Data.StatusWritable[] friendsWritable)
    {
      parent_ = parent;
      friendsWritable_ = friendsWritable;
    }
    public bool Notify(NotifyCode notifyCode)
    {
      switch (notifyCode)
      {
        case NotifyCode.Ok:
          if (tbTopArrows_[topArrowSelect_].Foreground == kCurrentBrush_)
          {
            tbTopArrows_[topArrowSelect_].Foreground = kSelectedBrush_;
            tbSecArrows_[secArrowSelect_].Foreground = kCurrentBrush_;
          }
          else if (tbSecArrows_[secArrowSelect_].Foreground == kCurrentBrush_)
          {
            tbSecArrows_[secArrowSelect_].Foreground = kSelectedBrush_;
            itemArrowSelect_ = 0;
            tbItemArrows_[itemArrowSelect_].Foreground = kCurrentBrush_;
          }
          else
          {
            /*int equiped = friends_[topArrowSelect_].GetEquipId((Data.EquipCategory)secArrowSelect__);
            int arrowEqId = viewPositionToEquipId_[itemArrowSelect_];// itemArrowSelect_ - 1;
            // はずすを含む装備する条件は、キャラ装備と選択装備が異なる。
            if (equiped != arrowEqId)
            {
              // ■装備
              int before = friendsWritable_[topArrowSelect_].Equip(
                (Data.EquipCategory)secArrowSelect__, arrowEqId);
              friendsWritable_[topArrowSelect_].ResetStatus();
              Data.Bag bag = Data.Bag.Instance;
              if (before >= 0)
              {
                bag.equipPlus((Data.EquipCategory)secArrowSelect__, before);
              }
              if (arrowEqId >= 0)
              {
                bag.equipPlus((Data.EquipCategory)secArrowSelect__, arrowEqId, -1);
              }
              menuStatus_.EquipChange(topArrowSelect_);
              UpdateView();
              tbSecArrows_[secArrowSelect__].Foreground = kCurrentBrush_;
            }*/
          }
          break;
        case NotifyCode.Cancel:
          if (itemArrowSelect_ >= 0)
          {
            tbSecArrows_[secArrowSelect_].Foreground = kCurrentBrush_;
            tbItemArrows_[itemArrowSelect_].Foreground = kErasedBrush_;
            itemArrowSelect_ = -1;
          }
          else if (tbSecArrows_[secArrowSelect_].Foreground == kCurrentBrush_)
          {
            tbTopArrows_[topArrowSelect_].Foreground = kCurrentBrush_;
            tbSecArrows_[secArrowSelect_].Foreground = kSelectedBrush_;
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
    void NotifyItem(int viewItemId)
    {
      tbTopArrows_[topArrowSelect_].Foreground = kSelectedBrush_;
      tbSecArrows_[secArrowSelect_].Foreground = kSelectedBrush_;
    }
    class Cells
    {
      Grid view_;
      Data.Bag bag_ = Data.DataSC.Bag();
      bool isEquip_;
      Data.ConstItem[] items_;
      Data.ConstStatus[] equips_ = null;
      public int length_ { get; private set; }
      Data.EquipCategory equipCategory_;
      public Cells(int topArrowSelect_, int secArrowSelect_, Grid view)
      {
        bool isAbcSort = topArrowSelect_ == kTopArrowSelectAbcOrder;
        items_ = ConstItem.ArrayForView(isAbcSort);
        length_ = items_.Length;
        isEquip_ = 0 < secArrowSelect_ && secArrowSelect_ <= 6;
        if (isEquip_)
        {
          equipCategory_ = (Data.EquipCategory)secArrowSelect_ - 1;
          equips_ = Data.DataSC.Equips(equipCategory_);
          equips_ = ConstStatus.ArrayForView(equips_, equipCategory_, isAbcSort);
          length_ = equips_.Length;
        }

        this.view_ = view;
      }
      public Border Add(int i, int nameLeft, int placeY, int numLeft)
      {
        int intNum = isEquip_ ?
          bag_.equip(equipCategory_, equips_[i].Id) :
          bag_.item(items_[i].Id);
        Border bdr;
        if (isEquip_)
        { //装備を一覧表示する
          bdr = UnivLib.WrapBorder(MenuUI.RunLavel(null, 0, 0, equips_[i].name()), view_, 155, nameLeft, placeY * 25);
          MenuUI.RunLavelRightAligned(view_, numLeft, placeY * 25, 33, intNum.ToString());
        }
        else
        { //「アイテム」または「だいじなもの」を一覧表示する
          bdr = UnivLib.WrapBorder(MenuUI.RunLavel(null, 0, 0, items_[i].Name), view_, 155, nameLeft, placeY * 25);
          MenuUI.RunLavelRightAligned(view_, numLeft, placeY * 25, 33, intNum.ToString());
        }
        return bdr;
      }
    }// class Cells
    protected override void UpdateView()
    {
      void SetTapEvent(Border bdrEvent, int viewItemId)
      {
        bdrEvent.Tapped += (Object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) =>
        {
          if (itemArrowSelect_ >= 0)
          {
            if (itemArrowSelect_ < tbItemArrows_.Length)
              tbItemArrows_[itemArrowSelect_].Foreground = kErasedBrush_;
          }
          tbItemArrows_[viewItemId].Foreground = kCurrentBrush_;
          itemArrowSelect_ = viewItemId;
          NotifyItem(viewItemId);
        };
      }
      view_.Children.Clear();

      tbItemArrows_ = new TextBlock[0];

      Cells cells = new Cells(topArrowSelect_, secArrowSelect_, view_);
      int half = 515 / 2;
      int viewPos = 0;
      for (int i = 0; i < cells.length_; i++)
      {
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
        tbItemArrows_[viewPos].Foreground = kErasedBrush_;
        SetTapEvent(cells.Add(i, nameLeft, placeY, numLeft), viewPos);
        //Array.Resize(ref this.viewPositionToEquipId_, this.viewPositionToEquipId_.Length + 1);
        //this.viewPositionToEquipId_[viewPos] = i;
        viewPos++;
      }
    }
    public void Create()
    {
      stackPanel_ = GetStackPanel(parent_, 0, 0, UnivLib.GetBrush(5, 50, 70),
        529, 519, kSelectedBrush_);

      //▲▲▲トップ（キャラ選択）▲▲▲
      StackPanel top = GetStackPanel(stackPanel_, 5, 15, UnivLib.GetBrush(0x63, 0x42, 0x42),
        515, 29, kErasedBrush_);
      string[] names = { "種類順", "あいうえお順" };
      tbTopArrows_ = new TextBlock[friends_.Length];
      topArrowSelect_ = 0;
      CreateGroup(top, names, tbTopArrows_);
      //▼▼▼トップ（キャラ選択）▼▼▼

      //▲▲▲セカンド（装備種類選択）▲▲▲
      StackPanel sec = GetStackPanel(stackPanel_, 5, 10, UnivLib.GetBrush(0x63, 0x42, 0x42),
        515, 29, kErasedBrush_);
      string[] secNames = { "アイテム", "武器", "体", "頭", "腕", "外装", "装飾", "その他" };
      tbSecArrows_ = new TextBlock[secNames.Length];
      CreateGroup(sec, secNames, tbSecArrows_, false);

      //NotifyTop(topArrowSelect_);
      //▼▼▼セカンド（装備種類選択）▼▼▼

      //▲▲▲ビュー（装備選択）▲▲▲
      view_ = CreateView();
      NotifyTop(topArrowSelect_); //UpdateView();
      stackPanel_.Children.Add(view_);
      //▼▼▼ビュー（装備選択）▼▼▼

      //menuStatus_.SetTitle("現", "新");
      //menuStatus_.EquipChange(topArrowSelect_);
    }
    public void Destroy()
    {
      parent_.Children.Remove(stackPanel_);
    }
  }
}
