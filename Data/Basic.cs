using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System;

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

    //Field>Field.csファイルの Run()メソッドで使用される。
    static string s_fieldName;
    static int s_fieldX;
    static int s_fieldY;

    public static VirtualKey s_OkKey = VirtualKey.M;
    public static VirtualKey s_CancelKey = VirtualKey.N;
    public static VirtualKey s_ChangeKey = VirtualKey.A;
    public static VirtualKey s_LeftKey = VirtualKey.S;
    public static VirtualKey s_RightKey = VirtualKey.D;
    public static VirtualKey s_UpKey = VirtualKey.E;
    public static VirtualKey s_DownKey = VirtualKey.X;
    static public void ResetKey()
    {
      s_OkKey = VirtualKey.M;
      s_CancelKey = VirtualKey.N;
      s_ChangeKey = VirtualKey.A;
      s_LeftKey = VirtualKey.S;
      s_RightKey = VirtualKey.D;
      s_UpKey = VirtualKey.E;
      s_DownKey = VirtualKey.X;
    }

    public int gold() { return s_gold; }
    public void gold(int num) { s_gold = num; }
    public int goldPlus(int num = 1) { s_gold += num; JsTrans.Assert(s_gold >= 0, "gold_"); return s_gold; }
    //public long msTime() { return s_msTime; }
    public long msTimeMinutes() { return s_msTime/1000/60; }
    public string msTimeString()
    {
      long minutes = s_msTime / 1000 / 60;
      long hour = minutes / 60;
      minutes %= 60;
      if (minutes < 10)
        return hour.ToString() + ":0" + minutes.ToString();
      else
        return hour.ToString() + ":" + minutes.ToString();
    }
    public void msTime(long ms) { s_msTime = ms; }
    public long msTimePlus(long ms) { s_msTime += ms; JsTrans.Assert(s_msTime >= 0, "msTime"); return s_msTime; }

    public void SetField(string name, int x, int y)
    {
      s_fieldName = name;
      s_fieldX = x;
      s_fieldY = y;
    }
    public void GetField(out string name, out int x, out int y)
    {
      name = s_fieldName;
      x = s_fieldX;
      y = s_fieldY;
    }
    public void Initialize()
    {
      gold(0);
      msTime(0);
      s_fieldName = "フィールド1";
      s_fieldX = 7;
      s_fieldY = 6;
    }
    public void Load(string txt)
    {
      string[] rows = txt.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      string[] demTab = new string[1];
      demTab[0] = "\t";
      for (int i = 0; i < rows.Length; i++)
      {
        //string row = rows[i];
        string[] cells = rows[i].Split("\t");
        switch (cells[0])
        {
          case "Gold":
            gold(int.Parse(cells[1]));
            break;
          case "Time":
            msTime(long.Parse(cells[1]));
            break;
          case "FieldName":
            s_fieldName = cells[1];
            break;
          case "FieldX":
            s_fieldX = int.Parse(cells[1]);
            break;
          case "FieldY":
            s_fieldY = int.Parse(cells[1]);
            break;
          case "OkKey":
            s_OkKey = (VirtualKey)cells[1][0];
            break;
          case "CancelKey":
            s_CancelKey = (VirtualKey)cells[1][0];
            break;
          case "ChangeKey":
            s_ChangeKey = (VirtualKey)cells[1][0];
            break;
          case "LeftKey":
            s_LeftKey = (VirtualKey)cells[1][0];
            break;
          case "RightKey":
            s_RightKey = (VirtualKey)cells[1][0];
            break;
          case "UpKey":
            s_UpKey = (VirtualKey)cells[1][0];
            break;
          case "DownKey":
            s_DownKey = (VirtualKey)cells[1][0];
            break;
        }
      }
    }

    public string TextForSave()
    {
      string s = "";
      s += "Gold\t" + gold().ToString() + Environment.NewLine;
      s += "Time\t" + s_msTime.ToString() + Environment.NewLine;
      s += "FieldName\t" + s_fieldName + Environment.NewLine;
      s += "FieldX\t" + s_fieldX + Environment.NewLine;
      s += "FieldY\t" + s_fieldY + Environment.NewLine;
      s += Environment.NewLine;
      s += "OkKey\t" + s_OkKey + Environment.NewLine;
      s += "CancelKey\t" + s_CancelKey + Environment.NewLine;
      s += "ChangeKey\t" + s_ChangeKey + Environment.NewLine;
      s += "LeftKey\t" + s_LeftKey + Environment.NewLine;
      s += "RightKey\t" + s_RightKey + Environment.NewLine;
      s += "UpKey\t" + s_UpKey + Environment.NewLine;
      s += "DownKey\t" + s_DownKey + Environment.NewLine;
      return s;
    }

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
