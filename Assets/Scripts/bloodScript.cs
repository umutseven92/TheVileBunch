using UnityEngine;
using System.Collections;

public class bloodScript : MonoBehaviour
{
    public AudioClip splat;

	// Use this for initialization
	void Start () {
	    Destroy(gameObject, 3);
	}

    void Awake()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(splat);
    }

	// Update is called once per frame
	void Update () {
	
	}
}
