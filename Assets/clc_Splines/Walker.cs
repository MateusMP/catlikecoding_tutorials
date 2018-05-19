using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Walker : MonoBehaviour
{
    public BezierSpline spline;

    public float duration;

    private float progress;

    private void Update()
    {
        progress += Time.deltaTime / duration;
        if (progress > 1f)
        {
            progress = 0;
        }
        Vector3 position = spline.GetPoint(progress);
        transform.localPosition = position;
        transform.LookAt(position + spline.GetDirection(progress));
    }
}