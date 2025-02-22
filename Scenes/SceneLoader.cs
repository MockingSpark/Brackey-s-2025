using Godot;
using System;
using System.Collections.Generic;

public partial class SceneLoader : Node2D
{
    [Export]
    public PackedScene[] scenesToLoad;

    List<LoadedScene> LoadedScenes = new List<LoadedScene>();

    public override void _Ready()
    {
        LoadScenes();
    }

    async void LoadScenes()
    {
        for (int i = 0; i < scenesToLoad.Length; i++)
        {
            LoadScene(i);
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        }
    }
    void LoadScene(int sceneToLoad)
    {
        var newScene = scenesToLoad[sceneToLoad].Instantiate<Node2D>();
        AddChild(newScene);
        LoadedScenes.Add(new LoadedScene(sceneToLoad, newScene));
    }

    void UnloadScene(int sceneToUnload)
    {
        int unloadIndex = LoadedScenes.FindIndex(x => x.id == sceneToUnload);
        LoadedScenes[unloadIndex].scene.QueueFree();
        LoadedScenes.RemoveAt(unloadIndex);
    }

    public void ReloadScenes()
    {
        for (int i = 0; i < LoadedScenes.Count; i++)
        {
            UnloadScene(i);
        }
        CallDeferred("LoadScenes");
    }

    public void ReloadScene(int sceneToReload)
    {
        UnloadScene(sceneToReload);
        CallDeferred("LoadScene", sceneToReload);
    }
}

public struct LoadedScene
{
    public int id;
    public Node2D scene;

    public LoadedScene(int id, Node2D scene)
    {
        this.id = id;
        this.scene = scene;
    }
}
