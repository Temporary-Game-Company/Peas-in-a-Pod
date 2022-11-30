using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Room))]
public class RoomAnimation : MonoBehaviour
{
    [SerializeField] Animator roomAnimator;
    Room room;

    void OnEnable()
    {
        room = GetComponent<Room>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
