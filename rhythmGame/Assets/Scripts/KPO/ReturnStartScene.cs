using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnStartScene : MonoBehaviour
{
    public TextMeshProUGUI Score;

    private void Start()
    {
        if(Score != null)
        {
            Score.text = GameManager.Instance.score.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}
