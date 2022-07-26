using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 기본 접근자 확장 클래스 */
public static partial class AccessExtension {
	#region 클래스 함수
	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EPlayMode a_eSender) {
		return a_eSender > EPlayMode.NONE && a_eSender < EPlayMode.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EItemType a_eSender) {
		return a_eSender > EItemType.NONE && a_eSender < EItemType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EItemKinds a_eSender) {
		return a_eSender > EItemKinds.NONE && a_eSender < EItemKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EMissionType a_eSender) {
		return a_eSender > EMissionType.NONE && a_eSender < EMissionType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EMissionKinds a_eSender) {
		return a_eSender > EMissionKinds.NONE && a_eSender < EMissionKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ERewardType a_eSender) {
		return a_eSender > ERewardType.NONE && a_eSender < ERewardType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ERewardKinds a_eSender) {
		return a_eSender > ERewardKinds.NONE && a_eSender < ERewardKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ERewardQuality a_eSender) {
		return a_eSender > ERewardQuality.NONE && a_eSender < ERewardQuality.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ERewardAcquirePopupType a_eSender) {
		return a_eSender > ERewardAcquirePopupType.NONE && a_eSender < ERewardAcquirePopupType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EEpisodeType a_eSender) {
		return a_eSender > EEpisodeType.NONE && a_eSender < EEpisodeType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EEpisodeKinds a_eSender) {
		return a_eSender > EEpisodeKinds.NONE && a_eSender < EEpisodeKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ETutorialType a_eSender) {
		return a_eSender > ETutorialType.NONE && a_eSender < ETutorialType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ETutorialKinds a_eSender) {
		return a_eSender > ETutorialKinds.NONE && a_eSender < ETutorialKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ETargetType a_eSender) {
		return a_eSender > ETargetType.NONE && a_eSender < ETargetType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ETargetKinds a_eSender) {
		return a_eSender > ETargetKinds.NONE && a_eSender < ETargetKinds.MAX_VAL;
	}
	
	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EFXType a_eSender) {
		return a_eSender > EFXType.NONE && a_eSender < EFXType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EFXKinds a_eSender) {
		return a_eSender > EFXKinds.NONE && a_eSender < EFXKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ESkillType a_eSender) {
		return a_eSender > ESkillType.NONE && a_eSender < ESkillType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ESkillKinds a_eSender) {
		return a_eSender > ESkillKinds.NONE && a_eSender < ESkillKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EAbilityType a_eSender) {
		return a_eSender > EAbilityType.NONE && a_eSender < EAbilityType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EAbilityKinds a_eSender) {
		return a_eSender > EAbilityKinds.NONE && a_eSender < EAbilityKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EObjType a_eSender) {
		return a_eSender > EObjType.NONE && a_eSender < EObjType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EObjKinds a_eSender) {
		return a_eSender > EObjKinds.NONE && a_eSender < EObjKinds.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EResType a_eSender) {
		return a_eSender > EResType.NONE && a_eSender < EResType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EResKinds a_eSender) {
		return a_eSender > EResKinds.NONE && a_eSender < EResKinds.MAX_VAL;
	}
	
	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EProductType a_eSender) {
		return a_eSender > EProductType.NONE && a_eSender < EProductType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EProductKinds a_eSender) {
		return a_eSender > EProductKinds.NONE && a_eSender < EProductKinds.MAX_VAL;
	}

	/** 컴포넌트 상호 작용 여부를 변경한다 */
	public static void ExSetInteractable(this Button a_oSender, bool a_bIsEnable, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		var oTouchInteractable = a_oSender?.GetComponentInChildren<CTouchInteractable>();
		oTouchInteractable?.SetInteractable(a_bIsEnable);
	}

	/** 텍스트를 변경한다 */
	public static void ExSetText(this Text a_oSender, string a_oStr, EFontSet a_eFontSet = EFontSet._1, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_eFontSet.ExIsValid()));

		// 텍스트가 존재 할 경우
		if(a_oSender != null && a_eFontSet.ExIsValid()) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			a_oSender.ExSetText(a_oStr, CLocalizeInfoTable.Inst.GetFontSetInfo(a_eFontSet), a_bIsEnableAssert);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}

	/** 텍스트를 변경한다 */
	public static void ExSetText(this TMP_Text a_oSender, string a_oStr, EFontSet a_eFontSet = EFontSet._1, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_eFontSet.ExIsValid()));

		// 텍스트가 존재 할 경우
		if(a_oSender != null && a_eFontSet.ExIsValid()) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			a_oSender.ExSetText(a_oStr, CLocalizeInfoTable.Inst.GetFontSetInfo(a_eFontSet), a_bIsEnableAssert);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}

	/** 텍스트를 변경한다 */
	public static void ExSetText(this InputField a_oSender, string a_oStr, EFontSet a_eFontSet = EFontSet._1, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_eFontSet.ExIsValid()));

		// 텍스트가 존재 할 경우
		if(a_oSender != null && a_eFontSet.ExIsValid()) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			a_oSender.ExSetText(a_oStr, CLocalizeInfoTable.Inst.GetFontSetInfo(a_eFontSet), a_bIsEnableAssert);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}

	/** 텍스트를 변경한다 */
	public static void ExSetText(this TMP_InputField a_oSender, string a_oStr, EFontSet a_eFontSet = EFontSet._1, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_eFontSet.ExIsValid()));

		// 텍스트가 존재 할 경우
		if(a_oSender != null && a_eFontSet.ExIsValid()) {
#if NEWTON_SOFT_JSON_MODULE_ENABLE
			a_oSender.ExSetText(a_oStr, CLocalizeInfoTable.Inst.GetFontSetInfo(a_eFontSet), a_bIsEnableAssert);
#endif			// #if NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}

	/** 시작 콜백을 변경한다 */
	public static void ExSetBeginCallback(this CTouchDispatcher a_oSender, System.Action<CTouchDispatcher, PointerEventData> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		// 터치 전달자가 존재 할 경우
		if(a_oSender != null) {
			a_oSender.BeginCallback = a_oCallback;
		}
	}

	/** 이동 콜백을 변경한다 */
	public static void ExSetMoveCallback(this CTouchDispatcher a_oSender, System.Action<CTouchDispatcher, PointerEventData> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		// 터치 전달자가 존재 할 경우
		if(a_oSender != null) {
			a_oSender.MoveCallback = a_oCallback;
		}
	}

	/** 종료 콜백을 변경한다 */
	public static void ExSetEndCallback(this CTouchDispatcher a_oSender, System.Action<CTouchDispatcher, PointerEventData> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		// 터치 전달자가 존재 할 경우
		if(a_oSender != null) {
			a_oSender.EndCallback = a_oCallback;
		}
	}
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
