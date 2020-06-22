using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//! 내비게이션 관리자
public class CNavigationManager : CSingleton<CNavigationManager> {
	#region 변수
	private List<KeyValuePair<int, CComponent>> m_oComponentInfoList = new List<KeyValuePair<int, CComponent>>();
	#endregion			// 변수

	#region 함수
	//! 내비게이션 콜백을 수신했을 경우
	public void OnReceiveNavigationCallback(CComponent a_oSender) {
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
			a_oComponent.NavigationCallback = this.OnReceiveNavigationCallback;
			var stKeyValue = new KeyValuePair<int, CComponent>(nID, a_oComponent);

			a_oComponent.OnReceiveNavigationEvent(ENavigationEventType.TOP);
			m_oComponentInfoList.ExAddValue(stKeyValue);
		}
	}

	//! 컴포넌트를 제거한다
	public void RemoveComponent(CComponent a_oComponent) {
		Func.Assert(a_oComponent != null);
		int nID = a_oComponent.GetInstanceID();

		int nIndex = m_oComponentInfoList.ExFindValue((a_stComponentInfo) => {
			return nID == a_stComponentInfo.Key;
		});

		if(nIndex > KDefine.B_INDEX_INVALID) {
			for(int i = nIndex; i < m_oComponentInfoList.Count; ++i) {
				m_oComponentInfoList[i].Value.NavigationCallback = null;

				if(!m_oComponentInfoList[i].Value.IsDestroy) {
					m_oComponentInfoList[i].Value.OnReceiveNavigationEvent(ENavigationEventType.REMOVE);
				}
			}

			m_oComponentInfoList.RemoveRange(nIndex, m_oComponentInfoList.Count - nIndex);

			// 최상위 컴포넌트 정보가 변경 되었을 경우
			if(m_oComponentInfoList.ExIsValid()) {
				var oComponentInfo = m_oComponentInfoList.Last();
				oComponentInfo.Value.OnReceiveNavigationEvent(ENavigationEventType.TOP);
			}
		}
	}

	//! 내비게이션 이벤트를 전송한다
	public void SendNavigationEvent(ENavigationEventType a_eEventType) {
		if(m_oComponentInfoList.Count >= 1) {
			int nIndex = m_oComponentInfoList.Count - 1;
			m_oComponentInfoList[nIndex].Value.OnReceiveNavigationEvent(a_eEventType);
		}
	}
	#endregion			// 함수
}
