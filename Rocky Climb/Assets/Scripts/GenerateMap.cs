using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour {

    public GameObject[] rocksPrefabs; //префабы камней
    public Transform rightHand; //координата правой кисти
    public GameObject coinPrefab; //префаб монеты
    public GameObject crack; //префаб трещины

    List<Vector3> rocksCoord; //Лист с координатами камней

    float maxRadius = 3f;
    
    void Start () {
        Destroy(Instantiate(rocksPrefabs[rocksPrefabs.Length -1],rightHand.position,Quaternion.identity),40f); //создание первого камня
        Destroy(Instantiate(crack,rightHand.position,Quaternion.identity),40f); //создание трещины на месте первого камня

        rocksCoord = new List<Vector3>();
        rocksCoord.Add(rightHand.position);
        for (int i = 0; i < 10; i++) //генерация следующих 10-ти камней
        {
            CreateNewPos(); 
        }
    }

    public void CreateNewPos() //Метод для создания камня
    {
        Vector3 previousPosition = rocksCoord[rocksCoord.Count-1];
        float rnd = Random.Range(-maxRadius,maxRadius);
        int rnd2 = Random.Range(0,3);
        int rnd3 = Random.Range(0,5);

        float secondCoord = Mathf.Sqrt(maxRadius * maxRadius - rnd * rnd); // Вычисление координаты для камня(можно было использовать insideUnitCircle)
        Vector3 position = previousPosition + new Vector3(rnd,secondCoord,0); // Конечная координата камня
        rocksCoord.Add(position); //Добавление в лист координаты
        Destroy(Instantiate(rocksPrefabs[rnd2],position,Quaternion.identity),40f); //Непосредственно создание камня

        if(rnd3 == 2)
        {
            GameObject coin = Instantiate(coinPrefab,position,Quaternion.identity); //Создание монеты с определенной вероятностью на камне
        }
    }
}
