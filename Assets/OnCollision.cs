using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class OnCollision : MonoBehaviour
{
    private bool isColliding = false;

    public GameObject videoPlane;
    private VideoPlayer videoPlayer;
    public VideoClip defaultVideo;
    private VideoClip currentVideo;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = videoPlane.GetComponent<VideoPlayer>();
        videoPlayer.clip = defaultVideo;
        videoPlayer.Play();
    }

    void OnTriggerEnter (Collider collision)
    {
        isColliding = true;
        ShowSkills();
    }

    void OnTriggerStay (Collider collision)
    {
        //Debug.Log("Collision occuring...");
    }

    void OnTriggerExit (Collider collision)
    {
        isColliding = false;
        videoPlayer.clip = defaultVideo;
    }

    private void ShowSkills()
    {
        if (isColliding)
        {
            videoPlayer.Play();
        }
    }

    public void SetSkills(VideoClip clip)
    {
        if (isColliding)
        {
            videoPlayer.clip = clip;
            ShowSkills();
        }
    }

}
