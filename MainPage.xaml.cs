using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging; // BitmapImage
using Windows.UI.Core; // CoreVirtualKeyStates
using Windows.System;
using Windows.UI.ViewManagement; // ApplicationView

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace Univ
{
  /// <summary>
  /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
  /// </summary>
  public sealed partial class MainPage : Page
  {
    //private DispatcherTimer timer_ = null;
    FrameTimer frameTimer_ = new FrameTimer();
    int count_;
    int delta = 4;
    const int kOneFrameTimeMs = 40;
    int x = 0;

    Field field_;

    public MainPage()
    {
      this.InitializeComponent();

      //https://social.msdn.microsoft.com/Forums/en-US/97556dc2-b01c-43a4-8c6a-9a3fd51d9151/updating-imagesource-causes-flickering?forum=wpdevelop
      this.NavigationCacheMode = NavigationCacheMode.Required;

      // 初期化処理
      field_ = new Field(frameTimer_, this.idMonitor);
      field_.Run();
      //frameTimer_.setTimeOut(FrameOne, kOneFrameTimeMs);
    }
    void FrameOne(object sender, object e)
    {
      // カウントを1加算
      this.count_ += delta;
      if (this.count_ > 200)
        delta = -4;
      else if (this.count_ <= 0)
        delta = 4;
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      Image image = new Image();
      Uri uri = new Uri("ms-appx:///Assets/ceres44.png");
      BitmapImage bitmapImage = new BitmapImage(uri);
      image.Source = bitmapImage;
      image.Name = "idImg1";
      image.Margin = new Thickness(x, 100, 0, 0);
      image.HorizontalAlignment = HorizontalAlignment.Left;
      image.VerticalAlignment = VerticalAlignment.Top;
      x += 10;
      image.Stretch = Stretch.None;
      this.idMonitor.Children.Add(image);
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
      if (frameTimer_.IsRunning)
        frameTimer_.Stop();
      else
        frameTimer_.setTimeOut(FrameOne, kOneFrameTimeMs);
    }
    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
      JsTrans.alert("aa");
    }
  }
}
