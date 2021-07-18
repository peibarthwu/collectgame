using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class gamemanager : MonoBehaviour
{
    public Text scoreText;
    public Image img;

    // Update is called once per frame
    public void Lose()
    {   
        var tempColor = img.color;
        tempColor.a = 0.4f;
        img.color = tempColor;
        scoreText.text = "SPORE LOCKED";
        StartCoroutine(LoadMenu());

    }
    public void Win()
    {
        var tempColor = img.color;
        tempColor.a = 0.4f;
        img.color = tempColor;
        scoreText.text = "SPORE UNLOCKED.";
        StartCoroutine(LoadMenu());

    }


    IEnumerator LoadMenu()
    { 
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("StartScene");
    }

}
