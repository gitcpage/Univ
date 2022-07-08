using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging; // BitmapImage

namespace Univ
{
  internal class BitmapImageCollection
  {
    static List<string> paths;
    static List<BitmapImage> bmps;

    static public BitmapImage Get(string uriPath)
    {
      Uri uri = new Uri(uriPath);
      return new BitmapImage(uri);
    }
  }
}
