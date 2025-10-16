using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovementGrid : MonoBehaviour
{
    public float moveSpeed = 5f; // velocità di scatto
    public LayerMask obstacleMask; // layer per muri o ostacoli

    private bool isMoving = false;
    private Vector2 input;

    void Update()
    {
        if (!isMoving)
        {
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            // impedisce movimenti diagonali
            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                Vector3 targetPos = transform.position + (Vector3)input;

                if (IsWalkable(targetPos))
                    StartCoroutine(MoveTo(targetPos));
            }
        }

        // interazione con tasto E o click
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    IEnumerator MoveTo(Vector3 target)
    {
        isMoving = true;

        while ((target - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target;
        isMoving = false;
    }

    bool IsWalkable(Vector3 targetPos)
    {
        // se colpisce un muro → blocca
        return !Physics2D.OverlapCircle(targetPos, 0.1f, obstacleMask);
    }

    void TryInteract()
    {
        // Raycast nella direzione in cui il player è rivolto
        Vector2 dir = input != Vector2.zero ? input : Vector2.up;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1f);

        if (hit.collider != null)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
                interactable.Interact();
        }
    }
}
