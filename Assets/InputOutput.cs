using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

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
            //string basePath = Application.dataPath;
            var www = new UnityWebRequest(Application.dataPath + "/Resources/Libraries/");
            www.SetRequestHeader("Accept", "application/json");
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.Send();

            if (www.isError)
            {
                Debug.Log(www.error);
            }
            else {
                Debug.Log(www.downloadHandler.text);
                var files = JsonHelper.getJsonArray<CaddyFileInfo>(www.downloadHandler.text);
                // Or retrieve results as binary data
                //byte[] results = www.downloadHandler.data;

                var libList = new List<Library>();
                foreach(var file in files)
                {
                    if (file.IsDir)
                    {
                        libList.Add(new Library()
                        {
                            Name = file.Name
                        });
                    }
                }
                LoadedLibraries = libList.ToArray();
            }
        }
        public IEnumerator LoadIcons(Library lib)
        {
            var libraryListRequest = new UnityWebRequest(Application.dataPath + "/Resources/Libraries/"+lib.Name+"/Icons/");
            libraryListRequest.SetRequestHeader("Accept", "application/json");
            libraryListRequest.downloadHandler = new DownloadHandlerBuffer();
            yield return libraryListRequest.Send();

            if (libraryListRequest.isError)
            {
                Debug.Log(libraryListRequest.error);
            }
            else {
                Debug.Log(libraryListRequest.downloadHandler.text);
                var files = JsonHelper.getJsonArray<CaddyFileInfo>(libraryListRequest.downloadHandler.text);

                var sprites = new List<Sprite>();
                foreach (var file in files)
                {
                    if (!file.IsDir && !file.Name.EndsWith("meta"))
                    {
                        var spriteRequest = new UnityWebRequest(Application.dataPath + "/Resources/Libraries/" + lib.Name + "/Icons/"+file.Name);
                        spriteRequest.SetRequestHeader("Accept", "application/json");
                        spriteRequest.downloadHandler = new DownloadHandlerTexture();
                        yield return spriteRequest.Send();

                        if (spriteRequest.isError)
                        {
                            Debug.Log(libraryListRequest.error);
                        }
                        else
                        {
                            var tex = (spriteRequest.downloadHandler as DownloadHandlerTexture).texture;
                            var newSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));
                            newSprite.name = Path.GetFileNameWithoutExtension(file.Name);
                            sprites.Add(newSprite);
                        }
                    }
                }
                lib.Icons = sprites.ToArray();
            }
        }
    }

    [Serializable]
    public class CaddyFileInfo
    {
        public bool IsDir;
        public string Name;
    }

    public class JsonHelper
    {
        public static T[] getJsonArray<T>(string json)
        {
            string newJson = "{ \"array\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
            return wrapper.array;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] array;
        }
    }
}
