using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;

public class FirebaseINIT : MonoBehaviour
{
    public static bool firebaseReady = false; // Tracks Firebase initialization status

    void Start()
    {
        CheckIfReady(); // Initialize Firebase when the scene starts
    }

    void Update()
    {
        // If Firebase is ready, load the next scene
        if (firebaseReady)
        {
            Debug.Log("Firebase is ready, loading LoginScene...");
            SceneManager.LoadScene("signup_scene");
        }
    }

    public static void CheckIfReady()
    {
        Debug.Log("Checking Firebase dependencies...");

        // Check and resolve Firebase dependencies
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance; // Initialize Firebase
                firebaseReady = true; // Mark Firebase as ready
                Debug.Log("Firebase is ready for use.");
            }
            else
            {
                firebaseReady = false; // Firebase not ready
                Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatus}");
            }
        });
    }
}
