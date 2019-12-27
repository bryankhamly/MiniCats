using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public int _secondsTillStart;
    public TextMeshProUGUI CountdownTimer;

    float timer;

    private void Start()
    {
        timer = _secondsTillStart;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        CountdownTimer.text = ((int)timer).ToString();

        if(timer <= 0)
        {
            CountdownTimer.text = "Loading...";
            SceneManager.LoadScene(1);
        }
    }
}
