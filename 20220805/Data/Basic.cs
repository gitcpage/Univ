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

    //Field>Field.csファイルの Run()メソッドで使用される。
    static string s_fieldName;
    static int s_fieldX;
    static int s_fieldY;


    public int gold() { return s_gold; }
    public void gold(int num) { s_gold = num; }
    public int goldPlus(int num = 1) { s_gold += num; JsTrans.Assert(s_gold >= 0, "gold"); return s_gold; }
    public long msTime() { return s_msTime; }
    public long msTimeMinutes() { return s_msTime/1000/60; }
    public void msTime(long ms) { s_msTime = ms; }
    public long msTimePlus(long ms = 1) { s_msTime += ms; JsTrans.Assert(s_msTime >= 0, "msTime"); return s_msTime; }

    public void SetField(ref string name, ref int x, ref int y)
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
      s_fieldName = "初期フィールド";
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
        }
      }
    }

    public string TextForSave()
    {
      string s = "";
      s += "Gold\t" + gold().ToString() + Environment.NewLine;
      s += "Time\t" + msTime().ToString() + Environment.NewLine;
      s += "FieldName\t" + s_fieldName + Environment.NewLine;
      s += "FieldX\t" + s_fieldX + Environment.NewLine;
      s += "FieldY\t" + s_fieldY + Environment.NewLine;
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
