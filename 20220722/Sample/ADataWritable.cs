using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
namespace Univ.Data
{
  internal class ADataWritable : AData
  {
    // 書き込みアクセサ。

    public void name(string name) { name_ = name; }

    // 多次元配列の場合の書き込みアクセサ。
    public void data(int[,] data) { data_ = data; }

    private ADataWritable() { }
    static readonly object lockObject_ = new object();
    public static new ADataWritable Instance()
    {
      ADataWritable inst;
      lock (lockObject_)
      {
        JsTrans.Assert(instance == null,
          typeof(ADataWritable).Name + " がインスタンスが既にインスタンス化されています。");
        inst = new ADataWritable();
        instance = inst;
      }
      return inst;
    }
  }
}
*/