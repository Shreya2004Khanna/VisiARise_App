using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

public class FurniturePlacementManager : MonoBehaviour
{
    public GameObject SpawnableFurniture; // Selected furniture object
    private List<GameObject> placedObjects = new List<GameObject>();

    public ARSessionOrigin sessionOrigin;
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    private List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private GameObject selectedObject = null;
    private bool isManipulating = false;

    public Button floorButton, wallButton, ceilingButton;
    private string selectedMode = "floor";

    public GameObject previewObject;
    private bool isPreviewActive = false;

    void Start()
    {
        floorButton.onClick.AddListener(() => SetMode("floor"));
        wallButton.onClick.AddListener(() => SetMode("wall"));
        ceilingButton.onClick.AddListener(() => SetMode("ceiling"));

        if (previewObject != null)
        {
            previewObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touchZero = Input.GetTouch(0);

            if (touchZero.phase == TouchPhase.Began && !IsButtonPressed())
            {
                if (raycastManager.Raycast(touchZero.position, raycastHits, TrackableType.PlaneWithinPolygon))
                {
                    PlaceObject();
                }
            }
            else if (isManipulating && touchZero.phase == TouchPhase.Moved)
            {
                DragObject(touchZero.position);
            }
            else if (touchZero.phase == TouchPhase.Ended)
            {
                isManipulating = false;
            }
            else if (Input.touchCount == 2 && selectedObject != null)
            {
                Touch touchOne = Input.GetTouch(1);
                if (touchOne.phase == TouchPhase.Moved)
                {
                    RotateObjectWithTwoFingers(touchZero, touchOne);
                }
            }
        }

        if (isPreviewActive && previewObject != null)
        {
            UpdatePreview();
        }
    }

    private void SetMode(string mode)
    {
        selectedMode = mode;
        Debug.Log("Selected Mode: " + mode);
    }

    public void SwitchFurniture(GameObject newFurniture)
    {
        SpawnableFurniture = newFurniture;
        Debug.Log("Switched Furniture to: " + newFurniture.name);
    }

    private void PlaceObject()
    {
        if (SpawnableFurniture == null)
        {
            Debug.LogError("No furniture selected!");
            return;
        }

        Pose hitPose = raycastHits[0].pose;
        GameObject placedObject = Instantiate(SpawnableFurniture, hitPose.position, hitPose.rotation);

        if (selectedMode == "wall")
            placedObject.transform.rotation = Quaternion.Euler(0, 0, 90);
        else if (selectedMode == "ceiling")
            placedObject.transform.rotation = Quaternion.Euler(180, 0, 0);

        placedObjects.Add(placedObject);
        selectedObject = placedObject;
        isManipulating = true;
    }

    private void UpdatePreview()
    {
        if (raycastManager.Raycast(Input.mousePosition, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = raycastHits[0].pose;
            previewObject.transform.position = hitPose.position;
            previewObject.transform.rotation = hitPose.rotation;
            previewObject.SetActive(true);
        }
    }

    private void DragObject(Vector2 touchPosition)
    {
        if (raycastManager.Raycast(touchPosition, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = raycastHits[0].pose;
            selectedObject.transform.position = hitPose.position;
        }
    }

    private void RotateObjectWithTwoFingers(Touch firstTouch, Touch secondTouch)
    {
        float rotationAngle = (secondTouch.deltaPosition.x - firstTouch.deltaPosition.x) * 0.5f;
        selectedObject.transform.Rotate(Vector3.up, -rotationAngle);
    }

    public void RemoveLastObject()
    {
        if (placedObjects.Count > 0)
        {
            Destroy(placedObjects[^1]);
            placedObjects.RemoveAt(placedObjects.Count - 1);
            selectedObject = null;
            isManipulating = false;
        }
    }

    private bool IsButtonPressed()
    {
        return EventSystem.current.currentSelectedGameObject?.GetComponent<Button>() != null;
    }

    public void TogglePlaneDetection(bool state)
    {
        planeManager.enabled = state;
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(state);
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
