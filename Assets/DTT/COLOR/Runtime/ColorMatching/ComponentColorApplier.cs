using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DTT.COLOR.ColorMatching
{
    /// <summary>
    /// Composite class for applying colors to components.
    /// </summary>
    internal class ComponentColorApplier : IColorApplier
    {
        /// <summary>
        /// The component to apply colors to.
        /// </summary>
        private readonly Component _component;

        /// <summary>
        /// All the objects that can have a color applied.
        /// </summary>
        private readonly List<IColorApplier> _colorAppliers = new List<IColorApplier>();

        /// <summary>
        /// Creates a new instance of the Component Color Applier.
        /// </summary>
        /// <param name="component">The component to search for components on.</param>
        public ComponentColorApplier(Component component)
        {
            _component = component;
            GatherComponents();
        }
        
        /// <summary>
        /// Applies the color to the detected components.
        /// </summary>
        /// <param name="color">The color to apply.</param>
        /// <param name="overrideAlpha">Whether to override the alpha value.</param>
        public void Apply(Color color, bool overrideAlpha)
        {
            foreach (IColorApplier colorApplier in _colorAppliers)
                colorApplier.Apply(color, overrideAlpha);
        }

        /// <summary>
        /// Searches for components.
        /// </summary>
        public void Refresh() => GatherComponents();

        /// <summary>
        /// Searches for components.
        /// </summary>
        private void GatherComponents()
        {
            if (_component.TryGetComponent(out Graphic graphic))
                _colorAppliers.Add(new GraphicColorApplier(graphic));
            if (_component.TryGetComponent(out SpriteRenderer spriteRenderer))
                _colorAppliers.Add(new SpriteRendererColorApplier(spriteRenderer));
            if (_component.TryGetComponent(out Camera camera))
                _colorAppliers.Add(new CameraColorApplier(camera));
        }
    }
}