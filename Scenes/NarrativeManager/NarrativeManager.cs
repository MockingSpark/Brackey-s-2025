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
	}

	private Fairy fairy;
	private List<FairyAction> actionQueue = new List<FairyAction>();
	private List<FairyAction> savedQueue = new List<FairyAction>();
	private int currentPriority = 0;

	public void RegisterFairy(Fairy fairy)
	{
		this.fairy = fairy;
		fairy.FairyActionDone += SendNewAction;
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
				SendNewAction();
				break;
			case E_FairyAction.Container:
				Debug.Fail("Should not process container");
				break;
			default:
				break;
		}
	}

	//Aller qq part
	//Dire qqch
	//Dire plusieurs choses
	//Suivre le joueur
	//Sauver le joueur (lakitu)
	//Sauver le joueur (enemy)
	//Donner un score
}
