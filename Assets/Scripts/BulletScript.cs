using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public static string player;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 3);
    }

    public void SetOwner(string _player)
    {
        player = _player;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var colName = other.name;

        if (!colName.StartsWith("ropeAttached") && !colName.StartsWith("slash_0") && !colName.StartsWith("Pickup"))
        {
            Destroy(gameObject);
        }
    }
}
