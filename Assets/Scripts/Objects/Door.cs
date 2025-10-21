using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    private Collider2D col;
    private SpriteRenderer sr;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetOpen(bool open)
    {
        isOpen = open;
        if (col != null) col.enabled = !isOpen;   // se aperta, disabilita collider per permettere il passaggio
        if (sr != null) sr.color = isOpen ? new Color(1f,1f,1f,0.6f) : Color.white;
        Debug.Log("Door SetOpen: " + isOpen);
    }
}
