using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40; //Число облаков.
    public GameObject cloudPrefab; // Шаблон для облаков
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;// Минимальный масштаб каждого облака
    public float cloudScaleMax = 3;// Максимальный масштаб каждого облака
    public float cloudSpeedMult = 0.5f;// Коэфициент скорости облаков
    private GameObject[] cloudInstance;
    void Awake()
    {
        // Создать масив для хранения всех экземпляров облаков.
        cloudInstance = new GameObject[numClouds];
        //найти родительський игровой объект cloudAchor
        GameObject achor = GameObject.Find("CloudAchor");
        // Создать в цикле заданое количество облаков.
        GameObject cloud;
        for(int i = 0; i<numClouds; i++)
        {
            //Создать экземпляр  cloudPrefab
            cloud = Instantiate<GameObject>(cloudPrefab);
            // Выбрать местоположение для облака
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            // Масштабировать облако
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            // Меньшие облака должны быть ближе к земле (с меньшим scaleU)
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            // Меньшие облака должны быть дальше 
            cPos.z = 100 - 90 * scaleU;
            // Применить полученные значения координат и масштаба к облаку
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            // Сделать облако дочерним по отношению к anchor
            cloud.transform.SetParent(achor.transform);
            // Добавить облако в масив cloudInstance.
            cloudInstance[i] = cloud;
        }
    }
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update()
    {
        // Обойти в цикле все созданные облака
        foreach(GameObject cloud in cloudInstance)
        {
            //Получить масштаб и координаты облака
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            // Увеличить скорость для ближних облаков
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            // Если облако сместилось слишком далеко влево.
            if(cPos.x<= cloudPosMin.x)
            {
                // Переместить его далеко в право
                cPos.x = cloudPosMax.x;
            }
            // Применить новые координаты к облаку
            cloud.transform.position = cPos;
        }
    }
}
