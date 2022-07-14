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
using Windows.UI.Input; //PointerPoint
// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace Univ
{
  /// <summary>
  /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
  /// </summary>
  public sealed partial class MainPage : Page
  {
    FrameManager frameManager_;
    public Point mousePoint;
    public bool isMouseLDown = false;

    //Field field_;
    //RunImplements runImplements_;
    //RunImplementsFade runImplementsFade_;

    string BottomTextFormer = ""; // シーケンス遷移用に使用する
    string BottomTextLatter = ""; // マウス用に使用する

    Data.CharsWritable[] chars_;

    public MainPage()
    {
      this.InitializeComponent();

      //https://social.msdn.microsoft.com/Forums/en-US/97556dc2-b01c-43a4-8c6a-9a3fd51d9151/updating-imagesource-causes-flickering?forum=wpdevelop
      this.NavigationCacheMode = NavigationCacheMode.Required;
      JsTrans.s_mainPage = this;

      // 1.0でidMonitorは見えなくなる。フェードインスタートの場合は1.0にする。
      this.idMonitorFade.Background.Opacity = 1.0;
      // Visibility.Collapsedのときは this.idMonitor要素にマウスイベントが発生する。
      this.idMonitorFade.Visibility = Visibility.Visible;

      // データインスタンス化処理
      chars_ = Data.CharsWritable.Instances(5);

      // フレーム初期化処理。
      // FrameManager で FrameOne を呼び出すのは整合性として気持ち悪いので、
      // MainPageコンストラクタで呼び出している。
      frameManager_ = new FrameManager(this, FrameOne);
      FrameOne(null, null);
    }
    //  △△△シーケンス遷移で渡すオブジェクトへのアクセス△△△
    public FrameManager GetFrameTimer()
    {
      return this.frameManager_;
    }
    public Grid GetMonitor()
    {
      return this.idMonitor;
    }
    public Grid GetMonitorBg()
    {
      return this.idMonitorBg;
    }
    // ▽▽▽シーケンス遷移で渡すオブジェクトへのアクセス▽▽▽
    private void Button_Click_Clear(object sender, RoutedEventArgs e)
    {
      ConsoleText = "";
    }
    //JsTransクラスでconsole_log()などからアクセスられるプロパティ。
    public string ConsoleText
    {
      get { return this.txtConsole.Text; }
      set { this.txtConsole.Text = value; }
    }
    // ■エントリーポイント
    void FrameOne(object sender, object e)
    {
      chars_[0].name("ホーン");
      chars_[0].hp(24);
      chars_[1].name("サンナン");
      chars_[1].hp(27);
      chars_[2].name("ナル");
      chars_[2].hp(21);
      chars_[3].name("リゼッタ");
      chars_[3].hp(22);
      chars_[4].name("アスラa");
      chars_[4].hp(9999);
      frameManager_.EnterSequence(FrameOne, new Battle(this, chars_));
    }
    // △△△モニタアクセス共通処理△△△
    public void Clear()
    {
      this.idMonitor.Children.Clear();
      this.idMonitorBg.Children.Clear();
    }
    // ▽▽▽モニタアクセス共通処理▽▽▽
    // △△△フェード処理△△△
    double fade_sum_ = 0;
    public void RunFadeOut()
    {
      if (this.idMonitorFade.Background.Opacity != 0.0) JsTrans.Assert(false,
        "フェードアウトできません。\nthis.idMonitorFade.Background.Opacityが0.0であることを確認してください。" +
        this.idMonitorFade.Background.Opacity.ToString());
      fade_sum_ = 0;
      this.idMonitorFade.Visibility = Visibility.Visible;
      frameManager_.ChangeSequence(FadeOut);
    }
    void FadeOut(object sender, object e)
    {
      int ms = 200;
      fade_sum_ += FrameManager.kOneFrameTimeMs;
      double opacity = (double)fade_sum_ / ms;
      if (opacity >= 1.0)
      {
        this.idMonitorFade.Background.Opacity = 1.0;
        frameManager_.ExitSequence();
      }
      else
      {
        this.idMonitorFade.Background.Opacity = opacity;
      }
    }
    public void RunFadeIn()
    {
      if (this.idMonitorFade.Background.Opacity != 1.0) JsTrans.Assert(false,
        "フェードインできません。\nthis.idMonitorFade.Background.Opacityが1.0であることを確認してください。\n" +
        this.idMonitorFade.Background.Opacity.ToString());
      fade_sum_ = 200;
      frameManager_.ChangeSequence(FadeIn);
    }
    void FadeIn(object sender, object e)
    {
      int ms = 200;
      fade_sum_ -= FrameManager.kOneFrameTimeMs;
      double opacity = (double)fade_sum_ / ms;
      if (opacity <= 0.0)
      {
        this.idMonitorFade.Background.Opacity = 0.0;
        this.idMonitorFade.Visibility = Visibility.Collapsed;
        frameManager_.ExitSequence();
      }
      else
      {
        this.idMonitorFade.Background.Opacity = opacity;
      }
    }
    // ▽▽▽フェード処理▽▽▽

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
    }
    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
    }

    private void Button_Click_4(object sender, RoutedEventArgs e)
    {
      UnivLib.MsgBitmapPaths();
    }

    private void AppBarReset_Click(object sender, RoutedEventArgs e)
    {
      frameManager_.Reset();
    }
    private void AppBarPause_Click(object sender, RoutedEventArgs e)
    {
      frameManager_.Pause();
      this.AppBarPause.IsEnabled = false;
      this.AppBarPlay.IsEnabled = true;
      this.AppBarFastPlay.IsEnabled = true;
    }

    private void AppBarPlay_Click(object sender, RoutedEventArgs e)
    {
      this.AppBarPause.IsEnabled = true;
      this.AppBarPlay.IsEnabled = false;
      this.AppBarFastPlay.IsEnabled = true;
      frameManager_.Start();
    }
    private void AppBarFastPlay_Click(object sender, RoutedEventArgs e)
    {
      this.AppBarPause.IsEnabled = true;
      this.AppBarPlay.IsEnabled = true;
      this.AppBarFastPlay.IsEnabled = false;
      frameManager_.Start(true);
    }
    public void BottomTextBySequence(string name)
    {
      BottomTextFormer = name;
      this.AppBar.Text = BottomTextFormer + " | " + BottomTextLatter;
    }

    void BottomTextByMouse()
    {
      BottomTextLatter = (int)mousePoint.X + ":" + (int)mousePoint.Y + (isMouseLDown ? " Down" : "");
      this.AppBar.Text = BottomTextFormer + " | " + BottomTextLatter;
    }
    private void idMonitor_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
      PointerPoint pp = e.GetCurrentPoint((UIElement)sender);
      mousePoint = new Point(pp.Position.X, pp.Position.Y);
      BottomTextByMouse();
    }

    private void idMonitor_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
      isMouseLDown = true;
      BottomTextByMouse();
    }

    private void idMonitor_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
      isMouseLDown = false;
      BottomTextByMouse();
    }
  }
}
