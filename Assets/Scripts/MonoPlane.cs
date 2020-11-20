using System;
using UnityEngine;

public class MonoPlane : MonoBehaviour
{
    //[SerializeField] Vector3 planeNormal = Vector3.up;
    
    Vector3 Normal => transform.up;
    Vector3 Position => transform.position;

    Sphere[] spheres = new Sphere[0];

    const float staticVelocityLimit = 0.1f;
    const float deltaMoveCoefficient = 0.2f;
    const float correctedPosCoef = 2.5f;

    PhysicalObject PhysicalParent => GetComponentInParent<PhysicalObject>();

    Vector3 ParentVelocity => PhysicalParent == null ? Vector3.zero : PhysicalParent.Velocity;


    private void Start()
    {
        spheres = FindObjectsOfType<Sphere>();
    }

    private void FixedUpdate()
    {
        UpdateSpheres();
    }

    private void UpdateSpheres()
    {
        foreach (Sphere sphere in spheres)
        {
            //if (sphere.onPlane)
            //{
                Choc(sphere, GlobalPhysicsPara.chocEnergyDissipation);
           // }
        }
    }

    Vector3 Projection(Sphere sphere)
    {
        Vector3 sphereToProjection = GetDistance(sphere.transform.position) * Normal;
        Vector3 projection = sphereToProjection + sphere.transform.position;
        return projection;
    }

    float GetDistance(Vector3 pos)
    {
        Vector3 sphereTOPlane = Position - pos;

        float dist = Vector3.Dot(sphereTOPlane, Normal);
        return dist;
    }

    bool IsColliding(Sphere sphere)
    {
        //if (!WillCollide(sphere))
        //    return false;

        //return GetDistance(sphere.transform.position) >= 0.0f || Mathf.Abs(GetDistance(sphere.transform.position)) <= sphere.Radius;
        return TouchingPlane(sphere) && WillCollide(sphere);
    }

    bool WillCollide(Sphere sphere)
    {
        //return Vector3.Dot(sphere.Velocity, Normal) < 0.0f;
        return Vector3.Dot(RelativeVelocity(sphere), Normal) < 0.0f;
    }

    Vector3 CorrectedPosition(Sphere sphere)
    {
        return Projection(sphere) + Normal * sphere.radius;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sphere"></param>
    /// <param name="energyDissipation" the ompact of the energy dissipation on the reflected velocity></param>
    void Choc(Sphere sphere, float energyDissipation = 0f)
    {
        if (!IsColliding(sphere))
            return;

        //Crazy formula
        if (IsSphereStatic(sphere))
        {
            sphere.transform.position = CorrectedPosition(sphere);
            sphere.ApplyForce(-sphere.mass * Physics.gravity);
        }
        else
        {
            InverseRelativeVelocity(sphere, Reflect(RelativeVelocity(sphere), energyDissipation));
        }
    }

    Vector3 Reflect(Vector3 v, float energyDissipation = 0.0f)
    {
        return (v - 2.0f * Vector3.Dot(v, Normal) * Normal) * (1.0f - energyDissipation);
    }

    bool IsSphereStatic(Sphere sphere)
    {
        bool lowVel = RelativeVelocity(sphere).magnitude < staticVelocityLimit;

        return lowVel && TouchingPlane(sphere);

    }

    bool TouchingPlane(Sphere sphere)
    {
        float deltaMove = Mathf.Max(RelativeVelocity(sphere).magnitude * Time.fixedDeltaTime, deltaMoveCoefficient * sphere.radius);
        return (CorrectedPosition(sphere) - sphere.transform.position).magnitude <= correctedPosCoef * deltaMove; 
    }

    Vector3 RelativeVelocity(Sphere other)
    {
        return other.Velocity - ParentVelocity;
    }

    void InverseRelativeVelocity(Sphere other, Vector3 vel)
    {
        other.Velocity = vel + 2f* ParentVelocity;
    }
}
