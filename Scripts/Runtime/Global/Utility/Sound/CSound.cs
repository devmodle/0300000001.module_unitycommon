using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! 사운드
public class CSound : CComponent {
	#region 컴포넌트
	private AudioSource m_oAudioSource = null;
	#endregion			// 컴포넌트

	#region 프로퍼티
	public bool IsPause { get; private set; } = false;
	public bool IsPlaying => m_oAudioSource.isPlaying;
	
	public bool IsMute {
		get {
			return m_oAudioSource.mute;
		} set {
			m_oAudioSource.mute = value;
		}
	}

	public float Volume {
		get {
			return m_oAudioSource.volume;
		} set {
			m_oAudioSource.volume = value;
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();

		m_oAudioSource = this.GetComponentInChildren<AudioSource>();
		m_oAudioSource.playOnAwake = false;
	}

	//! 사운드를 재생한다
	public void PlaySound(AudioClip a_oAudioClip, bool a_bIsLoop, bool a_bIs3DSound) {
		m_oAudioSource.clip = a_oAudioClip;
		m_oAudioSource.loop = a_bIsLoop;
		m_oAudioSource.dopplerLevel = a_bIs3DSound ? m_oAudioSource.dopplerLevel : 0.0f;
		m_oAudioSource.spatialBlend = a_bIs3DSound ? m_oAudioSource.spatialBlend : 0.0f;
		m_oAudioSource.reverbZoneMix = a_bIs3DSound ? m_oAudioSource.reverbZoneMix : 0.0f;
		
		this.IsPause = false;
		m_oAudioSource.Play();
	}

	//! 사운드를 재개한다
	public void ResumeSound() {
		this.IsPause = false;
		m_oAudioSource.UnPause();
	}

	//! 사운드를 정지한다
	public void PauseSound() {
		this.IsPause = true;
		m_oAudioSource.Pause();
	}

	//! 사운드를 중지한다
	public void StopSound() {
		this.IsPause = false;
		m_oAudioSource.Stop();
	}
	#endregion			// 함수
}
