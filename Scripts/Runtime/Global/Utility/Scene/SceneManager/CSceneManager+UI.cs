using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 씬 관리자 - UI
public abstract partial class CSceneManager : CComponent {
	#region 조건부 함수
#if UNITY_EDITOR
	//! 가이드 라인을 그린다
	public void EditorDrawGuideline() {
		float fScale = Func.GetResolutionScale(Application.isPlaying);

		var oScreenPositions = new Vector3[] {
			new Vector3(KDefine.B_SCREEN_WIDTH / -2.0f, KDefine.B_SCREEN_HEIGHT / -2.0f, 0.0f) * (KDefine.B_UNIT_SCALE * fScale),
			new Vector3(KDefine.B_SCREEN_WIDTH / -2.0f, KDefine.B_SCREEN_HEIGHT / 2.0f, 0.0f) * (KDefine.B_UNIT_SCALE * fScale),
			new Vector3(KDefine.B_SCREEN_WIDTH / 2.0f, KDefine.B_SCREEN_HEIGHT / 2.0f, 0.0f) * (KDefine.B_UNIT_SCALE * fScale),
			new Vector3(KDefine.B_SCREEN_WIDTH / 2.0f, KDefine.B_SCREEN_HEIGHT / -2.0f, 0.0f) * (KDefine.B_UNIT_SCALE * fScale)
		};

#if ADS_ENABLE
		var oAdsPositions = new Vector3[] {
			new Vector3((KDefine.B_SCREEN_WIDTH / -2.0f) * fScale, (KDefine.B_SCREEN_HEIGHT / -2.0f) + KDefine.U_OFFSET_BANNER_ADS, 0.0f) * KDefine.B_UNIT_SCALE,
			new Vector3(KDefine.B_SCREEN_WIDTH / -2.0f, KDefine.B_SCREEN_HEIGHT / 2.0f, 0.0f) * (KDefine.B_UNIT_SCALE * fScale),
			new Vector3(KDefine.B_SCREEN_WIDTH / 2.0f, KDefine.B_SCREEN_HEIGHT / 2.0f, 0.0f) * (KDefine.B_UNIT_SCALE * fScale),
			new Vector3((KDefine.B_SCREEN_WIDTH / 2.0f) * fScale, (KDefine.B_SCREEN_HEIGHT / -2.0f) + KDefine.U_OFFSET_BANNER_ADS, 0.0f) * KDefine.B_UNIT_SCALE
		};
#endif			// #if ADS_ENABLE

		for(int i = 0; i < oScreenPositions.Length; ++i) {
			var stPrevColor = Gizmos.color;

			int nIndexA = (i + 0) % oScreenPositions.Length;
			int nIndexB = (i + 1) % oScreenPositions.Length;

			Gizmos.color = Color.green;
			Gizmos.DrawLine(oScreenPositions[nIndexA], oScreenPositions[nIndexB]);

#if ADS_ENABLE
			Gizmos.color = Color.red;
			Gizmos.DrawLine(oAdsPositions[nIndexA], oAdsPositions[nIndexB]);
#endif			// #if ADS_ENABLE

			Gizmos.color = stPrevColor;
		}
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
