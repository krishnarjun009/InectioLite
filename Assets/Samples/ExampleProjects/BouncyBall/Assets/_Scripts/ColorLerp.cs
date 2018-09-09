using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace com.bonucyballs
{
    public class ColorLerp : MonoBehaviour
    {
        [SerializeField]
        private float speed = 5f;

        private SpriteRenderer img;
        private Color startColor = Color.white;
        private Color endColor = Color.yellow;

        private float startTime;
        float timeLeft;

        Color[] colors = new Color[4];

        Color targetColor;

        void Start()
        {
            startTime = Time.time;
            img = GetComponent<SpriteRenderer>();

            colors[0] = Color.red;
            colors[1] = Color.yellow;
            colors[2] = Color.cyan;
            colors[3] = Color.red;

            targetColor = colors[UnityEngine.Random.Range(0, colors.Length)];
        }
        float t=1;
        void Update()
        {
            img.material.color = Color.Lerp(img.material.color, colors[UnityEngine.Random.Range(0, colors.Length)], t);

            if (t < 1)
            { // while t below the end limit...
                Debug.Log("Hii");
              // increment it at the desired rate every update:
                t += Time.deltaTime / speed;
            }
        }
    }
}
