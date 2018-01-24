using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigator : MonoBehaviour {
    public MouseMove mover;
    public Button ZoomOut, ZoomIn;

    void Start()
    {
        ZoomIn.onClick.AddListener(() =>
        {
            mover.Zoom(.5f);
        });
        ZoomOut.onClick.AddListener(() =>
        {
            mover.Zoom(-.5f);
        });
    }
}
