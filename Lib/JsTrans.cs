using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement; // ApplicationView
using Windows.UI.Popups; // MessageDialog
using Windows.UI.Xaml.Controls; // Panel
using Windows.UI.Xaml;

namespace Univ
{
  internal class JsTrans // JavaScript Transplant
  {
    public static MainPage s_mainPage;
    static System.Random s_random = new System.Random();
    static public async void alert(string text)
    {
      MessageDialog md = new MessageDialog(text);
      await md.ShowAsync();
    }
    static public async void window_close(string msg = null)
    {
      if (msg != null)
      {
        MessageDialog md = new MessageDialog(msg);
        await md.ShowAsync();
      }
      await ApplicationView.GetForCurrentView().TryConsolidateAsync();
    }
    static public string document_title
    {
      get { 
        return ApplicationView.GetForCurrentView().Title;
      }
      set {
        ApplicationView.GetForCurrentView().Title = value;
      }
    }
    // 戻り値：0以上max未満の整数
    static public int getRandomInt(int max)
    { // https://developer.mozilla.org/ja/docs/Web/JavaScript/Reference/Global_Objects/Math/random
      return s_random.Next(max);
    }

    //
    static public void console_log(string log)
    {
      s_mainPage.ConsoleText += log + "\n";
    }
    static public void console_log(Grid g)
    {
      Thickness t = g.Margin;
      s_mainPage.ConsoleText += "Margin("+t.Left.ToString()+", "+t.Top.ToString()+", "+t.Right.ToString()
        + ", " + t.Bottom.ToString() +")" + "\n";
      t = g.Padding;
      s_mainPage.ConsoleText += "Padding(" + t.Left.ToString() + ", " + t.Top.ToString() + ", " + t.Right.ToString()
        + ", " + t.Bottom.ToString() + ")" + "\n";
      s_mainPage.ConsoleText += "Width: " + g.Width + " Height:" + g.Height+"\n\n";
    }
    static public void console_clear()
    {
      s_mainPage.ConsoleText = "";
    }

    // 独自の関数
    static public void Assert(string msg)
    {
      Assert(false, msg);
    }
    static public async void Assert(bool cond, string msg)
    {
      if (!cond)
      {
        if (msg != null)
        {
          MessageDialog md = new MessageDialog(msg);
          await md.ShowAsync();
        }
        await ApplicationView.GetForCurrentView().TryConsolidateAsync();
      }
    }
    static public async void Assert(int v ,int min, int max, string msg)
    {
      if (!(min <= v && v <= max))
      {
        if (msg != null)
        {
          MessageDialog md = new MessageDialog(msg);
          await md.ShowAsync();
        }
        await ApplicationView.GetForCurrentView().TryConsolidateAsync();
      }
    }
  }
}
