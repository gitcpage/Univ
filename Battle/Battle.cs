using System;
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
    bool isMouseBeforeDown;

    protected BattleData data_; // 派生先でHP操作を行うことが可能。
    BattleUI ui_;
    BattleNotify battleNotify_;
    BattleCommand battleCommand_;
    BattleCommandNotify commandNotify_;
    BattleTargetNotify targetNotify_;
    int cmdChar_ = -1;

    BattleMonsters monsters_;

    protected BattleAtb atb_; // 派生先で初期ゲージフルにすることが可能。
    BattleEffect effect_;

    BattleUIExWindow exWindow_;
    int sumExp_;
    int sumGold_;

    public Battle(MainPage mainPage, Data.StatusWritable[] charsWritable, int monsGroupId)
    {
      mainPage_ = mainPage;
      frameManager_ = mainPage.GetFrameManager();
      monitor_ = mainPage.GetMonitor();
      monitorBg_ = mainPage.GetMonitorBg();

      battleNotify_ = new BattleNotify(Notify);
      commandNotify_ = new BattleCommandNotify(CommandNotify);
      targetNotify_ = new BattleTargetNotify(TargetNotify);

      data_ = new BattleData(charsWritable, monsGroupId);
      ui_ = new BattleUI(monitor_, battleNotify_);
      battleCommand_ = null;

      monsters_ = new BattleMonsters(monitor_, targetNotify_, data_);

      atb_ = new BattleAtb(ui_);
      effect_ = new BattleEffect();

      exWindow_ = new BattleUIExWindow(monitor_);
      sumExp_ = sumGold_ = 0;
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
      //battleCommand_.Show(false);
      atb_.state = BattleAtb.State.Doing;
      int damage = data_.Friend(cmdChar_).atk();
      UnitInfo info = data_.MonsInfo(target);
      info.hp -= damage;
      JsTrans.console_log("Atk: from("+cmdChar_+") to("+target+") " + damage + ":" + info.hp);
      if (info.hp <= 0)
      {
        sumExp_ += data_.Monster(target).Exp;
        sumGold_ += data_.Monster(target).Gold;
      }
      //■
      effect_.Ready(monitor_, monsters_.GetThickness(target), monsters_.GetImage(target), damage, info.hp <= 0);
      battleCommand_.Show(false);
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
      charsGrid.Children.Add(ui_.RunStringPanelRightAlignment(Data.Status.NowHpStrings(), 90, 50));
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

      //▲▲▲上のウィンドウ▲▲▲
      //exWindow_.ShowTop("こころないてんし", false); //"たたかいにかった", false);
      //▼▼▼上のウィンドウ▼▼▼

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
        case BattleAtb.State.None:
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
          break;
        case BattleAtb.State.Command:
          break;
        case BattleAtb.State.TargetSelect:
          break;
        case BattleAtb.State.Doing:
          if (effect_.FrameOne())
          {
            atb_.state = BattleAtb.State.Done;
          }
          break;
        case BattleAtb.State.Done:
          ui_.SetActiveMark(cmdChar_, false);
          atb_.SetVarValue(cmdChar_, 0);
          if (data_.MonsFirstAlive() == -1)
          {
            atb_.state = BattleAtb.State.Win;
            Queue<string> ss = new Queue<string>();
            ss.Enqueue("たたかいにかった！");
            if (sumExp_ > 0)
            {
              ss.Enqueue("経験値" + sumExp_ + "かくとく！");
            }
            if (sumGold_ > 0)
            {
              ss.Enqueue(sumGold_.ToString() + "ゴールドてにいれた！");
            }
            exWindow_.SetTopStrings(ss);
          }
          else
          {
            atb_.state = BattleAtb.State.None;
          }
          break;
        case BattleAtb.State.Win:
          if (exWindow_.IsShowTop())
          {
            if (mainPage_.isMouseLDown)
            {
              if (!isMouseBeforeDown)
              {
                exWindow_.HiddenTop();
              }
            }
          }
          else
          {
            if (!exWindow_.NextTopStringsShow())
              frameManager_.EnterSequenceFadeOut(OnFadeOuted);
          }
          break;
      }
      isMouseBeforeDown = mainPage_.isMouseLDown;
    }
  }
}
