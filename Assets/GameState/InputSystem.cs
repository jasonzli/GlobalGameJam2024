using System;
using UnityEngine;

public class InputSystem : MonoBehaviour
{

    public event Action OnWPressed;
    public event Action OnAPressed;
    public event Action OnSPressed;
    public event Action OnDPressed;
    public event Action OnLeftClick;
    public event Action OnRightClick;
    public event Action OnSpacePressed;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Presssed");
            OnSpacePressed?.Invoke();
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
    

    public Vector2 InputAxisResponse() => new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
}
