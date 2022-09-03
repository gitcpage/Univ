using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Core; // CoreWindow, CoreVirtualKeyStates
using Windows.System; // VirtualKey
using Windows.UI.Xaml.Controls;
using Univ.Data;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Univ
{
  internal class Fade
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
  }


  // キー押下処理とフレームカウントも行う
  // CS0051コンパイルエラーを防ぐため、アクセス修飾子を internal から public に変更。
  public class FrameManager
  {
    EventHandler<object> handler_ = null;
    Stack<EventHandler<object>> sequenceStack_ = new Stack<EventHandler<object>>();
    DispatcherTimer timer_ = null;
    //bool isRunning = false;

    EventHandler<object> mainFrameOne_;

    //△△△キー制御△△△
    //byte[] jkey_ = new byte[256];
    Key key_ = new Key();
    VirtualKey keyOk_ = VirtualKey.M;
    VirtualKey keyCancel_ = VirtualKey.N;
    VirtualKey keyUp_ = VirtualKey.E;
    VirtualKey keyLeft_ = VirtualKey.S;
    VirtualKey keyRight_ = VirtualKey.D;
    VirtualKey keyDown_ = VirtualKey.X;
    VirtualKey keyAll_ = VirtualKey.A;
    //▽▽▽キー制御▽▽▽

    int frameCount_ = 0;
    // kOneFrameTimeMs は40で固定する。必要な場合のみ変更する。20220708
    public const int kOneFrameTimeMs = 40; // from MainPage.FadeOut
    bool isFast_ = false;

    //bool doSoftReset_ = false;
    MainPage mainPage_;

    bool enableFadeOut = true;
    
    int isMouseLDownCount = 0;

    public void Initialize()
    {
      if (timer_ != null)
      {
        timer_.Stop();
      }

      timer_ = new DispatcherTimer();
      // ChangeSequence()の第１引数より先に実行されてほしいが保証はない。
      timer_.Tick += (sender, e) => {
        /*CoreWindow coreWin = CoreWindow.GetForCurrentThread();
        for (int i = 0; i < jkey_.Length; i++)
        {
          var state = coreWin.GetKeyState((VirtualKey)i);
          if ((state & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down)
          {
            jkey_[i]++;
          }
          else
          {
            jkey_[i] = 0;
          }
        }*/
        key_.Update();

        if (mainPage_.isMouseLDown)
        {
          isMouseLDownCount++;
        }
        else
        {
          isMouseLDownCount = 0;
        }

        frameCount_++;
        mainPage_.UpdateAppBarFrameCountText(FrameCount);

        if (Data.Loader.loadingState == Data.LoadingState.Success)
        {
          Basic basic = Basic.Instance;
          basic.msTimePlus(kOneFrameTimeMs);
        }
      }; // timer_.Tick += (object sender, object e) => {

      //■
      sequenceStack_.Push(mainFrameOne_);
      ChangeSequence(mainFrameOne_);
    }

    public FrameManager(MainPage mainPage, EventHandler<object> MainFrameOne)
    {
      mainPage_ = mainPage;
      mainFrameOne_ = MainFrameOne;
      Initialize();
    }
    public void FadeOutEnable(bool enable = true)
    {
      this.enableFadeOut = enable;
    }
    //public void setTimeOut(EventHandler<object> method)
    public void ChangeSequence(EventHandler<object> method)
    {
      timer_.Tick -= handler_;
      handler_ = method;
      timer_.Tick += handler_;
      int oneFrameTimeMs = isFast_ ? kOneFrameTimeMs / 2 : kOneFrameTimeMs;
      timer_.Interval = TimeSpan.FromMilliseconds(oneFrameTimeMs);
      timer_.Start();
    }
    internal void EnterSequence(EventHandler<object> returnMethod,IRun iRun)
    {
      sequenceStack_.Push(returnMethod);
      iRun.Run();
    }
    public void EnterSequenceFadeIn(EventHandler<object> returnMethod)
    {
      sequenceStack_.Push(returnMethod);
      Fade.RunFadeIn();
    }
    public void EnterSequenceFadeOut(EventHandler<object> returnMethod)
    {
      sequenceStack_.Push(returnMethod);
      if (this.enableFadeOut)
        Fade.RunFadeOut();
      else
        ExitSequence();
    }
    public void ExitSequence()
    {
      EventHandler<object> e = sequenceStack_.Pop();
      ChangeSequence(e);
    }
    //※ニューゲーム処理を行う場合は、loader_.Reload(true); Initialize();とすること。
    public void Reset()
    {
      //JsTrans.console_log("ソフトリセット");
      mainPage_.ClearReload();
      Initialize();
    }
    /*public bool ResettingOnce()
    {
      bool ret = doSoftReset_;
      doSoftReset_ = false;
      return ret;
    }*/
    public void Pause()
    {
      timer_.Stop();
      //isRunning = false;
    }
    public void Start(bool isFast = false)
    {
      //isRunning = true;
      isFast_ = isFast;
      int oneFrameTimeMs = isFast_ ? kOneFrameTimeMs/2 : kOneFrameTimeMs;
      timer_.Interval = TimeSpan.FromMilliseconds(oneFrameTimeMs);
      timer_.Start();
    }

    // △△△ キー関連処理 △△△
    public bool IsCancelKeyDownFirstIgnoreAlpha()
    {
      return key_.IsKeyDownFirst(VirtualKey.Escape, VirtualKey.Delete);
    }
    public bool IsKeyDown(GameKey key)
    {
      switch (key)
      {
        case GameKey.Ok:
          return key_.IsKeyDown(keyOk_, VirtualKey.Enter, VirtualKey.Space);
        case GameKey.Cancel:
          return key_.IsKeyDown(keyCancel_, VirtualKey.Escape, VirtualKey.Delete);
        case GameKey.Change:
          return key_.IsKeyDown(keyAll_, VirtualKey.Control, VirtualKey.LeftControl);
        case GameKey.Left:
          return key_.IsKeyDown(keyLeft_, VirtualKey.Left);
        case GameKey.Right:
          return key_.IsKeyDown(keyRight_, VirtualKey.Right);
        case GameKey.Up:
          return key_.IsKeyDown(keyUp_, VirtualKey.Up);
        case GameKey.Down:
          return key_.IsKeyDown(keyDown_, VirtualKey.Down);
        case GameKey.N1:
          return key_.IsKeyDown(VirtualKey.Number1, VirtualKey.NumberPad1);
        case GameKey.N2:
          return key_.IsKeyDown(VirtualKey.Number2, VirtualKey.NumberPad2);
        case GameKey.N3:
          return key_.IsKeyDown(VirtualKey.Number3, VirtualKey.NumberPad3);
        case GameKey.N4:
          return key_.IsKeyDown(VirtualKey.Number4, VirtualKey.NumberPad4);
        case GameKey.N5:
          return key_.IsKeyDown(VirtualKey.Number5, VirtualKey.NumberPad5);
        case GameKey.N6:
          return key_.IsKeyDown(VirtualKey.Number6, VirtualKey.NumberPad6);
        case GameKey.N7:
          return key_.IsKeyDown(VirtualKey.Number7, VirtualKey.NumberPad7);
        case GameKey.N8:
          return key_.IsKeyDown(VirtualKey.Number8, VirtualKey.NumberPad8);
        case GameKey.N9:
          return key_.IsKeyDown(VirtualKey.Number9, VirtualKey.NumberPad9);
      }
      
      return false;
    }
    public bool IsKeyDownFirst(GameKey key)
    {
      switch (key)
      {
        case GameKey.Ok:
          return key_.IsKeyDownFirst(keyOk_, VirtualKey.Enter, VirtualKey.Space);
        case GameKey.Cancel:
          return key_.IsKeyDownFirst(keyCancel_, VirtualKey.Escape, VirtualKey.Delete);
        case GameKey.Change:
          return key_.IsKeyDownFirst(keyAll_, VirtualKey.Control, VirtualKey.LeftControl);
        case GameKey.Left:
          return key_.IsKeyDownFirst(keyLeft_, VirtualKey.Left);
        case GameKey.Right:
          return key_.IsKeyDownFirst(keyRight_, VirtualKey.Right);
        case GameKey.Up:
          return key_.IsKeyDownFirst(keyUp_, VirtualKey.Up);
        case GameKey.Down:
          return key_.IsKeyDownFirst(keyDown_, VirtualKey.Down);
        case GameKey.N1:
          return key_.IsKeyDownFirst(VirtualKey.Number1);
        case GameKey.N2:
          return key_.IsKeyDownFirst(VirtualKey.Number2);
        case GameKey.N3:
          return key_.IsKeyDownFirst(VirtualKey.Number3);
        case GameKey.N4:
          return key_.IsKeyDownFirst(VirtualKey.Number4);
        case GameKey.N5:
          return key_.IsKeyDownFirst(VirtualKey.Number5);
        case GameKey.N6:
          return key_.IsKeyDownFirst(VirtualKey.Number6);
        case GameKey.N7:
          return key_.IsKeyDownFirst(VirtualKey.Number7);
        case GameKey.N8:
          return key_.IsKeyDownFirst(VirtualKey.Number8);
        case GameKey.N9:
          return key_.IsKeyDownFirst(VirtualKey.Number9);
      }
      return false;
    }
    public bool IsKeyDownRepeat(GameKey key)
    {
      switch (key)
      {
        case GameKey.Ok:
          return key_.IsKeyDownRepeat(keyOk_, VirtualKey.Enter, VirtualKey.Space);
        case GameKey.Cancel:
          return key_.IsKeyDownRepeat(keyCancel_, VirtualKey.Escape, VirtualKey.Delete);
        case GameKey.Change:
          return key_.IsKeyDownRepeat(keyAll_, VirtualKey.Control, VirtualKey.LeftControl);
        case GameKey.Left:
          return key_.IsKeyDownRepeat(keyLeft_, VirtualKey.Left);
        case GameKey.Right:
          return key_.IsKeyDownRepeat(keyRight_, VirtualKey.Right);
        case GameKey.Up:
          return key_.IsKeyDownRepeat(keyUp_, VirtualKey.Up);
        case GameKey.Down:
          return key_.IsKeyDownRepeat(keyDown_, VirtualKey.Down);
        case GameKey.N1:
          return key_.IsKeyDownRepeat(VirtualKey.Number1);
        case GameKey.N2:
          return key_.IsKeyDownRepeat(VirtualKey.Number2);
        case GameKey.N3:
          return key_.IsKeyDownRepeat(VirtualKey.Number3);
        case GameKey.N4:
          return key_.IsKeyDownRepeat(VirtualKey.Number4);
        case GameKey.N5:
          return key_.IsKeyDownRepeat(VirtualKey.Number5);
        case GameKey.N6:
          return key_.IsKeyDownRepeat(VirtualKey.Number6);
        case GameKey.N7:
          return key_.IsKeyDownRepeat(VirtualKey.Number7);
        case GameKey.N8:
          return key_.IsKeyDownRepeat(VirtualKey.Number8);
        case GameKey.N9:
          return key_.IsKeyDownRepeat(VirtualKey.Number9);
      }
      return false;
    }
    public bool IsKeyDownFirst(GameKey key1, GameKey key2)
    {
      return IsKeyDownFirst(key1) || IsKeyDownFirst(key2);
    }
    public bool KeyDirection(out int x, out int y)
    {
      x = y = 0;
      if (IsKeyDown(GameKey.Left))
      { // ↑
        x = -1;
        return true;
      }
      else if (IsKeyDown(GameKey.Right))
      { // ↓
        x = 1;
        return true;
      }
      if (IsKeyDown(GameKey.Up))
      { // ↑
        y = -1;
        return true;
      }
      else if (IsKeyDown(GameKey.Down))
      { // ↓
        y = 1;
        return true;
      }
      return false;
    }
    public bool IsKeyDownAlpha(Char c)
    {
      if ('A' <= c && c <= 'Z')
        return key_.IsKeyDown((VirtualKey)c);
      return false;
    }
    // ▽▽▽ キー関連処理 ▽▽▽

    // マウス関連処理
    public int clientX { get { return (int)mainPage_.mousePoint.X; } }
    public int clientY { get { return (int)mainPage_.mousePoint.Y; } }
    public bool isMouseLDown { get { return mainPage_.isMouseLDown; } }
    public bool isMouseLDownFirst { get { return isMouseLDownCount == 1; } }

    // フレームカウント
    public int FrameCount
    {
      get { return frameCount_; }
    }
  }
  // FrameTimer frameTimer_ = new FrameTimer();
  // frameTimer_.setTimeOut(FrameOne, kOneFrameTimeMs);
}
