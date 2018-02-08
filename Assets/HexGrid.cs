using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HexGridData
{
    public HexData[][] Hexes; //UNITY CANNOT DESERIALIZE THIS!!!
    public string Name = "map";
}

public class HexGrid : MonoBehaviour {
    public RectTransform spawnThis;
    public Painter painter;
    public Commander commander;

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
        HexGridData newMap = new HexGridData()
        {
            Hexes = new HexData[numColumns][]
        };

        for (int i = 0; i < numColumns; i++)
        {
            newMap.Hexes[i] = new HexData[numRows];
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
                newMap.Hexes[i][j] = h.Data;
            }
        }

        return newMap;
    }

    internal void LoadMap(HexGridData data, float radius)
    {
        float unitLength = (useAsInnerCircleRadius) ? (radius / (Mathf.Sqrt(3) / 2)) : radius;

        offsetX = unitLength * Mathf.Sqrt(3);
        offsetY = unitLength * 1.5f;

        for (int i = 0; i < data.Hexes.Length; i++)
        {
            int numRows = data.Hexes[i].Length;

            for (int j = 0; j < numRows; j++)
            {
                Vector2 hexpos = HexOffset(i, j);
                Vector3 pos = new Vector3(hexpos.x, hexpos.y, 0);
                var obj = Instantiate<RectTransform>(spawnThis, this.transform);
                obj.anchoredPosition = hexpos;// + new Vector2(unitLength, unitLength);
                obj.localRotation = Quaternion.identity;
                Hex h = obj.gameObject.AddComponent<Hex>();
                h.painter = painter;
                h.Assign(data.Hexes[i][j]);
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
