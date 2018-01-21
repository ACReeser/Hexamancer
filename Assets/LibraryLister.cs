using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class LibraryLister : MonoBehaviour {
    public InputOutput IO;
    public Library CurrentLibrary;
    public Dropdown dropdown;

    public delegate void HandleLibraryChange(Library newLib);
    public event HandleLibraryChange OnLibraryChanged;

	// Use this for initialization
	void Start () {
        dropdown.onValueChanged.AddListener(this.dropdownValueChanged);
	    if (IO != null)
        {
            IO.OnLibrariesLoaded += IO_OnLibrariesLoaded;
        }
	}

    private void dropdownValueChanged(int newIndex)
    {
        CurrentLibrary = IO.Libraries[newIndex];
        if (OnLibraryChanged != null)
            OnLibraryChanged(CurrentLibrary);
    }

    private void IO_OnLibrariesLoaded(Library[] libs)
    {
        if (libs != null && libs.Length > 0)
        {
            CurrentLibrary = libs[0];
            dropdown.ClearOptions();
            List<Dropdown.OptionData> newOpts = libs.Select((x) =>
            {
                return new Dropdown.OptionData(x.Name);
            }).ToList();
            dropdown.AddOptions(newOpts);
            dropdown.value = 0;
            dropdownValueChanged(dropdown.value);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
