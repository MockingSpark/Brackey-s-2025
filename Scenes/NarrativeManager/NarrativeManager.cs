using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;


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
	private List<FairyAction> savedQueue = new List<FairyAction>();
	private int currentPriority = 0;

    private int awaitedSignals = 0;

	#region Narrative bools
	FairyActionContainer catchContainer;
    FairyActionContainer protectContainer;
    FairyActionContainer softlockContainer;
    #endregion

    private void GetRessources()
    {
		catchContainer = GD.Load<FairyActionContainer>("res://Source/RandomDialogues/CatchDialogues.tres");
		protectContainer = GD.Load<FairyActionContainer>("res://Source/RandomDialogues/ProtectDialogues.tres");
        softlockContainer = GD.Load<FairyActionContainer>("res://Source/RandomDialogues/SoftlockDialogues.tres");
    }

    public void RegisterFairy(Fairy fairy)
	{
		this.fairy = fairy;
		fairy.FairyActionError += SendNewAction;
	}

	public void ReceiveActions(FairyActionContainer actionContainer, bool flush = false)
	{
		var actionsToAdd = actionContainer.Actions;
		var priority = actionContainer.Priority;
		if(flush)
		{
			actionQueue.Clear();
			savedQueue.Clear();
			currentPriority = priority;
		}
		if (priority > 0)
		{
			actionQueue.Clear();
			actionQueue.AddRange(actionsToAdd);
			currentPriority = priority;
		}
		else
		{
			savedQueue.Clear();
			savedQueue.AddRange(actionsToAdd);
			actionQueue.AddRange(actionsToAdd);
			currentPriority = 0;
		}
		SendNewAction();
	}

	void SendNewAction()
	{
		if (actionQueue.Count > 0)
		{
			var nextAction = actionQueue[0];
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
			}
		}
	}

	private void ProcessAction(FairyAction action)
	{
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
				Dialogue dialogueToPLay = GetRandomDialogue(genericAction.TalkType);
				fairy.ReadDialogueAction(dialogueToPLay, genericAction.ActionTime);
				break;
			case E_FairyAction.HideText:
				fairy.HideText();
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
            case E_FairyAction.Container:
				Debug.Fail("Should not process container");
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

	public Dialogue GetRandomDialogue(E_FairyTalkType dialogueType)
	{
		int randomIndex = 0;
		switch (dialogueType)
		{
			case E_FairyTalkType.Save:
				randomIndex = GD.RandRange(0, catchContainer.Actions.Length-1);
				return ((FairyActionDialogue)catchContainer.Actions[randomIndex]).Dialogue;
			case E_FairyTalkType.Protect:
                randomIndex = GD.RandRange(0, protectContainer.Actions.Length-1);
                return ((FairyActionDialogue)protectContainer.Actions[randomIndex]).Dialogue;
            case E_FairyTalkType.Softlock:
                randomIndex = GD.RandRange(0, softlockContainer.Actions.Length-1);
                return ((FairyActionDialogue)softlockContainer.Actions[randomIndex]).Dialogue;
            default:
				return null;
		}
	}
}
