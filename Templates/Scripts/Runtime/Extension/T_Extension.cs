using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if SCRIPT_TEMPLATE_ONLY
#if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
/** 기본 확장 클래스 */
public static partial class Extension {
	#region 클래스 함수
	/** 종류 => 타입으로 변환한다 */
	public static EPriceType ExKindsToType(this EPriceKinds a_eSender) {
		return (EPriceType)((int)a_eSender).ExKindsToType();
	}

	/** 종류 => 타입으로 변환한다 */
	public static EItemType ExKindsToType(this EItemKinds a_eSender) {
		return (EItemType)((int)a_eSender).ExKindsToType();
	}

	/** 종류 => 타입으로 변환한다 */
	public static ESaleItemType ExKindsToType(this ESaleItemKinds a_eSender) {
		return (ESaleItemType)((int)a_eSender).ExKindsToType();
	}

	/** 종류 => 타입으로 변환한다 */
	public static ESaleProductType ExKindsToType(this ESaleProductKinds a_eSender) {
		return (ESaleProductType)((int)a_eSender).ExKindsToType();
	}

	/** 종류 => 타입으로 변환한다 */
	public static EMissionType ExKindsToType(this EMissionKinds a_eSender) {
		return (EMissionType)((int)a_eSender).ExKindsToType();
	}

	/** 종류 => 타입으로 변환한다 */
	public static ERewardType ExKindsToType(this ERewardKinds a_eSender) {
		return (ERewardType)((int)a_eSender).ExKindsToType();
	}

	/** 종류 => 타입으로 변환한다 */
	public static ELevelType ExKindsToType(this ELevelKinds a_eSender) {
		return (ELevelType)((int)a_eSender).ExKindsToType();
	}

	/** 종류 => 타입으로 변환한다 */
	public static EStageType ExKindsToType(this EStageKinds a_eSender) {
		return (EStageType)((int)a_eSender).ExKindsToType();
	}

	/** 종류 => 타입으로 변환한다 */
	public static EChapterType ExKindsToType(this EChapterKinds a_eSender) {
		return (EChapterType)((int)a_eSender).ExKindsToType();
	}

	/** 종류 => 타입으로 변환한다 */
	public static ETutorialType ExKindsToType(this ETutorialKinds a_eSender) {
		return (ETutorialType)((int)a_eSender).ExKindsToType();
	}

	/** 종류 => 기본 종류로 변환한다 */
	public static EPriceKinds ExKindsToBaseKinds(this EPriceKinds a_eSender) {
		return (EPriceKinds)((int)a_eSender).ExKindsToBaseKinds();
	}

	/** 종류 => 기본 종류로 변환한다 */
	public static EItemKinds ExKindsToBaseKinds(this EItemKinds a_eSender) {
		return (EItemKinds)((int)a_eSender).ExKindsToBaseKinds();
	}

	/** 종류 => 기본 종류로 변환한다 */
	public static ESaleItemKinds ExKindsToBaseKinds(this ESaleItemKinds a_eSender) {
		return (ESaleItemKinds)((int)a_eSender).ExKindsToBaseKinds();
	}

	/** 종류 => 기본 종류로 변환한다 */
	public static ESaleProductKinds ExKindsToBaseKinds(this ESaleProductKinds a_eSender) {
		return (ESaleProductKinds)((int)a_eSender).ExKindsToBaseKinds();
	}

	/** 종류 => 기본 종류로 변환한다 */
	public static EMissionKinds ExKindsToBaseKinds(this EMissionKinds a_eSender) {
		return (EMissionKinds)((int)a_eSender).ExKindsToBaseKinds();
	}

	/** 종류 => 기본 종류로 변환한다 */
	public static ERewardKinds ExKindsToBaseKinds(this ERewardKinds a_eSender) {
		return (ERewardKinds)((int)a_eSender).ExKindsToBaseKinds();
	}

	/** 종류 => 기본 종류로 변환한다 */
	public static ELevelKinds ExKindsToBaseKinds(this ELevelKinds a_eSender) {
		return (ELevelKinds)((int)a_eSender).ExKindsToBaseKinds();
	}

	/** 종류 => 기본 종류로 변환한다 */
	public static EStageKinds ExKindsToBaseKinds(this EStageKinds a_eSender) {
		return (EStageKinds)((int)a_eSender).ExKindsToBaseKinds();
	}

	/** 종류 => 기본 종류로 변환한다 */
	public static EChapterKinds ExKindsToBaseKinds(this EChapterKinds a_eSender) {
		return (EChapterKinds)((int)a_eSender).ExKindsToBaseKinds();
	}

	/** 종류 => 기본 종류로 변환한다 */
	public static ETutorialKinds ExKindsToBaseKinds(this ETutorialKinds a_eSender) {
		return (ETutorialKinds)((int)a_eSender).ExKindsToBaseKinds();
	}

	/** 종류 => 종류 타입으로 변환한다 */
	public static EPriceKinds ExKindsToKindsType(this EPriceKinds a_eSender) {
		return (EPriceKinds)((int)a_eSender).ExKindsToKindsType();
	}

	/** 종류 => 종류 타입으로 변환한다 */
	public static EItemKinds ExKindsToKindsType(this EItemKinds a_eSender) {
		return (EItemKinds)((int)a_eSender).ExKindsToKindsType();
	}

	/** 종류 => 종류 타입으로 변환한다 */
	public static ESaleItemKinds ExKindsToKindsType(this ESaleItemKinds a_eSender) {
		return (ESaleItemKinds)((int)a_eSender).ExKindsToKindsType();
	}

	/** 종류 => 종류 타입으로 변환한다 */
	public static ESaleProductKinds ExKindsToKindsType(this ESaleProductKinds a_eSender) {
		return (ESaleProductKinds)((int)a_eSender).ExKindsToKindsType();
	}

	/** 종류 => 종류 타입으로 변환한다 */
	public static EMissionKinds ExKindsToKindsType(this EMissionKinds a_eSender) {
		return (EMissionKinds)((int)a_eSender).ExKindsToKindsType();
	}

	/** 종류 => 종류 타입으로 변환한다 */
	public static ERewardKinds ExKindsToKindsType(this ERewardKinds a_eSender) {
		return (ERewardKinds)((int)a_eSender).ExKindsToKindsType();
	}

	/** 종류 => 종류 타입으로 변환한다 */
	public static ELevelKinds ExKindsToKindsType(this ELevelKinds a_eSender) {
		return (ELevelKinds)((int)a_eSender).ExKindsToKindsType();
	}

	/** 종류 => 종류 타입으로 변환한다 */
	public static EStageKinds ExKindsToKindsType(this EStageKinds a_eSender) {
		return (EStageKinds)((int)a_eSender).ExKindsToKindsType();
	}

	/** 종류 => 종류 타입으로 변환한다 */
	public static EChapterKinds ExKindsToKindsType(this EChapterKinds a_eSender) {
		return (EChapterKinds)((int)a_eSender).ExKindsToKindsType();
	}

	/** 종류 => 종류 타입으로 변환한다 */
	public static ETutorialKinds ExKindsToKindsType(this ETutorialKinds a_eSender) {
		return (ETutorialKinds)((int)a_eSender).ExKindsToKindsType();
	}

	/** 종류 => 서브 종류 타입으로 변환한다 */
	public static EPriceKinds ExKindsToSubKindsType(this EPriceKinds a_eSender) {
		return (EPriceKinds)((int)a_eSender).ExKindsToSubKindsType();
	}

	/** 종류 => 서브 종류 타입으로 변환한다 */
	public static EItemKinds ExKindsToSubKindsType(this EItemKinds a_eSender) {
		return (EItemKinds)((int)a_eSender).ExKindsToSubKindsType();
	}

	/** 종류 => 서브 종류 타입으로 변환한다 */
	public static ESaleItemKinds ExKindsToSubKindsType(this ESaleItemKinds a_eSender) {
		return (ESaleItemKinds)((int)a_eSender).ExKindsToSubKindsType();
	}

	/** 종류 => 서브 종류 타입으로 변환한다 */
	public static ESaleProductKinds ExKindsToSubKindsType(this ESaleProductKinds a_eSender) {
		return (ESaleProductKinds)((int)a_eSender).ExKindsToSubKindsType();
	}

	/** 종류 => 서브 종류 타입으로 변환한다 */
	public static EMissionKinds ExKindsToSubKindsType(this EMissionKinds a_eSender) {
		return (EMissionKinds)((int)a_eSender).ExKindsToSubKindsType();
	}

	/** 종류 => 서브 종류 타입으로 변환한다 */
	public static ERewardKinds ExKindsToSubKindsType(this ERewardKinds a_eSender) {
		return (ERewardKinds)((int)a_eSender).ExKindsToSubKindsType();
	}

	/** 종류 => 서브 종류 타입으로 변환한다 */
	public static ELevelKinds ExKindsToSubKindsType(this ELevelKinds a_eSender) {
		return (ELevelKinds)((int)a_eSender).ExKindsToSubKindsType();
	}

	/** 종류 => 서브 종류 타입으로 변환한다 */
	public static EStageKinds ExKindsToSubKindsType(this EStageKinds a_eSender) {
		return (EStageKinds)((int)a_eSender).ExKindsToSubKindsType();
	}

	/** 종류 => 서브 종류 타입으로 변환한다 */
	public static EChapterKinds ExKindsToSubKindsType(this EChapterKinds a_eSender) {
		return (EChapterKinds)((int)a_eSender).ExKindsToSubKindsType();
	}

	/** 종류 => 서브 종류 타입으로 변환한다 */
	public static ETutorialKinds ExKindsToSubKindsType(this ETutorialKinds a_eSender) {
		return (ETutorialKinds)((int)a_eSender).ExKindsToSubKindsType();
	}

	/** 종류 => 타입으로 변환한다 */
	public static EBlockType ExKindsToType(this EBlockKinds a_eSender) {
		return (EBlockType)((int)a_eSender).ExKindsToType();
	}

	/** 종류 => 기본 종류로 변환한다 */
	public static EBlockKinds ExKindsToBaseKinds(this EBlockKinds a_eSender) {
		return (EBlockKinds)((int)a_eSender).ExKindsToBaseKinds();
	}

	/** 종류 => 종류 타입으로 변환한다 */
	public static EBlockKinds ExKindsToKindsType(this EBlockKinds a_eSender) {
		return (EBlockKinds)((int)a_eSender).ExKindsToKindsType();
	}

	/** 종류 => 서브 종류 타입으로 변환한다 */
	public static EBlockKinds ExKindsToSubKindsType(this EBlockKinds a_eSender) {
		return (EBlockKinds)((int)a_eSender).ExKindsToSubKindsType();
	}
	
	/** JSON 문자열 => 지급 아이템 정보로 변환한다 */
	public static List<STPostItemInfo> ExJSONStrToPostItemInfos(this string a_oSender) {
		CAccess.Assert(a_oSender.ExIsValid());
		var oPostItemInfoList = new List<STPostItemInfo>();

#if FIREBASE_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
		var oJSONNode = SimpleJSON.JSON.Parse(a_oSender) as SimpleJSON.JSONClass;
		var oPostItemInfos = oJSONNode[KCDefine.B_KEY_JSON_ROOT_DATA];

		for(int i = 0; i < oPostItemInfos.Count; ++i) {
			var oPostItemInfoStr = oPostItemInfos[i].ToString();
			oPostItemInfoList.Add(oPostItemInfoStr.ExJSONStrToObj<STPostItemInfo>());
		}
#endif			// #if FIREBASE_MODULE_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE

		return oPostItemInfoList;
	}
	#endregion			// 클래스 함수
}
#endif			// #if EXTRA_SCRIPT_ENABLE && RUNTIME_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
