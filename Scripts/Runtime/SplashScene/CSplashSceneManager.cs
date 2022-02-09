using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
using UnityEngine.Rendering.Universal;
#endif			// #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

/** 스플래시 씬 관리자 */
public abstract class CSplashSceneManager : CSceneManager {
	#region 변수
	protected List<string> m_oSpriteAtlasPathList = new List<string>();
	#endregion			// 변수

	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_SPLASH;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_O_SPLASH_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 추상 함수
	/** 스플래시를 출력한다 */
	protected abstract void ShowSplash();
	#endregion			// 추상 함수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			for(int i = 0; i < KCDefine.B_VAL_9_INT; ++i) {
				m_oSpriteAtlasPathList.ExAddVal(string.Format(KCDefine.U_ASSET_P_FMT_SPRITE_ATLAS, i + KCDefine.B_VAL_1_INT));
				m_oSpriteAtlasPathList.ExAddVal(string.Format(KCDefine.U_ASSET_P_FMT_G_SPRITE_ATLAS, i + KCDefine.B_VAL_1_INT));
				m_oSpriteAtlasPathList.ExAddVal(string.Format(KCDefine.U_ASSET_P_FMT_G_FIX_PF_SPRITE_ATLAS, i + KCDefine.B_VAL_1_INT));
			}
		}
	}

	/** 초기화 */
	public sealed override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			for(int i = 0; i < m_oSpriteAtlasPathList.Count; ++i) {
				CResManager.Inst.LoadSpriteAtlas(m_oSpriteAtlasPathList[i]);
			}

			StartCoroutine(this.OnStart());
		}
	}

	/** 다음 씬을 로드한다 */
	protected void LoadNextScene() {
		CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_START, false);
	}

	/** 초기화 */
	private IEnumerator OnStart() {
		this.ShowSplash();
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);

		// 디바이스 정보를 설정한다 {
		var oTargetFrameInfoDict = new Dictionary<RuntimePlatform, (int, int)>() {
			// 모바일
			[RuntimePlatform.Android] = (CValTable.Inst.GetInt(KCDefine.VT_KEY_MOBILE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.VT_KEY_MOBILE_TARGET_FRAME_RATE)),
			[RuntimePlatform.IPhonePlayer] = (CValTable.Inst.GetInt(KCDefine.VT_KEY_MOBILE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.VT_KEY_MOBILE_TARGET_FRAME_RATE)),

			// 콘솔 {
			[RuntimePlatform.PS4] = (CValTable.Inst.GetInt(KCDefine.VT_KEY_CONSOLE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.VT_KEY_CONSOLE_TARGET_FRAME_RATE)),
			[RuntimePlatform.PS5] = (CValTable.Inst.GetInt(KCDefine.VT_KEY_CONSOLE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.VT_KEY_CONSOLE_TARGET_FRAME_RATE)),

			[RuntimePlatform.XboxOne] = (CValTable.Inst.GetInt(KCDefine.VT_KEY_CONSOLE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.VT_KEY_CONSOLE_TARGET_FRAME_RATE)),
			[RuntimePlatform.GameCoreXboxOne] = (CValTable.Inst.GetInt(KCDefine.VT_KEY_CONSOLE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.VT_KEY_CONSOLE_TARGET_FRAME_RATE)),
			[RuntimePlatform.GameCoreXboxSeries] = (CValTable.Inst.GetInt(KCDefine.VT_KEY_CONSOLE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.VT_KEY_CONSOLE_TARGET_FRAME_RATE)),
			// 콘솔 }

			// 휴대용 콘솔
			[RuntimePlatform.Stadia] = (CValTable.Inst.GetInt(KCDefine.VT_KEY_HANDHELD_CONSOLE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.VT_KEY_HANDHELD_CONSOLE_TARGET_FRAME_RATE)),
			[RuntimePlatform.Switch] = (CValTable.Inst.GetInt(KCDefine.VT_KEY_HANDHELD_CONSOLE_QUALITY_LEVEL), CValTable.Inst.GetInt(KCDefine.VT_KEY_HANDHELD_CONSOLE_TARGET_FRAME_RATE))
		};

#if MULTI_TOUCH_ENABLE
		Input.multiTouchEnabled = true;
#else
		Input.multiTouchEnabled = false;
#endif			// #if MULTI_TOUCH_ENABLE

		CSceneManager.SetupQuality(oTargetFrameInfoDict.ContainsKey(Application.platform) ? (EQualityLevel)oTargetFrameInfoDict[Application.platform].Item1 : (EQualityLevel)CValTable.Inst.GetInt(KCDefine.VT_KEY_DEF_QUALITY_LEVEL), true);
		Application.targetFrameRate = Mathf.Min(Screen.currentResolution.refreshRate, oTargetFrameInfoDict.ContainsKey(Application.platform) ? oTargetFrameInfoDict[Application.platform].Item2 : CValTable.Inst.GetInt(KCDefine.VT_KEY_DEF_TARGET_FRAME_RATE));
		// 디바이스 정보를 설정한다 }
	}
	#endregion			// 함수
}
