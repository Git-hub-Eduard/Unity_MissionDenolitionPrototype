using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;// скрытый статически экземпляр
    // Start is called before the first frame update
    [Header("Set in Inspecor")]
    public GameObject prefabProjectile;// ссылка на шаблон из котрого будут создаватса снаряды
    public float velocityMult = 8f;
    [Header("Set Dynamicaly")]
    public GameObject LaunchPoint;// игровой объект точки LaunchPoint
    public Vector3 LaunchPos;// будет хранить ммировые кординаты LaunchPoint
    public GameObject projectile;//это ссылка на вновь созданый экземпляр Projectile
    public bool aimingMode;// обычно имеет значение лошь правда когда игрок нажимает на кнопку мыши
                           // когда указатль находитса в районе объекта Slingshot
    public Rigidbody projectileRigidbody;// перменная отвечает за физическое состояние объекта,
                                         //в данном случаи делать снаряд кинематическим или нет, придавать ускорению
    static public  Vector3 LAUNCH_POS// статическое обще доступное свойство 
    {
        get
        {
            if (S == null) return Vector3.zero;// если поле S содержит null то возвращать (0.0.0)
            return S.LaunchPos;
        }
    }
    void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        /*
         * transform.Find("LaunchPoint") - найдет объект с именем LaunchPoint и вернет компонент transform
         */
        LaunchPoint = launchPointTrans.gameObject;//получить ссылку на игровой объект и сохранить в LaunchPoint
        LaunchPoint.SetActive(false);// SetActive(false) - этот метод сообщает игре должна ли она игнорировать
        LaunchPos = launchPointTrans.position;// записать координаты точки LaunchPoint в LaunchPos
    }
    void OnMouseEnter()
    {
        //print("Slingshot: OnMouseEnter");
        LaunchPoint.SetActive(true);// SetActive(false) - этот метод сообщает игре должна ли она игнорировать
    }
    void OnMouseExit()
    {
        //print("Slingshot:  OnMouseExit");
        LaunchPoint.SetActive(false);// SetActive(false) - этот метод сообщает игре должна ли она игнорировать
    }
    void OnMouseDown()// вызываетса когда игрок будет нажимать на кнопку мыши в районе объекта Slingshot(в зоне его колайдера)
    {
        //игрок нажал на кнопку мыши когда указатель находитса в районе колайдера
        aimingMode = true;
        //создать снаряд
        projectile = Instantiate(prefabProjectile);//1
        // Поместить в точку LaunchPoint
        projectile.transform.position = LaunchPos;//2
        //Сделать его кинематическим.
        projectileRigidbody = projectile.GetComponent<Rigidbody>();//3
        projectileRigidbody.isKinematic = true;//3
        /*
         * 1. Создаетса снаряд и сохраняет ссылку на него в projectile.
         * 2. Затем projectile помещаетса в точку LaunchPos
         * 3. Объявляем что projectile - кинематическим тоесть не перемещаетса физическим движком.
         */
    }
    void Update()
    {
        //Если рогатка не в режиме прицеливания не выполнять этот код
        if (!aimingMode) return;
        //Получить текущее экранное значение координат мыши
        Vector3 mousePos2D = Input.mousePosition;//1
        mousePos2D.z = -Camera.main.transform.position.z;//1
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);//1
        // найти разность координат между LaunchPos и mousePos3D
        Vector3 mouseDelta = mousePos3D - LaunchPos;//2
        //Ограничить mouseDelta радиусом колайдера объекта Slingshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude>maxMagnitude)// проверить если величина вектора mouseDelta больше ща радиус колайдера рогатки
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;// зделать вектор одинакового радиуса что и радиус колайдера рогатки
        }
        //Передвинуть снаряд в новую позицию
        Vector3 projPos = LaunchPos + mouseDelta;//3
        projectile.transform.position = projPos;//3
        if(Input.GetMouseButtonUp(0))
        {
            // Кнопка мышы отпущена
            projectileRigidbody.isKinematic = false;//4
            projectileRigidbody.velocity = -mouseDelta * velocityMult;//4
            FollowCam.POI = projectile;// передаем ссылку на объект что б камера могла следить за ней
            projectile = null;// чтоб при отпущении кнопки мыши была возможность вызвать новый снаряд
            MissionDemolition.ShortFired();//Обновить количестов выстрелов
            ProjectileLine.S.poi = projectile;//Заставить линию  следовать за новым снарядом после выстрела
        }
        /*
         * Принцып работы:
         * 1. Вычисляем кординаты точки мыши
         * 2. Находим разность
         * 3. в кординаты перемещаем снаряд.
         * 4. Придаем ускорения в противоположном напрвалении и снаряд летит.
         */
    }
}
