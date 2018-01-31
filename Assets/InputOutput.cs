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

    private IO Engine;

	// Use this for initialization
	void Start () {
        Engine = GetIO();
        StartCoroutine(LoadLibraries());
	}

    private IO GetIO()
    {
        bool isWeb = Application.dataPath.StartsWith("http");
        if (isWeb)
        {
            return new WebIO();
        }
        else
        {
            return new DesktopIO();
        }
    }

    // Update is called once per frame
    void Update () {

    }

    public IEnumerator LoadLibraries()
    {
        yield return Engine.GetLibraries();

        Libraries = Engine.LoadedLibraries;

        foreach(Library lib in Libraries)
        {
            yield return Engine.LoadIcons(lib);
        }

        if (OnLibrariesLoaded != null)
            OnLibrariesLoaded(Libraries);
    }

    public interface IO
    {
        Library[] LoadedLibraries { get; }
        IEnumerator GetLibraries();
        IEnumerator LoadIcons(Library lib);
    }

    public class DesktopIO: IO
    {
        public Library[] LoadedLibraries { get; private set; }

        private string libraryPath { get { return Path.Combine(Path.Combine(Application.dataPath.Replace('/', '\\'), "Resources"), "Libraries"); } }
        public IEnumerator GetLibraries()
        {
            string[] dirs = Directory.GetDirectories(libraryPath);//, "", SearchOption.TopDirectoryOnly);

            Library[] results = new Library[dirs.Length];

            int i = 0;
            foreach (string dirPath in dirs)
            {
                Library newLib = new Library();
                newLib.Name = Path.GetFileName(dirPath);
                results[i] = newLib;
                i++;
            }
            LoadedLibraries = results;
            yield return results;
        }

        public IEnumerator LoadIcons(Library lib)
        {
            string iconFolderPath = Path.Combine(Path.Combine(libraryPath, lib.Name), "Icons");
            string[] iconFiles = Directory.GetFiles(iconFolderPath, "*.png", SearchOption.AllDirectories);
            lib.Icons = new Sprite[iconFiles.Length];
            int spriteI = 0;
            foreach (string iconFilePath in iconFiles)
            {
                string iconName = Path.GetFileNameWithoutExtension(iconFilePath);
                string iconResourcePath = String.Format("Libraries/{0}/Icons/{1}", lib.Name, iconName);
                var request = Resources.LoadAsync<Sprite>(iconResourcePath);
                yield return request;
                //var tex = request.asset as Texture2D;
                //newLib.Icons[spriteI] = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                lib.Icons[spriteI] = request.asset as Sprite;
                lib.Icons[spriteI].name = iconName;
                spriteI++;
            }
        }
    }

    public class WebIO : IO
    {
        public Library[] LoadedLibraries { get; private set; }
        public IEnumerator GetLibraries()
        {
            bool isWeb = Application.dataPath.StartsWith("http");
            string basePath = Application.dataPath;
            yield return null;
        }
        public IEnumerator LoadIcons(Library lib)
        {
            yield return null;
        }
    }
}
