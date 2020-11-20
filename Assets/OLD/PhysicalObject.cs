using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalObject : MonoBehaviour
{
    [SerializeField] public float mass = 1;
    [SerializeField] bool useGravity = false;
    [SerializeField] public bool useVerlet = true;

    [SerializeField] Vector3 velocity = Vector3.zero;
    [SerializeField] public float maxVelocity = 40f;
    [SerializeField] bool constrainMoveX = false;
    [SerializeField] bool constrainMoveY = false;
    [SerializeField] bool constrainMoveZ = false;

    public Vector3 Velocity
    {
        get => velocity;
        set => velocity = new Vector3(constrainMoveX ? 0f : value.x,
            constrainMoveY ? 0f : value.y,
            constrainMoveZ ? 0f : value.z);
    }


    private void FixedUpdate()
    {
        FixedUpdateMethod();
    }

    private void Update()
    {
        UpdateMethod();
    }

    public void ApplyForce(Vector3 force)
    {
        Vector3 totalForce = useGravity ? force + mass * Physics.gravity : force;
        Vector3 acc = totalForce / mass;

        Integrate(acc, useVerlet);
    }

    // Verlet is closest to reality
    void Integrate(Vector3 acc, bool isVerlet = false)
    {
        if (!isVerlet) // use EUler
        {
            Velocity = Velocity + acc * Time.fixedDeltaTime;
            transform.position = transform.position + Velocity * Time.fixedDeltaTime;
        }
        else // use Verlet integration
        {
            transform.position += Velocity * Time.fixedDeltaTime + acc * (Time.fixedDeltaTime * Time.fixedDeltaTime * 0.5f);
            Velocity += acc * Time.fixedDeltaTime * 0.5f;
        }
    }

    protected void LimitVelocity()
    {
        Velocity = Velocity.normalized * Mathf.Min(Velocity.magnitude, maxVelocity);
    }

    protected virtual void UpdateMethod()
    { }

    protected virtual void FixedUpdateMethod()
    {
        ApplyForce(new Vector3(0f, 0f, 0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterMethod(other);
    }

    protected virtual void OnTriggerEnterMethod(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitMethod(other);
    }

    protected virtual void OnTriggerExitMethod(Collider other)
    {
        
    }
}
