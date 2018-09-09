using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using com.input;

namespace com.bonucyballs
{
    public enum DirectionAxis
    {
        X,
        Y
    }

    public class PlatformMover : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 0.3f;

        [SerializeField]
        private DirectionAxis directionAxis;

        private Vector3 pos1;
        private Vector3 pos2;

        Vector3 newPos;
        private void Awake()
        {
            if(directionAxis == DirectionAxis.X)
            {
                pos1 = new Vector3(transform.position.x, transform.position.y, 0);
                pos2 = new Vector3(transform.position.x + 0.5f, transform.position.y, 0);
            }
            else
            {
                pos1 = new Vector3(transform.position.x, transform.position.y, 0);
                pos2 = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
            }
        }

        private void Update()
        {
            
            transform.position = Vector3.Lerp(pos1, pos2, Mathf.PingPong(Time.time * moveSpeed, 1f));
        }
    }
}
