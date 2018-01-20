﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hex : MonoBehaviour {
    private Button button;
    private Text text;
    private Image hexImage, iconImage;
    private int hexX, hexY;

	// Use this for initialization
	void Start () {
        this.button = transform.GetChild(0).GetComponent<Button>();
        this.hexImage = button.transform.GetComponent<Image>();
        this.iconImage = button.transform.GetChild(0).GetComponent<Image>();
        this.text = button.transform.GetChild(1).GetComponent<Text>();
        this.text.text = hexX + "," + hexY;
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
        this.hexImage.color = selectedColor;
    }
}