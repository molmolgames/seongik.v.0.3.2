using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public GameManger gameManager;
    public ParticleSystem Particle;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject go = Instantiate(Particle).gameObject;
            go.transform.position = transform.position;
        }
    }
    public void PortalMoveButton()
    {
        GameManger.StageIndex = 1;
        gameManager.PortalMessagePanel.SetActive(false);
        gameManager.SceneSave();
        SceneManager.LoadScene(GameManger.StageIndex);
    }
   public void PortalCancelButton()
    {
        gameManager.PortalMessagePanel.SetActive(false);
        gameManager.scanObject.SetActive(true);
    }
}
