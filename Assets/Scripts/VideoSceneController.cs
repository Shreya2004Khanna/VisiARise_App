using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoSceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the Video Player
    public string nextSceneName; // Name of the scene to load after the video

    void Start()
    {
        // Set the video player to play
        videoPlayer.Play();

        // Start the coroutine to wait for the video to finish
        StartCoroutine(WaitForVideo());
    }

    private System.Collections.IEnumerator WaitForVideo()
    {
        // Wait for the video to finish playing
        yield return new WaitForSeconds((float)videoPlayer.length); // Use the length of the video

        // Optionally, you can wait for a specific duration instead of video length
        // yield return new WaitForSeconds(3f);

        // Load the next scene
        SceneManager.LoadScene(nextSceneName);
    }
}
