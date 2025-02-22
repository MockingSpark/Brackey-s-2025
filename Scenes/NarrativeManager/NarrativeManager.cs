using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using static Godot.RenderingDevice;


public partial class NarrativeManager : Node
{
	public static NarrativeManager Instance { get; private set; }

	public override void _Ready()
	{
		Instance = this;

		GetRessources();
	}

    private Fairy fairy;
	private List<FairyAction> actionQueue = new List<FairyAction>();
	FairyAction lastAction;
	private List<FairyAction> savedQueue = new List<FairyAction>();
	private int currentPriority = 0;

    private int awaitedSignals = 0;

	SceneLoader sceneLoader;

	#region Narrative bools
	FairyActionContainer catchContainer;
    FairyActionContainer protectContainer;
    FairyActionContainer softlockContainer;
    FairyActionContainer notAngryBadEndContainer;
    FairyActionContainer notAngryGoodEndContainer;
    FairyActionContainer angryBadEndContainer;
    #endregion

    private void GetRessources()
    {
		catchContainer = GD.Load<FairyActionContainer>("res://Source/RandomDialogues/CatchDialogues.tres");
		protectContainer = GD.Load<FairyActionContainer>("res://Source/RandomDialogues/ProtectDialogues.tres");
        softlockContainer = GD.Load<FairyActionContainer>("res://Source/RandomDialogues/SoftlockDialogues.tres");
        notAngryBadEndContainer = GD.Load<FairyActionContainer>("res://Source/Actions/Ending/EndRandomNotAngryBad.tres");
        notAngryGoodEndContainer = GD.Load<FairyActionContainer>("res://Source/Actions/Ending/EndRandomNotAngryGood.tres");
    }

    public void RegisterFairy(Fairy fairy)
    {
        this.fairy = fairy;
        fairy.FairyActionError += SendNewAction;
    }

    public void RegisterLoader(SceneLoader loader)
    {
        this.sceneLoader = loader;
    }
    public void ReceiveActions(FairyActionContainer actionContainer, bool flush = false)
	{
		var actionsToAdd = actionContainer.Actions;
		var priority = actionContainer.Priority;
		if(flush)
		{
			actionQueue.Clear();
			savedQueue.Clear();
		}
		if (priority > 0)
		{
			if (currentPriority == 0)
			{
				savedQueue.Clear();
				if (lastAction != null)
				{
					savedQueue.Add(lastAction);
				}
				savedQueue.AddRange(actionQueue);
			}
			actionQueue.Clear();
			actionQueue.AddRange(actionsToAdd);
		}
		else
		{
			actionQueue.AddRange(actionsToAdd);
		}
		currentPriority = priority;
		SendNewAction();
	}

	void SendNewAction()
	{
		if (actionQueue.Count > 0)
		{
			var nextAction = actionQueue[0];
            lastAction = nextAction;
			actionQueue.RemoveAt(0);
			ProcessAction(nextAction);
		}
		else
		{
			if(currentPriority > 0 && savedQueue.Count > 0)
			{
				actionQueue.AddRange(savedQueue);
				currentPriority = 0;
				SendNewAction();
			}
			else
			{
				fairy.NoAction();
				savedQueue.Clear();
				lastAction = null;
			}
		}
	}

	private void ProcessAction(FairyAction action)
	{
		if (!action.IsConditionMet())
		{
			SendNewAction();
			return;
		}

		switch (action.ActionType)
		{
			case E_FairyAction.FocusOnPlayer:
				fairy.FocusOnPlayer(action.ActionTime);
				break;
			case E_FairyAction.FocusOnObject:
				var focusAction = action as FairyActionFocus;
				fairy.FocusOnNode(focusAction.Target, focusAction.FocusOffset, focusAction.ActionTime);
				break;
			case E_FairyAction.SaySpecific:
				var dialogueAction = action as FairyActionDialogue;
				fairy.ReadDialogueAction(dialogueAction.Dialogue, dialogueAction.ActionTime);
				break;
			case E_FairyAction.SayRandom:
				var genericAction = action as FairyActionGenericDialogue;
				var dialogueToPlay = GetRandomDialogue(genericAction.TalkType);
				var dialAction = dialogueToPlay as FairyActionDialogue;
				var dialContainer = dialogueToPlay as FairyActionContainer;
                if (dialAction != null)
					fairy.ReadDialogueAction(dialAction.Dialogue, genericAction.ActionTime);
				if(dialContainer != null)
				{
                    actionQueue.InsertRange(0, dialContainer.Actions);
                    SendNewAction();
                }
				break;
            case E_FairyAction.HideText:
                FairyActionNoText noTextAction = action as FairyActionNoText;
                fairy.HideText(noTextAction.HideFairysBubble, noTextAction.HideInWorldBubbles);
				break;
			case E_FairyAction.CatchPlayer:
				fairy.SavePlayer(action.ActionTime);
				break;
			case E_FairyAction.ProtectPlayer:
				fairy.ProtectPlayer(action.ActionTime);
				break;
			case E_FairyAction.Score:
				fairy.GivePlayerScore(100000, action.ActionTime);
				break;
			case E_FairyAction.LockPlayer:
				var lockAction = action as FairyActionLock;
				fairy.Player.LockPlayer(lockAction.ShouldLock);
				break;
            case E_FairyAction.CameraMoveTarget:
                var cameraTargetAction = action as MoveCameraTargetAction;
                Camera.Instance.UseAttachMode(cameraTargetAction.TargetNode);
                if (cameraTargetAction.ZoomValue >= 0)
                {
                    if (cameraTargetAction.ZoomInRatio)
                    {
                        Camera.Instance.TransitionToZoomRelative(cameraTargetAction.ZoomValue);
                    }
                    else
                    {
                        Camera.Instance.TransitionToZoom(cameraTargetAction.ZoomValue);
                    }
                }
                break;
            case E_FairyAction.CameraMovePlayer:
				var cameraPlayerAction = action as MoveCameraPlayerAction;
				Camera.Instance.UseAttachMode(fairy.Player);
				if(cameraPlayerAction.ZoomValue >= 0)
                {
					if(cameraPlayerAction.ZoomInRatio)
                    {
						Camera.Instance.TransitionToZoomRelative(cameraPlayerAction.ZoomValue);
                    }
					else
                    {
                        Camera.Instance.TransitionToZoom(cameraPlayerAction.ZoomValue);
                    }
                }
				break;
			case E_FairyAction.SpearProduction:
                var spearProdAction = action as FairyActionSpearProduction;
				fairy.AllowSpearProdution(spearProdAction.ShouldAllowSpearProduction);
				break;
			case E_FairyAction.AngerChange:
                var angerAction = action as FairyActionAngerChange;
				fairy.UpdateAnger(angerAction.ChangeAmount);
                break;
            case E_FairyAction.Container:
				//we try to unpack it and we pray

                Debug.Fail("Should not process container");
                break;
			case E_FairyAction.ResetPlayer:
				fairy.Player.ReturnToStart();
                break;
			case E_FairyAction.ResetScene:
                var resetAction = action as ResetSceneAction;
				foreach (var sceneIndex in resetAction.ScenesToReload)
                {
					sceneLoader.ReloadScene(sceneIndex);
                }
                break;
			default:
				break;
        }
        WaitForNext(action.ActionTime);
    }


	async void WaitForNext(float actionTimer)
    {
        if (actionTimer < 0)
            return;
        if (actionTimer == 0)
        {
			SendNewAction();
            return;
        }
        awaitedSignals++;
        await ToSignal(CreateTween().TweenInterval(actionTimer), Tween.SignalName.Finished);
		if(awaitedSignals == 1)
        {
            SendNewAction();
        }
        awaitedSignals--;
    }

	public FairyAction GetRandomDialogue(E_FairyTalkType dialogueType)
	{
		int randomIndex = 0;
		switch (dialogueType)
		{
			case E_FairyTalkType.Save:
				randomIndex = GD.RandRange(0, catchContainer.Actions.Length-1);
				return catchContainer.Actions[randomIndex];
			case E_FairyTalkType.Protect:
                randomIndex = GD.RandRange(0, protectContainer.Actions.Length-1);
                return protectContainer.Actions[randomIndex];
            case E_FairyTalkType.Softlock:
                randomIndex = GD.RandRange(0, softlockContainer.Actions.Length-1);
                return softlockContainer.Actions[randomIndex];
            case E_FairyTalkType.EndGameNotAngryBad:
                randomIndex = GD.RandRange(0, notAngryBadEndContainer.Actions.Length - 1);
                return notAngryBadEndContainer.Actions[randomIndex];
            case E_FairyTalkType.EndGameNotAngryGood:
                randomIndex = GD.RandRange(0, notAngryGoodEndContainer.Actions.Length - 1);
                return notAngryGoodEndContainer.Actions[randomIndex];
			case E_FairyTalkType.EndGameAngryBad:
                randomIndex = GD.RandRange(0, angryBadEndContainer.Actions.Length - 1);
                return angryBadEndContainer.Actions[randomIndex];
            default:
				return null;
		}
	}
}
