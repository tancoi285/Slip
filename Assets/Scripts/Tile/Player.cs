using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Tile
{
    private Rigidbody2D _myBody;
    private Animator _AC;
    private float _speed;
    public Direct _direct;
    public bool _isSlip;
    public bool _isRun;
    private Vector2 _startDrag;
    private Vector2 _dragDelta;
    private int _countSnow;
    public int _starEarned;

    void Awake()
    {
        Init();
    }

    protected override void Create()
    {
        base.Create();
        _myBody = transform.GetComponent<Rigidbody2D>();
        _AC = transform.GetComponent<Animator>();
        _type = Type.Player;
        _speed = 5;
    }

    void ResetDrag()
    {
        _startDrag = _dragDelta = Vector2.zero;
    }

    void DragControl()
    {
        if (Input.GetMouseButtonDown(0))
            _startDrag = Input.mousePosition;
        else if (Input.GetMouseButtonUp(0))
            ResetDrag();
    }

    void SetDirect()
    {
        if (!_isSlip)
        {
            if (Input.GetMouseButton(0))
            {
                _dragDelta = (Vector2)Input.mousePosition - _startDrag;
                if (_dragDelta.magnitude > 100)
                {
                    _isSlip = true;
                    float x = _dragDelta.x;
                    float y = _dragDelta.y;
                    if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        if (x < 0)
                            _direct = Direct.Left;
                        else _direct = Direct.Right;
                    }
                    else
                    {
                        if (y < 0)
                            _direct = Direct.Down;
                        else _direct = Direct.Up;
                    }
                }
            }
        }
    }

    void MoveDirect(Vector3 direct)
    {
        transform.position = transform.position + direct * _speed * Time.deltaTime;
        if (!_isRun) 
            _AC.Play("Slip");
    }

    void Move()
    {
        if (_isSlip)
        {
            switch (_direct)
            {
                case Direct.Up:
                    MoveDirect(Vector3.up);
                    break;
                case Direct.Down:
                    MoveDirect(Vector3.down);
                    break;
                case Direct.Left:
                    MoveDirect(Vector3.left);
                    break;
                case Direct.Right:
                    MoveDirect(Vector3.right);
                    break;
            }
        }
    }

    public void ToIdle()
    {
        _AC.Play("Idle_" + _direct.ToString().ToLower());
        _isSlip = false;
        _speed = 5;
    }

    public void ToRun()
    {
        _isRun = true;
        _AC.Play("Run_" + _direct.ToString().ToLower());
        _speed = 0;
    }

    public void OnSnow()
    {
        _countSnow += 1;
    }

    public void IsOnSnow()
    {
        _countSnow -= 1;
        if (_countSnow < 1)
            _isRun = false;
        else _isRun = true;
    }

    public void Reset()
    {
        _isSlip = false;
        _isRun = false;
        _direct = Direct.None;
        _AC.Play("Idle_down");
        _starEarned = 0;
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        ToIdle();
        float magnitude = 50;
        Vector3 force = transform.position - other.transform.position;
        force.Normalize();
        _myBody.AddForce(force * magnitude);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        ToIdle();
    }

    void OnCollisionExit2D(Collision2D other)
    {
        _myBody.velocity = Vector2.zero;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Coin>())
            _starEarned += 1;
    }

    void Update()
    {
        DragControl();
        SetDirect();
    }

    void FixedUpdate()
    {
        Move();
    }

    public int starEarned
    {
        get { return _starEarned; }
        set { _starEarned = value; }
    }

    public enum Direct
    {
        None, Up, Down, Left, Right
    }
}
