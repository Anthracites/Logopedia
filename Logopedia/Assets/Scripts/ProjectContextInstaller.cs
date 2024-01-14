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
        Container.Bind<SettingsManager>().AsSingle().NonLazy();
        Container.Bind<SpritesManager>().AsSingle().NonLazy();
    }

    void BindFactories()
    {
        Container.BindFactory<string, Garment, Garment.Factory>().FromFactory<PrefabResourceFactory<Garment>>();
        Container.BindFactory<string, ItemTemplate, ItemTemplate.Factory>().FromFactory<PrefabResourceFactory<ItemTemplate>>();
        Container.BindFactory<string, BGTemplate, BGTemplate.Factory>().FromFactory<PrefabResourceFactory<BGTemplate>>();
        Container.BindFactory<string, CharacterTemplate, CharacterTemplate.Factory>().FromFactory<PrefabResourceFactory<CharacterTemplate>>();
        Container.BindFactory<string, StoryCreationPanel, StoryCreationPanel.Factory>().FromFactory<PrefabResourceFactory<StoryCreationPanel>>();
        Container.BindFactory<string, StoryPlayPanel, StoryPlayPanel.Factory>().FromFactory<PrefabResourceFactory<StoryPlayPanel>>();
        Container.BindFactory<string, GarmentForPlay, GarmentForPlay.Factory>().FromFactory<PrefabResourceFactory<GarmentForPlay>>();
        Container.BindFactory<string, TopicIcon, TopicIcon.Factory>().FromFactory<PrefabResourceFactory<TopicIcon>>();
        Container.BindFactory<string, AnimationTemplate, AnimationTemplate.Factory>().FromFactory<PrefabResourceFactory<AnimationTemplate>>();

    }

}