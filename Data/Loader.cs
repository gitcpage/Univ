using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage; // StorageFolder, StorageFile

namespace Univ.Data
{
  // シングルトンパターン
  internal class Loader
  {
    public readonly ConstStatus[] weapons = { };
    public readonly ConstStatus[] body = { };
    public readonly ConstStatus[] head = { };
    public readonly ConstStatus[] arm = { };
    public readonly ConstStatus[] exterior = { };
    public readonly ConstStatus[] accessory = { };

    // ロード関連
    static public string DataOfSetup = "";
    static public LoadingState LoadingState_ { get; private set; } = LoadingState.None;
    static public void UpdateLoadingState(LoadingState next)
    {
      LoadingState_ = DataEnum.UpdateLoadingState(LoadingState_, next);
    }
    static public void Load()
    {
      UpdateLoadingState(LoadingState.Loading);
      async void LoadBody()
      {
        StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Const.txt"));
        DataOfSetup = await FileIO.ReadTextAsync(file);
        UpdateLoadingState(LoadingState.Loaded);
        // Setup() まで記述すれば、LoadingState.Success まで進む。
        // ただし、その場合 Setup() 引数でのカスタマイズや、ロード直後の処理を記述しにくくなる。
      }
      LoadBody();
    }

    private Loader()
    {
      string[] dem = new string[1];
      dem[0] = Environment.NewLine;
      string[] rows = DataOfSetup.Split(dem, StringSplitOptions.RemoveEmptyEntries);

      for (int i = 0; i < rows.Length; i++)
      {
        string row = rows[i];
        if (row.StartsWith("Weapon"))
        {
          for (i++, row = rows[i];
            i < rows.Length && row.Length >= 15 && row.StartsWith("\t");
            i++, row = rows[i])
          {
            Array.Resize(ref weapons, weapons.Length + 1);
            weapons[weapons.Length - 1] = new ConstStatus(row.Remove(0, 1));
          }
        }
        else if(row.StartsWith("Body"))
        {
          for (i++, row = rows[i];
            i < rows.Length && row.Length >= 15 && row.StartsWith("\t");
            i++, row = rows[i])
          {
            Array.Resize(ref body, weapons.Length + 1);
            body[body.Length - 1] = new ConstStatus(row.Remove(0, 1));
          }
        }
        else if (row.StartsWith("Head"))
        {
          for (i++, row = rows[i];
            i < rows.Length && row.Length >= 15 && row.StartsWith("\t");
            i++, row = rows[i])
          {
            Array.Resize(ref body, body.Length + 1);
            body[body.Length - 1] = new ConstStatus(row.Remove(0, 1));
          }
        }
        else if (row.StartsWith("Arm"))
        {
          for (i++, row = rows[i];
            i < rows.Length && row.Length >= 15 && row.StartsWith("\t");
            i++, row = rows[i])
          {
            Array.Resize(ref arm, arm.Length + 1);
            arm[arm.Length - 1] = new ConstStatus(row.Remove(0, 1));
          }
        }
        else if (row.StartsWith("Exterior"))
        {
          for (i++, row = rows[i];
            i < rows.Length && row.Length >= 15 && row.StartsWith("\t");
            i++, row = rows[i])
          {
            Array.Resize(ref exterior, exterior.Length + 1);
            exterior[exterior.Length - 1] = new ConstStatus(row.Remove(0, 1));
          }
        }
        else if (row.StartsWith("Accessory"))
        {
          for (i++, row = rows[i];
            i < rows.Length && row.Length >= 15 && row.StartsWith("\t");
            i++)
          {
            row = rows[i];
            Array.Resize(ref accessory, accessory.Length + 1);
            accessory[accessory.Length - 1] = new ConstStatus(row.Remove(0, 1));
          }
        }
      }
      UpdateLoadingState(LoadingState.Success);
    }
    static private Loader instance = null;
    static public Loader Instance
    {
      get
      {
        if (instance == null)
        {
          JsTrans.window_close("セットアップ前にLoader.Instanceが呼び出されました。");
        }
        return instance;
      }
    }
    static public Loader Setup()
    {
      JsTrans.Assert(LoadingState_ == LoadingState.Loaded,
        "LoadingState is not LoadingState.Loaded. \nLoadingState_:" + LoadingState_.ToString());
      if (instance == null)
      {
        return instance = new Loader();
      }
      JsTrans.window_close("セットアップ済みなのに Loader.Setup() が呼び出されました。");
      return instance;
    }
  }
}
