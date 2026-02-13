using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using WinDemoHelperMethods;

namespace CustomCommandDemo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{ 
		private Atalasoft.Imaging.WinControls.WorkspaceViewer viewer;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuFile;
		private System.Windows.Forms.MenuItem menuFileOpen;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuExit;
		private System.Windows.Forms.MenuItem menuCommand;
		private System.Windows.Forms.MenuItem menuCommandFlip;
        private System.Windows.Forms.MenuItem menuFlipH;
        private MenuItem menuItem1;
        private MenuItem menuItemAbout;
        private IContainer components;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// Add a WorkspaceViewer.
			this.viewer = new Atalasoft.Imaging.WinControls.WorkspaceViewer();
			this.viewer.Dock = DockStyle.Fill;
			this.Controls.Add(this.viewer);
			HelperMethods.PopulateDecoders(Atalasoft.Imaging.Codec.RegisteredDecoders.Decoders);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuFile = new System.Windows.Forms.MenuItem();
            this.menuFileOpen = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.menuCommand = new System.Windows.Forms.MenuItem();
            this.menuFlipH = new System.Windows.Forms.MenuItem();
            this.menuCommandFlip = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItemAbout = new System.Windows.Forms.MenuItem();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFile,
            this.menuCommand,
            this.menuItem1});
            // 
            // menuFile
            // 
            this.menuFile.Index = 0;
            this.menuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFileOpen,
            this.menuItem3,
            this.menuExit});
            this.menuFile.Text = "&File";
            // 
            // menuFileOpen
            // 
            this.menuFileOpen.Index = 0;
            this.menuFileOpen.Text = "&Open";
            this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "-";
            // 
            // menuExit
            // 
            this.menuExit.Index = 2;
            this.menuExit.Text = "E&xit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuCommand
            // 
            this.menuCommand.Index = 1;
            this.menuCommand.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuFlipH,
            this.menuCommandFlip});
            this.menuCommand.Text = "&Command";
            // 
            // menuFlipH
            // 
            this.menuFlipH.Index = 0;
            this.menuFlipH.Text = "Flip Horizontal";
            this.menuFlipH.Click += new System.EventHandler(this.menuFlipH_Click);
            // 
            // menuCommandFlip
            // 
            this.menuCommandFlip.Index = 1;
            this.menuCommandFlip.Text = "Flip Vertical";
            this.menuCommandFlip.Click += new System.EventHandler(this.menuCommandFlip_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 2;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemAbout});
            this.menuItem1.Text = "&Help";
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Index = 0;
            this.menuItemAbout.Text = "&About ...";
            this.menuItemAbout.Click += new System.EventHandler(this.menuItemAbout_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(704, 478);
            this.Menu = this.mainMenu1;
            this.Name = "Form1";
            this.Text = "Atalasoft Custom ImageCommand Demo";
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void ShowErrorDialog(string message, string title, Exception exception)
		{
			message += "\r\n\r\nException:  " + exception.Message;
			if (exception.InnerException != null)
				message += "\r\n\r\nInner Exception:  " + exception.InnerException.Message;

			MessageBox.Show(this, message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void menuExit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void menuFileOpen_Click(object sender, System.EventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = HelperMethods.CreateDialogFilter(true);

			// try to locate images folder
			string imagesFolder = Application.ExecutablePath;
			// we assume we are running under the DotImage install folder
			int pos = imagesFolder.IndexOf("DotImage ");
			if (pos != -1)
			{
				imagesFolder = imagesFolder.Substring(0,imagesFolder.IndexOf(@"\",pos)) + @"\Images\PhotoEffects";
			}

			//use this folder as starting point			
			dlg.InitialDirectory = imagesFolder;

			try
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
					this.viewer.Open(dlg.FileName, 0);
			}
			catch (Exception ex)
			{
				ShowErrorDialog("Error loading image.", "Load Error", ex);
			}
			finally
			{
				dlg.Dispose();
			}
		}

		private void menuCommandFlip_Click(object sender, System.EventArgs e)
		{
			ApplyCustomCommand(Atalasoft.Imaging.FlipDirection.Vertical);
		}

		private void menuFlipH_Click(object sender, System.EventArgs e)
		{
			ApplyCustomCommand(Atalasoft.Imaging.FlipDirection.Horizontal);
		}

		private void ApplyCustomCommand(Atalasoft.Imaging.FlipDirection direction)
		{
			if (this.viewer.Image == null)
			{
				MessageBox.Show(this, "Please load an image before applying a command.", "No Image");
				return;
			}

			try
			{
				this.viewer.ApplyCommand(new CustomFlipCommand(direction));
			}
			catch (Exception ex)
			{
				ShowErrorDialog("There was an error while applying the command.", "Apply Command Error", ex);
			}
		}

        private void menuItemAbout_Click(object sender, EventArgs e)
        {
            AtalaDemos.AboutBox.About aboutBox = new AtalaDemos.AboutBox.About("About Atalasoft DotImage Custom Command Demo", "Custom Command Demo");
            aboutBox.Description = "If all you do is run this demo, you'll see that it merely lets you open an image and then flip it horizontally or vertically." +
                                    "\r\n\r\n" +
                                    "However, under the hood, you'll see that instead of just using DotImages built in and very capable FlipCommand class, we're actually doing the work inside CustomFlipCommand." +
                                    "\r\n\r\n" +
                                    "What is this CustomFlipCommand? It's an example of inheriting our base ImageCommand and using it to build your own. Using the PixelAccessor and PixelMemory classes, we're directly manipulating the underlying pixels that make up the image. What we end up doing is simply rearranging the image, flipping horizontally or vertically... what you do with it is left up to your imagination." +
                                    "\r\n\r\n" +
                                    "The PixelAccessor and PixelMemory objects are certainly available outside of the ImageCommand structure, but by implementing this as an ImageCommand, you can now use your CustomImageCommand anywhere you would use any of our existing ImageCommand classes.";
            aboutBox.ShowDialog();
        }
	}
}
