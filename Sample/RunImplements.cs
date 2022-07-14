
using Windows.System; // VirtualKey
using Windows.UI.Xaml.Controls; // Grid

namespace Univ
{
  internal class RunImplements : IRun
  {
    MainPage mainPage_;
    FrameManager frameManager_;
    Grid monitor_;   // 描画用
    Grid monitorBg_; //背景描画用

    public RunImplements(MainPage mainPage)
    {
      mainPage_ = mainPage;
      frameManager_ = mainPage.GetFrameTimer();
      monitor_ = mainPage.GetMonitor();
      monitorBg_ = mainPage.GetMonitorBg();
    }
    public void Run()
    {
      mainPage_.BottomTextBySequence("IRunImplements");

      //初期化処理
      TextBlock tb_ = new TextBlock();
      tb_.Text = "RunImplements";
      monitor_.Children.Add(tb_);

      //フレームループ開始
      frameManager_.ChangeSequence(FrameOne);
    }
    public void FrameOne(object senderDispatcherTimer, object eNull)
    {
      if (frameManager_.ResettingOnce())
      {
        JsTrans.console_log("ソフトリセット");
        mainPage_.Clear();
        frameManager_.ExitSequence();
        return;
      }

      // スペースキーで戻る
      if (frameManager_.IsKeyDownFirst(VirtualKey.Space))
      {
        mainPage_.Clear();
        frameManager_.ExitSequence();
      }
    }
  }
}
