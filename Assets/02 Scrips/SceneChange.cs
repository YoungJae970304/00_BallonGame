using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;
using UnityEngine.SocialPlatforms.Impl;

public class SceneChange : MonoBehaviour
{
    public GameObject ui;   //클릭하는 오브젝트의 이름을 구별하기 위해 만듬

    public static int maxScore = 0;         //최대 점수

    public int nowScene;    //메인 스테이지에서 충돌판정을 통해 다음 스테이지로 넘어가기 위해 만듬

    public Text maxS;

    void Start()
    {
    }

    
    void Update()
    {
        maxS.text = "Max Score : " + maxScore;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //골 오브젝트에 이 스크립트를 넣어줘서 골과 플레이어가 충돌하면 다음 스테이지로 넘어감
        //nowScene에 직접 현재 스테이지 번호를 입력해야함
        if (collision.gameObject.tag == "Player")
        {
            Player.gameScore += 30;
            nowScene++;
            SceneManager.LoadScene(nowScene);

            if (Player.gameScore >= maxScore)
            {
                maxScore = Player.gameScore;
            }
        }
    }

    private void OnMouseDown()
    {
        //nowScene과 상관없이 start, clear, over를 클릭하면 아래 명령을 실행

        if (ui.name == "start")
        {
            SceneManager.LoadScene("01_Stage1");
            Player.gameScore = 0;
        }

        else if (ui.name == "gameover")
        {
            SceneManager.LoadScene("00_Main");
            Player.gameScore = 0;
        }

        else if (ui.name == "gameclear")
        {
            SceneManager.LoadScene("00_Main");

            Player.gameScore = 0;
        }
    }
}
