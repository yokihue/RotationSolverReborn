using ECommons.DalamudServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace RotationSolver.Data
{
    /// <summary>
    /// Provides Chinese (Simplified) localization for UI strings.
    /// </summary>
    internal static class LocalizationHelper
    {
        /// <summary>
        /// Detects whether the game client is running in Chinese (Simplified or Traditional).
        /// </summary>
        private static bool? _isChineseClient;

        /// <summary>
        /// Detects whether the game client is running in Chinese.
        /// </summary>
        /// <remarks>
        /// Resolved once and cached. This is read on every ImGui frame via
        /// <c>GetLocalizedDescription</c>, so it must not re-enter Dalamud each time.
        /// Uses <see cref="Dalamud.Plugin.Services.IClientState.ClientLanguage"/>, which is a
        /// non-nullable enum, hence no null-conditional access.
        /// </remarks>
        public static bool IsChineseClient
        {
            get
            {
                if (_isChineseClient.HasValue)
                {
                    return _isChineseClient.Value;
                }

                try
                {
                    var clientState = Svc.ClientState;
                    if (clientState == null)
                    {
                        // Services not ready yet; do not cache a wrong answer.
                        return false;
                    }

                    var lang = clientState.ClientLanguage.ToString();
                    _isChineseClient = lang is "Chinese" or "ChineseSimplified" or "ChineseTraditional";
                    return _isChineseClient.Value;
                }
                catch
                {
                    return false;
                }
            }
        }
        private static readonly Dictionary<UiString, string> _chineseStrings = new()
        {
            { UiString.ConfigWindow_ConditionSetDesc, "您选择的条件值。点击修改。" },
            { UiString.ConfigWindow_ConditionSet, "条件值" },
            { UiString.ConfigWindow_ActionSet, "技能条件" },
            { UiString.ConfigWindow_TraitSet, "特性条件" },
            { UiString.ConfigWindow_TargetSet, "目标条件" },
            { UiString.ConfigWindow_RotationSet, "循环条件" },
            { UiString.ConfigWindow_NamedSet, "命名条件" },
            { UiString.ConfigWindow_Territoryset, "区域条件" },
            { UiString.ConfigWindow_NoRotation, "未加载职业循环！请检查职业循环标签页！" },
            { UiString.ConfigWindow_DutyRotationDesc, "当前副本逻辑。" },
            { UiString.ConfigWindow_List_Remove, "移除" },
            { UiString.ActionSequencer_Load, "从文件夹加载" },
            { UiString.ConfigWindow_About_Punchline, "在每一帧分析 PvE 战斗信息并找到最佳动作。" },
            { UiString.ConfigWindow_Rotation_InvalidRotation, "无效的循环！\n请更新到最新版本或联系 {0}！" },
            { UiString.ConfigWindow_Helper_SwitchRotation, "点击切换职业循环" },
            { UiString.ConfigWindow_Search_Result, "搜索结果" },
            { UiString.ConfigWindow_About_Description, "包含单帧战斗中几乎所有可用信息，包括所有队员状态、敌方目标状态、技能冷却、角色 MP/HP、角色位置、敌方读条状态、连击状态、战斗持续时间、玩家等级等。\n\n系统会在热键栏中高亮最佳动作，或帮助您点击它。" },
            { UiString.ConfigWindow_About_Warning, "此功能专为一般战斗设计，不适用于零式或绝本内容。\n\n请谨慎使用！虽然 RSR 并非专门为零式或绝本设计，但在这些内容中也能正常工作，但不会帮您处理机制。请注意规避并使用宏。" },
            { UiString.ConfigWindow_About_ClickingCount, "RSR 已为您点击技能 {0:N0} 次。" },
            { UiString.ConfigWindow_About_Macros, "状态宏" },
            { UiString.ConfigWindow_About_SettingMacros, "技能与设置宏" },
            { UiString.ConfigWindow_About_Compatibility, "兼容性" },
            { UiString.ConfigWindow_About_Supporters, "支持者" },
            { UiString.ConfigWindow_About_Links, "链接" },
            { UiString.ConfigWindow_About_Warnings, "系统警告" },
            { UiString.ConfigWindow_About_Warnings_Warning, "警告信息" },
            { UiString.ConfigWindow_About_Warnings_Time, "警告时间" },
            { UiString.ConfigWindow_About_Compatibility_Description, "Rotation Solver 帮助您选择目标和点击技能。任何会改变这些行为的插件都会影响其决策。\n\n以下是历来（但并非总是）导致兼容性问题的插件列表：" },
            { UiString.ConfigWindow_About_Compatibility_Mistake, "无法正确执行 RSR 需要执行的操作。" },
            { UiString.ConfigWindow_About_Compatibility_Mislead, "与 RSR 的决策产生冲突" },
            { UiString.ConfigWindow_About_Compatibility_Crash, "导致游戏崩溃" },
            { UiString.ConfigWindow_About_ThanksToSupporters, "非常感谢 Ko-fi 赞助者。" },
            { UiString.ConfigWindow_About_OpenConfigFolder, "打开配置文件夹" },
            { UiString.ConfigWindow_Rotation_Description, "说明" },
            { UiString.ConfigWindow_Rotation_Configuration, "配置" },
            { UiString.ConfigWindow_DutyRotation_Configuration, "副本配置" },
            { UiString.ConfigWindow_Rotation_Status, "状态" },
            { UiString.ConfigWindow_DutyRotation_Status, "副本循环状态" },
            { UiString.ConfigWindow_Actions_Description, "用于自定义 RSR 何时自动使用特定技能。点击左侧列表中的技能图标。在下方，您可以设置特定技能的使用条件。每个技能可以有不同的条件来覆盖默认的循环行为。" },
            { UiString.ConfigWindow_Actions_ShowOnCDWindow, "在冷却窗口中显示" },
            { UiString.ConfigWindow_Actions_IsIntercepted, "允许技能被拦截系统拦截" },
            { UiString.ConfigWindow_Actions_IsRestrictedDOT, "对特定怪物禁止使用此技能（如 Jagd Dolls）" },
            { UiString.ConfigWindow_Actions_MinHPFeature, "允许技能受最低 HP 特性限制" },
            { UiString.ConfigWindow_Actions_MinHPPercent, "如果目标血量低于此百分比，不使用此技能" },
            { UiString.ConfigWindow_Actions_SkipPositionSafetyCheck, "跳过 BossModReborn 对此位移技能的位置安全检查" },
            { UiString.ConfigWindow_Actions_TTK, "使用此技能所需的预估击杀时间阈值" },
            { UiString.ConfigWindow_Actions_AoeCount, "使用此技能所需的目标数量" },
            { UiString.ConfigWindow_Actions_CheckStatus, "是否检查自身所需状态效果" },
            { UiString.ConfigWindow_Actions_CheckTargetStatus, "是否检查目标所需状态效果" },
            { UiString.ConfigWindow_Actions_GcdCount, "DOT/状态效果重新应用的 GCD 数量" },
            { UiString.ConfigWindow_Actions_HealRatio, "自动治疗的 HP 比例（仅适用于治疗技能）" },
            { UiString.ConfigWindow_Actions_ConditionDescription, "强制条件具有更高优先级。如果满足强制条件，禁用条件将被忽略。" },
            { UiString.ConfigWindow_Actions_ForcedConditionSet, "强制条件（未支持）" },
            { UiString.ConfigWindow_Actions_ForcedConditionSet_Description, "强制自动使用此技能的条件" },
            { UiString.ConfigWindow_Actions_DisabledConditionSet, "禁用条件（未支持）" },
            { UiString.ConfigWindow_Actions_DisabledConditionSet_Description, "禁止自动使用技能的条件" },
            { UiString.ConfigWindow_List_Description, "在此窗口中，您可以设置可使用列表自定义的参数。" },
            { UiString.ConfigWindow_List_Statuses, "状态效果" },
            { UiString.ConfigWindow_List_Actions, "技能" },
            { UiString.ConfigWindow_List_Territories, "地图专属设置" },
            { UiString.ConfigWindow_List_StatusNameOrId, "状态名称或 ID" },
            { UiString.ConfigWindow_List_Invincibility, "无敌" },
            { UiString.ConfigWindow_List_Priority, "优先级" },
            { UiString.ConfigWindow_List_DangerousStatus, "可驱散的负面状态" },
            { UiString.ConfigWindow_List_NoCastingStatus, "无法施法的负面状态" },
            { UiString.ConfigWindow_List_InvincibilityDesc, "如果目标拥有这些状态之一，则忽略此目标" },
            { UiString.ConfigWindow_List_PriorityDesc, "如果目标拥有这些状态之一，则优先攻击此目标" },
            { UiString.ConfigWindow_List_DangerousStatusDesc, "可驱散的负面状态列表" },
            { UiString.ConfigWindow_List_NoCastingStatusDesc, "如果你拥有这些负面状态之一，则不执行任何动作" },
            { UiString.ConfigWindow_Actions_Copy, "复制到剪贴板" },
            { UiString.ActionSequencer_FromClipboard, "从剪贴板加载" },
            { UiString.ConfigWindow_List_AddStatus, "添加状态效果" },
            { UiString.ConfigWindow_List_ActionNameOrId, "技能名称或 ID" },
            { UiString.ConfigWindow_List_HostileCastingTank, "死刑" },
            { UiString.ConfigWindow_List_HostileCastingArea, "范围攻击" },
            { UiString.ConfigWindow_List_HostileCastingKnockback, "击退" },
            { UiString.ConfigWindow_List_HostileCastingStop, "石化/停止" },
            { UiString.ConfigWindow_List_HostileCastingTankDesc, "如果目标使用这些技能中的任何一个，则使用坦克个人减伤技能" },
            { UiString.ConfigWindow_List_HostileCastingAreaDesc, "如果目标使用这些技能中的任何一个，则使用范围减伤技能" },
            { UiString.ConfigWindow_List_HostileCastingKnockbackDesc, "如果目标使用这些技能中的任何一个，则使用防击退技能" },
            { UiString.ConfigWindow_List_HostileCastingStopDesc, "如果敌方使用此技能，则停止施法或执行动作" },
            { UiString.ConfigWindow_List_AddAction, "添加技能" },
            { UiString.ConfigWindow_List_NoHostile, "不攻击" },
            { UiString.ConfigWindow_List_NoProvoke, "不挑衅" },
            { UiString.ConfigWindow_List_BeneficialPositions, "增益范围位置" },
            { UiString.ConfigWindow_List_NoHostileDesc, "永远不会被选为目标的敌人" },
            { UiString.ConfigWindow_List_NoHostilesName, "不想攻击的敌人名称" },
            { UiString.ConfigWindow_List_NoProvokeDesc, "永远不会被挑衅的敌人" },
            { UiString.ConfigWindow_List_NoProvokeName, "不想挑衅的敌人名称" },
            { UiString.ConfigWindow_List_AddPosition, "添加增益范围位置" },
            { UiString.ActionAbility, "能力" },
            { UiString.ActionFriendly, "友方" },
            { UiString.ActionAttack, "攻击" },
            { UiString.NormalTargets, "普通目标" },
            { UiString.HotTargets, "HoT 目标" },
            { UiString.HpAoe0Gcd, "AoE 治疗能力技的 HP 阈值" },
            { UiString.HpAoeGcd, "AoE 治疗 GCD 技能的 HP 阈值" },
            { UiString.HpSingle0Gcd, "单体治疗能力技的 HP 阈值" },
            { UiString.HpSingleGcd, "单体治疗 GCD 技能的 HP 阈值" },
            { UiString.InfoWindowNoMove, "不移动" },
            { UiString.InfoWindowMove, "移动" },
            { UiString.ConfigWindow_Searching, "搜索设置" },
            { UiString.ConfigWindow_Basic_Timer, "计时器" },
            { UiString.ConfigWindow_Basic_AutoSwitch, "自动切换" },
            { UiString.ConfigWindow_Basic_NamedConditions, "命名条件" },
            { UiString.ConfigWindow_Basic_Others, "其他" },
            { UiString.ConfigWindow_Basic_AnimationLockTime, "单个技能的动画锁定时间。例如 0.6 秒。" },
            { UiString.ConfigWindow_Basic_ClickingDuration, "点击持续时间 - RSR 将在此时间点尝试点击。" },
            { UiString.ConfigWindow_Basic_IdealClickingTime, "理想点击时间" },
            { UiString.ConfigWindow_Basic_RealClickingTime, "实际点击时间" },
            { UiString.ConfigWindow_Basic_SwitchCancelConditionSet, "自动关闭条件" },
            { UiString.ConfigWindow_Basic_SwitchManualConditionSet, "自动手动模式条件" },
            { UiString.ConfigWindow_Basic_SwitchAutoConditionSet, "自动自动模式条件" },
            { UiString.ConfigWindow_Condition_ConditionName, "条件名称" },
            { UiString.ConfigWindow_UI_Information, "信息显示" },
            { UiString.ConfigWindow_UI_Overlay, "覆盖层" },
            { UiString.ConfigWindow_UI_Windows, "窗口" },
            { UiString.ConfigWindow_Auto_Description, "更改 RSR 自动使用技能的方式" },
            { UiString.ConfigWindow_Auto_ActionUsage, "技能使用和控制" },
            { UiString.ConfigWindow_Auto_ActionUsage_Description, "RSR 可以使用哪些技能" },
            { UiString.ConfigWindow_Auto_HealingCondition, "治疗使用和控制" },
            { UiString.ConfigWindow_Auto_HealingCondition_Description, "RSR 应如何使用治疗技能" },
            { UiString.ConfigWindow_Auto_StateCondition, "自定义状态条件（未支持）" },
            { UiString.ConfigWindow_Auto_HealAreaConditionSet, "范围治疗强制条件" },
            { UiString.ConfigWindow_Auto_HealSingleConditionSet, "单体治疗强制条件" },
            { UiString.ConfigWindow_Auto_DefenseAreaConditionSet, "范围防御强制条件" },
            { UiString.ConfigWindow_Auto_DefenseSingleConditionSet, "单体防御强制条件" },
            { UiString.ConfigWindow_Auto_DispelStancePositionalConditionSet, "驱散/姿态/身位强制条件" },
            { UiString.ConfigWindow_Auto_RaiseShirkConditionSet, "复活/退避强制条件" },
            { UiString.ConfigWindow_Auto_MoveForwardConditionSet, "前进强制条件" },
            { UiString.ConfigWindow_Auto_MoveBackConditionSet, "后退强制条件" },
            { UiString.ConfigWindow_Auto_AntiKnockbackConditionSet, "防击退强制条件" },
            { UiString.ConfigWindow_Auto_SpeedConditionSet, "加速强制条件" },
            { UiString.ConfigWindow_Auto_NoCastingConditionSet, "无法施法条件集" },
            { UiString.ConfigWindow_Auto_ActionCondition_Description, "这将改变 RSR 使用技能的方式" },
            { UiString.ConfigWindow_Target_Config, "配置" },
            { UiString.ConfigWindow_List_Hostile, "敌方" },
            { UiString.ConfigWindow_Param_HostileDesc, "敌人目标选择逻辑。添加更多选项可在使用 /rotation Auto 时循环切换。\n使用 /rotation Settings TargetingTypes add <选项> 添加，\n/rotation Settings TargetingTypes remove <选项> 移除，\n/rotation Settings TargetingTypes removeall 移除所有选项。" },
            { UiString.ConfigWindow_Actions_MoveUp, "上移" },
            { UiString.ConfigWindow_Actions_MoveDown, "下移" },
            { UiString.ConfigWindow_Param_HostileCondition, "敌方目标选择条件" },
            { UiString.ConfigWindow_Extra_Description, "RSR 专注于职业循环本身。这些是附加功能，可能随时被移除。" },
            { UiString.ConfigWindow_EventItem, "事件" },
            { UiString.ConfigWindow_Internal, "内部" },
            { UiString.ConfigWindow_Extra_Others, "其他" },
            { UiString.ConfigWindow_Events_AddEvent, "添加事件" },
            { UiString.ConfigWindow_Events_Description, "在此窗口中，你可以设置在使用某个技能后触发哪个宏。" },
            { UiString.ConfigWindow_Events_DutyStart, "副本开始：" },
            { UiString.ConfigWindow_Events_DutyEnd, "副本结束：" },
            { UiString.ConfigWindow_Events_RemoveEvent, "删除事件" },
            { UiString.ActionSequencer_NotDescription, "点击反转。\n已反转：{0}" },
            { UiString.ConfigWindow_Actions_MemberName, "成员名称" },
            { UiString.ConfigWindow_Condition_RotationNullWarning, "循环为空，请登录或切换职业！" },
            { UiString.ConfigWindow_Duty_Ultimate, "绝本" },
            { UiString.ConfigWindow_Duty_Savage, "零式" },
            { UiString.ConfigWindow_Duty_ChaoticAlliance, "灭本" },
            { UiString.ConfigWindow_Duty_Extreme, "极蛮神" },
            { UiString.ConfigWindow_Duty_Dungeon, "迷宫" },
            { UiString.ConfigWindow_Duty_DeepDungeon, "深层迷宫" },
            { UiString.ConfigWindow_Duty_VariantDungeon, "多变迷宫" },
            { UiString.ConfigWindow_Duty_TreasureDungeon, "宝物库" },
            { UiString.ConfigWindow_Duty_Alliance, "团队任务" },
            { UiString.ConfigWindow_Duty_FieldOps, "野外作战" },
            { UiString.ConfigWindow_Duty_PvP, "PvP" },
            { UiString.ConfigWindow_Duty_TheMaskedCarnivale, "假面嘉年华" },
            { UiString.ConfigWindow_Duty_CrucibleOfTheUnbroken, "殊荣之战" },
            { UiString.ActionSequencer_Delay_Description, "延迟转换为真" },
            { UiString.ActionSequencer_Offset_Description, "延迟转换" },
            { UiString.ActionConditionType_EnoughLevel, "足够等级" },
            { UiString.ActionSequencer_TimeOffset, "时间偏移" },
            { UiString.ActionSequencer_Charges, "充能" },
            { UiString.ActionSequencer_Original, "原始" },
            { UiString.ActionSequencer_Adjusted, "调整后" },
            { UiString.ActionSequencer_ActionTarget, "{0} 的目标" },
            { UiString.ActionSequencer_StatusAll, "来自全体" },
            { UiString.ActionSequencer_StatusSelf, "来自自身" },
            { UiString.ConfigWindow_Condition_TargetWarning, "不应使用此目标，因为这不是该技能的目标。请尝试从技能中选择。" },
            { UiString.ConfigWindow_Condition_TerritoryName, "区域名称" },
            { UiString.ConfigWindow_Condition_DutyName, "副本名称" },
            { UiString.HighEndWarning, "请单独绑定减伤/护盾技能防止 RSR 在 {0} 的关键时刻失败！" },
            { UiString.ConfigWindow_Helper_RunCommand, "点击执行命令" },
            { UiString.ConfigWindow_Helper_CopyCommand, "右键复制命令" },
            { UiString.ConfigWindow_Events_MacroIndex, "宏编号" },
            { UiString.ConfigWindow_Events_ShareMacro, "共享宏" },
            { UiString.ConfigWindow_Events_ActionName, "技能名称" },
            { UiString.CommandsChangeSettingsValue, "将 {0} 修改为 {1}" },
            { UiString.CommandsCannotFindConfig, "未能在当前职业循环中找到此配置，请检查。" },
            { UiString.CommandsInsertAction, "将在 {0} 秒内使用" },
            { UiString.CommandsInsertActionFailure, "找不到此技能，请检查技能名称。" },
            { UiString.CommandsMissingArgument, "无法从字符串中获取配置项和值，请确保您同时提供了配置选项和值。" },
            { UiString.SpecialCommandType_Start, "开启" },
            { UiString.SpecialCommandType_Cancel, "取消" },
            { UiString.SpecialCommandType_HealArea, "范围治疗" },
            { UiString.SpecialCommandType_HealSingle, "单体治疗" },
            { UiString.SpecialCommandType_DefenseArea, "范围防御" },
            { UiString.SpecialCommandType_DefenseSingle, "单体防御" },
            { UiString.SpecialCommandType_TankStance, "坦克姿态" },
            { UiString.SpecialCommandType_Dispel, "驱散" },
            { UiString.SpecialCommandType_Positional, "身位" },
            { UiString.SpecialCommandType_Shirk, "退避" },
            { UiString.SpecialCommandType_Raise, "复活" },
            { UiString.SpecialCommandType_MoveForward, "前进" },
            { UiString.SpecialCommandType_MoveBack, "后退" },
            { UiString.SpecialCommandType_AntiKnockback, "防击退" },
            { UiString.SpecialCommandType_Burst, "爆发" },
            { UiString.SpecialCommandType_EndSpecial, "结束特殊" },
            { UiString.SpecialCommandType_Speed, "加速" },
            { UiString.SpecialCommandType_LimitBreak, "极限技" },
            { UiString.SpecialCommandType_NoCasting, "禁止施法" },
            { UiString.SpecialCommandType_Smart, "自动目标" },
            { UiString.SpecialCommandType_Manual, "手动目标" },
            { UiString.SpecialCommandType_Off, "关闭" },
            { UiString.Commands_Rotation, "打开配置窗口" },
            { UiString.Commands_Start, "开启 RSR 战斗循环状态" },
            { UiString.Commands_Off, "关闭 RSR 战斗循环状态" },
            { UiString.ConfigWindowHeader, "Rotation Solver Reborn 设置 v" },
            { UiString.JobConfigTip, "此配置是职业专属的" },
            { UiString.NotInJob, "您当前职业无法使用此选项\n \n需要角色或职业：\n{0}" },
            { UiString.WelcomeWindow_Header, "欢迎使用 Rotation Solver Reborn！" },
            { UiString.WelcomeWindow_WelcomeBack, "以下是您上次离开后错过的内容" },
            { UiString.WelcomeWindow_Welcome, "看来您可能是新用户！让我们开始吧！" },
            { UiString.WelcomeWindow_Changelog, "近期更新：" }
        };

        /// <summary>
        /// Gets the Chinese (Simplified) translation for a UiString value.
        /// Returns the original English string if no translation exists.
        /// </summary>
        public static string GetChineseString(this UiString value)
        {
            if (_chineseStrings.TryGetValue(value, out var chinese))
            {
                return chinese;
            }

            // Fall back to the raw [Description] attribute, NOT GetDescription().
            // GetDescription() routes UiString values back into this method, so calling
            // it here would recurse infinitely for any untranslated enum member.
            return GetRawDescription(value);
        }

        /// <summary>
        /// Reads the <see cref="DescriptionAttribute"/> directly off the enum member.
        /// Deliberately independent of the localization-aware extension methods so it
        /// can safely serve as a fallback for them.
        /// </summary>
        private static string GetRawDescription(UiString value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }

        /// <summary>
        /// Gets the localized string based on whether the client is running in Chinese.
        /// </summary>
        public static string GetLocalizedString(this UiString value, bool isChineseClient)
        {
            return isChineseClient ? value.GetChineseString() : value.GetDescription();
        }

        /// <summary>
        /// Gets the Chinese (Simplified) translation for a RotationConfigWindowTab description.
        /// </summary>
        public static string GetTabDescriptionCN(string tabDescription)
        {
            return tabDescription switch
            {
                "Useful information and macro list." => "实用信息和宏列表。",
                "Rotation specific configs." => "职业循环专属配置。",
                "Configure Duty Rotation." => "配置副本循环。",
                "Configure abilities and custom conditions for your current job." => "配置当前职业的技能和自定义条件。",
                "Configure reactive actions and status effect lists." => "配置响应技能和状态效果列表。",
                "Configure basic settings." => "配置基础设置。",
                "Configure user interface settings." => "配置用户界面设置。",
                "Configure general action usage and control settings." => "配置常规技能使用和控制设置。",
                "Configure targeting settings." => "配置目标选择设置。",
                "Duty specific settings." => "副本专属设置。",
                "Configure optional helpful features." => "配置可选实用功能。",
                "Debug options for developers and rotation writers (disable when not in use)." => "面向开发者和循环编写者的调试选项（不用时请禁用）。",
                "Configure AutoDuty settings and view related information." => "配置 AutoDuty 设置并查看相关信息。",
                _ => tabDescription
            };
        }
    }
}
