using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Painter : MonoBehaviour {
    public RectTransform bgColorSelector, fgColorSelector, paintbucketParent, detailListParent, selectedToolBorder;
    public Commander commander;
    public Image bgPreview, fgPreview;
    public Sprite noSpriteImage;
    public Button[] ToolButtons;
    private Color selectedBGColor = Color.white;
    private Color selectedFGColor = Color.black;
    private Sprite selectedIcon;
    private string selectedIconName;

	// Use this for initialization
	void Start () {
		foreach(RectTransform t in paintbucketParent)
        {
            var b = t.GetComponent<Button>();
            if (b != null)
            {
                b.onClick.AddListener(() => this.OnPaintBucketLeftClick(b));
            }
            var rcb = t.GetComponent<RightClickButton>();
            if (rcb != null)
            {
                rcb.onRightClick.AddListener(() => this.OnPaintBucketRightClick(b));
            }
        }
        foreach(Button b in ToolButtons)
        {
            b.onClick.AddListener(() => SelectTool((Tool)b.transform.GetSiblingIndex() - 1));
        }
	}

    internal void OnHexClick(Hex h)
    {
        switch (commander.CurrentTool)
        {
            case Tool.BackgroundPainter:
                {
                    if (h != null)
                    {
                        h.SetBackgroundColor(selectedBGColor);
                    }
                    break;
                }
            case Tool.ForegroundPainter:
                {
                    if (h != null)
                    {
                        h.SetForegroundColor(selectedFGColor);
                    }
                    break;
                }
            case Tool.IconPlacer:
                {
                    if (h != null)
                    {
                        h.SetIcon(selectedIcon, selectedIconName);
                    }
                    break;
                }
            case Tool.PaintedIconPlacerPainter:
                {
                    if (h != null)
                    {
                        h.SetIcon(selectedIcon, selectedIconName);
                        h.SetForegroundColor(selectedFGColor);
                    }
                    break;
                }
            case Tool.OmniPainter:
                {
                    if (h != null)
                    {
                        h.SetBackgroundColor(selectedBGColor);
                        h.SetIcon(selectedIcon, selectedIconName);
                        h.SetForegroundColor(selectedFGColor);
                    }
                    break;
                }
        }
    }

    private void SelectTool(Tool buttonI)
    {
        selectedToolBorder.parent = ToolButtons[Convert.ToInt32(buttonI)].transform;
        selectedToolBorder.localPosition = Vector2.zero;
        commander.SetTool(buttonI);
    }

    private void OnPaintBucketLeftClick(Button b)
    {
        var gameObj = b.gameObject;
        if (gameObj != null)
        {
            if (gameObj.name == "paintBucket")
            {
                switch (commander.CurrentTool)
                {
                    case Tool.BackgroundPainter:
                        selectedBGColor = gameObj.GetComponent<Image>().color;
                        bgColorSelector.parent = gameObj.transform;
                        bgColorSelector.localPosition = Vector2.zero;
                        bgPreview.color = selectedBGColor;
                        break;
                    case Tool.ForegroundPainter:
                    case Tool.IconPlacer:
                    case Tool.PaintedIconPlacerPainter:
                        selectedFGColor = gameObj.GetComponent<Image>().color;
                        fgColorSelector.parent = gameObj.transform;
                        fgColorSelector.localPosition = Vector2.zero;
                        fgPreview.color = selectedFGColor;
                        break;
                }
            }
        }
    }

    public void OnPaintBucketRightClick(Button b)
    {

    }

    public void OnIconListClick(Button b)
    {
        if (b.transform.GetSiblingIndex() == 0)
        {
            fgPreview.sprite = b.transform.GetChild(1).GetComponent<Image>().sprite;
            selectedIcon = null;
        }
        else
        {
            selectedIcon = b.transform.GetChild(1).GetComponent<Image>().sprite;
            fgPreview.sprite = selectedIcon;
            selectedIconName = b.gameObject.name;
        }
    }
}
