﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tool { None = 0, BackgroundPainter, ForegroundPainter, IconPlacer, PaintedIconPlacerPainter, OmniPainter, NotesPen, Panner }
public enum Detail { None = 0, Icons }

public class Commander : MonoBehaviour {
    public Tool CurrentTool { get; private set; }
    public Painter painterUI;
    public InputOutput IO;

    internal Stack<ICommand> pastCommands = new Stack<ICommand>();
    internal Stack<ICommand> futureCommands = new Stack<ICommand>();
    internal HexGridData CurrentMap { get; private set; }

    internal void SetMap(HexGridData hexGridData)
    {
        this.CurrentMap = hexGridData;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl))
        {
            Undo();
        } else if (Input.GetKeyUp(KeyCode.Y) && Input.GetKey(KeyCode.LeftControl))
        {
            Redo();
        }
	}

    public void SetTool(Tool tool)
    {
        CurrentTool = tool;
    }

    public void EditHex(ICommand command)
    {
        command.Do();
        pastCommands.Push(command);
        futureCommands.Clear();
        IO.OnHexChange();
    }

    public void Undo()
    {
        if (pastCommands.Count > 0)
        {
            var command = pastCommands.Pop();
            command.Undo();
            futureCommands.Push(command);
            IO.OnHexChange();
        }
    }
    public void Redo()
    {
        if (futureCommands.Count > 0)
        {
            var command = futureCommands.Pop();
            command.Do();
            pastCommands.Push(command);
            IO.OnHexChange();
        }
    }
}
