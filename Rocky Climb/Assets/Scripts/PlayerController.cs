using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public Transform leftHand; //координаты левой кисти
    public Transform rightHand; //координаты правой кисти
    public Text scoreText; //префаб счета
    public GameObject crack; //префаб трещины

    [SerializeField]
    private float speed = 2f; //скорость вращения персонажа

    private int score; //переменная для подсчета количеста удачных хлдов

    public bool direction; //переменная для опредления нарпавления вращения персонажа
    bool rotate; //переменная для отключения вращения

    float timeBetweenClicks = 3f; // время которое можно держаться на одном камне
    public float nextTime; //вспомогательная переменная для timeBetweenClicks

    GenerateMap map;
    GameUI gameUI;
    LeftHandDetector lhd;
    RightHandDetector rhd;

    private Vector3 currentVelocity;
    

    public event System.Action OnDeath; //событие которое срабатывает при смерти

    // Use this for initialization
    void Start () {
        nextTime = Time.time + timeBetweenClicks;
        map = FindObjectOfType<GenerateMap>();
        gameUI = FindObjectOfType<GameUI>();
        lhd = FindObjectOfType<LeftHandDetector>();
        rhd = FindObjectOfType<RightHandDetector>();

        score = 0;
        rotate = true;
        OnDeath += DissbaleRotation;
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextTime) //условие удержания на одном камне
            OnDeath();

        if (Input.GetMouseButtonDown(0)) //событие клика на Mouse0
        {
            if (lhd.isRockedLeftHand || rhd.isRockedRightHand) //Условие, если кисть находится на камне, то можем менять направление
            {
                direction = !direction; //смена направления
                nextTime = Time.time + timeBetweenClicks; //обновление переменной для перерасчета времени до следущего нажатия
                if(direction)
                    Destroy(Instantiate(crack,lhd.PosTinyLeft,Quaternion.identity),40f);
                else
                    Destroy(Instantiate(crack,rhd.PosTinyRight,Quaternion.identity),40f);

                score++; //увеличение счета на табло сверху
                scoreText.text = score.ToString();

                map.CreateNewPos(); //создание нового камня
            }
            else
            {
                OnDeath(); //если нажатие было по пустой области срабатывает событие
            }
        }
        if (!rotate)
            return;

        if (direction) //изменение вращения вокруг камня
        {
            if (score != 0) //условие для избавления от ошибки при SceneManager.LoadScene, когда запоминается старая координата камня и начинает срабатывать SmoothDamp
            {
                Vector3 newPos1 = lhd.PosTinyLeft - lhd.transform.position; //вычисления координаты, куда нужно переместить нашего персонажа
                transform.position = Vector3.SmoothDamp(transform.position,transform.position + newPos1,ref currentVelocity,0.35f); //смазывание эффекта переммещения кисти в центр камня
            }

            transform.RotateAround(leftHand.position,new Vector3(0,0,10f),Time.deltaTime * 100f * speed); //вращение персонажа
            //переключение коллайдеров кистей
            leftHand.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            rightHand.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }
        else
        {
            if(score!= 0) //условие для избавления от ошибки при SceneManager.LoadScene, когда запоминается старая координата камня и начинает срабатывать SmoothDamp
            {
                Vector3 newPos2 = rhd.PosTinyRight - rhd.transform.position; //вычисления координаты, куда нужно переместить нашего персонажа
                transform.position = Vector3.SmoothDamp(transform.position,transform.position + newPos2,ref currentVelocity,0.35f); //смазывание эффекта переммещения кисти в центр камня
            }

            transform.RotateAround(rightHand.position,new Vector3(0,0,-10f),Time.deltaTime * 100f * speed); //вращение персонажа
            //переключение коллайдеров кистей
            leftHand.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            rightHand.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    //Отмена вращения при смерти
    void DissbaleRotation() 
    {
        rotate = false;
    }
}
