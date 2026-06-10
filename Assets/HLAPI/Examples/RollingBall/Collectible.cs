using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HLAPI.Examples
{
    public class Collectible : MonoBehaviour
    {
        public float rotateSpeed = 60f;
        void Update()
        {
            transform.Rotate(Vector3.one * rotateSpeed * Time.deltaTime, Space.World);
        }
    }
}
