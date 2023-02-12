using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;
    public LayerMask layerMask;
    public GameObject keyCodeJLeft;
    public GameObject keyCodeJRight;
    public GameObject keyCodeKLeft;
    public GameObject keyCodeKRight;
    public GameObject keyCodeLLeft;
    public GameObject keyCodeLRight;
    public GameObject keyCodeILeft;
    public GameObject keyCodeIRight;
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
    private bool _keyCodeK;
    private bool _keyCodeL;
    private bool _keyCodeI;

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
        if (!Game.IsGameOver)
        {
            Movement();
            AnimationUpdate();
            IsGameOver();
        }
        else
        {
            _animator.SetInteger("state", (int)State.Idle);
        }
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
                if (!(_keyCodeJ || _keyCodeK || _keyCodeL || _keyCodeI))
                {
                    _spriteRenderer.flipX = false;
                }

            }
            else if (_dirX < 0)
            {
                _state = State.Walk;
                _direction = Direction.Left;
                if (!(_keyCodeJ || _keyCodeK || _keyCodeL || _keyCodeI))
                {
                    _spriteRenderer.flipX = true;
                }
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

    // private void Attack()
    // {
    //     if (_keyCodeJ)
    //     {
    //         DamageCollider(DamageType.KeyCodeJ);
    //     }
    //
    //     if (_keyCodeK)
    //     {
    //         DamageCollider(DamageType.KeyCodeK);
    //     }
    //
    //     if (_keyCodeL)
    //     {
    //         DamageCollider(DamageType.KeyCodeL);
    //     }
    //
    //     if (_keyCodeI)
    //     {
    //         DamageCollider(DamageType.KeyCodeI);
    //     }
    // }

    // private void DamageCollider(DamageType damageType)
    // {
    //     switch (damageType)
    //     {
    //         case DamageType.KeyCodeJ:
    //             OpenCollider(keyCodeJLeft, keyCodeJRight, _direction);
    //             CloseCollider(keyCodeJLeft, keyCodeJRight, _direction == Direction.Left ? Direction.Right : Direction.Left);
    //             break;
    //         case DamageType.KeyCodeK:
    //             OpenCollider(keyCodeKLeft, keyCodeKRight, _direction);
    //             CloseCollider(keyCodeKLeft, keyCodeKRight, _direction == Direction.Left ? Direction.Right : Direction.Left);
    //             break;
    //         case DamageType.KeyCodeL:
    //             OpenCollider(keyCodeLLeft, keyCodeLRight, _direction);
    //             CloseCollider(keyCodeLLeft, keyCodeLRight, _direction == Direction.Left ? Direction.Right : Direction.Left);
    //             break;
    //         case DamageType.KeyCodeI:
    //             OpenCollider(keyCodeILeft, keyCodeIRight, _direction);
    //             CloseCollider(keyCodeILeft, keyCodeIRight, _direction == Direction.Left ? Direction.Right : Direction.Left);
    //             break;
    //     }
    // }

    private void OpenCollider(GameObject damageLeftCollider, GameObject damageRightCollider, Direction direction)
    {
        if (direction == Direction.Left)
        {
            damageLeftCollider.transform.position = transform.position;
            damageLeftCollider.SetActive(true);
        }
        else
        {
            damageRightCollider.transform.position = transform.position;
            damageRightCollider.SetActive(true);
        }
    }

    private void CloseCollider(GameObject damageLeftCollider, GameObject damageRightCollider, Direction direction)
    {
        if (direction == Direction.Left)
        {
            damageLeftCollider.SetActive(false);
            damageLeftCollider.transform.position = new Vector3(100, 100, 0);
        }
        else
        {
            damageRightCollider.SetActive(false);
            damageRightCollider.transform.position = new Vector3(100, 100, 0);
        }
    }

    private void StartAttack(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.KeyCodeJ:
                _keyCodeJ = true;
                OpenCollider(keyCodeJLeft, keyCodeJRight, _direction);
                break;
            case DamageType.KeyCodeK:
                _keyCodeK = true;
                OpenCollider(keyCodeKLeft, keyCodeKRight, _direction);
                break;
            case DamageType.KeyCodeL:
                _keyCodeL = true;
                OpenCollider(keyCodeLLeft, keyCodeLRight, _direction);
                break;
            case DamageType.KeyCodeI:
                _keyCodeI = true;
                OpenCollider(keyCodeILeft, keyCodeIRight, _direction);
                break;
        }
    }

    private void StopAttack()
    {
        _keyCodeJ = false;
        _keyCodeK = false;
        _keyCodeL = false;
        _keyCodeI = false;
        CloseCollider(keyCodeJLeft, keyCodeJRight, Direction.Left);
        CloseCollider(keyCodeJLeft, keyCodeJRight, Direction.Right);
        CloseCollider(keyCodeKLeft, keyCodeKRight, Direction.Left);
        CloseCollider(keyCodeKLeft, keyCodeKRight, Direction.Right);
        CloseCollider(keyCodeLLeft, keyCodeLRight, Direction.Left);
        CloseCollider(keyCodeLLeft, keyCodeLRight, Direction.Right);
        CloseCollider(keyCodeILeft, keyCodeIRight, Direction.Left);
        CloseCollider(keyCodeILeft, keyCodeIRight, Direction.Right);
    }

    private void EndHurt()
    {
        _hurt = false;
    }

    private void IsGameOver()
    {
        var blood = _player1 ? Game.Player1Blood : Game.Player2Blood;
        if (blood > 0) return;
        _state = State.Dead;
        _animator.SetInteger("state", (int)_state);
        Game.IsGameOver = true;
    }
}