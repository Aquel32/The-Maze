using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    public TextMeshProUGUI ammoIndicatorText;

    public Panels currentPanel;

    public GameObject pauseMenu, mainInventory, craftingPanel, furancePanel;
    public GameObject crosshair;

    public bool somePanelTurnedOn;

    [SerializeField] private PlayerCamera playerCamera;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        ChangeCurrentPanel(Panels.None);
    }

    public void ChangeCurrentPanel(Panels newPanel)
    {
        currentPanel = newPanel;

        switch (currentPanel)
        {
            case Panels.None:
                pauseMenu.SetActive(false);
                mainInventory.SetActive(false);
                playerMovement.canMove = true;
                playerCamera.canUseMouse = true;
                CursorOff();
                CrosshairState(true);
                somePanelTurnedOn = false;
                break;
            case Panels.Pause:
                pauseMenu.SetActive(true);
                mainInventory.SetActive(false);
                playerMovement.canMove = false;
                playerCamera.canUseMouse = false;
                CursorOn();
                CrosshairState(false);
                somePanelTurnedOn = true;
                break;
            case Panels.Inventory:
                pauseMenu.SetActive(false);
                mainInventory.SetActive(true);
                playerMovement.canMove = true;
                playerCamera.canUseMouse = false;
                CursorOn();
                CrosshairState(false);
                somePanelTurnedOn = true;
                break;
            default:
                pauseMenu.SetActive(false);
                mainInventory.SetActive(false);
                playerMovement.canMove = true;
                playerCamera.canUseMouse = true;
                CursorOff();
                CrosshairState(true);
                somePanelTurnedOn = false;
                break;
        }   
    }

    public void ChangeAdditionalPanelState(AdditionalPanelType tableType)
    {
        GameObject currentAdditionalPanel = null;
        switch (tableType)
        {
            case AdditionalPanelType.None:
                craftingPanel.SetActive(false);
                furancePanel.SetActive(false);
                break;
            case AdditionalPanelType.Crafting:
                craftingPanel.SetActive(true);
                furancePanel.SetActive(false);
                currentAdditionalPanel = craftingPanel;
                break;
            case AdditionalPanelType.Furance:
                craftingPanel.SetActive(false);
                furancePanel.SetActive(true);
                currentAdditionalPanel = furancePanel;
                break;
            default:
                craftingPanel.SetActive(false);
                furancePanel.SetActive(false);
                break;
        }

        if (currentAdditionalPanel == null) return;
        if (currentAdditionalPanel.TryGetComponent<ITable>(out ITable table)) table.Initialize();
    }

    public void CursorOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CursorOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
}

public enum AdditionalPanelType { Crafting, Furance, Chest, None }