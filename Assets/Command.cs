using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void Do();
    void Undo();
}

[Serializable]
public class BucketCommand: ICommand
{
    private Hex hex;
    private Color newColor, oldColor;
    public BucketCommand(Hex h, Color c)
    {
        hex = h;
        newColor = c;
        oldColor = hex.BackgroundColor;
    }

    public void Do()
    {
        hex.SetBackgroundColor(newColor);
    }
    public void Undo()
    {
        hex.SetBackgroundColor(oldColor);
    }
}

[Serializable]
public class PaintCommand : ICommand
{
    private Hex hex;
    private Color newColor, oldColor;

    public PaintCommand(Hex h, Color c)
    {
        hex = h;
        newColor = c;
        oldColor = hex.ForegroundColor;
    }

    public void Do()
    {
        hex.SetForegroundColor(newColor);
    }
    public void Undo()
    {
        hex.SetForegroundColor(oldColor);
    }
}

[Serializable]
public class PencilCommand : ICommand
{
    private Hex hex;
    private Sprite newSprite, oldSprite;
    private string oldIconName, newIconName;

    public PencilCommand(Hex h, Sprite s, string newIconName)
    {
        hex = h;
        oldSprite = hex.Sprite;
        oldIconName = hex.IconName;
        newSprite = s;
        this.newIconName = newIconName;
    }

    public void Do()
    {
        hex.SetIcon(newSprite, newIconName);
    }
    public void Undo()
    {
        hex.SetIcon(oldSprite, oldIconName);
    }
}

[Serializable]
public class PencilPaintCommand : ICommand
{
    private Hex hex;
    private Color newColor, oldColor;
    private Sprite newSprite, oldSprite;
    private string oldIconName, newIconName;

    public PencilPaintCommand(Hex h, Color c, Sprite s, string newIconName)
    {
        hex = h;
        newColor = c;
        oldColor = hex.ForegroundColor;
        oldSprite = hex.Sprite;
        oldIconName = hex.IconName;
        newSprite = s;
        this.newIconName = newIconName;
    }

    public void Do()
    {
        hex.SetForegroundColor(newColor);
        hex.SetIcon(newSprite, newIconName);
    }
    public void Undo()
    {
        hex.SetForegroundColor(oldColor);
        hex.SetIcon(oldSprite, oldIconName);
    }
}

[Serializable]
public class OmniCommand : ICommand
{
    private Hex hex;
    private Color newBGColor, oldBGColor;
    private Color newFGColor, oldFGColor;
    private Sprite newSprite, oldSprite;
    private string oldIconName, newIconName;

    public OmniCommand(Hex h, Color bg, Color fg, Sprite s, string newIconName)
    {
        hex = h;
        newFGColor = fg;
        newBGColor = bg;
        oldBGColor = hex.BackgroundColor;
        oldFGColor = hex.ForegroundColor;
        oldSprite = hex.Sprite;
        oldIconName = hex.IconName;
        newSprite = s;
        this.newIconName = newIconName;
    }

    public void Do()
    {
        hex.SetBackgroundColor(newBGColor);
        hex.SetForegroundColor(newFGColor);
        hex.SetIcon(newSprite, newIconName);
    }
    public void Undo()
    {
        hex.SetBackgroundColor(oldBGColor);
        hex.SetForegroundColor(oldFGColor);
        hex.SetIcon(oldSprite, oldIconName);
    }
}