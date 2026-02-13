# CustomCommandDemo (AKA Barcoder)
If all you do is run this demo, you'll see that it merely lets you open an image
and then flip it horizontally or vertically.

However, under the hood, you'll see that instead of just using DotImages built
in and very capable FlipCommand class, we're actually doing the work inside
CustomFlipCommand.

What is this CustomFlipCommand? It's an example of inheriting our base
ImageCommand and using it to build your own. Using the PixelAccessor and
PixelMemory classes, we're directly manipulating the underlying pixels that make
up the image. What we end up doing is simply rearranging the image, flipping
horizontally or vertically... what you do with it is left up to your
imagination.

The PixelAccessor and PixelMemory objects are certainly available outside of the
ImageCommand structure, but by implementing this as an ImageCommand, you can now
use your CustomImageCommand anywhere you would use any of our existing
ImageCommand classes.

This is the C# version

## Cloning
We recommend the following to ensure you clone with the required submodule

Example: git for windows
```bash
git clone https://github.com/AtalaSupport/DemoGallery_CustomCommandDemo_CS_x64.git CustomCommandDemo
cd CustomCommandDemo
git submodule init
git pull
```
