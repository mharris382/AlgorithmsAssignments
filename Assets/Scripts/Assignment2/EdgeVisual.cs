using UnityEngine;

namespace Assignment2
{
    [RequireComponent(typeof(LineRenderer))]
    public class EdgeVisual : MonoBehaviour
    {
        [SerializeField]  private float arrowLength = 0.25f;
        [SerializeField]  private float arrowWidth = 0.25f;
        [SerializeField] private float minLength = 0.5f;
        [SerializeField] private float vertexRadius = 1;
        [SerializeField] private float arrowOffset = 0.1f;
        private LineRenderer _lr;

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
            set
            {
                _lr = value;
            }
        }
        private bool _drawArrows;
        private bool _showCost;
        private float _cost;
        private NodeVisual _from;
        private NodeVisual _to;

        
        private void Awake()
        {
            lr = GetComponent<LineRenderer>();
        }

        public void SetCost(float cost, bool update = true)
        {
            if(update)
                UpdateVisual();
        }

        public void SetDrawMode(GraphType graphType, bool update = true)
        {
            _showCost = ((int) graphType & (int) GraphType.Weighted) != 0;
            _drawArrows = ((int) graphType & (int) GraphType.Directed) != 0;
            if(update)
                UpdateVisual();
        }
        
        public void SetTargets(NodeVisual from, NodeVisual to)
        {
            this._from = from;
            this._to = to;
            UpdateVisual();
        }

        void UpdateVisual()
        {
            if (_from == null || _to == null)
            {
                lr.enabled = false;
                return;
            }

            string t = _drawArrows ? "->" : "<->";
            name = $"Edge: {_from.name} {t} {_to.name}";

            lr.enabled = true;
            lr.useWorldSpace = true;
            var from = _from.transform.position;
            var to = _to.transform.position;
            to.z = 0;
            from.z = 0;
            var dir = to - from;
            var offset = dir.normalized * vertexRadius;
            var len = dir.magnitude;
            
            var distance = Vector2.Distance(to, from);
            if (distance - (2 * vertexRadius) < minLength)
            {
                var space = distance - minLength;
                if (space > 0)
                {
                    offset = dir.normalized * (space / 2f);
                    from += offset;
                    to -= offset;    
                }
            }
            else
            {
                from += offset;
                to -= offset;    
            }
            var points = _drawArrows ? GetArrowPoints(from, to) : GetLinePoints(from, to);
            lr.positionCount = points.Length;
            for (int i = 0; i < points.Length; i++)
            {
                lr.SetPosition(i, points[i]);
            }
        }

        Vector3[] GetLinePoints(Vector3 from, Vector3 to)
        {
            return new Vector3[2]
            {
                from,
                to
            };
        }

        Vector3[] GetArrowPoints(Vector3 from, Vector3 to)
        {
            var pnts = new Vector3[7];
            pnts[0] = from;
            pnts[1] = pnts[5] = to;
            Vector2 direction = (to - from);
            var dN = direction.normalized;
            var perp = Vector2.Perpendicular(dN);
            var distance = direction.magnitude;
            var arrowStart = (Vector2)to - (dN * (arrowLength));
            pnts[3] = pnts[6] = arrowStart;
            var offset = (perp.normalized * arrowWidth / 2f);
            
            var p1 = arrowStart + offset;
            var p2 = arrowStart - offset;
            pnts[2] = p1 - (arrowOffset * dN);
            pnts[4] = p2 - (arrowOffset * dN);
            
            pnts[3] = Vector3.Lerp(pnts[3], pnts[4], 0.1f);
            
            return pnts;
        }

        public void ResetVisual()
        {
            _from = null;
            _to = null;
            name = "Unused Edge";
            UpdateVisual();
        }
    }
}