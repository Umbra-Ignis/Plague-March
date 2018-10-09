using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Laser Sight for the player's aim
/// </summary>
public class Trajectory_Simulation : MonoBehaviour
{
    // Reference to the LineRenderer we will use to display the simulated path
    public LineRenderer sightLine;

    //Fire Strength
    public float fireStrength = 500;

    // Number of segments to calculate - more gives a smoother line
    public int segmentCount = 20;

    // Length scale for each segment
    public float segmentScale = 1;

    //Segment velocity
    public Vector3 segVelocity;

    private Vector3 velBeforeGrav;

    private void Start(){}

    void FixedUpdate()
    {
        simulatePath();
    }

    /// <summary>
    /// Simulate the path of a launched ball.
    /// Slight errors are inherent in the numerical method used.
    /// </summary>
    /// 

    void simulatePath()
    {
        Vector3[] segments = new Vector3[segmentCount];

        // The first line point is wherever the player's cannon, etc is
        segments[0] = transform.position;

        // The initial velocity
        segVelocity = (Camera.main.transform.forward + Camera.main.transform.up) * fireStrength * Time.deltaTime;
        velBeforeGrav = segVelocity;
        //Debug.Log(Camera.main.transform.forward + Camera.main.transform.up * Time.deltaTime);
        //Debug.DrawRay(this.transform.position, Camera.main.transform.forward + Camera.main.transform.up, Color.blue);
        for (int i = 1; i < segmentCount; i++)
        {
            // Time it takes to traverse one segment of length segScale (careful if velocity is zero)
            float segTime = (segVelocity.sqrMagnitude != 0) ? segmentScale / segVelocity.magnitude : 0;

            // Add velocity from gravity for this segment's timestep
            segVelocity = segVelocity + Physics.gravity * segTime;

            segments[i] = segments[i - 1] + segVelocity * segTime;
        }

        // At the end, apply our simulations to the LineRenderer



        sightLine.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++)
            sightLine.SetPosition(i, segments[i]);
    }

    public Vector3 getVelocity()
    {
        return velBeforeGrav;
    }
}
