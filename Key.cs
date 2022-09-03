using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;

namespace Univ
{
  //FrameManagerクラス内部で使用する。
  //アルファベットキー設定はメニューのカスタムで設定し、FrameManagerで制御する。
  internal class Key
  {
    byte[] jkey_ = new byte[256];

    public void Update()
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
    }
    public bool IsKeyDown(VirtualKey keyCode1,
                          VirtualKey keyCode2 = VirtualKey.None,
                          VirtualKey keyCode3 = VirtualKey.None)
    {
      if (jkey_[(int)keyCode1] > 0)
      {
        return true;
      }
      if (jkey_[(int)keyCode2] > 0)
      {
        return true;
      }
      if (jkey_[(int)keyCode3] > 0)
      {
        return true;
      }
      return false;
    }
    public bool IsKeyDownFirst(VirtualKey keyCode1, 
                               VirtualKey keyCode2 = VirtualKey.None,
                               VirtualKey keyCode3 = VirtualKey.None)
    {
      if (jkey_[(int)keyCode1] == 1)
      {
        return true;
      }
      if (jkey_[(int)keyCode2] == 1)
      {
        return true;
      }
      if (jkey_[(int)keyCode3] == 1)
      {
        return true;
      }
      return false;
    }
    public bool IsKeyDownRepeat(VirtualKey keyCode1,
                               VirtualKey keyCode2 = VirtualKey.None,
                               VirtualKey keyCode3 = VirtualKey.None)
    {
      if ((jkey_[(int)keyCode1] % 4) == 1 && jkey_[(int)keyCode1] != 5)
      {
        return true;
      }
      if ((jkey_[(int)keyCode2] % 4) == 1 && jkey_[(int)keyCode2] != 5)
      {
        return true;
      }
      if ((jkey_[(int)keyCode3] % 4) == 1 && jkey_[(int)keyCode3] != 5)
      {
        return true;
      }
      return false;
    }
  }//internal class Key
  public enum GameKey { Ok, Cancel, Change, Left, Right, Up, Down,
                        N1, N2, N3, N4, N5, N6, N7, N8, N9}
}
