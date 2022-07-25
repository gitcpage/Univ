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
using Windows.UI; // Colors
using Univ.NsField;


// 変数名規則
// チップ単位で扱われる変数には、先頭に t をつける。
// ピクセル単位のチップはつけない。
// 背景で座標を逆移動する変数には、先頭に r をつける。
// t と r が被った場合は r を先につける
// 背景の英単語は Background と長いので Bg と略する。

namespace Univ
{
  internal class Field : FieldInclude, IRun
  {
    MainPage mainPage_;
    FrameManager frameManager_;
    Data.StatusWritable[] charsWritable_;

    //FieldBlock obj_;
    FieldBlock player_;
    FieldBgAbstract bg_;

    enum WhatDoing { None, PlayerMove, PlayerMoveByBg, Talk };
    WhatDoing whatDoing_ = WhatDoing.None;
    int moveStep_ = 0;
    int moveDirectionX_ = 0;
    int moveDirectionY_ = 0;

    //int fieldFrameCount_ = 0;
    Lib.Talk talk_ = null;

    int tPosX_;
    int tPosY_;

    public Field(MainPage mainPage, Data.StatusWritable[] charsWritable) : base(mainPage.GetMonitorBg())
    {
      mainPage_ = mainPage;
      charsWritable_ = charsWritable;
      frameManager_ = mainPage.GetFrameTimer();
      monitor_ = mainPage.GetMonitor();

      //bg_ = new FieldBg(mainPage.GetMonitorBg());
      bg_ = new FieldBgEx(mainPage.GetMonitorBg());
      //obj_ = new FieldBlock("char/t5040walkt.png", monitor_);
      player_ = new FieldBlock("char/char1p", FieldBlock.DirectionSlot.DownUp, monitor_);
    }
    
    void ResetPosition(int tx, int ty)
    {
      void PositionCalculate(int tPos, int tDispLength, int tBgLength, out int tDispPos, out int tBgPos)
      { // Excel でバグ調査しながら出した計算。
        tDispPos = (tDispLength - 1) / 2;
        tBgPos = tPos - tDispPos;
        if (tBgPos < 0)
        {
          tDispPos += tBgPos;
          tBgPos = 0;
        }
        else if (tBgPos > -tDispLength + tBgLength)
        {
          int tmp = tBgPos;
          tBgPos = -tDispLength + tBgLength;
          tDispPos += tmp - tBgPos;
        }
      }
      int mapX, dispX, mapY, dispY;
      PositionCalculate(tx, kTipXNum, bg_.tWholeXNum_, out dispX, out mapX);
      PositionCalculate(ty, kTipYNum, bg_.tWholeYNum_, out dispY, out mapY);
      player_.SetBlockPosition(dispX, dispY);
      bg_.MoveTip(-mapX, -mapY);
    }
    public void FrameReturn(object senderDispatcherTimer, object eNull)
    {
      mainPage_.BottomTextBySequence("Field");
      // 初期化
      bg_.Run();
      //AppendXyIndex(0, 1, obj_, "imgObj", "obj", 0);
      player_.Image = AppendXyIndex(0, 0, player_.Bitmap, "imgBox", 0);

      //■位置情報を決定
      ResetPosition(tPosX_, tPosY_);

      //フェードイン後、フレームループ開始
      frameManager_.EnterSequenceFadeIn(FrameOne);
    }
    public void Run()
    {
      mainPage_.BottomTextBySequence("Field");
      // 初期化
      bg_.Run();
      //AppendXyIndex(0, 1, obj_, "imgObj", "obj", 0);
      player_.Image = AppendXyIndex(0, 0, player_.Bitmap, "imgBox", 0);
      //player_.SetBlockPosition(kTipXNum / 2 - 1, kTipYNum / 2 - 1);

      //■位置情報を決定
      ResetPosition(kTipXNum / 2 - 1, kTipYNum / 2 - 1);

      //フェードイン後、フレームループ開始
      frameManager_.EnterSequenceFadeIn(FrameOne);
    }
    public void OnFadeOuted(object senderDispatcherTimer, object eNull)
    {
      mainPage_.Clear();
      frameManager_.ExitSequence();
    }
    public void OnFadeOutedToMenu(object senderDispatcherTimer, object eNull)
    {
      mainPage_.Clear();
      int rtPosX, rtPosY;
      bg_.GetTipPosition(out rtPosX, out rtPosY);
      int px = player_.blockX_, py = player_.blockY_;
      player_.GetBlockPosition(out px, out py);
      this.tPosX_ = -rtPosX + px;
      this.tPosY_ = -rtPosY + py;
      frameManager_.EnterSequence(FrameReturn, new Menu(mainPage_, charsWritable_));
    }
    void JudgeMoving()
    {
      if (whatDoing_ == WhatDoing.None)
      {
        //▲▲移動操作を調べる▲▲
        moveDirectionX_ = moveDirectionY_ = 0;
        bool existMoveOperation = frameManager_.KeyDirection(out moveDirectionX_, out moveDirectionY_);
        if (!existMoveOperation && frameManager_.isMouseLDown)
        {
          int mouseX = frameManager_.clientX;
          int mouseY = frameManager_.clientY;
          int max = player_.GetCenterX() - mouseX;
          moveDirectionX_ = -1; // 仮左方向移動
          if (mouseX - player_.GetCenterX() > max)
          { //右方向移動
            max = mouseX - player_.GetCenterX();
            moveDirectionX_ = 1;
          }
          if (player_.GetCenterY() - mouseY > max)
          { //上方向移動
            max = player_.GetCenterY() - mouseY;
            moveDirectionY_ = -1;
            moveDirectionX_ = 0;
          }
          if (mouseY - player_.GetCenterY() > max)
          { //下方向移動
            max = mouseY - player_.GetCenterY();
            moveDirectionY_ = 1;
            moveDirectionX_ = 0;
          }
          if (max > kTipXSize / 2) existMoveOperation = true;
        }
        //▼▼移動操作を調べる▼▼


        if (existMoveOperation)
        {
          //▲▲どう移動するか判定する▲▲
          if (moveDirectionX_ == -1)
          {
            if (player_.blockX_ > 0) //画面上のキャラ位置による判定
            {
              if (player_.blockX_ > kTipXNum / 2 - 1)
                whatDoing_ = WhatDoing.PlayerMove;
              else if (bg_.CanMove(-moveDirectionX_, 0))
                whatDoing_ = WhatDoing.PlayerMoveByBg;
              else
                whatDoing_ = WhatDoing.PlayerMove;
            }
          }
          if (moveDirectionX_ == 1)
          {
            if (player_.blockX_ < kTipXNum - 1)
            {
              if (player_.blockX_ < kTipXNum / 2 - 1)
                whatDoing_ = WhatDoing.PlayerMove;
              else if (bg_.CanMove(-moveDirectionX_, 0))
                whatDoing_ = WhatDoing.PlayerMoveByBg;
              else
                whatDoing_ = WhatDoing.PlayerMove;
            }
          }
          if (moveDirectionY_ == -1)
          {
            if (player_.blockY_ > 0)
            {
              if (player_.blockY_ > kTipYNum / 2 - 1)
                whatDoing_ = WhatDoing.PlayerMove;
              else if (bg_.CanMove(0, -moveDirectionY_))
                whatDoing_ = WhatDoing.PlayerMoveByBg;
              else
                whatDoing_ = WhatDoing.PlayerMove;
            }
          }
          if (moveDirectionY_ == 1)
          {
            if (player_.blockY_ < kTipYNum - 1)
            {
              if (player_.blockY_ < kTipYNum / 2 - 1)
                whatDoing_ = WhatDoing.PlayerMove;
              else if (bg_.CanMove(0, -moveDirectionY_))
                whatDoing_ = WhatDoing.PlayerMoveByBg;
              else
                whatDoing_ = WhatDoing.PlayerMove;
            }
          }
        }//if (existMoveOperation)
      }//if (player_moving_ == PlayerMoving.None)
      //▼▼どう移動するか判定する▼▼
    }
    public void FrameOne(object senderDispatcherTimer, object eNull)
    {
      if (frameManager_.ResettingOnce())
      {
        JsTrans.console_log("ソフトリセット");
        frameManager_.EnterSequenceFadeOut(OnFadeOuted);
        return;
      }

      if (frameManager_.FrameCount % 50 == 0)
      {
        bg_.ChangeBg(0);
      }
      else if(frameManager_.FrameCount % 25 == 0)
      {
        bg_.ChangeBg(1);
      }

      //右に延々動くやつ
      //obj_.SetXIndex(fieldFrameCount_++ % kTipXNum);

      //▲▲▲プレイヤー移動処理▲▲▲
      JudgeMoving();

      if (whatDoing_ == WhatDoing.PlayerMoveByBg)
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
          whatDoing_ = WhatDoing.None;
        }
      }//if (player_moving_)
      else if (whatDoing_ == WhatDoing.PlayerMove)
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
          player_.blockX_ += moveDirectionX_;
          player_.blockY_ += moveDirectionY_;
          player_.BlockSync();
          moveStep_ = 0;
          whatDoing_ = WhatDoing.None;
        }
      }
      //▼▼▼プレイヤー移動処理▼▼▼

      if (whatDoing_ == WhatDoing.None || whatDoing_ == WhatDoing.Talk)
      {
        //bool yes;
        if (Lib.Talk.TalkMsg(ref talk_, monitor_, frameManager_,// out yes, 
          "ラジオを拾った。", "アクアテラリウム リミックス", "再生しますか？"))
        {
          whatDoing_ = WhatDoing.None;
        }
        if (talk_ != null)
        {
          whatDoing_ = WhatDoing.Talk;
        }
      }

      if (frameManager_.IsKeyDownFirst(VirtualKey.Escape))
      {
        //monitor_.Children.Clear();
        //mainPage_.GetMonitorBg().Children.Clear();
        //frameManager_.FadeOutEnable(false);
        frameManager_.EnterSequenceFadeOut(OnFadeOutedToMenu);
        //frameManager_.ChangeSequence(OnFadeOutedToMenu);
      }
    }
  }
}
