using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ADS_ENABLE
//! 배너 광고 보정자
public class CBannerAdsCorrector : CComponent {
	#region 프로퍼티
	public Vector3 OriginPos { get; set; } = Vector3.zero;
	public Vector3 CorrectPos { get; set; } = Vector3.zero;
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		CScheduleManager.Instance.AddComponent(this);
		this.gameObject.ExRemoveComponents<CBannerAdsCorrector>(false);
	}

	//! 초기화
	public override void Start() {
		base.Start();
		this.OriginPos = this.transform.localPosition;
	}

	//! 상태를 갱신한다
	public override void OnUpdate(float a_fDeltaTime) {
		var stPos = this.OriginPos;
		stPos.y += CAdsManager.Instance.BannerAdsHeight;

		if(!stPos.ExIsEquals(this.CorrectPos)) {
			this.CorrectPos = stPos;
			this.transform.localPosition = stPos;
		}
	}

	//! 제거 되었을 경우
	public override void OnDestroy() {
		base.OnDestroy();

		if(!CSceneManager.IsAppQuit) {
			CScheduleManager.Instance.RemoveComponent(this);
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if UNITY_EDITOR
	//! 스크립트 순서를 설정한다
	protected override void SetupScriptOrder() {
		this.ExSetScriptOrder(KDefine.U_SCRIPT_ORDER_BANNER_ADS_CORRECTOR);
	}
#endif			// #if UNITY_EDITOR
	#endregion			// 조건부 함수
}
#endif			// #if ADS_ENABLE
