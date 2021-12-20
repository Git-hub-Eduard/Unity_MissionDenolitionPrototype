using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;// Для взаимодействия с элементами интерфейса

public enum GameMode//Это перечисление созданое для обозначение текущего состояния игры
{
    idle,//ожидание
    playing,//уже играет
    levelEnd// окончание уровня
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;// скрытый объект одиночка для вызова методов

    [Header("Set in Inspector")]
    public Text uitLevel;// Ссылка на объект UIText_Level
    public Text uitShots;// Ссылка на объект UIText_Shots
    public Text uitButton;// Ссылка на дочерний объект Text в UIButton_View
    public Vector3 castlePos; // Местоположение замка 
    public GameObject[] castles;// Масив замков
    [Header("Set Dunamically")]
    public int level;// Текущий уровень
    public int levelMax;//Количество уровней
    public int shotsTaken;// Количество выстрелов
    public GameObject castle;// Текущий замок
    public GameMode mode = GameMode.idle;// состояние игры  - ожидание
    public string showing = "Show Slingshot";// Режым FollowCam

    // Start is called before the first frame update
    void Start()
    {
        S = this;// Определить объект одиночку 
        level = 0;// уровень 0
        levelMax = castles.Length;//определить количество элементов в масиве castles и записать в переменную levelMax
        StartLevel();// функция которая начинает новый уровень
    }
    void StartLevel()//Начать новй уровень
    {
        //Уничтожить прежний замок, если он существует 
        if(castle!= null)//Проверить существует ли замок
        {
            Destroy(castle);//Если замок существует уничтожить его
        }
        //Уничтожить прежние снаряды, если они уществуют
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");//Найти все снаряды и записать в масив gos
        foreach(GameObject pTemp in gos)
        {
            Destroy(pTemp);//уничтожить каждый елмент масива gos
        }
        //Создать новый замок
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;//установить его место положение в уровне
        shotsTaken = 0;// Количество выстрело на старте уровня
        //Переустановить камеру в начальную позицию 
        SwitchView("Show Both");
        ProjectileLine.S.Clear();//Очистить линию траектории снаряда
        //Сбросить цель 
        Goal.goalMeet = false;//При создании нового уровня объявить false - игрок ещё не попал в цель
        UpdateGUI();// Обновить пользовательский интерфейс
        mode = GameMode.playing;// переход в режым игры 
    }
    void UpdateGUI()//Метод для обновления пользовательского интерфейса
    {
        //Показать данные в элементах ПИ
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;//Написать текущий уровень на котором находитса игрок
        uitShots.text = "Shots Taken: " + shotsTaken;//написать количество выстрелов
    }
    // Update is called once per frame
    void Update()
    {
        UpdateGUI();//Обнволять пользовательский интерфейс каждый кадр
        //Проверить завершение уровня
        if ((mode == GameMode.playing) && Goal.goalMeet)// Проверить если игра в режиме playing и если игрок попал цель
        {
            //Изменить режим чтобы прекратить проверку на завершение уровня
            mode = GameMode.levelEnd;// уровень закончен
            //Уменьшить масштаб 
            SwitchView("Show Both");
            //Начать новый уровень через 2 секунды 
            Invoke("NextLevel", 2f);// Перезагрузить уровень через 2 секунды 
        }
    }
    void NextLevel()
    {
        level++;// обновить на какой текущий уровень игры
        if(level == levelMax)// Если уровень равен максимальному количеству уровней 
        {
            level = 0;//Начать с перввого уровня
        }
        StartLevel();// Начать новый уровень 
    }
    public void SwitchView( string eView = "")
    {
        if(eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch(showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;//Тоесть вернуть камеру на изначальную позицию 
                uitButton.text = "Show Castle";// Написать на кнопке соотвецтвующе сообщение
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;//Перенести камеру на замок
                uitButton.text = "Show Both";//Написать на кнопке соотвецтвующе сообщение
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");//Расположить камеру сферху
                uitButton.text = "Show Slingshot";//Написать на кнопке соотвецтвующе сообщение
                break;
        }
    }
    public static void ShortFired()//Статический обще доступный метод, который вызываетса из сценария Slingshot
    {
        S.shotsTaken++;//Обновить количество выстрелов
    }
}
