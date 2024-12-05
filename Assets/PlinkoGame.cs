using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

public class PlinkoGame : MonoBehaviour
{
    [Header("grid")]
    [SerializeField] private RectTransform gridParent;
    [SerializeField] private GameObject PlinkoDotPrefab;
    [SerializeField] private GridScalingModule gridScaling;

    [Header("balls")]
    [SerializeField] private BallsPool ballsPool;

    private bool _ballLaunchAwailable;
    private PlinkoGrid grid;

    private GameServerApi gameApi;
    private UserWallet wallet;
    private UnityMainThreadActionLauncher mainThreadLauncher;

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
    void Start()
    {
        grid = new PlinkoGrid(12, PlinkoDotPrefab, gridParent);
        ballsPool.Init(grid.gridFieldTransform);
        gridScaling.Init(gridParent, grid.gridFieldTransform);
        ballsPool.Init(grid.gridFieldTransform);
        ballLaunchAwailable = true;
    }

    public void tryLaunchBall()
    {
        PlinkoBall ball;
        if (!ballsPool.tryGetBallFromPool(out ball))
        {
            Debug.LogError("There is no balls awailable but launch ball was performed");
            return;
        }
        if (ballsPool.hasFreeBall) ballLaunchAwailable = false;

        Task ballLaunchingTask = Task.Factory.StartNew(async () =>
        {
            float moneyRevenue;
            List<int> ballPathIndeces;
            try
            {
                ballPathIndeces = await gameApi.getBallPathFromServer(grid.pinsCount);
                moneyRevenue = await gameApi.getBallRunRevenueFromServer(grid.pinsCount, ballPathIndeces.Last());
            }
            catch(Exception e)
            {
                Debug.LogError($"Ball launch failed due to exception {e.Message}\n{e.StackTrace}");
                BallReachedDestination(ball, 0, false);
                return;
            }
            try
            {
                List<PlinkoDot> ballPath = grid.CalculatePhysicalPathForBall(ballPathIndeces);
                mainThreadLauncher.Enqueue(() =>
                {
                    ball.MoveBall(ballPath, () => BallReachedDestination(ball, moneyRevenue, true));
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
        if(success) { }
        ballsPool.putBallInPool(ball);
        ballLaunchAwailable = true;
    }


}
