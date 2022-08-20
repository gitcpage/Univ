using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls; // Grid

namespace Univ.NsMenu
{
  internal class MenuSaveLoad
  {
    Lib.Talk talk_ = null;
    FrameManager frameManager_;
    Grid monitor_;
    EventHandler<object> frameOne_;

    public bool IsShowing { get; private set; }
    int step_;

    public MenuSaveLoad(FrameManager frameManager, Grid monitor, EventHandler<object> frameOne)
    {
      frameManager_ = frameManager;
      monitor_ = monitor;
      frameOne_ = frameOne;
      IsShowing = false;
    }

    //△△△セーブ△△△
    private void FrameOneSaveTalk(object senderDispatcherTimer, object eNull)
    {
      bool yes;
      switch (step_)
      {
        case 0:
          if (Lib.Talk.TalkYesNoOverlap(ref talk_, monitor_, frameManager_, out yes,
            "セーブしますか？", "", ""))
          {
            if (yes)
            {
              Lib.Talk.TalkWait(ref talk_, monitor_, false, "セーブ中です．．．");
              Data.Loader loader = Data.Loader.Instance;
              step_ = 1;
              // ■セーブする
              loader.Save();
            }
            else
            {
              IsShowing = false;
              frameManager_.ChangeSequence(frameOne_);
            }
          }
          break;

        case 1:
          if (Data.Loader.isSaveComplete)
          {
            Lib.Talk.TalkWaitOverlap(ref talk_, monitor_, true, "セーブ中です．．．");
            step_ = 2;
          }
          break;

        case 2:
          if (Lib.Talk.TalkMsgOverlap(ref talk_, monitor_, frameManager_,
            "セーブが完了しました。"))
          {
            IsShowing = false;
            frameManager_.ChangeSequence(frameOne_);
          }
          break;
      }
    }
    public void Save()
    {
      IsShowing = true;
      step_ = 0;
      frameManager_.ChangeSequence(FrameOneSaveTalk);
    }
    //▽▽▽セーブ▽▽▽

    //△△△ロード△△△
    private void FrameOneLoadTalk(object senderDispatcherTimer, object eNull)
    {
      bool yes;
      if (Lib.Talk.TalkYesNoOverlap(ref talk_, monitor_, frameManager_, out yes,
        "ロードしますか？", "(実装上ソフトリセット)", ""))
      {
        if (yes)
        {
          // ■リセットする（ソフトリセットと等価）
          frameManager_.Reset();
          return;
        }
        else
        {
          IsShowing = false;
          frameManager_.ChangeSequence(frameOne_);
        }
      }
    }
    public void Load()
    {
      IsShowing = true;
      frameManager_.ChangeSequence(FrameOneLoadTalk);
    }
    //▽▽▽ロード▽▽▽
  }
}
