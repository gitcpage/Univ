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
  internal class FrameTimer
  {
    EventHandler<object> handler_ = null;
    Stack<EventHandler<object>> handlerStack_ = new Stack<EventHandler<object>>();
    DispatcherTimer timer_;
    bool isRunning = false;

    public byte[] jkey_ = new byte[256];
    int frameCount_ = 0;

    // kOneFrameTimeMs は40で固定する。必要な場合のみ変更する。20220708
    const int kOneFrameTimeMs = 40;
    bool isFast_ = false;

    static bool doSoftReset_ = false;
    MainPage mainPage_;

    private void Update(object sender, object e)
    {
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
      frameCount_++;
    }
    public FrameTimer(MainPage mainPage)
    {
      mainPage_ = mainPage;
      timer_ = new DispatcherTimer();
      // setTimeOut()の第１引数より先に実行されてほしいが保証はない。
      timer_.Tick += Update;
    }
    public void setTimeOut(EventHandler<object> handler)
    {
      timer_.Tick -= handler_;
      handler_ = handler;
      timer_.Tick += handler_;
      int oneFrameTimeMs = isFast_ ? kOneFrameTimeMs / 2 : kOneFrameTimeMs;
      timer_.Interval = TimeSpan.FromMilliseconds(oneFrameTimeMs);
      if (!isRunning)
      {
        isRunning = true;
        timer_.Start();
      }
    }
    public void PushStackHandler(
      EventHandler<object> handler_,
      IRun iRun)
    {
      handlerStack_.Push(handler_);
      iRun.Run(mainPage_, this);
    }
    public void PopStackHandler()
    {
      EventHandler<object> e = handlerStack_.Pop();
      setTimeOut(e);
    }
    public void Reset()
    {
      doSoftReset_ = true;
    }
    static public bool ResettingOnce()
    {
      bool ret = doSoftReset_;
      doSoftReset_ = false;
      return ret;
    }
    public void Pause()
    {
      timer_.Stop();
      isRunning = false;
    }
    public void Start(bool isFast = false)
    {
      isRunning = true;
      isFast_ = isFast;
      int oneFrameTimeMs = isFast_ ? kOneFrameTimeMs/2 : kOneFrameTimeMs;
      timer_.Interval = TimeSpan.FromMilliseconds(oneFrameTimeMs);
      timer_.Start();
    }
    public bool IsRunning
    {
      get { return isRunning; }
    }

    // ▲▲▲ キー関連処理 ▲▲▲
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
    // ▼▼▼ キー関連処理 ▼▼▼

    // フレームカウント
    public int FrameCount
    {
      get { return frameCount_; }
    }
  }
  // FrameTimer frameTimer_ = new FrameTimer();
  // frameTimer_.setTimeOut(FrameOne, kOneFrameTimeMs);
}
