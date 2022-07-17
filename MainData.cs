using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Univ.Data;

namespace Univ
{
  internal class MainData
  {
    //public const int kCharsNum = 6;
    public StatusWritable[] chars;
    //public ConstStatus[] kCharsUnique;

    public MainData()
    {
      const int kCharsNum = 6;
      ConstStatus[] kCharsUnique = new ConstStatus[kCharsNum];
      kCharsUnique[0] = new ConstStatus("ホーン", 800, 700, 350, 210, 330, 170, 200, 0, 0, 0, 0, 0, 0);
      kCharsUnique[1] = new ConstStatus("サンナン", 900, 400, 400, 220, 180, 150, 180, 0, 0, 0, 0, 0, 0);
      kCharsUnique[2] = new ConstStatus("ナル", 700, 800, 250, 200, 380, 250, 450, 0, 0, 0, 0, 0, 0);
      kCharsUnique[3] = new ConstStatus("リゼッタ", 750, 850, 310, 240, 450, 130, 350, 0, 0, 0, 0, 0, 0);
      kCharsUnique[4] = new ConstStatus("アスラ", 750, 600, 330, 190, 300, 230, 250, 0, 0, 0, 0, 0, 0);
      kCharsUnique[5] = new ConstStatus("マリア", 650, 990, 280, 170, 420, 180, 400, 0, 0, 0, 0, 0, 0);

      chars = StatusWritable.Instances(kCharsUnique);//MainData.kCharsNum);
      for (int i = 0; i < kCharsNum; i++)
        chars[i].Level(1);
    }
  }
}
