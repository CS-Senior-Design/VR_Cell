using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WhiteboardMarker : MonoBehaviour
{
    [SerializeField] private Transform _tip;
    [SerializeField] private int _penSize = 5;

    private Renderer _renderer;
    private Color[] _colors;
    private float _tipHeight;

    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchLastFrame;
    private Quaternion _lastTouchRot;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = _tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray();
        _tipHeight = _tip.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if(Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight))
        {
            if(_touch.transform.CompareTag("Whiteboard"))
            {
                if(_whiteboard == null)
                {
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _whiteboard.textureSize.x - (_penSize/2));
                var y = (int)(_touchPos.y * _whiteboard.textureSize.y - (_penSize/2));

                if(x < 0 || x > _whiteboard.textureSize.x || y < 0 || y > _whiteboard.textureSize.y)
                {
                    return;
                }

                if(_touchLastFrame)
                {
                    _whiteboard.texture.SetPixels(x,y,_penSize,_penSize,_colors);

                    for(float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x,x,f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y,y,f);
                        _whiteboard.texture.SetPixels(lerpX,lerpY,_penSize,_penSize,_colors);
                    }

                    transform.rotation = _lastTouchRot;

                    _whiteboard.texture.Apply();
                }

                _lastTouchPos = new Vector2(x,y);
                _lastTouchRot = transform.rotation;
                _touchLastFrame = true;
                return;
            }
        }

        _whiteboard = null;
        _touchLastFrame = false;
    }
}
