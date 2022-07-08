using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging; // BitmapImage

namespace Univ
{
  internal class UnivLib
  {
    static public BitmapImage BitmapImageFromAssets(string path)
    {
      Uri uri = new Uri("ms-appx:///Assets/" + path);
      return new BitmapImage(uri);
    }
  }
}
