using UnityEngine;
using System.Collections;

public class FairyLifeMovement : MonoBehaviour {
	
	public float moveDistance = 2f;
	public float moveDuration = 2f;

	private const float MOVE_LIMIT = 1f;
	
	private Transform _lifeObjTransform;
	private SpriteRenderer _lifeRenderer;
	private float _currentTime, _currentAlphaTime;
	private float _alphaDuration = 0.5f;
	private float _startPos = 1f;

	void Start(){
		_lifeObjTransform = this.transform;
		_lifeRenderer = _lifeObjTransform.FindChild("life").GetComponent<SpriteRenderer>();
		_startPos = moveDistance/2;
		_alphaDuration = moveDuration/2;
	}

	void Update(){
		_currentTime += Time.deltaTime;
		Color lifeColor = _lifeRenderer.color;

		if(_currentTime < moveDuration){
			_lifeObjTransform.localPosition = new Vector3(EaseInOutQuad(_currentTime, _startPos, -moveDistance, moveDuration), 0, 0);

			// Moving orb behind the fairy, fdae out
			if(_startPos < 0){
				if(_currentTime < _alphaDuration){
					lifeColor.a = EaseInOutQuad(_currentTime, 1, -1, _alphaDuration);
				}else if(_currentTime > moveDuration - _alphaDuration){
					_currentAlphaTime += Time.deltaTime;
					lifeColor.a = EaseInOutQuad(_currentAlphaTime, 0, 1, _alphaDuration);
				}
			}
		}else{
			_currentTime = 0;
			_currentAlphaTime = 0;
			_startPos *= -1;
			moveDistance *= -1;
		}
		_lifeRenderer.color = lifeColor;
	}

	float EaseInOutQuad(float currentTime, float start, float changeAmount, float duration) {
		currentTime /= duration/2;
		if (currentTime < 1) return changeAmount/2*currentTime*currentTime + start;
		currentTime--;
		return -changeAmount/2 * (currentTime*(currentTime-2) - 1) + start;
	}
}
