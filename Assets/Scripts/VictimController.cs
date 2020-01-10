using System.Collections;
using UnityEngine;

public class VictimController : MonoBehaviour
{
    private static Hooks hookController;
    private static PlayerDrownController playerDrownController;
    private static IInteractible sceneLoader;
    private Task playVictim;

    private void Start()
    {
        hookController = FindObjectOfType<Hooks>();
        playerDrownController = FindObjectOfType<PlayerDrownController>();
        sceneLoader = GetComponent<IInteractible>();

        playVictim = new Task(PlayVictim());
    }
    
    IEnumerator PlayVictim()
    {
        for (int i = 0; i < 3; i++)
        {
            hookController.SetupNextPig();
            yield return new WaitForSeconds(1);
            playerDrownController.SetGoingDown(true);
            yield return new WaitForSeconds(3);
            if (i < 2)
            {
                playerDrownController.SetGoingDown(false);
                yield return new WaitForSeconds(3);
                playerDrownController.Tilt();
                yield return new WaitForSeconds(5);
            }
        }
        yield return new WaitForSeconds(3);
        sceneLoader.Interact();
    }
}
