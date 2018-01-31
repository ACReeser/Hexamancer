using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hex : MonoBehaviour {
    private Button button;
    private Text text;
    private Image hexImage, iconImage;
    private int hexX, hexY;
    private string iconName;
    private Color bgColor, fgColor;
    internal Painter painter;

	// Use this for initialization
	void Start () {
        this.button = transform.GetChild(0).GetComponent<Button>();
        this.hexImage = button.transform.GetComponent<Image>();
        this.iconImage = button.transform.GetChild(0).GetComponent<Image>();
        this.text = button.transform.GetChild(1).GetComponent<Text>();
        this.text.text = hexX + "," + hexY;
        this.button.onClick.AddListener(() => painter.OnHexClick(this));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Assign(int x, int y)
    {
        hexX = x;
        hexY = y;
    }

    internal void SetBackgroundColor(Color selectedColor)
    {
        bgColor = selectedColor;
        this.hexImage.color = selectedColor;
    }

    internal void SetForegroundColor(Color color)
    {
        this.iconImage.color = color;
        this.fgColor = color;
    }

    internal void SetIcon(Sprite selectedIcon, string name)
    {
        this.iconImage.enabled = selectedIcon != null;
        this.iconImage.sprite = selectedIcon;
        this.iconName = name;
    }
}
