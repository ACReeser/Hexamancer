using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Library
{
    public string Name { get; set; }
    public Sprite[] Icons { get; set; }
    public string[] Tiles { get; set; }
}

public class InputOutput : MonoBehaviour {
    public Library[] Libraries;
    public delegate void HandleLibrariesLoaded(Library[] libs);
    public event HandleLibrariesLoaded OnLibrariesLoaded;

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadLibraries());	
	}
	
	// Update is called once per frame
	void Update () {

    }

    public IEnumerator LoadLibraries()
    {
        string resourcePath = Path.Combine(Path.Combine(Application.dataPath.Replace('/', '\\'), "Resources"), "Libraries");
        string[] dirs = Directory.GetDirectories(resourcePath);//, "", SearchOption.TopDirectoryOnly);

        Library[] results = new Library[dirs.Length];

        int i = 0;
        foreach (string dirPath in dirs)
        {
            Library newLib = new Library();
            newLib.Name = Path.GetFileName(dirPath);
            string iconFolderPath = Path.Combine(dirPath, "Icons");
            string[] iconFiles = Directory.GetFiles(iconFolderPath, "*.png", SearchOption.AllDirectories);
            newLib.Icons = new Sprite[iconFiles.Length];
            int spriteI = 0;
            foreach (string iconFilePath in iconFiles)
            {
                string iconName = Path.GetFileNameWithoutExtension(iconFilePath);
                string iconResourcePath = String.Format("Libraries/{0}/Icons/{1}", newLib.Name, iconName);
                var request = Resources.LoadAsync<Sprite>(iconResourcePath);
                yield return request;
                //var tex = request.asset as Texture2D;
                //newLib.Icons[spriteI] = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                newLib.Icons[spriteI] = request.asset as Sprite;
                newLib.Icons[spriteI].name = iconName;
                spriteI++;
            }
            results[i] = newLib;
            i++;
        }

        Libraries = results;
        if (OnLibrariesLoaded != null)
            OnLibrariesLoaded(Libraries);
    }
}
