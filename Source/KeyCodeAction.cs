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
			if ( ModInput.GetKeyDown( null, Key ) )
			{
				DownTime = Time.unscaledTime;
			}
			else if ( ModInput.GetKey( null, Key ) && !HandledHold )
			{
				float HeldDuration = Time.unscaledTime - DownTime;
				if ( HeldDuration >= Settings.Instance.QuickSelectHoldDuration )
				{
					HandledHold = true;
					OnHold();
                }
			}

			if ( ModInput.GetKeyUp( null, Key ) )
			{
				HandledHold = false;
				OnTap();
			}
		}
    }

}
