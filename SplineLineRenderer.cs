using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineLineRenderer : MonoBehaviour
{
    public SplineMesh.Spline spline;
    public LineRenderer line;

    public float segmentLength = 1f;

    void Start()
    {
        if(!spline)
        {
            spline = GetComponent<SplineMesh.Spline>();
        }
        if (!line)
        {
            line = GetComponent<LineRenderer>();
        }
        Debug.Assert(spline);
        Debug.Assert(line);

        spline.CurveChanged.AddListener(UpdateLineFromSpline);

        UpdateLineFromSpline();
    }

    void UpdateLineFromSpline()
    {
        Debug.Assert(spline);
        Debug.Assert(line);
        
        int zSegments = Mathf.FloorToInt(spline.Length / segmentLength);

        Vector3[] positions = new Vector3[zSegments+1];
        for(int i = 0; i < zSegments; i++)
        {
            positions[i] = GetPosition(i * segmentLength);
        }

        positions[zSegments] = GetPosition(spline.Length);

        line.positionCount = zSegments + 1;
        line.SetPositions(positions);
    }

    Vector3 GetPosition(float aLength)
    {
        if (line.useWorldSpace)
        {
            return transform.TransformPoint(spline.GetSampleAtDistance(aLength).location);
        }
        else
        {
            return spline.GetSampleAtDistance(aLength).location;
        }
    }
}
