using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

    public Image fadePlane; //переход к черному экрану при "смерти"
    public GameObject gameoverUI; //префаб UI для события "смерть"
    public GameObject player; //префаб персонажа

    void Start()
    {
        FindObjectOfType<PlayerController>().OnDeath += OnGameOver; //добавление метода событию
    }

    public void OnGameOver()
    {
        StartCoroutine(Fade(Color.clear,Color.black,2)); //запуск коурутины
        player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic; //добавление физики персонажу при "смерти"
        gameoverUI.SetActive(true); //запуск UI
    }

    IEnumerator Fade(Color from,Color to,float time) //Метод для погасания экрана
    {
        float speed = 1 / time;
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from,to,percent);
            yield return null;
        }
        yield return null;
    }

    //UI input
    public void StartNewGame() //Метод для обработки события по нажатию кнопки "Play Again" при "смерти"
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Рестарт сцены
    }
}
