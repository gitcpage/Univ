
using Windows.System; // VirtualKey
using Windows.UI.Xaml.Controls; // Grid

namespace Univ
{
  internal class RunImplementsFade : IRun
  {
    MainPage mainPage_;
    FrameManager frameManager_;
    Grid monitor_;   // 描画用
    Grid monitorBg_; //背景描画用

    public RunImplementsFade(MainPage mainPage)
    {
      mainPage_ = mainPage;
      frameManager_ = mainPage.GetFrameTimer();
      monitor_ = mainPage.GetMonitor();
    }
    public void Run()
    {
      mainPage_.BottomTextBySequence("IRunImplementsFade");

      //初期化処理
      TextBlock tb_ = new TextBlock();
      tb_.Text = "RunImplementsFade";
      monitor_.Children.Add(tb_);

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
