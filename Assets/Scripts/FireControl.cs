using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour {

    Rigidbody2D rbody = null;
    public GameObject Cat;

    [SerializeField] float speed = 5f;  //炎のスピード.
    bool dir;                           //炎を発射する向き.

    // Start is called before the first frame update
    void Start() {
        Cat = GameObject.Find("Cat");
        rbody = GetComponent<Rigidbody2D>();

        if (Cat.transform.right.x > 0) {    //右.
            dir = true;
        } else if (Cat.transform.right.x < 0) { //左.
            dir = false;
        }

    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// 炎を発射する.
    ///　</summary>
    private void FixedUpdate() {

        if (dir) {
            rbody.velocity =  new Vector2(speed, 0);
        } else if (!dir) {
            rbody.velocity =  new Vector2(-speed, 0);
        }

    }


    /// <summary>
    /// オブジェクトのコライダーが別のコライダーに衝突したとき炎を消す
    ///　</summary>
    private void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.gameObject.CompareTag("Enemy")) {
            Destroy(gameObject);
        }

    }

    /// <summary>
    /// カメラでオブジェクトが表示されなくなるときに呼ばれる炎を消す
    ///　</summary>
    private void OnBecameInvisible() {
        Destroy(gameObject);    //映らなくなったら消す
    }

}
