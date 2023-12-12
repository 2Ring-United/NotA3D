using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void ShowInteractionOnGUI();
    public void HideInteractionOnGUI();
    public bool Interact();
}
