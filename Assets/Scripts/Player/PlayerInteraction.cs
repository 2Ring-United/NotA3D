using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    List<Collider> colliders;
    Collider closestCollider = null;

    private void Start()
    {
        colliders = new List<Collider>();
    }
    private void Update()
    {
        FindClosest();
        Interact();

    }

    private void Interact()
    {
        if(closestCollider == null)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            if (closestCollider.TryGetComponent<IInteractable>(out IInteractable pickup) && closestCollider != null)
            {
                if (pickup.Interact())
                {
                    colliders.Remove(closestCollider);
                    Destroy(closestCollider.gameObject);
                }
            }
            
        }
    }
    private void FindClosest()
    {
        float minDistance = 100000f;
        float distance;
        Collider tempClosest = null;
        if (colliders.Count == 0)
        {
            closestCollider = null;
            return;
        }
        foreach (var collider in colliders)
        {   
            if (collider == null)
                continue;
            distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                tempClosest = collider;
            }
            else
            {

                collider.GetComponent<IInteractable>().HideInteractionOnGUI();
            }

        }

        closestCollider = tempClosest;
        closestCollider.GetComponent<IInteractable>().ShowInteractionOnGUI();

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("contact " + other.name);

        if (other.TryGetComponent<IInteractable>(out IInteractable i))
        {
            colliders.Add(other);
            Debug.Log("Added " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (colliders.Contains(other))
        {
            other.GetComponent<IInteractable>().HideInteractionOnGUI();
            colliders.Remove(other);    
            Debug.Log("Added " + other.name);

        }
    }
}
