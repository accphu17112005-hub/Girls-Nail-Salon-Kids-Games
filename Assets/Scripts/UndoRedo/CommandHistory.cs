using System;
using System.Collections.Generic;
using KidsNailSalon.Data;

namespace KidsNailSalon.UndoRedo
{
    public interface ICommand
    {
        string Description { get; }
        void Execute();
        void Undo();
    }

    public class CommandHistory
    {
        private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();
        private const int MaxHistory = 50;

        public event Action OnHistoryChanged;

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public void ExecuteCommand(ICommand command)
        {
            if (command == null) return;
            command.Execute();
            _undoStack.Push(command);
            _redoStack.Clear();

            if (_undoStack.Count > MaxHistory)
            {
                // Cap history
                var list = new List<ICommand>(_undoStack);
                list.RemoveAt(list.Count - 1);
                _undoStack.Clear();
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    _undoStack.Push(list[i]);
                }
            }

            OnHistoryChanged?.Invoke();
        }

        public void Undo()
        {
            if (!CanUndo) return;
            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);
            OnHistoryChanged?.Invoke();
        }

        public void Redo()
        {
            if (!CanRedo) return;
            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);
            OnHistoryChanged?.Invoke();
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
            OnHistoryChanged?.Invoke();
        }
    }

    // Concrete command: Paint single nail
    public class PaintNailCommand : ICommand
    {
        private readonly SingleNailData _nailData;
        private readonly Action _onRefreshUI;
        private readonly string _prevColor;
        private readonly bool _prevGradient;
        private readonly string _prevGradientCol;
        private readonly TextureEffectType _prevEffect;
        private readonly PatternType _prevPattern;

        private readonly string _newColor;
        private readonly bool _newGradient;
        private readonly string _newGradientCol;
        private readonly TextureEffectType _newEffect;
        private readonly PatternType _newPattern;

        public string Description => $"Paint {_nailData.nailId}";

        public PaintNailCommand(SingleNailData nailData, string newColor, bool newGradient, string newGradientCol, TextureEffectType newEffect, PatternType newPattern, Action onRefreshUI)
        {
            _nailData = nailData;
            _onRefreshUI = onRefreshUI;

            _prevColor = nailData.primaryColorHex;
            _prevGradient = nailData.hasGradient;
            _prevGradientCol = nailData.secondaryColorHex;
            _prevEffect = nailData.effect;
            _prevPattern = nailData.pattern;

            _newColor = newColor;
            _newGradient = newGradient;
            _newGradientCol = newGradientCol;
            _newEffect = newEffect;
            _newPattern = newPattern;
        }

        public void Execute()
        {
            _nailData.primaryColorHex = _newColor;
            _nailData.hasGradient = _newGradient;
            _nailData.secondaryColorHex = _newGradientCol;
            _nailData.effect = _newEffect;
            _nailData.pattern = _newPattern;
            _onRefreshUI?.Invoke();
        }

        public void Undo()
        {
            _nailData.primaryColorHex = _prevColor;
            _nailData.hasGradient = _prevGradient;
            _nailData.secondaryColorHex = _prevGradientCol;
            _nailData.effect = _prevEffect;
            _nailData.pattern = _prevPattern;
            _onRefreshUI?.Invoke();
        }
    }

    // Concrete command: Change nail shape
    public class ChangeShapeCommand : ICommand
    {
        private readonly SingleNailData _nailData;
        private readonly NailShapeType _prevShape;
        private readonly NailShapeType _newShape;
        private readonly Action _onRefreshUI;

        public string Description => $"Change Shape {_nailData.nailId} to {_newShape}";

        public ChangeShapeCommand(SingleNailData nailData, NailShapeType newShape, Action onRefreshUI)
        {
            _nailData = nailData;
            _prevShape = nailData.shape;
            _newShape = newShape;
            _onRefreshUI = onRefreshUI;
        }

        public void Execute()
        {
            _nailData.shape = _newShape;
            _onRefreshUI?.Invoke();
        }

        public void Undo()
        {
            _nailData.shape = _prevShape;
            _onRefreshUI?.Invoke();
        }
    }

    // Concrete command: Add Sticker
    public class AddStickerCommand : ICommand
    {
        private readonly SingleNailData _nailData;
        private readonly StickerInstance _sticker;
        private readonly Action _onRefreshUI;

        public string Description => $"Add sticker {_sticker.stickerId}";

        public AddStickerCommand(SingleNailData nailData, StickerInstance sticker, Action onRefreshUI)
        {
            _nailData = nailData;
            _sticker = sticker;
            _onRefreshUI = onRefreshUI;
        }

        public void Execute()
        {
            if (!_nailData.stickers.Contains(_sticker))
            {
                _nailData.stickers.Add(_sticker);
            }
            _onRefreshUI?.Invoke();
        }

        public void Undo()
        {
            _nailData.stickers.Remove(_sticker);
            _onRefreshUI?.Invoke();
        }
    }

    // Concrete command: Remove Sticker
    public class RemoveStickerCommand : ICommand
    {
        private readonly SingleNailData _nailData;
        private readonly StickerInstance _sticker;
        private readonly int _index;
        private readonly Action _onRefreshUI;

        public string Description => $"Remove sticker {_sticker.stickerId}";

        public RemoveStickerCommand(SingleNailData nailData, StickerInstance sticker, Action onRefreshUI)
        {
            _nailData = nailData;
            _sticker = sticker;
            _index = nailData.stickers.IndexOf(sticker);
            _onRefreshUI = onRefreshUI;
        }

        public void Execute()
        {
            _nailData.stickers.Remove(_sticker);
            _onRefreshUI?.Invoke();
        }

        public void Undo()
        {
            if (_index >= 0 && _index <= _nailData.stickers.Count)
            {
                _nailData.stickers.Insert(_index, _sticker);
            }
            else
            {
                _nailData.stickers.Add(_sticker);
            }
            _onRefreshUI?.Invoke();
        }
    }
}
