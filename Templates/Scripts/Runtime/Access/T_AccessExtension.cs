using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
//! 기본 접근 확장 클래스
public static partial class AccessExtension {
	#region 클래스 함수
	//! 유효 여부를 검사한다
	public static bool ExIsValid(this EPriceType a_eSender) {
		return a_eSender > EPriceType.NONE && a_eSender < EPriceType.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this EPriceKinds a_eSender) {
		return a_eSender > EPriceKinds.NONE && a_eSender < EPriceKinds.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this EItemType a_eSender) {
		return a_eSender > EItemType.NONE && a_eSender < EItemType.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this EItemKinds a_eSender) {
		return a_eSender > EItemKinds.NONE && a_eSender < EItemKinds.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this ESaleItemType a_eSender) {
		return a_eSender > ESaleItemType.NONE && a_eSender < ESaleItemType.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this ESaleItemKinds a_eSender) {
		return a_eSender > ESaleItemKinds.NONE && a_eSender < ESaleItemKinds.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this ESaleProductType a_eSender) {
		return a_eSender > ESaleProductType.NONE && a_eSender < ESaleProductType.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this ESaleProductKinds a_eSender) {
		return a_eSender > ESaleProductKinds.NONE && a_eSender < ESaleProductKinds.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this EMissionType a_eSender) {
		return a_eSender > EMissionType.NONE && a_eSender < EMissionType.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this EMissionKinds a_eSender) {
		return a_eSender > EMissionKinds.NONE && a_eSender < EMissionKinds.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this ERewardType a_eSender) {
		return a_eSender > ERewardType.NONE && a_eSender < ERewardType.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this ERewardKinds a_eSender) {
		return a_eSender > ERewardKinds.NONE && a_eSender < ERewardKinds.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this ERewardQuality a_eSender) {
		return a_eSender > ERewardQuality.NONE && a_eSender < ERewardQuality.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this ERewardPopupType a_eSender) {
		return a_eSender > ERewardPopupType.NONE && a_eSender < ERewardPopupType.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this ETutorialType a_eSender) {
		return a_eSender > ETutorialType.NONE && a_eSender < ETutorialType.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this ETutorialKinds a_eSender) {
		return a_eSender > ETutorialKinds.NONE && a_eSender < ETutorialKinds.MAX_VAL;
	}

	//! 유효 여부를 검사한다
	public static bool ExIsValid(this ELevelMode a_eSender) {
		return a_eSender > ELevelMode.NONE && a_eSender < ELevelMode.MAX_VAL;
	}
	
	//! 컴포넌트 상호 작용 여부를 변경한다
	public static void ExSetInteractable(this Button a_oSender, bool a_bIsEnable) {
		CAccess.Assert(a_oSender != null);

		var oTouchInteractable = a_oSender.GetComponentInChildren<CTouchInteractable>();
		oTouchInteractable?.SetInteractable(a_bIsEnable);
	}

	//! 종류 => 타입으로 변환한다
	public static int ExKindsToType(this int a_nSender) {
		return a_nSender / KCDefine.B_UNIT_TYPE_TO_KINDS;
	}
	
	//! 종류 => 기본 종류로 변환한다
	public static int ExKindsToBaseKinds(this int a_nSender) {
		int nType = a_nSender.ExKindsToType();
		return nType * KCDefine.B_UNIT_TYPE_TO_KINDS;
	}

	//! 종류 => 종류 타입으로 변환한다
	public static int ExKindsToKindsType(this int a_nSender) {
		int nBaseKinds = a_nSender.ExKindsToBaseKinds();
		int nKindsType = a_nSender % KCDefine.B_UNIT_TYPE_TO_KINDS;
		
		return nBaseKinds + ((nKindsType / KCDefine.B_UNIT_TYPE_TO_KINDS_TYPE) * KCDefine.B_UNIT_TYPE_TO_KINDS_TYPE);
	}

	//! 종류 => 서브 종류 타입으로 변환한다
	public static int ExKindsToSubKindsType(this int a_nSender) {
		int nKindsType = a_nSender.ExKindsToKindsType();
		int nSubKindsType = a_nSender % KCDefine.B_UNIT_TYPE_TO_KINDS_TYPE;

		return nKindsType + ((nSubKindsType / KCDefine.B_UNIT_TYPE_TO_SUB_KINDS_TYPE) * KCDefine.B_UNIT_TYPE_TO_SUB_KINDS_TYPE);
	}
	#endregion			// 클래스 함수
}
#endif			// #if NEVER_USE_THIS
