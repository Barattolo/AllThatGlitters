using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    PlayerInputs inputs;

    public List<Interactable> availableInteractables;
    public Interactable closestInteractable;

    private void Start()
    {
        inputs = GetComponent<PlayerInputs>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable otherInteractable))
        {            
            availableInteractables.Add(otherInteractable);            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Interactable>(out Interactable otherInteractable))
        {
            availableInteractables.Remove(otherInteractable);
            otherInteractable.isPlayerNear = false;
        }
    }

    void FindClosestInteractable()
    {
        float maxDistance = Mathf.Infinity;
        closestInteractable = null;

        foreach (var interactable in availableInteractables)
        {
            float targetDistance = Vector3.Distance(transform.position, interactable.transform.position);
            interactable.isPlayerNear = false;

            if (targetDistance < maxDistance)
            {
                maxDistance = targetDistance;
                closestInteractable = interactable;
                closestInteractable.isPlayerNear = true; 
            }
        }
        
    }

    private void Update()
    {
        if (availableInteractables != null)
        {
            FindClosestInteractable();

        }

        if (inputs.inputE && closestInteractable != null)
        {
            //closestInteractable.  QUI METTO L'INTERAZIONE CHE DEVE FARE
        }
      
    }
}
