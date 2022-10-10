using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerScript : MonoBehaviour
{
    public GameObject posingPanel;
    public bool isPosing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPosing == false) //ポーズ中ではない時
        {
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                //ポーズパネルをオンにして、ポーズ中に変更
                posingPanel.SetActive(true);
                isPosing = true;
            }
        } else
        {
            if (Input.GetKeyUp(KeyCode.LeftShift)) //ポーズ中の時
            {
                BackToTheGame();
            }
        }
    }

    public void BackToTheTitle()
    {
        //（未実装）変数をすべて戻す処理
    }

    //（途中から未実装）タイマーを再開させる等の処理
    public void BackToTheGame()
    {
        //ポーズパネルをオフにして、ポーズ中を解除
        posingPanel.SetActive(false);
        isPosing = true;
    }
}
