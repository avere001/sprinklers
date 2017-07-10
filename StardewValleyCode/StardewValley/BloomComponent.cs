using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace StardewValley
{
	public class BloomComponent : DrawableGameComponent
	{
		public enum IntermediateBuffer
		{
			PreBloom,
			BlurredHorizontally,
			BlurredBothWays,
			FinalResult
		}

		private SpriteBatch spriteBatch;

		private Effect bloomExtractEffect;

		private Effect brightWhiteEffect;

		private Effect bloomCombineEffect;

		private Effect gaussianBlurEffect;

		private RenderTarget2D sceneRenderTarget;

		private RenderTarget2D renderTarget1;

		private RenderTarget2D renderTarget2;

		public float hueShiftR;

		public float hueShiftG;

		public float hueShiftB;

		public float timeLeftForShifting;

		public float totalTime;

		public float shiftRate;

		public float offsetShift;

		public float shiftFade;

		public float blurLevel;

		public float saturationLevel;

		public float contrastLevel;

		public float bloomLevel;

		public float brightnessLevel;

		public float globalIntensity;

		public float globalIntensityMax;

		public float rabbitHoleTimer;

		private bool cyclingShift;

		private BloomSettings settings = BloomSettings.PresetSettings[5];

		private BloomSettings targetSettings = BloomSettings.PresetSettings[5];

		private BloomSettings oldSetting = BloomSettings.PresetSettings[5];

		private BloomComponent.IntermediateBuffer showBuffer = BloomComponent.IntermediateBuffer.FinalResult;

		public BloomSettings Settings
		{
			get
			{
				return this.settings;
			}
			set
			{
				this.settings = value;
			}
		}

		public BloomComponent.IntermediateBuffer ShowBuffer
		{
			get
			{
				return this.showBuffer;
			}
			set
			{
				this.showBuffer = value;
			}
		}

		public BloomComponent(Game game) : base(game)
		{
			if (game == null)
			{
				throw new ArgumentNullException("game");
			}
		}

		public void startShifting(float howLongMilliseconds, float shiftRate, float shiftFade, float globalIntensityMax, float blurShiftLevel, float saturationShiftLevel, float contrastShiftLevel, float bloomIntensityShift, float brightnessShift, float globalIntensityStart = 1f, float offsetShift = 3000f, bool cyclingShift = true)
		{
			this.timeLeftForShifting = howLongMilliseconds;
			this.totalTime = howLongMilliseconds;
			this.shiftRate = shiftRate;
			this.blurLevel = blurShiftLevel;
			this.saturationLevel = saturationShiftLevel;
			this.contrastLevel = contrastShiftLevel;
			this.bloomLevel = bloomIntensityShift;
			this.brightnessLevel = brightnessShift;
			base.Visible = true;
			this.oldSetting = new BloomSettings("old", this.settings.BloomThreshold, this.settings.BlurAmount, this.settings.BloomIntensity, this.settings.BaseIntensity, this.settings.BloomSaturation, this.settings.BaseSaturation, false);
			this.targetSettings = new BloomSettings("old", this.settings.BloomThreshold, this.settings.BlurAmount, this.settings.BloomIntensity, this.settings.BaseIntensity, this.settings.BloomSaturation, this.settings.BaseSaturation, false);
			this.cyclingShift = cyclingShift;
			this.shiftFade = shiftFade;
			this.globalIntensity = globalIntensityStart;
			this.globalIntensityMax = globalIntensityMax / 2f;
			this.offsetShift = offsetShift;
			Game1.debugOutput = string.Concat(new object[]
			{
				howLongMilliseconds,
				" ",
				shiftRate,
				" ",
				shiftFade,
				" ",
				globalIntensityMax,
				" ",
				blurShiftLevel,
				" ",
				saturationShiftLevel,
				" ",
				contrastShiftLevel,
				" ",
				bloomIntensityShift,
				" ",
				brightnessShift,
				" ",
				globalIntensityStart,
				" ",
				offsetShift
			});
			this.hueShiftR = 0f;
			this.hueShiftB = 0f;
			this.hueShiftG = 0f;
		}

		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch(base.GraphicsDevice);
			this.bloomExtractEffect = base.Game.Content.Load<Effect>("BloomExtract");
			this.bloomCombineEffect = base.Game.Content.Load<Effect>("BloomCombine");
			this.gaussianBlurEffect = base.Game.Content.Load<Effect>("GaussianBlur");
			this.brightWhiteEffect = base.Game.Content.Load<Effect>("BrightWhite");
			PresentationParameters presentationParameters = base.GraphicsDevice.PresentationParameters;
			int num = presentationParameters.BackBufferWidth;
			int num2 = presentationParameters.BackBufferHeight;
			SurfaceFormat backBufferFormat = presentationParameters.BackBufferFormat;
			this.sceneRenderTarget = new RenderTarget2D(base.GraphicsDevice, num, num2, false, backBufferFormat, presentationParameters.DepthStencilFormat, presentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
			num /= 2;
			num2 /= 2;
			this.renderTarget1 = new RenderTarget2D(base.GraphicsDevice, num, num2, false, backBufferFormat, DepthFormat.None);
			this.renderTarget2 = new RenderTarget2D(base.GraphicsDevice, num, num2, false, backBufferFormat, DepthFormat.None);
		}

		public void reload()
		{
			PresentationParameters presentationParameters = base.GraphicsDevice.PresentationParameters;
			int num = presentationParameters.BackBufferWidth;
			int num2 = presentationParameters.BackBufferHeight;
			SurfaceFormat backBufferFormat = presentationParameters.BackBufferFormat;
			this.sceneRenderTarget = new RenderTarget2D(base.GraphicsDevice, num, num2, false, backBufferFormat, presentationParameters.DepthStencilFormat, presentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
			num /= 2;
			num2 /= 2;
			this.renderTarget1 = new RenderTarget2D(base.GraphicsDevice, num, num2, false, backBufferFormat, DepthFormat.None);
			this.renderTarget2 = new RenderTarget2D(base.GraphicsDevice, num, num2, false, backBufferFormat, DepthFormat.None);
		}

		protected override void UnloadContent()
		{
			this.sceneRenderTarget.Dispose();
			this.renderTarget1.Dispose();
			this.renderTarget2.Dispose();
		}

		public void BeginDraw()
		{
			if (base.Visible)
			{
				base.GraphicsDevice.SetRenderTarget(this.sceneRenderTarget);
			}
		}

		public void tick(GameTime time)
		{
			if (this.timeLeftForShifting > 0f)
			{
				base.Visible = true;
				this.timeLeftForShifting -= (float)time.ElapsedGameTime.Milliseconds;
				this.shiftRate = Math.Max(0.0001f, this.shiftRate + this.shiftFade * (float)time.ElapsedGameTime.Milliseconds);
				if (this.cyclingShift)
				{
					this.offsetShift += (float)time.ElapsedGameTime.Milliseconds / 10f;
					this.globalIntensity = this.globalIntensityMax * (float)Math.Cos(((double)this.timeLeftForShifting - (double)this.totalTime * 3.1415926535897931 * 4.0) * (6.2831853071795862 / (double)this.totalTime)) + this.globalIntensityMax;
					float num = this.offsetShift * (float)Math.Sin((double)this.timeLeftForShifting * 6.2831853071795862 / (double)this.totalTime);
					this.targetSettings.BaseSaturation = Math.Max(1f, 0.25f * this.globalIntensity * (this.saturationLevel * (float)Math.Sin((double)(this.timeLeftForShifting - num / 2f) * 6.2831853071795862 / (double)this.shiftRate) + (0.25f * this.globalIntensity + this.saturationLevel)));
					this.targetSettings.BloomIntensity = Math.Max(0f, 0.5f * this.globalIntensity * (this.bloomLevel / 2f * (float)Math.Sin((double)(this.timeLeftForShifting - num * 2f) * 6.2831853071795862 / (double)this.shiftRate) + (0.5f * this.globalIntensity + this.bloomLevel / 2f)));
					this.targetSettings.BlurAmount = Math.Max(0f, 1f * this.globalIntensity * (this.blurLevel * (float)Math.Sin((double)this.timeLeftForShifting * 6.2831853071795862 / (double)(this.shiftRate / 2f))) + (1f * this.globalIntensity + this.blurLevel));
					this.settings.BaseSaturation += (this.targetSettings.BaseSaturation - this.settings.BaseSaturation) / 10f;
					this.settings.BloomIntensity += (this.targetSettings.BloomIntensity - this.settings.BloomIntensity) / 10f;
					this.settings.BaseIntensity += (this.targetSettings.BaseIntensity - this.settings.BaseIntensity) / 10f;
					this.settings.BlurAmount += (this.targetSettings.BaseSaturation - this.settings.BlurAmount) / 10f;
					this.hueShiftR = this.globalIntensity / 2f * (float)(Math.Cos((double)(this.timeLeftForShifting - num / 2f) * 6.2831853071795862 / (double)(this.shiftRate / 2f)) + 1.0) / 4f;
					this.hueShiftG = this.globalIntensity / 2f * (float)(Math.Sin((double)(this.timeLeftForShifting - num / 2f) * 6.2831853071795862 / (double)(this.shiftRate / 2f)) + 1.0) / 4f;
					this.hueShiftB = this.globalIntensity / 2f * (float)(Math.Cos((double)(this.timeLeftForShifting - num / 2f - this.totalTime / 2f) * 6.2831853071795862 / (double)this.shiftRate) + 1.0) / 4f;
					this.rabbitHoleTimer -= (float)time.ElapsedGameTime.Milliseconds;
					if (this.rabbitHoleTimer <= 0f)
					{
						this.rabbitHoleTimer = 1000f;
						Console.WriteLine(string.Concat(new object[]
						{
							"timeLeft: ",
							this.timeLeftForShifting,
							" shiftRate: ",
							this.shiftRate,
							" globalIntensity: ",
							this.globalIntensity,
							" settings.BloomThreshold: ",
							this.settings.BloomThreshold,
							" settings.BaseSaturation: ",
							this.settings.BaseSaturation,
							" settings.BloomIntensity: ",
							this.settings.BloomIntensity,
							" settings.BaseIntensity: ",
							this.settings.BaseIntensity,
							" settings.BlurAmount: ",
							this.settings.BlurAmount,
							" hueShift: ",
							this.hueShiftR,
							",",
							this.hueShiftG,
							",",
							this.hueShiftB,
							" x,y: "
						}));
					}
				}
				if (this.timeLeftForShifting <= 0f)
				{
					this.hueShiftR = 0f;
					this.hueShiftG = 0f;
					this.hueShiftB = 0f;
					this.settings = this.oldSetting;
					if (Game1.bloomDay && Game1.currentLocation.isOutdoors)
					{
						base.Visible = true;
						return;
					}
					base.Visible = false;
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			if (this.settings == null)
			{
				return;
			}
			base.GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
			if (this.settings.brightWhiteOnly)
			{
				this.DrawFullscreenQuad(this.sceneRenderTarget, this.renderTarget1, this.brightWhiteEffect, BloomComponent.IntermediateBuffer.PreBloom);
			}
			else
			{
				this.bloomExtractEffect.Parameters["BloomThreshold"].SetValue(this.Settings.BloomThreshold);
				this.DrawFullscreenQuad(this.sceneRenderTarget, this.renderTarget1, this.bloomExtractEffect, BloomComponent.IntermediateBuffer.PreBloom);
			}
			this.SetBlurEffectParameters(1f / (float)this.renderTarget1.Width, 0f);
			this.DrawFullscreenQuad(this.renderTarget1, this.renderTarget2, this.gaussianBlurEffect, BloomComponent.IntermediateBuffer.BlurredHorizontally);
			this.SetBlurEffectParameters(0f, 1f / (float)this.renderTarget1.Height);
			this.DrawFullscreenQuad(this.renderTarget2, this.renderTarget1, this.gaussianBlurEffect, BloomComponent.IntermediateBuffer.BlurredBothWays);
			base.GraphicsDevice.SetRenderTarget(null);
			EffectParameterCollection expr_108 = this.bloomCombineEffect.Parameters;
			expr_108["BloomIntensity"].SetValue(this.Settings.BloomIntensity);
			expr_108["BaseIntensity"].SetValue(this.Settings.BaseIntensity);
			expr_108["BloomSaturation"].SetValue(this.Settings.BloomSaturation);
			expr_108["BaseSaturation"].SetValue(this.Settings.BaseSaturation);
			expr_108["HueR"].SetValue((float)Math.Round((double)this.hueShiftR, 2));
			expr_108["HueG"].SetValue((float)Math.Round((double)this.hueShiftG, 2));
			expr_108["HueB"].SetValue((float)Math.Round((double)this.hueShiftB, 2));
			base.GraphicsDevice.Textures[1] = this.sceneRenderTarget;
			Viewport viewport = base.GraphicsDevice.Viewport;
			this.DrawFullscreenQuad(this.renderTarget1, viewport.Width, viewport.Height, this.bloomCombineEffect, BloomComponent.IntermediateBuffer.FinalResult);
		}

		private void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget, Effect effect, BloomComponent.IntermediateBuffer currentBuffer)
		{
			base.GraphicsDevice.SetRenderTarget(renderTarget);
			this.DrawFullscreenQuad(texture, renderTarget.Width, renderTarget.Height, effect, currentBuffer);
		}

		private void DrawFullscreenQuad(Texture2D texture, int width, int height, Effect effect, BloomComponent.IntermediateBuffer currentBuffer)
		{
			if (this.showBuffer < currentBuffer)
			{
				effect = null;
			}
			this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, null, null, effect);
			this.spriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
			this.spriteBatch.End();
		}

		private void SetBlurEffectParameters(float dx, float dy)
		{
			EffectParameter effectParameter = this.gaussianBlurEffect.Parameters["SampleWeights"];
			EffectParameter effectParameter2 = this.gaussianBlurEffect.Parameters["SampleOffsets"];
			int count = effectParameter.Elements.Count;
			float[] array = new float[count];
			Vector2[] array2 = new Vector2[count];
			array[0] = this.ComputeGaussian(0f);
			array2[0] = new Vector2(0f);
			float num = array[0];
			for (int i = 0; i < count / 2; i++)
			{
				float num2 = this.ComputeGaussian((float)(i + 1));
				array[i * 2 + 1] = num2;
				array[i * 2 + 2] = num2;
				num += num2 * 2f;
				float scaleFactor = (float)(i * 2) + 1.5f;
				Vector2 vector = new Vector2(dx, dy) * scaleFactor;
				array2[i * 2 + 1] = vector;
				array2[i * 2 + 2] = -vector;
			}
			for (int j = 0; j < array.Length; j++)
			{
				array[j] /= num;
			}
			effectParameter.SetValue(array);
			effectParameter2.SetValue(array2);
		}

		private float ComputeGaussian(float n)
		{
			float blurAmount = this.Settings.BlurAmount;
			return (float)(1.0 / Math.Sqrt(6.2831853071795862 * (double)blurAmount) * Math.Exp((double)(-(double)(n * n) / (2f * blurAmount * blurAmount))));
		}
	}
}
