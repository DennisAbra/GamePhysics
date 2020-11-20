using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : MonoBehaviour
{
    public float mass = 1f;
    public HalfWidths halfWidth;
    public float dissipationFraction = 1f;

    private void Start()
    {
        halfWidth = new HalfWidths(transform.localScale.x * .75f, transform.localScale.y * 0.5f, transform.localScale.z * .75f); ;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawCube(transform.position, new Vector3(transform.localScale.x * 1.5f, transform.localScale.y, transform.localScale.z * 1.5f));
    //}

    public struct HalfWidths
    {
        public float x;
        public float y;
        public float z;
        public HalfWidths(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
        

}
