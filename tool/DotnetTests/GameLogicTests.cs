using System;
using System.Collections.Generic;
using Xunit;
using KidsNailSalon.Data;
using KidsNailSalon.UndoRedo;

namespace KidsNailSalon.Tests
{
    public class GameLogicTests
    {
        [Fact]
        public void TestCatalogsCount()
        {
            Assert.True(GameCatalogs.PolishColors.Count >= 50, "Should have at least 50 polish colors");
            Assert.Equal(5, GameCatalogs.NailIds.Length);
            Assert.Equal(5, GameCatalogs.SkinTones.Length);
            Assert.Equal(5, GameCatalogs.Backgrounds.Length);
        }

        [Fact]
        public void TestNailDataCreationAndClone()
        {
            var design = new NailDesignData
            {
                designId = "test_001",
                skinToneId = "fair"
            };

            var nail = design.GetNail("middle");
            nail.primaryColorHex = "#FF1493";
            nail.shape = NailShapeType.Coffin;
            nail.effect = TextureEffectType.Glitter;

            Assert.Equal("middle", nail.nailId);
            Assert.Equal("#FF1493", nail.primaryColorHex);
            Assert.Equal(NailShapeType.Coffin, nail.shape);

            var clone = design.Clone();
            Assert.Equal(design.designId, clone.designId);
            Assert.Equal(design.GetNail("middle").primaryColorHex, clone.GetNail("middle").primaryColorHex);
        }

        [Fact]
        public void TestUndoRedoPainting()
        {
            var history = new CommandHistory();
            var nail = new SingleNailData
            {
                nailId = "index",
                primaryColorHex = "#FFB6C1",
                shape = NailShapeType.Almond
            };

            int refreshCount = 0;
            Action onRefresh = () => refreshCount++;

            // 1. Paint with new color
            var cmd = new PaintNailCommand(nail, "#39FF14", false, "#FFFFFF", TextureEffectType.Glossy, PatternType.None, onRefresh);
            history.ExecuteCommand(cmd);

            Assert.Equal("#39FF14", nail.primaryColorHex);
            Assert.True(history.CanUndo);
            Assert.False(history.CanRedo);
            Assert.Equal(1, refreshCount);

            // 2. Undo
            history.Undo();
            Assert.Equal("#FFB6C1", nail.primaryColorHex);
            Assert.False(history.CanUndo);
            Assert.True(history.CanRedo);
            Assert.Equal(2, refreshCount);

            // 3. Redo
            history.Redo();
            Assert.Equal("#39FF14", nail.primaryColorHex);
            Assert.True(history.CanUndo);
            Assert.False(history.CanRedo);
            Assert.Equal(3, refreshCount);
        }

        [Fact]
        public void TestUndoRedoStickers()
        {
            var history = new CommandHistory();
            var nail = new SingleNailData { nailId = "ring" };
            var sticker = new StickerInstance
            {
                instanceId = "st_001",
                stickerId = "flower_sakura",
                category = "Flowers",
                scale = 0.5f
            };

            int refreshCount = 0;
            Action onRefresh = () => refreshCount++;

            var addCmd = new AddStickerCommand(nail, sticker, onRefresh);
            history.ExecuteCommand(addCmd);

            Assert.Single(nail.stickers);
            Assert.Equal("flower_sakura", nail.stickers[0].stickerId);

            history.Undo();
            Assert.Empty(nail.stickers);

            history.Redo();
            Assert.Single(nail.stickers);
        }
    }
}
