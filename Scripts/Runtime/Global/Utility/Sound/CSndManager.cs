using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//! 사운드 관리자
public class CSndManager : CSingleton<CSndManager> {
	#region 변수
	private bool m_bIsMuteFXSnds = false;
	private float m_fFXSndsVolume = 0.0f;

	private string m_oBGSndFilepath = string.Empty;
	#endregion			// 변수

	#region 컴포넌트
	private CSnd m_oBGSnd = null;
	private Dictionary<string, List<CSnd>> m_oFXSndListContainer = new Dictionary<string, List<CSnd>>();
	#endregion			// 컴포넌트

	#region 프로퍼티
	public bool IsDisableVibrate { get; set; } = false;
	public bool IsPlayingBGSnd => m_oBGSnd.IsPlaying;

	public bool IsMuteBGSnd {
		get {
			return m_oBGSnd.IsMute;
		} set {
			m_oBGSnd.IsMute = value;
		}
	}

	public bool IsMuteFXSnds {
		get {
			return m_bIsMuteFXSnds;
		} set {
			m_bIsMuteFXSnds = value;

			this.EnumerateFXSnds((a_oKey, a_oSnd) => {
				a_oSnd.IsMute = value;
			});
		}
	}

	public float BGSndVolume {
		get {
			return m_oBGSnd.Volume;
		} set {
			m_oBGSnd.Volume = value;
		}
	}

	public float FXSndsVolume {
		get {
			return m_fFXSndsVolume;
		} set {
			this.SetFXSndsVolume(value);
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 배경음을 생성한다 {
		m_oBGSnd = Func.CreateCloneObj<CSnd>(KDefine.U_OBJ_NAME_SND_M_BG_SND,
			CResourceManager.Instance.GetPrefab(KDefine.U_OBJ_PATH_BG_SND), this.gameObject);

		m_oBGSnd.transform.localPosition = Vector3.zero;
		// 배경음을 생성한다 }
	}

	//! 효과음 볼륨을 변경한다
	public void SetFXSndsVolume(float a_fVolume, bool a_bIsByForce = true) {
		m_fFXSndsVolume = a_fVolume;

		this.EnumerateFXSnds((a_oKey, a_oSnd) => {
			if(a_bIsByForce) {
				a_oSnd.Volume = a_fVolume;
			} else {
				a_oSnd.Volume = a_oSnd.Volume.ExIsEquals(a_fVolume) ? a_fVolume : a_oSnd.Volume;
			}
		});
	}

	//! 진동을 시작한다
	public void Vibrate() {
		if(!this.IsDisableVibrate && SystemInfo.supportsVibration) {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
			Handheld.Vibrate();
#endif			// #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		}
	}

	//! 단일음 재생힌다
	public void PlayOneShotSnd(string a_oFilepath, Vector3 a_stPos) {
		Func.Assert(a_oFilepath.ExIsValid());
		
		if(!this.IsMuteFXSnds) {
			AudioSource.PlayClipAtPoint(CResourceManager.Instance.GetAudioClip(a_oFilepath), a_stPos, this.FXSndsVolume);
		}
	}

	//! 배경음을 재생힌다
	public CSnd PlayBGSnd(string a_oFilepath, bool a_bIsLoop = true) {
		Func.Assert(a_oFilepath.ExIsValid());

		if(!m_oBGSnd.IsPlaying || !m_oBGSndFilepath.ExIsEquals(a_oFilepath)) {
			m_oBGSnd.IsMute = this.IsMuteBGSnd;
			m_oBGSnd.Volume = this.BGSndVolume;
			
			m_oBGSndFilepath = a_oFilepath;
			m_oBGSnd.PlaySnd(CResourceManager.Instance.GetAudioClip(a_oFilepath), a_bIsLoop, false);
		}

		return m_oBGSnd;
	}

	//! 효과음을 재생한다
	public CSnd PlayFXSnd(string a_oFilepath, float a_fVolume = 0.0f, bool a_bIsLoop = false) {
		return this.PlayFXSnd(a_oFilepath, CSceneManager.MainCamera.transform.position, a_fVolume, a_bIsLoop);
	}

	//! 효과음을 재생한다
	public CSnd PlayFXSnd(string a_oFilepath, Vector3 a_stPos, float a_fVolume = 0.0f, bool a_bIsLoop = false) {
		Func.Assert(a_oFilepath.ExIsValid());
		var oSnd = this.FindPlayableFXSnd(a_oFilepath);

		if(oSnd != null) {
			oSnd.IsMute = this.IsMuteFXSnds;
			oSnd.Volume = a_fVolume.ExIsEquals(0.0f) ? this.FXSndsVolume : a_fVolume;
			oSnd.transform.position = a_stPos;

			bool bIs3DSnd = !CSceneManager.MainCamera.transform.position.ExIsEquals(a_stPos);
			oSnd.PlaySnd(CResourceManager.Instance.GetAudioClip(a_oFilepath), a_bIsLoop, bIs3DSnd);
		}

		return oSnd;
	}

	//! 배경음을 재개한다
	public void ResumeBGSnd() {
		m_oBGSnd.ResumeSnd();
	}

	//! 효과음을 재개한다
	public void ResumeFXSnds() {
		this.EnumerateFXSnds((a_oKey, a_oSnd) => {
			a_oSnd.ResumeSnd();
		});
	}

	//! 배경음을 정지한다
	public void PauseBGSnd() {
		m_oBGSnd.PauseSnd();
	}

	//! 효과음을 정지한다
	public void PauseFXSnds() {
		this.EnumerateFXSnds((a_oKey, a_oSnd) => {
			a_oSnd.ResumeSnd();
		});
	}

	//! 배경음을 중지한다
	public void StopBGSnd() {
		m_oBGSnd.StopSnd();
	}

	//! 효과음을 중지한다
	public void StopFXSnds() {
		this.EnumerateFXSnds((a_oKey, a_oSnd) => {
			a_oSnd.StopSnd();
		});
	}

	//! 재생 가능한 효과음을 탐색한다
	private CSnd FindPlayableFXSnd(string a_oKey) {
		if(!m_oFXSndListContainer.ContainsKey(a_oKey)) {
			m_oFXSndListContainer.Add(a_oKey, new List<CSnd>());
		}

		var oFXSndList = m_oFXSndListContainer[a_oKey];

		// 최대 중첩 가능한 개수를 벗어났을 경우
		if(oFXSndList.Count >= KDefine.U_MAX_NUM_DUPLICATE_FX_SNDS) {
			for(int i = 0; i < oFXSndList.Count; ++i) {
				if(!oFXSndList[i].IsPause && !oFXSndList[i].IsPlaying) {
					return oFXSndList[i];
				}
			}

			return null;
		}

		// 효과음을 생성한다 {
		var oSnd = Func.CreateCloneObj<CSnd>(KDefine.U_OBJ_NAME_SND_M_FX_SND,
			CResourceManager.Instance.GetPrefab(KDefine.U_OBJ_PATH_FX_SND), this.gameObject);

		oSnd.transform.localPosition = Vector3.zero;
		oFXSndList.Add(oSnd);
		// 효과음을 생성한다 }

		return oSnd;
	}

	//! 효과음을 순회한다
	private void EnumerateFXSnds(System.Action<string, CSnd> a_oCallback) {
		foreach(var stKeyValue in m_oFXSndListContainer) {
			for(int i = 0; i < stKeyValue.Value.Count; ++i) {
				a_oCallback?.Invoke(stKeyValue.Key, stKeyValue.Value[i]);
			}
		}
	}
	#endregion			// 함수

	#region 조건부 함수
#if HAPTIC_FEEDBACK_ENABLE
	//! 진동을 시작한다
	public void Vibrate(float a_fDuration, 
		EVibrateType a_eType = EVibrateType.IMPACT, EVibrateStyle a_eStyle = EVibrateStyle.LIGHT, float a_fIntensity = KDefine.U_DEF_INTENSITY_VIBRATE) {
		if(!this.IsDisableVibrate && SystemInfo.supportsVibration) {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
			if(!Func.IsSupportHapticFeedback()) {
				this.Vibrate();
			} else {
				float fDuration = Mathf.Clamp01(a_fDuration);
				float fIntensity = Mathf.Clamp01(a_fIntensity);

				CUnityMsgSender.Instance.SendVibrateMsg(a_eType, a_eStyle, fDuration, fIntensity);
			}
#endif			// #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		}
	}
#endif			// #if HAPTIC_FEEDBACK_ENABLE
	#endregion			// 조건부 함수
}
