using UnityEngine;
using UnityEngine.Events;

namespace Assignment1
{
    [RequireComponent(typeof(LineRenderer))]
    public class TreeNodeVisual : MonoBehaviour
    {
        private LineRenderer _lr;
        public UnityEvent<string> DisplayInfo;
        private LineRenderer lr
        {
            get
            {
                if (_lr == null)
                {
                    _lr = GetComponent<LineRenderer>();
                }
                return _lr;
            }
        }

        public void SetInfo(string info)
        {
            DisplayInfo?.Invoke(info);
        }
  
        public void Update()
        {
            var childCount = transform.childCount;
         

            switch (childCount)
            {
                case 0:
                    lr.positionCount = 0;
                    break;
                case 1:
                    lr.positionCount = 2;
                    lr.SetPosition(0, transform.GetChild(0).position);
                    lr.SetPosition(1, transform.position);
                    break;
                case 2:
                    lr.positionCount = 3;
                    lr.SetPosition(0, transform.GetChild(0).position);
                    lr.SetPosition(1, transform.position);
                    lr.SetPosition(2, transform.GetChild(1).position);
                    break;
            }
        }
    }
}