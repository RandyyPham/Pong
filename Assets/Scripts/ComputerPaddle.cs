using UnityEngine;

public class ComputerPaddle : Paddle
{
    [SerializeField] private Rigidbody2D ball;
    private Color computerColor;

    private void Start()
    {
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("Computer Color"), out computerColor))
        {
            _spriteRenderer.color = computerColor;
        }
    }

    private void FixedUpdate()
    {
        // track the position of the ball and see if the computer moves up or down

        // check if the ball is going towards the computer
        if (ball.velocity.x > 0.0f)
        {
            // if the ball is above the computer, go up
            if (ball.position.y > transform.position.y)
            {
                _rigidbody2D.AddForce(Vector2.up * this._speed);
            }
            else if (ball.position.y < transform.position.y)
            {
                _rigidbody2D.AddForce(Vector2.down * this._speed);
            }
        }
        // if the ball is going away from the computer, the computer can idle in the middle
        else
        {
            if (transform.position.y > 0.0f)
            {
                _rigidbody2D.AddForce(Vector2.down * this._speed);
            }
            else if (transform.position.y < 0.0f)
            {
                _rigidbody2D.AddForce(Vector2.up * this._speed);
            }
        }
    }
}