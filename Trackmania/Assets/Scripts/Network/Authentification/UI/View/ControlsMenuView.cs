using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControlsMenuView : View
{
    public Button nextControls;
    public Button BackButton;
    public List<GameObject> controlsImages = new List<GameObject> ();
    private int currentIndex = 0;

    public override void Initialize()
    {
        foreach (GameObject item in controlsImages)
        {
            item.SetActive(false);
        }
        controlsImages[currentIndex].SetActive(true);

        BackButton.onClick.AddListener(() => ViewManager.ShowLast());
        nextControls.onClick.AddListener(nextGameObject);
            
    }

    private void nextGameObject()
    {
        controlsImages[currentIndex].gameObject.SetActive(false);
        currentIndex = (currentIndex + 1) % controlsImages.Count;
        controlsImages[currentIndex].gameObject.SetActive(true);
    }
}
