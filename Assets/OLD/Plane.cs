using UnityEngine;

public class Plane : MonoBehaviour
{
    Vector3 position;
    Vector3 normal;
    Vector3 startPos = Vector3.zero;
    Vector3 result;

    private void Start()
    {
        position = transform.position;
        normal = Vector3.up;
        
    }

    private void Update()
    {
        startPos = FindObjectOfType<NewSphere>().transform.position;
        Projection(startPos);
    }


    Vector3 Projection(Vector3 c)
    {
        Vector3 cp = c - position;
        Debug.DrawLine(c, position);
        float dot = Vector3.Dot(cp, -normal);
        result = c + dot * normal;
        return result;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPos, result);
    }
}
