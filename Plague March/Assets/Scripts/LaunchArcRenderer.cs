using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class LaunchArcRenderer : MonoBehaviour {

    private float velocity;
    public float angle;
    public int resolution = 10;

    //Force of Gravity on the y axis
    float g;
    float radianAngle;

    public GameObject rThrower;
    private RockThrower_Adrian rThrow;
    private LineRenderer lr;

    // Use this for initialization
    void Start ()
    {
        lr = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics.gravity.y);
        rThrow = rThrower.GetComponent<RockThrower_Adrian>();
    }

    void Update()
    {
        angle = rThrow.GetAngle();
        velocity = rThrow.GetVelocity();

        RenderArc();
    }

    //populating the LineRender With Settings
    void RenderArc()
    {
        lr.positionCount = resolution;
        lr.SetPositions(CalculateArcArray());
    }

    //Create an array of vector3 positions for arc
    private Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];
        radianAngle = Mathf.Deg2Rad * angle;

        // Equation https://en.wikipedia.org/wiki/Projectile_motion
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * angle)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }
        return arcArray;
    }

    //Calculate hight and distance of each vertex
    private Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;

        // Equation https://en.wikipedia.org/wiki/Projectile_motion
        float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }



}
