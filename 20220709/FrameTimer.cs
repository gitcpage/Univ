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
    DispatcherTimer timer_;
    bool isRunning = false;

    byte[] jkey_ = new byte[256];
    int frameCount_ = 0;

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
    public FrameTimer()
    {
      timer_ = new DispatcherTimer();
      // setTimeOut()の第１引数より先に実行されてほしいが保証はない。
      timer_.Tick += Update;
    }
    public void setTimeOut(EventHandler<object> handler, int oneFrameTimeMs = -1)
    {
      timer_.Tick -= handler_;
      handler_ = handler;
      timer_.Tick += handler_;
      if (oneFrameTimeMs > 0)
        this.timer_.Interval = TimeSpan.FromMilliseconds(oneFrameTimeMs);
      if (!isRunning)
      {
        isRunning = true;
        timer_.Start();
      }
    }
    public void Stop()
    {
      timer_.Stop();
      isRunning = false;
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
