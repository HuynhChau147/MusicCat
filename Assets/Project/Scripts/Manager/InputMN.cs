using UnityEngine;
using System;
using System.Collections.Generic;

public class InputMN : MonoBehaviour
{
    public static InputMN Instance;
    public Action<int, float> OnLaneMove;
    // int = lane, float = targetX
    Dictionary<int, int> fingerLaneMap = new Dictionary<int, int>();

    private int mouseLane;
    private bool isMouseHolding;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (GameplayMN.Instance.StateController.CurrentState != GameState.Playing) return;

#if UNITY_EDITOR
        HandleMouse();
#else
        HandleTouch();
#endif
        HandleKeyboard();
    }

    void HandleMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseLane = GetLaneFromScreenPos(Input.mousePosition);
            isMouseHolding = true;
        }

        if (Input.GetMouseButton(0) && isMouseHolding)
        {
            float worldX = ScreenToWorldX(Input.mousePosition);

            OnLaneMove?.Invoke(mouseLane, worldX);
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMouseHolding = false;
            mouseLane = -1;
        }
    }

    void HandleTouch()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                int lane = GetLaneFromScreenPos(touch.position);

                if (IsLaneOccupied(lane)) return;

                fingerLaneMap[touch.fingerId] = lane;
            }

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (!fingerLaneMap.ContainsKey(touch.fingerId)) continue;

                int lane = fingerLaneMap[touch.fingerId];
                float worldX = ScreenToWorldX(touch.position);

                OnLaneMove?.Invoke(lane, worldX);
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                fingerLaneMap.Remove(touch.fingerId);
            }
        }
    }

    void HandleKeyboard()
    {
        float moveSpeed = 10f; // tốc độ di chuyển
        float delta = Time.deltaTime;

        // Lane 0 (A / D)
        if (Input.GetKey(KeyCode.A))
        {
            MoveLaneByKeyboard(0, -moveSpeed * delta);
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveLaneByKeyboard(0, moveSpeed * delta);
        }

        // Lane 1 (← / →)
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLaneByKeyboard(1, -moveSpeed * delta);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveLaneByKeyboard(1, moveSpeed * delta);
        }
    }

    void MoveLaneByKeyboard(int lane, float deltaX)
    {
        float currentX = GetCurrentLaneX(lane);

        float targetX = currentX + deltaX;

        OnLaneMove?.Invoke(lane, targetX);
    }

    float GetCurrentLaneX(int lane)
    {
        return GameplayMN.Instance.GetLaneXPos(lane);
    }

    float ScreenToWorldX(Vector2 screenPos)
    {
        Vector3 world = Camera.main.ScreenToWorldPoint(
            new Vector3(screenPos.x, screenPos.y, 10f));

        return world.x;
    }

    int GetLaneFromScreenPos(Vector2 screenPos)
    {
        float screenMid = Screen.width * 0.5f;
        Debug.LogError("lane input: " + ((screenPos.x < screenMid) ? 0 : 1));
        return (screenPos.x < screenMid) ? 0 : 1;
    }

    bool IsLaneOccupied(int lane)
    {
        return fingerLaneMap.ContainsValue(lane);
    }
}