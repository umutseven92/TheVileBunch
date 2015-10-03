using UnityEngine;
using System.Collections;

public class explosionScript : MonoBehaviour
{
    public AudioClip explosion;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, 1.8f);
	}

    void Awake()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        audioSource.PlayOneShot(explosion);
    }
}
