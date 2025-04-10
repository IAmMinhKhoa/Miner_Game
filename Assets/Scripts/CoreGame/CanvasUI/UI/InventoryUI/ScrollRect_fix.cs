	using UnityEngine.UI;

public class ScrollRect_fix : ScrollRect
{

    override protected void LateUpdate()
    {

        base.LateUpdate();

        if (this.verticalScrollbar)
        {

            this.verticalScrollbar.size = 0.05f;
        }
		if (this.horizontalScrollbar)
		{
			this.horizontalScrollbar.size = 0.05f;
		}
    }

    override public void Rebuild(CanvasUpdate executing)
    {

        base.Rebuild(executing);

        if (this.verticalScrollbar)
        {
			this.verticalScrollbar.value = 1;
            this.verticalScrollbar.size = 0.05f;
        }
		if (this.horizontalScrollbar)
		{
			this.horizontalScrollbar.size = 0.05f;
		}
	}
}
