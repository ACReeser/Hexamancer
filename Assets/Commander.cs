using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tool { None = 0, BackgroundPainter, ForegroundIconPlacer }
public enum Detail { None = 0, Icons }

public class Commander : MonoBehaviour {
    public Tool CurrentTool { get; private set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTool(int index)
    {
        CurrentTool = (Tool)index;
    }
}
