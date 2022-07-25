using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Univ
{
  public class IntXY
  {
    public int X;
    public int Y;
  }
}

namespace Univ.Data
{
  internal class Basic
  {
    Basic()
    {
      s_gold = 0;
      s_msTime = 0;
    }
    static int s_gold;
    static long s_msTime;
    //static IntXY s_fieldchar;
    //static IntXY s_field;

    public int gold() { return s_gold; }
    public void gold(int num) { s_gold = num; }
    public int goldPlus(int num = 1) { s_gold += num; JsTrans.Assert(s_gold >= 0, "gold"); return s_gold; }
    public long msTime() { return s_msTime; }
    public long msTimeMinutes() { return s_msTime/1000/60; }
    public void msTime(long ms) { s_msTime = ms; }
    public long msTimePlus(long ms = 1) { s_msTime += ms; JsTrans.Assert(s_msTime >= 0, "msTime"); return s_msTime; }
    
    // ▲▲▲シングルトンパターン▲▲▲
    static private Basic instance = null;
    static public Basic Instance
    {
      get
      {
        //Univ.JsTrans.console_log("static public Basic Instance");
        if (instance == null)
        {
          JsTrans.window_close("セットアップ前に Basic.Instance が呼び出されました。");
        }
        return instance;
      }
    }
    static public Basic Setup()
    {
      //Univ.JsTrans.console_log("static public Basic Setup()");
      if (instance == null)
      {
        return instance = new Basic();
      }
      return null;
      //JsTrans.window_close("セットアップ済みなのに Basic.Setup() が呼び出されました。");
    }
    // ▼▼▼シングルトンパターン▼▼▼
  }
}
