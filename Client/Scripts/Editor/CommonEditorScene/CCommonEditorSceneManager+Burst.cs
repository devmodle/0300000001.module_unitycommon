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
		var oListPathSettings = new List<string>() {
			KCDefineEditor.G_BURST_DATA_P_SETTINGS_AOT_IOS, 
			KCDefineEditor.G_BURST_DATA_P_SETTINGS_AOT_ANDROID, 
			KCDefineEditor.G_BURST_DATA_P_SETTINGS_AOT_MAC, 
			KCDefineEditor.G_BURST_DATA_P_SETTINGS_AOT_WNDS
		};

		for(int i = 0; i < oListPathSettings.Count; ++i) {
			// 설정 파일이 없을 경우
			if(!File.Exists(oListPathSettings[i])) {
				continue;
			}

			string oJSONStr = CFunc.ReadStr(oListPathSettings[i], false);
			var oBurstAOTSettingsDatas = JsonConvert.DeserializeObject<JObject>(oJSONStr);

			// 데이터가 없을 경우
			if(oBurstAOTSettingsDatas == null || !oBurstAOTSettingsDatas.TryGetValue(KCDefineEditor.G_BURST_KEY_BEHAVIOUR_MONO, out JToken oMonoBehaviourDatas)) {
				continue;
			}

			var oListIsSetupOpts = new List<bool>() {
				oMonoBehaviourDatas.Contains(KCDefineEditor.G_BURST_KEY_ENABLE_OPTIMISATIONS) ? (bool)oMonoBehaviourDatas[KCDefineEditor.G_BURST_KEY_ENABLE_OPTIMISATIONS] : false,
				oMonoBehaviourDatas.Contains(KCDefineEditor.G_BURST_KEY_ENABLE_COMPILATION_BURST) ? (bool)oMonoBehaviourDatas[KCDefineEditor.G_BURST_KEY_ENABLE_COMPILATION_BURST] : false,

				oMonoBehaviourDatas.Contains(KCDefineEditor.G_BURST_KEY_ENABLE_CHECKS_SAFETY) ? (bool)oMonoBehaviourDatas[KCDefineEditor.G_BURST_KEY_ENABLE_CHECKS_SAFETY] == false : false,
				oMonoBehaviourDatas.Contains(KCDefineEditor.G_BURST_KEY_ENABLE_BUILDS_ALL_IN_DEBUG) ? (bool)oMonoBehaviourDatas[KCDefineEditor.G_BURST_KEY_ENABLE_BUILDS_ALL_IN_DEBUG] == false : false,
				oMonoBehaviourDatas.Contains(KCDefineEditor.G_BURST_KEY_LINKER_SDK_PLATFORM_USE) ? (bool)oMonoBehaviourDatas[KCDefineEditor.G_BURST_KEY_LINKER_SDK_PLATFORM_USE] == false : false,

				oMonoBehaviourDatas.Contains(KCDefineEditor.G_BURST_KEY_FOR_OPTIMIZE) ? (int)oMonoBehaviourDatas[KCDefineEditor.G_BURST_KEY_FOR_OPTIMIZE] == (int)EBurstCompilerOptimization.SIZE : false
			};

			// 설정 갱신이 필요 없을 경우
			if(!oListIsSetupOpts.Contains(false)) {
				continue;
			}

			oMonoBehaviourDatas[KCDefineEditor.G_BURST_KEY_ENABLE_OPTIMISATIONS] = true;
			oMonoBehaviourDatas[KCDefineEditor.G_BURST_KEY_ENABLE_COMPILATION_BURST] = true;

			oMonoBehaviourDatas[KCDefineEditor.G_BURST_KEY_ENABLE_CHECKS_SAFETY] = false;
			oMonoBehaviourDatas[KCDefineEditor.G_BURST_KEY_ENABLE_BUILDS_ALL_IN_DEBUG] = false;
			oMonoBehaviourDatas[KCDefineEditor.G_BURST_KEY_LINKER_SDK_PLATFORM_USE] = false;

			oMonoBehaviourDatas[KCDefineEditor.G_BURST_KEY_FOR_OPTIMIZE] = (int)EBurstCompilerOptimization.SIZE;
			CFunc.WriteStr(oListPathSettings[i], JsonConvert.SerializeObject(oBurstAOTSettingsDatas, Formatting.Indented), false);
		}
#endif // #if BURST_MODULE_ENABLE && NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR
