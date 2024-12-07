using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class LaunchBallButton : MonoBehaviour
{
    [SerializeField] private Button button;
    private PlinkoGame plinkoGame;
    private BettingSystem bettingSys;
    private bool active;
    [Inject]
    private void Construct(PlinkoGame plinkoGame, BettingSystem bettingSys)
    {
        this.plinkoGame = plinkoGame;
        this.bettingSys = bettingSys;
    }

    public void Start()
    {
        button.onClick.AddListener(LaunchBallWithCurrentBetAmountSettings);
        plinkoGame.onBallLaunchAvailabilityChanged += BallLaunchAvailabilityChanged;
    }

    public void LaunchBallWithCurrentBetAmountSettings()
    {
        LaunchBall(bettingSys.GetBetAmount());
    }

    public void LaunchBall(float bet) => plinkoGame.tryLaunchBall(bet);

    private void BallLaunchAvailabilityChanged(bool awailableToLaunch)
    {
        if (awailableToLaunch) ActivateBut();
        else DeactiavateBut();
    }

    public void DeactiavateBut()
    {
        if (!active) return;
        button.interactable = false;
    }

    public void ActivateBut()
    {
        if (active) return;
        button.interactable = true;
    }
}
