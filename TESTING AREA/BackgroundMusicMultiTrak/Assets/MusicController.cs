using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicController : MonoBehaviour {
    public AudioSource[] sources;
    private bool playing = false;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown("space"))
        {
            if(playing)
            {
                foreach (AudioSource a in sources)
                {
                    a.Stop();
                }
                playing = false;
            }
            else
            {
                foreach (AudioSource a in sources)
                {
                    a.Play();
                }
                playing = true;
            }
        }
	}
}
