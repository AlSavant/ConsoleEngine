using ConsoleEngine.Editor.Services.Encoding;
using ConsoleEngine.Editor.Services.SpriteGrid;
using DataModel.Rendering;
using LiteNetLib.Utils;
using System;

namespace ConsoleEngine.Editor.Services.Serialization.Implementations
{
    internal sealed class SpriteSerializationService : ISpriteSerializationService
    {
        private readonly ISpriteGridStateService spriteGridStateService;
        private readonly ICanvasDrawingService canvasDrawingService;
        private readonly ICharToOEM437ConverterService charToOEM437ConverterService;

        public SpriteSerializationService(
            ISpriteGridStateService spriteGridStateService, 
            ICanvasDrawingService canvasDrawingService, 
            ICharToOEM437ConverterService charToOEM437ConverterService)
        {
            this.spriteGridStateService = spriteGridStateService;
            this.canvasDrawingService = canvasDrawingService;
            this.charToOEM437ConverterService = charToOEM437ConverterService;
        }

        public byte[] SerializeSprite()
        {
            var gridSize = spriteGridStateService.GetGridSize();
            var colors = new byte[gridSize.x * gridSize.y];
            var characters = new byte[colors.Length];
            for (int i = 0; i < canvasDrawingService.PixelCount; i++)
            {
                var pixel = canvasDrawingService.Get(i);
                if (pixel.colorEntry.ConsoleColor == ConsoleColor.Black || pixel.character == ' ')
                {
                    colors[i] = (byte)ConsoleColor.Black;
                    characters[i] = (byte)' ';
                    continue;
                }
                colors[i] = (byte)pixel.colorEntry.ConsoleColor;
                characters[i] = charToOEM437ConverterService.CharToByte(pixel.character);
            }
            var sprite = new Sprite(gridSize.x, gridSize.y, characters, colors, spriteGridStateService.SupportsTransparency());
            var writer = new NetDataWriter();
            sprite.Serialize(writer);
            return writer.Data;
        }
    }
}
