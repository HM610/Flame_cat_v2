using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵ステータスクラス
/// </summary>
[RequireComponent(typeof(EnemyStatus))]
public class EnemyStatus : MonoBehaviour
{
    /// <summary>
    /// 敵の最大HP
    /// </summary>
    public int MaxHP{
        get{
            string tagName = this.gameObject.tag;
            if(tagName == "EnemyA"){
                return 100;
            }
            else if(tagName == "EnemyB"){
                return 150;
            }
            else if(tagName == "EnemyC"){
                return 200;
            }
            else{
                return 1;
            }
        }
    }
    /// <summary>
    /// 敵の歩行速度
    /// </summary>
    public float Speed{
        get{
            string tagName = this.gameObject.tag;
            if(tagName == "EnemyA"){
                return 2f;
            }
            else if(tagName == "EnemyB"){
                return 2.5f;
            }
            else if(tagName == "EnemyC"){
                return 3f;
            }
            else{
                return 2f;
            }
        }

    }
}
