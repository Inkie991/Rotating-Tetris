using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using DG.Tweening;

public class MovingPlane : MonoBehaviour
{
    private const float NO_BOOST_TRIGGER_OFFSET_X = 0.0113f;
    private const float BOOST_TRIGGER_OFFSET_X = 0.04513f;
    
    private bool _isCollide;

    private Rigidbody _body;
    private int speed;
    
    [SerializeField]
    private List<Transform> barriers = new List<Transform>();
    [SerializeField]
    private List<Transform> triggers = new List<Transform>();
    
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init();
        Figure.StopPlaneEvent += OnTrigger;
        SwipeDetection.LongTouchEvent += OnBoost;
        _isCollide = false;
        _body = transform.GetComponent<Rigidbody>();
        speed = 10;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 380) return;
        CheckTriggers();
        if (!_isCollide)
        {
            MovePlane();
        }
    }

    void MovePlane()
    {
        _body.DOMoveX(transform.position.x + speed, 0.125f, false);
    }

    void OnTrigger()
    {
        if (!_isCollide)
        {
            speed = 10;
            StartCoroutine(Wait());
        }
    }

    void OnBoost()
    {
        if (!_isCollide)
        {
            if (speed == 10)
            {
                speed = speed * 3;
            }
        }
    }

    void CheckTriggers()
    {
        if (speed == 10)
        {
            for (int i = 0; i < barriers.Count; i++)
            {
                triggers[i].localPosition = new Vector3(barriers[i].localPosition.x + NO_BOOST_TRIGGER_OFFSET_X,
                    triggers[i].localPosition.y, triggers[i].localPosition.z);
            }
        }
        else if (speed == 30)
        {
            for (int i = 0; i < barriers.Count; i++)
            {
                triggers[i].localPosition = new Vector3(barriers[i].localPosition.x + BOOST_TRIGGER_OFFSET_X,
                    triggers[i].localPosition.y, triggers[i].localPosition.z);
            }
        }
    }

    private IEnumerator Wait()
    {
        _isCollide = true;
        _body.DOTogglePause();
        _body.DOMoveX(transform.position.x - speed, 0.25f, false);
        yield return new WaitForSeconds(0.25f);
        _body.DOTogglePause();
        _isCollide = false;
    }
}
