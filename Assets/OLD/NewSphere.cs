using System;
using UnityEngine;

public class NewSphere : PhysicalObject
{

    public bool onPlane = false;
    public float Radius => transform.localScale.x * 0.5f;
    protected override void FixedUpdateMethod()
    {
        //foreach(MonoPlane plane in planes)
        //{
        //Vector3 projection = plane.Projection(this);
        //bool isColliding = plane.IsColliding(this);
        //Debug.DrawLine(transform.position, projection, (isColliding) ? Color.red : Color.blue);

        base.FixedUpdateMethod();
    }

    //Assuming the initial velocity is 0
    public float VelocityOnGround(MonoPlane plane)
    {
        return Mathf.Sqrt((2*Physics.gravity.magnitude) * (transform.position.y - plane.transform.position.y));
    }

    public float ErrorVelocityOnGround(MonoPlane plane)
    {
        return Mathf.Abs(Velocity.magnitude - VelocityOnGround(plane));
    }

    protected override void OnTriggerEnterMethod(Collider other)
    {
        UpdatedOnPlaneWhenEnter(other);
    }

    protected override void OnTriggerExitMethod(Collider other)
    {
        UpdatedOnPlaneWhenExit(other);
    }

    private void UpdatedOnPlaneWhenExit(Collider other)
    {
        MonoPlane plane = other.GetComponent<MonoPlane>();
        if (!plane)
            return;
        else
            onPlane = false;
    }

    private void UpdatedOnPlaneWhenEnter(Collider other)
    {
        MonoPlane plane = other.GetComponent<MonoPlane>();
        if (!plane)
            onPlane = false;
        else
            onPlane = true;
    }

}
