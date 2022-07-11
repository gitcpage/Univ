﻿using System;
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
  public sealed partial class MainPage : Page, IRun
  {
    //private DispatcherTimer timer_ = null;
    FrameTimer frameTimer_;
    public Point mousePoint;
    public bool isMouseLDown = false;

    Field field_;

    string BottomTextFormer = ""; // シーケンス遷移用に使用する
    string BottomTextLatter = ""; // マウス用に使用する

    public MainPage()
    {
      this.InitializeComponent();

      //https://social.msdn.microsoft.com/Forums/en-US/97556dc2-b01c-43a4-8c6a-9a3fd51d9151/updating-imagesource-causes-flickering?forum=wpdevelop
      this.NavigationCacheMode = NavigationCacheMode.Required;
      JsTrans.s_mainPage = this;

      this.idMonitorFade.Background.Opacity = 1.0; // 0.0で透明
      //Canvas.SetZIndex(this.idMonitorFade, 100000);

      // 初期化処理
      frameTimer_ = new FrameTimer(this);
      //frameTimer_.PushStackHandler(FrameOne, this);
      frameTimer_.setTimeOut(FrameOne);
    }
    //  △△△シーケンス遷移で渡すオブジェクトへのアクセス△△△
    public FrameTimer GetFrameTimer()
    {
      return this.frameTimer_;
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
    void FrameOne(object sender, object e)
    {
      //if (frameTimer_.IsKeyDown(VirtualKey.Space))
      {
        field_ = new Field(this/*frameTimer_, this.idMonitor, monitorBg: this.idMonitorBg*/);
        frameTimer_.PushStackHandler(FrameOne, field_);
      }
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
    void FrameOneFadeOut(object sender, object e)
    {
      frameTimer_.PushStackHandler(FrameOne, field_);
    }
    public void Run()
    {
      fade_sum_ = 0;
      //frameTimer_.setTimeOut(FrameOne);
      if (this.idMonitorFade.Background.Opacity == 1.0)
      {
        fade_sum_ = 200;
        frameTimer_.setTimeOut(FadeIn);
      }
      else
      {
        fade_sum_ = 0;
        frameTimer_.setTimeOut(FadeOut);
      }
    }
    void FadeOut(object sender, object e)
    {
      int ms = 200;
      fade_sum_ += FrameTimer.kOneFrameTimeMs;
      double opacity = (double)fade_sum_ / ms;
      if (opacity >= 1.0)
      {
        this.idMonitorFade.Background.Opacity = 1.0;
        frameTimer_.PopStackHandler();
      }
      else
      {
        this.idMonitorFade.Background.Opacity = opacity;
      }
    }
    void FadeIn(object sender, object e)
    {
      int ms = 200;
      fade_sum_ -= FrameTimer.kOneFrameTimeMs;
      double opacity = (double)fade_sum_ / ms;
      if (opacity <= 0.0)
      {
        this.idMonitorFade.Background.Opacity = 0.0;
        frameTimer_.PopStackHandler();
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
      frameTimer_.Reset();
    }
    private void AppBarPause_Click(object sender, RoutedEventArgs e)
    {
      frameTimer_.Pause();
      this.AppBarPause.IsEnabled = false;
      this.AppBarPlay.IsEnabled = true;
      this.AppBarFastPlay.IsEnabled = true;
    }

    private void AppBarPlay_Click(object sender, RoutedEventArgs e)
    {
      this.AppBarPause.IsEnabled = true;
      this.AppBarPlay.IsEnabled = false;
      this.AppBarFastPlay.IsEnabled = true;
      frameTimer_.Start();
    }
    private void AppBarFastPlay_Click(object sender, RoutedEventArgs e)
    {
      this.AppBarPause.IsEnabled = true;
      this.AppBarPlay.IsEnabled = true;
      this.AppBarFastPlay.IsEnabled = false;
      frameTimer_.Start(true);
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
