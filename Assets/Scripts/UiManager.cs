using Photon.Pun;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{

    public TextMeshProUGUI ammoIndicatorText;

    public Panels currentPanel;

    public GameObject pauseMenu, mainInventory, craftingPanel, furancePanel, anvilPanel, researchPanel;
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
        if(newPanel == Panels.None) ChangeAdditionalPanelState(AdditionalPanelType.None);
        if (newPanel == currentPanel) newPanel = Panels.None;
        currentPanel = newPanel;

        pauseMenu.SetActive(newPanel == Panels.Pause);
        mainInventory.SetActive(newPanel == Panels.Inventory);
        researchPanel.SetActive(newPanel == Panels.Research);

        ChangeCursorState(newPanel != Panels.None);
        CrosshairState(newPanel == Panels.None);
        somePanelTurnedOn = newPanel != Panels.None;

        playerMovement.canMove = newPanel == Panels.None || newPanel == Panels.Inventory;
        playerCamera.canUseMouse = newPanel == Panels.None;
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
    Research
}

public enum AdditionalPanelType { Crafting, Furance, Anvil, Chest, None }