using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Security;
using JetBrains.Annotations;
using Unity.VisualScripting;
using System;

public class Player : MonoBehaviour
{
    Rigidbody2D rig;            //player�� Rigidbody2D
    public Text txt;
    Canvas canvas;

    public GameObject boom;    
    public static GameObject boomOb;    //enemy���� boomOb�� �����ϱ� ���� static ���
    public static int gameScore;        //���� ���� ����

    public float speed = 5f;    //ĳ���� �¿� �̵� �ӵ��� ����
    public float jumpPower = 300f; //ĳ������ ������

    int dir;                    //�¿� ���⿡ ����
    int atDir;                  //���� �¿� ����
    int doubleJ = 0;                //��������
    int atCooldown = 0;         //���� ��Ÿ��

    bool flipFlag = false;      //flip ���θ� �Ǵ�
    //bool canJump = true;       //jump���� ���θ� �Ǵ�
    bool pause = false;        //�Ͻ����� ���� �Ǵ�

    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();

        Enemy.enemyMov = true;
        atDir = 1;
    }


    void Update()
    {
        //����
        txt.text = "Score : " + gameScore;

        //�̵�
        dir = 0;    //�ƹ� �Է��� ������ ������ �ְ��ϱ�, �̲������� �ʰ� �ϱ�
        if (Input.GetKey("d"))
        {
            dir = 1;
            atDir = 1;
            flipFlag = false;
        }

        if (Input.GetKey("a"))
        {
            dir = -1;
            atDir = -1;
            flipFlag = true;
        }

        if (Input.GetKeyDown("space") && doubleJ < 2)
        {
            doubleJ++;
            rig.AddForce(new Vector2 (0, jumpPower));
        }

        this.rig.velocity = new Vector2(dir * speed, rig.velocity.y);   //�̵��� ����
        this.GetComponent<SpriteRenderer>().flipX = flipFlag;           //������ȯ �� �ø�

        //����
        atCooldown--;
        if (Input.GetMouseButtonDown(0) && atCooldown <= 0)
        {
            atCooldown = 600;

            Vector3 pos = this.transform.position;
            Vector3 boomPos = new Vector3(pos.x, pos.y + 1f, pos.z);

            boomOb = Instantiate(boom, boomPos, Quaternion.identity);
            boomOb.GetComponent<Rigidbody2D>().AddForce(new Vector2(500f * atDir, 10f));

            Destroy(boomOb, 0.5f);
        }
        
        //�Ͻ�����
        if (Input.GetKeyDown("escape") && pause == false)
        {
            pause = true;
            Time.timeScale = 0;
            //GameObject.Find("PlayButton").SetActive(true);
        }
        else if (Input.GetKeyDown("escape") && pause == true)
        {
            pause = false;
            Time.timeScale = 1;
            //GameObject.Find("PlayButton").SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //���� ���� ���θ� �Ǵ�
        if (collision.gameObject.tag == "Floor")
        {
            doubleJ = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //�̰� ��ԵǸ� ��� ���Ͱ� �Ѿƿ��Ե� �ذ��Ϸ���? �ذ�
        //���� ���� �Ѿƿ��� ����� ���� �Ǵ�
        if (collision.gameObject.tag == "EnemyCome")
        {
            Enemy.enemyMov = false;  
        }

        else if (collision.gameObject.tag == "Item")
        {
            gameScore += 10;
        }

        Destroy(collision.gameObject);  //�浹��.���ӿ�����Ʈ�� destroy
    }
}
