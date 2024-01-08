using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class BrokenActor : GraphicEffect
    {
        [SerializeField] ActorModel actorModel;
        BrokenActorGraphicEffectHandler brokenActorGraphicEffectHandler;
        ActorGameObjectHandler actorGameObjectHandler;

        float lifeTime;
        float currentLifeTime;
        float smallTime;

        Vector3 movementVelocity;

        Rigidbody[] pieces;
        BrokenActorSmokeGraphicEffectHandler[] smokeList;

        protected override void OnInit()
        {
            brokenActorGraphicEffectHandler = (BrokenActorGraphicEffectHandler)GraphicEffectHandler;
            actorGameObjectHandler = actorModel.Init(brokenActorGraphicEffectHandler.ActorData);

            movementVelocity = brokenActorGraphicEffectHandler.MovementVelocity;

            pieces = GetComponentsInChildren<Rigidbody>();

            transform.position = GraphicEffectHandler.PositionData.Position;
            transform.rotation = GraphicEffectHandler.PositionData.Rotation;

            currentLifeTime = 0.0f;
            lifeTime = 15.0f;
            smallTime = 5.0f;

            foreach (var piece in pieces)
            {
                // 位置をリセット(キャッシュから取っているので)
                piece.transform.localPosition = Vector3.zero;
                piece.transform.localRotation = Quaternion.identity;

                // Meshの重心を設定
                var mesh = piece.GetComponent<MeshFilter>().mesh;
                var centerOffset = mesh.bounds.center;
                mesh.SetVertices(mesh.vertices.Select(v => v - centerOffset).ToArray());
                mesh.RecalculateBounds();
                piece.transform.localPosition = piece.transform.localPosition + centerOffset;

                // AddForce
                // 小さいものは回転しやすくする
                var sizeScale = (mesh.bounds.size.x + mesh.bounds.size.y + mesh.bounds.size.z) / 500.0f;
                piece.AddForce(movementVelocity * 100.0f, ForceMode.VelocityChange);
                piece.AddForce(Random.insideUnitSphere * 200.0f, ForceMode.Force);
                piece.AddTorque(Random.insideUnitSphere * 2f * (1.0f / sizeScale), ForceMode.Force);
            }

            // つけるスモークの数(適当)
            smokeList = Enumerable.Range(0, (int)(pieces.Length * 0.1f + 3)).Select(_ =>
            {
                var smoke = new BrokenActorSmokeGraphicEffectHandler(new TransformPositionData(
                    brokenActorGraphicEffectHandler.ActorData.AreaId,
                    pieces[Random.Range(0, pieces.Length)].transform));

                MessageBus.Instance.Data.SpawnGraphicEffect.Broadcast(brokenActorGraphicEffectHandler.BrokenActorSmokeGraphicEffectSpecVO, smoke);
                return smoke;
            }).ToArray();
        }

        public override void OnLateUpdate(float deltaTime)
        {
            currentLifeTime += deltaTime;

            foreach (var piece in pieces)
            {
                piece.transform.localScale = Vector3.one * (1.0f - Mathf.Clamp01((currentLifeTime - smallTime) / (lifeTime - smallTime)));
            }

            if (currentLifeTime > lifeTime)
            {
                IsCompleted = true;
                foreach (var smoke in smokeList)
                {
                    smoke.Abandon();
                }
            }
        }
    }
}
