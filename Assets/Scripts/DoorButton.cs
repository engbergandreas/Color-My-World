using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class DoorButton : MonoBehaviour
{

    public float timeUntilClosed = 5;

    public GameObject door;

    public DrawableObject button;

    private AudioSource audioSource;
    public AudioMixer audioMixer;

    public Transform gateToOpen;
    private bool open = false;
    private float timer = 0;
    private BoxCollider doorcollider;
    private void Start()
    {
        doorcollider = door.GetComponent<BoxCollider>();
        button._event.AddListener(OnFullyColoredButton);
        audioSource = GetComponent<AudioSource>();
    }
    public void OnFullyColoredButton()
    {
        if (open)
            return;

        OpenDoor();
   
    }

    private void OpenDoor()
    {
        WalkThrough(true);
        timer = timeUntilClosed;
        audioSource.Play();
        gateToOpen.eulerAngles = new Vector3(0, -40, 0);
    }

    private void Update()
    {
        if(open)
        {
            timer -= Time.deltaTime;
            float pitchMixerRatio = 1.0f - ((timer - (timeUntilClosed / 3.0f)) / timeUntilClosed);
            pitchMixerRatio = Mathf.Clamp(pitchMixerRatio, 0.4f, 1.1f);
            audioSource.pitch = pitchMixerRatio;
            audioMixer.SetFloat("MixerPitch", 1.0f / pitchMixerRatio);
            

            if (timer <= 0)
            {
                CloseDoor();
            }
        }
    }

    public void CloseDoor()
    {
        WalkThrough(false);
        button.ResetToOriginal();
        audioSource.Stop();
        gateToOpen.eulerAngles = new Vector3(0, 0, 0);
    }


    public void WalkThrough(bool status)
    {
        doorcollider.isTrigger = status;
        open = status;
    }


}
