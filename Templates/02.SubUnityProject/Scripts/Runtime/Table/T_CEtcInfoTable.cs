#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 기타 정보 테이블 */
public partial class CEtcInfoTable : CSingleton<CEtcInfoTable> {
	#region 프로퍼티
	private string EtcInfoTablePath {
		get {
#if AB_TEST_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO_SET_B : KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO_SET_A;
#else
			return (CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? KCDefine.U_TABLE_P_G_ETC_INFO_SET_B : KCDefine.U_TABLE_P_G_ETC_INFO_SET_A;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#else
#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
			return KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO;
#else
			return KCDefine.U_TABLE_P_G_ETC_INFO;
#endif			// #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
#endif			// #if AB_TEST_ENABLE && NEWTON_SOFT_JSON_MODULE_ENABLE
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	/** 기타 정보를 리셋한다 */
	public virtual void ResetEtcInfos() {
		CCalcInfoTable.Inst.ResetCalcInfos();
		CEpisodeInfoTable.Inst.ResetEpisodeInfos();
		CTutorialInfoTable.Inst.ResetTutorialInfos();
		CFXInfoTable.Inst.ResetFXInfos();
	}

	/** 기타 정보를 리셋한다 */
	public virtual void ResetEtcInfos(string a_oJSONStr) {
		CCalcInfoTable.Inst.ResetCalcInfos(a_oJSONStr);
		CEpisodeInfoTable.Inst.ResetEpisodeInfos(a_oJSONStr);
		CTutorialInfoTable.Inst.ResetTutorialInfos(a_oJSONStr);
		CFXInfoTable.Inst.ResetFXInfos(a_oJSONStr);
	}

	/** 기타 정보를 로드한다 */
	public void LoadEtcInfos() {
		// Do Something
	}

	/** 기타 정보를 저장한다 */
	public void SaveEtcInfos(string a_oJSONStr) {
		CCalcInfoTable.Inst.SaveCalcInfos(a_oJSONStr);
		CEpisodeInfoTable.Inst.SaveEpisodeInfos(a_oJSONStr);
		CTutorialInfoTable.Inst.SaveTutorialInfos(a_oJSONStr);
		CFXInfoTable.Inst.SaveFXInfos(a_oJSONStr);
	}
	#endregion			// 함수
}
#endif			// #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif			// #if SCRIPT_TEMPLATE_ONLY
