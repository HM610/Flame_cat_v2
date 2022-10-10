using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour
{

    //ゲーム画面で一番最初に表示されるタイトル画面のスクリプト

    TimerMonoBehaviour tmb;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //タイトル画面がクリックされると画面が消え、カウントダウンが始まる
        //Timerのスクリプトをどう取得すればいいか分からなかったので、カウントダウンに関しては未実装です

        if (Input.GetMouseButtonUp(0))
        {
            gameObject.SetActive(false);
        }
    }
}
