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

namespace Univ
{
  internal class Menu : MenuInclude, IRun
  {
    MainPage mainPage_;
    FrameManager frameManager_;
    Grid monitor_;   // 描画用
    Grid monitorBg_; //背景描画用
    Grid mainGrid_;

    MenuUI ui_;
    MenuNotify menuNotify_;

    Data.StatusWritable[] charsWritable_;

    readonly Grid[] mainLeftItems_ = new Grid[kMainLeftStringsNum];
    readonly MainChar[] mainChar_ = new MainChar[kMainCenterNum];

    MenuStatus menuStatus_;

    NotifyCode leftItemSelected_;
    int charSelected03_;

    MenuEquip menuEquip_;
    
    public Menu(MainPage mainPage, Data.StatusWritable[] charsWritable)
    {
      mainPage_ = mainPage;
      charsWritable_ = charsWritable;
      frameManager_ = mainPage.GetFrameTimer();
      monitor_ = mainPage.GetMonitor();
      monitorBg_ = mainPage.GetMonitorBg();

      menuNotify_ = new MenuNotify(Notify);

      ui_ = new MenuUI(monitor_, menuNotify_);
      menuEquip_ = null;
      charSelected03_ = 0;
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
      if (menuEquip_ != null)
      {
        if (menuEquip_.Notify(notifyCode))
        {
          // ■装備画面を閉じる。
          charSelected03_ = menuEquip_.Destroy();
          menuStatus_.SetTitle();
          if (charSelected03_ > 3)
            charSelected03_ = 0;
          for (int i = 0; i < mainChar_.Length; i++)
          {
            if (i == charSelected03_)
            {
              mainChar_[i].Tap();
              mainChar_[i].TapLeftMenu();
            }
            else
            {
              mainChar_[i].TapOtherChar();
            }
          }
        }
        menuEquip_ = null;
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
      {
        MainLeftArrowClear();
        foreach (MainChar c in mainChar_)
        {
          c.TapLeftMenu();
        }
        leftItemSelected_ = notifyCode;
      }
      else if (NotifyCode.Char0 <= notifyCode && notifyCode <= NotifyCode.Char3)
      {
        MainLeftArrowClear();
        foreach (MainChar c in mainChar_)
        {
          c.TapOtherChar();
        }
        menuStatus_.Change(notifyCode - NotifyCode.Char0);
        leftItemSelected_ = NotifyCode.None;
        charSelected03_ = notifyCode - NotifyCode.Char0;
      }
      else if (notifyCode == NotifyCode.Ok)
      {
        switch (leftItemSelected_)
        {
          case NotifyCode.Equip:
            // ■装備画面を表示
            menuEquip_ = new MenuEquip(mainGrid_, menuStatus_, charsWritable_);
            menuEquip_.Create(charSelected03_);
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

      monitorBg_.Background = UnivLib.GetBrush(11, 70, 100);

      //▲▲▲▲メインウィンドウ▲▲▲▲
      mainGrid_ = ui_.RunGrid(0, 0, 1000, 529, 5, 5, monitor_, false, UnivLib.GetBrush(Colors.Orange));

      //▲▲▲左のグリッド▲▲▲
      Grid leftGrid = ui_.RunGrid(0, 0, 100, 519, 10, 0, mainGrid_);
      for (int i = 0; i < kMainLeftStrings.Length; i++)
      {
        mainLeftItems_[i] = ui_.RunMainLeftItem(56*i, kMainLeftStrings[i], leftGrid, kMainLeftNotifies[i]);
      }
      //▼▼▼左のグリッド▼▼▼

      //▲▲▲各キャラクターのグリッド▲▲▲
      for (int i = 0; i < kMainCenterNum; i++)
      {
        Grid charGrid = ui_.RunGrid(108, 131 * i, 420, 125, 10, 15, mainGrid_);
        charGrid.Padding = new Thickness(10, 10, 10, 10);
        mainChar_[i] = new MainChar(charGrid, i, Notify, NotifyCode.Char0+i);
      }
      //▼▼▼各キャラクターのグリッド▼▼▼
      
      //▲▲▲右上のグリッド▲▲▲
      //Grid rightTopGrid = ui_.RunGrid(535, 0, 255, 519, 5, 15, mainGrid_);
      Grid rightTopGrid = ui_.RunGrid(535, 0, 255, 450, 5, 15, mainGrid_);
      menuStatus_ = new MenuStatus();
      menuStatus_.Create(rightTopGrid);
      //▼▼▼右上のグリッド▼▼▼
      
      //▲▲▲右下のグリッド▲▲▲
      Grid rightBottom1Grid = ui_.RunGrid(535, 458, 125, 60, 5, 5, mainGrid_);
      //TextBlock tb = MenuUI.RunLavelCenterAligned(rightBottom1Grid, 0, 10, 125, "1 G");
      Data.Basic basic = Data.Basic.Instance;
      TextBlock tb = MenuUI.RunLavel(rightBottom1Grid, 0, 10, basic.gold().ToString() + " G");
      UnivLib.MeasureWidth(tb, rightBottom1Grid);

      Grid rightBottom2Grid = ui_.RunGrid(665, 458, 125, 60, 5, 5, mainGrid_);
      //tb = MenuUI.RunLavelCenterAligned(rightBottom2Grid, 0, 10, 125, "TIME 00:00");
      long minutes = basic.msTimeMinutes();
      long hour = minutes / 60;
      minutes %= 60;
      tb = MenuUI.RunLavel(rightBottom2Grid, 0, 10, "TIME "+hour+":"+minutes);
      UnivLib.MeasureWidth(tb, rightBottom2Grid);
      //▼▼▼右下のグリッド▼▼▼

      //▼▼▼▼メインウィンドウ▼▼▼▼

      //▲▲▲下のウィンドウ▲▲▲
      ui_.RunButton(5, 530, "決定", monitor_, NotifyCode.Ok);
      ui_.RunButton(111, 530, "戻る", monitor_, NotifyCode.Cancel);
      //▼▼▼下のウィンドウ▼▼▼

      // キャラクター選択状態を設定する。
      Notify(NotifyCode.Char0 + charSelected03_);
      mainChar_[charSelected03_].Tap();

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
      if (frameManager_.ResettingOnce())
      {
        JsTrans.console_log("ソフトリセット");
        frameManager_.EnterSequenceFadeOut(OnFadeOuted);
        return;
      }

      // スペースキーで戻る
      if (frameManager_.IsKeyDownFirst(VirtualKey.Space))
      {
        Exit();
      }
    }
  }
}
