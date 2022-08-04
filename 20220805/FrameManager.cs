using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Core; // CoreWindow, CoreVirtualKeyStates
using Windows.System; // VirtualKey

namespace Univ
{
  // キー押下処理とフレームカウントも行う
  // CS0051コンパイルエラーを防ぐため、アクセス修飾子を internal から public に変更。
  public class FrameManager
  {
    EventHandler<object> handler_ = null;
    Stack<EventHandler<object>> sequenceStack_ = new Stack<EventHandler<object>>();
    DispatcherTimer timer_ = null;
    //bool isRunning = false;

    EventHandler<object> mainFrameOne_;

    byte[] jkey_ = new byte[256];
    int frameCount_ = 0;

    // kOneFrameTimeMs は40で固定する。必要な場合のみ変更する。20220708
    public const int kOneFrameTimeMs = 40; // from MainPage.FadeOut
    bool isFast_ = false;

    //bool doSoftReset_ = false;
    MainPage mainPage_;

    bool enableFadeOut = true;
    
    int isMouseLDownCount = 0;

    void Initialize()
    {
      if (timer_ != null)
      {
        timer_.Stop();
      }

      timer_ = new DispatcherTimer();
      // ChangeSequence()の第１引数より先に実行されてほしいが保証はない。
      timer_.Tick += (sender, e) => {
        CoreWindow coreWin = CoreWindow.GetForCurrentThread();
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
        }

        if (mainPage_.isMouseLDown)
        {
          isMouseLDownCount++;
        }
        else
        {
          isMouseLDownCount = 0;
        }

        frameCount_++;
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
      //if (!isRunning)
      {
        //isRunning = true;
        timer_.Start();
      }
    }
    internal void EnterSequence(EventHandler<object> returnMethod,IRun iRun)
    {
      sequenceStack_.Push(returnMethod);
      iRun.Run();
    }
    public void EnterSequenceFadeIn(EventHandler<object> returnMethod)
    {
      sequenceStack_.Push(returnMethod);
      mainPage_.RunFadeIn();
    }
    public void EnterSequenceFadeOut(EventHandler<object> returnMethod)
    {
      sequenceStack_.Push(returnMethod);
      if (this.enableFadeOut)
        mainPage_.RunFadeOut();
      else
        ExitSequence();
    }
    public void ExitSequence()
    {
      EventHandler<object> e = sequenceStack_.Pop();
      ChangeSequence(e);
    }
    public void Reset()
    {
      JsTrans.console_log("ソフトリセット");
      mainPage_.Clear(true);
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
    public bool IsKeyDown(char keyCode)
    {
      if (jkey_[keyCode] > 0)
      {
        return true;
      }
      return false;
    }
    public bool IsKeyDown(VirtualKey keyCode)
    {
      if (jkey_[(int)keyCode] > 0)
      {
        return true;
      }
      return false;
    }

    public bool IsKeyDownFirst(char keyCode)
    {
      if (jkey_[keyCode] == 1)
      {
        jkey_[keyCode]++;
        return true;
      }
      return false;
    }
    public bool IsKeyDownFirst(VirtualKey keyCode)
    {
      if (jkey_[(int)keyCode] == 1)
      {
        return true;
      }
      return false;
    }
    public bool KeyDirection(out int x, out int y)
    {
      x = y = 0;
      if (IsKeyDown(VirtualKey.Left))
      { // ↑
        x = -1;
        return true;
      }
      else if (IsKeyDown(VirtualKey.Right))
      { // ↓
        x = 1;
        return true;
      }
      if (IsKeyDown(VirtualKey.Up))
      { // ↑
        y = -1;
        return true;
      }
      else if (IsKeyDown(VirtualKey.Down))
      { // ↓
        y = 1;
        return true;
      }
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
