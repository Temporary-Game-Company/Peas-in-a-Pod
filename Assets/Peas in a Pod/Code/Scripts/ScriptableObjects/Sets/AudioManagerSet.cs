using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;

namespace TemporaryGameCompany
{
    [CreateAssetMenu(menuName = "Sets/AudioManagerSet")]
    public class AudioManagerSet : RuntimeSet<AudioManager>
    {
        private new List<AudioManager> Items
        {
            get
            {
                return base.Items;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public AudioManager Get()
        {
            if (Items.Count > 0)
            {
                return Items[0];
            }

            return null;
        }
    }
}