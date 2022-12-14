using UnityEngine;

public class PlayerPaddle : Paddle
{
    private Vector2 direction;
    private Color playerColor;

    private void Start()
    {
        if (ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("Player Color"), out playerColor))
        {
            _spriteRenderer.color = playerColor;
        }

        pongs = PlayerPrefs.GetInt("Pongs");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        // checks if we are moving
        if (direction.sqrMagnitude != 0)
        {
            _rigidbody2D.AddForce(direction * this._speed);
        }
    }
}