using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* data example
*初期フィールド,32,30
11001100110011000011110111011001
01000010011101110110110111111100
10011010101001011001010110110101
00101000011010001100000110100011
00101011001101111101110110000011
11010000000010001001000011001000
00110001010010011110100100111101
10101110111111000110100011000110
01100010010101011100001011110100
11110010101010101111011110111000
00011011100001101001100011111011
00110000101101011010111010000100
01010111001100111000011011101010
00111010010111110000110101011101
10101101000101100010101001000001
10011101010101101110111010100100
11111101001011011000011101111001
01111110010011010100011111110110
00100000001011010100110110011011
00000011110000011000010000010000
00001100000011110001100101000110
00110100100101000110000010110100
11110100000000010010010111010011
00001101011000100000010010001101
00100011001010001111111100110101
10100101011011001111101100101011
11101001010011101001000011111000
10011110010110010000010111111010
11000011110010001100100001100011
10001010101010000101010110010110
*/
namespace Univ.Data
{
  internal class FieldMoveData
  {
    public readonly int fromX;
    public readonly int fromY;

    public readonly string toName;
    public readonly int toX;
    public readonly int toY;

    FieldMoveData(string txt, int width, int height, FieldData[] fieldDatas, bool isOutside)
    {
      string[] items = txt.Split(",");
      {
        if (items[0] == "L")
          fromX = isOutside ? -1 : 0;
        else if (items[0] == "R")
          fromX = isOutside ? width : width - 1;
        else
          fromX = int.Parse(items[0]);
      }
      {
        if (items[1] == "T")
          fromY = isOutside ? -1 : 0;
        else if (items[1] == "B")
          fromY = isOutside ? height : height - 1;
        else
          fromY = int.Parse(items[1]);
      }
      toName = items[2];
      int toWidth = 0;
      int toHeight = 0;
      int i;
      for (i = 0; i < fieldDatas.Length; i++)// FieldData d in fieldDatas)
      {
        if (fieldDatas[i].Name == toName)
        {
          toWidth = fieldDatas[i].Width;
          toHeight = fieldDatas[i].Height;
          break;
        }
      }
      JsTrans.Assert(i < fieldDatas.Length, "FieldData.cs FieldMoveData >Not found field name.");
      {
        if (items[3] == "L")
          toX = 0;
        else if (items[3] == "R")
          toX = toWidth - 1;
        else
          toX = int.Parse(items[3]);
      }
      {
        if (items[4] == "T")
          toY = 0;
        else if (items[4] == "B")
          toY = toHeight - 1;
        else
          toY = int.Parse(items[4]);
      }
    }

    static public FieldMoveData[] Create(string txt, int width, int height, FieldData[] fieldDatas, bool isOutside)
    {
      FieldMoveData[] datas = { };
      string[] chunks = txt.Split(";");
      JsTrans.Assert(chunks.Length != 0, "FieldData.cs FieldMoveData.Create() >chunks.Length != 0");
      if (chunks[0] == "-")
        return datas;
      
      datas = new FieldMoveData[chunks.Length];
      for (int i = 0; i < chunks.Length; i++)
      {
        datas[i] = new FieldMoveData(chunks[i], width, height, fieldDatas, isOutside);
      }
      return datas;
    }
  }
  internal class FieldData
  {
    public string Name;
    public int Width;
    public int Height;
    public int[,] Data;

    public FieldMoveData[] Doors;
    public FieldMoveData[] Stairs;

    string doorsText;
    string stairsText;

    // コンストラクタは*を取り除くこと("*"でsplitする)。
    public FieldData(string txt)
    {
      string[] rows = txt.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
      string[] heads = rows[0].Split(",", StringSplitOptions.RemoveEmptyEntries);
      JsTrans.Assert(heads.Length == 3, "Data FieldData.cs heads.Length == 3");
      Name = heads[0];
      Width = int.Parse(heads[1]);
      Height = int.Parse(heads[2]);
      JsTrans.Assert(rows.Length == Height + 3, "Data FieldData.cs chunks.Length == Height + 3");

      //Doors = FieldMoveData.Create(rows[1]);
      //Stairs = FieldMoveData.Create(rows[2]);
      doorsText = rows[1];
      stairsText = rows[2];

      //Array.Copy(chunks, 1, chunks, 0, Height);
      rows = rows.Where((item, index) => index >= 3).ToArray();
      Data = new int[Height, Width];
      for (int y = 0; y < rows.Length; y++)
      {
        string row = rows[y];
        for (int x = 0; x < row.Length; x++)
        {
          Data[y, x] = int.Parse(row[x].ToString());
        }
      }
    }
    public void ConstructorLatter(FieldData[] fieldDatas)
    {
      Doors = FieldMoveData.Create(doorsText, Width, Height, fieldDatas, true);
      Stairs = FieldMoveData.Create(stairsText, Width, Height, fieldDatas, false);
    }

    static public FieldData Instance(string name)
    {
      Data.Loader loader = Loader.Instance;
      Data.FieldData[] fData = loader.fieldData;
      foreach (var f in fData)
      {
        if (f.Name == name)
          return f;
      }
      JsTrans.Assert("FieldData.cs Instance");
      return null;
    }
  }
}
