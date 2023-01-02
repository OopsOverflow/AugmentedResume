using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Vuforia;
public class ImageChanger : MonoBehaviour
{
    public Sprite[] images;
    public VideoClip[] videos;
    public int currentImage = 0;
    public VirtualButtonBehaviour virtualButton;
    public GameObject imageObject;

    // 
    OnCollision skillsCollisionScript;

    void Start()
    {
        // making the collision script of skills card accessible
        skillsCollisionScript = GameObject.FindGameObjectWithTag("skills_target").GetComponent<OnCollision>();

        virtualButton.RegisterOnButtonPressed(OnButtonPressed);
        virtualButton.RegisterOnButtonReleased(OnButtonReleased);

        // use the sprite renderer of the game object to display the sprite
        SpriteRenderer imageComponent = imageObject.GetComponent<SpriteRenderer>();
        imageComponent.sprite = images[currentImage];
        skillsCollisionScript.SetSkills(videos[currentImage]);

    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("Button pressed");
        SpriteRenderer imageComponent = imageObject.GetComponent<SpriteRenderer>();
        currentImage = (currentImage + 1) % images.Length;
        imageComponent.sprite = images[currentImage];
        skillsCollisionScript.SetSkills(videos[currentImage]);
        
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        // Optional: Add code here to handle the button release event
    }
}

