using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    GameObject boy;
    Vector3 firstMousePosition;
    Vector3 targetPosition;
    Camera mainCamera;
    Animator boyAnim;
    Rigidbody boyRigidbody;
    public float speed;
    public GameObject finish;
    bool collisionStatus;
    public GameObject particle;
    public GameObject replayCanvas;
    public GameObject finishCanvas;
    public GameObject rankCanvas;
    public TextMeshProUGUI myRank;
    public GameObject aiParent;
    [Range(1,11)]int rank;
    void Start()//Gerekli atamalar.
    {
        Time.timeScale = 1;
        boy = transform.GetChild(0).gameObject;
        mainCamera = Camera.main;
        boyAnim = boy.GetComponent<Animator>();
        boyAnim.SetBool("idle", true);
        boyRigidbody = gameObject.GetComponent<Rigidbody>();
        collisionStatus = false;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstMousePosition.x = Input.mousePosition.x;
            firstMousePosition.y = Input.mousePosition.y;
        }
        else if (Input.GetMouseButton(0))
        {
            targetPosition.x = Map(Input.mousePosition.x , firstMousePosition.x - Screen.width/4, firstMousePosition.x + Screen.width/4, -4, 4);
            targetPosition.z = Map(Input.mousePosition.y , firstMousePosition.y - Screen.height/4, firstMousePosition.y + Screen.height/4, -1, 1);
            targetPosition.y = transform.position.y;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            targetPosition = new Vector3(transform.position.x, 0, 0);
            
        }
       
        Ranking();
        if (transform.position.z >= finish.transform.position.z+1)
            FinisCanvas();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.CompareTag("TriggerObject")|| collision.gameObject.CompareTag("RotatingObject")|| collision.gameObject.CompareTag("HorizontalObstacle")|| collision.gameObject.CompareTag("HalfDonuts") )
        collisionStatus = true;
    }
    private void OnTriggerExit(Collider collision)
    {
        collisionStatus = false;
    }
    
        
    
    private void FixedUpdate()
    {
        if (!collisionStatus)//Çarpışma durumu gerçekleşmediyse oyuncu hareket edebilir. Gerçekleştiyse oyun durur.
            PlayerMovement();
        else
            LoadReplayCanvas();
        if(Random.Range(0,100)>97)//çok sık efect oluşmasını önlemek adına...
        StartCoroutine(ParticleEfect());
        if (transform.position.y < -7)
        {
            replayCanvas.SetActive(true);
        }
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, Mathf.MoveTowards(mainCamera.transform.position.z,transform.position.z - 10,Time.deltaTime*100));// Kamera pozisyonu oyuncunu z konumuna göre -10 birimlik farkla belirlenmiştir. Kamera oyuncuyu takip eder.
    }

    float Map(float value , float from1, float to1, float from2, float to2)//Limitleri belli bir değişkeni, limitleri bell başka bir değişkene dönüştürür.
    {
         return Mathf.Clamp(((value - from1) / (to1 - from1) * (to2 - from2) + from2), from2, to2);
    }
   
    void PlayerMovement() //Player hareketleri.
    {
       


        if (targetPosition.z != 0)//hedef pozisyonu 0 dan faklı olduğunda
        {
            boyAnim.SetBool("idle", false);//bekleme pozisyonunu pasif yapar. but demektir ki oyuncu yürüme animasyonuna geçmiştir.
            transform.LookAt(new Vector3(targetPosition.x, targetPosition.y, targetPosition.z + transform.position.z));// oyuncunun yönü gideceği yöne döner. Böylece daha gerçekçi bir yönelim olur.
            boyRigidbody.velocity = new Vector3((targetPosition.x - transform.position.x) * Time.deltaTime * speed / 4, -9.81f, (targetPosition.z) * Time.deltaTime * speed);// oyuncunun hızı hedef yönüne göre değiştirilip, değeri inputtan gelen değere ve belirlenen hız çarpanına bağlıdır.

        }
        else
        {
            boyAnim.SetBool("idle", true);//bekleme animasyonu aktif.
            boyRigidbody.velocity = new Vector3(0, -9.81f, 0);
            transform.LookAt(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1));//oyuncu durağan halde ileri bakar.Burada kritik nokta kendi poziyonuna göre bakmasıdır.
        }

        
        

    }
    IEnumerator ParticleEfect()//havai fişek efekti. Burada oyuna renklilik katmak istedim.
    {
        yield return new WaitForSeconds(Random.Range(0.5f,2f));
        GameObject newParticle =Instantiate(particle, new Vector3(Random.Range(-10,11), 15+Random.Range(0,20), finish.transform.position.z), Quaternion.identity);
        Destroy(newParticle, 1);
    }
    void LoadReplayCanvas()
    {
        replayCanvas.SetActive(true);
        Time.timeScale = 0;
    }
    void FinisCanvas()
    {
        finishCanvas.SetActive(true);
        rankCanvas.SetActive(false);
        finishCanvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text= "You are Rank: "+rank.ToString();
        Time.timeScale = 0;
    }
    void Ranking()
    {
        rank = 11;
        for (int i = 0; i < aiParent.transform.childCount; i++)
        {
            if (finish.transform.position.z - aiParent.transform.GetChild(i).transform.position.z >finish.transform.position.z- transform.position.z && finish.transform.position.z - aiParent.transform.GetChild(i).transform.position.z>5)
            {
                rank--;
            }
        }
        myRank.text = rank.ToString();
    }
}
