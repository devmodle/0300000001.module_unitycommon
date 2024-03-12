using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using System.Linq;

#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE

/** 공용 에디터 씬 관리자 - 버스트 */
public static partial class CCommonEditorSceneManager
{
	#region 클래스 함수
	/** 버스트 컴파일러를 설정한다 */
	private static void SetupBurst()
	{
#if BURST_MODULE_ENABLE && NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
		var oSettingsPathList = new List<string>() {
			KCEditorDefine.B_DATA_P_IOS_BURST_AOT_SETTINGS, 
			KCEditorDefine.B_DATA_P_ANDROID_BURST_AOT_SETTINGS, 
			KCEditorDefine.B_DATA_P_MAC_BURST_AOT_SETTINGS, 
			KCEditorDefine.B_DATA_P_WNDS_BURST_AOT_SETTINGS
		};

		for(int i = 0; i < oSettingsPathList.Count; ++i) {
			// 설정 파일이 없을 경우
			if(!File.Exists(oSettingsPathList[i])) {
				continue;
			}

			string oJSONStr = CFunc.ReadStr(oSettingsPathList[i], false);
			var oBurstAOTSettingsDatas = JsonConvert.DeserializeObject<JObject>(oJSONStr);

			// 데이터가 없을 경우
			if(oBurstAOTSettingsDatas == null || !oBurstAOTSettingsDatas.TryGetValue(KCEditorDefine.B_KEY_BURST_AS_MONO_BEHAVIOUR, out JToken oMonoBehaviourDatas)) {
				continue;
			}

			var oIsSetupOptsList = new List<bool>() {
				oMonoBehaviourDatas.Contains(KCEditorDefine.B_KEY_BURST_AS_ENABLE_OPTIMISATIONS) ? (bool)oMonoBehaviourDatas[KCEditorDefine.B_KEY_BURST_AS_ENABLE_OPTIMISATIONS] : false,
				oMonoBehaviourDatas.Contains(KCEditorDefine.B_KEY_BURST_AS_ENABLE_BURST_COMPILATION) ? (bool)oMonoBehaviourDatas[KCEditorDefine.B_KEY_BURST_AS_ENABLE_BURST_COMPILATION] : false,

				oMonoBehaviourDatas.Contains(KCEditorDefine.B_KEY_BURST_AS_ENABLE_SAFETY_CHECKS) ? (bool)oMonoBehaviourDatas[KCEditorDefine.B_KEY_BURST_AS_ENABLE_SAFETY_CHECKS] == false : false,
				oMonoBehaviourDatas.Contains(KCEditorDefine.B_KEY_BURST_AS_ENABLE_DEBUG_IN_ALL_BUILDS) ? (bool)oMonoBehaviourDatas[KCEditorDefine.B_KEY_BURST_AS_ENABLE_DEBUG_IN_ALL_BUILDS] == false : false,
				oMonoBehaviourDatas.Contains(KCEditorDefine.B_KEY_BURST_AS_USE_PLATFORM_SDK_LINKER) ? (bool)oMonoBehaviourDatas[KCEditorDefine.B_KEY_BURST_AS_USE_PLATFORM_SDK_LINKER] == false : false,

				oMonoBehaviourDatas.Contains(KCEditorDefine.B_KEY_BURST_AS_OPTIMIZE_FOR) ? (int)oMonoBehaviourDatas[KCEditorDefine.B_KEY_BURST_AS_OPTIMIZE_FOR] == (int)EBurstCompilerOptimization.SIZE : false
			};

			// 설정 갱신이 필요 없을 경우
			if(!oIsSetupOptsList.Contains(false)) {
				continue;
			}

			oMonoBehaviourDatas[KCEditorDefine.B_KEY_BURST_AS_ENABLE_OPTIMISATIONS] = true;
			oMonoBehaviourDatas[KCEditorDefine.B_KEY_BURST_AS_ENABLE_BURST_COMPILATION] = true;

			oMonoBehaviourDatas[KCEditorDefine.B_KEY_BURST_AS_ENABLE_SAFETY_CHECKS] = false;
			oMonoBehaviourDatas[KCEditorDefine.B_KEY_BURST_AS_ENABLE_DEBUG_IN_ALL_BUILDS] = false;
			oMonoBehaviourDatas[KCEditorDefine.B_KEY_BURST_AS_USE_PLATFORM_SDK_LINKER] = false;

			oMonoBehaviourDatas[KCEditorDefine.B_KEY_BURST_AS_OPTIMIZE_FOR] = (int)EBurstCompilerOptimization.SIZE;
			CFunc.WriteStr(oSettingsPathList[i], JsonConvert.SerializeObject(oBurstAOTSettingsDatas, Formatting.Indented), false);
		}
#endif // #if BURST_MODULE_ENABLE && NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
