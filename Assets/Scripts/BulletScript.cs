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
        var colName = other.name;

        if (!colName.Equals("ropeAttached") && !colName.Equals("slash_0(Clone)") && !colName.Contains("Pickup"))
        {
            Destroy(gameObject);
        }
    }
}
