using UnityEngine;

public class Door : MonoBehaviour
{
    private bool isOpen = false;
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
        col.enabled = !isOpen;
        sr.color = isOpen ? Color.gray : Color.white; // feedback visivo
    }
}
