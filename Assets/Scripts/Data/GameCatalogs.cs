using System.Collections.Generic;
using UnityEngine;

namespace KidsNailSalon.Data
{
    public static class GameCatalogs
    {
        public static readonly string[] NailIds = { "thumb", "index", "middle", "ring", "pinky" };

        public static readonly List<PolishPreset> PolishColors = new List<PolishPreset>
        {
            // === PASTELS (10) ===
            new PolishPreset { id = "p_baby_pink", name = "Baby Pink", hexColor = "#FFC0CB", category = PolishCategory.Pastel, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "p_lavender_mist", name = "Lavender Mist", hexColor = "#E6E6FA", category = PolishCategory.Pastel, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "p_peach_fuzz", name = "Peach Fuzz", hexColor = "#FFDAB9", category = PolishCategory.Pastel, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "p_mint_cream", name = "Mint Cream", hexColor = "#B2F5EA", category = PolishCategory.Pastel, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "p_buttercup", name = "Buttercup Yellow", hexColor = "#FFF9A6", category = PolishCategory.Pastel, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "p_cloud_blue", name = "Cloud Blue", hexColor = "#BEE3F8", category = PolishCategory.Pastel, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "p_lilac_dream", name = "Lilac Dream", hexColor = "#D8B4E2", category = PolishCategory.Pastel, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "p_cotton_candy", name = "Cotton Candy", hexColor = "#FFB6C1", category = PolishCategory.Pastel, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "p_honeydew", name = "Honeydew", hexColor = "#E2F0D9", category = PolishCategory.Pastel, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "p_vanilla_latte", name = "Vanilla Cream", hexColor = "#FDF5E6", category = PolishCategory.Pastel, defaultEffect = TextureEffectType.Glossy },

            // === SOLIDS (10) ===
            new PolishPreset { id = "s_ruby_red", name = "Ruby Red", hexColor = "#E53E3E", category = PolishCategory.Solid, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "s_cherry_pop", name = "Cherry Pop", hexColor = "#D53F8C", category = PolishCategory.Solid, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "s_bubblegum", name = "Bubblegum Pink", hexColor = "#ED64A6", category = PolishCategory.Solid, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "s_tangerine", name = "Sweet Tangerine", hexColor = "#ED8936", category = PolishCategory.Solid, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "s_sunflower", name = "Sunflower", hexColor = "#ECC94B", category = PolishCategory.Solid, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "s_emerald", name = "Emerald Green", hexColor = "#38A169", category = PolishCategory.Solid, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "s_royal_blue", name = "Royal Ocean", hexColor = "#3182CE", category = PolishCategory.Solid, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "s_deep_violet", name = "Deep Violet", hexColor = "#805AD5", category = PolishCategory.Solid, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "s_pure_white", name = "Pure White", hexColor = "#FFFFFF", category = PolishCategory.Solid, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "s_midnight_black", name = "Midnight Onyx", hexColor = "#1A202C", category = PolishCategory.Solid, defaultEffect = TextureEffectType.Glossy },

            // === NEONS (8) ===
            new PolishPreset { id = "n_electric_pink", name = "Electric Pink", hexColor = "#FF1493", category = PolishCategory.Neon, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "n_acid_lime", name = "Acid Lime", hexColor = "#39FF14", category = PolishCategory.Neon, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "n_laser_cyan", name = "Laser Cyan", hexColor = "#00FFFF", category = PolishCategory.Neon, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "n_blaze_orange", name = "Blaze Orange", hexColor = "#FF6600", category = PolishCategory.Neon, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "n_neon_yellow", name = "Neon Sunshine", hexColor = "#FFFF33", category = PolishCategory.Neon, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "n_ultra_purple", name = "Ultraviolet", hexColor = "#BF00FF", category = PolishCategory.Neon, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "n_radioactive_green", name = "Radioactive Green", hexColor = "#00FF66", category = PolishCategory.Neon, defaultEffect = TextureEffectType.Glossy },
            new PolishPreset { id = "n_hot_coral", name = "Hot Coral", hexColor = "#FF4040", category = PolishCategory.Neon, defaultEffect = TextureEffectType.Glossy },

            // === METALLICS (8) ===
            new PolishPreset { id = "m_rose_gold", name = "Rose Gold", hexColor = "#B76E79", category = PolishCategory.Metallic, defaultEffect = TextureEffectType.Metallic },
            new PolishPreset { id = "m_champagne", name = "Champagne Gold", hexColor = "#D4AF37", category = PolishCategory.Metallic, defaultEffect = TextureEffectType.Metallic },
            new PolishPreset { id = "m_sterling_silver", name = "Sterling Silver", hexColor = "#C0C0C0", category = PolishCategory.Metallic, defaultEffect = TextureEffectType.Metallic },
            new PolishPreset { id = "m_bronze_glow", name = "Bronze Glow", hexColor = "#CD7F32", category = PolishCategory.Metallic, defaultEffect = TextureEffectType.Metallic },
            new PolishPreset { id = "m_metallic_copper", name = "Bright Copper", hexColor = "#B87333", category = PolishCategory.Metallic, defaultEffect = TextureEffectType.Metallic },
            new PolishPreset { id = "m_platinum", name = "Pure Platinum", hexColor = "#E5E4E2", category = PolishCategory.Metallic, defaultEffect = TextureEffectType.Metallic },
            new PolishPreset { id = "m_metallic_teal", name = "Metallic Teal", hexColor = "#008080", category = PolishCategory.Metallic, defaultEffect = TextureEffectType.Metallic },
            new PolishPreset { id = "m_gunmetal", name = "Gunmetal Shine", hexColor = "#536267", category = PolishCategory.Metallic, defaultEffect = TextureEffectType.Metallic },

            // === GLITTER (8) ===
            new PolishPreset { id = "g_pink_sparkle", name = "Pink Sparkle", hexColor = "#FF69B4", category = PolishCategory.Glitter, defaultEffect = TextureEffectType.Glitter },
            new PolishPreset { id = "g_holo_silver", name = "Holo Starlight", hexColor = "#E0E8F0", category = PolishCategory.Glitter, defaultEffect = TextureEffectType.Glitter },
            new PolishPreset { id = "g_golden_dust", name = "Golden Pixie Dust", hexColor = "#FFD700", category = PolishCategory.Glitter, defaultEffect = TextureEffectType.Glitter },
            new PolishPreset { id = "g_mermaid_aqua", name = "Mermaid Lagoon", hexColor = "#40E0D0", category = PolishCategory.Glitter, defaultEffect = TextureEffectType.Glitter },
            new PolishPreset { id = "g_violet_galaxy", name = "Violet Nebula", hexColor = "#9370DB", category = PolishCategory.Glitter, defaultEffect = TextureEffectType.Glitter },
            new PolishPreset { id = "g_ruby_glitz", name = "Ruby Glitz", hexColor = "#DC143C", category = PolishCategory.Glitter, defaultEffect = TextureEffectType.Glitter },
            new PolishPreset { id = "g_sapphire_frost", name = "Sapphire Frost", hexColor = "#1E90FF", category = PolishCategory.Glitter, defaultEffect = TextureEffectType.Glitter },
            new PolishPreset { id = "g_emerald_glimmer", name = "Emerald Glimmer", hexColor = "#50C878", category = PolishCategory.Glitter, defaultEffect = TextureEffectType.Glitter },

            // === PEARL (6) ===
            new PolishPreset { id = "pr_mother_pearl", name = "Mother of Pearl", hexColor = "#F0EAE1", category = PolishCategory.Pearl, defaultEffect = TextureEffectType.Pearl },
            new PolishPreset { id = "pr_pink_pearl", name = "Blushing Pearl", hexColor = "#FCE4EC", category = PolishCategory.Pearl, defaultEffect = TextureEffectType.Pearl },
            new PolishPreset { id = "pr_golden_pearl", name = "Sunlight Pearl", hexColor = "#FFF8DC", category = PolishCategory.Pearl, defaultEffect = TextureEffectType.Pearl },
            new PolishPreset { id = "pr_opal_sheen", name = "Opal Iridescent", hexColor = "#E6F3F5", category = PolishCategory.Pearl, defaultEffect = TextureEffectType.Pearl },
            new PolishPreset { id = "pr_lavender_pearl", name = "Moonlit Lavender", hexColor = "#EDE7F6", category = PolishCategory.Pearl, defaultEffect = TextureEffectType.Pearl },
            new PolishPreset { id = "pr_peach_pearl", name = "Nectar Pearl", hexColor = "#FBE9E7", category = PolishCategory.Pearl, defaultEffect = TextureEffectType.Pearl },

            // === RAINBOW / SPECIAL (5) ===
            new PolishPreset { id = "rb_unicorn_pastel", name = "Unicorn Dream", hexColor = "#F8BBD0", category = PolishCategory.Rainbow, defaultEffect = TextureEffectType.Holographic },
            new PolishPreset { id = "rb_sunset_blush", name = "Sunset Glow", hexColor = "#FF8A65", category = PolishCategory.Rainbow, defaultEffect = TextureEffectType.Holographic },
            new PolishPreset { id = "rb_aurora_borealis", name = "Aurora Magic", hexColor = "#80CBC4", category = PolishCategory.Rainbow, defaultEffect = TextureEffectType.Holographic },
            new PolishPreset { id = "rb_sweet_berry", name = "Berry Ombre", hexColor = "#BA68C8", category = PolishCategory.Rainbow, defaultEffect = TextureEffectType.Holographic },
            new PolishPreset { id = "rb_cosmic_twilight", name = "Cosmic Twilight", hexColor = "#7986CB", category = PolishCategory.Rainbow, defaultEffect = TextureEffectType.Holographic }
        };

        public static readonly string[] SkinTones = { "fair", "light", "tan", "caramel", "deep" };

        public static readonly string[] Backgrounds = {
            "bg_salon_pink",
            "bg_rainbow_cloud",
            "bg_lavender_glitter",
            "bg_mint_studio",
            "bg_peach_sparkle"
        };

        public static readonly string[] Rings = {
            "ring_gold_solitaire",
            "ring_princess_crown",
            "ring_pearl_band",
            "ring_kitty_ears",
            "ring_heart_gem",
            "ring_butterfly"
        };

        public static readonly string[] Bracelets = {
            "bracelet_pearl_bangle",
            "bracelet_gold_charms",
            "bracelet_rainbow_beads",
            "bracelet_ribbon_corsage",
            "bracelet_silver_chain"
        };
    }
}
