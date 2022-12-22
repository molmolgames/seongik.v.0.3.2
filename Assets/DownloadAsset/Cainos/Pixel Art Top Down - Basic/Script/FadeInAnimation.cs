using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAnimation : MonoBehaviour
{
    public Image image;
    public float time;
    private float timer = 0;

    private void Start()
    {
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

        }
        timer += Time.deltaTime;
    }
}
