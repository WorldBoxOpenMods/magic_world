using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace magic_world
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MagicSave
    {
        public Guid Id { get; set; }
        [JsonProperty]
        public string a = "null";
        [JsonProperty]
        public string id = "null";
        [JsonProperty]
        public bool destroy = false;
        [JsonProperty]
        public bool animation = false;
        [JsonProperty]
        public float interval = 100f;
        [JsonProperty]
        public float intervals = 100f;
        [JsonProperty]
        public bool paused_ = true;
        [JsonProperty]
        public bool for_ = false;
        [JsonProperty]
        public float time = -1f;
        [JsonProperty]
        public List<string> statsId = new List<string>();
        [JsonProperty]
        public List<float> statsValue = new List<float>();
        [JsonProperty]
        public List<string> textures = new List<string>();
    }

}