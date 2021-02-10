using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIScripts : MonoBehaviour
{
    NavMeshAgent navMesh;
    GameObject newTarget;
    public GameObject[] AllMobileObjects;// Tüm hareketli objelerin dizisi.
    public GameObject finishPoint;// Bitiş noktası. Burada oyun objesi olmasının sebebi ekstra bir fonksiyon yazmak yerine sonraki hedefi hep bitişe götürmek amaçlıdır.
    GameObject ItsMe;//Beeen Beeen!!
    Animator aiAnim;
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        aiAnim = transform.GetChild(0).GetComponent<Animator>();
        ItsMe = finishPoint;
        navMesh.destination = finishPoint.transform.position;
        navMesh.isStopped = true;
        Invoke("StartMove", 0.5f);//Başlar başlamaz hareketin başlaması istenmeyen birşeydir. Oyun başladıktan 1 sn. sonra hareket başlar.
    }
    private void Update()
    {
        if (navMesh.velocity.z > 0.2f)//hıza bağlı animasyon algoritması
        {
            aiAnim.SetBool("run", true);
        }
        else
            aiAnim.SetBool("run", false);
   

    }

    void FixedUpdate()
    {
        newTarget = WhoAreYou(transform);// AI önündeki nesnenin atanması.
        if (newTarget.CompareTag("Finish"))//Hedef bitiş ise.
        {
            navMesh.destination = finishPoint.transform.position;
        }
        else
        {

            if (newTarget.CompareTag("HalfDonuts"))//Önündeki nesne donut ise
                PassItHalfDonut(newTarget);
            else if (newTarget.CompareTag("HorizontalObstacle"))//Önündeki nesne horizontal obstacle ise
                PassItHorOb(newTarget);
            else if (newTarget.CompareTag("RotatingObject"))//Önündeki nesne dönen çubuk ise
                PassRotStick(newTarget);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("HorizontalObstacle") || other.CompareTag("HalfDonuts") || other.CompareTag("RotatingObject") || other.CompareTag("TriggerObject"))//algılanan şartlar varsa yapay zeka baştan başlar.
        {
            navMesh.Warp(new Vector3(0, 0, 0));
            navMesh.destination = finishPoint.transform.position;
        }
        


    }
    void PassItHorOb(GameObject HorOb)//objenin konumuna bağlı olarak beklemeyi veya geçmeyi kontrol eder.
    {
        if(HorOb.transform.position.z - transform.position.z < 5 && HorOb.transform.position.z - transform.position.z >0 && Mathf.Abs(HorOb.transform.position.x - transform.position.x) < 2) // Engelin pozisyonuna bağlı olarak durma.
        {
            navMesh.destination = new Vector3(HorOb.transform.position.x, transform.position.y, HorOb.transform.position.z-4);
        }
        else
            navMesh.destination = finishPoint.transform.position;

    }
    void PassItHalfDonut(GameObject donut)//objenin konumuna bağlı olarak beklemeyi veya geçmeyi kontrol eder.
    {
        
        if (donut.transform.GetChild(0).transform.rotation.y == -180 && donut.transform.GetChild(0).transform.position.z-transform.position.z<5.5f && donut.transform.GetChild(0).transform.position.z - transform.position.z >0f)
        {
            if (donut.transform.GetChild(0).transform.position.x >= 0.1f)
            {
                navMesh.destination = new Vector3(4.5f, transform.position.y, donut.transform.position.z-2);

            }
            else if(donut.transform.GetChild(0).transform.position.x <- 5f)
            {

                navMesh.destination = finishPoint.transform.position;
            }
                
            
        }
        else if(donut.transform.GetChild(0).transform.rotation.y == 0 && donut.transform.GetChild(0).transform.position.z - transform.position.z < 5.5f && donut.transform.GetChild(0).transform.position.z - transform.position.z > 0f)
        {
            if (donut.transform.GetChild(0).transform.position.x <=- 0.1f)
            {
                navMesh.destination = new Vector3(-4.5f, transform.position.y, donut.transform.position.z-2);
            }
            else if (donut.transform.GetChild(0).transform.position.x > 5f )
            {
                navMesh.destination = finishPoint.transform.position;
            }
        }
    }
    void PassRotStick(GameObject stick)//objenin konumuna bağlı olarak beklemeyi veya geçmeyi kontrol eder.
    {
        if (stick.transform.position.z - transform.position.z <= 6 && stick.transform.position.x <0  && stick.transform.position.z - transform.position.z >0) // Dönen çubuk soldadır.
        {
            if (stick.transform.GetChild(0).transform.rotation.y >=-90 && stick.transform.rotation.y <=0)
            {
                navMesh.destination = finishPoint.transform.position;
            }
            else
                navMesh.destination = new Vector3(0f, transform.position.y, stick.transform.position.z - 5f);
        }
        else if(stick.transform.position.z - transform.position.z <= 6 && stick.transform.position.x - transform.position.z > 0 && stick.transform.position.z - transform.position.z > 0) // Dönen çubuk sağdadır.
        {
            if (stick.transform.GetChild(0).transform.rotation.y >= -90 && stick.transform.rotation.y <=0)
            {
                navMesh.destination = finishPoint.transform.position;
            }
            else
                navMesh.destination = new Vector3(0, transform.position.y, stick.transform.position.z - 5f);
        }
        

    }
    public GameObject WhoAreYou(Transform MyTransform)// Önümde kim var?
    {
        for (int i = 0; i < AllMobileObjects.Length; i++)// Objelerde gezinme.
        {
            if (AllMobileObjects[i].transform.position.z - MyTransform.position.z <= 6 && AllMobileObjects[i].transform.position.z - MyTransform.position.z > 0)// Yapay zekanın kendi ile döngüdeki obje arası mesafe belirlenen yakınlıkta ise ve gerisindeki obje olmamalı...
            {
                ItsMe = AllMobileObjects[i];// obje yapay zekanın önündedir. Yani geçilmesi gereken obje biliniyordur.
            }
            
        }
        return ItsMe;// Obje gönderilir.
    }
    void StartMove()
    {
        navMesh.isStopped = false;
    }
    
}

