using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Painter : MonoBehaviour {
    public RectTransform selector;
    private Color selectedColor = Color.white;

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
                else
                {
                    var h = gameObj.transform.parent.GetComponent<Hex>();
                    if (h != null)
                    {
                        h.SetBackgroundColor(selectedColor);
                    }
                }
            }
        }
    }
}
