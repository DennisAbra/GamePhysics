using UnityEngine;

public class Sphere : MonoBehaviour
{
    public float radius;
    //[HideInInspector]
    public Vector3 Velocity;
    [SerializeField] public float maxVelocity;
    [SerializeField] float dragCoef;
    public float mass;
    public bool isPlayer = false;
    public Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }

    private void FixedUpdate()
    {
        LimitVelocity();
        ApplyDynamicDrag();
        ApplyForce(Vector3.zero);
        HandleLastCollision();
    }

    private void LimitVelocity()
    {
        Velocity = Velocity.normalized * Mathf.Min(Velocity.magnitude, maxVelocity);
    }

    private void ApplyDynamicDrag()
    {
        if (Velocity.magnitude > .05f)
        {
            Vector3 dragForce = -GetDragAmount() * Velocity;
            ApplyForce(dragForce);
        }
    }

    private float GetDragAmount()
    {
        return Vector3.Dot(Velocity, Velocity) * dragCoef / 1000f;
    }

    public void ApplyForce(Vector3 force)
    {
        Vector3 totalForce = force + mass * Physics.gravity;
        Vector3 acc = totalForce / mass;
        transform.position += Velocity * Time.fixedDeltaTime + acc * Time.fixedDeltaTime * Time.fixedDeltaTime * 0.5f;

        Velocity += acc * Time.fixedDeltaTime * 0.5f;
    }

    public void OutOfBounds()
    {
        Velocity = Vector3.zero;
        transform.position = startPos;
        gameObject.SetActive(true);
    }

    Sphere lastCollider = null;
    const float lastCollisionDelay = 0.01f;
    float lastCollisionDelayCounter = lastCollisionDelay;

    private void HandleLastCollision()
    {
        if (lastCollider != null)
        {
            lastCollisionDelayCounter -= Time.deltaTime;
            if (lastCollisionDelayCounter <= 0f)
            {
                lastCollisionDelayCounter = lastCollisionDelay;
                lastCollider = null;
            }
        }
    }

    public bool IsLastColliderEqual(Sphere other)
    {
        if (lastCollider == null)
            return false;

        return other == lastCollider;
    }

    public void SetLastCollider(Sphere other)
    {
        lastCollisionDelayCounter = lastCollisionDelay;
        lastCollider = other;
    }
}
