using UnityEngine;
using Zenject;
using Logopedia.GamePlay;
using Logopedia.UserInterface;
using Logopedia.UIConnection;

public class ProjectContextInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindManagers();
        BindFactories();
    }

    void BindManagers()
    {
        Container.Bind<ItemsManager>().AsSingle().NonLazy();
        Container.Bind<PopUpManager>().AsSingle().NonLazy();
        Container.Bind<StoryManager>().AsSingle().NonLazy();
    }

    void BindFactories()
    {
        Container.BindFactory<string, Garment, Garment.Factory>().FromFactory<PrefabResourceFactory<Garment>>();
        Container.BindFactory<string, ItemTemplate, ItemTemplate.Factory>().FromFactory<PrefabResourceFactory<ItemTemplate>>();
        Container.BindFactory<string, BGTemplate, BGTemplate.Factory>().FromFactory<PrefabResourceFactory<BGTemplate>>();
        Container.BindFactory<string, CharacterTemplate, CharacterTemplate.Factory>().FromFactory<PrefabResourceFactory<CharacterTemplate>>();
    }

}