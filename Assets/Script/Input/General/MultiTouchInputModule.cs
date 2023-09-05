using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.EventSystems
{
	public class MultiTouchInputModule : PointerInputModule
	{	
		private PinchEventData _pinchData;
		private RotateEventData _rotateData;
		
		private enum _MultiTouchMode
		{
			Idle, // no touch input.
			Began, // 2 input touches received.
			Pinching, // touches have passed the min pinch threshold
			Rotating // touches have passed the min rotating threshold
		}
		;
		
		private _MultiTouchMode _touchMode = _MultiTouchMode.Idle;
		
		private Vector2 _prevVector = Vector2.zero;
		
		private float _stationaryTime;
		
		public float minRotationThreshold = 10;
		public float minPinchThreshold = 10;
		public float stationaryTimeThreshold = 1;
		
		protected override void Start ()
		{
			_pinchData = new PinchEventData (eventSystem);
			_rotateData = new RotateEventData (eventSystem);
		}
		
		public override void Process ()
		{
			if (Input.touchCount == 1) {
				bool pressed, released;
				PointerEventData touchData = GetTouchPointerEventData (Input.GetTouch (0), out pressed, out released);
				
				eventSystem.RaycastAll (touchData, m_RaycastResultCache);
				RaycastResult firstHit = FindFirstRaycast (m_RaycastResultCache);
				
				if (Input.GetTouch (0).phase == TouchPhase.Began) {
					ExecuteEvents.Execute (firstHit.gameObject, touchData, ExecuteEvents.beginDragHandler);
				}
				
				if (Input.GetTouch (0).phase == TouchPhase.Moved) {
					ExecuteEvents.Execute (firstHit.gameObject, touchData, ExecuteEvents.dragHandler);
				}
				
				if (Input.GetTouch (0).phase == TouchPhase.Ended) {
					ExecuteEvents.Execute (firstHit.gameObject, touchData, ExecuteEvents.endDragHandler);
				}
				
			} else if (Input.touchCount == 2) {
				bool pressed0, released0;
				bool pressed1, released1;
				
				PointerEventData touchData0 = GetTouchPointerEventData (Input.GetTouch (0), out pressed0, out released0);
				PointerEventData touchData1 = GetTouchPointerEventData (Input.GetTouch (1), out pressed1, out released1);
				
				eventSystem.RaycastAll (touchData0, m_RaycastResultCache);
				RaycastResult firstHit0 = FindFirstRaycast (m_RaycastResultCache);
				
				eventSystem.RaycastAll (touchData1, m_RaycastResultCache);
				RaycastResult firstHit1 = FindFirstRaycast (m_RaycastResultCache);
				
				if (Input.GetTouch (0).phase == TouchPhase.Began || Input.GetTouch (1).phase == TouchPhase.Began) {
					_prevVector = Input.GetTouch (1).position - Input.GetTouch (0).position;
					_touchMode = _MultiTouchMode.Began;
				}
				
				if (Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetTouch (1).phase == TouchPhase.Moved) {
					if (firstHit0.gameObject != null && firstHit1.gameObject != null) {
						if (firstHit0.gameObject.Equals (firstHit1.gameObject)) {
							bool executeHandler = DetectMultiTouchMotion ();
							
							if (executeHandler) {
								if (_touchMode == _MultiTouchMode.Pinching) {
									_pinchData.data [0] = touchData0;
									_pinchData.data [1] = touchData1;
									
									ExecuteEvents.Execute (firstHit0.gameObject, _pinchData, MultiTouchModuleEvents.pinchHandler);
								} else if (_touchMode == _MultiTouchMode.Rotating) {
									_rotateData.data [0] = touchData0;
									_rotateData.data [1] = touchData1;
									
									ExecuteEvents.Execute (firstHit0.gameObject, _rotateData, MultiTouchModuleEvents.rotateHandler);
								}
							}
						}
					}
				}
				
				//check for ended or cancelled touched fingers and set the mode back to "idle".
				
				if (Input.GetTouch (0).phase == TouchPhase.Ended || Input.GetTouch (1).phase == TouchPhase.Ended ||
					Input.GetTouch (0).phase == TouchPhase.Canceled || Input.GetTouch (1).phase == TouchPhase.Canceled) {
					
					_touchMode = _MultiTouchMode.Idle;
					_prevVector = Vector2.zero;
				}
				
				
				//check for stationary fingers and set the mode back to "Began" if above the threshold.
				
				if (Input.GetTouch (0).phase == TouchPhase.Stationary || Input.GetTouch (1).phase == TouchPhase.Stationary) {
					_stationaryTime += Time.deltaTime;
					
					if (_stationaryTime > stationaryTimeThreshold) {
						_touchMode = _MultiTouchMode.Began;
						_prevVector = Input.GetTouch (1).position - Input.GetTouch (0).position;
						_stationaryTime = 0;
					}
				}
			}
		}
		
		bool DetectMultiTouchMotion ()
		{
			if (_touchMode == _MultiTouchMode.Began) {
				Vector2 currentVector = Input.GetTouch (1).position - Input.GetTouch (0).position;
				
				//check for rotation threshold
				
				float angleOffset = Vector2.Angle (_prevVector, currentVector);
				
				if (angleOffset > minRotationThreshold) {
					_touchMode = _MultiTouchMode.Rotating;
					_prevVector = currentVector;
				}
				
				// check for pinch threshold
				
				if (Mathf.Abs (currentVector.magnitude - _prevVector.magnitude) > minPinchThreshold) {
					_touchMode = _MultiTouchMode.Pinching;
					_prevVector = currentVector;
				}					
				
				return false;
				
			} else if (_touchMode == _MultiTouchMode.Rotating) {
				Vector2 currentVector = Input.GetTouch (1).position - Input.GetTouch (0).position;
				
				float rotateDelta = Vector2.Angle (_prevVector, currentVector);
				
				// to get the direction of rotation
				Vector3 dirVec = Vector3.Cross (_prevVector, currentVector);
				
				_prevVector = currentVector;
				
				_rotateData.rotateDelta = dirVec.z < 0 ? -rotateDelta : rotateDelta;
				
				return true;
				
			} else if (_touchMode == _MultiTouchMode.Pinching) {
				Vector2 currentVector = Input.GetTouch (1).position - Input.GetTouch (0).position;
				
				float pinchDelta = currentVector.magnitude - _prevVector.magnitude;
				
				_prevVector = currentVector;
				
				_pinchData.pinchDelta = pinchDelta;
				
				return true;
			}
			
			return false;
		}
		
		public override string ToString ()
		{
			return string.Format ("[MultiTouchInputModule]");
		}
	}
	
	public class PinchEventData : BaseEventData
	{
		public PointerEventData[] data = new PointerEventData[2];
		public float pinchDelta;
		
		public PinchEventData (EventSystem ES, float d = 0) : base (ES)
		{
			pinchDelta = d;
		}
	}
	
	public class RotateEventData : BaseEventData
	{
		public PointerEventData[] data = new PointerEventData[2];
		public float rotateDelta;
		
		public RotateEventData (EventSystem ES, float d = 0) : base (ES)
		{
			rotateDelta = d;
		}
	}
	
	public interface IPinchHandler : IEventSystemHandler
	{
		void OnPinch (PinchEventData data);
	}
	
	public interface IRotateHandler : IEventSystemHandler
	{
		void OnRotate (RotateEventData data);
	}
	
	
	public static class MultiTouchModuleEvents
	{
		private static void Execute (IPinchHandler handler, BaseEventData eventData)
		{
			handler.OnPinch (ExecuteEvents.ValidateEventData<PinchEventData> (eventData));
		}
		
		private static void Execute (IRotateHandler handler, BaseEventData eventData)
		{
			handler.OnRotate (ExecuteEvents.ValidateEventData<RotateEventData> (eventData));
		}
		
		public static ExecuteEvents.EventFunction<IPinchHandler> pinchHandler {
			get { return Execute; }
		}
		
		public static ExecuteEvents.EventFunction<IRotateHandler> rotateHandler {
			get { return Execute; }
		}
	}
}
