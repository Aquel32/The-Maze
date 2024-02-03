using Photon.Pun;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    public TextMeshProUGUI ammoIndicatorText;

    public Panels currentPanel;

    public GameObject pauseMenu, mainInventory, craftingPanel, furancePanel, anvilPanel;
    public GameObject crosshair;

    public bool somePanelTurnedOn;

    [SerializeField] private PlayerCamera playerCamera;
    private PlayerMovement playerMovement;

    public GameObject currentChestPanel;

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

    public void SetChestPanel(GameObject chestPanel) { currentChestPanel = chestPanel; }

    public void ChangeAdditionalPanelState(AdditionalPanelType tableType)
    {
        GameObject currentAdditionalPanel = null;

        if (currentChestPanel != null)
        {
            currentChestPanel.SetActive(tableType == AdditionalPanelType.Chest);
            if(tableType == AdditionalPanelType.None)
            {
                currentChestPanel.GetComponentInParent<Chest>().animationState = false;
                currentChestPanel.GetComponentInParent<Chest>().photonView.RPC("ChangeAnimationStateRPC", RpcTarget.AllBuffered, false);
            }
        }
        furancePanel.SetActive(tableType == AdditionalPanelType.Furance);
        craftingPanel.SetActive(tableType == AdditionalPanelType.Crafting);
        anvilPanel.SetActive(tableType == AdditionalPanelType.Anvil);

        if (tableType == AdditionalPanelType.Furance) currentAdditionalPanel = furancePanel;
        if (tableType == AdditionalPanelType.Crafting) currentAdditionalPanel = craftingPanel;
        if (tableType == AdditionalPanelType.Anvil) currentAdditionalPanel = anvilPanel;


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

public enum AdditionalPanelType { Crafting, Furance, Anvil, Chest, None }