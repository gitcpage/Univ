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

namespace Univ
{
  internal class Battle : IRun
  {
    MainPage mainPage_;
    FrameManager frameManager_;
    Grid monitor_;   // 描画用
    Grid monitorBg_; // 背景描画用

    public Battle(MainPage mainPage)
    {
      mainPage_ = mainPage;
      frameManager_ = mainPage.GetFrameTimer();
      monitor_ = mainPage.GetMonitor();
      monitorBg_ = mainPage.GetMonitorBg();
    }
    public void Run()
    {
      mainPage_.BottomTextBySequence("IRunImplementsFade");

      //初期化処理
      TextBlock tb_ = new TextBlock();
      tb_.Text = "RunImplementsFade";
      tb_.Foreground = UnivLib.GetBrush(Colors.Red);
      monitor_.Children.Add(tb_);

      Image monster1 = UnivLib.ImageInstance(130, 200, "battle/mon1.png", "mon1", "mon");
      monitor_.Children.Add(monster1);
      Image monster2 = UnivLib.ImageInstance(350, 200, "battle/mon2.png", "mon1", "mon");
      monitor_.Children.Add(monster2);

      Image bg = UnivLib.ImageInstance("battle/forest1.jpg", "backGround", "backGround");
      monitorBg_.Children.Add(bg);

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
