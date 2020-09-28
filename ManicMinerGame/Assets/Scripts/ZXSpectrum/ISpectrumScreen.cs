using System;

namespace Com.SloanKelly.ZXSpectrum
{
	/// <summary>
	/// Interface for a ZX Spectrum screen implementation. Used for the fluent interface. See https://en.wikipedia.org/wiki/Fluent_interface
	/// for details.
	/// </summary>
	public interface ISpectrumScreen
	{
		ISpectrumScreen FillAttribute (int x, int y, int width, int height, int ink, int paper, bool bright = false, bool flashing = false);

		ISpectrumScreen OverwriteDraw ();

		ISpectrumScreen OrDraw();

		ISpectrumScreen ColumnOrderSprite();

		ISpectrumScreen RowOrderSprite();

		ISpectrumScreen DrawSprite(int x, int y, int cols, int rows, params byte[] data);

		ISpectrumScreen SetAttribute (int x, int y, int ink, int paper, bool bright = false, bool flashing = false);
	}
}

