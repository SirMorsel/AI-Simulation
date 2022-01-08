using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour
{
    private NoiseManager noiseManager;

    [SerializeField] private bool makeNoise = false;
    [SerializeField] private float noiseVolume;
    // Start is called before the first frame update
    void Start()
    {
        noiseManager = NoiseManager.Instance;
        noiseManager.AddObjectToNoiseList(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckIfIsMakingNoise()
    {
        return makeNoise;
    }

    public float GetNoiseVolume()
    {
        return noiseVolume;
    }
}
