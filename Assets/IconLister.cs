using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IconLister : MonoBehaviour {
    public LibraryLister LibraryLister;
    public RectTransform DetailList, DetailTemplate;

	// Use this for initialization
	void Start () {
        LibraryLister.OnLibraryChanged += LibraryLister_OnLibraryChanged;
        DetailTemplate.gameObject.SetActive(false);
	}

    private void LibraryLister_OnLibraryChanged(Library newLib)
    {
        int i = 1;
        foreach(Sprite s in newLib.Icons)
        {
            RectTransform detail;
            if (i < DetailList.childCount - 2)
            {
                detail = DetailList.GetChild(i) as RectTransform;
            }
            else
            {
                detail = GameObject.Instantiate(DetailTemplate, DetailList) as RectTransform;
                detail.GetComponent<Button>().onClick.AddListener(() => this.onDetailClick(i));
                DetailTemplate.SetAsLastSibling();
            }
            detail.gameObject.SetActive(true);
            detail.GetChild(0).GetComponent<Text>().text = s.name;
            detail.GetChild(1).GetComponent<Image>().sprite = s;

            i++;
        }

        if (i < DetailList.childCount - 2)
        {
            for (int j = i; j < DetailList.childCount - 2 ; j++)
            {
                DetailList.GetChild(j).gameObject.SetActive(false);
            }
        }
    }

    private void onDetailClick(int index)
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
