using UnityEngine;
using System.Collections;

public class deadPlayerScript : MonoBehaviour
{
    private float degreesPerSecond = 90f;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * degreesPerSecond * Time.deltaTime);

    }

    void Die(string hit)
    {
        if (hit.Equals("right"))
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-15, 30), ForceMode2D.Impulse);
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(15, 30), ForceMode2D.Impulse);
        }
    }

}
