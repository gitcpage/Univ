using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System; // VirtualKey
using Windows.UI; // Colors
using Windows.UI.Xaml; // HorizontalAlignment, VerticalAlignment
using Windows.UI.Xaml.Controls; // StackPanel
using Windows.UI.Xaml.Media; // FontFamily

namespace Univ.Lib
{
  internal class Talk
  {
    static Grid s_GridOvelap_ = null;
    static StackPanel gTalk_ = null;
    static StackPanel gTalkYesNo_ = null;
    static TextBlock[] msgs_ = new TextBlock[3];
    static TextBlock arrowYes_;
    static TextBlock arrowNo_;
    enum YesNoState { None, Yes, No, YesDecide, NoDecide }
    static YesNoState yesNoState_;
    static Brush showBrush_;
    static Brush hideBrush_;

    TextBlock CreateTextBlock(Panel parent, string text = "", bool isYesNoItem = false)
    {
      TextBlock tb = new TextBlock();
      tb.Text = text;
      tb.FontSize = isYesNoItem ? 32 : 40;
      tb.FontFamily = new FontFamily("メイリオ");
      tb.Foreground = new SolidColorBrush(Colors.White);
      parent.Children.Add(tb);
      return tb;
    }
    public Talk()
    {
      if (gTalk_ == null)
      {// history/h20220602monchandemo/field4/css/field05.css
       //▲▲▲メッセージ本体▲▲▲
        gTalk_ = new StackPanel();
        gTalk_.Width = 776;
        gTalk_.Height = 202;
        gTalk_.HorizontalAlignment = HorizontalAlignment.Center;
        gTalk_.VerticalAlignment = VerticalAlignment.Bottom;
        gTalk_.Background = UnivLib.GetBrush(40, 40, 40);
        gTalk_.Padding = new Thickness(5, 5, 0, 0);

        gTalk_.BorderBrush = UnivLib.GetBrush(Colors.White);
        gTalk_.BorderThickness = new Thickness(8);
        gTalk_.CornerRadius = new CornerRadius(12);

        msgs_[0] = CreateTextBlock(gTalk_);
        msgs_[1] = CreateTextBlock(gTalk_);
        msgs_[2] = CreateTextBlock(gTalk_);
        //▼▼▼メッセージ本体▼▼▼

        //▲▲▲YesNo▲▲▲
        gTalkYesNo_ = new StackPanel();
        gTalkYesNo_.Width = 190;
        gTalkYesNo_.Height = 125;
        gTalkYesNo_.HorizontalAlignment = HorizontalAlignment.Right;
        gTalkYesNo_.VerticalAlignment = VerticalAlignment.Bottom;
        gTalkYesNo_.Background = UnivLib.GetBrush(40, 40, 40);
        gTalkYesNo_.Margin = new Thickness(0, 0, 50, 232);

        gTalkYesNo_.BorderBrush = UnivLib.GetBrush(Colors.White);
        gTalkYesNo_.BorderThickness = new Thickness(8);
        gTalkYesNo_.CornerRadius = new CornerRadius(12);

        Grid grid = new Grid();
        TextBlock tb;
        arrowYes_ = tb = CreateTextBlock(grid, "➤" , true);
        tb.Margin = new Thickness(5, 5, 0, 0);
        tb = CreateTextBlock(grid, "はい", true);
        tb.Margin = new Thickness(45, 5, 0, 0);
        tb.Tapped += (Object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) =>
        {
          if (yesNoState_ == YesNoState.Yes)
          {
            yesNoState_ = YesNoState.YesDecide;
          }
          else
            Select();
        };
        arrowNo_ = tb = CreateTextBlock(grid, "➤", true);
        tb.Margin = new Thickness(5, 55, 0, 0);
        tb = CreateTextBlock(grid, "いいえ", true);
        tb.Margin = new Thickness(45, 55, 0, 0);
        tb.Tapped += (Object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) =>
        {
          if (yesNoState_ == YesNoState.No)
          {
            yesNoState_ = YesNoState.NoDecide;
          }
          else
            Select(false);
        };
        gTalkYesNo_.Children.Add(grid);
        //▼▼▼YesNo▼▼▼

        showBrush_ = new SolidColorBrush(Colors.White);
        hideBrush_ = UnivLib.GetBrush(0, 0, 0, 0);
      }
    }
    void Select(bool yes = true)
    {
      if (yes)
      {
        yesNoState_ = YesNoState.Yes;
        arrowYes_.Foreground = showBrush_;
        arrowNo_.Foreground = hideBrush_;
      }
      else
      {
        yesNoState_ = YesNoState.No;
        arrowYes_.Foreground = hideBrush_;
        arrowNo_.Foreground = showBrush_;
      }
    }
    public void Change()
    {
      switch (yesNoState_)
      {
        case YesNoState.None:
          JsTrans.Assert("public void Change() Change.cs");
          break;
        case YesNoState.Yes:
          Select(false);
          break;
        case YesNoState.No:
          Select(true);
          break;
      }
    }
    public bool IsDecide(out bool isYes)
    {
      isYes = false;
      if (yesNoState_ == YesNoState.YesDecide)
      {
        isYes = true;
        return true;
      }
      else if(yesNoState_ == YesNoState.NoDecide)
      {
        isYes = false;
        return true;
      }
      return false;
    }
    public bool IsYes()
    {
      return yesNoState_ == YesNoState.Yes;
    }
    public void Add(Panel parent, string s0, string s1 = "", string s2 = "", bool showYesNo = false)
    {
      parent.Children.Add(gTalk_);
      yesNoState_ = YesNoState.None;
      msgs_[0].Text = s0;
      msgs_[1].Text = s1;
      msgs_[2].Text = s2;
      if (showYesNo)
      {
        Select();
        parent.Children.Add(gTalkYesNo_);
      }
    }
    public void Remove(Panel parent)
    {
      parent.Children.Remove(gTalk_);
      if (yesNoState_ != YesNoState.None)
        parent.Children.Remove(gTalkYesNo_);
    }

    static private void OverlapGrid(Grid parent, Brush brush)
    {
      if (s_GridOvelap_ == null)
      {
        s_GridOvelap_ = new Grid();
        s_GridOvelap_.Width = parent.Width;
        s_GridOvelap_.Height = parent.Height;
      }
      s_GridOvelap_.Background = brush;
    }
    // Change を簡略化して使えるようにした静的メソッド。
    // 戻り値：消去時にtrue
    // 以下使用例。
    //if (whatDoing_ == WhatDoing.None || whatDoing_ == WhatDoing.Change)
    //  bool yes;
    //  if (Lib.Change.TalkYesNo(ref talk_, monitor_, frameManager_, out yes, 
    //    "ラジオを拾った。", "アクアテラリウム リミックス", "再生しますか？"))
    //  {
    //    whatDoing_ = WhatDoing.None;
    //  }
    //  if (talk_ != null)
    //  {
    //    whatDoing_ = WhatDoing.Change;
    //  }
    //}
    static public bool TalkYesNoOverlap(ref Talk talk, Grid parent, FrameManager frameManager, out bool yes,
      string s0, string s1 = "", string s2 = "")
    {
      yes = false;
      if (talk == null)
      {
        OverlapGrid(parent, UnivLib.GetBrush(126, 0, 0, 0));
        parent.Children.Add(s_GridOvelap_);
        talk = new Lib.Talk();
        talk.Add(parent, s0, s1, s2, true);
      }
      else
      {
        if (talk.IsDecide(out yes))
        {
          talk.Remove(parent);
          talk = null;
          parent.Children.Remove(s_GridOvelap_);
          return true;
        }
        else
        {
          if (frameManager.IsKeyDownFirst(GameKey.Change))
            talk.Change();//選択肢切り替え
          else if (frameManager.IsKeyDownFirst(GameKey.Ok))
          {
            yes = talk.IsYes();
            talk.Remove(parent);
            talk = null;
            parent.Children.Remove(s_GridOvelap_);
            return true;
          }
        }
      }
      return false;
    }
    static public bool TalkMsg(ref Talk talk, Grid parent, FrameManager frameManager,
      string s0, string s1 = "", string s2 = "")
    {
      if (talk == null)
      {
        talk = new Lib.Talk();
        talk.Add(parent, s0, s1, s2);
      }
      else
      {
        if (frameManager.IsKeyDownFirst(GameKey.Ok, GameKey.Change) || 
          frameManager.isMouseLDownFirst)
        {
          talk.Remove(parent);
          talk = null;
          return true;
        }
      }
      return false;
    }
    static public bool TalkMsgOverlap(ref Talk talk, Grid parent, FrameManager frameManager,
      string s0, string s1 = "", string s2 = "")
    {
      if (talk == null)
      {
        OverlapGrid(parent, UnivLib.GetBrush(126, 0, 0, 0));
        parent.Children.Add(s_GridOvelap_);
        talk = new Lib.Talk();
        talk.Add(parent, s0, s1, s2);
      }
      else
      {
        if (frameManager.IsKeyDownFirst(GameKey.Ok, GameKey.Change) ||
          frameManager.isMouseLDownFirst)
        {
          talk.Remove(parent);
          talk = null;
          parent.Children.Remove(s_GridOvelap_);
          return true;
        }
      }
      return false;
    }
    static public void TalkWait(ref Talk talk, Grid parent, bool showEnd,
      string s0, string s1 = "", string s2 = "")
    {
      if (talk == null)
      {
          talk = new Lib.Talk();
          talk.Add(parent, s0, s1, s2);
      }
      if(showEnd)
      {
        talk.Remove(parent);
        talk = null;
      }
    }
    static public void TalkWaitOverlap(ref Talk talk, Grid parent, bool showEnd,
      string s0, string s1 = "", string s2 = "")
    {
      if (talk == null)
      {
        OverlapGrid(parent, UnivLib.GetBrush(126, 0, 0, 0));
        parent.Children.Add(s_GridOvelap_);
        talk = new Lib.Talk();
        talk.Add(parent, s0, s1, s2);
      }
      if (showEnd)
      {
        talk.Remove(parent);
        talk = null;
        parent.Children.Remove(s_GridOvelap_);
      }
    }
  }
}
