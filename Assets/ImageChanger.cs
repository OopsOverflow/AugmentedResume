using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class ImageChanger : MonoBehaviour
{
    public Sprite[] images;
    public int currentImage = 0;
    public VirtualButtonBehaviour virtualButton;
    public GameObject imageObject;

    void Start()
    {
        virtualButton.RegisterOnButtonPressed(OnButtonPressed);
        virtualButton.RegisterOnButtonReleased(OnButtonReleased);

        // use the sprite renderer of the game object to display the sprite
        SpriteRenderer imageComponent = imageObject.GetComponent<SpriteRenderer>();
        imageComponent.sprite = images[currentImage];

    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        SpriteRenderer imageComponent = imageObject.GetComponent<SpriteRenderer>();
        currentImage = (currentImage + 1) % images.Length;
        imageComponent.sprite = images[currentImage];
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        // Optional: Add code here to handle the button release event
    }
}

