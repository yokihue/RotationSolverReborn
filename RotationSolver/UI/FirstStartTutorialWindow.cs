using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using ECommons.DalamudServices;
using RotationSolver.Data;

namespace RotationSolver.UI;

internal sealed class FirstStartTutorialWindow : Window
{
	private const ImGuiWindowFlags BaseFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoResize;
	private int _stepIndex;

	private static readonly string[] StarterMacros =
	[
		"/rotation Settings AoEType Full\r\n/rotation Auto",
		"/rotation Settings AoEType Cleave\r\n/rotation Manual",
		"/rotation Off",
	];

	private static readonly TutorialStep[] Steps =
	[
		new(
			"Welcome!",
			"This walkthrough explains how to set up Rotation Solver Reborn and what each section controls and includes recommended macros.",
			Bullets:
			[
				"Open the config with /rotation or the plugin UI button.",
				"Use Next/Back to move through sections and apply changes as you go.",
				"Most settings are safe to change while logged in, but avoid in-combat tweaks until you’re comfortable.",
				"Right-click any setting or action label to copy its macro command."
			]),
		new(
			"Main Screen",
			"Main is your overview hub: plugin info, compatibility info, links, and macro list live here.",
			RotationConfigWindowTab.Main,
			[
				"Use this tab to verify incompatible plugins and open support links.",
				"Read the macros section to learn quick chat commands.",
				"If something breaks after an update, check this tab first."
			]),
		new(
			"Job Settings",
			"Job config controls rotation selection and job-specific options for your current class.",
			RotationConfigWindowTab.Job,
			[
				"Pick the rotation preset you want to run by clicking the rotation name (ie. Reborn).",
				"Adjust job priorities (e.g., DNC partner, SGE Kardia) when applicable.",
				"If a job feels off, start here before touching global settings."
			]),
		new(
			"Actions",
			"Actions config decides what abilities RSR can use and how they behave.",
			RotationConfigWindowTab.Actions,
			[
				"Click an action icon in a category to see settings to enable/disable it or change its usage rules.",
				"Use intercept if you want RSR to fire actions you queue manually.",
				"Toggle cooldown window inclusion so overlays show only what you want."
			]),
		new(
			"Auto",
			"Auto controls global action usage, AoE logic, interrupts, tinctures, and healing behavior.",
			RotationConfigWindowTab.Auto,
			[
				"Here you can adjust your AOE logic, (Off, Cleave, and Full).",
				"Adjust healer thresholds and non-healer support options.",
				"If you want a more conservative rotation, tighten these settings first."
			]),
		new(
			"Basic",
			"Basic contains core timing and automation behaviors that affect all jobs.",
			RotationConfigWindowTab.Basic,
			[
				"Action Ahead affects weave count and clipping—smaller values = more oGCDs. You typically don't need to change this.",
				"Min Updating Time trades performance for responsiveness.",
				"Auto Switch controls when RSR turns on/off automatically (countdowns, deaths, duty events)."
			]),
		new(
			"UI",
			"UI controls overlays, info windows, and Teaching Mode highlights.",
			RotationConfigWindowTab.UI,
			[
				"Enable Control, Next Action, Cooldown, and Timeline windows here.",
				"Use Teaching Mode to highlight hotbar buttons and learn rotations visually.",
				"If you want windows to only show in duty/with enemies, toggle that option here."
			]),
		new(
			"Target",
			"Target controls what enemies or allies RSR considers valid.",
			RotationConfigWindowTab.Target,
			[
				"Tune vision cone and engage behavior to avoid unwanted pulls.",
				"Configure target priority rules (FATE, quest mobs, markers).",
				"If targeting feels wrong, adjust filters before changing rotations."
			]),
		new(
			"List",
			"List manages curated status lists: dispels, priority targets, knockbacks, and more.",
			RotationConfigWindowTab.List,
			[
				"Use Reset and Update to restore curated lists when needed.",
				"Add or remove statuses by ID or name using the + buttons.",
				"These lists drive smart reactions across all jobs."
			]),
		new(
			"Duty",
			"Duty holds encounter‑specific toggles for special behavior.",
			RotationConfigWindowTab.Duty,
			[
				"Most of these at the moment can be left enabled but there will be more granular controls in the future.",
				"These settings override general targeting/rotation behavior in specific fights."
			]),
		new(
			"Extra",
			"Extra is for advanced or experimental tweaks.",
			RotationConfigWindowTab.Extra,
			[
				"Animation lock and cooldown delay tweaks for those not using BMR.",
				"Only change these if you understand the side effects.",
			]),
		new(
			"Macros",
			"Starter macros let you control RSR quickly without opening the UI.",
			RotationConfigWindowTab.Main,
			[
				"Use the macros below to toggle Auto/Manual/Off instantly.",
				"Right-click any setting or action to copy its macro command.",
				"Build a small macro bar for fast in combat control."
			],
			StarterMacros),
	];

	private static readonly TutorialStep[] StepsCN =
	[
		new(
			"\u6b22\u8fce\uff01",
			"\u672c\u6559\u7a0b\u5c06\u8bf4\u660e\u5982\u4f55\u8bbe\u7f6e Rotation Solver Reborn\uff0c\u4ee5\u53ca\u5404\u4e2a\u90e8\u5206\u7684\u529f\u80fd\u548c\u63a8\u8350\u7684\u5b8f\u3002",
			Bullets:
			[
				"\u4f7f\u7528 /rotation \u6216\u63d2\u4ef6 UI \u6309\u94ae\u6253\u5f00\u914d\u7f6e\u7a97\u53e3\u3002",
				"\u4f7f\u7528\u4e0b\u4e00\u6b65/\u4e0a\u4e00\u6b65\u6d4f\u89c8\u5404\u4e2a\u90e8\u5206\uff0c\u5e76\u5728\u8fc7\u7a0b\u4e2d\u5e94\u7528\u66f4\u6539\u3002",
				"\u5927\u591a\u6570\u8bbe\u7f6e\u5728\u767b\u5f55\u540e\u4fee\u6539\u662f\u5b89\u5168\u7684\uff0c\u4f46\u5728\u719f\u6089\u4e4b\u524d\u8bf7\u907f\u514d\u5728\u6218\u6597\u4e2d\u8c03\u6574\u3002",
				"\u53f3\u952e\u70b9\u51fb\u4efb\u4f55\u8bbe\u7f6e\u6216\u52a8\u4f5c\u6807\u7b7e\u53ef\u590d\u5236\u5176\u5b8f\u547d\u4ee4\u3002"
			]),
		new(
			"\u4e3b\u7a97\u53e3",
			"\u4e3b\u7a97\u53e3\u662f\u4f60\u7684\u6982\u89c8\u4e2d\u67a2\uff1a\u63d2\u4ef6\u4fe1\u606f\u3001\u517c\u5bb9\u6027\u4fe1\u606f\u3001\u94fe\u63a5\u548c\u5b8f\u5217\u8868\u5747\u5728\u6b64\u5904\u3002",
			RotationConfigWindowTab.Main,
			[
				"\u4f7f\u7528\u6b64\u6807\u7b7e\u9875\u67e5\u770b\u4e0d\u517c\u5bb9\u63d2\u4ef6\u5e76\u6253\u5f00\u652f\u6301\u94fe\u63a5\u3002",
				"\u67e5\u770b\u5b8f\u90e8\u5206\u4ee5\u5b66\u4e60\u5feb\u901f\u804a\u5929\u547d\u4ee4\u3002",
				"\u5982\u679c\u66f4\u65b0\u540e\u51fa\u73b0\u95ee\u9898\uff0c\u8bf7\u5148\u68c0\u67e5\u6b64\u6807\u7b7e\u9875\u3002"
			]),
		new(
			"\u804c\u4e1a\u8bbe\u7f6e",
			"\u804c\u4e1a\u914d\u7f6e\u63a7\u5236\u5f53\u524d\u804c\u4e1a\u7684\u5faa\u73af\u9009\u62e9\u548c\u804c\u4e1a\u4e13\u5c5e\u9009\u9879\u3002",
			RotationConfigWindowTab.Job,
			[
				"\u70b9\u51fb\u5faa\u73af\u540d\u79f0\uff08\u5982 Reborn\uff09\u9009\u62e9\u8981\u4f7f\u7528\u7684\u5faa\u73af\u9884\u8bbe\u3002",
				"\u5728\u9002\u7528\u65f6\u8c03\u6574\u804c\u4e1a\u4f18\u5148\u7ea7\uff08\u5982\u821e\u8005\u821e\u4f34\u3001\u8d24\u8005\u53d1\u708e\uff09\u3002",
				"\u5982\u679c\u67d0\u4e2a\u804c\u4e1a\u611f\u89c9\u4e0d\u5bf9\u52b2\uff0c\u5728\u89e6\u78b0\u5168\u5c40\u8bbe\u7f6e\u4e4b\u524d\u5148\u68c0\u67e5\u6b64\u5904\u3002"
			]),
		new(
			"\u6280\u80fd",
			"\u6280\u80fd\u914d\u7f6e\u51b3\u5b9a RSR \u53ef\u4ee5\u4f7f\u7528\u54ea\u4e9b\u6280\u80fd\u4ee5\u53ca\u5b83\u4eec\u7684\u884c\u4e3a\u65b9\u5f0f\u3002",
			RotationConfigWindowTab.Actions,
			[
				"\u70b9\u51fb\u7c7b\u522b\u4e2d\u7684\u6280\u80fd\u56fe\u6807\u67e5\u770b\u8bbe\u7f6e\uff0c\u53ef\u542f\u7528/\u7981\u7528\u6216\u66f4\u6539\u4f7f\u7528\u89c4\u5219\u3002",
				"\u5982\u679c\u60f3\u8ba9 RSR \u81ea\u52a8\u91ca\u653e\u4f60\u624b\u52a8\u6392\u961f\u7684\u6280\u80fd\uff0c\u8bf7\u4f7f\u7528\u62e6\u622a\u529f\u80fd\u3002",
				"\u5207\u6362\u51b7\u5374\u7a97\u53e3\u663e\u793a\uff0c\u786e\u4fdd\u8986\u76d6\u5c42\u4ec5\u663e\u793a\u4f60\u60f3\u8981\u7684\u5185\u5bb9\u3002"
			]),
		new(
			"\u81ea\u52a8",
			"\u81ea\u52a8\u63a7\u5236\u5168\u5c40\u6280\u80fd\u4f7f\u7528\u3001AoE \u903b\u8f91\u3001\u6253\u65ad\u3001\u836f\u6c34\u548c\u6cbb\u7597\u884c\u4e3a\u3002",
			RotationConfigWindowTab.Auto,
			[
				"\u5728\u6b64\u5904\u53ef\u4ee5\u8c03\u6574 AoE \u903b\u8f91\uff08\u5173\u95ed\u3001Cleave \u548c Full\uff09\u3002",
				"\u8c03\u6574\u6cbb\u7597\u804c\u4e1a\u7684\u6cbb\u7597\u9608\u503c\u548c\u975e\u6cbb\u7597\u804c\u4e1a\u7684\u8f85\u52a9\u9009\u9879\u3002",
				"\u5982\u679c\u60f3\u8981\u66f4\u4fdd\u5b88\u7684\u5faa\u73af\uff0c\u8bf7\u5148\u8c03\u6574\u8fd9\u4e9b\u8bbe\u7f6e\u3002"
			]),
		new(
			"\u57fa\u7840",
			"\u57fa\u7840\u5305\u542b\u5f71\u54cd\u6240\u6709\u804c\u4e1a\u7684\u6838\u5fc3\u65f6\u5e8f\u548c\u81ea\u52a8\u5316\u884c\u4e3a\u3002",
			RotationConfigWindowTab.Basic,
			[
				"\u63d0\u524d\u91cf\u5f71\u54cd\u63d2\u5165\u80fd\u529b\u6570\u91cf\u548c\u5361 GCD\u2014\u2014\u8f83\u5c0f\u7684\u503c = \u66f4\u591a\u80fd\u529b\u6280\u3002\u901a\u5e38\u4e0d\u9700\u8981\u66f4\u6539\u6b64\u9879\u3002",
				"\u6700\u77ed\u66f4\u65b0\u65f6\u95f4\u5728\u6027\u80fd\u548c\u54cd\u5e94\u901f\u5ea6\u4e4b\u95f4\u53d6\u820d\u3002",
				"\u81ea\u52a8\u5207\u6362\u63a7\u5236 RSR \u4f55\u65f6\u81ea\u52a8\u5f00\u5173\uff08\u5012\u8ba1\u65f6\u3001\u6b7b\u4ea1\u3001\u526f\u672c\u4e8b\u4ef6\uff09\u3002"
			]),
		new(
			"\u754c\u9762",
			"\u754c\u9762\u63a7\u5236\u8986\u76d6\u5c42\u3001\u4fe1\u606f\u7a97\u53e3\u548c\u6559\u5b66\u6a21\u5f0f\u9ad8\u4eae\u663e\u793a\u3002",
			RotationConfigWindowTab.UI,
			[
				"\u5728\u6b64\u5904\u542f\u7528\u63a7\u5236\u3001\u4e0b\u4e00\u52a8\u4f5c\u3001\u51b7\u5374\u548c\u65f6\u95f4\u7ebf\u7a97\u53e3\u3002",
				"\u4f7f\u7528\u6559\u5b66\u6a21\u5f0f\u9ad8\u4eae\u70ed\u952e\u680f\u6309\u94ae\uff0c\u76f4\u89c2\u5b66\u4e60\u5faa\u73af\u3002",
				"\u5982\u679c\u60f3\u7a97\u53e3\u4ec5\u5728\u526f\u672c\u6216\u6709\u654c\u4eba\u65f6\u663e\u793a\uff0c\u5728\u6b64\u5904\u5207\u6362\u9009\u9879\u3002"
			]),
		new(
			"\u76ee\u6807",
			"\u76ee\u6807\u63a7\u5236 RSR \u8ba4\u4e3a\u54ea\u4e9b\u654c\u4eba\u6216\u53cb\u65b9\u662f\u6709\u6548\u76ee\u6807\u3002",
			RotationConfigWindowTab.Target,
			[
				"\u8c03\u6574\u89c6\u91ce\u9525\u5f62\u548c\u8fdb\u6218\u884c\u4e3a\u4ee5\u907f\u514d\u4e0d\u5fc5\u8981\u7684\u5f00\u602a\u3002",
				"\u914d\u7f6e\u76ee\u6807\u4f18\u5148\u7ea7\u89c4\u5219\uff08FATE\u3001\u4efb\u52a1\u602a\u3001\u6807\u8bb0\uff09\u3002",
				"\u5982\u679c\u76ee\u6807\u9009\u62e9\u611f\u89c9\u4e0d\u5bf9\uff0c\u5728\u66f4\u6539\u5faa\u73af\u4e4b\u524d\u5148\u8c03\u6574\u8fc7\u6ee4\u5668\u3002"
			]),
		new(
			"\u5217\u8868",
			"\u5217\u8868\u7ba1\u7406\u7cbe\u9009\u72b6\u6001\u5217\u8868\uff1a\u9a71\u6563\u3001\u4f18\u5148\u76ee\u6807\u3001\u51fb\u9000\u7b49\u3002",
			RotationConfigWindowTab.List,
			[
				"\u9700\u8981\u65f6\u4f7f\u7528\u91cd\u7f6e\u5e76\u66f4\u65b0\u6062\u590d\u7cbe\u9009\u5217\u8868\u3002",
				"\u901a\u8fc7 + \u6309\u94ae\u901a\u8fc7 ID \u6216\u540d\u79f0\u6dfb\u52a0\u6216\u79fb\u9664\u72b6\u6001\u3002",
				"\u8fd9\u4e9b\u5217\u8868\u9a71\u52a8\u6240\u6709\u804c\u4e1a\u7684\u667a\u80fd\u53cd\u5e94\u3002"
			]),
		new(
			"\u4efb\u52a1",
			"\u4efb\u52a1\u5305\u542b\u7279\u5b9a\u6218\u6597\u7684\u4e13\u5c5e\u5f00\u5173\u3002",
			RotationConfigWindowTab.Duty,
			[
				"\u76ee\u524d\u5927\u591a\u6570\u8fd9\u4e9b\u8bbe\u7f6e\u53ef\u4ee5\u4fdd\u6301\u542f\u7528\uff0c\u4f46\u672a\u6765\u4f1a\u6709\u66f4\u7ec6\u81f4\u7684\u63a7\u5236\u3002",
				"\u8fd9\u4e9b\u8bbe\u7f6e\u4f1a\u5728\u7279\u5b9a\u6218\u6597\u4e2d\u8986\u76d6\u9ed8\u8ba4\u76ee\u6807/\u5faa\u73af\u884c\u4e3a\u3002"
			]),
		new(
			"\u989d\u5916",
			"\u989d\u5916\u662f\u9ad8\u7ea7\u6216\u5b9e\u9a8c\u6027\u5fae\u8c03\u3002",
			RotationConfigWindowTab.Extra,
			[
				"\u4e3a\u4e0d\u4f7f\u7528 BMR \u7684\u7528\u6237\u63d0\u4f9b\u52a8\u753b\u9501\u5b9a\u548c\u51b7\u5374\u5ef6\u8fdf\u5fae\u8c03\u3002",
				"\u4ec5\u5728\u4f60\u7406\u89e3\u526f\u4f5c\u7528\u7684\u60c5\u51b5\u4e0b\u66f4\u6539\u8fd9\u4e9b\u8bbe\u7f6e\u3002",
			]),
		new(
			"\u5b8f",
			"\u5165\u95e8\u5b8f\u8ba9\u4f60\u65e0\u9700\u6253\u5f00 UI \u5c31\u80fd\u5feb\u901f\u63a7\u5236 RSR\u3002",
			RotationConfigWindowTab.Main,
			[
				"\u4f7f\u7528\u4e0b\u65b9\u5b8f\u7acb\u5373\u5207\u6362\u81ea\u52a8/\u624b\u52a8/\u5173\u95ed\u3002",
				"\u53f3\u952e\u70b9\u51fb\u4efb\u4f55\u8bbe\u7f6e\u6216\u52a8\u4f5c\u53ef\u590d\u5236\u5176\u5b8f\u547d\u4ee4\u3002",
				"\u5efa\u7acb\u4e00\u4e2a\u5c0f\u578b\u5b8f\u680f\u4ee5\u4fbf\u5728\u6218\u6597\u4e2d\u5feb\u901f\u63a7\u5236\u3002"
			],
			StarterMacros),
	];

	public FirstStartTutorialWindow()
		: base("RSR First Start Tutorial", BaseFlags)
	{
		Size = new Vector2(720, 530);
		SizeCondition = ImGuiCond.FirstUseEver;
		RespectCloseHotkey = true;
	}

	public override bool DrawConditions()
	{
		return DataCenter.PlayerAvailable();
	}

	private TutorialStep[] CurrentSteps => LocalizationHelper.IsChineseClient ? StepsCN : Steps;

	public override void Draw()
	{
		var step = CurrentSteps[_stepIndex];

		ImGui.PushFont(FontManager.GetFont(ImGui.GetFontSize() + 6));
		ImGui.TextColored(ImGuiColors.ParsedGold, step.Title);
		ImGui.PopFont();

		DrawWrappedText(step.Description);
		ImGui.Spacing();

		if (step.Bullets is { Length: > 0 })
		{
			foreach (var bullet in step.Bullets)
			{
				DrawWrappedBullet(bullet);
			}
			ImGui.Spacing();
		}

		if (step.RecommendedMacros is { Length: > 0 })
		{
			ImGui.TextColored(ImGuiColors.HealerGreen, LocalizationHelper.IsChineseClient ? "\u63a8\u8350\u5b8f\uff1a" : "Recommended macros:");
			for (var i = 0; i < step.RecommendedMacros.Length; i++)
			{
				var macro = step.RecommendedMacros[i];
				DrawWrappedBullet(macro);

				ImGui.SameLine();
				var buttonId = $"Copy##TutorialMacro_{i}";
				if (ImGui.SmallButton(buttonId))
				{
					ImGui.SetClipboardText(macro);
					Svc.Toasts.ShowNormal(LocalizationHelper.IsChineseClient ? "\u5b8f\u5df2\u590d\u5236\u5230\u526a\u8d34\u677f\u3002" : "Macro copied to clipboard.");
				}
			}

			ImGui.Spacing();
		}

		if (step.Tab != null)
		{
			var openTabText = LocalizationHelper.IsChineseClient
				? $"\u6253\u5f00 {step.Tab.Value.CNString()} \u6807\u7b7e\u9875"
				: $"Open {step.Tab} tab";
			if (ImGui.Button(openTabText))
			{
				RotationSolverPlugin.ShowConfigWindow(step.Tab.Value);
			}
			ImGui.Spacing();
		}

		ImGui.Separator();
		DrawNavigation();
	}

	public override void OnClose()
	{
		MarkTutorialComplete();
		base.OnClose();
	}

	private static void DrawWrappedText(string text)
	{
		var wrapPos = ImGui.GetCursorPosX() + ImGui.GetContentRegionAvail().X;
		ImGui.PushTextWrapPos(wrapPos);
		ImGui.TextWrapped(text);
		ImGui.PopTextWrapPos();
	}

	private static void DrawWrappedBullet(string text)
	{
		ImGui.Bullet();
		ImGui.SameLine();
		DrawWrappedText(text);
	}

	private void DrawNavigation()
	{
		var isCN = LocalizationHelper.IsChineseClient;
		ImGui.BeginDisabled(_stepIndex == 0);
		if (ImGui.Button(isCN ? "\u4e0a\u4e00\u6b65" : "Back"))
		{
			_stepIndex = Math.Max(0, _stepIndex - 1);
		}
		ImGui.EndDisabled();

		ImGui.SameLine();

		if (_stepIndex < CurrentSteps.Length - 1)
		{
			if (ImGui.Button(isCN ? "\u4e0b\u4e00\u6b65" : "Next"))
			{
				_stepIndex = Math.Min(CurrentSteps.Length - 1, _stepIndex + 1);
			}
		}
		else
		{
			if (ImGui.Button(isCN ? "\u5b8c\u6210" : "Finish"))
			{
				FinishTutorial();
			}
		}
	}

	private void FinishTutorial()
	{
		MarkTutorialComplete();
		IsOpen = false;
	}

	private static void MarkTutorialComplete()
	{
		Service.Config.TutorialDone = true;
		Service.Config.Save();
	}

	private sealed record TutorialStep(
		string Title,
		string Description,
		RotationConfigWindowTab? Tab = null,
		string[]? Bullets = null,
		string[]? RecommendedMacros = null);
}