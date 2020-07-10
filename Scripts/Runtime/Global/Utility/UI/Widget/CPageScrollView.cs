using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

//! 페이지 스크롤 뷰
public class CPageScrollView : CUIComponent {
	#region 변수
	private int m_nPage = 0;
	private bool m_bIsPaging = false;
	private Vector2 m_stDragStartPos = Vector2.zero;

	private Tweener m_oAnimation = null;

	[SerializeField] private float m_fPageDistance = KDefine.U_DEF_DISTANCE_PAGE;
	[SerializeField] private EScrollDirection m_eDirection = EScrollDirection.NONE;
	#endregion			// 변수

	#region 컴포넌트
	private ScrollRect m_oScrollView = null;

	private RectTransform m_oContentTransform = null;
	private RectTransform m_oViewportTransform = null;
	#endregion			// 컴포넌트

	#region 프로퍼티
	public int Page {
		get {
			return m_nPage;
		} set {
			m_nPage = Mathf.Clamp(value, 0, this.LastPage);
			this.PageScrollView(m_nPage);
		}
	}

	public int LastPage {
		get {
			int nLastPage = 1;

			if(m_eDirection == EScrollDirection.VERTICAL) {
				nLastPage = (int)(m_oContentTransform.rect.height / m_oViewportTransform.rect.height);

				if(m_oContentTransform.rect.height > (m_oViewportTransform.rect.height * nLastPage)) {
					nLastPage += 1;
				}
			} else if(m_eDirection == EScrollDirection.HORIZONTAL) {
				nLastPage = (int)(m_oContentTransform.rect.width / m_oViewportTransform.rect.width);

				if(m_oContentTransform.rect.width > (m_oViewportTransform.rect.width * nLastPage)) {
					nLastPage += 1;
				}
			}

			return Mathf.Max(0, nLastPage - 1);
		}
	}
	#endregion			// 프로퍼티

	#region 함수
	//! 초기화
	public override void Awake() {
		base.Awake();
		m_oScrollView = this.GetComponentInChildren<ScrollRect>();

		// 트랜스 폼을 설정한다 {
		var oContent = this.gameObject.ExFindChild(KDefine.U_OBJ_NAME_SCROLL_V_CONTENT);
		var oViewport = this.gameObject.ExFindChild(KDefine.U_OBJ_NAME_SCROLL_V_VIEWPORT);

		m_oContentTransform = oContent.transform as RectTransform;
		m_oViewportTransform = oViewport.transform as RectTransform;
		// 트랜스 폼을 설정한다 }

		// 드래그 전달자를 설정한다
		var oDragDispatcher = this.GetComponentInChildren<CDragDispatcher>();
		oDragDispatcher.BeganCallback = this.OnDragBegan;
		oDragDispatcher.DragCallback = this.OnDrag;
		oDragDispatcher.EndedCallback = this.OnDragEnded;
	}

	//! 활성화 되었을 경우
	public override void OnEnable() {
		base.OnEnable();
		this.UpdateScrollViewState(m_eDirection);
	}

	//! 비활성화 되었을 경우
	public override void OnDisable() {
		base.OnDisable();
		this.UpdateScrollViewState(EScrollDirection.NONE);
	}

	//! 제거 되었을 경우
	public override void OnDestroy() {
		base.OnDisable();

		if(!CSceneManager.IsAppQuit) {
			this.ResetAnimation();
		}
	}

	//! 드래그를 시작했을 경우
	public void OnDragBegan(CDragDispatcher a_oSender, PointerEventData a_oEventData) {
		float fDeltaX = Mathf.Abs(a_oEventData.delta.x);
		float fDeltaY = Mathf.Abs(a_oEventData.delta.y);

		if(m_eDirection == EScrollDirection.VERTICAL) {
			m_bIsPaging = fDeltaY > fDeltaX;
			m_oScrollView.horizontal = fDeltaX > fDeltaY;
		} else if(m_eDirection == EScrollDirection.HORIZONTAL) {
			m_bIsPaging = fDeltaX > fDeltaY;
			m_oScrollView.vertical = fDeltaY > fDeltaX;
		}

		m_stDragStartPos = a_oEventData.position;
	}

	//! 드래그 중 일 경우
	public void OnDrag(CDragDispatcher a_oSender, PointerEventData a_oEventData) {
		if(m_bIsPaging) {
			if(m_eDirection == EScrollDirection.VERTICAL) {
				m_oContentTransform.anchoredPosition += new Vector2(0.0f, a_oEventData.delta.y * KDefine.U_DEF_SCALE_PAGE_SCROLL);
			} else if(m_eDirection == EScrollDirection.HORIZONTAL) {
				m_oContentTransform.anchoredPosition += new Vector2(a_oEventData.delta.x * KDefine.U_DEF_SCALE_PAGE_SCROLL, 0.0f);
			}
		}
	}

	//! 드래그를 종료했을 경우
	public void OnDragEnded(CDragDispatcher a_oSender, PointerEventData a_oEventData) {
		if(m_bIsPaging) {
			var stDelta = a_oEventData.position - m_stDragStartPos;

			// 스크롤 거리보다 작을 경우
			if(stDelta.magnitude <= m_fPageDistance) {
				this.Page = m_nPage;
			} else {
				int nOffset = 0;

				if(m_eDirection == EScrollDirection.VERTICAL) {
					nOffset = stDelta.y.ExIsLessEquals(0.0f) ? -1 : 1;
				} else if(m_eDirection == EScrollDirection.HORIZONTAL) {
					nOffset = stDelta.x.ExIsLessEquals(0.0f) ? 1 : -1;
				}

				this.Page = m_nPage + nOffset;
			}
		}

		m_bIsPaging = false;
		this.UpdateScrollViewState(m_eDirection);
	}

	//! 페이지 거리를 변경한다
	public void SetPageDistance(float a_fDistance) {
		m_fPageDistance = a_fDistance;
	}

	//! 애니메이션을 리셋한다
	private void ResetAnimation() {
		m_oAnimation?.Kill();
	}

	//! 스크롤 뷰 상태를 갱신한다
	private void UpdateScrollViewState(EScrollDirection a_eDirection) {
		m_oScrollView.vertical = a_eDirection == EScrollDirection.HORIZONTAL;
		m_oScrollView.horizontal = a_eDirection == EScrollDirection.VERTICAL;
	}

	//! 스크롤 뷰를 페이지한다
	private void PageScrollView(int a_nPage) {
		this.ResetAnimation();

		if(m_eDirection == EScrollDirection.VERTICAL) {
			m_oAnimation = m_oContentTransform.DOAnchorPos(new Vector3(0.0f, a_nPage * m_oViewportTransform.rect.height, 0.0f),
				KDefine.U_DEF_DURATION_SCROLL_ANIMATION).SetEase(Ease.Linear);
		} else if(m_eDirection == EScrollDirection.HORIZONTAL) {
			m_oAnimation = m_oContentTransform.DOAnchorPos(new Vector3(a_nPage * -m_oViewportTransform.rect.width, 0.0f, 0.0f),
				KDefine.U_DEF_DURATION_SCROLL_ANIMATION).SetEase(Ease.Linear);
		}
	}
	#endregion			// 함수
}
