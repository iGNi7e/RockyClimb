using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Transform leftHand; //координаты левой кисти
    public Transform rightHand; //координаты правой кисти
    public Text scoreText; //префаб счета

    [SerializeField]
    private float speed = 2f; //скорость вращения персонажа
    private int score;

    public bool direction; //переменная для опредления нарпавления вращения персонажа
    float timeBetweenClicks = 5f; // время которое можно держаться на одном камне
    public float nextTime; //вспомогательная переменная для timeBetweenClicks

    GenerateMap map;
    GameUI gameUI;

    public event System.Action OnDeath; //событие которое срабатывает при смерти

    // Use this for initialization
    void Start () {
        nextTime = timeBetweenClicks;
        map = FindObjectOfType<GenerateMap>();
        gameUI = FindObjectOfType<GameUI>();
        score = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextTime && transform.position.y > -2f) //условие удержания на одном камне
        {
            OnDeath();
        }

        if (Input.GetMouseButtonDown(0)) //событие клика на Mouse0
        {
            if (LeftHandDetector.isRockedLeftHand || RightHandDetector.isRockedRightHand) //Условие, если кисть находится на камне, то можем менять направление
            {
                direction = !direction; //смена направления
                nextTime = Time.time + timeBetweenClicks; //обновление переменной

                score++; //увеличение счета на табло сверху
                scoreText.text = score.ToString();

                map.CreateNewPos(); //создание нового камня
            }
            else
            {
                OnDeath(); //если нажатие было по пустой области срабатывает событие
            }
        }


        if (direction) //изменение вращения вокруг камня
        {
            transform.RotateAround(leftHand.position,new Vector3(0,0,10f),Time.deltaTime * 100f * speed);
            leftHand.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            rightHand.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }
        else
        {
            transform.RotateAround(rightHand.position,new Vector3(0,0,-10f),Time.deltaTime * 100f * speed);
            leftHand.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            rightHand.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }
    

}
