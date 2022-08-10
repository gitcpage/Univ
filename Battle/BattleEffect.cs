using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml;

namespace Univ.NsBattle
{
  internal class BattleEffect
  {
    //▲▲▲Effect▲▲▲
    int effectCount_;
    Grid parent_;
    Canvas canvas_;
    Line line_;

    int effectSpan_;
    int effectSpanHalf_;
    int sx_, sy_, ex_, ey_, dx_, dy_;
    //▼▼▼Effect▼▼▼

    //▲▲▲Damage▲▲▲
    int damageCount_;
    TextBlock tb_;
    Border bdr_;
    int damageSpan_;
    //int damageSpanHalf_;
    double ty_;
    //▼▼▼Damage▼▼▼

    //▲▲▲Dead▲▲▲
    bool isDead_;
    Image image_;
    int deadCount_;
    int deadSpan_;
    //▼▼▼Dead▼▼▼

    enum Step { Effect, Damage, Dead, End }
    Step step_;

    public BattleEffect()
    {

    }
    public void Ready(Grid parent, Thickness thickness, Image image, int damage, bool isDead)
    {
      //▲▲▲Effect▲▲▲
      double ox = thickness.Left + image.Width / 2;
      double oy = thickness.Top + image.ActualHeight / 2;
      double hw = image.Width / 3;// 2;
      sx_ = (int)(ox - hw);
      sy_ = (int)(oy - hw);
      ex_ = (int)(ox + hw);
      ey_ = (int)(oy + hw);
      dx_ = ex_ - sx_;
      dy_ = ey_ - sy_;

      line_ = new Line();
      //line_.Width = 700;
      //line_.Height = 700;
      line_.X1 = 0;
      line_.Y1 = 0;
      line_.X2 = 0;// 200;
      line_.Y2 = 0;// 200;
      line_.Stroke = UnivLib.GetBrush(Colors.White);
      line_.StrokeThickness = 5;
      line_.HorizontalAlignment = HorizontalAlignment.Left;
      line_.VerticalAlignment = VerticalAlignment.Top;

      canvas_ = new Canvas();
      //canvas_.Width = 100;
      //canvas_.Height = 500;
      canvas_.HorizontalAlignment = HorizontalAlignment.Left;
      canvas_.VerticalAlignment = VerticalAlignment.Top;
      canvas_.Children.Add(line_);

      parent_ = parent;
      parent_.Children.Add(canvas_);

      effectCount_ = 0;
      effectSpan_ = 10;
      effectSpanHalf_ = effectSpan_ / 2;
      //▼▼▼Effect▼▼▼

      //▲▲▲Damage▲▲▲
      damageCount_ = 0;
      ty_ = thickness.Top + image.ActualHeight;

      bdr_ = new Border();
      bdr_.Margin = new Thickness(sx_, 0, 0, 0);
      bdr_.Width = 100;// ex_ - sx_;
      bdr_.Height = ty_;
      bdr_.HorizontalAlignment = HorizontalAlignment.Left;
      bdr_.VerticalAlignment = VerticalAlignment.Top;

      tb_ = new TextBlock();
      tb_ = BattleUI.RunTextBlockInstance(damage.ToString(), TextAlignment.Center);
      tb_.FontSize = 30;
      tb_.Height = 55;
      tb_.FontFamily = new Windows.UI.Xaml.Media.FontFamily("HGP創英角ﾎﾟｯﾌﾟ体");
      tb_.VerticalAlignment = VerticalAlignment.Bottom;
      tb_.HorizontalAlignment = HorizontalAlignment.Center;
      tb_.TextAlignment = TextAlignment.Center;
      tb_.HorizontalTextAlignment = TextAlignment.Center;
      tb_.Foreground = UnivLib.GetBrush(Colors.White);
      bdr_.Child = tb_;
      damageSpan_ = 15;
      //▼▼▼Damage▼▼▼

      //▲▲▲Dead▲▲▲
      isDead_ = isDead;
      image_ = image;
      deadCount_ = 0;
      deadSpan_ = 10;
      //▼▼▼Dead▼▼▼

      step_ = Step.Effect;
    }
    void EffectFrameOne()
    {
      if (effectCount_ == effectSpan_)
      {
        parent_.Children.Remove(canvas_);
        effectCount_++;
        step_ = Step.Damage;
      }
      if (effectCount_ <= effectSpanHalf_)
      {
        line_.X1 = sx_;
        line_.Y1 = sy_;
        line_.X2 = sx_ + effectCount_ * dx_ / effectSpanHalf_;
        line_.Y2 = sy_ + effectCount_ * dy_ / effectSpanHalf_;
      }
      else
      {
        line_.X1 = sx_ + (effectCount_ - effectSpanHalf_) * dx_ / effectSpanHalf_;
        line_.Y1 = sy_ + (effectCount_ - effectSpanHalf_) * dy_ / effectSpanHalf_;
        line_.X2 = ex_;
        line_.Y2 = ey_;
      }
      effectCount_++;
    }
    void DamageFrameOne()
    {
      const int kStepDelta = 5;
      int kDivide3 = damageSpan_ / 3;

      if (damageCount_ == 0)
      {
        parent_.Children.Add(bdr_);
      }
      else if (damageCount_ == damageSpan_)
      {
        parent_.Children.Remove(bdr_);
        step_ = isDead_ ? Step.Dead : Step.End;
      }
      else if (damageCount_ < kDivide3)
      {
        bdr_.Height = ty_ - damageCount_ * kStepDelta;
      }
      else if (damageCount_ < damageSpan_ * 2 / 3)
      {
        bdr_.Height = ty_ - kDivide3 * kStepDelta + (damageCount_ - kDivide3) * kStepDelta;
      }
      else
      {
        bdr_.Height = ty_;
      }
      damageCount_++;
    }
    void DeadFrameOne()
    {
      deadCount_++;
      image_.Opacity = (deadSpan_ - deadCount_) / (double)deadSpan_;
      if (deadCount_ >= deadSpan_) step_ = Step.End;
    }
    public bool FrameOne()
    {
      switch (step_)
      {
        case Step.Effect:
          EffectFrameOne();
          break;
        case Step.Damage:
          DamageFrameOne();
          break;
        case Step.Dead:
          DeadFrameOne();
          break;
        case Step.End:
          return true;
        default:
          JsTrans.Assert("BattkeEffect.cs FrameOne()");
          break;
      }
      return false;
      /*if (effectCount_ == effectSpan_)
      {
        parent_.Children.Remove(canvas_);
        effectCount_++;
        //return true;
      }
      if (effectCount_ == effectSpan_ + 1)
      {
        return DamageFrameOne();
      }

      if (effectCount_ <= effectSpanHalf_)
      {
        line_.X1 = sx_;
        line_.Y1 = sy_;
        line_.X2 = sx_ + effectCount_ * dx_ / effectSpanHalf_;
        line_.Y2 = sy_ + effectCount_ * dy_ / effectSpanHalf_;
      }
      else
      {
        line_.X1 = sx_ + (effectCount_ - effectSpanHalf_) * dx_ / effectSpanHalf_;
        line_.Y1 = sy_ + (effectCount_ - effectSpanHalf_) * dy_ / effectSpanHalf_;
        line_.X2 = ex_;
        line_.Y2 = ey_;
      }
      effectCount_++;
      return false;*/
    }
  }
}
