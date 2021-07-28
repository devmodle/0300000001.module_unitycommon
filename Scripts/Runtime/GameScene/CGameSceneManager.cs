using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! 게임 씬 관리자
public class CGameSceneManager : CSceneManager {
	#region 객체
	protected GameObject m_oBlockObjs = null;
	#endregion			// 객체

	#region 프로퍼티
	public override string SceneName => KCDefine.B_SCENE_N_GAME;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 초기화 되었을 경우
		if(CSceneManager.IsAppInit) {
			// 블럭 객체를 설정한다 {
			var oBlockObjsA = this.SubObjs.ExFindChild(KCDefine.GS_OBJ_N_BLOCKS);
			var oBlockObjsB = this.SubCanvasObjs.ExFindChild(KCDefine.GS_OBJ_N_BLOCKS);

			m_oBlockObjs = (oBlockObjsA != null) ? oBlockObjsA : oBlockObjsB;
			m_oBlockObjs = (m_oBlockObjs != null) ? m_oBlockObjs : CFactory.CreateObj(KCDefine.GS_OBJ_N_BLOCKS, this.SubObjs);
			// 블럭 객체를 설정한다 }
		}
	}
	#endregion			// 함수
}
