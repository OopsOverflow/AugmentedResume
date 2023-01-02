using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contactInfo : MonoBehaviour
{
    public GameObject imageObjectFrom;

    private string currentSkills;
    private bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        // floatingText.gameObject.SetActive(false);
    }

    void OnTriggerEnter (Collider otherObj)
    {

        if ( otherObj.name == "aboutme_ImageTarget" && hasCollided == false){

            Debug.Log("Collision entered " + otherObj.name);
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
