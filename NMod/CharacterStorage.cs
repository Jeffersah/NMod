using RoR2;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMod
{
    public class CharacterStorage<T>
    {
        private Dictionary<CharacterBody, T> Storage { get; } = new Dictionary<CharacterBody, T>();

        public CharacterStorage()
        {
            On.RoR2.CharacterBody.OnDestroy += (orig, self) =>
            {
                Remove(self);
                orig(self);
            };
        }

        public T GetOrCreate(CharacterBody body, T initial)
        {
            if (!Storage.ContainsKey(body))
            {
                Storage[body] = initial;
            }
            return Storage[body];
        }
        public T GetOrCreate(CharacterBody body, Func<T> initial)
        {
            if (!Storage.ContainsKey(body))
            {
                Storage[body] = initial();
            }
            return Storage[body];
        }

        public bool TryGetValue(CharacterBody body, out T value)
        {
            return Storage.TryGetValue(body, out value);
        }

        public void Add(CharacterBody body, T initial)
        {
            Storage[body] = initial;
        }

        public bool Remove(CharacterBody body)
        {
            return Storage.Remove(body);
        }
    }
}
