﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System; // VirtualKey
using Windows.UI.Xaml; // Thickness
using Windows.UI.Xaml.Controls; // Grid
using Windows.UI.Xaml.Media; // Stretch
using Windows.UI;
using System.Windows;
using Univ.NsBattle;

namespace Univ
{
  internal class Battle : IRun
  {
    public enum NotifyCode
    {
      Left, Up, Right, Down, Top, Bottom, Escape, Next, Ok, Cancel
    }

    MainPage mainPage_;
    FrameManager frameManager_;
    Grid monitor_;   // 描画用
    Grid monitorBg_; // 背景描画用

    BattleUI ui_;
    BattleNotify battleNotify_;
    BattleCommand battleCommand_;
    BattleCommandNotify commandNotify_;
    BattleTargetNotify targetNotify_;
    int cmdChar_ = -1;

    BattleMonsters monsters_;

    protected BattleAtb atb_;
    BattleEffect effect_;

    //Data.StatusWritable[] charsWritable_;

    public Battle(MainPage mainPage, Data.StatusWritable[] charsWritable)
    {
      mainPage_ = mainPage;
      //charsWritable_ = charsWritable;
      frameManager_ = mainPage.GetFrameManager();
      monitor_ = mainPage.GetMonitor();
      monitorBg_ = mainPage.GetMonitorBg();

      battleNotify_ = new BattleNotify(Notify);
      commandNotify_ = new BattleCommandNotify(CommandNotify);
      targetNotify_ = new BattleTargetNotify(TargetNotify);

      ui_ = new BattleUI(monitor_, battleNotify_);
      battleCommand_ = null;

      monsters_ = new BattleMonsters(monitor_, targetNotify_);

      atb_ = new BattleAtb(ui_);
      effect_ = new BattleEffect();
    }
    void Notify(NotifyCode notifyCode)
    {
      if (notifyCode == NotifyCode.Left)
      {
        ui_.ShowCharsArrow(0);
      }
      JsTrans.alert("NotifyCode: " + notifyCode.ToString());
    }
    void CommandNotify(int cmd1)
    {
      if (cmd1 == -1)
      {
        monsters_.ShowTarget(false);
        atb_.state = BattleAtb.State.Command;
      }
      else
      {
        monsters_.ShowTarget();
        atb_.state = BattleAtb.State.TargetSelect;
      }
    }
    void TargetNotify(int target)
    {
      JsTrans.console_log("Attack: " + target.ToString());
      atb_.state = BattleAtb.State.Doing;
      Data.Status[] dchars = Data.Status.Instances;
      int damage = dchars[cmdChar_].atk();
      //■
      effect_.Ready(monitor_, monsters_.GetThickness(target), monsters_.GetImage(target), damage);
    }
    public virtual void Run()
    {
      mainPage_.BottomTextBySequence("Battle");

      //初期化処理
      atb_.Initialize();
      /*TextBlock tb_ = new TextBlock();
      tb_.Text = "RunImplementsFade";
      tb_.Foreground = UnivLib.GetBrush(Colors.Red);
      monitor_.Children.Add(tb_);*/

      //背景
      Image bg = UnivLib.ImageInstance("battle/forest1.jpg", "backGround"/*, "backGround"*/);
      monitorBg_.Children.Add(bg);

      //▲▲▲モンスターグラフィック▲▲▲
      monsters_.Create(atb_);
      //▼▼▼モンスターグラフィック▼▼▼

      ui_.RunChars();

      //▲▲▲下のウィンドウ▲▲▲
      Grid underGrid = new Grid();
      underGrid.Width = 800;
      underGrid.Height = 160;
      underGrid.Margin = new Thickness(0, 440, 0, 0);
      underGrid.Background = UnivLib.GetBrush(157,181,183);

      //▲▲モンスター一覧▲▲
      Grid monstersGrid = BattleUI.RunUnderGrid(7, 330);

      string[] monsterStrings = new string[5] { 
        "ブルーフォックス", "ブルーフォックス", "ブルーフォックス", "ブルーフォックス", "ブルーフォックス" };
      monstersGrid.Children.Add(BattleUI.RunStringPanel(monsterStrings));
      underGrid.Children.Add(monstersGrid);
      //▼▼モンスター一覧▼▼

      //▲▲キャラクター一覧▲▲
      Grid charsGrid = BattleUI.RunUnderGrid(341, 330);
      Data.Status[] dchars = Data.Status.Instances;
      charsGrid.Children.Add(BattleUI.RunStringPanel(Data.Status.names()));
      charsGrid.Children.Add(ui_.RunStringPanelRightAlignment(Data.Status.hpStrings(), 90, 50));
      underGrid.Children.Add(charsGrid);

      underGrid.Children.Add(ui_.RunSliderPanel(atb_));
      //▼▼キャラクター一覧▼▼

      //▲▲コントローラー▲▲
      Grid controllerGrid = BattleUI.RunUnderGrid(678, 115);
      {
        Grid g = controllerGrid;
        ui_.RunControllerTextBlock(7, 10, 30, "逃", g, NotifyCode.Escape);
        ui_.RunControllerTextBlock(7, 45, 30, "←", g, NotifyCode.Left);
        ui_.RunControllerTextBlock(7, 80, 30, "次", g, NotifyCode.Next);
        ui_.RunControllerTextBlock(35, 10, 37, "↑", g, NotifyCode.Up);
        ui_.RunControllerTextBlock(35, 45, 37, "決定", g, NotifyCode.Ok);
        ui_.RunControllerTextBlock(35, 80, 37, "↓", g, NotifyCode.Down);
        ui_.RunControllerTextBlock(70, 10, 35, "↑↑", g, NotifyCode.Top);
        ui_.RunControllerTextBlock(70, 45, 35, "→", g, NotifyCode.Right);
        ui_.RunControllerTextBlock(70, 80, 35, "↓↓", g, NotifyCode.Bottom);
        ui_.RunControllerTextBlock(0, 115, 115, "キャンセル", g, NotifyCode.Cancel);
      }
      underGrid.Children.Add(controllerGrid);
      //▼▼コントローラー▼▼

      //▲▲コマンド▲▲
      battleCommand_ = new BattleCommand(underGrid, commandNotify_);
      //▼▼コマンド▼▼

      monitor_.Children.Add(underGrid);
      //▼▼▼下のウィンドウ▼▼▼

      atb_.state = BattleAtb.State.None;

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
      monsters_.Adjust();
      switch (atb_.state)
      {
        case BattleAtb.State.TargetSelect:
          break;
        case BattleAtb.State.Doing:
          if (effect_.EffectFrameOne())
          {
            atb_.state = BattleAtb.State.Done;
          }
          break;
        case BattleAtb.State.Done:
          ui_.SetActiveMark(cmdChar_, false);
          atb_.SetVarValue(cmdChar_, 0);
          //JsTrans.console_log("State.Doing to None cmdChar:" + cmdChar);
          atb_.state = BattleAtb.State.None;
          battleCommand_.Show(false);
          break;
      }

      if (battleCommand_.IsShowing)
      {

      }
      else
      {
        int charId = atb_.BarFullId();
        if (charId >= 0)
        {
          atb_.state = BattleAtb.State.Command;
          battleCommand_.Show();
          cmdChar_ = charId;
          ui_.SetActiveMark(charId, true);
        }
        else
        {
          atb_.Accumulate();
        }
      }

      // スペースキーで戻る
      if (frameManager_.IsKeyDownFirst(VirtualKey.Space))
      {
        frameManager_.EnterSequenceFadeOut(OnFadeOuted);
      }
    }
  }
}
