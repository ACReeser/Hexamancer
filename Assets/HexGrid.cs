using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HexGridData
{
    [SerializeField]
    private HexData[] flatHexes;
    public HexData[,] Hexes { get; private set; }
    public int NumberOfColumns, NumberOfRows;
    public string Name = "map";
    public void PrepareForSerialization()
    {
        if (flatHexes == null)
        {
            flatHexes = new HexData[NumberOfColumns * NumberOfRows];
            int i = 0;
            foreach(HexData h in Hexes)
            {
                flatHexes[i] = h;
                i++;
            }
        }
    }
    public void DoAfterDeserialization()
    {
        Hexes = new HexData[NumberOfColumns, NumberOfRows];

        foreach (HexData h in flatHexes)
        {
            Hexes[h.HexX, h.HexY] = h;
        }
    }

    public HexGridData() { }
    public HexGridData(int cols, int rows)
    {
        NumberOfColumns = cols;
        NumberOfRows = rows;
        Hexes = new HexData[cols, rows];
    }
}

public class HexGrid : MonoBehaviour {
    public RectTransform spawnThis;
    public Painter painter;
    public Commander commander;
    public InputOutput IO;

    //public int x = 21;
    //public int y = 15;

    //public float radius = 50f;
    public bool useAsInnerCircleRadius = true;

    private float offsetX, offsetY;

    void Start()
    {
    }

    internal HexGridData CreateNewMap(int numColumns, int numRows, float radius)
    {
        float unitLength = (useAsInnerCircleRadius) ? (radius / (Mathf.Sqrt(3) / 2)) : radius;

        offsetX = unitLength * Mathf.Sqrt(3);
        offsetY = unitLength * 1.5f;
        HexGridData newMap = new HexGridData(numColumns, numRows);

        for (int i = 0; i < numColumns; i++)
        {
            for (int j = 0; j < numRows; j++)
            {
                Vector2 hexpos = HexOffset(i, j);
                Vector3 pos = new Vector3(hexpos.x, hexpos.y, 0);
                var obj = Instantiate<RectTransform>(spawnThis, this.transform);
                obj.anchoredPosition = hexpos;// + new Vector2(unitLength, unitLength);
                obj.localRotation = Quaternion.identity;
                Hex h = obj.gameObject.AddComponent<Hex>();
                h.painter = painter;
                h.Assign(new HexData()
                {
                    HexX = i,
                    HexY = j,
                    BackgroundColor = Color.white,
                    ForegroundColor = Color.black,
                    IconName = ""
                });
                newMap.Hexes[i,j] = h.Data;
            }
        }

        return newMap;
    }

    internal void LoadMap(HexGridData data, float radius)
    {
        float unitLength = (useAsInnerCircleRadius) ? (radius / (Mathf.Sqrt(3) / 2)) : radius;

        offsetX = unitLength * Mathf.Sqrt(3);
        offsetY = unitLength * 1.5f;

        for (int i = 0; i < data.NumberOfColumns; i++)
        {
            for (int j = 0; j < data.NumberOfRows; j++)
            {
                Vector2 hexpos = HexOffset(i, j);
                Vector3 pos = new Vector3(hexpos.x, hexpos.y, 0);
                var obj = Instantiate<RectTransform>(spawnThis, this.transform);
                obj.anchoredPosition = hexpos;// + new Vector2(unitLength, unitLength);
                obj.localRotation = Quaternion.identity;
                Hex h = obj.gameObject.AddComponent<Hex>();
                h.painter = painter;
                h.Assign(data.Hexes[i,j]);
                string iconName = data.Hexes[i, j].IconName;
                if (IO.Libraries != null && iconName != null && iconName.Length > 0)
                {
                    string[] splits = iconName.Split('|');
                    foreach(var lib in IO.Libraries)
                    {
                        if (lib.Name == splits[0])
                        {
                            if (lib.Icons != null)
                            {
                                foreach(var sprite in lib.Icons)
                                {
                                    if (sprite.name == splits[1])
                                    {
                                        data.Hexes[i, j].Sprite = sprite;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    Vector2 HexOffset(int x, int y)
    {
        Vector2 position = Vector2.zero;

        if (y % 2 == 0)
        {
            position.x = x * offsetX;
            position.y = y * offsetY;
        }
        else {
            position.x = (x + 0.5f) * offsetX;
            position.y = y * offsetY;
        }

        return position;
    }
}
