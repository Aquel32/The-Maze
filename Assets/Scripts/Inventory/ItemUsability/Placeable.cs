using Photon.Pun;
using UnityEngine;

public class Placeable : MonoBehaviourPunCallbacks, IUsableItem
{
    public GameObject buildPrefab;

    private UiManager uiManager;
    private InventoryManager inventoryManager;

    private Transform placeholder;
    private Transform attackDirection;

    private float yRotation;
    private float zRotation;

    void Start()
    {
        this.enabled = photonView.IsMine;
        if(buildPrefab.TryGetComponent<BuildingData>(out BuildingData data))
        {
            zRotation = data.zRotation;
        }
    }

    public void Update()
    {
        if (uiManager.somePanelTurnedOn) return;

        if (placeholder == null)
        {
            placeholder = Instantiate(buildPrefab).transform;
            placeholder.GetComponent<Collider>().enabled = false;
        }

        Vector3 pos;
        Ray ray = new Ray(attackDirection.position, attackDirection.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 4f)) pos = hitInfo.point;
        else pos = ray.GetPoint(4f);

        placeholder.position = pos;
        placeholder.rotation = Quaternion.Euler(0, yRotation, zRotation);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PhotonNetwork.Instantiate(buildPrefab.name, pos, placeholder.rotation);
            Destroy(placeholder.gameObject);
            inventoryManager.GetItem(inventoryManager.selectedSlot, true);
        }

        if (Input.GetKey(KeyCode.R)) yRotation = (yRotation + 200 * Time.deltaTime) % 360;
    }

    public void Initialize(InventoryItem newInventoryItem)
    {
        inventoryManager = newInventoryItem.inventoryManager;
        uiManager = inventoryManager.uiManager;
        attackDirection = inventoryManager.cameraTransform;
    }

    public void Deinitialize()
    {
        if (placeholder != null)
        {
            Destroy(placeholder.gameObject);
        }
    }
}
