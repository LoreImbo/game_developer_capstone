using System.Collections;
using UnityEngine;

public class PlayerMovementGrid : MonoBehaviour
{
    public float moveSpeed = 5f;
    public LayerMask obstacleMask;

    private bool isMoving = false;
    private Vector2 lastFacing = Vector2.up; // direzione in cui il player guarda

    void Update()
    {
        if (!isMoving)
        {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (input.x != 0) input.y = 0; // niente diagonali

            if (input != Vector2.zero)
            {
                lastFacing = input.normalized;
                Vector3 targetPos = transform.position + (Vector3)input;

                if (IsWalkable(targetPos))
                    StartCoroutine(MoveTo(targetPos));
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    IEnumerator MoveTo(Vector3 target)
    {
        isMoving = true;
        while ((target - transform.position).sqrMagnitude > 0.0001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
        isMoving = false;
    }

    bool IsWalkable(Vector3 targetPos)
    {
        // uso OverlapCircle per verificare collider su obstacleMask
        return !Physics2D.OverlapCircle(targetPos, 0.1f, obstacleMask);
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