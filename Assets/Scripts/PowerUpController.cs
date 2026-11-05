using UnityEngine;

public class PowerUpController : ItemController
{
    PlayerController playerController;

    protected override void ItemGain()
    {
        base.ItemGain();

        playerController = base.player.GetComponent<PlayerController>();
        if (playerController.damage < 3)
        {
            playerController.damage++;
        }
        else
        {
            UIManagerController.instance.AddScore(base.score);
        }
    }
}
