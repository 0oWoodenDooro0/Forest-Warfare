using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public float jumpSpeed;
    public float knockBack;
    public LayerMask layerMask;
    public GameObject keyCodeJLeft;
    public GameObject keyCodeJRight;
    public GameObject keyCodeKLeft;
    public GameObject keyCodeKRight;
    public GameObject keyCodeLLeft;
    public GameObject keyCodeLRight;
    public GameObject keyCodeILeft;
    public GameObject keyCodeIRight;
    public GameObject attackObject;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;
    private GameObject _anotherPlayer;
    private SpriteRenderer _spriteRenderer;
    private State _movementState;
    private DamageType _attackState;
    private Direction _attackDirection;
    private Direction _direction;
    private int _hurtState;
    private int _currentLayer;
    private int _movementLayer;
    private int _attackLayer;
    private int _hurtLayer;
    private float _dirX;
    private float _dirY;
    private bool _isGrounded;
    private bool _player1;
    private bool _hurt;
    private bool _attack;
    private bool _knockBack;

    private enum State
    {
        Idle,
        Walk,
        Jump
    }

    public enum DamageType
    {
        None,
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
        if (other.gameObject.CompareTag("AttackCollider") && (_player1 && !Game.Player1God || !_player1 && !Game.Player2God))
        {
            if (_anotherPlayer.transform.position.x > transform.position.x)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                _spriteRenderer.flipX = true;
            }

            _attackState = DamageType.None;
            _hurt = true;
            _hurtState = 1;
            _knockBack = true;
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
        _movementLayer = _animator.GetLayerIndex("Movement");
        _attackLayer = _animator.GetLayerIndex("Attack");
        _hurtLayer = _animator.GetLayerIndex("Hurt");
        ChangeAnimationLayer(_movementLayer);
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
            _animator.SetInteger("attack", (int)DamageType.None);
            ChangeAnimationLayer(_movementLayer);
            _animator.SetInteger("movement", (int)State.Idle);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Choose Charater");
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

        if (_hurt && !_knockBack)
        {
            return;
        }

        if (_knockBack)
        {
            _rigidbody2D.AddForce(-(_anotherPlayer.transform.position - transform.position).normalized * knockBack, ForceMode2D.Impulse);
            _knockBack = false;
            return;
        }

        _rigidbody2D.velocity = new Vector2(_dirX, _dirY);
    }

    private void AnimationUpdate()
    {
        if (_hurt)
        {
            ChangeAnimationLayer(_hurtLayer);
            _animator.SetInteger("hurt", _hurtState);
            return;
        }

        if (_attack)
        {
            ChangeAnimationLayer(_attackLayer);
        }
        else
        {
            ChangeAnimationLayer(_movementLayer);
        }

        _movementState = State.Idle;
        if (_dirX > 0)
        {
            _movementState = State.Walk;
            _direction = Direction.Right;
            if (!_attack)
            {
                _spriteRenderer.flipX = false;
            }
        }
        else if (_dirX < 0)
        {
            _movementState = State.Walk;
            _direction = Direction.Left;
            if (!_attack)
            {
                _spriteRenderer.flipX = true;
            }
        }

        if (!_isGrounded)
        {
            _movementState = State.Jump;
        }

        if (!_attack)
        {
            if (Input.GetKeyDown(KeyCode.J) && _player1 || Input.GetKeyDown(KeyCode.Keypad4) && !_player1)
            {
                _attackState = DamageType.KeyCodeJ;
                _attack = true;
                _attackDirection = _direction;
            }

            if (Input.GetKeyDown(KeyCode.K) && _player1 || Input.GetKeyDown(KeyCode.Keypad5) && !_player1)
            {
                _attackState = DamageType.KeyCodeK;
                _attack = true;
                _attackDirection = _direction;
            }

            if (Input.GetKeyDown(KeyCode.L) && _player1 || Input.GetKeyDown(KeyCode.Keypad6) && !_player1)
            {
                _attackState = DamageType.KeyCodeL;
                _attack = true;
                _attackDirection = _direction;
            }

            if (Input.GetKeyDown(KeyCode.I) && _player1 || Input.GetKeyDown(KeyCode.Keypad8) && !_player1)
            {
                _attackState = DamageType.KeyCodeI;
                _attack = true;
                _attackDirection = _direction;
            }
        }

        _animator.SetInteger("attack", (int)_attackState);
        _animator.SetInteger("movement", (int)_movementState);
    }

    private void ChangeAnimationLayer(int newLayer)
    {
        if (_currentLayer == newLayer) return;

        switch (newLayer)
        {
            case var _ when newLayer == _movementLayer:
                _animator.SetLayerWeight(_movementLayer, 1);
                _animator.SetLayerWeight(_attackLayer, 0);
                _animator.SetLayerWeight(_hurtLayer, 0);
                break;
            case var _ when newLayer == _attackLayer:
                _animator.SetLayerWeight(_movementLayer, 1);
                _animator.SetLayerWeight(_attackLayer, 1);
                _animator.SetLayerWeight(_hurtLayer, 0);
                break;
            case var _ when newLayer == _hurtLayer:
                _animator.SetLayerWeight(_movementLayer, 1);
                _animator.SetLayerWeight(_attackLayer, 0);
                _animator.SetLayerWeight(_hurtLayer, 1);
                break;
        }
    }

    private void OpenCollider(GameObject damageLeftCollider, GameObject damageRightCollider, Direction direction)
    {
        if (direction == Direction.Left)
        {
            damageLeftCollider.SetActive(true);
        }
        else
        {
            damageRightCollider.SetActive(true);
        }
    }

    private void CloseCollider(GameObject damageLeftCollider, GameObject damageRightCollider, Direction direction)
    {
        if (direction == Direction.Left)
        {
            damageLeftCollider.SetActive(false);
        }
        else
        {
            damageRightCollider.SetActive(false);
        }
    }

    private void StartAttack(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.KeyCodeJ:
                OpenCollider(keyCodeJLeft, keyCodeJRight, _attackDirection);
                break;
            case DamageType.KeyCodeK:
                OpenCollider(keyCodeKLeft, keyCodeKRight, _attackDirection);
                break;
            case DamageType.KeyCodeL:
                OpenCollider(keyCodeLLeft, keyCodeLRight, _attackDirection);
                break;
            case DamageType.KeyCodeI:
                OpenCollider(keyCodeILeft, keyCodeIRight, _attackDirection);
                break;
        }
    }

    private void StopAttack()
    {
        _attack = false;
        _attackState = DamageType.None;
        ChangeAnimationLayer(_movementLayer);
    }

    private void CloseAttackCollider(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.KeyCodeJ:
                CloseCollider(keyCodeJLeft, keyCodeJRight, _attackDirection);
                break;
            case DamageType.KeyCodeK:
                CloseCollider(keyCodeKLeft, keyCodeKRight, _attackDirection);
                break;
            case DamageType.KeyCodeL:
                CloseCollider(keyCodeLLeft, keyCodeLRight, _attackDirection);
                break;
            case DamageType.KeyCodeI:
                CloseCollider(keyCodeILeft, keyCodeIRight, _attackDirection);
                break;
        }
    }

    private void StartGod(DamageType damageType)
    {
        if (_player1)
        {
            Game.Player1God = true;
        }
        else
        {
            Game.Player2God = true;
        }
        StartAttack(damageType);
    }

    private void StopGod(DamageType damageType)
    {
        if (_player1)
        {
            Game.Player1God = false;
        }
        else
        {
            Game.Player2God = false;
        }
        CloseAttackCollider(damageType);
    }

    private void SummonObject(DamageType damageType)
    {
        if (_attackDirection == Direction.Left)
        {
            var rangedWeapon =  Instantiate(attackObject, keyCodeLLeft.transform.position, new Quaternion(0, 0, 0, 0));
            rangedWeapon.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            Instantiate(attackObject, keyCodeLRight.transform.position, new Quaternion(0, 0, 0, 0));
        }
    }

    private void StartHurt()
    {
        StopAttack();
        CloseAttackCollider(DamageType.KeyCodeJ);
        CloseAttackCollider(DamageType.KeyCodeK);
        CloseAttackCollider(DamageType.KeyCodeL);
        CloseAttackCollider(DamageType.KeyCodeI);
    }

    private void EndHurt()
    {
        _hurt = false;
        _hurtState = 0;
        _animator.SetInteger("hurt", _hurtState);
    }

    private void IsGameOver()
    {
        var blood = _player1 ? Game.Player1Blood : Game.Player2Blood;
        if (blood > 0) return;
        ChangeAnimationLayer(_hurtLayer);
        _hurtState = 2;
        _animator.SetInteger("hurt", _hurtState);
        Game.IsGameOver = true;
    }
}