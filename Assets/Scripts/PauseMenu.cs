using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Settings.Instance.CloseSettingsMenu();
            UiManager.Instance.ChangeCurrentPanel(Panels.Pause);
        }
    }

    public void Resume()
    {
        Settings.Instance.CloseSettingsMenu();
        UiManager.Instance.ChangeCurrentPanel(Panels.Pause);
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
