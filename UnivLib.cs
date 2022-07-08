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
    static List<string> s_paths_ = new List<string>();
    static List<BitmapImage> s_bmps_ = new List<BitmapImage>();
    static public BitmapImage BitmapImageFromAssets(string path)
    {
      for (int i = 0; i < s_paths_.Count; i++)
      {
        if (s_paths_[i] == path)
        {
          return s_bmps_[i];
        }
      }
      Uri uri = new Uri("ms-appx:///Assets/" + path);
      BitmapImage img =  new BitmapImage(uri);
      s_paths_.Add(path);
      s_bmps_.Add(img);
      return img;
    }
    static public void MsgBitmapPaths()
    {
      string s = "MsgBitmapPaths() \n\n";
      foreach (string path in s_paths_)
      {
        s += path + "\n";
      }
      JsTrans.alert(s);
    }
  }
}
