using UnityEngine;

namespace _Project.Scripts.GameLogic.Rendering
{
    public class BloomPoint : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _material;
        
        private MaterialPropertyBlock _propertyBlock;
        
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void SetBloomEnabled(bool isEnabled)
        {
            _renderer.GetPropertyBlock(_propertyBlock);
            
            _propertyBlock.SetColor(EmissionColor, isEnabled ? _material.GetColor(EmissionColor) : Color.gray);

            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}