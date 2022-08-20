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
  internal class Battle : IRun, IIsTargetSelect
  {
    class ActionQueue : BattleInclude
    {
      BattleCommandInfo[] ActionQueue_;
      int num_;
      int dequeueDoneId_;
      public ActionQueue()
      {
        ActionQueue_ = new BattleCommandInfo[kFriendMax];
        num_ = 0;
        dequeueDoneId_ = -1;
      }
      public void Enqueue(BattleCommandInfo info)
      {
        ActionQueue_[num_] = info;
        num_++;
      }
      public BattleCommandInfo Dequeue()
      {
        if (num_ == 0) return null;
        BattleCommandInfo ret = ActionQueue_[0];
        for (int i = 1; i < num_; i++)
        {
          ActionQueue_[i - 1] = ActionQueue_[i];
        }
        num_--;
        dequeueDoneId_ = ret.from;
        return ret;
      }
      public int DequeueDoneId()
      {
        return dequeueDoneId_;
      }
      public void ResetDequeueDoneId()
      {
        dequeueDoneId_ = -1;
      }
    }

    enum State
    {
      None, Command, TargetSelect, Win, Lose
    }

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
    BattleCommand battleCommand_ = null;
    BattleCommandNotify commandNotify_;
    BattleTargetNotify targetNotify_;
    int cmdChar_ = -1;

    BattleMonsters monsters_;

    protected BattleAtb atb_; // 派生先で初期ゲージフルにすることが可能。
    BattleEffect effect_ = new BattleEffect();
    ActionQueue actionQueue_ = new ActionQueue();

    BattleUIExWindow exWindow_;
    BattleAcquisition acquisition_ = new BattleAcquisition();
    State state_;

    public Battle(MainPage mainPage, Data.SecurityToken stFriends, int monsGroupId)
    {
      mainPage_ = mainPage;
      frameManager_ = mainPage.GetFrameManager();
      monitor_ = mainPage.GetMonitor();
      monitorBg_ = mainPage.GetMonitorBg();

      battleNotify_ = new BattleNotify(Notify);
      commandNotify_ = new BattleCommandNotify(CommandNotify);
      targetNotify_ = new BattleTargetNotify(TargetNotify);

      data_ = new BattleData(stFriends, monsGroupId);
      ui_ = new BattleUI(monitor_, battleNotify_);

      monsters_ = new BattleMonsters(monitor_, targetNotify_, data_);

      atb_ = new BattleAtb(ui_);

      exWindow_ = new BattleUIExWindow(monitor_);
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
        state_ = State.Command;
      }
      else
      {
        monsters_.ShowTarget();
        state_ = State.TargetSelect;
      }
    }
    void TargetNotify(int target)
    {
      actionQueue_.Enqueue(new BattleCommandInfo(cmdChar_, target));//■
      ui_.SetActiveMark(cmdChar_, false, true);
      atb_.SetGaugeValueAction(cmdChar_);
      battleCommand_.Hidden();
      state_ = State.None;
    }
    public virtual void Run()
    {
      mainPage_.BottomTextBySequence("Battle");

      //初期化処理
      atb_.Initialize();

      //背景
      Image bg = UnivLib.ImageInstance("battle/forest1.jpg", "backGround"/*, "backGround"*/);
      monitorBg_.Children.Add(bg);

      //▲▲▲モンスターグラフィック▲▲▲
      monsters_.Create(this);
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
      //Data.Status[] friends_ = Data.DataSC.Friends();//Status.Instances;
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

      state_ = State.None;

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
      switch (state_)
      {
        case State.None:
          int charId = atb_.GaugeFullId();
          if (charId >= 0)
          {
            state_ = State.Command;
            battleCommand_.Show();//■
            cmdChar_ = charId;
            ui_.SetActiveMark(charId, true);
          }
          else
          {
            atb_.Accumulate();
          }
          break;
        case State.Command:
          atb_.Accumulate();
          break;
        case State.TargetSelect:
          atb_.Accumulate();
          break;
        case State.Win:
          battleCommand_.Hidden();
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

      if (this.actionQueue_.DequeueDoneId() == -1)
      {
        BattleCommandInfo bci = actionQueue_.Dequeue();
        if (bci != null)
        {
          int damage = data_.Friend(bci.from).atk();
          UnitInfo info = data_.MonsInfo(bci.to);
          info.hp -= damage;
          JsTrans.console_log("Atk: from(" + bci.from + ") to(" + bci.to + ") " + damage + ":" + info.hp);
          if (info.hp <= 0)
          {
            Data.ConstStatusMons mons = data_.Monster(bci.to);
            acquisition_.Acquire(mons.Exp, mons.Gold, mons.Item, mons.ItemPer);
          }
          //■
          effect_.Ready(monitor_, monsters_.GetMargin(bci.to), monsters_.GetImage(bci.to), damage, info.hp <= 0);
        }
      }
      else
      {
        if (!effect_.FrameOne())
        {
          ui_.SetActiveMark(this.actionQueue_.DequeueDoneId(), false);
          atb_.SetGaugeValueZero(this.actionQueue_.DequeueDoneId());
          if (data_.MonsFirstAlive() == -1)
          {
            state_ = State.Win;
            Queue<string> ss = new Queue<string>();
            ss.Enqueue("たたかいにかった！");
            acquisition_.AppendExpText(ss);

            Data.Status[] friends = Data.DataSC.Friends();
            foreach(var friend in data_.FriendsWritable())
            {
              int levelup = friend.experiencePlus(acquisition_.Exp);//経験値
              if (levelup > 0)
              {
                ss.Enqueue(friend.name() + "はレベルが" + 
                  (levelup == 1 ? "" : levelup.ToString()) + "あがった！");
              }
            }

            acquisition_.AppendGoldAndItemText(ss);
            Data.DataSC.Basic().goldPlus(acquisition_.Gold);//お金

            int item, num;
            for (int i = 0; acquisition_.Item(i, out item, out num); i++)
            {
              Data.DataSC.Bag().itemPlus(item, num);//お宝
            }
            exWindow_.SetTopStrings(ss);
          }
          this.actionQueue_.ResetDequeueDoneId();
        }
      }
    }
    public bool IsTargetSelect()
    {
      return state_ == State.TargetSelect;
    }
  }
}
