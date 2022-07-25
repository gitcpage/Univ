using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls; // Panel, Image
using Windows.UI.Xaml; // UIElement

namespace Univ
{
  internal class Draw
  {
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
  }
}
