using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    public GameObject enemys;   //�� �ͽ��� �����ϱ� ���� ���� ����
    public GameObject followT;  //���� ������� �ϴ� Ʈ���Ÿ� �����ϱ� ���� ���� ����

    Vector3 spawnPos;   //enemy�� �����Ǵ� ��ġ
    Vector3 triPos;     //followT�� �����Ǵ� ��ġ


    float mobOffset;    //player���� �󸶳� �������� ������ų�� �����ϴ� ����
    float triOffset;    //��

    void Start()
    {
        mobOffset = 8;
        triOffset = 4;
    }

    void Update()
    {
        //��ġ������ ���
        Vector3 playerPos = GameObject.Find("ballon").transform.position;
        Vector3 thisPos = this.transform.position;

        spawnPos = new Vector3(playerPos.x + mobOffset, 1.5f, thisPos.z);
        triPos = new Vector3(playerPos.x + triOffset, 10f, thisPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Spawn Trigger�� �� ��ũ��Ʈ�� �־��༭ Spawn Trigger�� player�� ������ enemy�� followT�� �����ϰ� �ڽ�(Spawn Trigger)�� ����
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(enemys, spawnPos, Quaternion.identity);
            Instantiate(followT, triPos, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
