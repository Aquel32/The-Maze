using Photon.Pun;
using UnityEngine;

public class Placeable : MonoBehaviourPunCallbacks, IUsableItem
{
    public GameObject buildPrefab;

    private Transform placeholder;

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
        if (UiManager.Instance.somePanelTurnedOn) return;

        if (placeholder == null)
        {
            placeholder = Instantiate(buildPrefab).transform;
            placeholder.GetComponent<Collider>().enabled = false;
        }

        Vector3 pos;
        Ray ray = new Ray(InventoryManager.Instance.playerCamera.position, InventoryManager.Instance.playerCamera.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 4f)) pos = hitInfo.point;
        else pos = ray.GetPoint(4f);

        placeholder.position = pos;
        placeholder.rotation = Quaternion.Euler(0, yRotation, zRotation);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PhotonNetwork.Instantiate(buildPrefab.name, pos, placeholder.rotation);
            Destroy(placeholder.gameObject);
            InventoryManager.Instance.GetItem(InventoryManager.Instance.selectedSlot, true);
        }

        if (Input.GetKey(KeyCode.R)) yRotation = (yRotation + 200 * Time.deltaTime) % 360;
    }

    public void Initialize(InventoryItem newInventoryItem)
    {
    }

    public void Deinitialize()
    {
        if (placeholder != null)
        {
            Destroy(placeholder.gameObject);
        }
    }
}
