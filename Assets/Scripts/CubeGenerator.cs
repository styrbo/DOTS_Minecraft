using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using BoxCollider = Unity.Physics.BoxCollider;
using MeshCollider = Unity.Physics.Collider;
using Material = UnityEngine.Material;

public class CubeGenerator : MonoBehaviour
{
    public Mesh CubeMesh;
    public Material CubeMaterial;

    private void Start()
    {
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var entityAcretype = entityManager.CreateArchetype(
            typeof(LocalToWorld),
            typeof(Rotation),
            typeof(Translation),
            typeof(RenderBounds),
            typeof(WorldRenderBounds),
            typeof(PerInstanceCullingTag),
            typeof(RenderMesh),
            typeof(ChunkWorldRenderBounds),
            typeof(PhysicsCollider)
        );

        var entities = entityManager.CreateEntity(entityAcretype, 1000000, Allocator.Temp);

        for (int x = 0; x < 1000; x++)
        {
            for (int y = 0; y < 1000; y++)
            {
                var entity = entities[x + y * 1000];

                entityManager.SetComponentData(entity, new PhysicsCollider {Value = BoxCollider.Create(new BoxGeometry
                {
                    Size = new float3(1f, 1f, 1f),
                    Orientation = Quaternion.identity
                })});

                entityManager.SetSharedComponentData(entity, new RenderMesh {mesh = CubeMesh, material = CubeMaterial});
                entityManager.SetComponentData(entity,
                    new RenderBounds
                    {
                        Value = new AABB
                        {
                            Center = float3.zero, Extents = new float3(.5f, .5f, .5f)
                        }
                    });

                entityManager.SetComponentData(entity, new Translation() {Value = new float3(x, 0, y) + .5f});
            }
        }

        entities.Dispose();
    }
}