using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 시작 씬 관리자
public abstract class CStartSceneManager : CSceneManager {
	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_NAME_START;

#if UNITY_EDITOR
	public override int ScriptOrder => KCDefine.U_SCRIPT_ORDER_START_SCENE_MANAGER;
#endif			// #if UNITY_EDITOR
	#endregion			// 프로퍼티

	#region 추상 함수
	//! 약관 동의 씬 관리자 이벤트를 수신했을 경우
	protected abstract void OnReceiveAgreeSceneManagerEvent(EAgreeSceneManagerEventType a_eEventType);
	#endregion			// 추상 함수

	#region 함수
	//! 초기화
	public sealed override void Start() {
		base.Start();
		StartCoroutine(this.OnStart());
	}

	//! 씬을 설정한다
	protected virtual void Setup() {
		// Do Nothing
	}

	//! 초기화
	private IEnumerator OnStart() {
		CAccess.Assert(!CSceneManager.IsStart);
		yield return CFactory.CreateWaitForSeconds(KCDefine.U_DELAY_INIT);

		this.Setup();

		CSceneManager.IsStart = true;
		CSceneLoader.Instance.LoadAdditiveScene(KCDefine.B_SCENE_NAME_SETUP);
	}
	#endregion			// 함수
}
