using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(SplineDecorator))]
public class SplineDecoratorInspector : Editor
{
    private List<Transform> decorated;


    private void Awake()
    {
        decorated = new List<Transform>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SplineDecorator decorator = target as SplineDecorator;

        if (GUILayout.Button("Decorate"))
        {
            decorate(decorator);
        }
    }

    private void decorate(SplineDecorator decorator)
    {
        float frequency = decorator.frequency;
        Transform[] items = decorator.items;

        foreach (Transform t in decorated)
        {
            DestroyImmediate(t.gameObject);
        }
        decorated.Clear();
        
        if (frequency <= 0 || items == null || items.Length == 0)
        {
            return;
        }
        float stepSize = 1f / (frequency * items.Length);
        for (int p = 0, f = 0; f < frequency; f++)
        {
            for (int i = 0; i < items.Length; i++, p++)
            {
                Transform item = Instantiate(items[i]) as Transform;
                Vector3 position = decorator.spline.GetPoint(p * stepSize);
                item.transform.localPosition = position;
                if (decorator.lookForward)
                {
                    item.transform.LookAt(position + decorator.spline.GetDirection(p * stepSize));
                }
                item.transform.parent = decorator.spline.transform;
                Undo.RegisterCreatedObjectUndo(item.gameObject, "Decorate");
                decorated.Add(item);
            }
        }
    }
}
