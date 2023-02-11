using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;
    public LayerMask layerMask;
    public GameObject keyCodeJLeft;
    public GameObject keyCodeJRight;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private GameObject _anotherPlayer;
    private SpriteRenderer _spriteRenderer;
    private State _state;
    private Direction _direction;
    private float _dirX;
    private float _dirY;
    private bool _isGrounded;
    private bool _player1;
    private bool _hurt;
    private bool _keyCodeJ;

    private enum State
    {
        Idle,
        Walk,
        Jump,
        KeyCodeJ,
        KeyCodeK,
        KeyCodeL,
        KeyCodeI,
        Hurt,
        Dead
    }

    public enum DamageType
    {
        KeyCodeJ,
        KeyCodeK,
        KeyCodeL,
        KeyCodeI
    }

    public enum Direction
    {
        Left,
        Right
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("AttackCollider"))
        {
            if (_anotherPlayer.transform.position.x > transform.position.x)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.flipX = true;
            }
            _state = State.Hurt;
            _hurt = true;
        }
    }

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player1 = gameObject.CompareTag("Player");
        _anotherPlayer = _player1 ? GameObject.FindGameObjectWithTag("Player2") : GameObject.FindGameObjectWithTag("Player");
        _direction = Direction.Right;
    }

    private void Update()
    {
        Movement();
        AnimationUpdate();
        Attack();
    }

    private void IsGrounded()
    {
        _isGrounded = Physics2D.BoxCast(transform.position, new Vector2(.5f, .5f), 0f, new Vector2(0, -1), 1.75f, layerMask);
    }

    private void Movement()
    {
        IsGrounded();
        _dirX = _player1 ? Input.GetAxisRaw("Horizontal") * moveSpeed : Input.GetAxisRaw("Horizontal2") * moveSpeed;
        _dirY = _rigidbody2D.velocity.y;
        if ((Input.GetKeyDown(KeyCode.W) && _player1 || Input.GetKeyDown(KeyCode.UpArrow) && !_player1) && _isGrounded)
        {
            _dirY = jumpSpeed;
        }

        if (_hurt)
        {
            _dirX = 0;
            _dirY = 0;
        }

        _rigidbody2D.velocity = new Vector2(_dirX, _dirY);
    }

    private void AnimationUpdate()
    {
        if (!_hurt)
        {
            _state = State.Idle;
            if (_dirX > 0)
            {
                _state = State.Walk;
                _direction = Direction.Right;
                _spriteRenderer.flipX = false;
            }
            else if (_dirX < 0)
            {
                _state = State.Walk;
                _direction = Direction.Left;
                _spriteRenderer.flipX = true;
            }

            if (!_isGrounded)
            {
                _state = State.Jump;
            }

            if (Input.GetKeyDown(KeyCode.J) && _player1 || Input.GetKeyDown(KeyCode.Keypad4) && !_player1)
            {
                _state = State.KeyCodeJ;
            }

            if (Input.GetKeyDown(KeyCode.K) && _player1 || Input.GetKeyDown(KeyCode.Keypad5) && !_player1)
            {
                _state = State.KeyCodeK;
            }

            if (Input.GetKeyDown(KeyCode.L) && _player1 || Input.GetKeyDown(KeyCode.Keypad6) && !_player1)
            {
                _state = State.KeyCodeL;
            }

            if (Input.GetKeyDown(KeyCode.I) && _player1 || Input.GetKeyDown(KeyCode.Keypad8) && !_player1)
            {
                _state = State.KeyCodeI;
            }
        }

        _animator.SetInteger("state", (int)_state);
    }

    private void Attack()
    {
        if (_keyCodeJ)
        {
            if (_direction == Direction.Left)
            {
                keyCodeJLeft.SetActive(true);
                keyCodeJRight.SetActive(false);
            }
            else
            {
                keyCodeJLeft.SetActive(false);
                keyCodeJRight.SetActive(true);
            }
        }
    }

    private void StartAttack(DamageType damageType)
    {
        if (damageType == DamageType.KeyCodeJ)
        {
            _keyCodeJ = true;
        }
    }

    private void StopAttack()
    {
        _keyCodeJ = false;
        keyCodeJLeft.SetActive(false);
        keyCodeJRight.SetActive(false);
    }

    private void EndHurt()
    {
        _hurt = false;
    }
}