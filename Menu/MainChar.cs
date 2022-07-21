using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls; // Grid
using Windows.UI;
using Windows.UI.Xaml; // Thickness
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Univ.NsMenu
{
  internal class MainChar
  {
    MenuNotify notify_;
    Grid left_;
    TextBlock leftArrow_;
    public MainChar(Grid parent, int charId, MenuNotify menuNotify, NotifyCode notifyCode)
    {
      notify_ = menuNotify;
      Data.Status st = Data.Status.Instances[charId];

      //▲▲▲Left▲▲▲
      left_ = new Grid();

      left_.HorizontalAlignment = HorizontalAlignment.Left;
      left_.Height = 50;
      left_.VerticalAlignment = VerticalAlignment.Center;
      left_.Margin = new Thickness(0, 10, 10, 10);

      left_.Children.Add(leftArrow_ = MenuInclude.GetTextBlock("")); // ➤

      string str = charId.ToString();
      Image img = UnivLib.ImageInstance(0, 0, "char/char" + str + "p00.png", "char" + str, "char");
      img.Margin = new Thickness(30, 0, 0, 0);
      left_.Children.Add(img);

      parent.Children.Add(left_);
      //▼▼▼Left▼▼▼

      //▲▲▲Center▲▲▲
      Grid center = new Grid();
      center.HorizontalAlignment = HorizontalAlignment.Left;
      center.Height = 210;
      center.VerticalAlignment = VerticalAlignment.Top;
      center.Margin = new Thickness(90, 5, 0, 10);

      MenuUI.RunLavel(center, 0, 0, st.name(), true, fontSize: 22);
      MenuUI.RunLavel(center, 10, 50, "Lv " + st.level(), true, fontSize:20);

      parent.Children.Add(center);
      //▼▼▼Center▼▼▼
      
      //▲▲▲Right▲▲▲
      Grid right = new Grid();
      right.HorizontalAlignment = HorizontalAlignment.Left;
      right.Height = 100;
      right.VerticalAlignment = VerticalAlignment.Top;
      right.Margin = new Thickness(220, 5, 0, 10);

      string fmt = string.Format("HP {0, 4:D1}/{1, 4:D1}", st.hp(), st.hp());
      MenuUI.RunLavel(right, 0, 0, fmt, true);

      fmt = string.Format("MP {0, 4:D1}/{1, 4:D1}", st.mp(), st.mp());
      MenuUI.RunLavel(right, 0, 30, fmt, true);

      fmt = string.Format("NEXT {0, 6:D1}", st.NeedEexperienceUntilNextLevel());
      MenuUI.RunLavel(right, 0, 60, fmt, true);

      parent.Children.Add(right);
      //▼▼▼Right▼▼▼

      parent.PointerEntered += (Object sender, PointerRoutedEventArgs e) =>
      {
        Grid o = sender as Grid;
        o.Background = UnivLib.GetBrush(187, 211, 213);
      };
      parent.PointerExited += (Object sender, PointerRoutedEventArgs e) =>
      {
        Grid o = sender as Grid;
        o.Background = UnivLib.GetBrush(157, 181, 183);
      };
      parent.Tapped += (Object sender, TappedRoutedEventArgs e) =>
      {
        notify_(notifyCode);
        Tap();
      };
    }

    public void Tap()
    {
      leftArrow_.Foreground = UnivLib.GetBrush(14, 77, 108);
      leftArrow_.Text = "➤";
    }
    public void TapOtherChar()
    {
      leftArrow_.Text = "";
    }
    public void TapLeftMenu()
    {
      if (leftArrow_.Text == "➤")
        leftArrow_.Foreground = UnivLib.GetBrush(128, 14, 77, 108);
    }
  }
}
