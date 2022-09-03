using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.System;
using System.Xml.Linq;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace Univ.NsMenu
{
  internal class MenuCustom
  {
    Grid parent_;
    StackPanel wholePanel_;

    const int kCustomKeyNum = 7;
    StackPanel[] children_ = new StackPanel[kCustomKeyNum];
    TextBlock[] tbChildren_ = new TextBlock[kCustomKeyNum];
    VirtualKey[] settingKey_ = new VirtualKey[kCustomKeyNum];
    int activeChildIndex_ = 0;
    StackPanel topRightGroup_;
    TextBlock tbKeyTest_;
    bool isKeyTest_ = false;

    //△△△ビフォーコンストラクタ△△△
    void ResetBorder()
    {
      foreach (StackPanel sp in children_)
      {
        sp.BorderBrush = MenuUI.kChildPanelBgBrush;
      }
      topRightGroup_.BorderBrush = MenuUI.kChildPanelBgBrush;
    }
    void ActivateLeft()
    {
      ResetBorder();
      children_[activeChildIndex_].BorderBrush = MenuUI.kPanelBorderBrush;
      isKeyTest_ = false;
    }
    void ActivateRight()
    {
      ResetBorder();
      topRightGroup_.BorderBrush = MenuUI.kPanelBorderBrush;
      isKeyTest_ = true;
    }
    //▽▽▽ビフォーコンストラクタ▽▽▽

    public MenuCustom(Grid parent)
    {     
      parent_ = parent;
      wholePanel_ = MenuUI.CreateStackPanel(parent, 0, 0, MenuUI.kPanelBorderBrush, 529, 519, MenuUI.kPanelBgBrush);
      StackPanel top = MenuUI.CreateStackPanel(wholePanel_, 5, 15, MenuUI.kChildPanelBorderBrush,
        515, 290, MenuUI.kChildPanelBgBrush);
      top.Orientation = Orientation.Horizontal;

      StackPanel topLeft = MenuUI.CreateStackPanel(top, 0, 0, null,
        300, 290, null);
      MenuUI.RunLavelCenterAligned(topLeft, 10+220+5, 0, 40, "キー", useBold:true);
      Thickness childBorder = new Thickness(0, 0, 0, 2); // カレント識別するためのボーダー
      int index = 0;
      void CreateGroup(string text, GameKey key)
      {
        int leftMargin = 10;
        int subWidth = 0; // Menu123StackのWidthは515
        const int kStackWidth = 265;
        if (text.Length < 8)
        {
          leftMargin += kStackWidth / 2;
          subWidth = kStackWidth / 2;
        }
        StackPanel child = MenuUI.CreateStackPanel(topLeft, 0, 0, null,
                                        kStackWidth - subWidth, 25, MenuUI.kChildPanelBgBrush);
        child.Margin = new Thickness(leftMargin, 0, 10, 10);
        child.Orientation = Orientation.Horizontal;
        child.BorderThickness = childBorder;
        child.BorderBrush = index==0 ? MenuUI.kPanelBorderBrush : MenuUI.kChildPanelBgBrush;
        MenuUI.RunLavelRightAligned(child, 0, 0, 220 - subWidth, text);
        string customKey;
        switch (key)
        {
          case GameKey.Ok:
            customKey = Data.Basic.s_OkKey.ToString();
            settingKey_[index] = Data.Basic.s_OkKey;
            break;
          case GameKey.Cancel:
            customKey = Data.Basic.s_CancelKey.ToString();
            settingKey_[index] = Data.Basic.s_CancelKey;
            break;
          case GameKey.Change:
            customKey = Data.Basic.s_ChangeKey.ToString();
            settingKey_[index] = Data.Basic.s_ChangeKey;
            break;
          case GameKey.Left:
            customKey = Data.Basic.s_LeftKey.ToString();
            settingKey_[index] = Data.Basic.s_LeftKey;
            break;
          case GameKey.Right:
            customKey = Data.Basic.s_RightKey.ToString();
            settingKey_[index] = Data.Basic.s_RightKey;
            break;
          case GameKey.Up:
            customKey = Data.Basic.s_UpKey.ToString();
            settingKey_[index] = Data.Basic.s_UpKey;
            break;
          case GameKey.Down:
            customKey = Data.Basic.s_DownKey.ToString();
            settingKey_[index] = Data.Basic.s_DownKey;
            break;
          default:
            JsTrans.Assert("Menu7Custom.cs MenuCustom key:" + key.ToString());
            return;
        }
        TextBlock tb = MenuUI.RunLavelCenterAligned(child, 5, 0, 40, customKey);
        int indexForTapped = index;
        child.Tapped += (Object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) =>
        {
          activeChildIndex_ = indexForTapped;
          ActivateLeft();
        };
        children_[index] = child;
        tbChildren_[index] = tb;
        index++;
      }
      CreateGroup("決定(Enter,Space)", GameKey.Ok);
      CreateGroup("キャンセル、メニュー(Esc,Del)", GameKey.Cancel);
      CreateGroup("対象単全切替(Ctrl)", GameKey.Change);
      CreateGroup("左(←)", GameKey.Left);
      CreateGroup("右(→)", GameKey.Right);
      CreateGroup("上(↑)", GameKey.Up);
      CreateGroup("下(↓)", GameKey.Down);

      StackPanel topRight = MenuUI.CreateStackPanel(top, 0, 0, null,
        200, 290, null);//MenuUI.kMonitorBgBrush);
      topRight.Margin = new Thickness(0, 20, 0, 20);
      topRightGroup_ = MenuUI.CreateStackPanel(topRight, 0, 0, null,
        180, 120, null);
      topRightGroup_.BorderThickness = new Thickness(2, 2, 2, 2);
      topRightGroup_.Padding = new Thickness(15, 15, 0, 20);
      topRightGroup_.BorderBrush = MenuUI.kChildPanelBgBrush;
      topRightGroup_.Tapped += (Object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e) =>
      {
        this.ActivateRight();
      };
      StackPanel trChild = MenuUI.CreateStackPanel(topRightGroup_, 0, 0, null,
                                        100, 25, MenuUI.kChildPanelBgBrush);
      trChild.Orientation = Orientation.Horizontal;
      MenuUI.RunLavel(trChild, 0, 0, "キーテスト", useBold:true);
      MenuUI.RunLavel(topRightGroup_, 0, 0, "(EscかDelで戻る)");
      tbKeyTest_ = MenuUI.RunLavel(topRightGroup_, 0, 0, "");
      MenuUI.RunButton(15, 20, "デフォルトに戻す", topRight, new MenuNotify(NotifyResetKey), NotifyCode.Ok, 150);
    }
    public bool Notify(NotifyCode notifyCode)
    {
      switch (notifyCode)
      {
        case NotifyCode.Ok:
          break;
        case NotifyCode.Cancel:
          // ■戻る
          return true;
      }
      return false;
    }
    public void NotifyResetKey(NotifyCode notifyCode)
    {
      Data.Basic.ResetKey();
      settingKey_[0] = Data.Basic.s_OkKey;
      tbChildren_[0].Text = Data.Basic.s_OkKey.ToString();
      settingKey_[1] = Data.Basic.s_CancelKey;
      tbChildren_[1].Text = Data.Basic.s_CancelKey.ToString();
      settingKey_[2] = Data.Basic.s_ChangeKey;
      tbChildren_[2].Text = Data.Basic.s_ChangeKey.ToString();
      settingKey_[3] = Data.Basic.s_LeftKey;
      tbChildren_[3].Text = Data.Basic.s_LeftKey.ToString();
      settingKey_[4] = Data.Basic.s_RightKey;
      tbChildren_[4].Text = Data.Basic.s_RightKey.ToString();
      settingKey_[5] = Data.Basic.s_UpKey;
      tbChildren_[5].Text = Data.Basic.s_UpKey.ToString();
      settingKey_[6] = Data.Basic.s_DownKey;
      tbChildren_[6].Text = Data.Basic.s_DownKey.ToString();
    }
    public void Destroy()
    {
      parent_.Children.Remove(wholePanel_);
    }
    public bool FrameOne(FrameManager frameManager)
    {
      if (isKeyTest_)
      {
        if (frameManager.IsCancelKeyDownFirstIgnoreAlpha())
        {
          this.ActivateLeft();
          this.tbKeyTest_.Text = "";
          return false;
        }
        string s = "[";
        bool isFirst = true;
        if (frameManager.IsKeyDown(GameKey.Ok))
        {
          s += "決";
          isFirst = false;
        }
        if (frameManager.IsKeyDown(GameKey.Change))
        {
          s += (isFirst ? "" : ",") + "対";
          isFirst = false;
        }
        if (frameManager.IsKeyDown(GameKey.Left))
        {
          s += (isFirst ? "" : ",") + "左";
          isFirst = false;
        }
        if (frameManager.IsKeyDown(GameKey.Right))
        {
          s += (isFirst ? "" : ",") + "右";
          isFirst = false;
        }
        if (frameManager.IsKeyDown(GameKey.Up))
        {
          s += (isFirst ? "" : ",") + "上";
          isFirst = false;
        }
        if (frameManager.IsKeyDown(GameKey.Down))
        {
          s += (isFirst ? "" : ",") + "下";
        }
        tbKeyTest_.Text = s + "]";
        return false;
      } // if (isKeyTest_)

      if (frameManager.IsKeyDownFirst(GameKey.Left) ||
        frameManager.IsKeyDownFirst(GameKey.Right))
      {
        if (isKeyTest_)
        {
          this.ActivateLeft();
        }
        else
        {
          this.ActivateRight();
        }
        return false;
      }
      else if (frameManager.IsKeyDownRepeat(GameKey.Up))
      {
        activeChildIndex_--;
        if (activeChildIndex_ < 0) activeChildIndex_ = kCustomKeyNum - 1;
        ActivateLeft();
      }
      else if (frameManager.IsKeyDownRepeat(GameKey.Down))
      {
        activeChildIndex_++;
        if (activeChildIndex_ >= kCustomKeyNum) activeChildIndex_ = 0;
        ActivateLeft();
      }
      else if (frameManager.IsCancelKeyDownFirstIgnoreAlpha())
      {
        return true; // ■閉じる
      }

      //▲▲▲キーカスタム▲▲▲
      for (char c = 'A'; c <= 'Z'; c++)
      {
        if (frameManager.IsKeyDownAlpha(c))
        {
          tbChildren_[activeChildIndex_].Text = c.ToString();
          VirtualKey oldKey = settingKey_[activeChildIndex_];
          if (oldKey == (VirtualKey)c) return false;
          VirtualKey cKey = (VirtualKey)c;
          if (cKey == Data.Basic.s_OkKey)
          {
            if (activeChildIndex_ != 0)
            {
              Data.Basic.s_OkKey = oldKey;
              tbChildren_[0].Text = oldKey.ToString();
            }
          }
          else if (cKey == Data.Basic.s_CancelKey)
          {
            if (activeChildIndex_ != 1)
            {
              Data.Basic.s_CancelKey = oldKey;
              tbChildren_[1].Text = oldKey.ToString();
            }
          }
          else if (cKey == Data.Basic.s_ChangeKey)
          {
            if (activeChildIndex_ != 2)
            {
              Data.Basic.s_ChangeKey = oldKey;
              tbChildren_[2].Text = oldKey.ToString();
            }
          }
          else if (cKey == Data.Basic.s_LeftKey)
          {
            if (activeChildIndex_ != 3)
            {
              Data.Basic.s_LeftKey = oldKey;
              tbChildren_[3].Text = oldKey.ToString();
            }
          }
          else if (cKey == Data.Basic.s_RightKey)
          {
            if (activeChildIndex_ != 4)
            {
              Data.Basic.s_RightKey = oldKey;
              tbChildren_[4].Text = oldKey.ToString();
            }
          }
          else if (cKey == Data.Basic.s_UpKey)
          {
            if (activeChildIndex_ != 5)
            {
              Data.Basic.s_UpKey = oldKey;
              tbChildren_[5].Text = oldKey.ToString();
            }
          }
          else if (cKey == Data.Basic.s_DownKey)
          {
            if (activeChildIndex_ != 6)
            {
              Data.Basic.s_DownKey = oldKey;
              tbChildren_[6].Text = oldKey.ToString();
            }
          }
          switch (activeChildIndex_)
          {
            case 0:
              Data.Basic.s_OkKey = cKey;
              break;
            case 1:
              Data.Basic.s_CancelKey = cKey;
              break;
            case 2:
              Data.Basic.s_ChangeKey = cKey;
              break;
            case 3:
              Data.Basic.s_LeftKey = cKey;
              break;
            case 4:
              Data.Basic.s_RightKey = cKey;
              break;
            case 5:
              Data.Basic.s_UpKey = cKey;
              break;
            case 6:
              Data.Basic.s_DownKey = cKey;
              break;
            default:
              JsTrans.Assert("Menu7Custom.cs MenuCustom cKey:" + cKey.ToString());
              return false;
          }
          tbChildren_[activeChildIndex_].Text = cKey.ToString();
          Data.Basic.s_OkKey = cKey;
          return false;
        }
      }
      //▼▼▼キーカスタム▼▼▼
      return false;
    }
  }
}
