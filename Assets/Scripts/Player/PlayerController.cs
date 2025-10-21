using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5;
    public Vector2 Direction { get; private set; }
    private Rigidbody2D _rb;
    private float _horizontal;
    private float _vertical;
    private Vector2 lastFacing = Vector2.up; // direzione in cui il player guarda


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void FixedUpdate()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        Direction = new Vector2(_horizontal, _vertical).normalized;

        if (Direction != Vector2.zero)
        {
            lastFacing = Direction;
            _rb.MovePosition(_rb.position + Direction * (_speed * Time.fixedDeltaTime));
        }
    }

    void TryInteract()
    {
        // punto di origine leggermente davanti al player per evitare di colpire se stesso
        Vector2 origin = (Vector2)transform.position + lastFacing * 0.5f;
        float radius = 0.4f;

        // prova OverlapCircleAll davanti al player (più robusto del Raycast singolo)
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, radius);
        foreach (var c in hits)
        {
            if (c == null) continue;
            // ignora il collider del player
            if (c.gameObject == this.gameObject) continue;

            var interactable = c.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                return;
            }
        }

        Debug.Log("Nessun oggetto con cui interagire davanti a te.");
    }

    // (utile per debug) disegna l'area di interazione nella Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 origin = transform.position + (Vector3)lastFacing * 0.5f;
        Gizmos.DrawWireSphere(origin, 0.4f);
    }

}
