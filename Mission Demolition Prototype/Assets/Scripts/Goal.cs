using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    // Статическое поле кторое доступно из любого кода 
    static public bool goalMeet = false;
     void OnTriggerEnter(Collider other)
     {
      //  Когда в область действия колайдера попадет что-то
      // Проверить, являетса ли это что то снарядом
        if(other.gameObject.tag == "Projectile")
        {
            //Если это снаряд присвоить полю  goalMeet  = true
            Goal.goalMeet = true;
            // Также изменить альфа-канал цвета, чтобы увеличить непрозрачность
            Material mat = GetComponent<Renderer>().material;
            print("шарик");
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
