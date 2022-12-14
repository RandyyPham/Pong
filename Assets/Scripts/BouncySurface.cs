using UnityEngine;

public class BouncySurface : MonoBehaviour
{
    [SerializeField] private float bounceStrength;

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        // gets the ball object when the gameObject collides with anything
        Ball ball = collision2D.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            // get the normal vector of the collision
            Vector2 normal = collision2D.GetContact(0).normal;
            // speeds up the ball along the normal vector
            ball.GetRigidbody2D().AddForce(-normal * bounceStrength);
        }
    }
}
