using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if NEVER_USE_THIS
#if UNITY_EDITOR || UNITY_STANDALONE
//! 서브 에디터 레벨 생성 팝업
public class T_CSubEditorLevelCreatePopup : CEditorLevelCreatePopup {
	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
	}

	//! 초기화
	public override void Init(System.Action<CEditorLevelCreatePopup, STEditorLevelCreateInfo> a_oCallback) {
		base.Init(a_oCallback);
		this.UpdateUIsState();
	}

	//! UI 상태를 갱신한다
	protected new void UpdateUIsState() {
		// Do Something
	}
	#endregion			// 함수
}
#endif			// #if UNITY_EDITOR || UNITY_STANDALONE
#endif			// #if NEVER_USE_THIS