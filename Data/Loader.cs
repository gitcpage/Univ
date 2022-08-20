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
  // シングルトンパターンを使用しています。
  // 処理の流れは以下の通りです 。
  // １．Loader.Load() を行います。
  // ２．Data.Loader.LoadingState_ == Data.LoadingState.Loadingであればロード中です。
  //     Data.Loader.LoadingState_ == Data.LoadingState.Loaded になるまで待機します。
  // ３．Data.Loader.Setup();を呼び出します。
  // ４．Data.Loader loader_ = Data.Loader.Instance; ローダーを取得します。
  // ５．loader_.EquipArray(); や loader_.weapons などでデータを取得します。
  internal class Loader
  {
    public readonly ConstStatus[] charsUnique = { };

    public readonly ConstStatus[] weapons = { };
    public readonly ConstStatus[] body = { };
    public readonly ConstStatus[] head = { };
    public readonly ConstStatus[] arm = { };
    public readonly ConstStatus[] exterior = { };
    public readonly ConstStatus[] accessory = { };

    readonly StatusWritable[] friends_;
    Data.SecurityToken stFriends_ = null;
    public StatusWritable[] Friends(SecurityToken stFriends)
    {
      JsTrans.Assert(stFriends_ != null, "Loader Frinds stFriends_!=null");
      JsTrans.Assert(stFriends.Authorize(stFriends_), "Loader Frinds stFriends.Authorize(stFriends_)");
      return friends_;
    }

    public readonly FieldData[] fieldData;
    public readonly ConstStatusMons[] monsData;
    public readonly ConstMonsGroups[] monsGroupsData;
    public readonly ConstItem[] itemData;

    public ConstStatus[] EquipArray(EquipCategory equipCategory)
    {
      switch (equipCategory)
      {
        case EquipCategory.Weapon: return weapons;
        case EquipCategory.Body: return body;
        case EquipCategory.Head: return head;
        case EquipCategory.Arm: return arm;
        case EquipCategory.Exterior: return exterior;
        case EquipCategory.Accessory: return accessory;
        default:
          JsTrans.Assert("Loader.cs EquipArray");
          return null;
      }
    }

    // △△△ロード関連△△△
    static string kCStatusDataOfSetup = "";
    static string kCFieldDataOfSetup = "";
    static string kCMonsDataOfSetup = "";
    static string kCMonsGroupsDataOfSetup = "";
    static string kCItemDataOfSetup = "";

    // 以下３つの静的文字列はセーブ後のロードに使われるので、セーブ時に上書きする。
    static string SaveBagDataOfSetup = "";
    static string SaveBasicDataOfSetup = "";
    static string SaveCharsDataOfSetup = "";

    static public LoadingState loadingState { get; private set; } = LoadingState.None;
    static public bool isSaveComplete = false;
    static public void UpdateLoadingState(LoadingState next)
    {
      loadingState = DataEnum.UpdateLoadingState(loadingState, next);
    }
    static public void Load()
    {
      UpdateLoadingState(LoadingState.Loading);
      async void LoadBody()
      {
        //▲▲▲固定データ▲▲▲
        // kCStatusDataOfSetup
        StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///CStatus.txt"));
        kCStatusDataOfSetup = await FileIO.ReadTextAsync(file);

        // kCFieldDataOfSetup
        file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///CField.txt"));
        kCFieldDataOfSetup = await FileIO.ReadTextAsync(file);

        //kCMonsDataOfSetup
        file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///CMons.txt"));
        kCMonsDataOfSetup = await FileIO.ReadTextAsync(file);

        //kCMonsDataOfSetup
        file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///CMons.txt"));
        kCMonsDataOfSetup = await FileIO.ReadTextAsync(file);

        //kCMonsGroupDataOfSetup
        file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///CMonsGroup.txt"));
        kCMonsGroupsDataOfSetup = await FileIO.ReadTextAsync(file);

        //kCItemDataOfSetup
        file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///CItem.txt"));
        kCItemDataOfSetup = await FileIO.ReadTextAsync(file);
        //▼▼▼固定データ▼▼▼

        //▲▲▲流動データ▲▲▲
        // SaveBagDataOfSetup
        StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        file = await storageFolder.GetFileAsync("Bag.txt");
        SaveBagDataOfSetup = await FileIO.ReadTextAsync(file);

        //JsTrans.console_log("[Data Loader.cs]" + storageFolder.Path);

        file = await storageFolder.GetFileAsync("Basic.txt");
        SaveBasicDataOfSetup = await FileIO.ReadTextAsync(file);

        file = await storageFolder.GetFileAsync("Chars.txt");
        SaveCharsDataOfSetup = await FileIO.ReadTextAsync(file);
        //▼▼▼流動データ▼▼▼

        UpdateLoadingState(LoadingState.Loaded);

        // Setup() まで記述すれば、LoadingState.Success まで進む。
        // ただし、その場合 Setup() 引数でのカスタマイズや、ロード直後の処理を記述しにくくなる。
      }
      LoadBody();
    }

    /*public void NewGame()
    {
      // ▲▲▲持ち物の初期化▲▲▲
      Bag bag = Bag.Instance;
      for (int i = 0; i < (int)Data.EquipCategory.Number; i++)
      {
        Data.ConstStatus[] eqs = EquipArray((Data.EquipCategory)i);
        for (int j = 0; j < eqs.Length; j++)
        {
          bag.equipSubstitution((Data.EquipCategory)i, j, 0);
        }
      }
      // ▼▼▼持ち物の初期化▼▼▼

      // ▲▲▲キャラクターの経験値・レベル・装備・初期HPとMPを初期化▲▲▲
      foreach (var c in friends_)
      {
        c.experience(0);
        c.Level(1);
        for (int i = 0; i < (int)Data.EquipCategory.Number; i++)
          c.Equip((Data.EquipCategory)i, -1);
        c.Heal();
      }
      // ▼▼▼キャラクターの装備を初期化▼▼▼
      Basic basic = Basic.Instance;
      basic.Initialize();
    }*/
    private void ReloadInside()
    {
      // ▲▲SaveBagDataOfSetup▲▲
      string[] groupNames = { "CharsUnique", "Weapon", "Body", "Head", "Arm", "Exterior", "Accessory", "Item" };
      string[] rows = SaveBagDataOfSetup.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      Bag bag = Bag.Instance;
      for (int i = 0; i < rows.Length; i++)
      {
        string row = rows[i];
        for (int j = 1; j < groupNames.Length; j++)
        {
          int id = 0;
          if (row == groupNames[j])
          {
            for (i++; i < rows.Length; i++)
            {
              row = rows[i];
              int have;
              if (!int.TryParse(row, out have))
              {
                i--;
                goto Next;
              }
              if (groupNames[j] == "Item")
                bag.item(id, have);
              else
                bag.equipSubstitution((Data.EquipCategory)(j - 1), id, have);
              id++;
            }
            goto Next;
          }
        }
      Next:;
      }
      // ▼▼SaveBagDataOfSetup▼▼

      // ▲▲SaveBasicDataOfSetup▲▲
      Basic basic = Basic.Instance;
      basic.Load(SaveBasicDataOfSetup);
      // ▼▼SaveBasicDataOfSetup▼▼

      // ▲▲SaveCharsDataOfSetup▲▲
      rows = SaveCharsDataOfSetup.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      for (int i = 0; i < friends_.Length; i++)
      {
        friends_[i].Load(rows[i]);
      }
      // ▼▼SaveCharsDataOfSetup▼▼
    }
    public void Reload(bool isNewGame)
    {
      if (instance == null)
      {
        JsTrans.window_close("セットアップ前にLoader.Reloadが呼び出されました。");
        return;
      }
      if (isNewGame)
      {
        // ▲▲▲持ち物の初期化▲▲▲
        Bag bag = Bag.Instance;
        for (int i = 0; i < (int)Data.EquipCategory.Number; i++)
        {
          Data.ConstStatus[] eqs = EquipArray((Data.EquipCategory)i);
          for (int j = 0; j < eqs.Length; j++)
          {
            bag.equipSubstitution((Data.EquipCategory)i, j, 0);
          }
        }
        // ▼▼▼持ち物の初期化▼▼▼

        // ▲▲▲キャラクターの経験値・レベル・装備・初期HPとMPを初期化▲▲▲
        foreach (var c in friends_)
        {
          c.experience(0);
          c.Level(1);
          for (int i = 0; i < (int)Data.EquipCategory.Number; i++)
            c.Equip((Data.EquipCategory)i, -1);
          c.Heal();
        }
        // ▼▼▼キャラクターの装備を初期化▼▼▼
        Basic basic = Basic.Instance;
        basic.Initialize();
      }
      else
        ReloadInside();
    }
    private Loader(SecurityToken stFriends)
    {
      stFriends_ = stFriends;

      // ▲▲kConstData▲▲
      string[] rows = kCStatusDataOfSetup.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      string[] groupNames = { "CharsUnique", "Weapon", "Body", "Head", "Arm", "Exterior", "Accessory" };

      for (int i = 0; i < rows.Length; i++)
      {
        string row = rows[i];
        for (int j = 0; j < groupNames.Length; j++)
        {
          if (row.StartsWith(groupNames[j]))
          {
            ConstStatus[] css = { };
            for (i++; i < rows.Length; i++)
            {
              row = rows[i];
              if (row.Length < 15 || !row.StartsWith("\t")) break;

              Array.Resize(ref css, css.Length + 1);
              css[css.Length - 1] = new ConstStatus(row.Remove(0, 1), css.Length - 1);
            }
            switch (j)
            {
              case 0: charsUnique = css; break;
              case 1: weapons = css; break;
              case 2: body = css; break;
              case 3: head = css; break;
              case 4: arm = css; break;
              case 5: exterior = css; break;
              case 6: accessory = css; break;
              default:
                JsTrans.window_close("[Loader.cs Loader 1] j("+j+") is Illegal value.");
                break;
            }
            goto Next;
          }
        }
      Next:;
      }

      friends_ = StatusWritable.Instances(charsUnique);
      for (int i = 0; i < charsUnique.Length; i++)
        friends_[i].Level(1);
      // ▼▼kConstData▼▼

      // ▲▲kFieldDataOfSetup▲▲
      string[] chunks = kCFieldDataOfSetup.Split("*", StringSplitOptions.RemoveEmptyEntries);
      fieldData = new FieldData[chunks.Length];
      for (int i = 0; i < chunks.Length; i++)
      {
        fieldData[i] = new FieldData(chunks[i]);
      }
      // ▼▼kFieldDataOfSetup▼▼

      // ▲▲kMonsDataOfSetup▲▲
      chunks = kCMonsDataOfSetup.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      monsData = new ConstStatusMons[chunks.Length];
      for (int i = 0; i < chunks.Length; i++)
      {
        monsData[i] = new ConstStatusMons(chunks[i], i);
      }
      // ▼▼kMonsDataOfSetup▼▼

      // ▲▲kCMonsGroupsDataOfSetup▲▲
      chunks = kCMonsGroupsDataOfSetup.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      monsGroupsData = new ConstMonsGroups[chunks.Length];
      for (int i = 0; i < chunks.Length; i++)
      {
        monsGroupsData[i] = new ConstMonsGroups(chunks[i]);
      }
      // ▼▼kCMonsGroupsDataOfSetup▼▼

      // ▲▲kCItemDataOfSetup▲▲
      chunks = kCItemDataOfSetup.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      itemData = new ConstItem[chunks.Length];
      for (int i = 0; i < chunks.Length; i++)
      {
        itemData[i] = new ConstItem(chunks[i], i);
      }
      // ▼▼kCItemDataOfSetup▼▼

      Bag.Setup(weapons.Length, body.Length, head.Length, arm.Length, exterior.Length, accessory.Length, itemData.Length);

      Basic.Setup();
      ReloadInside();

      UpdateLoadingState(LoadingState.Success);
    }
    // ▽▽▽ロード関連▽▽▽

    // △△△セーブ関連△△△
    private async void SaveInside()
    {
      Bag bag = Bag.Instance;
      StringBuilder s = new StringBuilder();
      string[] groupNames = { "Weapon", "Body", "Head", "Arm", "Exterior", "Accessory" };
      for (int i = 0; i < groupNames.Length; i++)
      {
        s.AppendLine(groupNames[i]);
        int[] equips = bag.GetArrayByCategory((EquipCategory)i);
        for (int j = 0; j < equips.Length; j++)
        {
          s.AppendLine(equips[j].ToString());
        }
        s.AppendLine();
      }
      s.AppendLine("Item");
      for (int i = 0; i < itemData.Length; i++)
      {
        s.AppendLine(bag.item(i).ToString());
      }
      s.AppendLine();
      StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
      StorageFile file = await storageFolder.GetFileAsync("Bag.txt");
      SaveBagDataOfSetup = s.ToString(); //■ロードに使われるので忘れずに
      await FileIO.WriteTextAsync(file, SaveBagDataOfSetup);

      file = await storageFolder.GetFileAsync("Basic.txt");
      SaveBasicDataOfSetup = Basic.Instance.TextForSave(); //■ロードに使われるので忘れずに
      await FileIO.WriteTextAsync(file, SaveBasicDataOfSetup);

      s.Clear();
      foreach (StatusWritable sw in friends_)
      {
        s.AppendLine(sw.TextOfSave());
      }
      file = await storageFolder.GetFileAsync("Chars.txt");
      SaveCharsDataOfSetup = s.ToString(); //■ロードに使われるので忘れずに
      await FileIO.WriteTextAsync(file, SaveCharsDataOfSetup);

      isSaveComplete = true;
    }
    public void Save()
    {
      isSaveComplete = false;
      SaveInside();
    }
    // ▽▽▽セーブ関連▽▽▽

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
    static public Loader Setup(SecurityToken stFriends)
    {
      JsTrans.Assert(loadingState == LoadingState.Loaded,
        "LoadingState is not LoadingState.Loaded. \nLoadingState_:" + loadingState.ToString());
      if (instance == null)
      {
        return instance = new Loader(stFriends);
      }
      JsTrans.window_close("セットアップ済みなのに Loader.Setup() が呼び出されました。");
      return instance;
    }
  }
}
