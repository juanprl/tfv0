using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class WorldSpaceVideo : MonoBehaviour
{
    VideoPlayer videoPlayer;
    public VideoClip[] vClips;
    public VideoClip[] vBonusMap;
    int vClipIndex = 0;
    bool x = false;


    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.clip = vClips[0];
        videoPlayer.Play();

      /*  
        if ()
        {
            videoPlayer.clip = vBonusMap[0];
            videoPlayer.Play();
        }
      */
    }

    void Update()
    {
        if(!videoPlayer.isPlaying && x == false)
        {
            Invoke("ChangeBool", 1f);

            SetNextClip();
            print("kj");
        }

     /*   if (Input.GetKeyDown(KeyCode.Space))
        {
            videoPlayer.frame--;
        }*/
    }

    public void SetNextClip()
    {
        vClipIndex++;

        if(vClipIndex >= vClips.Length)
        {
            vClipIndex = vClipIndex % vClips.Length;
        }

        videoPlayer.clip = vClips[vClipIndex];
        videoPlayer.Play();
       
        x = true;
    }

    void ChangeBool()
    {
        x = false;
    }
}
