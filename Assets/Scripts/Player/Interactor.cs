using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractible
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{
    public float interactionDistance;

    void Update()
    {
        if (UiManager.Instance.somePanelTurnedOn) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, interactionDistance))
            {
                if(hitInfo.collider.gameObject.TryGetComponent(out IInteractible interactObj))
                {
                    interactObj.Interact();
                }
            }
        }
    }
}
