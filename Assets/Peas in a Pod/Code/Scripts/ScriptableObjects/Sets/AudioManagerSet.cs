using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;

namespace TemporaryGameCompany
{
    [CreateAssetMenu(menuName = "Sets/AudioManagerSet")]
    public class AudioManagerSet : RuntimeSet<AudioManager>
    {
        new private List<AudioManager> Items = new List<AudioManager>();

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