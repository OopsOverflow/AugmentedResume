using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    public GameObject imageObject;
    public GameObject floatingText;

    private string currentSkills;
    private bool isColliding = false;

    // Start is called before the first frame update
    void Start()
    {
        floatingText.gameObject.SetActive(false);
    }

    void OnTriggerEnter (Collider otherObj)
    {
        if ( otherObj.name == "projects_ImageTarget"){
            isColliding = true;
            ShowSkills();
        }
    }

    void OnTriggerStay (Collider otherObj)
    {
        //Debug.Log("Collision occuring...");
    }

    void OnTriggerExit (Collider otherObj)
    {
        if ( otherObj.name == "projects_ImageTarget"){
            isColliding = false;
            floatingText.gameObject.SetActive(false);
        }

    }

    void ShowSkills()
    {
        floatingText.GetComponentInChildren<TextMesh>().text = currentSkills;
        floatingText.gameObject.SetActive(true);
    }

    public void SetSkills(string text)
    {
        currentSkills = text;
        if (isColliding)
        {
            ShowSkills();
        }
    }

}
