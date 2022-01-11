using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    private NoiseManager noiseManager;
    

    [SerializeField] private bool makeNoise = false;
    [SerializeField] private float noiseVolume;

    [SerializeField] private float soundDuration; // Sound duration in seconds
    private float soundCountdown;

    [SerializeField] private ObstaclesUI obstaclesUI;
    // Start is called before the first frame update
    void Start()
    {
        noiseManager = NoiseManager.Instance;
        noiseManager.AddObjectToNoiseList(this.gameObject);
        soundCountdown = soundDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeNoise)
        {
            soundCountdown -= Time.deltaTime;
            print($"Sound: {soundCountdown}");
            obstaclesUI.ChangeCountdownUITextVisibility(true);
            obstaclesUI.SetCountdownValueOnTextField(soundCountdown);
            if (soundCountdown <= 0)
            {
                print("Sound off");
                makeNoise = false;
                soundCountdown = soundDuration;
                obstaclesUI.ChangeCountdownUITextVisibility(false);
            }
        }
    }

    public bool CheckIfIsMakingNoise()
    {
        return makeNoise;
    }

    public float GetNoiseVolume()
    {
        return noiseVolume;
    }

    public void ActivateNoise()
    {
        makeNoise = true;
    }
}
