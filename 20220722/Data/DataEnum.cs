using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ.Data
{
  public enum EquipCategory { Weapon, Body, Head, Arm, Exterior, Accessory }

  public enum LoadingState { 
    None,
    Loading, // ストレージアクセス中
    Loaded,  // ストレージアクセス完了
    Success  // ローディング成功
  }
  internal class DataEnum
  {
    static public LoadingState UpdateLoadingState(LoadingState current, LoadingState next)
    {
      if (current == LoadingState.None && next == LoadingState.Loading)
        return LoadingState.Loading;
      else if (current == LoadingState.Loading && next == LoadingState.Loaded)
        return LoadingState.Loaded;
      else if (current == LoadingState.Loaded && next == LoadingState.Success)
        return LoadingState.Success;

      JsTrans.Assert("current:" + current.ToString() + ", next:" + next.ToString());
      return current;
    }
  }

}
