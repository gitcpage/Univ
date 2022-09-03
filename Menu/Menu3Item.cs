using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Univ.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Univ.NsMenu
{
  internal class MenuItem : Menu123Stack
  {
    //topArrowSelect_, secArrowSelect__, itemArrowSelect_ は基底クラスに定義されている。
    //クラスが分かれているため、列挙体は使用しない。
    //const int kTopArrowSelectKindOrder = 0;//種類順
    const int kTopArrowSelectAbcOrder  = 1;//あいうえお順

    //Grid parent_;
    //MenuStatus menuStatus_;

    Data.Status[] friends_ = Data.DataSC.Friends();
    Data.StatusWritable[] friendsWritable_;

    public MenuItem(Grid parent, Data.StatusWritable[] friendsWritable) : base(parent)
    {
      friendsWritable_ = friendsWritable;

      //▲▲▲Create▲▲▲
      //▲▲トップ（キャラ選択）▲▲
      StackPanel top = MenuUI.CreateStackPanel(wholePanel_, 5, 15, MenuUI.kChildPanelBorderBrush,
        515, 29, kErasedCursorBrush_);
      string[] names = { "種類順", "あいうえお順" };
      tbTopArrows_ = new TextBlock[friends_.Length];
      topArrowSelect_ = 0;
      CreateGroup(top, names, tbTopArrows_);
      //▼▼トップ（キャラ選択）▼▼

      //▲▲セカンド（装備種類選択）▲▲
      string[] secNames = { "アイテム", "武器", "体", "頭", "腕", "外装", "装飾", "その他" };
      base.CreateSecondPanel(secNames);

      //NotifyTop(topArrowSelect_);
      //▼▼セカンド（装備種類選択）▼▼

      //▲▲ビュー（装備選択）▲▲
      view_ = CreateView();
      NotifyTop(topArrowSelect_); //UpdateView();
      wholePanel_.Children.Add(view_);
      //▼▼ビュー（装備選択）▼▼

      //menuStatus_.SetTitle("現", "新");
      //menuStatus_.EquipChange(topArrowSelect_);
      //▼▼▼Create▼▼▼
    }
    public override bool Notify(NotifyCode notifyCode)
    {
      switch (notifyCode)
      {
        case NotifyCode.Ok:
          if (tbTopArrows_[topArrowSelect_].Foreground == kCurrentCursorBrush_)
          {
            tbTopArrows_[topArrowSelect_].Foreground = kSelectedCursorBrush_;
            tbSecArrows_[secArrowSelect_].Foreground = kCurrentCursorBrush_;
          }
          else if (tbSecArrows_[secArrowSelect_].Foreground == kCurrentCursorBrush_)
          {
            tbSecArrows_[secArrowSelect_].Foreground = kSelectedCursorBrush_;
            itemArrowSelect_ = 0;
            tbItemArrows_[itemArrowSelect_].Foreground = kCurrentCursorBrush_;
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
              tbSecArrows_[secArrowSelect__].Foreground = kCurrentCursorBrush_;
            }*/
          }
          break;
        case NotifyCode.Cancel:
          if (itemArrowSelect_ >= 0)
          {
            tbSecArrows_[secArrowSelect_].Foreground = kCurrentCursorBrush_;
            tbItemArrows_[itemArrowSelect_].Foreground = kErasedCursorBrush_;
            itemArrowSelect_ = -1;
          }
          else if (tbSecArrows_[secArrowSelect_].Foreground == kCurrentCursorBrush_)
          {
            tbTopArrows_[topArrowSelect_].Foreground = kCurrentCursorBrush_;
            tbSecArrows_[secArrowSelect_].Foreground = kSelectedCursorBrush_;
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
    protected override void NotifyItem(int viewItemId)
    {
      tbTopArrows_[topArrowSelect_].Foreground = kSelectedCursorBrush_;
      tbSecArrows_[secArrowSelect_].Foreground = kSelectedCursorBrush_;
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
      public string GetName(int i)
      {
        if (isEquip_)
        { //装備を一覧表示する
          return equips_[i].name();
        }
        else
        { //「アイテム」または「だいじなもの」を一覧表示する
          return items_[i].Name;
        }
      }
      public string GetNum(int i)
      {
        int intNum = isEquip_ ?
          bag_.equip(equipCategory_, equips_[i].Id) :
          bag_.item(items_[i].Id);
        return intNum.ToString();
      }
    }// class Cells
    protected override void UpdateView()
    {
      view_.Children.Clear();

      tbItemArrows_ = new TextBlock[0];

      Cells cells = new Cells(topArrowSelect_, secArrowSelect_, view_);
      int viewPos = 0;
      for (int i = 0; i < cells.length_; i++)
      {
        string name = cells.GetName(i);
        string num = cells.GetNum(i);
        AddViewItem(viewPos, name, num);
        viewPos++;
      }
    }
    public override int Destroy()
    {
      parent_.Children.Remove(wholePanel_);
      return 0;
    }
  }
}
