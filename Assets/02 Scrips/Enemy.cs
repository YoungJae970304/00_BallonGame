using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEditor;
using UnityEditor.Experimental.GraphView;

public class Enemy : MonoBehaviour
{
    Rigidbody2D enemyRig;       //몬스터의 Rigidbody2D

    public GameObject uni;
    public static bool enemyMov = true; //몬스터가 플레이어를 쫓아오게 하는지 왕복운동만 할지 판단

    float eSpeed;               //왕복운동만 할때 몬스터의 속도
    float eSpeedF;              //player를 쫓아올때 몬스터의 속도

    int count;                  //몬스터가 방향전환 하는 카운트
    int maxCount = 600;         //몬스터가 방향전환 하는 카운트
    int chaseRange = 8;

    bool conff = true;

    Vector3 cloud;
    Vector3 obake;
    Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    void Start()
    {
        enemyRig = this.GetComponent<Rigidbody2D>();
        
        eSpeed = 4f;
        eSpeedF = 6f;
        
        InvokeRepeating("Rainy", 0.1f, 0.5f);
    }

  
    void Update()
    {
        if (this.transform.tag == "Cloud")
        {
            cloud = this.transform.position;

            //maxCount = 1200;
            MaxCountFix();
            count++;

            if (count >= maxCount)
            {
                eSpeed = -eSpeed;
                count = 0;
            }
            
            //cloudMov = new Vector3(Random.Range(-2f, 2f), 0, 0);

            transform.Translate(Vector2.right * eSpeed * Time.deltaTime);
        }

        else if (this.transform.tag == "Obake")
        {
            Vector3 playerPos = GameObject.Find("ballon").transform.position;
            obake = this.transform.position;
            
            //일정시간마다 왔다갔다 한다
            if (enemyMov == true)
            {
                count++;

                if (count >= maxCount)
                {
                    eSpeed = -eSpeed;
                    count = 0;
                }

                enemyRig.velocity = Vector2.right * eSpeed;
                this.GetComponent<SpriteRenderer>().flipX = eSpeed < 0;
            }

            //player를 쫓아오게 한다.
            else if (enemyMov == false)
            {
                //playerPos에 ballon이라는 플레이어의 위치값을 대입
                //thisPos에 몬스터의 현재 위치값을 대입

                float sum = playerPos.x - obake.x;    //플립 가능 여부를 판단하기 위한 변수
                Vector3 pos = (playerPos - obake).normalized; //플레이어로의 방향

                enemyRig.velocity = new Vector3(pos.x, 0f, 0f) * eSpeedF;   //이동
                this.GetComponent<SpriteRenderer>().flipX = sum < 0;        //플립

                //플레이어가 몹 범위를 벗어나면 몹은 안쫓아오고 시작지점으로 돌아감
                if (startPos.x - chaseRange >= playerPos.x || startPos.x + chaseRange <= playerPos.x)
                {
                    float sum2 = startPos.x - obake.x;    //플립 가능 여부를 판단하기 위한 변수
                    Vector3 pos2 = (startPos - obake).normalized; //startPos로의 방향

                    enemyRig.velocity = new Vector3(pos2.x, 0f, 0f) * eSpeedF;   //이동
                    this.GetComponent<SpriteRenderer>().flipX = sum2 < 0;        //플립
                }
            }
        }
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //플레이어와 적이 부딪히면 게임오버
        if (collision.gameObject.tag == "Player")
        {
            //enemyMov = true;
            SceneManager.LoadScene("05_GameOver");
            if (Player.gameScore >= SceneChange.maxScore)
            {
                SceneChange.maxScore = Player.gameScore;
            }
        }

        //플레이어의 공격에 맞으면 나오는 명령
        else if (collision.gameObject.tag == "AtOb")
        {
            Player.gameScore += 20;
            enemyMov = true;
            Destroy(gameObject);
            Destroy(Player.boomOb);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //uni(cloud의 공격)이 플레이어에게 닿으면 나오는 명령
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("05_GameOver");
            if (Player.gameScore >= SceneChange.maxScore)
            {
                SceneChange.maxScore = Player.gameScore;
            }
        }
    }

    private void Rainy()
    {
        //uni를 계속 생성해주는 명령
        if (this.transform.tag == "Cloud")
        {
            Vector3 area = GetComponent<SpriteRenderer>().bounds.size;
            Vector3 cloudPos = cloud;

            cloudPos.x += Random.Range(-area.x / 2, area.x / 2);
            cloudPos.y += Random.Range(-area.y / 2, area.y / 2);

            GameObject unis = Instantiate(uni, cloudPos, Quaternion.identity);
            Destroy(unis, 1.3f);
        }   
    }

    void MaxCountFix()
    {
        //cloud가 각자 다르게 움직이게 하기위해 방향전환 하는 시점을 다 다르게 만들어주는 조건
        if (conff == true)
        {
            maxCount = Random.Range(600, 1200);
            conff = false;
        }
    }

}
