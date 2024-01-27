using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenFade : MonoBehaviour
{
    public Animator animator;
    private int sceneToLoad;
    private bool sameScene;
    void Update()
    {
        //temp controls to test
        if (Input.GetKeyDown(KeyCode.F)) //fade within scene
        {
            FadeOut(true, 0);
        }
        if (Input.GetKeyDown(KeyCode.G)) //fade to another scene
        {
            FadeOut(false, 1);
        }
    }

    public void FadeOutOnly()
    {
        FadeOut();
    }
    public void FadeOut(bool stay = true, int sceneIndex = 0) //bool is whether it stays in the same scene, int is the index for the scene it needs to change to
    {
        if (stay)
        {
            sameScene = true;
        }
        else if (!stay)
        {
            sameScene = false;
            sceneToLoad = sceneIndex;
        }
        animator.SetTrigger("FadeOut");
    }
    public void OnFadeComplete()
    {
        Debug.Log("Fade out complete"); //first animation complete and animation event triggered

        if (!sameScene)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else if (sameScene)
        {
            //Add camera change etc. here
            FadeIn();
        }

    }
    public void FadeIn() //fade back in
    {
        animator.SetTrigger("FadeIn");
    }


}
