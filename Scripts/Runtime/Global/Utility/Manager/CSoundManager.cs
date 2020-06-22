using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//! 사운드 관리자
public class CSoundManager : CSingleton<CSoundManager> {
	#region 변수
	private bool m_bIsMuteFXSounds = false;
	private float m_fFXSoundsVolume = 0.0f;

	private CSound m_oBGSound = null;
	private string m_oBGSoundFilepath = string.Empty;
	private Dictionary<string, List<CSound>> m_oFXSoundListContainer = new Dictionary<string, List<CSound>>();
	#endregion			// 변수

	#region 프로퍼티
	public bool IsDisableVibrate { get; set; } = false;
	public bool IsPlayingBGSound => m_oBGSound.IsPlaying;

	public bool IsMuteBGSound {
		get {
			return m_oBGSound.IsMute;
		} set {
			m_oBGSound.IsMute = value;
		}
	}

	public bool IsMuteFXSounds {
		get {
			return m_bIsMuteFXSounds;
		} set {
			m_bIsMuteFXSounds = value;

			this.EnumerateFXSounds((a_oKey, a_oSound) => {
				a_oSound.IsMute = value;
			});
		}
	}

	public float BGSoundVolume {
		get {
			return m_oBGSound.Volume;
		} set {
			m_oBGSound.Volume = value;
		}
	}

	public float FXSoundsVolume {
		get {
			return m_fFXSoundsVolume;
		} set {
			this.SetFXSoundsVolume(value);
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		// 배경음을 생성한다 {
		m_oBGSound = Func.CreateCloneGameObject<CSound>(KDefine.U_OBJ_NAME_SOUND_M_BG_SOUND,
			CResourceManager.Instance.GetGameObject(KDefine.U_OBJ_PATH_BG_SOUND), this.gameObject);

		m_oBGSound.transform.localPosition = Vector3.zero;
		// 배경음을 생성한다 }
	}

	//! 효과음 볼륨을 변경한다
	public void SetFXSoundsVolume(float a_fVolume, bool a_bIsByForce = true) {
		m_fFXSoundsVolume = a_fVolume;

		this.EnumerateFXSounds((a_oKey, a_oSound) => {
			if(a_bIsByForce) {
				a_oSound.Volume = a_fVolume;
			} else {
				a_oSound.Volume = a_oSound.Volume.ExIsEquals(a_fVolume) ? a_fVolume : a_oSound.Volume;
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
	public void PlayOneShotSound(string a_oFilepath, Vector3 a_stPos) {
		Func.Assert(a_oFilepath.ExIsValid());
		
		if(!this.IsMuteFXSounds) {
			AudioSource.PlayClipAtPoint(CResourceManager.Instance.GetAudioClip(a_oFilepath), a_stPos, this.FXSoundsVolume);
		}
	}

	//! 배경음을 재생힌다
	public CSound PlayBGSound(string a_oFilepath, bool a_bIsLoop = true) {
		Func.Assert(a_oFilepath.ExIsValid());

		if(!m_oBGSound.IsPlaying || !m_oBGSoundFilepath.ExIsEquals(a_oFilepath)) {
			m_oBGSound.IsMute = this.IsMuteBGSound;
			m_oBGSound.Volume = this.BGSoundVolume;
			
			m_oBGSoundFilepath = a_oFilepath;
			m_oBGSound.PlaySound(CResourceManager.Instance.GetAudioClip(a_oFilepath), a_bIsLoop, false);
		}

		return m_oBGSound;
	}

	//! 효과음을 재생한다
	public CSound PlayFXSound(string a_oFilepath, float a_fVolume = 0.0f, bool a_bIsLoop = false) {
		return this.PlayFXSound(a_oFilepath, CSceneManager.MainCamera.transform.position, a_fVolume, a_bIsLoop);
	}

	//! 효과음을 재생한다
	public CSound PlayFXSound(string a_oFilepath, Vector3 a_stPos, float a_fVolume = 0.0f, bool a_bIsLoop = false) {
		Func.Assert(a_oFilepath.ExIsValid());
		var oSound = this.FindPlayableFXSound(a_oFilepath);

		if(oSound != null) {
			oSound.IsMute = this.IsMuteFXSounds;
			oSound.Volume = a_fVolume.ExIsEquals(0.0f) ? this.FXSoundsVolume : a_fVolume;
			oSound.transform.position = a_stPos;

			bool bIs3DSound = !CSceneManager.MainCamera.transform.position.ExIsEquals(a_stPos);
			oSound.PlaySound(CResourceManager.Instance.GetAudioClip(a_oFilepath), a_bIsLoop, bIs3DSound);
		}

		return oSound;
	}

	//! 배경음을 재개한다
	public void ResumeBGSound() {
		m_oBGSound.ResumeSound();
	}

	//! 효과음을 재개한다
	public void ResumeFXSounds() {
		this.EnumerateFXSounds((a_oKey, a_oSound) => {
			a_oSound.ResumeSound();
		});
	}

	//! 배경음을 정지한다
	public void PauseBGSound() {
		m_oBGSound.PauseSound();
	}

	//! 효과음을 정지한다
	public void PauseFXSounds() {
		this.EnumerateFXSounds((a_oKey, a_oSound) => {
			a_oSound.ResumeSound();
		});
	}

	//! 배경음을 중지한다
	public void StopBGSound() {
		m_oBGSound.StopSound();
	}

	//! 효과음을 중지한다
	public void StopFXSounds() {
		this.EnumerateFXSounds((a_oKey, a_oSound) => {
			a_oSound.StopSound();
		});
	}

	//! 재생 가능한 효과음을 탐색한다
	private CSound FindPlayableFXSound(string a_oKey) {
		if(!m_oFXSoundListContainer.ContainsKey(a_oKey)) {
			m_oFXSoundListContainer.Add(a_oKey, new List<CSound>());
		}

		var oFXSoundList = m_oFXSoundListContainer[a_oKey];

		// 최대 중첩 가능한 개수를 벗어났을 경우
		if(oFXSoundList.Count >= KDefine.U_MAX_NUM_DUPLICATE_FX_SOUNDS) {
			for(int i = 0; i < oFXSoundList.Count; ++i) {
				if(!oFXSoundList[i].IsPause && !oFXSoundList[i].IsPlaying) {
					return oFXSoundList[i];
				}
			}

			return null;
		}

		// 효과음을 생성한다 {
		var oSound = Func.CreateCloneGameObject<CSound>(KDefine.U_OBJ_NAME_SOUND_M_FX_SOUND,
			CResourceManager.Instance.GetGameObject(KDefine.U_OBJ_PATH_FX_SOUND), this.gameObject);

		oSound.transform.localPosition = Vector3.zero;
		oFXSoundList.Add(oSound);
		// 효과음을 생성한다 }

		return oSound;
	}

	//! 효과음을 순회한다
	private void EnumerateFXSounds(System.Action<string, CSound> a_oCallback) {
		foreach(var stKeyValue in m_oFXSoundListContainer) {
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

				CUnityMessageSender.Instance.SendVibrateMessage(a_eType, a_eStyle, fDuration, fIntensity);
			}
#endif			// #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		}
	}
#endif			// #if HAPTIC_FEEDBACK_ENABLE
	#endregion			// 조건부 함수
}
