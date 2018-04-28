using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target; //координаты тела парснажа
    public Transform rightHand; //координаты правой кисти персонажа
    public Transform leftHand; //координаты левой кисти персонажа
    public float smoothSpeed = .5f; //задержка для эффекта smoothdamp

    private Vector3 currentVelocity;

    PlayerController playerController; //экземпляр класса PlayerController

    Vector3 newPos; //координаты для нового положения камеры

    public void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    private void LateUpdate()
    {
        if (target.position.y > transform.position.y-3)
        {
            //обработка координаты для "смазывания" камеры
            if (playerController.direction == true)
            {
                newPos = new Vector3(leftHand.position.x,leftHand.position.y,transform.position.z);
            }
            else
            {
                newPos = new Vector3(rightHand.position.x,rightHand.position.y,transform.position.z);
            }

            transform.position = Vector3.SmoothDamp(transform.position,newPos,ref currentVelocity,smoothSpeed); //"смазывание" движения камеры
            //Vector3.Lerp(transform.position,newPos,smoothSpeed);
        }
    }
}
