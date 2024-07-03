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
    Rigidbody2D enemyRig;       //������ Rigidbody2D

    public GameObject uni;
    public static bool enemyMov = true; //���Ͱ� �÷��̾ �Ѿƿ��� �ϴ��� �պ���� ���� �Ǵ�

    float eSpeed;               //�պ���� �Ҷ� ������ �ӵ�
    float eSpeedF;              //player�� �Ѿƿö� ������ �ӵ�

    int count;                  //���Ͱ� ������ȯ �ϴ� ī��Ʈ
    int maxCount = 600;         //���Ͱ� ������ȯ �ϴ� ī��Ʈ
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
            
            //�����ð����� �Դٰ��� �Ѵ�
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

            //player�� �Ѿƿ��� �Ѵ�.
            else if (enemyMov == false)
            {
                //playerPos�� ballon�̶�� �÷��̾��� ��ġ���� ����
                //thisPos�� ������ ���� ��ġ���� ����

                float sum = playerPos.x - obake.x;    //�ø� ���� ���θ� �Ǵ��ϱ� ���� ����
                Vector3 pos = (playerPos - obake).normalized; //�÷��̾���� ����

                enemyRig.velocity = new Vector3(pos.x, 0f, 0f) * eSpeedF;   //�̵�
                this.GetComponent<SpriteRenderer>().flipX = sum < 0;        //�ø�

                //�÷��̾ �� ������ ����� ���� ���Ѿƿ��� ������������ ���ư�
                if (startPos.x - chaseRange >= playerPos.x || startPos.x + chaseRange <= playerPos.x)
                {
                    float sum2 = startPos.x - obake.x;    //�ø� ���� ���θ� �Ǵ��ϱ� ���� ����
                    Vector3 pos2 = (startPos - obake).normalized; //startPos���� ����

                    enemyRig.velocity = new Vector3(pos2.x, 0f, 0f) * eSpeedF;   //�̵�
                    this.GetComponent<SpriteRenderer>().flipX = sum2 < 0;        //�ø�
                }
            }
        }
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�÷��̾�� ���� �ε����� ���ӿ���
        if (collision.gameObject.tag == "Player")
        {
            //enemyMov = true;
            SceneManager.LoadScene("05_GameOver");
            if (Player.gameScore >= SceneChange.maxScore)
            {
                SceneChange.maxScore = Player.gameScore;
            }
        }

        //�÷��̾��� ���ݿ� ������ ������ ���
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
        //uni(cloud�� ����)�� �÷��̾�� ������ ������ ���
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
        //uni�� ��� �������ִ� ���
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
        //cloud�� ���� �ٸ��� �����̰� �ϱ����� ������ȯ �ϴ� ������ �� �ٸ��� ������ִ� ����
        if (conff == true)
        {
            maxCount = Random.Range(600, 1200);
            conff = false;
        }
    }

}
