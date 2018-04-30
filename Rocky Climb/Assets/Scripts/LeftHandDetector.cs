using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftHandDetector : MonoBehaviour {

    public Text coinText; //префаб текста для количества собранных монет

    public Vector3 PosTinyLeft; //позиция камня на котором держится кисть

    public bool isRockedLeftHand; //переменная для определения нахождения ладони на камне

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Rock")
        {
            isRockedLeftHand = true;
            PosTinyLeft = collision.transform.position;
        }
        if (collision.gameObject.tag == "Coin")
        {
            StartCoroutine(ScoreCoin(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Rock")
        {
            isRockedLeftHand = false;
        }
    }

    IEnumerator ScoreCoin(Collider2D collision) //Метод для добавления собранных монет
    {
        yield return new WaitForSeconds(0.3f); //Задержка
        if (FindObjectOfType<PlayerController>().direction)
        {
            int scoreCoin = Convert.ToInt32(coinText.text); // Получение текущего значения количества монет
            scoreCoin++;
            coinText.text = scoreCoin.ToString(); //изменение текста количества собранных монет
            Destroy(collision.gameObject); //Уничтожение собранной монеты
        }
    }
}
