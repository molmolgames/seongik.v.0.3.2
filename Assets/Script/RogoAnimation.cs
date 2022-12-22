using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RogoAnimation : MonoBehaviour
{
    private Image image;
    public float time;
    private float timer = 0;
    public Gradient gradient;
    public float time_G;
    private float timer_G;


    private void Start()
    {
        timer_G = time_G * Random.value;
        image = GetComponent<Image>();
        image.color = new Color(1, 1, 1, 0);
    }
    void Update()
    {
        if (timer < time)
        {
            image.color = new Color(1, 1, 1, timer / time);
        }
        else
        {
            if (image)
            {
                timer_G += Time.deltaTime;
                if (timer_G > time_G) timer_G = 0.0f;

                image.color = gradient.Evaluate(timer_G / time_G);
            }
        }
        timer += Time.deltaTime;
      
    }
}
