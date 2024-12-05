using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainSystemsInstaller : MonoInstaller
{
    [SerializeField] private UserWallet wallet;

    public override void InstallBindings()
    {
        Container.BindInstance(wallet);
    }
}
