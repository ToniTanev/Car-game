using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPath : MonoBehaviour
{
    public Color pathColor;
    public float elevation = 0f;
    private void OnDrawGizmos()
    {
        Gizmos.color = pathColor;

        Transform[] parentAndChildren = GetComponentsInChildren<Transform>();
        List<Transform> children = new List<Transform>();

        for(int i = 1; i < parentAndChildren.Length; i++)
        {
            children.Add(parentAndChildren[i]);
        }

        for (int i = 0; i < children.Count; i++)
        {
            Vector3 from = children[i].position;
            from.y = elevation;
            Vector3 to = children[(i + 1) % children.Count].position;
            to.y = elevation;

            Gizmos.DrawLine(from, to);
            Gizmos.DrawWireSphere(from, 5f);
        }
    }
}
