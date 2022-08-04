using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls; // Grid
using Windows.UI.Xaml.Media; // Stretch
using Windows.UI.Xaml; // Thickness
using Windows.UI; // Colors
using Windows.UI.Xaml.Input;

namespace Univ.NsBattle
{
  delegate void BattleTargetNotify(int target);
  internal class BattleMonsters
  {
    Grid gParent_;

    int num_;
    Grid[] grids_;
    TextBlock[] tbs_;
    Image[] images_;
    int[] adjusts_;
    int target_;

    BattleTargetNotify targetNotify_;

    public BattleMonsters(Grid parent, BattleTargetNotify targetNotify)
    {
      gParent_ = parent;
      targetNotify_ = targetNotify;

      grids_ = new Grid[5];
      tbs_ = new TextBlock[5];
      images_ = new Image[5];
      adjusts_ = new int[5];

      //int target_;
      //bool targetDicide_;
    }
    void Target(int target, bool canDecide)
    {
      if (canDecide && target == this.target_)
      {
        tbs_[target].Text = "";
        //■ターゲット決定
        targetNotify_(target);
        return;
      }
      for (int i = 0; i < num_; i++)
      {
        if (i == target)
        {
          tbs_[i].Text = "◀";
          this.target_ = target;
        }
        else
          tbs_[i].Text = "";
      }
    }
    public void Create(BattleAtb atb)
    {
      for (int i = 0; i < 5; i++) adjusts_[i] = 0;

      void CreateOne(int idx, int width, int x, int y, string path, string name)
      {
        grids_[idx] = new Grid();
        grids_[idx].Margin = new Thickness(x, y, 0, 0);
        images_[idx] = UnivLib.ImageInstance(0, 0, path, name);
        images_[idx].Stretch = Stretch.Uniform;
        images_[idx].Width = width;
        grids_[idx].Children.Add(images_[idx]);

        tbs_[idx] = new TextBlock();
        tbs_[idx].Text = "";//"◀";
        tbs_[idx].FontSize = 26;
        tbs_[idx].Margin = new Thickness(width, 0, 0, 0);
        tbs_[idx].Foreground = UnivLib.GetBrush(Colors.White);
        grids_[idx].Children.Add(tbs_[idx]);

        grids_[idx].Tapped += (Object sender, TappedRoutedEventArgs e) =>
        {
          if (atb.state == BattleAtb.State.TargetSelect)
          {
            Target(idx, true);
          }
        };

        gParent_.Children.Add(grids_[idx]);
      }
      CreateOne(0, 200, 160, 110, "battle/mon1.png", "mon1");
      CreateOne(1, 200, 160, 280, "battle/mon2.png", "mon2");

      num_ = 2;
    }
    public void ShowTarget(bool doShow = true)
    {
      if (doShow)
      {
        target_ = 0;
        Target(target_, false);
      }
      else
      {
        Target(-1, false);
      }
    }
    public void Adjust()
    {
      for (int i = 0; i < num_; i++)
      {
        if (adjusts_[i] == 0)
        {
          double h = images_[i].ActualHeight;
          if (h > 0)
          {
            h -=tbs_[i].ActualHeight;
            h /= 2;
            tbs_[i].Margin = new Thickness(200, h, 0, 0);
            adjusts_[i] = (int)h;
            //JsTrans.console_log("BattleMonsters.cs Adjust()" + h.ToString());
          }
        }
      }
    }
    public Thickness GetThickness(int idx)
    {
      return grids_[idx].Margin;
    }
    public Image GetImage(int idx)
    {
      return images_[idx];
    }
  }
}
