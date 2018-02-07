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
    internal string IconName { get; private set; }
    internal Color BackgroundColor { get; private set; }
    internal Color ForegroundColor { get; private set; }
    internal Sprite Sprite { get; private set; }
    internal Painter painter;

	// Use this for initialization
	void Start () {
        this.button = transform.GetChild(0).GetComponent<Button>();
        this.hexImage = button.transform.GetComponent<Image>();
        this.iconImage = button.transform.GetChild(0).GetComponent<Image>();
        this.text = button.transform.GetChild(1).GetComponent<Text>();
        this.text.text = hexX + "," + hexY;
        this.button.onClick.AddListener(() => painter.OnHexClick(this));
        this.BackgroundColor = Color.white;
        this.ForegroundColor = Color.black;

        if (this.iconImage.enabled)
            this.Sprite = this.iconImage.sprite;
        else
            this.Sprite = null;
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
