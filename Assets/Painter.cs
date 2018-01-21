using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Painter : MonoBehaviour {
    public RectTransform selector, detailListParent;
    public Commander commander;
    private Color selectedColor = Color.white;
    private Sprite selectedIcon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonUp(0))
        {
            var gameObj = EventSystem.current.currentSelectedGameObject;
            if (gameObj != null)
            {
                if (gameObj.name == "paintBucket")
                {
                    selectedColor = gameObj.GetComponent<Image>().color;
                    selector.parent = gameObj.transform;
                    selector.localPosition = Vector2.zero;
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
                                h.SetBackgroundColor(selectedColor);
                            }
                            break;
                        }
                        case Tool.ForegroundIconPlacer:
                        {
                            if (h != null)
                            {
                                h.SetForegroundIcon(selectedIcon);
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
