using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void back_to_menu_scene()//menuscene
    {
        SceneManager.LoadScene("MenuScene");
    }
    public void back_to_choice_scene()//choicescene
    {
        SceneManager.LoadScene("choice_scene");
    }

    public void ShowAbout()//aboutscene
    {
        SceneManager.LoadScene("about_scene");
    }

    public void servicesGame()//website
    {
        // Display about information
        Debug.Log("About button clicked.");
        Application.OpenURL("https://tryvisi.netlify.app/about");
        // You can load a separate "About" scene or show info using UI elements
    }

    public void show_furniture()//choice1
    {
        SceneManager.LoadScene("furniture_choice");
    }

    public void show_automobile()//choice2
    {
        SceneManager.LoadScene("automobile_choice");
    }

    public void show_buisnesscard()//choice3
    {
        SceneManager.LoadScene("buisnesscard_choices");
    }

    public void show_electronics()//choice4
    {
        SceneManager.LoadScene("electronics_choice");
    }

    public void show_homedecor()//choice5
    {
        SceneManager.LoadScene("homedecor_scene");
    }

    public void show_arbook()//choice6
    {
        SceneManager.LoadScene("arbook_scene");
    }
    public void menu2scene()//explain1
    {
        SceneManager.LoadScene("menu3scene");  
    }

    public void menu3scene()//explain2
    {
        SceneManager.LoadScene("menu4scene");  
    }

    public void cart_scene()//user cart
    {
        SceneManager.LoadScene("SampleScene"); 
    }

    public void car_scene()//game2
    {
        SceneManager.LoadScene("car_sample");
    }

    public void comming_soon()
    {
        SceneManager.LoadScene("comming_soon_scene");
    }

    public void signin()
    {
        SceneManager.LoadScene("signindetail_scene");
    }

    public void signup()
    {
        SceneManager.LoadScene("register_scene");
    }

    public void main()
    {
            SceneManager.LoadScene("signup_scene");     
    }

}