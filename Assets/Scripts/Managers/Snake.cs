using System;
using System.Collections.Generic;
using Extensions.Core;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class Snake : Singleton<Snake>
    {
        public class Segment
        {
            public SpriteRenderer segmentSprite;
            public Transform segmentTransform;
        }

        [SerializeField]
        private Transform _head;

        public Transform Head => _head;

        [SerializeField]
        private float _segmentWidth;

        [Header("Segment Properties"),SerializeField]
        //private List<GameObject> _segmentsObjects = new();
        private GameObject _segmentPrefab;

        [SerializeField]
        private GameObject _segmentParent;

        public List<Segment> Segments { get; } = new();

        [Header("Sprites"), SerializeField]
        private Sprite _straightSprite;

        [SerializeField]
        private Sprite _swivelSprite;

        [SerializeField]
        private Sprite _endSprite;

        [SerializeField]
        private Material _snakeMaterial;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //MoveToTarget(_head.position + new Vector3(0, _segmentWidth));
                AddSegment();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                //MoveToTarget(_head.position + new Vector3(0, _segmentWidth));
                FieldManager.Instance.MoveSnakeDirection(Vector2Int.up);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                //MoveToTarget(_head.position + new Vector3(0, -_segmentWidth));
                FieldManager.Instance.MoveSnakeDirection(Vector2Int.down);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                //MoveToTarget(_head.position + new Vector3(-_segmentWidth, 0));
                FieldManager.Instance.MoveSnakeDirection(Vector2Int.left);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                //MoveToTarget(_head.position + new Vector3(_segmentWidth, 0));
                FieldManager.Instance.MoveSnakeDirection(Vector2Int.right);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                //MoveToTarget(_head.position + new Vector3(_segmentWidth, 0));
                SetBloomAmount(0);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                //MoveToTarget(_head.position + new Vector3(_segmentWidth, 0));
                SetBloomAmount(0.04f);
            }
        }

        public void MoveToTarget(Vector2 target)
        {
            //int  


            if (Segments.Count > 0)
            {
                for (int i = Segments.Count - 1; i >= 0; --i)
                {
                    Transform nextSegment = (i == 0) ? _head : Segments[i - 1].segmentTransform;
                    Segments[i].segmentTransform.position = nextSegment.position;
                }
            }

            var position = _head.position;
            Vector2 tg = target - (Vector2) position;
            _head.position = target;
            UpdateSnakeSprites(target + tg);
        }

        public void UpdateSnakeSprites(Vector2 target)
        {
            _head.LookAt2D(target);
            for (int i = 0; i < Segments.Count; i++)
            {
                Transform nextSegment = (i == 0) ? _head : Segments[i - 1].segmentTransform;
                Transform prevSegment = (i == Segments.Count - 1) ? null : Segments[i + 1].segmentTransform;
                var position = nextSegment.position;
                //Vector2 direction = (position - Segments[i].segmentTransform.position).normalized;
                Segments[i].segmentTransform.LookAt2D(position);
                Segments[i].segmentTransform.gameObject.name = "Segment" + (i + 1);
                if (prevSegment != null)
                {
                    Vector2 offset = nextSegment.position - prevSegment.position;
                    Segments[i].segmentSprite.sprite =
                        (offset.x == 0 || offset.y == 0) ? _straightSprite : _swivelSprite;
                    if (!(offset.x == 0 || offset.y == 0))
                    {
                        Segments[i].segmentSprite.flipX =
                            !((Segments[i].segmentTransform.position +
                                  Segments[i].segmentTransform.right * _segmentWidth - prevSegment.position).magnitude <
                              0.01f);
                    }
                }
                else Segments[i].segmentSprite.sprite = _endSprite;
            }
        }

     

        public void AddSegment()
        {
            GameObject obj = Instantiate(_segmentPrefab);

            Segment segment = new Segment
            {
                segmentSprite = obj.GetComponent<SpriteRenderer>()
            };
            segment.segmentSprite.sprite = _endSprite;
            segment.segmentTransform = obj.transform;
            segment.segmentTransform.SetParent(_segmentParent.transform);
            Transform transformSegment = (Segments.Count > 0) ? Segments[^1].segmentTransform : _head;
            segment.segmentTransform.position = transformSegment.position;
            segment.segmentTransform.up = transformSegment.up;
            Segments.Add(segment);
            FieldManager.Instance.ExpandSnakeList();
        }

        public void SetBloomAmount(float value)
        {
           _snakeMaterial.SetFloat("_Thickness", value);
        }        
    }
}

