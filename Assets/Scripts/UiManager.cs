using Photon.Pun;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    private void Awake() { Instance = this; }


    public Panels currentPanel;

    public GameObject pauseMenu, mainInventory, craftingPanel, furancePanel, anvilPanel, researchPanel, consolePanel;
    public GameObject crosshair;

    public bool somePanelTurnedOn;

    public void ChangeCurrentPanel(Panels newPanel)
    {
        if(newPanel == Panels.None) ChangeAdditionalPanelState(AdditionalPanelType.None);
        if (newPanel == currentPanel) newPanel = Panels.None;
        currentPanel = newPanel;

        pauseMenu.SetActive(newPanel == Panels.Pause);
        mainInventory.SetActive(newPanel == Panels.Inventory);
        researchPanel.SetActive(newPanel == Panels.Research);
        consolePanel.SetActive(newPanel == Panels.Console);

        ChangeCursorState(newPanel != Panels.None);
        CrosshairState(newPanel == Panels.None);
        somePanelTurnedOn = newPanel != Panels.None;

        PlayerMovement.Instance.canMove = newPanel == Panels.None;
        PlayerCamera.Instance.canUseMouse = newPanel == Panels.None;
    }

    public void ChangeAdditionalPanelState(AdditionalPanelType tableType)
    {
        GameObject currentAdditionalPanel = null;

        furancePanel.SetActive(tableType == AdditionalPanelType.Furance);
        craftingPanel.SetActive(tableType == AdditionalPanelType.Crafting);
        anvilPanel.SetActive(tableType == AdditionalPanelType.Anvil);

        if (tableType != AdditionalPanelType.None) ChangeCurrentPanel(Panels.Inventory);

        if (tableType == AdditionalPanelType.Furance) currentAdditionalPanel = furancePanel;
        if (tableType == AdditionalPanelType.Crafting) currentAdditionalPanel = craftingPanel;
        if (tableType == AdditionalPanelType.Anvil) currentAdditionalPanel = anvilPanel;


        if (currentAdditionalPanel == null) return;
        if (currentAdditionalPanel.TryGetComponent<ITable>(out ITable table)) table.Initialize();
    }

    public void ChangeCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = newState;
    }

    public void CrosshairState(bool newState)
    {
        crosshair.SetActive(newState);
    }
}

public enum Panels
{
    None,
    Pause,
    Inventory,
    Research,
    Console
}

public enum AdditionalPanelType { Crafting, Furance, Anvil, None }