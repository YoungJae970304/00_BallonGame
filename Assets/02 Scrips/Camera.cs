using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject player;

    Vector3 pos;

    void Start()
    {
        pos = this.transform.position;
    }


    void Update()
    {
        //플레이어의 x값을 카메라의 x값에 계속 대입해줘서 카메라가 플레이어를 계속 비춤

        Vector3 cameraPos = new Vector3(player.transform.position.x, pos.y * 5, pos.z);
        this.transform.position = cameraPos;
    }
}
