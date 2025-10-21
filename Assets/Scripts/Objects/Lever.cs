using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public Door connectedDoor;
    public bool isOn = false;

    public void Interact()
    {
        isOn = !isOn;
        Debug.Log("Leva azionata: " + (isOn ? "ON" : "OFF"));

        if (connectedDoor != null)
        {
            connectedDoor.SetOpen(isOn);
        }
    }
    public void ResetLever()
    {
        isOn = false;
        if (connectedDoor != null)
            connectedDoor.SetOpen(false);
    }
}
