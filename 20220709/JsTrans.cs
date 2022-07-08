using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement; // ApplicationView

namespace Univ
{
  internal class JsTrans // JavaScript Transplant
  {
    static public async void alert(string text)
    {
      Windows.UI.Popups.MessageDialog md = new Windows.UI.Popups.MessageDialog(text);
      await md.ShowAsync();
    }
    static public async void window_close()
    {
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

  }
}
