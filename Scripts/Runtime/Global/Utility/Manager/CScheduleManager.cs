using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Timers;

//! 스케줄 관리자
public class CScheduleManager : CSingleton<CScheduleManager> {
	#region 변수
	private List<KeyValuePair<string, System.Action>> m_oCallbackInfoList = new List<KeyValuePair<string, System.Action>>();
	private List<KeyValuePair<string, System.Action>> m_oAddCallbackInfoList = new List<KeyValuePair<string, System.Action>>();
	private List<KeyValuePair<string, System.Action>> m_oRemoveCallbackInfoList = new List<KeyValuePair<string, System.Action>>();
	#endregion			// 변수

	#region 컴포넌트
	private List<KeyValuePair<int, CComponent>> m_oComponentInfoList = new List<KeyValuePair<int, CComponent>>();
	private List<KeyValuePair<int, CComponent>> m_oAddComponentInfoList = new List<KeyValuePair<int, CComponent>>();
	private List<KeyValuePair<int, CComponent>> m_oRemoveComponentInfoList = new List<KeyValuePair<int, CComponent>>();
	#endregion			// 컴포넌트
	
	#region 프로퍼티
	public float DeltaTime { get; private set; } = 0.0f;
	public float UnscaleDeltaTime { get; private set; } = 0.0f;
	#endregion			// 프로퍼티

	#region 함수
	//! 상태를 갱신한다
	public virtual void Update() {
		bool bIsEnableUpdate = m_oAddComponentInfoList.Count >= 1 || m_oRemoveComponentInfoList.Count >= 1;

		if(bIsEnableUpdate || m_oComponentInfoList.Count >= 1) {
			this.UpdateComponentState();

			this.DeltaTime = Mathf.Min(Time.deltaTime, (1.0f / Application.targetFrameRate) * 2.0f);
			this.UnscaleDeltaTime = Time.unscaledDeltaTime;

			for(int i = 0; i < m_oComponentInfoList.Count; ++i) {
				bool bIsDestroy = m_oComponentInfoList[i].Value.IsDestroy;

				if(!bIsDestroy && m_oComponentInfoList[i].Value.gameObject.activeSelf) {
					m_oComponentInfoList[i].Value.OnUpdate(this.DeltaTime);
				}
			}
		}
	}

	//! 상태를 갱신한다
	public virtual void LateUpdate() {
		bool bIsEnableUpdate = m_oAddCallbackInfoList.Count >= 1 || m_oRemoveCallbackInfoList.Count >= 1;

		if(bIsEnableUpdate || m_oCallbackInfoList.Count >= 1) {
			lock(KDefine.U_LOCK_OBJECT_UPDATE_M_UPDATE) {
				this.UpdateCallbackState();

				for(int i = 0; i < m_oCallbackInfoList.Count; ++i) {
					m_oCallbackInfoList[i].Value?.Invoke();
				}

				m_oCallbackInfoList.Clear();
			}
		}
	}

	//! 스케쥴 콜백을 수신했을 경우
	public void OnReceiveScheduleCallback(CComponent a_oSender) {
		this.RemoveComponent(a_oSender);
	}

	//! 컴포넌트를 추가한다
	public void AddComponent(CComponent a_oComponent) {
		Func.Assert(a_oComponent != null);
		int nID = a_oComponent.GetInstanceID();

		int nIndex = m_oComponentInfoList.ExFindValue((a_stComponentInfo) => {
			return nID == a_stComponentInfo.Key;
		});

		if(nIndex <= KDefine.B_INDEX_INVALID) {
			var stKeyValue = new KeyValuePair<int, CComponent>(nID, a_oComponent);
			m_oAddComponentInfoList.Add(stKeyValue);
		}
	}

	//! 콜백을 추가한다
	public void AddCallback(string a_oKey, System.Action a_oCallback) {
		Func.Assert(a_oKey.ExIsValid());

		lock(KDefine.U_LOCK_OBJECT_UPDATE_M_UPDATE) {
			int nIndex = m_oCallbackInfoList.ExFindValue((a_stCallbackInfo) => {
				return a_stCallbackInfo.Key.ExIsEquals(a_oKey);
			});

			if(nIndex <= KDefine.B_INDEX_INVALID) {
				var stKeyValue = new KeyValuePair<string, System.Action>(a_oKey, a_oCallback);
				m_oAddCallbackInfoList.Add(stKeyValue);
			}
		}
	}

	//! 타이머를 추가한다
	public void AddTimer(CComponent a_oComponent, float a_fDeltaTime, uint a_nRepeatTimes, UnityAction a_oCallback) {
		Func.Assert(a_oComponent != null && a_oCallback != null);
		TimersManager.SetTimer(a_oComponent, a_fDeltaTime, a_nRepeatTimes, a_oCallback);
	}

	//! 실시간 타이머를 추가한다
	public void AddRealtimeTimer(CComponent a_oComponent, float a_fDeltaTime, uint a_nRepeatTimes, UnityAction a_oCallback) {
		Func.Assert(a_oComponent != null && a_oCallback != null);

		TimersManager.SetTimer(a_oComponent,
			new Timer(a_fDeltaTime, a_nRepeatTimes, a_oCallback, TimerMode.REALTIME));
	}

	//! 반복 타이머를 추가한다
	public void AddRepeatTimer(CComponent a_oComponent, float a_fDeltaTime, UnityAction a_oCallback) {
		Func.Assert(a_oComponent != null && a_oCallback != null);
		TimersManager.SetLoopableTimer(a_oComponent, a_fDeltaTime, a_oCallback);
	}

	//! 컴포넌트를 제거한다
	public void RemoveComponent(CComponent a_oComponent) {
		Func.Assert(a_oComponent != null);
		int nID = a_oComponent.GetInstanceID();

		int nIndex = m_oComponentInfoList.ExFindValue((a_stComponentInfo) => {
			return nID == a_stComponentInfo.Key;
		});

		if(nIndex > KDefine.B_INDEX_INVALID) {
			var stKeyValue = new KeyValuePair<int, CComponent>(nID, a_oComponent);
			m_oRemoveComponentInfoList.Add(stKeyValue);
		}
	}

	//! 콜백을 제거한다
	public void RemoveCallback(string a_oKey) {
		Func.Assert(a_oKey.ExIsValid());

		lock(KDefine.U_LOCK_OBJECT_UPDATE_M_UPDATE) {
			int nIndex = m_oCallbackInfoList.ExFindValue((a_stCallbackInfo) => {
				return a_stCallbackInfo.Key.ExIsEquals(a_oKey);
			});

			if(nIndex > KDefine.B_INDEX_INVALID) {
				var stKeyValue = new KeyValuePair<string, System.Action>(a_oKey, null);
				m_oRemoveCallbackInfoList.Add(stKeyValue);
			}
		}
	}

	//! 타이머를 제거한다
	public void RemoveTimer(UnityAction a_oCallback) {
		TimersManager.ClearTimer(a_oCallback);
	}

	//! 컴포넌트 상태를 갱신한다
	private void UpdateComponentState() {
		for(int i = 0; i < m_oAddComponentInfoList.Count; ++i) {
			m_oAddComponentInfoList[i].Value.ScheduleCallback = this.OnReceiveScheduleCallback;
			var stKeyValue = new KeyValuePair<int, CComponent>(m_oAddComponentInfoList[i].Key, m_oAddComponentInfoList[i].Value);

			m_oComponentInfoList.ExAddValue(stKeyValue);
		}

		for(int i = 0; i < m_oRemoveComponentInfoList.Count; ++i) {
			int nIndex = m_oComponentInfoList.ExFindValue((a_stComponentInfo) => {
				return a_stComponentInfo.Key == m_oRemoveComponentInfoList[i].Key;
			});

			m_oRemoveComponentInfoList[i].Value.ScheduleCallback = null;
			m_oComponentInfoList.ExRemoveValueAt(nIndex);
		}

		m_oAddComponentInfoList.Clear();
		m_oRemoveComponentInfoList.Clear();
	}

	//! 콜백 상태를 갱신한다
	private void UpdateCallbackState() {
		for(int i = 0; i < m_oAddCallbackInfoList.Count; ++i) {
			var stKeyValue = new KeyValuePair<string, System.Action>(m_oAddCallbackInfoList[i].Key, m_oAddCallbackInfoList[i].Value);
			m_oCallbackInfoList.ExAddValue(stKeyValue);
		}

		for(int i = 0; i < m_oRemoveCallbackInfoList.Count; ++i) {
			int nIndex = m_oCallbackInfoList.ExFindValue((a_stCallbackInfo) => {
				return a_stCallbackInfo.Key.ExIsEquals(m_oRemoveCallbackInfoList[i].Key);
			});

			m_oCallbackInfoList.ExRemoveValueAt(nIndex);
		}

		m_oAddCallbackInfoList.Clear();
		m_oRemoveCallbackInfoList.Clear();
	}
	#endregion			// 함수
}
