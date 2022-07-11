using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging; // BitmapImage
using Windows.UI.Xaml.Controls; // Image
using Windows.UI.Xaml; // Thickness
using Windows.UI.Xaml.Media; // Stretch
using Windows.UI.Xaml.Media.Animation; // ObjectAnimationUsingKeyFrames
using Windows.System; //VirtualKey

namespace Univ
{
  internal partial class Field : FieldInclude, IRun
  {
    FrameTimer frameTimer_;

    FieldBlock obj_;
    FieldBlock player_;
    FieldBgEx bg_;

    enum PlayerMoving { None, Player, Bg };
    PlayerMoving player_moving_ = PlayerMoving.None;
    int moveStep_ = 0;
    int moveDirectionX_ = 0;
    int moveDirectionY_ = 0;

    public Field(FrameTimer frameTimer, Grid monitor, Grid monitorBg) : base(monitorBg)
    {
      s_frameTimer_ = frameTimer;
      monitor_ = monitor;

      bg_ = new FieldBgEx(monitorBg);
      obj_ = new FieldBlock("char/t5040walkt.png", monitor);
      player_ = new FieldBlock("char/char1p", FieldBlock.DirectionSlot.DownUp, monitor);
    }
    public void Run(MainPage mainPage, FrameTimer frameTimer)
    {
      mainPage.BottomTextBySequence("Field");
      frameTimer_ = frameTimer;
      // 初期化
      bg_.Run();
      AppendXyIndex(0, 1, obj_, "imgObj", "obj", 2);
      player_.Image = AppendXyIndex(0, 0, player_.Bitmap, "imgBox", "box", 2);
      player_.BlockSync(kTipXNum / 2 - 1, kTipYNum / 2 - 1);

      //フレームループ開始
      s_frameTimer_.setTimeOut(FrameOne);
    }
    public void FrameOne(
      object sender, // DispatcherTimer型
      object e) // null
    {
      if (FrameTimer.ResettingOnce())
      {
        JsTrans.console_log("FrameTimer.ResettingOnce()");
        monitor_.Children.Clear();
        frameTimer_.PopStackHandler();
        return;
      }

      if (s_frameTimer_.FrameCount % 50 == 0)
      {
        bg_.ChangeBg(0);
      }
      else if(s_frameTimer_.FrameCount % 25 == 0)
      {
        bg_.ChangeBg(1);
      }

      //右に延々動くやつ
      obj_.SetXIndex(s_frameTimer_.FrameCount % kTipXNum);

      //▲▲▲プレイヤー移動処理▲▲▲
      if (player_moving_ == PlayerMoving.None)
      {
        //▲▲移動操作を調べる▲▲
        moveDirectionX_ = moveDirectionY_ = 0;
        bool existMoveOperation = s_frameTimer_.KeyDirection(out moveDirectionX_, out moveDirectionY_);
        if (!existMoveOperation && JsTrans.isMouseLDown)
        {
          int max;
          int mouseX = JsTrans.clientX;
          int mouseY = JsTrans.clientY;
          max = player_.GetX() - mouseX;
          moveDirectionX_ = -1;
          if (mouseX - player_.GetX() > max)
          {
            max = mouseX - player_.GetX();
            moveDirectionX_ = 1;
          }
          if (player_.GetY() - mouseY > max)
          {
            max = player_.GetY() - mouseY;
            moveDirectionY_ = -1;
            moveDirectionX_ = 0;
          }
          if (mouseY - player_.GetY() > max)
          {
            max = mouseY - player_.GetY();
            moveDirectionY_ = 1;
            moveDirectionX_ = 0;
          }
          if (max > kTipYSize) existMoveOperation = true;
        }
        //▼▼移動操作を調べる▼▼
        if (existMoveOperation)
        {
          if (moveDirectionX_ == -1)
          {
            if (player_.blockX > 0)
            {
              if (player_.blockX > kTipXNum / 2 - 1)
                player_moving_ = PlayerMoving.Player;
              else if (bg_.CanMove(moveDirectionX_, 0))
                player_moving_ = PlayerMoving.Bg;
              else
                player_moving_ = PlayerMoving.Player;
            }
          }
          if (moveDirectionX_ == 1)
          {
            if (player_.blockX < kTipXNum - 1)
            {
              if (player_.blockX < kTipXNum / 2 - 1)
                player_moving_ = PlayerMoving.Player;
              else if (bg_.CanMove(moveDirectionX_, 0))
                player_moving_ = PlayerMoving.Bg;
              else
                player_moving_ = PlayerMoving.Player;
            }
          }
          if (moveDirectionY_ == -1)
          {
            if (player_.blockY > 0)
            {
              if (player_.blockY > kTipYNum / 2 - 1)
                player_moving_ = PlayerMoving.Player;
              else if (bg_.CanMove(0, -moveDirectionY_))
                player_moving_ = PlayerMoving.Bg;
              else
                player_moving_ = PlayerMoving.Player;
            }
          }
          if (moveDirectionY_ == 1)
          {
            if (player_.blockY < kTipYNum - 1)
            {
              if (player_.blockY < kTipYNum / 2 - 1)
                player_moving_ = PlayerMoving.Player;
              else if (bg_.CanMove(0, -moveDirectionY_))
                player_moving_ = PlayerMoving.Bg;
              else
                player_moving_ = PlayerMoving.Player;
            }
          }
        }//if (existMoveOperation)
      }//if (player_moving_ == PlayerMoving.None)
      if (player_moving_ == PlayerMoving.Bg)
      {
        if (moveStep_ < kMoveTime-1)
        {
          moveStep_++;
          bg_.Move(-moveDirectionX_ * kMoveXStep, -moveDirectionY_ * kMoveYStep);
        }
        else
        {
          bg_.Move(-moveDirectionX_ * kMoveXStep, -moveDirectionY_ * kMoveYStep);
          moveStep_ = 0;
          player_moving_ = PlayerMoving.None;
        }
      }//if (player_moving_)
      else if (player_moving_ == PlayerMoving.Player)
      {
        if (moveStep_ < kMoveTime - 1)
        {
          moveStep_++;
          player_.SetXY(
            player_.GetX() + moveDirectionX_ * kMoveXStep,
            player_.GetY() + moveDirectionY_ * kMoveYStep);
        }
        else
        {
          player_.blockX += moveDirectionX_;
          player_.blockY += moveDirectionY_;
          player_.BlockSync();
          moveStep_ = 0;
          player_moving_ = PlayerMoving.None;
        }
      }
      //▼▼▼プレイヤー移動処理▼▼▼
    }
  }
}
