using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System; // VirtualKey
using Windows.UI.Xaml.Controls; // Grid
using Windows.UI.Xaml; // Thickness
using Windows.UI.Xaml.Input; // PointerRoutedEventArgs
using Windows.UI.Xaml.Media; // Stretch
using Windows.UI;

namespace Univ
{
  internal class Opening : IRun
  {
    MainPage mainPage_;
    FrameManager frameManager_;
    Grid monitor_;   // 描画用
    Grid monitorBg_; //背景描画用

    int selected_ = -1;
    int waitRemainFrame_;
    public int Selected { get { return selected_; } }

    public Opening(MainPage mainPage)
    {
      mainPage_ = mainPage;
      frameManager_ = mainPage.GetFrameManager();
      monitor_ = mainPage.GetMonitor();
      monitorBg_ = mainPage.GetMonitorBg();
    }
    public void Run()
    {
      mainPage_.BottomTextBySequence("Opening");
      selected_ = -1;
      waitRemainFrame_ = 20;
      monitorBg_.Background = UnivLib.GetBrush(Colors.Orange);

      TextBlock textBlock = new TextBlock();
      textBlock.HorizontalAlignment = HorizontalAlignment.Left;
      textBlock.VerticalAlignment = VerticalAlignment.Top;
      textBlock.Text = "もんちゃん";
      textBlock.Margin = new Thickness(100, 100, 0, 0);
      textBlock.FontSize = 60;
      textBlock.Foreground = UnivLib.GetBrush(14, 77, 108);
      monitor_.Children.Add(textBlock);
      //Border bdr = UnivLib.WrapBorder(textBlock, monitor_, 400, 0, 0);

      TextBlock tbNew = new TextBlock();
      tbNew.HorizontalAlignment = HorizontalAlignment.Left;
      tbNew.VerticalAlignment = VerticalAlignment.Top;
      tbNew.Text = "はじめから";
      //tbNew.Margin = new Thickness(150, 200, 0, 0);
      tbNew.FontSize = 60;
      tbNew.Foreground = UnivLib.GetBrush(14, 77, 108);
      Border bdrNew = UnivLib.WrapBorder(tbNew, monitor_, 400, 150, 200);
      bdrNew.Tapped += (Object sender, TappedRoutedEventArgs e) =>
      {
        selected_ = 0;
      };

      TextBlock tbContinue = new TextBlock();
      tbContinue.HorizontalAlignment = HorizontalAlignment.Left;
      tbContinue.VerticalAlignment = VerticalAlignment.Top;
      tbContinue.Text = "つづきから";
      //tbContinue.Margin = new Thickness(150, 300, 0, 0);
      tbContinue.FontSize = 60;
      tbContinue.Foreground = UnivLib.GetBrush(14, 77, 108);
      Border bdrContinue = UnivLib.WrapBorder(tbContinue, monitor_, 400, 150, 300);
      bdrContinue.Tapped += (Object sender, TappedRoutedEventArgs e) =>
      {
        selected_= 1;
      };


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
      if (selected_ != -1)
      {
        if (waitRemainFrame_ > 0)
        {
          waitRemainFrame_--;
          return;
        }
        else
        {
          frameManager_.EnterSequenceFadeOut(OnFadeOuted);
          return;
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
