using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Input; //PointerPoint
using Windows.Storage; // StorageFolder, ApplicationData, StorageFile, CreationCollisionOption, FileIO
// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x411 を参照してください

namespace Univ
{
  /// <summary>
  /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
  /// </summary>
  public sealed partial class MainPage : Page
  {
    FrameManager frameManager_;
    public Point mousePoint;
    public bool isMouseLDown = false;

    //Opening opening_ = null;
    //Field field_;
    //RunImplements runImplements_;
    //RunImplementsFade runImplementsFade_;

    string BottomTextFormer = ""; // シーケンス遷移用に使用する
    string BottomTextLatter = ""; // マウス用に使用する

    //データ関連
    Data.Loader loader;

    public MainPage()
    {
      // ファイルデータの非同期読み込み
      Data.Loader.Load();

      this.InitializeComponent();

      //https://social.msdn.microsoft.com/Forums/en-US/97556dc2-b01c-43a4-8c6a-9a3fd51d9151/updating-imagesource-causes-flickering?forum=wpdevelop
      this.NavigationCacheMode = NavigationCacheMode.Required;
      JsTrans.s_mainPage = this;

      // 1.0でidMonitorは見えなくなる。フェードインスタートの場合は1.0にする。
      this.idMonitorFade.Background.Opacity = 1.0;
      // Visibility.Collapsedのときは this.idMonitor要素にマウスイベントが発生する。
      this.idMonitorFade.Visibility = Visibility.Visible;

      // フレーム初期化処理。
      // FrameManager で FrameOne を呼び出すのは整合性として気持ち悪いので、
      // MainPageコンストラクタで呼び出している。
      frameManager_ = new FrameManager(this, FrameOne);
      //FrameOne(null, null);

      Univ.Fade.Constructor(this.idMonitorFade, this.frameManager_);
    }
    //  △△△シーケンス遷移で渡すオブジェクトへのアクセス△△△
    public FrameManager GetFrameManager()
    {
      return this.frameManager_;
    }
    public Grid GetMonitor()
    {
      return this.idMonitor;
    }
    public Grid GetMonitorBg()
    {
      return this.idMonitorBg;
    }
    // ▽▽▽シーケンス遷移で渡すオブジェクトへのアクセス▽▽▽
    private void Button_Click_Clear(object sender, RoutedEventArgs e)
    {
      ConsoleText = "";
    }
    //JsTransクラスでconsole_log()などからアクセスられるプロパティ。
    public string ConsoleText
    {
      get { return this.txtConsole.Text; }
      set { this.txtConsole.Text = value; }
    }
    // ■エントリーポイント
    void FrameOne(object sender, object e)
    {
      //this.idProgress.Value = frameManager_.FrameCount;
      if (Data.Loader.loadingState == Data.LoadingState.Loading)
      {
        // データロード中
        frameManager_.EnterSequence(FrameOne, new Loading(this));
        return;
      }
      else if (Data.Loader.loadingState == Data.LoadingState.Loaded)
      {
        // データロード完了
        loader = Data.Loader.Setup();
        //loader.chars[0].Equip(Data.EquipCategory.Weapon, 0);
      }

      //frameManager_.EnterSequence(FrameOne, new BattleDebug(this, loader.chars, 0));
      frameManager_.EnterSequence(FrameOne, new Field(this, loader.chars));
      /*if (opening_ == null || opening_.Selected == -1)
      {
        frameManager_.EnterSequence(FrameOne, opening_ = new Opening(this));
      }
      else
      {
        if (opening_.Selected == 0 || opening_.Selected == 1)
        {
          if (opening_.Selected == 0)
          { // はじめから
            loader.NewGame();
          }
          else
          { // つづきから
            loader.Reload();
          }
          opening_ = null;
          Field field = new Field(this);
          frameManager_.EnterSequence(FrameOne, field);
        }
        else
        {
          JsTrans.Assert("「はじめから」でも「つづきから」でもありません。");
        }
      }*/
      //frameManager_.EnterSequence(FrameOne, new Menu(this, loader.chars));
    }
    // △△△モニタアクセス共通処理△△△
    public void Clear(bool doDataReload = false)
    {
      this.idMonitor.Children.Clear();
      this.idMonitorBg.Children.Clear();
      if (doDataReload)
      {
        //■データをリセットする
        loader.Reload();
      }
      this.idMonitorFade.Background.Opacity = 1.0;
      this.idMonitorFade.Visibility = Visibility.Visible;
    }
    // ▽▽▽モニタアクセス共通処理▽▽▽

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
    }
    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
      StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
      JsTrans.console_log("[Data Loader.cs]" + storageFolder.Path);
    }

    private void Button_Click_4(object sender, RoutedEventArgs e)
    {
      UnivLib.MsgBitmapPaths();
    }

    private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
    {
      throw new NotImplementedException();
    }

    private void AppBarReset_Click(object sender, RoutedEventArgs e)
    {
      frameManager_.Reset();
    }
    private void AppBarPause_Click(object sender, RoutedEventArgs e)
    {
      frameManager_.Pause();
      this.AppBarPause.IsEnabled = false;
      this.AppBarPlay.IsEnabled = true;
      this.AppBarFastPlay.IsEnabled = true;
    }

    private void AppBarPlay_Click(object sender, RoutedEventArgs e)
    {
      this.AppBarPause.IsEnabled = true;
      this.AppBarPlay.IsEnabled = false;
      this.AppBarFastPlay.IsEnabled = true;
      frameManager_.Start();
    }
    private void AppBarFastPlay_Click(object sender, RoutedEventArgs e)
    {
      this.AppBarPause.IsEnabled = true;
      this.AppBarPlay.IsEnabled = true;
      this.AppBarFastPlay.IsEnabled = false;
      frameManager_.Start(true);
    }
    public void BottomTextBySequence(string name)
    {
      BottomTextFormer = name;
      this.AppBar.Text = BottomTextFormer + " | " + BottomTextLatter;
    }

    void BottomTextByMouse()
    {
      BottomTextLatter = (int)mousePoint.X + ":" + (int)mousePoint.Y + (isMouseLDown ? " Down" : "");
      this.AppBar.Text = BottomTextFormer + " | " + BottomTextLatter;
    }
    private void idMonitor_PointerMoved(object sender, PointerRoutedEventArgs e)
    {
      PointerPoint pp = e.GetCurrentPoint((UIElement)sender);
      mousePoint = new Point(pp.Position.X, pp.Position.Y);
      BottomTextByMouse();
    }

    private void idMonitor_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
      isMouseLDown = true;
      BottomTextByMouse();
    }

    private void idMonitor_PointerReleased(object sender, PointerRoutedEventArgs e)
    {
      isMouseLDown = false;
      BottomTextByMouse();
    }
  }
}
