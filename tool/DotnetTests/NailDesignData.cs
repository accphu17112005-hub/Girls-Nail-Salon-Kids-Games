using System;
using System.Collections.Generic;
using UnityEngine;

namespace KidsNailSalon.Data
{
    public enum NailShapeType
    {
        Round,
        Square,
        Oval,
        Almond,
        Coffin,
        Stiletto
    }

    public enum PolishCategory
    {
        Solid,
        Pastel,
        Neon,
        Metallic,
        Glitter,
        Pearl,
        Rainbow
    }

    public enum TextureEffectType
    {
        None,
        Glossy,
        Matte,
        Glitter,
        Metallic,
        Pearl,
        Holographic,
        Sparkle
    }

    public enum GradientDirection
    {
        Vertical,
        Horizontal,
        Diagonal
    }

    public enum PatternType
    {
        None,
        Stripes,
        PolkaDots,
        Hearts,
        Stars,
        Flowers,
        Checkerboard,
        Waves,
        Rainbow
    }

    [Serializable]
    public class PolishPreset
    {
        public string id;
        public string name;
        public string hexColor;
        public PolishCategory category;
        public TextureEffectType defaultEffect;

        public Color GetColor()
        {
            if (ColorUtility.TryParseHtmlString(hexColor, out Color col))
            {
                return col;
            }
            return Color.white;
        }
    }

    [Serializable]
    public class StickerInstance
    {
        public string instanceId;
        public string stickerId;
        public string category;
        public float localX;
        public float localY;
        public float scale = 1.0f;
        public float rotationAngle = 0.0f;
        public int zOrder;
    }

    [Serializable]
    public class GemInstance
    {
        public string instanceId;
        public string gemId;
        public float localX;
        public float localY;
        public float scale = 1.0f;
        public float rotationAngle = 0.0f;
    }

    [Serializable]
    public class SingleNailData
    {
        public string nailId; // thumb, index, middle, ring, pinky
        public NailShapeType shape = NailShapeType.Almond;
        public string primaryColorHex = "#FFB6C1";
        public bool hasGradient = false;
        public string secondaryColorHex = "#FFFFFF";
        public string tertiaryColorHex = "#FFDAB9";
        public GradientDirection gradientDirection = GradientDirection.Vertical;
        public TextureEffectType effect = TextureEffectType.Glossy;
        public PatternType pattern = PatternType.None;
        public string patternColorHex = "#FFFFFF";
        public List<StickerInstance> stickers = new List<StickerInstance>();
        public List<GemInstance> gems = new List<GemInstance>();

        public SingleNailData Clone()
        {
            string json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<SingleNailData>(json);
        }
    }

    [Serializable]
    public class NailDesignData
    {
        public string designId;
        public string designName = "My Kawaii Nails";
        public long timestamp;
        public string skinToneId = "fair";
        public string skinToneHex = "#FDE2D2";
        public string backgroundId = "bg_salon_pink";
        public List<SingleNailData> nails = new List<SingleNailData>();
        public List<string> rings = new List<string>(); // equipped ring IDs
        public string bracelet = ""; // equipped bracelet ID
        public int schemaVersion = 1;

        public SingleNailData GetNail(string nailId)
        {
            if (nails == null) nails = new List<SingleNailData>();
            foreach (var n in nails)
            {
                if (n.nailId == nailId) return n;
            }
            var newNail = new SingleNailData { nailId = nailId };
            nails.Add(newNail);
            return newNail;
        }

        public NailDesignData Clone()
        {
            string json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<NailDesignData>(json);
        }
    }
}
