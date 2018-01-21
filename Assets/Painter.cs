using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Painter : MonoBehaviour {
    public RectTransform bgColorSelector, fgColorSelector, paintbucketParent, detailListParent;
    public Commander commander;
    public Image bgPreview, fgPreview;
    public Sprite noSpriteImage;
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
	}

    private void OnPaintBucketLeftClick(Button b)
    {
        var gameObj = b.gameObject;
        if (gameObj != null)
        {
            if (gameObj.name == "paintBucket")
            {
                selectedBGColor = gameObj.GetComponent<Image>().color;
                bgColorSelector.parent = gameObj.transform;
                bgColorSelector.localPosition = Vector2.zero;
                bgPreview.color = selectedBGColor;
            }
            else if (gameObj.transform.parent == detailListParent)
            {
                if (gameObj.transform.GetSiblingIndex() == 0)
                {
                    selectedIcon = null;
                }
                else
                {
                    selectedIcon = gameObj.transform.GetChild(1).GetComponent<Image>().sprite;
                    selectedIconName = gameObj.name;
                }
            }
            else
            {
                var h = gameObj.transform.parent.GetComponent<Hex>();
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
                    case Tool.ForegroundIconPlacer:
                        {
                            if (h != null)
                            {
                                h.SetForegroundIcon(selectedIcon, selectedIconName, selectedFGColor);
                            }
                            break;
                        }
                }
            }
        }
    }

    void Update()
    {
        var gameObj = EventSystem.current.currentSelectedGameObject;
        if (gameObj != null)
        {
            if(Input.GetMouseButtonUp(0))
            {
                if (gameObj.transform.parent == detailListParent)
                {
                    if (gameObj.transform.GetSiblingIndex() == 0)
                    {
                        selectedIcon = null;
                    }
                    else
                    {
                        selectedIcon = gameObj.transform.GetChild(1).GetComponent<Image>().sprite;
                        selectedIconName = gameObj.name;
                    }
                }
                else
                {
                    var h = gameObj.transform.parent.GetComponent<Hex>();
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
                        case Tool.ForegroundIconPlacer:
                            {
                                if (h != null)
                                {
                                    h.SetForegroundIcon(selectedIcon, selectedIconName, selectedFGColor);
                                }
                                break;
                            }
                    }
                }
            }
        }
    }

    public void OnPaintBucketRightClick(Button b)
    {
        var gameObj = b.gameObject;
        if (gameObj != null)
        {
            if (gameObj.name == "paintBucket")
            {
                selectedFGColor = gameObj.GetComponent<Image>().color;
                fgColorSelector.parent = gameObj.transform;
                fgColorSelector.localPosition = Vector2.zero;
                fgPreview.color = selectedFGColor;
            }
        }
    }
}
