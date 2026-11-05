using UnityEngine;

public class BoomController : ItemController
{
    PlayerController playerController;

    protected override void ItemGain()
    {
        base.ItemGain();
        
        playerController = base.player.GetComponent<PlayerController>();
        if (playerController.boom < 3)
        {
            playerController.boom++;
            UIManagerController.instance.CheckBoom(playerController.boom);
        }
        else
        {
            UIManagerController.instance.AddScore(base.score);
        }
    }
}
