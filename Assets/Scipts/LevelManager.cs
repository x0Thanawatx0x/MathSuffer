using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    public void T_lv1()
    {
        SceneManager.LoadScene("Level_1_IT");
    }
    public void T_lv2()
    {
        SceneManager.LoadScene("Level_2_IT");
    }
    public void T_lv3()
    {
        SceneManager.LoadScene("Level_3_IT");
    }

    public void T_lv4()
    {
        SceneManager.LoadScene("Level_4_IT");
    }

    public void T_lv5()
    {
        SceneManager.LoadScene("Level_5_IT");
    }

    public void T_lv6()
    {
        SceneManager.LoadScene("Level_6_IT");
    }

    public void A_lv1()
    {
        SceneManager.LoadScene("Level_1_An");
    }

    public void A_lv2()
    {
        SceneManager.LoadScene("Level_2_An");
    }

    public void A_lv3()
    {
        SceneManager.LoadScene("Level_3_An");
    }

    public void A_lv4()
    {
        SceneManager.LoadScene("Level_4_An");
    }

    public void A_lv5()
    {
        SceneManager.LoadScene("Level_5_An");
    }

    public void A_lv6()
    {
        SceneManager.LoadScene("Level_6_An");
    }






    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

