using UnityEngine;

namespace QualityOfLife
{
    internal class KeyCodeAction
	{
        private float DownTime;

		public Func<KeyCode> KeyCodeFunc = () => KeyCode.None;
        public bool HandledHold { get; private set; }

		public Action OnHold = () => { };
		public Action OnTap = () => { };


		public void Update()
		{
			KeyCode Key = KeyCodeFunc();
			if ( Input.GetKeyDown( Key ) )
			{
				DownTime = Time.unscaledTime;
			}
			else if ( Input.GetKey( Key ) && !HandledHold )
			{
				float HeldDuration = Time.unscaledTime - DownTime;
				if ( HeldDuration >= Settings.Instance.QuickSelectHoldDuration )
				{
					HandledHold = true;
					OnHold();
                }
			}

			if ( Input.GetKeyUp( Key ) )
			{
				HandledHold = false;
				OnTap();
			}
		}
    }

}
