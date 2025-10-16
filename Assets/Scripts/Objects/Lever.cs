using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    public Door connectedDoor;
    private bool isOn = false;

    public void Interact()
    {
        isOn = !isOn;
        Debug.Log("Leva azionata: " + (isOn ? "ON" : "OFF"));

        if (connectedDoor != null)
        {
            connectedDoor.SetOpen(isOn);
        }
    }
}
