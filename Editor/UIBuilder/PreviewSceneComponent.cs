using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CCDKEditor
{
    public class PreviewSceneComponent : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<PreviewSceneComponent, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_String =
                new UxmlStringAttributeDescription { name = "string-attr", defaultValue = "default_value" };
            UxmlIntAttributeDescription m_Int =
                new UxmlIntAttributeDescription { name = "int-attr", defaultValue = 2 };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as PreviewSceneComponent;

                ate.stringAttr = m_String.GetValueFromBag(bag, cc);
                ate.intAttr = m_Int.GetValueFromBag(bag, cc);
            }
        }

        public string stringAttr { get; set; }
        public int intAttr { get; set; }
    }
}