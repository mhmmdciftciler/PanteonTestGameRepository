using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject MappCanvas;
    public GameObject MainMenuCanvas;
    public GameObject PauseCanvas;
    public GameObject RankCanvas;
    public GameObject PaintCanvas;
    Animator mainCameraAnim;
    public GameObject Script;
    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ReturnMainManu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 1;
        }
        else
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }

    }

    public void Mapp()
    {
        MainMenuCanvas.SetActive(false);
        MappCanvas.SetActive(true);
    }
    public void BackMenu()
    {
        MappCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
    }
    public void GameScene1()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void GameScene2()
    {
        SceneManager.LoadScene(2);
        Time.timeScale = 1;
    }
    public void GameScene3()
    {
        SceneManager.LoadScene(3);
        Time.timeScale = 1;
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        RankCanvas.SetActive(false);
        PauseCanvas.SetActive(true);
        if (SceneManager.GetActiveScene().buildIndex == 3)
            PaintCanvas.SetActive(false);
    }
    public void ContaineGame()
    {
        Time.timeScale = 1;
        PauseCanvas.SetActive(false);
        RankCanvas.SetActive(true);
        if (SceneManager.GetActiveScene().buildIndex == 3)
            PaintCanvas.SetActive(true);


    }
    public void ItCanPaint()
    {
        Time.timeScale = 1;
        mainCameraAnim = Camera.main.GetComponent<Animator>();
        mainCameraAnim.SetBool("itCanPaint", true);
        Invoke("InvBrushTrue", 2);


    }
    public void ItCantPaint()
    {
        mainCameraAnim = Camera.main.GetComponent<Animator>();
        mainCameraAnim.SetBool("itCanPaint", false);
        Script.SetActive(false);
        Invoke("InvBrushFalse", 2);
        PaintCanvas.SetActive(false);

    }
    void InvBrushFalse()
    {
        Script.SetActive(false);
        SceneManager.LoadScene(0);
    }
    void InvBrushTrue()
    {
        Script.SetActive(true);
        PaintCanvas.SetActive(true);
        GameObject.Find("AnimCanvas").SetActive(false);

    }
}
