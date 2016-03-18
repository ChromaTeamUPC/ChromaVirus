using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class DebugScript : MonoBehaviour {

    public DebugKeys keys;

    public GameObject mainCam;
    public AudioSource mainMusic;
    public AudioSource noiseFx;

    private Grayscale grayScript;
    private NoiseAndGrain noiseScript;

    void Start()
    {
        grayScript = mainCam.GetComponent<Grayscale>();
        noiseScript = mainCam.GetComponent<NoiseAndGrain>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(keys.toggleNoiseKey))
        {
            noiseScript.enabled = !noiseScript.enabled;
        }

        if (Input.GetKeyDown(keys.toggleGrayScaleKey))
        {
            grayScript.enabled = !grayScript.enabled;
        }

        if (Input.GetKeyDown(keys.toggleMusic))
        {
            mainMusic.mute = !mainMusic.mute;
        }

        if (Input.GetKeyDown(keys.playNoiseFx))
        {
            StartCoroutine("PlayNoiseFx");
        }
    }

    IEnumerator PlayNoiseFx()
    {
        mainMusic.volume = 0.1f;
        noiseFx.Play();
        yield return new WaitForSeconds(3f);
        noiseFx.Stop();
        mainMusic.volume = 1f;
    }
}
