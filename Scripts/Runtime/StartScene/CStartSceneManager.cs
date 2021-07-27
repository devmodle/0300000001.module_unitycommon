using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 시작 씬 관리자
public abstract class CStartSceneManager : CSceneManager {
	#region 변수
	protected List<string> m_oSpriteAtlasList = new List<string>();
	protected Dictionary<string, int> m_oMaxNumDuplicateFXSndsDict = new Dictionary<string, int>();
	#endregion			// 변수

	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_START;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_O_START_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 추상 함수
	//! 시작 씬 이벤트를 수신했을 경우
	protected abstract void OnReceiveStartSceneEvent(EStartSceneEvent a_eEvent);
	#endregion			// 추상 함수

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			m_oSpriteAtlasList.ExAddVal(KCDefine.U_ASSET_P_G_SPRITE_ATLAS_01);
		}
	}

	//! 초기화
	public sealed override void Start() {
		base.Start();

		// 초기화 되었을 경우
		if(CSceneManager.IsInit) {
			StartCoroutine(this.OnStart());
		}
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		for(int i = 0; i < this.m_oSpriteAtlasList.Count; ++i) {
			CResManager.Inst.LoadSpriteAtlas(this.m_oSpriteAtlasList[i]);
		}

		foreach(var stKeyVal in this.m_oMaxNumDuplicateFXSndsDict) {
			CSndManager.Inst.SetMaxNumDuplicateFXSnds(stKeyVal.Key, stKeyVal.Value);
		}
	}

	//! 초기화
	private IEnumerator OnStart() {
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);

		this.Setup();
		yield return CFactory.CreateWaitForSecs(KCDefine.U_DELAY_INIT);

		CFunc.BroadcastMsg(KCDefine.SS_FUNC_N_START_SCENE_EVENT, EStartSceneEvent.LOAD_SETUP_SCENE, false);
		CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_SETUP);
	}
	#endregion			// 함수
}
