using UnityEngine;
using UnityEngine.UI;
using Zenject;


public class LaunchBallButton : MonoBehaviour
{
    [SerializeField] private Button button;
    private PlinkoGame plinkoGame;
    private bool active;
    [Inject]
    private void Construct(PlinkoGame plinkoGame)
    {
        this.plinkoGame = plinkoGame;
    }

    public void Start()
    {
        button.onClick.AddListener(LaunchBall);
        plinkoGame.onBallLaunchAvailabilityChanged += BallLaunchAvailabilityChanged;
    }

    public void LaunchBall()
    {
        plinkoGame.tryLaunchBall();
    }

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
