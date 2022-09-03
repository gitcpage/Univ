using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Univ.Data;
using Windows.UI.Xaml.Controls;

namespace Univ.NsMenu
{
  internal class MenuSkill : Menu123Stack
  {
    //MenuStatus menuStatus_;

    Data.Status[] friends_ = Data.DataSC.Friends();
    Data.StatusWritable[] friendsWritable_;
    //int[] viewPositionToSkillId_ = { };

    public MenuSkill(Grid parent, MenuStatus menuStatus, Data.StatusWritable[] friendsWritable,
      int friendSelected) : base(parent)
    {
      //menuStatus_ = menuStatus;
      friendsWritable_ = friendsWritable;

      //▲▲▲Create▲▲▲

      //▲▲トップ（キャラ選択）▲▲
      base.CreateTopFriends(friendSelected);
      //▼▼トップ（キャラ選択）▼▼

      //▲▲セカンド（装備種類選択）▲▲
      string[] secNames = { "技", "魔法" };
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
    protected override void NotifySec(int id)
    {
      base.NotifySec(id);
      //menuStatus_.EquipChange(topArrowSelect_);
    }
    protected override void NotifyItem(int viewItemId)
    {
      tbTopArrows_[topArrowSelect_].Foreground = kSelectedCursorBrush_;
      tbSecArrows_[secArrowSelect_].Foreground = kSelectedCursorBrush_;
      //menuStatus_.EquipChangeCalc(topArrowSelect_, secArrowSelect_, viewItemId - 1);
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
    protected override void UpdateView()
    {
      view_.Children.Clear();
      tbItemArrows_ = new TextBlock[0];
      int viewPos = 0;
      ConstSkill[] skills;
      if (secArrowSelect_ == 0)
        skills = DataSC.Skills(SkillKind.Tech);
      else
        skills = DataSC.MagicSkills();
      for (int i = 0; i < skills.Length; i++)
      {
        AddViewItem(viewPos, skills[i].Name, skills[i].Mp.ToString());
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
