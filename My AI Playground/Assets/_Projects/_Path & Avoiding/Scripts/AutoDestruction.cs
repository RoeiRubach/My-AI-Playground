using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruction : MonoBehaviour
{
    [SerializeField]
    private float _lifeSpan = 2.0f;

    private void Start()
    {
        Destroy(gameObject, _lifeSpan);
    }
}
