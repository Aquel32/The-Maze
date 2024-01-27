using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private UiManager uiManager;

    private void Start()
    {
        uiManager = GetComponent<UiManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Settings.Instance.CloseSettingsMenu();
            if (uiManager.currentPanel == Panels.None)
            {
                uiManager.ChangeCurrentPanel(Panels.Pause);
            }
            else if (uiManager.currentPanel == Panels.Pause)
            {
                uiManager.ChangeCurrentPanel(Panels.None);
            }
        }
    }

    public void Resume()
    {
        Settings.Instance.CloseSettingsMenu();
        uiManager.ChangeCurrentPanel(Panels.None);
    }

    public void SettingsButton()
    {
        Settings.Instance.OpenSettingsMenu();
    }

    public void Quit()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
