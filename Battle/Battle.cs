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

    BattleUI ui_;
    BattleNotify battleNotify_;

    Data.StatusWritable[] charsWritable_;

    public Battle(MainPage mainPage, Data.StatusWritable[] charsWritable)
    {
      mainPage_ = mainPage;
      charsWritable_ = charsWritable;
      frameManager_ = mainPage.GetFrameManager();
      monitor_ = mainPage.GetMonitor();
      monitorBg_ = mainPage.GetMonitorBg();

      battleNotify_ = new BattleNotify(Notify); 

      ui_ = new BattleUI(monitor_, battleNotify_);
    }
    void Notify(NotifyCode notifyCode)
    {
      if (notifyCode == NotifyCode.Left)
      {
        ui_.ShowCharsArrow(0);
      }
      JsTrans.alert("NotifyCode: " + notifyCode.ToString());
    }
    public void Run()
    {
      mainPage_.BottomTextBySequence("Battle");

      //初期化処理
      /*TextBlock tb_ = new TextBlock();
      tb_.Text = "RunImplementsFade";
      tb_.Foreground = UnivLib.GetBrush(Colors.Red);
      monitor_.Children.Add(tb_);*/

      //背景
      Image bg = UnivLib.ImageInstance("battle/forest1.jpg", "backGround"/*, "backGround"*/);
      monitorBg_.Children.Add(bg);

      //▲▲▲モンスターグラフィック▲▲▲
      Image monster1 = UnivLib.ImageInstance(160, 110, "battle/mon1.png", "mon1"/*, "mon"*/);
      monster1.Stretch = Stretch.Uniform;
      monster1.Width = 200;
      monitor_.Children.Add(monster1);
      Image monster2 = UnivLib.ImageInstance(160, 280, "battle/mon2.png", "mon2"/*, "mon"*/);
      monster2.Stretch = Stretch.Uniform;
      monster2.Width = 200;
      monitor_.Children.Add(monster2);
      //▼▼▼モンスターグラフィック▼▼▼

      ui_.RunChars();

      //▲▲▲下のウィンドウ▲▲▲
      Grid underGrid = new Grid();
      underGrid.Width = 800;
      underGrid.Height = 160;
      underGrid.Margin = new Thickness(0, 440, 0, 0);
      underGrid.Background = UnivLib.GetBrush(157,181,183);

      //▲▲モンスター一覧▲▲
      Grid monstersGrid = ui_.RunUnderGrid(7, 330);

      string[] monsterStrings = new string[5] { 
        "ブルーフォックス", "ブルーフォックス", "ブルーフォックス", "ブルーフォックス", "ブルーフォックス" };
      monstersGrid.Children.Add(ui_.RunStringPanel(monsterStrings));
      underGrid.Children.Add(monstersGrid);
      //▼▼モンスター一覧▼▼

      //▲▲キャラクター一覧▲▲
      Grid charsGrid = ui_.RunUnderGrid(341, 330);
      Data.Status[] dchars = Data.Status.Instances;
      //string[] charStrings = new string[5] {
      //  dchars[0].name(), dchars[1].name(), dchars[2].name(), dchars[3].name(), dchars[4].name() };
        //"ホーン", "サンナン", "ナル", "リゼッタ", "アスラ" };
      //charsGrid.Children.Add(ui_.RunStringPanel(charStrings));
      charsGrid.Children.Add(ui_.RunStringPanel(Data.Status.names()));
      /*string[] charHpStrings = new string[5] {
        "24", "27", "21", "22", "9999" };
      charsGrid.Children.Add(ui_.RunStringPanelRightAlignment(charHpStrings, 90, 50));*/
      charsGrid.Children.Add(ui_.RunStringPanelRightAlignment(Data.Status.hpStrings(), 90, 50));
      underGrid.Children.Add(charsGrid);

      underGrid.Children.Add(ui_.RunSliderPanel());
      //▼▼キャラクター一覧▼▼

      //▲▲コントローラー▲▲
      Grid controllerGrid = ui_.RunUnderGrid(678, 115);
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
      Grid commnadGrid = ui_.RunUnderGrid(170, 130);
      commnadGrid.Background = UnivLib.GetBrush(127, 201, 203);
      string[] charArrows = new string[5] {
        "➤", "➤", "➤", "➤", "➤" };
      commnadGrid.Children.Add(ui_.RunStringPanel(charArrows));
      string[] commandsString = new string[5] {
        "たたかう", "とくぎ", "まほう", "アイテム", "そうび" };
      commnadGrid.Children.Add(ui_.RunStringPanel(commandsString, 35));
      underGrid.Children.Add(commnadGrid);
      //▼▼コマンド▼▼

      monitor_.Children.Add(underGrid);
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
      // スペースキーで戻る
      if (frameManager_.IsKeyDownFirst(VirtualKey.Space))
      {
        frameManager_.EnterSequenceFadeOut(OnFadeOuted);
      }
    }
  }
}
