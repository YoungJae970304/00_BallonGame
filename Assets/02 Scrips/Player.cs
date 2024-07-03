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
    Rigidbody2D rig;            //player의 Rigidbody2D
    public Text txt;
    Canvas canvas;

    public GameObject boom;    
    public static GameObject boomOb;    //enemy에서 boomOb에 접근하기 위해 static 사용
    public static int gameScore;        //게임 점수 변수

    public float speed = 5f;    //캐릭터 좌우 이동 속도에 관여
    public float jumpPower = 300f; //캐릭터의 점프력

    int dir;                    //좌우 방향에 관여
    int atDir;                  //공격 좌우 방향
    int doubleJ = 0;                //더블점프
    int atCooldown = 0;         //공격 쿨타임

    bool flipFlag = false;      //flip 여부를 판단
    //bool canJump = true;       //jump가능 여부를 판단
    bool pause = false;        //일시정지 여부 판단

    void Start()
    {
        rig = this.GetComponent<Rigidbody2D>();

        Enemy.enemyMov = true;
        atDir = 1;
    }


    void Update()
    {
        //점수
        txt.text = "Score : " + gameScore;

        //이동
        dir = 0;    //아무 입력이 없을때 가만이 있게하기, 미끄러지지 않게 하기
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

        this.rig.velocity = new Vector2(dir * speed, rig.velocity.y);   //이동에 관련
        this.GetComponent<SpriteRenderer>().flipX = flipFlag;           //방향전환 시 플립

        //공격
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
        
        //일시정지
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
        //점프 가능 여부를 판단
        if (collision.gameObject.tag == "Floor")
        {
            doubleJ = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //이걸 밟게되면 모든 몬스터가 쫓아오게됨 해결하려면? 해결
        //적이 나를 쫓아오게 만들기 위한 판단
        if (collision.gameObject.tag == "EnemyCome")
        {
            Enemy.enemyMov = false;  
        }

        else if (collision.gameObject.tag == "Item")
        {
            gameScore += 10;
        }

        Destroy(collision.gameObject);  //충돌한.게임오브젝트를 destroy
    }
}
