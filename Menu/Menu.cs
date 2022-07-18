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

    MenuUI ui_;
    MenuNotify menuNotify_;

    Data.StatusWritable[] charsWritable_;

    readonly Grid[] mainLeftItems_ = new Grid[kMainLeftStringsNum];
    readonly MainChar[] mainChar_ = new MainChar[kMainCenterNum];

    public Menu(MainPage mainPage, Data.StatusWritable[] charsWritable)
    {
      mainPage_ = mainPage;
      charsWritable_ = charsWritable;
      frameManager_ = mainPage.GetFrameTimer();
      monitor_ = mainPage.GetMonitor();
      monitorBg_ = mainPage.GetMonitorBg();

      menuNotify_ = new MenuNotify(Notify);

      ui_ = new MenuUI(monitor_, menuNotify_);
    }
    void Notify(NotifyCode notifyCode)
    {
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
      }
      else if (NotifyCode.Char0 <= notifyCode && notifyCode <= NotifyCode.Char3)
      {
        MainLeftArrowClear();
        foreach (MainChar c in mainChar_)
        {
          c.TapOtherChar();
        }
      }
      JsTrans.console_log("NotifyCode: " + notifyCode.ToString());
    }
    public void Run()
    {
      mainPage_.BottomTextBySequence("Menu");

      monitorBg_.Background = UnivLib.GetBrush(11, 70, 100);

      //▲▲▲▲メインウィンドウ▲▲▲▲
      Grid mainGrid = ui_.RunGrid(0, 0, 1000, 529, 0, 0, monitor_, false, UnivLib.GetBrush(Colors.Orange));

      //▲▲▲左のグリッド▲▲▲
      Grid leftGrid = ui_.RunGrid(5, 5, 100, 519, 10, 0, mainGrid);
      for (int i = 0; i < kMainLeftStrings.Length; i++)
      {
        mainLeftItems_[i] = ui_.RunMainLeftItem(56*i, kMainLeftStrings[i], leftGrid, kMainLeftNotifies[i]);
      }
      //▼▼▼左のグリッド▼▼▼

      //▲▲▲各キャラクターのグリッド▲▲▲
      for (int i = 0; i < kMainCenterNum; i++)
      {
        Grid charGrid = ui_.RunGrid(115, 5 + 131 * i, 420, 125, 10, 15, mainGrid);
        charGrid.Padding = new Thickness(10, 10, 10, 10);
        mainChar_[i] = new MainChar(charGrid, i, Notify, NotifyCode.Char0+i);
      }
      //▼▼▼各キャラクターのグリッド▼▼▼
      
      //▲▲▲右のグリッド▲▲▲
      Grid rightGrid = ui_.RunGrid(540, 5, 255, 519, 5, 15, mainGrid);
      MenuStatus ms = new MenuStatus();
      ms.Create(rightGrid);
      //▼▼▼右のグリッド▼▼▼

      //▼▼▼▼メインウィンドウ▼▼▼▼

      //▲▲▲下のウィンドウ▲▲▲
      ui_.RunButton(5, 530, "決定", monitor_, NotifyCode.Ok);
      ui_.RunButton(111, 530, "戻る", monitor_, NotifyCode.Cancel);
      //▼▼▼下のウィンドウ▼▼▼



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
        frameManager_.EnterSequenceFadeOut(OnFadeOuted);
      }
    }
  }
}
