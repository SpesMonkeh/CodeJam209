using System.Globalization;
using P209;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class DebugPanel : MonoBehaviour
{
	[SerializeField] GameObject debugPanel;
	
	[Header("Debug Panel Button")]
	[SerializeField] Button debugPanelButton;
	[SerializeField] TMP_Text debugPanelButtonTMP;

	[Header("Debug Fields")]
	[SerializeField] TMP_Text primaryTouchPositionTMP;

	[Header("Gyroscope")]
	[SerializeField] TMP_Text xGyroTMP;
	[SerializeField] TMP_Text yGyroTMP;
	[SerializeField] TMP_Text zGyroTMP;

	const string SHOW_DEBUG_PANEL = "Vis Debug";
	const string HIDE_DEBUG_PANEL = "Skjul Debug";
	
	void OnEnable()
	{
		debugPanelButton.onClick.AddListener(OnDebugButtonPressed);
		
		InputManager.primaryTouchAction += OnPrimaryTouchAction;
		InputManager.angularVelocityAction += OnAngularVelocity;
		
		debugPanel.SetActive(false);
	}

	void OnDisable()
	{
		debugPanelButton.onClick.RemoveListener(OnDebugButtonPressed);
		
		InputManager.primaryTouchAction -= OnPrimaryTouchAction;
		InputManager.angularVelocityAction -= OnAngularVelocity;
	}

	void OnDebugButtonPressed()
	{
		bool isActive = !debugPanel.activeInHierarchy;
		debugPanel.SetActive(isActive);
		debugPanelButtonTMP.text = isActive ? HIDE_DEBUG_PANEL : SHOW_DEBUG_PANEL;
	}
	
	void OnPrimaryTouchAction(Vector2 pos) { }
	
	void OnAngularVelocity(Vector3 vel)
	{
		if (debugPanel.activeInHierarchy is false) return;
		
		xGyroTMP.text = vel.x.ToString(CultureInfo.InvariantCulture);
		yGyroTMP.text = vel.y.ToString(CultureInfo.InvariantCulture);
		zGyroTMP.text = vel.z.ToString(CultureInfo.InvariantCulture);
	}
}
