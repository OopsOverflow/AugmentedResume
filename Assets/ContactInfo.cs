using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactInfo : MonoBehaviour
{
    public GameObject imageObjectFrom;

    private bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        // floatingText.gameObject.SetActive(false);
    }

    void OnTriggerEnter (Collider otherObj)
    {


        if ( otherObj.name == "Intro" && hasCollided == false){
            // Debug.Log("Collision entered " + otherObj.name);
            hasCollided = true;
            Instantiate(imageObjectFrom, transform);
        }
    }

    void OnTriggerStay (Collider otherObj)
    {
        //Debug.Log("Collision occuring...");
    }

    void OnTriggerExit (Collider otherObj)
    {

    }
}
