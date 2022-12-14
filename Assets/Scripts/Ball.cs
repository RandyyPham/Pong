using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    [SerializeField] private float speed;

    private void Awake()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        ResetPosition();
        AddStartingForce();
    }

    /// <summary>AddStartingForce is a method that will add a random starting force to the ball when the game starts</summary>
    public void AddStartingForce()
    {
        // if the random value is less than .5, go negative direction. else, go positive direction
        float x = Random.value < 0.5f ? -1.0f : 1.0f;
        float y = Random.value < 0.5f ? Random.Range(-1.0f, -0.5f) : Random.Range(0.5f, 1.0f);

        Vector2 direction = new Vector2(x, y);

        _rigidbody2D.AddForce(direction * speed);
    }

    ///<summary>Returns the ball's rigidbody</summary>
    public Rigidbody2D GetRigidbody2D()
    {
        return _rigidbody2D;
    }

    public void ResetPosition()
    {
        _rigidbody2D.position = Vector2.zero;
        _rigidbody2D.velocity = Vector2.zero;
    }
}