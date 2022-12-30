using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public GameObject collisionResult;

    // // Start is called before the first frame update
    void Start()
    {
        collisionResult.gameObject.SetActive(false);
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    void OnCollisionEnter (Collision collision)
    {
        Debug.Log("Enter called");
        collisionResult.gameObject.SetActive(true);
    }

    void OnCollisionStay (Collision collision)
    {
        Debug.Log("Collision occuring...");
    }

    void OnCollisionExit (Collision collision)
    {
        Debug.Log("Exit called");
        collisionResult.gameObject.SetActive(false);
    }

}
