using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{

    private bool first = true;
    public int damage = 1;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 5);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (first)
        {
            first = false;

        }
        else
        {
            if (!other.name.Equals("ropeAttached"))
            {
                Destroy(gameObject);
            }
        }
    }
}
