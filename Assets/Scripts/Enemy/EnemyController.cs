using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵キャラクタークラス
/// </summary>
public class EnemyController : MonoBehaviour
{
    GameObject _player;
    Transform _target;
    Vector3 _originVec;
    Rigidbody2D _rigidBody;

    EnemyStatus _enemyStatus;

    /// <summary>
    /// 敵の歩行速度
    /// </summary>
    float _speed = 0f;
    float _waitTime;
    /// <summary>
    /// 歩行時のタイミング
    /// </summary>
    const float _intervalTime = 1f;

    /// <summary>
    /// プレイヤーを感知した際のフラグ
    /// </summary>
    bool _foundPlayer;

    /// <summary>
    /// 敵のHP
    /// </summary>
    int _enemyHP = 0;

    public int EnemyHP
    {
        private set => _enemyHP = value;
        get => _enemyHP;
    }

    /// <summary>
    /// 左右の移動範囲
    /// </summary>
    [Header("移動範囲")]
    [SerializeField]
    float _movementRange;
    float _movementRangeLeft;
    float _movementRangeRight;


    public enum MOVE_DIRECTION
    {
        RIGHT,
        LEFT,
        STOP,
    }
    MOVE_DIRECTION _moveDirection;

    void Awake()
    {
        TryGetComponent(out _rigidBody);
        TryGetComponent(out _enemyStatus);
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start() {
        _movementRangeLeft = -(_movementRange / 2f) + transform.position.x;
        _movementRangeRight = (_movementRange / 2f) + transform.position.x;
        _target = _player.transform;
        _originVec = transform.position;
        _waitTime = _intervalTime;
        _moveDirection = MOVE_DIRECTION.LEFT;
        _enemyHP = _enemyStatus.MaxHP;
        _speed = _enemyStatus.Speed;
        //Debug.Log("enemyHP:" + EnemyHP);
        //Debug.Log("speed:" + _speed);

    }

    void FixedUpdate()
    {
        //プレイヤーを見つけた時の追尾
        if(_foundPlayer)
        {
            Vector2 direction = _target.position - transform.position;
            direction = direction.normalized;

            _rigidBody.velocity = direction;
        }
        //敵の徘徊
        else{
            if(_waitTime > 0){
                _waitTime -= Time.deltaTime;
            }
            switch (_moveDirection)
            {
                case MOVE_DIRECTION.STOP:
                    _speed = 0;
                    break;
                case MOVE_DIRECTION.LEFT:
                    transform.localScale = new Vector3(-1, 1, 1);
                    var minusSpeed = -(Mathf.Abs(_speed));
                    _speed = minusSpeed;
                    break;
                case MOVE_DIRECTION.RIGHT:
                    transform.localScale = new Vector3(1, 1, 1);
                    var plusSpeed = (Mathf.Abs(_speed));
                    _speed = plusSpeed;
                    break;
            }
            if(_waitTime <= 0){

                if(transform.position.x - Mathf.Abs(_speed) <= _movementRangeLeft){
                    ChangeDirectionRight();
                }
                if(transform.position.x + Mathf.Abs(_speed) >= _movementRangeRight){
                    ChangeDirectionLeft();
                }

                _rigidBody.velocity = new Vector2(_speed, _rigidBody.velocity.y);
                // Vector2 force = new Vector2(_speed * 50f, _rigidBody.velocity.y);
                // _rigidBody.AddForce(force);
                _waitTime = _intervalTime;
            }
        }
    }

    /// <summary>
    /// 敵へのダメージ
    /// </summary>
    /// <param name="damage"></param>
    public void DamageToEnemy(){
        _enemyHP -= 100;
    }

    /// <summary>
    /// 敵の索敵範囲内にプレイヤーが入っているかを判定　
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.CompareTag("Player")){
            _foundPlayer = true;
        }
    }

    /// <summary>
    /// 敵の索敵範囲内からプレイヤーが外れたかを判定
    /// </summary>
    /// <param name="collider"></param>
    void OnTriggerExit2D(Collider2D collider) {
        if(collider.gameObject.CompareTag("Player")){
            _foundPlayer = false;
        }
    }

    /// <summary>
    /// プレイヤーに接触した時の処理　TODO：体力とダメージ量の追加。
    /// </summary>
    /// <param name="collider"></param>
    void OnCollisionEnter2D(Collision2D collider){
        if(collider.gameObject.CompareTag("Player")){
            Debug.Log("プレイヤーに接触");
        }
    }
    void ChangeDirectionRight(){
        _moveDirection = MOVE_DIRECTION.RIGHT;
    }
    void ChangeDirectionLeft(){
        _moveDirection = MOVE_DIRECTION.LEFT;
    }

    /// <summary>
    /// 方向転換処理。TODO：挙動が少しおかしい。ChangeDirectionRight,ChengeDirectionLeftで代用
    /// </summary>
    // void ChangeDirection(){
    //     Debug.Log($"left:{_movementRangeLeft},right:{_movementRangeRight}");
    //     if(_moveDirection == MOVE_DIRECTION.RIGHT){
    //         _moveDirection = MOVE_DIRECTION.LEFT;
    //     }
    //     else{
    //         _moveDirection = MOVE_DIRECTION.RIGHT;
    //     }
    // }

    /// <summary>
    /// 敵の移動範囲の可視化(デバッグ用)
    /// </summary>
    void OnDrawGizmos() {
        Vector3 startVec = transform.position - new Vector3((_movementRange/2), 0f, 0f);
        Vector3 endVec = transform.position + new Vector3((_movementRange/2), 0f, 0f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(startVec, endVec);

    }
}
