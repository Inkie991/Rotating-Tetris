using UnityEngine;
using DG.Tweening;

public class RotationContrloller : MonoBehaviour
{
    public static event OnRotationStarted RotationStartedEvent;
    public delegate void OnRotationStarted();
    
    public static event OnRotationEnded RotationEndedEvent;
    public delegate void OnRotationEnded();
    
    private Vector3 expectedVector;
    
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        SwipeDetection.SwipeEvent += OnSwipe;
        
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localRotation.eulerAngles == expectedVector)
        {
            expectedVector = Vector3.left;
            RotationEndedEvent();
        }
    }

    void OnSwipe(Vector2 direction)
    {
        RotationStartedEvent();
        
        UpdateExpectedVector(direction);
        
        if (direction == Vector2.down || direction == Vector2.up) return;
        
        Vector3 newRotation = Vector3.zero;
        newRotation.y = GetNextY(direction);

        transform.DORotate(newRotation,0.5f, RotateMode.WorldAxisAdd);
    }

    int GetNextY(Vector2 direction)
    {
        int rotationDir = 0;
        
        if (direction == Vector2.left) rotationDir = 1;
        else if (direction == Vector2.right) rotationDir = -1;
        
        int nextY = 90 * rotationDir;
        nextY = Mathf.RoundToInt(nextY / 10) * 10;
        
        return nextY;
    }

    void UpdateExpectedVector(Vector2 direction)
    {
        expectedVector = Vector3.zero;
        Vector3 oldRotation = transform.localRotation.eulerAngles;
        int rotationDir = 0;
        
        if (direction == Vector2.left) rotationDir = 1;
        else if (direction == Vector2.right) rotationDir = -1;

        float temp = oldRotation.y + (90 * rotationDir);
        int expectedY = Mathf.RoundToInt(temp / 10) * 10;

        if (expectedY == -90) expectedY = 270;
        if (expectedY == 360) expectedY = 0;
        
        expectedVector = new Vector3(0, expectedY, 0);
    }
    
}
