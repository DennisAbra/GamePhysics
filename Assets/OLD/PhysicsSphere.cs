using UnityEngine;

public class PhysicsSphere : MonoBehaviour
{
    Vector3 position = new Vector3(0, 0, 0);
    Vector3 velocity = new Vector3(0, 0, 0);
    Vector3 gravityForce;
    [SerializeField] float mass = 1.0f;
    [SerializeField] bool shouldUseGravity = true;

    Vector3 gravity = Physics.gravity;

    private void Update()
    {
        ApplyForce(new Vector3(0, 0, 0), shouldUseGravity);
    }

    // Force = mass * acceleration
    // a = f / m
    private void ApplyForce(Vector3 Force, bool applyGravity)
    {
        if(applyGravity)
            gravityForce = Force + mass * Physics.gravity;

        Vector3 acc = (applyGravity) ? Force + gravityForce / mass : Force / mass;
        // v1 = v0 + acc * deltaT
        velocity = velocity + acc * Time.deltaTime;

        // p1 = p0 + velocity * deltaT
        position = position + velocity * Time.deltaTime;

        transform.position = position;
    }
}
