using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    // ����������� ���� ������ �������� �� ������ ���� 
    static public bool goalMeet = false;
     void OnTriggerEnter(Collider other)
     {
      //  ����� � ������� �������� ��������� ������� ���-��
      // ���������, �������� �� ��� ��� �� ��������
        if(other.gameObject.tag == "Projectile")
        {
            //���� ��� ������ ��������� ����  goalMeet  = true
            Goal.goalMeet = true;
            // ����� �������� �����-����� �����, ����� ��������� ��������������
            Material mat = GetComponent<Renderer>().material;
            print("�����");
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
