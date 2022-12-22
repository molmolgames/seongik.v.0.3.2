    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageColorAnimation : MonoBehaviour
{
    //animate the sprite color base on the gradient and time
    private Image image;
    public Gradient gradient;
    public float time;

    private float timer;

    private void Start()
    {
        timer = time * Random.value;
        image = GetComponent<Image>();
    }

    private void Update()
    {
        if (image)
        {
            timer += Time.deltaTime;
            if (timer > time) timer = 0.0f;

            image.color = gradient.Evaluate(timer / time);
        }
    }

}
