using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ViewState { LoggedOut, LoggingIn, LoggedIn, SelectLoadedMap, SetUpNewMap, Closed }

public class Menu : MonoBehaviour {
    public RectTransform modal, title, loggedInPanel, loggedOutPanel, loginPanel, loadMapPanel, newMapPanel;
    public Input user, pass;
    public Commander commander;
    public HexGrid grid;
    public InputOutput IO;

    private ViewState CurrentViewState = ViewState.LoggedOut;

    // Use this for initialization
    void Start () {
        modal.gameObject.SetActive(true);
        RefreshViewState();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Toggle()
    {
        if (CurrentViewState == ViewState.Closed)
        {
            RefreshViewState(ViewState.LoggedIn);
        }
        else
        {
            RefreshViewState(ViewState.Closed);
        }
    }

    public void OpenLoginInputs()
    {
        RefreshViewState(ViewState.LoggingIn);
    }

    private void RefreshViewState(ViewState? newViewState = null)
    {
        if (newViewState.HasValue)
            CurrentViewState = newViewState.Value;

        //switch (CurrentViewState)
        //{
        //    case 
        //}
        this.modal.gameObject.SetActive(CurrentViewState != ViewState.Closed);
        this.title.gameObject.SetActive(CurrentViewState != ViewState.SelectLoadedMap);
        this.loggedOutPanel.gameObject.SetActive(CurrentViewState == ViewState.LoggedOut);
        this.loginPanel.gameObject.SetActive(CurrentViewState == ViewState.LoggingIn);
        this.loggedInPanel.gameObject.SetActive(CurrentViewState == ViewState.LoggedIn);
        this.loadMapPanel.gameObject.SetActive(CurrentViewState == ViewState.SelectLoadedMap);
        //this.newMapPanel.gameObject.SetActive(CurrentViewState == ViewState.SetUpNewMap);
    }

    public void Login()
    {
        RefreshViewState(ViewState.LoggedIn);
    }
    public void BackToLoggedIn()
    {
        RefreshViewState(ViewState.LoggedIn);
    }
    public void LogOut()
    {
        RefreshViewState(ViewState.LoggedOut);
    }
    public void SetupNewMap()
    {
        commander.SetMap(grid.CreateNewMap(21, 15, 50f));

        //RefreshViewState(ViewState.SetUpNewMap);
        RefreshViewState(ViewState.Closed);
    }
    public void OpenSavedMaps()
    {
        IO.Load("map");
        grid.LoadMap(commander.CurrentMap, 50f);
        RefreshViewState(ViewState.Closed);
        //RefreshViewState(ViewState.SelectLoadedMap);
    }


}
