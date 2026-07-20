using System;
using System.IO;

namespace PowerKit;

/// <summary>
/// Represents a saved position in a stream that can be jumped to and returned from.
/// </summary>
public class StreamPortal(Stream stream, long position)
{
    /// <summary>
    /// Gets the position this portal points to.
    /// </summary>
    public long Position { get; } =
        stream.CanSeek
            ? position >= 0
                ? position
                : throw new ArgumentOutOfRangeException(
                    nameof(position),
                    "Position must be non-negative."
                )
            : throw new ArgumentException("Stream must support seeking.", nameof(stream));

    /// <summary>
    /// Seeks the stream to the portal's position and returns a disposable that,
    /// when disposed, seeks back to the original position.
    /// </summary>
    public IDisposable Jump()
    {
        var oldPosition = stream.Position;
        stream.Seek(Position, SeekOrigin.Begin);

        return Disposable.Create(() => stream.Seek(oldPosition, SeekOrigin.Begin));
    }
}
