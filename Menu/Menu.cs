using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System; // VirtualKey
using Windows.UI.Xaml.Controls; // Grid
using Windows.UI;
using Windows.UI.Xaml; // Thickness
using Windows.UI.Xaml.Media; // Stretch
using Windows.UI.Xaml.Input;
using Windows.UI.Text;
using Univ.NsMenu;
using Univ.Data;

namespace Univ
{
  internal class Menu : MenuInclude, IRun
  {
    MainPage mainPage_;
    FrameManager frameManager_;
    Grid monitor_;   // 描画用
    Grid monitorBg_; //背景描画用
    Grid mainGrid_;
    long msTimeMinuts_ = 0;
    TextBlock tbTime_;

    MenuUI ui_;
    MenuNotify menuNotify_;

    Data.StatusWritable[] FriendsWritable_;

    readonly Grid[] mainLeftItems_ = new Grid[kMainLeftStringsNum];
    readonly MainFriend[] mainChar_ = new MainFriend[kMainCenterNum];

    MenuStatus menuStatus_;

    NotifyCode leftItemSelected_;
    int friendSelected_ = 0;

    Menu123Stack menuStack_ = null;
    //MenuSkill menuSkill_ = null;
    MenuEquip menuEquip_ = null;
    //MenuItem  menuItem_ = null;
    MenuCustom menuCustom_ = null;

    MenuSaveLoad menuSaveLoad_;
    
    public Menu(MainPage mainPage, Data.SecurityToken stFriends)
    {
      mainPage_ = mainPage;
      FriendsWritable_ = Data.DataSC.FriendsWritable(stFriends);
      frameManager_ = mainPage.GetFrameManager();
      monitor_ = mainPage.GetMonitor();
      monitorBg_ = mainPage.GetMonitorBg();

      menuNotify_ = new MenuNotify(Notify);

      //ui_ = new MenuUI(menuNotify_);

      menuSaveLoad_ = new MenuSaveLoad(frameManager_, monitor_, FrameOne);
    }
    void Exit()
    {
      frameManager_.EnterSequenceFadeOut(OnFadeOuted);
      //monitor_.Children.Clear();
      //monitorBg_.Children.Clear();
      //frameManager_.ExitSequence();
    }
    void Notify(NotifyCode notifyCode)
    {
      if (menuSaveLoad_.IsShowing) return;


      if (menuStack_ != null)
      {
        if (menuStack_.Notify(notifyCode))
        {
          // ■特技または画面を閉じる。
          menuStack_.Destroy();
          menuStack_ = null;
        }
        return;
      }
      if (menuEquip_ != null)
      {
        if (menuEquip_.Notify(notifyCode))
        {
          // ■装備画面を閉じる。
          friendSelected_ = menuEquip_.Destroy();
          menuStatus_.SetTitle();
          if (friendSelected_ > 3)
            friendSelected_ = 0;
          for (int i = 0; i < mainChar_.Length; i++)
          {
            if (i == friendSelected_)
            {
              mainChar_[i].Tap();
              mainChar_[i].TapLeftMenu();
              menuStatus_.Change(friendSelected_);
            }
            else
            {
              mainChar_[i].TapOtherFriend();
            }
          }
          menuStatus_.SetTitle();
          menuStatus_.ResetColorBold();
          menuEquip_ = null;
        }
        return;
      }
      if (menuCustom_ != null)
      {
        if (menuCustom_.Notify(notifyCode))
        {
          menuCustom_.Destroy();
          menuCustom_ = null;
        }
        return;
      }

      void MainLeftArrowClear()
      {
        foreach (Grid item in mainLeftItems_)
        {
          foreach (UIElement uie in item.Children)
          {
            TextBlock tb = uie as TextBlock;
            if (tb != null) tb.Text = "";
          }
        }
      }

      if (notifyCode == NotifyCode.Left)
      {
        //ui_.ShowCharsArrow(0);
      }
      else if (NotifyCode.Skill <= notifyCode && notifyCode <= NotifyCode.Load)
      { //■クリック通知処理
        MainLeftArrowClear();
        foreach (MainFriend c in mainChar_)
        {
          c.TapLeftMenu();
        }
        leftItemSelected_ = notifyCode;
        if (notifyCode == NotifyCode.Custom)
        {
          menuCustom_ = new MenuCustom(mainGrid_);
        }
      }
      else if (NotifyCode.Char0 <= notifyCode && notifyCode <= NotifyCode.Char3)
      {
        MainLeftArrowClear();
        foreach (MainFriend c in mainChar_)
        {
          c.TapOtherFriend();
        }
        friendSelected_ = notifyCode - NotifyCode.Char0;
        menuStatus_.Change(friendSelected_);
        leftItemSelected_ = NotifyCode.None;
      }
      else if (notifyCode == NotifyCode.Ok)
      {
        switch (leftItemSelected_)
        {
          case NotifyCode.Skill:
            menuStack_ = new MenuSkill(mainGrid_, menuStatus_, FriendsWritable_, friendSelected_);
            break;
          case NotifyCode.Equip:
            // ■装備画面を表示
            menuEquip_ = new MenuEquip(mainGrid_, menuStatus_, FriendsWritable_, friendSelected_);
            //menuEquip_.Create(friendSelected_);
            break;
          case NotifyCode.Item:
            // ■アイテム
            menuStack_ = new MenuItem(mainGrid_, FriendsWritable_);
            //menuItem_.Create();
            break;
          case NotifyCode.Custom:
            // ■カスタム
            menuCustom_ = new MenuCustom(mainGrid_);
            break;
          case NotifyCode.Save:
            // ■セーブ
            menuSaveLoad_.Save();
            break;
          case NotifyCode.Load:
            // ■ロード
            menuSaveLoad_.Load();
            break;
        }
      }
      else if (notifyCode == NotifyCode.Cancel)
      {
        Exit();
      }
    }
    public void Run()
    {
      mainPage_.BottomTextBySequence("Menu");

      monitorBg_.Background = MenuUI.kMonitorBgBrush;

      //▲▲▲▲メインウィンドウ▲▲▲▲
      mainGrid_ = MenuUI.RunGrid(0, 0, 1000, 529, 5, 5, monitor_, false, MenuUI.kMainGridBrush);

      //▲▲▲左のグリッド▲▲▲
      Grid leftGrid = MenuUI.RunGrid(0, 0, 100, 519, 10, 0, mainGrid_);
      for (int i = 0; i < kMainLeftStrings.Length; i++)
      {
        mainLeftItems_[i] = MenuUI.RunMainLeftItem(56*i, kMainLeftStrings[i], leftGrid, menuNotify_, kMainLeftNotifies[i]);
      }
      //▼▼▼左のグリッド▼▼▼

      //▲▲▲各キャラクターのグリッド▲▲▲
      for (int i = 0; i < kMainCenterNum; i++)
      {
        Grid charGrid = MenuUI.RunGrid(108, 131 * i, 420, 125, 10, 15, mainGrid_);
        charGrid.Padding = new Thickness(10, 10, 10, 10);
        mainChar_[i] = new MainFriend(charGrid, i, Notify, NotifyCode.Char0+i);
      }
      //▼▼▼各キャラクターのグリッド▼▼▼
      
      //▲▲▲右上のグリッド▲▲▲
      //Grid rightTopGrid = ui_.RunGrid(535, 0, 255, 519, 5, 15, mainGrid_);
      Grid rightTopGrid = MenuUI.RunGrid(535, 0, 255, 450, 5, 15, mainGrid_);
      menuStatus_ = new MenuStatus();
      menuStatus_.Create(rightTopGrid);
      //▼▼▼右上のグリッド▼▼▼
      
      //▲▲▲右下のグリッド▲▲▲
      Grid rightBottom1Grid = MenuUI.RunGrid(535, 458, 125, 60, 5, 5, mainGrid_);
      Data.Basic basic = Data.Basic.Instance;
      TextBlock tb = MenuUI.RunLavel(rightBottom1Grid, 0, 10, basic.gold().ToString() + " G");
      UnivLib.MeasureWidth(tb, rightBottom1Grid);

      Grid rightBottom2Grid = MenuUI.RunGrid(665, 458, 125, 60, 5, 5, mainGrid_);
      msTimeMinuts_ = basic.msTimeMinutes();
      tbTime_ = MenuUI.RunLavel(rightBottom2Grid, 0, 10, "TIME " + basic.msTimeString());
      UnivLib.MeasureWidth(tbTime_, rightBottom2Grid);
      //▼▼▼右下のグリッド▼▼▼

      //▼▼▼▼メインウィンドウ▼▼▼▼

      //▲▲▲下のウィンドウ▲▲▲
      MenuUI.RunButton(5, 530, "決定", monitor_, menuNotify_, NotifyCode.Ok);
      MenuUI.RunButton(111, 530, "戻る", monitor_, menuNotify_, NotifyCode.Cancel);
      //▼▼▼下のウィンドウ▼▼▼

      // キャラクター選択状態を設定する。
      Notify(NotifyCode.Char0 + friendSelected_);
      mainChar_[friendSelected_].Tap();

      //■
      //menuEquip_.Create();

      //frameManager_.ChangeSequence(FrameOne);
      //フェードイン後、フレームループ開始
      frameManager_.EnterSequenceFadeIn(FrameOne);
    }
    public void OnFadeOuted(object senderDispatcherTimer, object eNull)
    {
      mainPage_.Clear();
      frameManager_.ExitSequence();
    }
    public void FrameOne(object senderDispatcherTimer, object eNull)
    {
      Data.Basic basic = Data.Basic.Instance;
      if (msTimeMinuts_ != basic.msTimeMinutes())
      {
        msTimeMinuts_ = basic.msTimeMinutes();
        tbTime_.Text = "TIME " + basic.msTimeString();
      }
      if (menuCustom_ != null)
      {
        if (menuCustom_.FrameOne(frameManager_))
        {
          menuCustom_.Destroy();
          menuCustom_ = null;
          return;
        }
      }

      if (menuStack_ == null && 
        menuEquip_ == null &&
        menuCustom_ == null&&
        frameManager_.IsKeyDownFirst(GameKey.Cancel))
      { // キャンセルキー処理
        Exit();
      }
    }
  }
}
