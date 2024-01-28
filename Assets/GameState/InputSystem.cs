using System;
using UnityEngine;

public class InputSystem : MonoBehaviour
{

    public Action OnWPressed;
    public Action OnAPressed;
    public Action OnSPressed;
    public Action OnDPressed;
    public Action OnLeftClick;
    public Action OnRightClick;

    // Update is called once per frame
    void Update()
    {
        // Track inputs for wasd and fire an event
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W Pressed");
            OnWPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A Pressed");
            OnAPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S Pressed");
            OnSPressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D Pressed");
            OnDPressed?.Invoke();
        }
        // Track Inputs for the mouse left and right clicks
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Left Click");
            OnLeftClick?.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Right Click");
            OnRightClick?.Invoke();
        }
    }
}
