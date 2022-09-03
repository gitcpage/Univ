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
  internal class MenuEquip : Menu123Stack
  {
    MenuStatus menuStatus_;

    Data.Status[] friends_ = Data.DataSC.Friends();
    Data.StatusWritable[] friendsWritable_;
    int[] viewPositionToEquipId_ = { };

    public MenuEquip(Grid parent, MenuStatus menuStatus, Data.StatusWritable[] friendsWritable,
      int friendSelected) : base(parent)
    {
      menuStatus_ = menuStatus;
      friendsWritable_ = friendsWritable;

      //▲▲▲Create▲▲▲
      //▲▲トップ（キャラ選択）▲▲
      base.CreateTopFriends(friendSelected);
      //▼▼トップ（キャラ選択）▼▼

      //▲▲セカンド（装備種類選択）▲▲
      string[] secNames = { "武器", "体", "頭", "腕", "外装", "装飾" };
      base.CreateSecondPanel(secNames);
      //▼▼セカンド（装備種類選択）▼▼

      //▲▲ビュー（装備選択）▲▲
      view_ = CreateView();
      base.NotifyTop(topArrowSelect_); //UpdateView();
      wholePanel_.Children.Add(view_);
      //▼▼ビュー（装備選択）▼▼

      menuStatus_.SetTitle("現", "新");
      menuStatus_.EquipChange(topArrowSelect_);
      //▼▼▼Create▼▼▼
    }
    protected override void NotifyTop(int id)
    {
      if (id != topArrowSelect_)
        menuStatus_.EquipChange(id);
      base.NotifyTop(id);
      menuStatus_.EquipChange(topArrowSelect_);
    }
    protected override void NotifySec(int id)
    {
      base.NotifySec(id);
      //menuStatus_.EquipChange(topArrowSelect_);
    }
    protected override void NotifyItem(int viewItemId)
    {
      tbTopArrows_[topArrowSelect_].Foreground = kSelectedCursorBrush_;
      tbSecArrows_[secArrowSelect_].Foreground = kSelectedCursorBrush_;
      menuStatus_.EquipChangeCalc(topArrowSelect_, secArrowSelect_, viewItemId-1);
    }
    // 戻り値：装備ウィンドウを終了する場合は true を返す。
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
            int equiped = friends_[topArrowSelect_].GetEquipId((Data.EquipCategory)secArrowSelect_);
            int arrowEqId = viewPositionToEquipId_[itemArrowSelect_];// itemArrowSelect_ - 1;
            // はずすを含む装備する条件は、キャラ装備と選択装備が異なる。
            if (equiped != arrowEqId)
            {
              // ■装備
              int before = friendsWritable_[topArrowSelect_].Equip(
                (Data.EquipCategory)secArrowSelect_, arrowEqId);
              friendsWritable_[topArrowSelect_].ResetStatus();
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
              tbSecArrows_[secArrowSelect_].Foreground = kCurrentCursorBrush_;
            }
          }
          break;
        case NotifyCode.Cancel:
          if (itemArrowSelect_ >= 0)
          {
            tbSecArrows_[secArrowSelect_].Foreground = kCurrentCursorBrush_;
            if (tbItemArrows_.Length >= itemArrowSelect_)//◇
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
    protected override void UpdateView()
    {
      view_.Children.Clear();
      Data.Loader loader = Data.Loader.Instance;
      Data.ConstStatus[] eqs = loader.EquipArray((Data.EquipCategory)secArrowSelect_);

      int viewPos = 0;
      tbItemArrows_ = new TextBlock[0];
      AddViewItem(viewPos, "はずす", "-");
      viewPos++;

      Data.Bag bag = Data.DataSC.Bag();
      int eqId = friends_[topArrowSelect_].GetEquipId((Data.EquipCategory)secArrowSelect_);
      Array.Resize(ref this.viewPositionToEquipId_, 1);
      viewPositionToEquipId_[0] = -1; // はずす分
      for (int i = 0; i < eqs.Length; i++)
      {
        string num = eqId == i ? "E" : bag.equip((Data.EquipCategory)secArrowSelect_, i).ToString();
        if (num == "0") continue;
        AddViewItem(viewPos, eqs[i].name(), num);
        Array.Resize(ref this.viewPositionToEquipId_, this.viewPositionToEquipId_.Length + 1);
        this.viewPositionToEquipId_[viewPos] = i;
        viewPos++;
      }
    }

    // 戻り値：選択しているキャラクター番号を返す。
    public override int Destroy()
    {
      parent_.Children.Remove(wholePanel_);
      return topArrowSelect_;
    }
  }
}
