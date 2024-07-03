using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    public GameObject enemys;   //적 귀신을 생성하기 위해 만든 변수
    public GameObject followT;  //적이 따라오게 하는 트리거를 생성하기 위해 만든 변수

    Vector3 spawnPos;   //enemy가 생성되는 위치
    Vector3 triPos;     //followT가 생성되는 위치


    float mobOffset;    //player보다 얼마나 떨어져서 생성시킬지 결정하는 변수
    float triOffset;    //상동

    void Start()
    {
        mobOffset = 8;
        triOffset = 4;
    }

    void Update()
    {
        //위치값들을 계산
        Vector3 playerPos = GameObject.Find("ballon").transform.position;
        Vector3 thisPos = this.transform.position;

        spawnPos = new Vector3(playerPos.x + mobOffset, 1.5f, thisPos.z);
        triPos = new Vector3(playerPos.x + triOffset, 10f, thisPos.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Spawn Trigger에 이 스크립트를 넣어줘서 Spawn Trigger에 player가 닿으면 enemy와 followT를 생성하고 자신(Spawn Trigger)은 제거
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(enemys, spawnPos, Quaternion.identity);
            Instantiate(followT, triPos, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
