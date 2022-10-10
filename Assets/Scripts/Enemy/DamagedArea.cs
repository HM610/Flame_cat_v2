using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ダメージ感知用クラス
/// </summary>
[RequireComponent(typeof(DamagedArea))]
public class DamagedArea : MonoBehaviour
{
    EnemyController enemyController;
    // Start is called before the first frame update
    void Awake()
    {
        enemyController = transform.parent.gameObject.GetComponent<EnemyController>();
    }
    /// <summary>
    /// プレイヤーからの攻撃を受けた時の処理
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.CompareTag("Fire")){
            enemyController.DamageToEnemy();
        }
    }
}
