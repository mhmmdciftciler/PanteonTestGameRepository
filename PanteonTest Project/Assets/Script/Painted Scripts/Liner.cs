using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class Liner : MonoBehaviour
{
    public GameObject brush;
    public GameObject brushModel;
    public GameObject brushTrail;
    public GameObject wall;
    public RenderTexture renderTexture;
    public GameObject saveButton;
    public GameObject textureCamera;
    public Material paintedMaterial;
    public Material wallMaterial;
    public Slider paintedSurfaces;
    Vector3 brushPosition;
    Vector3[] checkPoints;
    bool[] status;
    int index;
    int painted_value;
    float distance;
    float brush_x;
    float brush_y;
    float brush_z = 4.195f;
    bool mouseButtonDown, mouseButton;
    void Start()
    {
        wall.GetComponent<MeshRenderer>().material = wallMaterial;
        CheckPoints();
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseButtonDown = true;
        }
        else if (Input.GetMouseButton(0))
        {
            mouseButton = true;
        }
        else
            mouseButton = false;
    }
    //Oluşturulan görüntüyü texture olarak kaydedip duvarın materyaline atar.
    private void FixedUpdate()
    {
        if (mouseButtonDown)
        {
            
            brush.SetActive(true);
            brushModel.SetActive(true);
            mouseButtonDown = false;
        }
        else if (mouseButton)
        {
            PaintStatus();
            brush.transform.position = Vector3.MoveTowards(brush.transform.position, BrushPosition(), 0.25f);
        }

    }
    public void SaveImage()
    {
        brushModel.SetActive(false);//Fırca Render kamerada görünmemeli.
        textureCamera.SetActive(true); //Render camera aktif.
        RenderTexture.active = renderTexture; //Render kaydı yapılacak Texture dosyası.
        Texture2D paintTexture2D = new Texture2D(renderTexture.width, renderTexture.height);//Dosyanın fiziksil boyutları.
        paintTexture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);//Kayıt olarak alınacak dosyanın boyutları render dosyasına eşit.
        paintTexture2D.Apply();
        var paintImage = paintTexture2D.EncodeToPNG();//image png formatında.
        File.WriteAllBytes(Application.dataPath + "/Myimage.png", paintImage);//kaydetme yolu
        paintedMaterial.SetTexture("_MainTex", paintTexture2D);//kaydedilmiş dosyanın materyale atanması.
        wall.GetComponent<MeshRenderer>().material = paintedMaterial;//atama yapılmış materyalin duvarın materyali olarak atanması
        textureCamera.SetActive(false);//render cameranın pasifleştirilmesi.Fırça yok edildikten sonra aktif edilmişti...
    }
    //Fare pozisyonunu duvarın alanına uyarlayan fonksiyon.
    float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return Mathf.Clamp(((value - from1) / (to1 - from1) * (to2 - from2) + from2), from2,to2);//Limitleri belirlenmiş bir değişkenin değerini, limitleri belli başka bir değişkene dönüştürür.
    }
    //Kontrol noktaları oluşturur.
    void CheckPoints()
    {
        checkPoints = new Vector3[441];
        status = new bool[441];
        for (float i = -5; i <= 5; i+=0.5f)
        {
            for (float j = -5; j <=5 ; j+=0.5f)
            {
                checkPoints[index] = new Vector3(i, j, 4.195f);
                status[index] = true;
                index++;
            }
        }
    }
    //Kontrol noktalarına belirli bir mesafeyle geçildiğinde, boyanmış yüzeyi bildirir.Her Kontrol noktası bir kere uyarı verir.
    void PaintStatus()
    {
        for (int i = 0; i < index; i++)
        {
            distance = Vector3.Distance(brushPosition, checkPoints[i]);
            if (distance < 0.5f && status[i])
            {
                status[i] = false;
                painted_value++;
                paintedSurfaces.value = painted_value;
            }
        }
    }
    Vector3 BrushPosition()
    {
        brush_x = Remap(Input.mousePosition.x, 100, Screen.width-100, -4.6f, 4.6f);
        brush_y = Remap(Input.mousePosition.y, 100, Screen.height-100, -5.4f, 4.61f);
        brushPosition = new Vector3(brush_x, brush_y, brush_z);
        return brushPosition;
    }
    
}
