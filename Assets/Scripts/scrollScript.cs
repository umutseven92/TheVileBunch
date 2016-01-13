using UnityEngine;
using System.Collections;

public class scrollScript : MonoBehaviour
{
    public float Speed = 0.5f;
    private Renderer _renderer;

    // Use this for initialization
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        var offset = new Vector2(Time.time * Speed, 0);
        _renderer.material.mainTextureOffset = offset;
    }
}
