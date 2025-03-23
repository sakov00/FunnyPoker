using UnityEngine;

namespace _Project.Scripts.GameLogic.Rendering
{
    public class BloomPoint : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Material _material;
        
        private MaterialPropertyBlock _propertyBlock;
        
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        public void SetBloomEnabled(bool isEnabled)
        {
            _propertyBlock ??= new MaterialPropertyBlock();
            
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(EmissionColor, isEnabled ? _material.GetColor(EmissionColor) : Color.black);

            _renderer.SetPropertyBlock(_propertyBlock);
        }
    }
}