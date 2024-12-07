using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class LowMoneyModalControler : MonoBehaviour
{
    [SerializeField] private float AppearTreshold = 100;
    [SerializeField] protected LowMoneyModal lowMoneyModal;

    private UnityMainThreadActionLauncher mainThreadActionLauncher;
    private PlinkoGame game;
    private UserWallet userWallet;

    [Inject]
    private void Construct(UserWallet userWallet, PlinkoGame game,
        UnityMainThreadActionLauncher mainThreadActionLAuncher)
    {
        this.userWallet = userWallet;
        this.game = game;
        this.mainThreadActionLauncher = mainThreadActionLAuncher;
    }

    private void Start()
    {
        game.onRolling += (rolling) => mainThreadActionLauncher.Enqueue(async () => await OnBallsRolling(rolling));
    }

    public async Task OnBallsRolling(bool rolling)
    {

        if (!rolling)
        {
            await Task.Delay(1000);
            float currentMoney = userWallet.GetMoney();
            if(currentMoney < AppearTreshold)
            {
                bool refillMoney = await lowMoneyModal.OpenModalAndAskForMoneyRefill();
                if(refillMoney)
                {
                    userWallet.tryIncreaseMoney(500);
                }
                lowMoneyModal.CloseModal();
            }
        }
    }
}
