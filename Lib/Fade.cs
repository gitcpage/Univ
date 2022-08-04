using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Univ.Lib
{
  /*internal class Fade
  {
    static double s_fade_sum_;
    static Grid s_idMonitorFade;
    static FrameManager s_frameManager_;

    static bool isSetted = false;

    static public void Constructor(Grid idMonitorFade, FrameManager frameManager_)
    {
      JsTrans.Assert(!isSetted, "Lib.Fade.cs Constructor");

      s_idMonitorFade = idMonitorFade;
      s_frameManager_ = frameManager_;

      isSetted = true;
    }

    static public void RunFadeOut()
    {
      if (s_idMonitorFade.Background.Opacity != 0.0) JsTrans.Assert(false,
        "フェードアウトできません。\nthis.idMonitorFade.Background.Opacityが0.0であることを確認してください。" +
        s_idMonitorFade.Background.Opacity.ToString());
      s_fade_sum_ = 0;
      s_idMonitorFade.Visibility = Visibility.Visible;
      s_frameManager_.ChangeSequence(FadeOut);
    }
    static void FadeOut(object sender, object e)
    {
      int ms = 200;
      s_fade_sum_ += FrameManager.kOneFrameTimeMs;
      double opacity = (double)s_fade_sum_ / ms;
      if (opacity >= 1.0)
      {
        s_idMonitorFade.Background.Opacity = 1.0;
        s_frameManager_.ExitSequence();
      }
      else
      {
        s_idMonitorFade.Background.Opacity = opacity;
      }
    }
    static public void RunFadeIn()
    {
      s_idMonitorFade.Background.Opacity = 1.0;
      if (s_idMonitorFade.Background.Opacity != 1.0)
      {
        JsTrans.Assert(false,
        "フェードインできません。\nthis.idMonitorFade.Background.Opacityが1.0であることを確認してください。\n" +
        s_idMonitorFade.Background.Opacity.ToString());
        s_frameManager_.ExitSequence();
        return;
      }
      s_fade_sum_ = 200;
      s_frameManager_.ChangeSequence(FadeIn);
    }
    static void FadeIn(object sender, object e)
    {
      int ms = 200;
      s_fade_sum_ -= FrameManager.kOneFrameTimeMs;
      double opacity = (double)s_fade_sum_ / ms;
      if (opacity <= 0.0)
      {
        s_idMonitorFade.Background.Opacity = 0.0;
        s_idMonitorFade.Visibility = Visibility.Collapsed;
        s_frameManager_.ExitSequence();
      }
      else
      {
        s_idMonitorFade.Background.Opacity = opacity;
      }
    }
  }*/
}
