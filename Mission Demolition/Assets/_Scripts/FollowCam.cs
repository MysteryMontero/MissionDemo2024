using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // The static point of interest

    [Header("Inscribed")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero; // Vector2.zero is [0,0]

    [Header("Dynamic")]
    public float camZ; // The desired Z pos of the camera

    private void Awake()
    {
        camZ = this.transform.position.z;
    }

    void FixedUpdate()
    {
        Vector3 destination = Vector3.zero;

        if (POI != null)
        {
            // If the POI has a Rigidbody, check to see if it is sleeping
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if ((poiRigid != null) && poiRigid.IsSleeping())
            {
                POI = null;
            }
        }

        if (POI != null)
        {
            destination = POI.transform.position;
        }

        // Limit the minimum values of destination.x & destination.y
        destination.x = Mathf.Max(minXY.x, destination.x);

        // Interpolate from the current Camera position toward destination
        destination = Vector3.Lerp(transform.position, destination, easing);

        // Force destination.z to be camZ to keep the camera far enough away
        destination.z = camZ;

        // Set the camera to the destination
        transform.position = destination;

        // Set the orthographicSize of the Camera to keep Ground in view
        Camera.main.orthographicSize = destination.y + 10;
    }
}