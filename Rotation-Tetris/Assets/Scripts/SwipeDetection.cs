using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public static event OnSwipeInput SwipeEvent;
    public delegate void OnSwipeInput(Vector2 direction);
    
    public static event OnLongTouchInput LongTouchEvent;
    public delegate void OnLongTouchInput();

    private Vector2 tapPosition;
    private Vector2 swipeDelta;

    private float deadZone = 80;

    private bool _isSwiping;
    private bool _isRotating;
    
    void Start()
    {
        RotationContrloller.RotationStartedEvent += OnRotStarted;
        RotationContrloller.RotationEndedEvent += OnRotEnded;
    }

    void Update()
    {
        if (_isRotating) return; 
        if (Input.touchCount > 0)
        {
            TapOrLongTounch();
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _isSwiping = true;
                tapPosition = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Canceled ||
                     Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                ResetSwipe();
            }

            if (Input.GetTouch(0).deltaTime > 1.5f)
            {
                
            }
        }
        
        CheckSwipe();
    }

    private void CheckSwipe()
    {
        swipeDelta = Vector2.zero;

        if (_isSwiping)
        {
            if (Input.touchCount > 0) swipeDelta = Input.GetTouch(0).position - tapPosition;
        }

        if (swipeDelta.magnitude > deadZone)
        {
            if (SwipeEvent != null)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                {
                    SwipeEvent(swipeDelta.x > 0 ? Vector2.right : Vector2.left);
                }
                else
                    SwipeEvent(swipeDelta.y > 0 ? Vector2.up : Vector2.down);
            }
            
            ResetSwipe();
        }
    }
    
    private void ResetSwipe()
    {
        _isSwiping = false;
        
        tapPosition = Vector2.zero;
        swipeDelta = Vector2.zero;
    }

    private void OnRotStarted()
    {
        _isRotating = true;
    }

    private void OnRotEnded()
    {
        _isRotating = false;
    }
    
    float pressTime = 0;

    void TapOrLongTounch()
    {
        if (Input.touchCount <= 0) return;
    
        Touch touch = Input.GetTouch(0);

        switch(touch.phase)
        {
            case TouchPhase.Began:
                pressTime = 0;
                break;

            case TouchPhase.Stationary:
                pressTime += Time.deltaTime;
                break;
        }
        
        if (pressTime > 0.5f)
        {
            LongTouchEvent();
            pressTime = 0;
        }
    }
}
