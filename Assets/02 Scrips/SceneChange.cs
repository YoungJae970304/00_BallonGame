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
    public GameObject ui;   //Ŭ���ϴ� ������Ʈ�� �̸��� �����ϱ� ���� ����

    public static int maxScore = 0;         //�ִ� ����

    public int nowScene;    //���� ������������ �浹������ ���� ���� ���������� �Ѿ�� ���� ����

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
        //�� ������Ʈ�� �� ��ũ��Ʈ�� �־��༭ ��� �÷��̾ �浹�ϸ� ���� ���������� �Ѿ
        //nowScene�� ���� ���� �������� ��ȣ�� �Է��ؾ���
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
        //nowScene�� ������� start, clear, over�� Ŭ���ϸ� �Ʒ� ����� ����

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
