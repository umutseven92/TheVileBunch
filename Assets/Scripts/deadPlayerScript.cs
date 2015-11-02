using UnityEngine;

public class deadPlayerScript : MonoBehaviour
{
    public float degreesPerSecond = 90f; // How much does the dead body turn in air
    public int pushX = 15; // How far in X plane the body flies
    public int pushY = 30; // How far in Y plane the body flies

    void Start()
    {
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * degreesPerSecond * Time.deltaTime);
    }

    void SetColor(Color color)
    {
        this.GetComponent<SpriteRenderer>().color = color;
    }

    void Die(string hit)
    {
        GetComponent<Rigidbody2D>()
            .AddForce(hit.Equals("right") ? new Vector2(-pushX, pushY) : new Vector2(pushX, pushY), ForceMode2D.Impulse);
    }
}
