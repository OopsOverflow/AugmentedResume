using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
public class DistanceChecker : MonoBehaviour
{
    public DataSetTrackableBehaviour  imageTarget1;
    public DataSetTrackableBehaviour  imageTarget2;
    public float maxDistance = 1f; // maximum allowed distance between the image targets
    public GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        cube.SetActive(false);
    }

    void Update()
    {
        // get the transform positions of the image targets
        Vector3 position1 = imageTarget1.transform.position;
        Vector3 position2 = imageTarget2.transform.position;

        // calculate the distance between the image targets
        float distance = Vector3.Distance(position1, position2);

        Debug.Log("Distance: " + distance);

        if (distance <= maxDistance)
        {
            // set the children of the imagetarget to active
            foreach (Transform child in imageTarget1.transform)
            {
                child.gameObject.SetActive(true);
            }

            Debug.Log("Distance: IN RANGE");

            cube.SetActive(false);
        }
        else
        {
            // set the children of the imagetarget to inactive
            foreach (Transform child in imageTarget1.transform)
            {
                child.gameObject.SetActive(false);
            }

            cube.SetActive(false);
        }
    }
}
