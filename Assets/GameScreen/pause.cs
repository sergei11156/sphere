using UnityEngine;
using UnityEngine.UI;

public class pause: MonoBehaviour
{
	bool paus = false;
	public Sprite pausePic;
	public Sprite unPausePic;
	public ToMenu toMenu;

	public void Pause()
	{
		if (!paus)
		{
			Time.timeScale = 0;
			paus = true;
			GetComponent<Image>().sprite = unPausePic;
		} else
		{
			Time.timeScale = 1;
			GetComponent<Image>().sprite = pausePic;
			paus = false;
		}
		toMenu.ChangeState();
	}
}
