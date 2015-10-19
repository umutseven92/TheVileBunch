using UnityEngine;

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
        Debug.Log(name);

        if (!name.Equals("ropeAttached") && !name.Equals("slash_0(Clone)") && !name.Contains("Pickup"))
        {
            Destroy(gameObject);
        }
    }
}
