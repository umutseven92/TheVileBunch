using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    public int damage = 1;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 5);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        string name = other.name;

        if (!name.Equals("ropeAttached") && !name.Equals("slash_0(Clone)"))
        {
            Destroy(gameObject);
        }
    }
}
