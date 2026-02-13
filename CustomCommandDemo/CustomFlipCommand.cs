using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Atalasoft.Imaging;
using Atalasoft.Imaging.ImageProcessing;
using Atalasoft.Imaging.Memory;

namespace CustomCommandDemo
{

	/// <summary>This code demonstrates how to use PixelAccessor in an ImageCommand.</summary>
	/// <remarks>
	/// This is an extremely simple command and does not work with 1-bit or 4-bit
	/// images.
	/// </remarks>
	public class CustomFlipCommand : ImageCommand, ISerializable
	{
		private FlipDirection _direction = FlipDirection.Horizontal;

		/// <summary>
		/// Creates a new instance of CustomFlipCommand that will perform a horizontal
		/// flip.
		/// </summary>
		public CustomFlipCommand()
		{
		}

		/// <summary>
		/// Creates a new instance of CustomFlipCommand and specifies the type of flip to
		/// perform.
		/// </summary>
		/// <param name="direction">The flip direction.</param>
		public CustomFlipCommand(FlipDirection direction)
		{
			this._direction = direction;
		}

		#region ISerializable

		protected CustomFlipCommand(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (info == null)
				throw new ArgumentNullException("info", "The parameter 'info' can't be null.");
			
			this._direction = (FlipDirection)SerializationHelper.GetValue(info, "Direction", typeof(FlipDirection));
		}
		
		/// <summary>Fills a SerializationInfo object with information about this command.</summary>
		/// <param name="info">The SerializationInfo to fill.</param>
		/// <param name="context">A StreamingContext for this info.</param>
		[SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException("info", "The parameter 'info' can't be null.");
			
			ImageCommandGetObjectData(info, context);
			info.AddValue("Direction", this._direction);
		}

		#endregion

		#region Properties
			
		/// <summary>Gets or sets the flip direction.</summary>
		/// <value>The flip direction.</value>
		public FlipDirection Direction
		{
			get { return this._direction; }
			set	{ this._direction = value; }
		}

		static PixelFormat[] _supportedPixelFormats = new PixelFormat[] 
		{
			PixelFormat.Pixel24bppBgr,
			PixelFormat.Pixel8bppGrayscale,
			PixelFormat.Pixel32bppBgr,
			PixelFormat.Pixel8bppIndexed,
			PixelFormat.Pixel32bppBgra,
			PixelFormat.Pixel32bppCmyk,
			PixelFormat.Pixel16bppGrayscaleAlpha,
			PixelFormat.Pixel16bppGrayscale,
			PixelFormat.Pixel48bppBgr,
			PixelFormat.Pixel64bppBgra
		};

		/// <summary>Returns the pixel formats supported by this command.</summary>
		public override PixelFormat[] SupportedPixelFormats { get { return _supportedPixelFormats; } }

		#endregion

		protected override void VerifyProperties(AtalaImage image)
		{

		}

		/// <summary>Indicates that this command uses in-place processing.</summary>
		public override bool InPlaceProcessing
		{
			get	{ return true; }
		}

		protected override AtalaImage ConstructFinalImage(AtalaImage image)
		{
			// We are performing in-place processing.
			return null;
		}

		protected override AtalaImage PerformActualCommand(AtalaImage source, AtalaImage dest, Rectangle imageArea, ref ImageResults results)
		{
			PixelMemory pm = source.PixelMemory;

			using (PixelAccessor srcPa = pm.AcquirePixelAccessor())
			{
				if (this._direction == FlipDirection.Horizontal)
				{
					// Because we are limiting this demo to whole byte
					// images, this code is simplified.
					int bytesPerPixel = (source.ColorDepth / 8);
					int pixelRowBytes = bytesPerPixel * source.Width;
					byte[] tmp = new byte[pm.RowStride];

					for (int h = 0; h < pm.Height; h++)
					{
						int rPos = pixelRowBytes - bytesPerPixel;
						byte[] bytes = srcPa.AcquireScanline(h);

						for (int w = 0; w < source.Width; w++)
						{
							int lPos = w * bytesPerPixel;

							for (int i = 0; i < bytesPerPixel; i++)
							{
								tmp[rPos + i] = bytes[lPos + i];
							}

							rPos -= bytesPerPixel;
						}

						Array.Copy(tmp, 0, bytes, 0, pm.RowStride);
						srcPa.ReleaseScanline();
					}
				}
				else
				{
					using (PixelAccessor destPa = pm.AcquirePixelAccessor())
					{
						// For vertical flipping we will simply swap scan lines,
						// moving from the outside toward the center.
						int rs = pm.RowStride;
						int halfHeight = pm.Height / 2;
						byte[] topHalf;
						byte[] bottomHalf;
						byte[] tmp = new byte[rs];

						for (int h = 0; h < halfHeight; h++)
						{
							topHalf = srcPa.AcquireScanline(h);
							bottomHalf = destPa.AcquireScanline(pm.Height - h - 1);

							Array.Copy(topHalf, 0, tmp, 0, rs);
							Array.Copy(bottomHalf, 0, topHalf, 0, rs);
							Array.Copy(tmp, 0, bottomHalf, 0, rs);

							destPa.ReleaseScanline();
							srcPa.ReleaseScanline();
						}
					}
				}
			}

			return null;
		}
	}
}
