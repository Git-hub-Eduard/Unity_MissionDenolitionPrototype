using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;// ������ �� ������ �� ������ ����� �������
    [Header("Set in Inspector")]
    public float easing = 0.05f;// ��� ���������� ������������� ��� ������������ �����������, �� ������� ������ ����� �����������
                                // � �������� ������� � ����� ������ ����������� �� 5%.
    public Vector2 minXY = Vector2.zero;// ����������� ��������� minXY ���������� (0;0) ��� ���� � ������ �������������
    [Header("Set Dynamically")]
    public float camZ;// ��������� ���������� Z ������
    // Start is called before the first frame update
    void Awake()
    {
        camZ = this.transform.position.z;//���������� ���������� Z ������ � ���������� camZ
    }
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        Vector3 destination;
        //���� ��� ������������� ������� ������� ���������� ������ (0.0.0)
        if(POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //�������� ������� ������������ ������� 
            destination = POI.transform.position;
            //���� ������������ ������  - ������, �������� ��� �� �����������
            if(POI.tag== "Projectile")
            {
                //���� �� ����� �� ����� ������ �� ���������
                if(POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    //������� �������� ���������  ���� ������ ������
                    POI = null;
                    //� ��������� �����
                    return;
                }
            }
        }
        //���������� ������������ ���������� ���� � ������
        destination.x = Mathf.Max(minXY.x, destination.x);// ����� Mathf.Max �������� ������������ ����� minXY.x � destination.x
        destination.y = Mathf.Max(minXY.y, destination.y);// ����� Mathf.Max �������� ������������ ����� minXY.� � destination.�
        /*
         * ����� ������ ���� ����������� �� ��������� ���������� ������ � ������������� ����������
         * ��� ���� ����������� �� ��������� �������� ������ ���� �=0 ��� ����� �=0.
         */
        destination = Vector3.Lerp(transform.position, destination, easing);// ���������� ������� ��������� ����� 
        destination.z = camZ;
        /*
         * ��� ��� ��� �� ����� ���������� ������ �������� � �������,
         * �� ������������� �������������  ���������� Z ������
         */
        transform.position = destination;// �������� ������ � ���������� destination, ������ ���������� ������ � ������������� ��� �������.
        //�������� ������ orthographicSize ������ ��� � ����� ���������� � ���� ������
        Camera.main.orthographicSize = destination.y + 10;//���� ���������� orthographicSize ������������� ���� ��������� ������.
    }
}
