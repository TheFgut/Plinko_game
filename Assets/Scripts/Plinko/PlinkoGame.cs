using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class PlinkoGame : MonoBehaviour
{
    [Header("grid")]
    [SerializeField] private RectTransform gridParent;
    [SerializeField] private GameObject plinkoDotPrefab;
    [SerializeField] private PlinkoWinCell winCellPrefab;
    [SerializeField] private GridScalingModule gridScaling;
    [SerializeField] private int gridPinsCount = 12;

    [Header("balls")]
    [SerializeField] private BallsPool ballsPool;

    private bool _ballLaunchAwailable;
    private PlinkoGrid grid;

    private GameServerApi gameApi;
    private UserWallet wallet;
    private UnityMainThreadActionLauncher mainThreadLauncher;

    public Action<bool> onRolling;

    public bool ballLaunchAwailable { get => _ballLaunchAwailable;
        private set
        {
            if (value != _ballLaunchAwailable) 
            {
                _ballLaunchAwailable = value;
                onBallLaunchAvailabilityChanged?.Invoke(_ballLaunchAwailable);
            }
        } 
    }

    public Action<bool> onBallLaunchAvailabilityChanged;

    [Inject]
    private void Construct(GameServerApi gameApi, UserWallet wallet,
        UnityMainThreadActionLauncher mainThreadLauncher)
    {
        this.gameApi = gameApi;
        this.wallet = wallet;
        this.mainThreadLauncher = mainThreadLauncher;
    }
    async void Start()
    {
        float[] winCoefs = await gameApi.getBallRunRevenueFieldCoeficients(gridPinsCount);
        grid = new PlinkoGrid(gridPinsCount, winCoefs, plinkoDotPrefab, winCellPrefab, gridParent);
        ballsPool.Init(grid.gridFieldTransform);
        gridScaling.Init(gridParent, grid.gridFieldTransform);
        ballLaunchAwailable = true;
    }

    private void Update()
    {
        gridScaling.Update();
    }

    public void tryLaunchBall(float bet)
    {
        if (!wallet.tryDecreaseMoney(bet))
        {
            throw new Exception($"Not enougth money but trying to launch plinko " +
                $"with bet {bet} < wallet money {wallet.GetMoney()}");
        }
        PlinkoBall ball;
        if (!ballsPool.tryGetBallFromPool(out ball))
        {
            Debug.LogError("There is no balls awailable but launch ball was performed");
            return;
        }
        if (ballsPool.hasFreeBall) ballLaunchAwailable = false;

        onRolling?.Invoke(true);
        Task ballLaunchingTask = Task.Factory.StartNew(async () =>
        {
            float moneyRevenue;
            List<int> ballPathIndeces;
            try
            {
                ballPathIndeces = await gameApi.getBallPathFromServer(grid.pinsCount);
                moneyRevenue = await gameApi.getBallRunRevenueFromServer(grid.pinsCount, ballPathIndeces.Last(), bet);
            }
            catch(Exception e)
            {
                Debug.LogError($"Ball launch failed due to exception {e.Message}\n{e.StackTrace}");
                BallReachedDestination(ball, 0, false);
                return;
            }

            try
            {
                PlinkoWinCell winCell;
                List<PlinkoDot> ballPath = grid.CalculatePhysicalPathForBall(ballPathIndeces, out winCell);
                mainThreadLauncher.Enqueue(() =>
                {
                    ball.Activate();
                    ball.transform.anchoredPosition = ballPath[0].dotPos.anchoredPosition;
                    ball.MoveBall(ballPath, winCell, () => BallReachedDestination(ball, moneyRevenue, true));
                });
            }
            catch (Exception e)
            {
                Debug.LogError($"Ball launch anim failed due to exception {e.Message}\n{e.StackTrace}");
                BallReachedDestination(ball, 0, true);
                return;
            }
        });
    }

    public void BallReachedDestination(PlinkoBall ball,float revenue, bool success)
    {
        if(success) 
        {
            wallet.tryIncreaseMoney(revenue);
        }
        ball.Deactivate();
        ballsPool.putBallInPool(ball);
        ballLaunchAwailable = true;

        if (ballsPool.allBallsAreInPool)
        {
            onRolling?.Invoke(false);
        }
    }


}
