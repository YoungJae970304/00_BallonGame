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
        //�÷��̾��� x���� ī�޶��� x���� ��� �������༭ ī�޶� �÷��̾ ��� ����

        Vector3 cameraPos = new Vector3(player.transform.position.x, pos.y * 5, pos.z);
        this.transform.position = cameraPos;
    }
}
