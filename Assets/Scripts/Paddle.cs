using UnityEngine;

public class Paddle : MonoBehaviour
{
    protected Rigidbody2D _rigidbody2D;
    protected SpriteRenderer _spriteRenderer;
    protected ParticleSystem.MainModule _settings;
    [SerializeField] protected ParticleSystem _particles;
    [SerializeField] protected AudioSource _blip;

    [SerializeField] protected float _speed;
    protected int pongs;

    private void Awake()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _settings = _particles.main;
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        // gets the ball object when the gameObject collides with anything
        Ball ball = collision2D.gameObject.GetComponent<Ball>();

        if (ball != null)
        {
            this._blip.Play();
            _particles.transform.position = collision2D.GetContact(0).point;
            _settings.startColor = _spriteRenderer.color;
            _particles.Play();

            // checks if we hit the playerpaddle
            if (this.gameObject.GetComponent<PlayerPaddle>() != null)
            {
                pongs++;
                PlayerPrefs.SetInt("Pongs", pongs);
            }
        }
    }

    public void ResetPosition()
    {
        _rigidbody2D.position = new Vector2(_rigidbody2D.position.x, 0.0f);
        _rigidbody2D.velocity = Vector2.zero;
    }
}