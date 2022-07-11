using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Univ
{
  // 必要な参照はコンストラクタで渡す。
  // mainPage.BottomTextBySequence("Field");
  // CS0051コンパイルエラーを防ぐため、アクセス修飾子を internal から public に変更。
  public interface IRun
  {
    void Run();
  }
}
