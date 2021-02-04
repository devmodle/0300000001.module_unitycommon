using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 시작 씬 관리자
public abstract class CStartSceneManager : CSceneManager {
	#region 프로퍼티
	public List<string> SpriteAtlasList { get; protected set; } = new List<string>();
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
		SpriteAtlasList.ExAddValue(KCDefine.U_ASSET_P_G_SPRITE_ATLAS_01);
	}

	//! 초기화
	public sealed override void Start() {
		base.Start();
		StartCoroutine(this.OnStart());
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		for(int i = 0; i < this.SpriteAtlasList.Count; ++i) {
			CResManager.Inst.LoadSpriteAtlas(this.SpriteAtlasList[i]);
		}
	}

	//! 초기화
	private IEnumerator OnStart() {
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

		this.Setup();
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

		CFunc.BroadcastMsg(KCDefine.SS_FUNC_N_START_SCENE_EVENT, EStartSceneEvent.LOAD_SETUP_SCENE);
		CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_SETUP);
	}
	#endregion			// 함수
}
