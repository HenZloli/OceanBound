using UnityEngine;

public class PlayerRestriction : MonoBehaviour
{
    public LayerMask walkableLayer;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & walkableLayer) == 0)
        {
            Debug.Log("khong dc di vao layer nay" + LayerMask.LayerToName(collision.gameObject.layer));
            Vector2 pushBack = -collision.relativeVelocity.normalized;
            transform.position += (Vector3)pushBack * 0.1f;
        }
    }
}
