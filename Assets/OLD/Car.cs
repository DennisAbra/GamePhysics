using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : PhysicalObject
{
    [SerializeField] float thrust = 100.0f;
    [SerializeField] float steering = 120f;
    [SerializeField] float steeringForce = 120f;

    protected override void FixedUpdateMethod()
    {
        base.FixedUpdateMethod();
        Velocity = transform.forward * ForwardInput() * thrust;

        //ApplyForce(transform.forward * ForwardInput() * thrust);
        ApplyForce(transform.right * SteeringInput() * steeringForce * Velocity.magnitude);
    }

    protected override void UpdateMethod()
    {
        transform.Rotate(transform.up * SteeringInput() * steering * Time.deltaTime);
    }

    float ForwardInput()
    {
        return Input.GetAxis("Vertical");
    }

    float SteeringInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }



}
