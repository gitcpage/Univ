using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System; // VirtualKey
using Windows.UI.Xaml.Controls; // Grid

namespace Univ
{
  internal class Loading : IRun
  {
    MainPage mainPage_;
    FrameManager frameManager_;
    Grid monitor_;   // 描画用
    Grid monitorBg_; //背景描画用

    public Loading(MainPage mainPage)
    {
      mainPage_ = mainPage;
      frameManager_ = mainPage.GetFrameManager();
      monitor_ = mainPage.GetMonitor();
      monitorBg_ = mainPage.GetMonitorBg();
    }
    public void Run()
    {
      mainPage_.BottomTextBySequence("Loading");

      //フレームループ開始
      frameManager_.ChangeSequence(FrameOne);
    }
    public void FrameOne(object senderDispatcherTimer, object eNull)
    {
      // スペースキーで戻る
      if (Data.Loader.LoadingState_ == Data.LoadingState.Loaded)
      {
        frameManager_.ExitSequence();
      }
    }
  }
}
