using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;// ������� ���������� ���������
    // Start is called before the first frame update
    [Header("Set in Inspecor")]
    public GameObject prefabProjectile;// ������ �� ������ �� ������� ����� ���������� �������
    public float velocityMult = 8f;
    [Header("Set Dynamicaly")]
    public GameObject LaunchPoint;// ������� ������ ����� LaunchPoint
    public Vector3 LaunchPos;// ����� ������� �������� ��������� LaunchPoint
    public GameObject projectile;//��� ������ �� ����� �������� ��������� Projectile
    public bool aimingMode;// ������ ����� �������� ���� ������ ����� ����� �������� �� ������ ����
                           // ����� �������� ��������� � ������ ������� Slingshot
    public Rigidbody projectileRigidbody;// ��������� �������� �� ���������� ��������� �������,
                                         //� ������ ������ ������ ������ �������������� ��� ���, ��������� ���������
    static public  Vector3 LAUNCH_POS// ����������� ���� ��������� �������� 
    {
        get
        {
            if (S == null) return Vector3.zero;// ���� ���� S �������� null �� ���������� (0.0.0)
            return S.LaunchPos;
        }
    }
    void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        /*
         * transform.Find("LaunchPoint") - ������ ������ � ������ LaunchPoint � ������ ��������� transform
         */
        LaunchPoint = launchPointTrans.gameObject;//�������� ������ �� ������� ������ � ��������� � LaunchPoint
        LaunchPoint.SetActive(false);// SetActive(false) - ���� ����� �������� ���� ������ �� ��� ������������
        LaunchPos = launchPointTrans.position;// �������� ���������� ����� LaunchPoint � LaunchPos
    }
    void OnMouseEnter()
    {
        //print("Slingshot: OnMouseEnter");
        LaunchPoint.SetActive(true);// SetActive(false) - ���� ����� �������� ���� ������ �� ��� ������������
    }
    void OnMouseExit()
    {
        //print("Slingshot:  OnMouseExit");
        LaunchPoint.SetActive(false);// SetActive(false) - ���� ����� �������� ���� ������ �� ��� ������������
    }
    void OnMouseDown()// ���������� ����� ����� ����� �������� �� ������ ���� � ������ ������� Slingshot(� ���� ��� ���������)
    {
        //����� ����� �� ������ ���� ����� ��������� ��������� � ������ ���������
        aimingMode = true;
        //������� ������
        projectile = Instantiate(prefabProjectile);//1
        // ��������� � ����� LaunchPoint
        projectile.transform.position = LaunchPos;//2
        //������� ��� ��������������.
        projectileRigidbody = projectile.GetComponent<Rigidbody>();//3
        projectileRigidbody.isKinematic = true;//3
        /*
         * 1. ��������� ������ � ��������� ������ �� ���� � projectile.
         * 2. ����� projectile ���������� � ����� LaunchPos
         * 3. ��������� ��� projectile - �������������� ������ �� ������������ ���������� �������.
         */
    }
    void Update()
    {
        //���� ������� �� � ������ ������������ �� ��������� ���� ���
        if (!aimingMode) return;
        //�������� ������� �������� �������� ��������� ����
        Vector3 mousePos2D = Input.mousePosition;//1
        mousePos2D.z = -Camera.main.transform.position.z;//1
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);//1
        // ����� �������� ��������� ����� LaunchPos � mousePos3D
        Vector3 mouseDelta = mousePos3D - LaunchPos;//2
        //���������� mouseDelta �������� ��������� ������� Slingshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude>maxMagnitude)// ��������� ���� �������� ������� mouseDelta ������ �� ������ ��������� �������
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;// ������� ������ ����������� ������� ��� � ������ ��������� �������
        }
        //����������� ������ � ����� �������
        Vector3 projPos = LaunchPos + mouseDelta;//3
        projectile.transform.position = projPos;//3
        if(Input.GetMouseButtonUp(0))
        {
            // ������ ���� ��������
            projectileRigidbody.isKinematic = false;//4
            projectileRigidbody.velocity = -mouseDelta * velocityMult;//4
            FollowCam.POI = projectile;// �������� ������ �� ������ ��� � ������ ����� ������� �� ���
            projectile = null;// ���� ��� ��������� ������ ���� ���� ����������� ������� ����� ������
            MissionDemolition.ShortFired();//�������� ���������� ���������
            ProjectileLine.S.poi = projectile;//��������� �����  ��������� �� ����� �������� ����� ��������
        }
        /*
         * ������� ������:
         * 1. ��������� ��������� ����� ����
         * 2. ������� ��������
         * 3. � ��������� ���������� ������.
         * 4. ������� ��������� � ��������������� ����������� � ������ �����.
         */
    }
}
