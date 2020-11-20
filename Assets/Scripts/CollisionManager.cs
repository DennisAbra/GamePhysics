using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    Sphere[] spheres;
    Rectangle[] rects;

    private void Awake()
    {
        spheres = FindObjectsOfType<Sphere>();
        rects = FindObjectsOfType<Rectangle>();
    }

    private void OnEnable()
    {
        Menu.ResetGame.AddListener(ResetSpheres);
    }

    private void OnDisable()
    {
        Menu.ResetGame.RemoveListener(ResetSpheres);
    }

    private void ResetSpheres()
    {
        foreach(Sphere s in spheres)
        {
            s.Velocity = Vector3.zero;
            s.transform.position = s.startPos;
            s.gameObject.SetActive(true);
        }
    }

    private void CorrectSpherePos(Sphere a, Rectangle aa)
    {
        a.transform.position = new Vector3(a.transform.position.x, aa.transform.position.y + aa.halfWidth.y + a.radius, a.transform.position.z);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < spheres.Length; i++)
        {
            Sphere a = spheres[i];
            for (int j = 1; j < spheres.Length; j++)
            {
                Sphere b = spheres[j];
                if (a == b) continue;
                if (WillSpheresCollide(a, b))
                {
                    CheckSphereCollision(a, b);
                }
            }
        }

        for (int i = 0; i < rects.Length; i++)
        {
            Rectangle r = rects[i];
            for (int j = 0; j < spheres.Length; j++)
            {
                Sphere s = spheres[j];
                if (CheckSphereAABB(s, r))
                {
                    CorrectSpherePos(s, r);

                    Vector3 vel = (s.Velocity - Vector3.zero - 2f * Vector3.Dot(s.Velocity, r.transform.up) * r.transform.up) * (1f - GlobalPhysicsPara.chocEnergyDissipation/r.dissipationFraction);
                    s.Velocity = new Vector3(vel.x, 0, vel.z); /*+ 2f * Vector3.zero*/;
                }
            }
        }
    }

    private Vector3 ReflectSphere(Vector3 v, Rectangle rect, float energyDissipation = 0f)
    {
        Vector3 r = (v - 2f * Vector3.Dot(v, rect.transform.position.normalized) * rect.transform.position.normalized) * (1f - energyDissipation);
        return r;
    }

    public void CheckSphereCollision(Sphere a, Sphere b)
    {
        if (a.IsLastColliderEqual(b) || b.IsLastColliderEqual(a))
            return;

        a.SetLastCollider(b);
        b.SetLastCollider(a);

        Vector3 x = a.transform.position - b.transform.position;
        float distance = x.magnitude;
        float radiusSum = a.radius + b.radius;

        if (distance < radiusSum)
        {
            //Collision response
            Vector3 aToB = (b.transform.position - a.transform.position).normalized;
            Vector3 pushFromA = radiusSum * aToB;
            b.transform.position = a.transform.position + 1f * pushFromA;
            SphereCollisionResponse(x, a, b);
        }
    }

    private void SphereCollisionResponse(Vector3 x, Sphere a, Sphere b)
    {
        float massSum = a.mass + b.mass;
        float doubleMassA = a.mass * 2f;
        float doubleMassB = b.mass * 2f;

        float doubleAOverSum = doubleMassA / massSum;
        float doubleBOverSum = doubleMassB / massSum;

        Vector3 vAMinusVB = a.Velocity - b.Velocity;
        Vector3 vBMinusVA = -vAMinusVB;

        Vector3 posAMinusPosB = a.transform.position - b.transform.position;
        Vector3 posBMinusPosA = -posAMinusPosB;

        float posesSquare = Vector3.Dot(posAMinusPosB, posAMinusPosB);

        float dotA = Vector3.Dot(vAMinusVB, posAMinusPosB);
        float dotB = Vector3.Dot(vBMinusVA, posBMinusPosA);

        Vector3 newVelA = (doubleBOverSum * (dotA / posesSquare) * posAMinusPosB) * (1f - GlobalPhysicsPara.chocEnergyDissipation);
        Vector3 newVelB = (doubleAOverSum * (dotB / posesSquare) * posBMinusPosA) * (1f - GlobalPhysicsPara.chocEnergyDissipation);

        a.Velocity -= new Vector3(newVelA.x, 0, newVelA.z);
        b.Velocity -= new Vector3(newVelB.x, 0, newVelB.z);
    }

    public bool CheckAABBCollision(Rectangle a, Rectangle b)
    {
        if (Mathf.Abs(a.transform.position.x - b.transform.position.x) > (a.halfWidth.x + b.halfWidth.x)) return false;
        if (Mathf.Abs(a.transform.position.y - b.transform.position.y) > (a.halfWidth.y + b.halfWidth.y)) return false;
        if (Mathf.Abs(a.transform.position.z - b.transform.position.z) > (a.halfWidth.z + b.halfWidth.z)) return false;
        Debug.Log("AABB COLLISION");
        return true;
    }

    public bool CheckSphereAABB(Sphere s, Rectangle r)
    {
        float squaredDistance = SquaredDistPointAABB(s.transform.position, r);
        return squaredDistance <= (s.radius * s.radius);
    }

    float SquaredDistPointAABB(Vector3 p, Rectangle r)
    {
        float sq = 0f;
        sq += Check(p.x, r.transform.position.x - r.halfWidth.x, r.transform.position.x + r.halfWidth.x);
        sq += Check(p.y, r.transform.position.y - r.halfWidth.y, r.transform.position.y + r.halfWidth.y);
        sq += Check(p.z, r.transform.position.z - r.halfWidth.z, r.transform.position.z + r.halfWidth.z);
        return sq;
    }

    public bool WillSpheresCollide(Sphere a, Sphere b)
    {
        Vector3 s = a.transform.position - b.transform.position;
        Vector3 v = a.Velocity - b.Velocity;
        float radiusSum = a.radius + b.radius;
        float dist = Vector3.Dot(s, s) - radiusSum * radiusSum;

        if (dist < 0f)
        {
            return true;
        }

        float vDotV = Vector3.Dot(v, v);
        float vDotS = Vector3.Dot(v, s);
        if (vDotS >= 0f)
        {
            return false; // spheres not moving towards eachother
        }

        if (vDotS * vDotS - vDotV * dist < 0f)
        {
            return false;
        }

        return true;
    }

    private static float Check(float point, float min, float max)
    {
        float returnV = 0f;

        if (point < min)
        {
            float val = (min - point);
            returnV += val * val;
        }

        if (point > max)
        {
            float val = (point - max);
            returnV += val * val;
        }

        return returnV;
    }
}
