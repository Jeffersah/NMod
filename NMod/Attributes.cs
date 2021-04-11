using System;
using System.Collections.Generic;
using System.Text;

namespace NMod
{
    [AttributeUsage(AttributeTargets.Class)]
    class NModItemAttribute: Attribute
    {
        public string FriendlyName { get; }
        public string PickupText { get; }
        public string Description { get; }
        public string LoreText { get; }

        public NModItemAttribute(string friendlyName, string pickupText, string description, string loreText = null)
        {
            FriendlyName = friendlyName;
            PickupText = pickupText;
            Description = description;
            LoreText = loreText;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    class NModBuffAttribute : Attribute
    {
    }
}
