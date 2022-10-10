using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour
{
    Rigidbody2D rbody;
    SpriteRenderer srend;
    
    float axisH = 0.0f;
    [SerializeField] float move_speed = 5.0f;       //移動速度.
    [SerializeField] float jump_power = 300f;       //ジャンプ力.
    private bool isGround = false;                  //接地判定用.

    public GameObject fireObject;                   //炎オブジェクト.
    [SerializeField]float canFireTime = 0.2f;       //炎の発射可能時間.
    float elapsed_fireTime;                         //炎の発射経過時間.
    bool canFire = true;                            //発射可能か.

    public int catHp = 3;
    bool onDamege = false;

    KeyCode jumpKey = KeyCode.Space;      //一応変更できるように定義.
    KeyCode fireKey = KeyCode.F;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        srend = GetComponent<SpriteRenderer>();
        elapsed_fireTime = canFireTime;
    }

    // Update is called once per frame
    void Update()
    {

        axisH = Input.GetAxis("Horizontal");
        if (axisH < 0) {    //猫の向き.
            transform.eulerAngles = new Vector3(0, 180, 0);
        } else if (axisH > 0) {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }

        if (Input.GetKeyDown(jumpKey) && rbody.velocity.y == 0) { //スペース押し且つ上方向に速度がないとき.
            rbody.AddForce(transform.up * jump_power);
        }

        //一定時間超えたら炎を撃てる.
        elapsed_fireTime += Time.deltaTime;

        if (canFireTime < elapsed_fireTime) {
            canFire = true;
        }

        if (Input.GetKeyDown(fireKey)) {
            if (canFire) {
                //TODO:炎出る位置調整必要そう.
                Instantiate(fireObject, transform.position, Quaternion.identity);
                canFire = false;
                elapsed_fireTime = 0f;
            }
        }
        
    }

    void FixedUpdate() {

        if ( isGround == true ) {   //接地中なら移動.
            rbody.velocity = new Vector2(axisH * move_speed, rbody.velocity.y);
        }

    }

    void Damage() {

        if (onDamege == true) { //無敵時間中なら.
            return;
        }

        catHp -= 1;
        if (catHp <= 0) {　//ゲームオーバー
            Debug.Log("ゲームオーバー");
        }else {
            Debug.Log("Damege:" + catHp);
            WaitForDamege();
        }

    }

    IEnumerator WaitForDamege() {
        // 1秒間処理を止める
        onDamege = true;
        //transform.Translate(Vector3.left * move_speed);
        //yield return new WaitForSeconds(1);
        //srend.color = new Color(1f, 1f, 1f, 1f);
        for (int i = 0; i < 10; i++) {
            srend.enabled = false;
            yield return new WaitForSeconds(0.05f);
            srend.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
        onDamege = false;

    }

    //接地判定
    void OnCollisionStay2D(Collision2D collision) {
        if (collision.collider.CompareTag("Ground")) {
            isGround = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.CompareTag("Ground")) {
            isGround = false;
        }
    }

   void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            Damage();
        }
    }



}
