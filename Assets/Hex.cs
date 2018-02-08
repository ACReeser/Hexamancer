using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HexData
{
    public int HexX, HexY;
    public string IconName;
    public Color BackgroundColor;
    public Color ForegroundColor;

}

public class Hex : MonoBehaviour {
    private Button button;
    private Text text;
    private Image hexImage, iconImage;

    internal Sprite Sprite { get; private set; }

    public int hexX { get { return Data.HexX; } private set { Data.HexX = value; } }
    public int hexY { get { return Data.HexY; } private set { Data.HexY = value; } }
    internal string IconName { get { return Data.IconName; } private set { Data.IconName = value; } }
    internal Color BackgroundColor { get { return Data.BackgroundColor; } private set { Data.BackgroundColor = value; } }
    internal Color ForegroundColor { get { return Data.ForegroundColor; } private set { Data.ForegroundColor = value; } }

    internal Painter painter;

    internal HexData Data { get; set; }

	// Use this for initialization
	void Start () {
        this.button = transform.GetChild(0).GetComponent<Button>();
        this.hexImage = button.transform.GetComponent<Image>();
        this.iconImage = button.transform.GetChild(0).GetComponent<Image>();
        this.text = button.transform.GetChild(1).GetComponent<Text>();
        this.text.text = hexX + "," + hexY;
        this.button.onClick.AddListener(() => painter.OnHexClick(this));
        
        this.iconImage.enabled = IconName != null && IconName.Length > 0;

        if (this.iconImage.enabled)
            this.Sprite = this.iconImage.sprite;
        else
            this.Sprite = null;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Assign(HexData data)
    {
        this.Data = data;
    }

    internal void SetBackgroundColor(Color selectedColor)
    {
        BackgroundColor = selectedColor;
        this.hexImage.color = selectedColor;
    }

    internal void SetForegroundColor(Color color)
    {
        this.iconImage.color = color;
        this.ForegroundColor = color;
    }

    internal void SetIcon(Sprite selectedIcon, string name)
    {
        this.iconImage.enabled = selectedIcon != null;
        this.iconImage.sprite = selectedIcon;
        this.IconName = name;
    }
}
