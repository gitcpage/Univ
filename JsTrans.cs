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

    // タグ（クラス）要素を丸ごと削除する.remove()
    // JavaScript例：$("#cls").remove();
    static public void remove(Panel panel, string tag, bool useStartsWith = false)
    {
      var uiec = panel.Children.ToArray();
      foreach (UIElement e in uiec)
      {
        Image img = e as Image;
        if (img != null)
        {
          bool b;
          if (useStartsWith)
            b = img.Tag.ToString().StartsWith(tag);
          else
            b = img.Tag.ToString() == tag;

          if (b)
          {
            panel.Children.Remove(e);
          }
        }
      }
    }

    //
    static public void console_log(string log)
    { s_mainPage.ConsoleText += log + "\n";
    }
    static public void console_clear(string log)
    {
      s_mainPage.ConsoleText = "";
    }

    // 独自の関数
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
  }
}
