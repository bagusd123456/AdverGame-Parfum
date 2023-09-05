//#define DEFAULTCODE
#define CODEv2

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using Lean.Common;
using Lean.Touch;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class DragAndDrop : MonoBehaviour
{
    public Vector3 defaultPosition;

    private Transform lastParent;
    public int objectIndex;
    public float hitArea = 70f;
    public bool isDragging = true;
    public LeanFinger currentFinger;
    private LeanSelectableByFinger selectable;

    #region Default Code
    #if DEFAULTCODE
    private void Awake()
    {
        canvas = gameObject.transform.parent.GetComponentInParent<Canvas>();
    }
    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
    #region OnMouseButton3D
        /*if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                //Select stage    
                if (hit.transform.CompareTag("Grid"))
                {
                    Debug.Log(hit.transform.gameObject.name);
                }
            }
        }*/
    #endregion

    #region OnMouseButton2D
        /*RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
            Debug.Log(rayHit.transform.name);

        }*/
    #endregion
    }

    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerdata = (PointerEventData)data;

        Vector2 position;
        /*RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform, pointerdata.position,
            canvas.worldCamera, out position);*/

        position = Input.mousePosition;

        gameObject.GetComponentInParent<HorizontalLayoutGroup>().enabled = false;
        transform.position = position;
    }

    public void DropHandler(BaseEventData data)
    {
        gameObject.GetComponentInParent<HorizontalLayoutGroup>().enabled = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));

		if (rayHit.transform)
		{
            GridData gridData = rayHit.transform.gameObject.GetComponent<GridData>();
            CardData cardData = gameObject.GetComponent<Card>().cardDataSO;

            if (rayHit && rayHit.transform.gameObject.CompareTag("Grid") && gridData != null && cardData != null)
            {
                if (rayHit.collider.GetType() == typeof(BoxCollider2D))
                {
                    int gridTypeID = (int)gridData._gridType;
                    int cardTypeID = (int)cardData._cardType;

                    if (gridTypeID == 0 && cardTypeID == 0) //Check front grid for Monster card only
                    {
                        //gameObject.GetComponentInParent<HorizontalLayoutGroup>().enabled = true;
                        //transform.SetParent(rayHit.transform);
                        transform.position = rayHit.transform.position;
                        transform.SetParent(GameObject.Find("Board").GetComponent<Transform>());
                        Debug.Log("On Hit: " + rayHit.transform.name);
                    }

                    else if(gridTypeID == 1 && (cardTypeID == 1 || cardTypeID == 2)) //Check back grid for Magic and Spell card only
                    {
                        //gameObject.GetComponentInParent<HorizontalLayoutGroup>().enabled = true;
                        //transform.SetParent(rayHit.transform);
                        transform.position = rayHit.transform.position;
                        transform.SetParent(GameObject.Find("Board").GetComponent<Transform>());
                        Debug.Log("On Hit: " + rayHit.transform.name);
                    }
                        
                }
                else if (rayHit.collider.GetType() != typeof(BoxCollider2D)) //Put Card back to position if didn't hit Grid
                {
                    transform.position = defaultPosition;
                    Debug.Log("Cancel");
                }
                else //Put Card back to position
                {
                    transform.position = defaultPosition;
                    Debug.Log("Cancel");
                }
            }
            else {
                transform.position = defaultPosition; //Put Card back to position
                Debug.Log("Cancel");
            }
		}
		else {
            transform.position = defaultPosition; //Put Card back to position if Mouse didn't hit anything
        }
    }

    private void OnDrawGizmos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        //RaycastHit2D[] rayHitArray = Physics2D.GetRayIntersectionAll(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (rayHit)
        {
            //Debug.Log(rayHit.transform.name);
        }

        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red);
    }
#endif
    #endregion

    #region CODEv2
#if CODEv2
    private void Awake()
    {
        selectable = GetComponent<LeanSelectableByFinger>();

        defaultPosition = transform.position;
    }

    private void Start()
    {
        selectable.Use = LeanSelectableByFinger.UseType.AllFingers;
        selectable.OnSelected.AddListener(x =>
        {
            SelectHandler(selectable.SelectingFinger);
        });
        selectable.OnSelectedFingerUp.AddListener(DropHandler);
    }

    private void OnEnable()
    {
        //LeanTouch.OnFingerDown += SelectHandler;
        //LeanTouch.OnFingerUpdate += DragHandler;
        //LeanTouch.OnFingerUp += DropHandler;

        //selectable.OnSelected.AddListener(x=>
        //{
        //    DragHandler();
        //});

    }

    private void OnDisable()
    {
        //LeanTouch.OnFingerDown -= SelectHandler;
        //LeanTouch.OnFingerUpdate -= DragHandler;
        //LeanTouch.OnFingerUp -= DropHandler;
    }

    // Update is called once per frame
    void Update()
    {
        if (lastParent != null)
        {
            transform.position = lastParent.position;
        }
        #region OnMouseButton3D
        /*if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                //Select stage    
                if (hit.transform.CompareTag("Grid"))
                {
                    Debug.Log(hit.transform.gameObject.name);
                }
            }
        }*/
        #endregion
        #region OnMouseButton2D
        RaycastHit2D hit2D = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        #endregion

        isDragging = selectable.IsSelected;
    }

    public void SelectHandler(LeanFinger finger)
    {
        Debug.Log($"Finger: {finger.Index} Selecting");
        isDragging = true;
        //currentFinger = finger;
        if (isDragging)
        {
            //objectIndex = transform.GetSiblingIndex();
        }

    }

    public void DragHandler(LeanFinger finger)
    {
        if (isDragging)
        {
            //transform.SetParent(GameObject.Find("Canvas").transform);

            //for (int i = 0; i < gridDataList.Count; i++)
            //{
            //    float distance = Vector2.Distance(gameObject.transform.position, gridDataList[i].transform.position);
            //    if (distance <= 90f)
            //    {
            //        //gridData = gridDataList[i].transform.GetComponent<GridData>();
            //        //Debug.Log("Current GD: " + gridDataList[i].name);
            //    }
            //}

            /*if (gridData != null)
            {
                float distanceFromClosest = Vector2.Distance(gameObject.transform.position, gridData.transform.position);
                //Debug.Log(distanceFromClosest);
                if (distanceFromClosest >= 100f)
                {
                    gridData = null;
                }
            }*/

            Vector3 targetPos = Camera.main.ScreenToWorldPoint(finger.ScreenPosition);
            targetPos.z = 0;
            transform.position = targetPos;
        }
    }

    public void DropHandler(LeanFinger finger)
    {
        if (!isDragging) return;

        isDragging = false;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(currentFinger.ScreenPosition);
        targetPos.z = 0;
        transform.position = targetPos;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(gridData.transform.position, hitArea);
    }
#endif
    #endregion
}
