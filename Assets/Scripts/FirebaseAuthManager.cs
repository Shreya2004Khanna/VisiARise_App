using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions; // To handle async tasks

public class FirebaseAuthManager : MonoBehaviour
{
    // Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    // Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    // Registration variables
    [Header("Registration")]
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;

    [Header("Scene Management")]
    public string menuSceneName = "MenuScene"; // Name of the scene to load after login

    private void Awake()
    {
        Debug.Log("Awake: Checking Firebase dependencies...");
        // Check and initialize Firebase dependencies
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Awake: Firebase dependencies are available.");
                InitializeFirebase();
            }
            else
            {
                Debug.LogError($"Awake: Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("InitializeFirebase: Initializing Firebase Authentication...");
        // Initialize Firebase Auth
        auth = FirebaseAuth.DefaultInstance;

        // Listen for authentication state changes
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != null)
        {
            user = auth.CurrentUser;
            Debug.Log($"AuthStateChanged: User signed in: {user.Email}");
        }
        else
        {
            user = null;
            Debug.Log("AuthStateChanged: User signed out.");
        }
    }

    public void Login()
    {
        string email = emailLoginField.text;
        string password = passwordLoginField.text;

        Debug.Log($"Login: Attempting to log in with Email: {email}");

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Login: Email or Password is empty.");
            return;
        }

        StartCoroutine(LoginAsync(email, password));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        Debug.Log($"LoginAsync: Initiating login for Email: {email}");
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            Debug.LogError($"LoginAsync: Login failed for {email}. Exception: {loginTask.Exception}");
            HandleAuthError(loginTask.Exception);
        }
        else
        {
            user = loginTask.Result.User;
            Debug.Log($"LoginAsync: Login Successful! Welcome {user.Email}");
            SceneManager.LoadScene(menuSceneName); // Load the MenuScene
        }
    }

    public void Register()
    {
        string name = nameRegisterField.text;
        string email = emailRegisterField.text;
        string password = passwordRegisterField.text;
        string confirmPassword = confirmPasswordRegisterField.text;

        Debug.Log($"Register: Attempting to register user with Email: {email}");

        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Register: Name, Email, or Password is empty.");
            return;
        }

        if (password != confirmPassword)
        {
            Debug.LogError("Register: Passwords do not match.");
            return;
        }

        StartCoroutine(RegisterAsync(name, email, password));
    }

    private IEnumerator RegisterAsync(string name, string email, string password)
    {
        Debug.Log($"RegisterAsync: Attempting to create user with Email: {email}");
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            Debug.LogError($"RegisterAsync: Registration failed for {email}. Exception: {registerTask.Exception}");
            HandleAuthError(registerTask.Exception);
        }
        else
        {
            user = registerTask.Result.User;
            Debug.Log($"RegisterAsync: Registration Successful! Welcome {user.DisplayName}");

            // Set the user's display name after registration
            UserProfile profile = new UserProfile { DisplayName = name };
            var profileUpdateTask = user.UpdateUserProfileAsync(profile);
            yield return new WaitUntil(() => profileUpdateTask.IsCompleted);

            if (profileUpdateTask.Exception != null)
            {
                Debug.LogError("RegisterAsync: Profile update failed.");
                Debug.LogError(profileUpdateTask.Exception);
            }
            else
            {
                Debug.Log($"RegisterAsync: Profile updated for user {user.DisplayName}");
            }
        }
    }

    private void HandleAuthError(System.AggregateException exception)
    {
        FirebaseException firebaseEx = exception.GetBaseException() as FirebaseException;
        if (firebaseEx != null)
        {
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
            string message = $"Authentication Failed: {firebaseEx.Message} (Error Code: {firebaseEx.ErrorCode})";

            Debug.LogError(message);

            switch (errorCode)
            {
                case AuthError.InvalidEmail:
                    message += " - Invalid Email.";
                    break;
                case AuthError.WrongPassword:
                    message += " - Wrong Password.";
                    break;
                case AuthError.MissingEmail:
                    message += " - Email Missing.";
                    break;
                case AuthError.MissingPassword:
                    message += " - Password Missing.";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message += " - Email already in use.";
                    break;
                default:
                    message += " - Unknown Error.";
                    break;
            }

            Debug.LogError(message);
        }
        else
        {
            Debug.LogError("Firebase Exception: Unable to parse exception.");
        }
    }
}
