using System;
using UnityEngine;
using System.Collections.Generic;
using UnitySpriteCutter.Cutters;
using UnitySpriteCutter.Tools;

namespace UnitySpriteCutter 
{
    public class SpriteCutterInput 
    {
        public enum GameObjectCreationMode
        {
            CUT_OFF_ONE = 0,
            CUT_INTO_TWO = 1,
        }

        private Vector2 _lineStart;
        private Vector2 _lineEnd;

        private Cuttable _cuttable;
        private GameObject _gameObject;
        private bool _dontCutColliders;

        private GameObjectCreationMode _creationMode;

        public Vector2 LineStart => _lineStart;
        public Vector2 LineEnd => _lineEnd;

        public Cuttable Cuttable => _cuttable;
        public GameObject GameObject => _gameObject;
        public bool DontCutColliders => _dontCutColliders;

        public GameObjectCreationMode CreationMode => _creationMode;

        public SpriteCutterInput(Vector2 lineStart, Vector2 lineEnd, Cuttable cuttable, bool dontCutColliders = false, GameObjectCreationMode creationMode = GameObjectCreationMode.CUT_OFF_ONE)
        {
            _lineStart = lineStart;
            _lineEnd = lineEnd;
            _cuttable = cuttable;
            _gameObject = cuttable.gameObject;
            _dontCutColliders = dontCutColliders;
            _creationMode = creationMode;
        }
    }

	public class SpriteCutterOutput {
		public Cuttable firstSide;
		public Cuttable secondSide;
	}

	public static class SpriteCutter 
    {
        public static bool Cut( SpriteCutterInput input, Action<SpriteCutterOutput> onCutSuccess) {

			if ( input.Cuttable == null ) {
				Debug.LogWarning( "SpriteCutter.Cut exceuted with null cuttable!" );
				return false;
			}

            UnityEngine.Object.DestroyImmediate(input.Cuttable);
			
			Vector3 localLineStart = input.GameObject.transform.InverseTransformPoint( input.LineStart );
			Vector3 localLineEnd = input.GameObject.transform.InverseTransformPoint( input.LineEnd );

			SpriteRenderer spriteRenderer = input.GameObject.GetComponent<SpriteRenderer>();
			MeshRenderer meshRenderer = input.GameObject.GetComponent<MeshRenderer>();
			
            FlatConvexCollidersCutter.CutResult collidersCutResults;
            if (input.DontCutColliders)
            {
                collidersCutResults = new FlatConvexCollidersCutter.CutResult();
            }
            else
            {
                collidersCutResults =
                    CutColliders(localLineStart, localLineEnd, input.GameObject.GetComponents<Collider2D>());

                if (collidersCutResults.DidNotCut())
                {
                    return false;
                }
            }

			FlatConvexPolygonMeshCutter.CutResult meshCutResult =
				CutSpriteOrMeshRenderer( localLineStart, localLineEnd, spriteRenderer, collidersCutResults, meshRenderer);
			if ( meshCutResult.DidNotCut() ) {
				return false;
			}


			SpriteCutterGameObject secondSideResult = SpriteCutterGameObject.CreateAsCopyOf( input.GameObject, true );
			PrepareResultGameObject( secondSideResult, spriteRenderer, meshRenderer,
			                         meshCutResult.secondSideMesh, collidersCutResults.secondSideColliderRepresentations );

			SpriteCutterGameObject firstSideResult = null;
			if ( input.CreationMode == SpriteCutterInput.GameObjectCreationMode.CUT_INTO_TWO ) 
            {
				firstSideResult = SpriteCutterGameObject.CreateAsCopyOf( input.GameObject, true );
				PrepareResultGameObject( firstSideResult, spriteRenderer, meshRenderer, meshCutResult.firstSideMesh, collidersCutResults.firstSideColliderRepresentations );

                UnityEngine.Object.Destroy(input.GameObject);

                onCutSuccess?.Invoke(new SpriteCutterOutput()
                {
                    firstSide = firstSideResult.gameObject.AddComponent<Cuttable>(),
                    secondSide = secondSideResult.gameObject.AddComponent<Cuttable>()
                });
            } 
            else if ( input.CreationMode == SpriteCutterInput.GameObjectCreationMode.CUT_OFF_ONE ) 
            {
				firstSideResult = SpriteCutterGameObject.CreateAs( input.GameObject);
				if ( spriteRenderer != null ) 
                {
					RendererParametersRepresentation tempParameters = new RendererParametersRepresentation();
					tempParameters.CopyFrom( spriteRenderer );

					SafeSpriteRendererRemoverBehaviour.get.RemoveAndWaitOneFrame( spriteRenderer, onFinish: () => 
                    {
						PrepareResultGameObject( firstSideResult, tempParameters, meshCutResult.firstSideMesh, collidersCutResults.firstSideColliderRepresentations);
						
                        onCutSuccess?.Invoke(new SpriteCutterOutput()
                        {
                            firstSide = firstSideResult.gameObject.AddComponent<Cuttable>(),
                            secondSide = secondSideResult.gameObject.AddComponent<Cuttable>()
                        });
					} );

				} else 
                {
					PrepareResultGameObject(firstSideResult, spriteRenderer, meshRenderer, meshCutResult.firstSideMesh, collidersCutResults.firstSideColliderRepresentations);

					onCutSuccess?.Invoke(new SpriteCutterOutput()
					{
						firstSide = firstSideResult.gameObject.AddComponent<Cuttable>(),
                        secondSide = secondSideResult.gameObject.AddComponent<Cuttable>()
					});
				}
			}

            return true;
        }

		static FlatConvexPolygonMeshCutter.CutResult CutSpriteOrMeshRenderer( Vector2 lineStart, Vector2 lineEnd, SpriteRenderer spriteRenderer, FlatConvexCollidersCutter.CutResult cutResult, MeshRenderer meshRenderer ) {
			Mesh originMesh = GetOriginMeshFrom( spriteRenderer, meshRenderer );
			return FlatConvexPolygonMeshCutter.Cut( lineStart, lineEnd, cutResult, originMesh);
		}

		static Mesh GetOriginMeshFrom( SpriteRenderer spriteRenderer, MeshRenderer meshRenderer ) {
			if ( spriteRenderer != null ) {
				return SpriteMeshConstructor.ConstructFromRendererBounds( spriteRenderer );
			} else {
				return meshRenderer.GetComponent<MeshFilter>().mesh;
			}
		}

		static FlatConvexCollidersCutter.CutResult CutColliders( Vector2 lineStart, Vector2 lineEnd, Collider2D[] colliders ) {
			return FlatConvexCollidersCutter.Cut( lineStart, lineEnd, colliders );
		}
		
		static void PrepareResultGameObject( SpriteCutterGameObject resultGameObject, SpriteRenderer spriteRenderer,
		                                            MeshRenderer meshRenderer, Mesh mesh, List<PolygonColliderParametersRepresentation> colliderRepresentations ) {
			resultGameObject.AssignMeshFilter( mesh );
			if ( spriteRenderer != null ) {
				resultGameObject.AssignMeshRendererFrom( spriteRenderer );
			} else {
				resultGameObject.AssignMeshRendererFrom( meshRenderer );
			}

			if ( colliderRepresentations != null ) {
				resultGameObject.BuildCollidersFrom( colliderRepresentations );
			}
		}
		
		static void PrepareResultGameObject( SpriteCutterGameObject resultGameObject, RendererParametersRepresentation tempParameters,
		                                            Mesh mesh, List<PolygonColliderParametersRepresentation> colliderRepresentations ) {
			resultGameObject.AssignMeshFilter( mesh );
			resultGameObject.AssignMeshRendererFrom( tempParameters );
			
			if ( colliderRepresentations != null ) {
				resultGameObject.BuildCollidersFrom( colliderRepresentations );
			}
		}

	}

}