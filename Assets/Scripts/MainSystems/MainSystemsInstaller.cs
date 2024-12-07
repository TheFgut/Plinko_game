using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainSystemsInstaller : MonoInstaller
{
    [SerializeField] private UserWallet wallet;
    [SerializeField] private GameServerApi gameServerApi;
    [SerializeField] private PlinkoGame game;
    [SerializeField] private UnityMainThreadActionLauncher mainThreadLauncher;
    [SerializeField] private BettingSystem bettingSys;

    public override void InstallBindings()
    {
        Container.BindInstance(wallet);
        Container.BindInstance(gameServerApi);
        Container.BindInstance(game);
        Container.BindInstance(mainThreadLauncher);
        Container.BindInstance(bettingSys);
    }
}
