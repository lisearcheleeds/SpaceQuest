using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.Common
{
    // バッチング効かないので使い所に注意
    [RequireComponent(typeof(CanvasRenderer))]
    [ExecuteInEditMode]  
    public class Circle2D : Graphic
    {
        [SerializeField] float radius;
        [SerializeField, Range(2,100)] int division;
        [SerializeField, Range(0f,1f)] float fillOrigin;
        [SerializeField, Range(0f,1f)] float fillAmount;
        
        [SerializeField] bool isPierced;
        [SerializeField] float innerRadius;

        public float Radius => radius;
        public int Division => division;
        public float FillOrigin => fillOrigin;
        public float FillAmount => fillAmount;
        public bool IsPierced => isPierced;
        public float InnerRadius => innerRadius;
        
        public void Apply(float radius, int division, float fillOrigin, float fillAmount, bool isPierced, float innerRadius)
        {
            this.radius = radius;
            this.division = division;
            this.fillOrigin = fillOrigin;
            this.fillAmount = fillAmount;
            this.isPierced = isPierced;
            this.innerRadius = innerRadius;
            
            SetVerticesDirty();
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (!isActiveAndEnabled)
            {
                return;
            }
            
            vh.Clear();
            RectCache();

            if (isPierced)
            {
                DrawPiercedCircle(vh, 0);
            }
            else
            {
                DrawCircle(vh, 0);
            }
        }

        /// <summary>
        /// ベクトルを回転させて円を作成する
        /// </summary>
        /// <param name="vh">VertexHelper</param>
        /// <param name="firstVertIndex">最初の頂点番号</param>
        /// <returns>最終番号の次</returns>
        int DrawCircle(VertexHelper vh, int firstVertIndex)
        {
            var vertex = UIVertex.simpleVert;
            vertex.color = color;
            
            var centerPos = new Vector3((pCLeft + pCRight) / 2, (pCTop + pCBottom) / 2);
            var fillRad = Mathf.PI * 2f * fillAmount;
            var divRad = fillRad / (division + 1);
            
            vertex.position = centerPos;
            vh.AddVert(vertex);
            
            var startPos = centerPos + new Vector3(radius * Mathf.Cos(fillOrigin * 2f * Mathf.PI), radius * Mathf.Sin(fillOrigin * 2f * Mathf.PI));
            var startVector = startPos - centerPos;
            
            for (var i = 0; i < division + 2; i++)
            {
                vertex.position = new Vector3(
                    startVector.x * Mathf.Cos(divRad * i) - startVector.y * Mathf.Sin(divRad * i) + centerPos.x,
                    startVector.x * Mathf.Sin(divRad * i) + startVector.y * Mathf.Cos(divRad * i) + centerPos.y
                );
                vh.AddVert(vertex);   
            }
            
            for (var i = 0; i < division + 2; i++)
            {
                vh.AddTriangle(firstVertIndex, i + 1, i);                
            }
            
            return division + 1;
        }

        /// <summary>
        /// ベクトルを回転させて穴あきの円を作成する
        /// </summary>
        /// <param name="vh">VertexHelper</param>
        /// <param name="firstVertIndex">最初の頂点番号</param>
        /// <returns>最終番号の次</returns>
        int DrawPiercedCircle(VertexHelper vh, int firstVertIndex)
        {
            var vertex = UIVertex.simpleVert;
            vertex.color = color;
            
            var centerPos = new Vector3((pCLeft + pCRight) / 2, (pCTop + pCBottom) / 2);
            var fillRad = Mathf.PI * 2f * fillAmount;
            var divRad = fillRad / (division + 1);
            
            // outside
            var startOutsidePos = centerPos + new Vector3(radius * Mathf.Cos(fillOrigin * 2f * Mathf.PI), radius * Mathf.Sin(fillOrigin * 2f * Mathf.PI));
            var startOutsideVector = startOutsidePos - centerPos;
            for (var i = 0; i < division + 2; i++)
            {
                vertex.position = new Vector3(
                    startOutsideVector.x * Mathf.Cos(divRad * i) - startOutsideVector.y * Mathf.Sin(divRad * i) + centerPos.x,
                    startOutsideVector.x * Mathf.Sin(divRad * i) + startOutsideVector.y * Mathf.Cos(divRad * i) + centerPos.y
                );
                vh.AddVert(vertex);   
            }
            
            // inside
            var startInsidePos = centerPos + new Vector3(innerRadius * Mathf.Cos(fillOrigin * 2f * Mathf.PI), innerRadius * Mathf.Sin(fillOrigin * 2f * Mathf.PI));
            var startInsideVector = startInsidePos - centerPos;
            for (var i = 0; i < division + 2; i++)
            {
                vertex.position = new Vector3(
                    startInsideVector.x * Mathf.Cos(divRad * i) - startInsideVector.y * Mathf.Sin(divRad * i) + centerPos.x,
                    startInsideVector.x * Mathf.Sin(divRad * i) + startInsideVector.y * Mathf.Cos(divRad * i) + centerPos.y
                );
                vh.AddVert(vertex);   
            }
            
            for (var i = 0; i < division + 1; i++)
            {
                var offset = firstVertIndex;
                vh.AddTriangle(offset + i, offset + i + division + 2, offset + i + 1);
                vh.AddTriangle(offset + i + division + 2, offset + i + division + 3, offset + i + 1);
            }
            
            return (division + 1) * 2;
        }
        
        // cache
        float anchorLeft, anchorBottom, anchorTop, anchorRight;
        float pivotX, pivotY;
        float posX, posY, posZ;
        float width, height;
        float paddingTop, paddingBottom, paddingLeft, paddingRight;
        
        // extensions
        float pCLeft, pCBottom, pCTop, pCRight;
        float nRectLeft, nRectBottom, nRectTop, nRectRight;
        
        void RectCache()
        {
            //Anchors
            anchorLeft   = rectTransform.anchorMin.x;
            anchorBottom = rectTransform.anchorMin.y;
            anchorTop    = rectTransform.anchorMax.y;
            anchorRight  = rectTransform.anchorMax.x;
            
            //pivots
            pivotX = rectTransform.pivot.x;
            pivotY = rectTransform.pivot.y;
            
            //width & height
            width = rectTransform.rect.width;
            height = rectTransform.rect.height;
            
            //一致したアンカーとピボットの位置の差
            posX = rectTransform.position.x;
            posY = rectTransform.position.y;
            posZ = rectTransform.position.z;
            
            //アンカーから内部方向へのパディング
            paddingLeft   = rectTransform.offsetMin.x;
            paddingRight  = rectTransform.offsetMax.x;
            paddingTop    = rectTransform.offsetMax.y;
            paddingBottom = rectTransform.offsetMin.y;
            
            //ピボット中央に合わせる場合のUIVertex矩形座標
            pCLeft   = - width  / 2f;
            pCBottom = - height / 2f;
            pCTop    =   height / 2f;
            pCRight  =   width  / 2f;
            
            //ピボットに影響されないUIVertex矩形座標
            nRectLeft   = - pivotX * width;
            nRectRight  = (1 - pivotX) * width;
            nRectTop    = (1 - pivotY) * height;
            nRectBottom = - pivotY * height;
        }
    }
}