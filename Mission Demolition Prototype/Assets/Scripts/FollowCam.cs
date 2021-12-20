using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;// ссылка на объект за кторым будем следить
    [Header("Set in Inspector")]
    public float easing = 0.05f;// эта переменна€ придназначена дл€ установлени€ приближени€, на сколько камера может приближатса
                                // к текущему объекту в даном случае постанвлено на 5%.
    public Vector2 minXY = Vector2.zero;// присваивает перемнной minXY координаты (0;0) дл€ икса и игрика соотвецтвенно
    [Header("Set Dynamically")]
    public float camZ;// стартова€ координата Z камеры
    // Start is called before the first frame update
    void Awake()
    {
        camZ = this.transform.position.z;//записываем координату Z камеры в переменную camZ
    }
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        Vector3 destination;
        //≈сли нет интересующего объекта вернить координаты камеры (0.0.0)
        if(POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //ѕолучить позицию интереующего объекта 
            destination = POI.transform.position;
            //≈сли интересующий объект  - снар€д, убедитса что он остановилс€
            if(POI.tag== "Projectile")
            {
                //≈сли он стоит на месте тоесть не двигаетса
                if(POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    //¬ернуть исходные настройки  пол€ зрени€ камеры
                    POI = null;
                    //в следующем кадре
                    return;
                }
            }
        }
        //ќграничить минимальными занчени€ми икса и игрика
        destination.x = Mathf.Max(minXY.x, destination.x);// метод Mathf.Max находить максимальное среди minXY.x и destination.x
        destination.y = Mathf.Max(minXY.y, destination.y);// метод Mathf.Max находить максимальное среди minXY.у и destination.у
        /*
         * “акие своего рода ограничени€ не позвол€ют перемещать камеру в отрицательние координати
         * это даст возможность не допустить движение камери ниже ”=0 или ливее ’=0.
         */
        destination = Vector3.Lerp(transform.position, destination, easing);// установить среднее расто€ние между 
        destination.z = camZ;
        /*
         * “ак как нам не нужно приближать камеру вплотную к снар€ду,
         * мы принудительно устанавливаем  координата Z камеры
         */
        transform.position = destination;// ѕомещаем камеру в координаты destination, тоесть перемещаем камеру к интересующему нас объекту.
        //изменить размер orthographicSize камеры что б земл€ оставалась в поле зрени€
        Camera.main.orthographicSize = destination.y + 10;//путЄм увелечени€ orthographicSize увеличиваетса зона видимости камеры.
    }
}
