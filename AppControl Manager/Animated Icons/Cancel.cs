//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//       LottieGen version:
//           8.1.240821.1+077322fa26
//       
//       Command:
//           LottieGen -Language CSharp -Public -WinUIVersion 3.0 -InputFile Cancel.json
//       
//       Input file:
//           Cancel.json (39720 bytes created 13:22+03:00 Apr 3 2025)
//       
//       LottieGen source:
//           http://aka.ms/Lottie
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
// ____________________________________
// |       Object stats       | Count |
// |__________________________|_______|
// | All CompositionObjects   |    50 |
// |--------------------------+-------|
// | Expression animators     |     1 |
// | KeyFrame animators       |     4 |
// | Reference parameters     |     1 |
// | Expression operations    |     0 |
// |--------------------------+-------|
// | Animated brushes         |     - |
// | Animated gradient stops  |     - |
// | ExpressionAnimations     |     1 |
// | PathKeyFrameAnimations   |     - |
// |--------------------------+-------|
// | ContainerVisuals         |     1 |
// | ShapeVisuals             |     1 |
// |--------------------------+-------|
// | ContainerShapes          |     2 |
// | CompositionSpriteShapes  |     3 |
// |--------------------------+-------|
// | Brushes                  |     2 |
// | Gradient stops           |     4 |
// | CompositionVisualSurface |     - |
// ------------------------------------
using Microsoft.Graphics;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.UI.Composition;
using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;

namespace AnimatedVisuals
{
	// Name:        close-window
	// Frame rate:  24 fps
	// Frame count: 28
	// Duration:    1166.7 mS
	// _____________________________________________________________________________________________
	// |           Marker           |           Constant           | Frame |   mS   |   Progress   |
	// |____________________________|______________________________|_______|________|______________|
	// | NormalToPressed_Start      | M_NormalToPressed_Start      |     0 |    0.0 | 0F           |
	// | NormalToPressed_End        | M_NormalToPressed_End        |     9 |  375.0 | 0.323214293F |
	// | PointerOverToPressed_Start | M_PointerOverToPressed_Start |     9 |  375.0 | 0.323214293F |
	// | PointerOverToPressed_End   | M_PointerOverToPressed_End   |    19 |  791.7 | 0.680357158F |
	// | PressedToNormal_Start      | M_PressedToNormal_Start      |    20 |  833.3 | 0.716071427F |
	// | PressedToNormal_End        | M_PressedToNormal_End        |    28 | 1166.7 | 1F           |
	// | PressedToPointerOver_Start | M_PressedToPointerOver_Start |    28 | 1166.7 | 1F           |
	// | PressedToPointerOver_End   | M_PressedToPointerOver_End   |    28 | 1166.7 | 1F           |
	// ---------------------------------------------------------------------------------------------
	sealed partial class Cancel
		: Microsoft.UI.Xaml.Controls.IAnimatedVisualSource
		, Microsoft.UI.Xaml.Controls.IAnimatedVisualSource2
	{
		// Animation duration: 1.167 seconds.
		internal const long c_durationTicks = 11666666;

		// Marker: NormalToPressed_Start.
		internal const float M_NormalToPressed_Start = 0F;

		// Marker: NormalToPressed_End.
		internal const float M_NormalToPressed_End = 0.323214293F;

		// Marker: PointerOverToPressed_Start.
		internal const float M_PointerOverToPressed_Start = 0.323214293F;

		// Marker: PointerOverToPressed_End.
		internal const float M_PointerOverToPressed_End = 0.680357158F;

		// Marker: PressedToNormal_Start.
		internal const float M_PressedToNormal_Start = 0.716071427F;

		// Marker: PressedToNormal_End.
		internal const float M_PressedToNormal_End = 1F;

		// Marker: PressedToPointerOver_Start.
		internal const float M_PressedToPointerOver_Start = 1F;

		// Marker: PressedToPointerOver_End.
		internal const float M_PressedToPointerOver_End = 1F;

		public Microsoft.UI.Xaml.Controls.IAnimatedVisual TryCreateAnimatedVisual(Compositor compositor)
		{
			object ignored = null;
			return TryCreateAnimatedVisual(compositor, out ignored);
		}

		public Microsoft.UI.Xaml.Controls.IAnimatedVisual TryCreateAnimatedVisual(Compositor compositor, out object diagnostics)
		{
			diagnostics = null;

			var res =
				new Cancel_AnimatedVisual(
					compositor
					);
			res.CreateAnimations();
			return res;
		}

		/// <summary>
		/// Gets the number of frames in the animation.
		/// </summary>
		public double FrameCount => 28d;

		/// <summary>
		/// Gets the frame rate of the animation.
		/// </summary>
		public double Framerate => 24d;

		/// <summary>
		/// Gets the duration of the animation.
		/// </summary>
		public TimeSpan Duration => TimeSpan.FromTicks(11666666);

		/// <summary>
		/// Converts a zero-based frame number to the corresponding progress value denoting the
		/// start of the frame.
		/// </summary>
		public double FrameToProgress(double frameNumber)
		{
			return frameNumber / 28d;
		}

		/// <summary>
		/// Returns a map from marker names to corresponding progress values.
		/// </summary>
		public IReadOnlyDictionary<string, double> Markers =>
			new Dictionary<string, double>
			{
				{ "NormalToPressed_Start", 0d },
				{ "NormalToPressed_End", 0.323214285714286 },
				{ "PointerOverToPressed_Start", 0.323214285714286 },
				{ "PointerOverToPressed_End", 0.680357142857143 },
				{ "PressedToNormal_Start", 0.716071428571429 },
				{ "PressedToNormal_End", 1d },
				{ "PressedToPointerOver_Start", 1d },
				{ "PressedToPointerOver_End", 1d },
			};

		/// <summary>
		/// Sets the color property with the given name, or does nothing if no such property
		/// exists.
		/// </summary>
		public void SetColorProperty(string propertyName, Color value)
		{
		}

		/// <summary>
		/// Sets the scalar property with the given name, or does nothing if no such property
		/// exists.
		/// </summary>
		public void SetScalarProperty(string propertyName, double value)
		{
		}

		sealed partial class Cancel_AnimatedVisual
			: Microsoft.UI.Xaml.Controls.IAnimatedVisual
			, Microsoft.UI.Xaml.Controls.IAnimatedVisual2
		{
			const long c_durationTicks = 11666666;
			readonly Compositor _c;
			readonly ExpressionAnimation _reusableExpressionAnimation;
			AnimationController _animationController_0;
			CompositionColorBrush _colorBrush_White;
			CompositionContainerShape _containerShape_0;
			CompositionContainerShape _containerShape_1;
			CompositionSpriteShape _spriteShape_1;
			CompositionSpriteShape _spriteShape_2;
			ContainerVisual _root;
			CubicBezierEasingFunction _cubicBezierEasingFunction_0;
			StepEasingFunction _holdThenStepEasingFunction;
			StepEasingFunction _stepThenHoldEasingFunction;

			void BindProperty(
				CompositionObject target,
				string animatedPropertyName,
				string expression,
				string referenceParameterName,
				CompositionObject referencedObject)
			{
				_reusableExpressionAnimation.ClearAllParameters();
				_reusableExpressionAnimation.Expression = expression;
				_reusableExpressionAnimation.SetReferenceParameter(referenceParameterName, referencedObject);
				target.StartAnimation(animatedPropertyName, _reusableExpressionAnimation);
			}

			ScalarKeyFrameAnimation CreateScalarKeyFrameAnimation(float initialProgress, float initialValue, CompositionEasingFunction initialEasingFunction)
			{
				var result = _c.CreateScalarKeyFrameAnimation();
				result.Duration = TimeSpan.FromTicks(c_durationTicks);
				result.InsertKeyFrame(initialProgress, initialValue, initialEasingFunction);
				return result;
			}

			Vector2KeyFrameAnimation CreateVector2KeyFrameAnimation(float initialProgress, Vector2 initialValue, CompositionEasingFunction initialEasingFunction)
			{
				var result = _c.CreateVector2KeyFrameAnimation();
				result.Duration = TimeSpan.FromTicks(c_durationTicks);
				result.InsertKeyFrame(initialProgress, initialValue, initialEasingFunction);
				return result;
			}

			AnimationController AnimationController_0()
			{
				if (_animationController_0 != null) { return _animationController_0; }
				var result = _animationController_0 = _c.CreateAnimationController();
				result.Pause();
				BindProperty(_animationController_0, "Progress", "_.Progress", "_", _root);
				return result;
			}

			// - - - - Layer aggregator
			// - - ShapeGroup: Group 1
			CanvasGeometry Geometry_0()
			{
				CanvasGeometry result;
				using (var builder = new CanvasPathBuilder(null))
				{
					builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
					builder.BeginFigure(new Vector2(16F, -18F));
					builder.AddLine(new Vector2(-16F, -18F));
					builder.AddCubicBezier(new Vector2(-17.1049995F, -18F), new Vector2(-18F, -17.1049995F), new Vector2(-18F, -16F));
					builder.AddLine(new Vector2(-18F, 16F));
					builder.AddCubicBezier(new Vector2(-18F, 17.1049995F), new Vector2(-17.1049995F, 18F), new Vector2(-16F, 18F));
					builder.AddLine(new Vector2(16F, 18F));
					builder.AddCubicBezier(new Vector2(17.1049995F, 18F), new Vector2(18F, 17.1049995F), new Vector2(18F, 16F));
					builder.AddLine(new Vector2(18F, -16F));
					builder.AddCubicBezier(new Vector2(18F, -17.1049995F), new Vector2(17.1049995F, -18F), new Vector2(16F, -18F));
					builder.EndFigure(CanvasFigureLoop.Closed);
					result = CanvasGeometry.CreatePath(builder);
				}
				return result;
			}

			// - - - - - Layer aggregator
			// - - - Transforms: C | cross
			// - - Transforms: stick-2
			CanvasGeometry Geometry_1()
			{
				CanvasGeometry result;
				using (var builder = new CanvasPathBuilder(null))
				{
					builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
					builder.BeginFigure(new Vector2(7.0710001F, -8.48499966F));
					builder.AddLine(new Vector2(8.48499966F, -7.0710001F));
					builder.AddCubicBezier(new Vector2(8.8760004F, -6.67999983F), new Vector2(8.8760004F, -6.04699993F), new Vector2(8.48499966F, -5.65700006F));
					builder.AddLine(new Vector2(-5.65700006F, 8.48499966F));
					builder.AddCubicBezier(new Vector2(-6.04799986F, 8.8760004F), new Vector2(-6.68100023F, 8.8760004F), new Vector2(-7.0710001F, 8.48499966F));
					builder.AddLine(new Vector2(-8.48499966F, 7.0710001F));
					builder.AddCubicBezier(new Vector2(-8.8760004F, 6.67999983F), new Vector2(-8.8760004F, 6.04699993F), new Vector2(-8.48499966F, 5.65700006F));
					builder.AddLine(new Vector2(5.65700006F, -8.48499966F));
					builder.AddCubicBezier(new Vector2(6.04699993F, -8.8760004F), new Vector2(6.68100023F, -8.8760004F), new Vector2(7.0710001F, -8.48499966F));
					builder.EndFigure(CanvasFigureLoop.Closed);
					result = CanvasGeometry.CreatePath(builder);
				}
				return result;
			}

			// - - - - - Layer aggregator
			// - - - Transforms: C | cross
			// - - Transforms: stick-1
			CanvasGeometry Geometry_2()
			{
				CanvasGeometry result;
				using (var builder = new CanvasPathBuilder(null))
				{
					builder.SetFilledRegionDetermination(CanvasFilledRegionDetermination.Winding);
					builder.BeginFigure(new Vector2(8.48499966F, 7.0710001F));
					builder.AddLine(new Vector2(7.0710001F, 8.48499966F));
					builder.AddCubicBezier(new Vector2(6.67999983F, 8.8760004F), new Vector2(6.04699993F, 8.8760004F), new Vector2(5.65700006F, 8.48499966F));
					builder.AddLine(new Vector2(-8.48499966F, -5.65700006F));
					builder.AddCubicBezier(new Vector2(-8.8760004F, -6.04799986F), new Vector2(-8.8760004F, -6.68100023F), new Vector2(-8.48499966F, -7.0710001F));
					builder.AddLine(new Vector2(-7.0710001F, -8.48499966F));
					builder.AddCubicBezier(new Vector2(-6.67999983F, -8.8760004F), new Vector2(-6.04699993F, -8.8760004F), new Vector2(-5.65700006F, -8.48499966F));
					builder.AddLine(new Vector2(8.48499966F, 5.65700006F));
					builder.AddCubicBezier(new Vector2(8.8760004F, 6.04699993F), new Vector2(8.8760004F, 6.68100023F), new Vector2(8.48499966F, 7.0710001F));
					builder.EndFigure(CanvasFigureLoop.Closed);
					result = CanvasGeometry.CreatePath(builder);
				}
				return result;
			}

			CompositionColorBrush ColorBrush_White()
			{
				return (_colorBrush_White == null)
					? _colorBrush_White = _c.CreateColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF))
					: _colorBrush_White;
			}

			// - - - Layer aggregator
			// - ShapeGroup: Group 1
			// Stop 0
			CompositionColorGradientStop GradientStop_0_AlmostTomato_FFF44F5A()
			{
				return _c.CreateColorGradientStop(0F, Color.FromArgb(0xFF, 0xF4, 0x4F, 0x5A));
			}

			// - - - Layer aggregator
			// - ShapeGroup: Group 1
			// Stop 1
			CompositionColorGradientStop GradientStop_0p221_AlmostTomato_FFF04652()
			{
				return _c.CreateColorGradientStop(0.221000001F, Color.FromArgb(0xFF, 0xF0, 0x46, 0x52));
			}

			// - - - Layer aggregator
			// - ShapeGroup: Group 1
			// Stop 2
			CompositionColorGradientStop GradientStop_0p443_AlmostTomato_FFED3C49()
			{
				return _c.CreateColorGradientStop(0.442999989F, Color.FromArgb(0xFF, 0xED, 0x3C, 0x49));
			}

			// - - - Layer aggregator
			// - ShapeGroup: Group 1
			// Stop 3
			CompositionColorGradientStop GradientStop_1_AlmostCrimson_FFE41F2F()
			{
				return _c.CreateColorGradientStop(1F, Color.FromArgb(0xFF, 0xE4, 0x1F, 0x2F));
			}

			// Layer aggregator
			CompositionContainerShape ContainerShape_0()
			{
				if (_containerShape_0 != null) { return _containerShape_0; }
				var result = _containerShape_0 = _c.CreateContainerShape();
				result.Offset = new Vector2(96F, 96F);
				var shapes = result.Shapes;
				// ShapeGroup: Group 1
				shapes.Add(SpriteShape_0());
				// Transforms: C | cross
				shapes.Add(ContainerShape_1());
				return result;
			}

			// - Layer aggregator
			// Transforms for C | cross
			CompositionContainerShape ContainerShape_1()
			{
				if (_containerShape_1 != null) { return _containerShape_1; }
				var result = _containerShape_1 = _c.CreateContainerShape();
				result.CenterPoint = new Vector2(50F, 50F);
				result.Offset = new Vector2(-50F, -50F);
				result.Scale = new Vector2(0.0250000004F, 0.0250000004F);
				var shapes = result.Shapes;
				// Transforms: stick-2
				shapes.Add(SpriteShape_1());
				// Transforms: stick-1
				shapes.Add(SpriteShape_2());
				return result;
			}

			// - - Layer aggregator
			// ShapeGroup: Group 1
			CompositionLinearGradientBrush LinearGradientBrush()
			{
				var result = _c.CreateLinearGradientBrush();
				var colorStops = result.ColorStops;
				colorStops.Add(GradientStop_0_AlmostTomato_FFF44F5A());
				colorStops.Add(GradientStop_0p221_AlmostTomato_FFF04652());
				colorStops.Add(GradientStop_0p443_AlmostTomato_FFED3C49());
				colorStops.Add(GradientStop_1_AlmostCrimson_FFE41F2F());
				result.MappingMode = CompositionMappingMode.Absolute;
				result.StartPoint = new Vector2(-18F, -18F);
				result.EndPoint = new Vector2(15.25F, 16.9050007F);
				return result;
			}

			// - - Layer aggregator
			// ShapeGroup: Group 1
			CompositionPathGeometry PathGeometry_0()
			{
				return _c.CreatePathGeometry(new CompositionPath(Geometry_0()));
			}

			// - - - Layer aggregator
			// - Transforms: C | cross
			// Transforms: stick-2
			CompositionPathGeometry PathGeometry_1()
			{
				return _c.CreatePathGeometry(new CompositionPath(Geometry_1()));
			}

			// - - - Layer aggregator
			// - Transforms: C | cross
			// Transforms: stick-1
			CompositionPathGeometry PathGeometry_2()
			{
				return _c.CreatePathGeometry(new CompositionPath(Geometry_2()));
			}

			// - Layer aggregator
			// Path 1
			CompositionSpriteShape SpriteShape_0()
			{
				var result = _c.CreateSpriteShape(PathGeometry_0());
				result.FillBrush = LinearGradientBrush();
				return result;
			}

			// - - Layer aggregator
			// Transforms: C | cross
			// Path 1
			CompositionSpriteShape SpriteShape_1()
			{
				if (_spriteShape_1 != null) { return _spriteShape_1; }
				var result = _spriteShape_1 = _c.CreateSpriteShape(PathGeometry_1());
				result.Offset = new Vector2(50F, 50F);
				result.Scale = new Vector2(40F, 40F);
				result.FillBrush = ColorBrush_White();
				return result;
			}

			// - - Layer aggregator
			// Transforms: C | cross
			// Path 1
			CompositionSpriteShape SpriteShape_2()
			{
				if (_spriteShape_2 != null) { return _spriteShape_2; }
				var result = _spriteShape_2 = _c.CreateSpriteShape(PathGeometry_2());
				result.Offset = new Vector2(50F, 50F);
				result.Scale = new Vector2(40F, 40F);
				result.FillBrush = ColorBrush_White();
				return result;
			}

			// The root of the composition.
			ContainerVisual Root()
			{
				if (_root != null) { return _root; }
				var result = _root = _c.CreateContainerVisual();
				var propertySet = result.Properties;
				propertySet.InsertScalar("Progress", 0F);
				// Layer aggregator
				result.Children.InsertAtTop(ShapeVisual_0());
				return result;
			}

			CubicBezierEasingFunction CubicBezierEasingFunction_0()
			{
				return (_cubicBezierEasingFunction_0 == null)
					? _cubicBezierEasingFunction_0 = _c.CreateCubicBezierEasingFunction(new Vector2(0.333000004F, 0F), new Vector2(0.666999996F, 1F))
					: _cubicBezierEasingFunction_0;
			}

			// - - - Layer aggregator
			// - Transforms: C | cross
			// Transforms: stick-1
			// Rotation
			ScalarKeyFrameAnimation RotationAngleInDegreesScalarAnimation_0_to_0()
			{
				// Frame 0.
				var result = CreateScalarKeyFrameAnimation(0F, 0F, HoldThenStepEasingFunction());
				// Frame 6.
				result.InsertKeyFrame(0.214285716F, 35F, CubicBezierEasingFunction_0());
				// Frame 12.
				result.InsertKeyFrame(0.428571433F, 0F, CubicBezierEasingFunction_0());
				return result;
			}

			// - - - Layer aggregator
			// - Transforms: C | cross
			// Transforms: stick-2
			// Rotation
			ScalarKeyFrameAnimation RotationAngleInDegreesScalarAnimation_0_to_180()
			{
				// Frame 0.
				var result = CreateScalarKeyFrameAnimation(0F, 0F, HoldThenStepEasingFunction());
				// Frame 12.
				result.InsertKeyFrame(0.428571433F, 180F, CubicBezierEasingFunction_0());
				return result;
			}

			// - - Layer aggregator
			// Transforms: C | cross
			// Rotation
			ScalarKeyFrameAnimation RotationAngleInDegreesScalarAnimation_0_to_m90()
			{
				// Frame 0.
				var result = CreateScalarKeyFrameAnimation(0F, 0F, StepThenHoldEasingFunction());
				// Frame 16.
				result.InsertKeyFrame(0.571428597F, 0F, HoldThenStepEasingFunction());
				// Frame 28.
				result.InsertKeyFrame(1F, -90F, CubicBezierEasingFunction_0());
				return result;
			}

			// Layer aggregator
			ShapeVisual ShapeVisual_0()
			{
				var result = _c.CreateShapeVisual();
				result.Size = new Vector2(192F, 192F);
				result.Shapes.Add(ContainerShape_0());
				return result;
			}

			StepEasingFunction HoldThenStepEasingFunction()
			{
				if (_holdThenStepEasingFunction != null) { return _holdThenStepEasingFunction; }
				var result = _holdThenStepEasingFunction = _c.CreateStepEasingFunction();
				result.IsFinalStepSingleFrame = true;
				return result;
			}

			StepEasingFunction StepThenHoldEasingFunction()
			{
				if (_stepThenHoldEasingFunction != null) { return _stepThenHoldEasingFunction; }
				var result = _stepThenHoldEasingFunction = _c.CreateStepEasingFunction();
				result.IsInitialStepSingleFrame = true;
				return result;
			}

			// - Layer aggregator
			// Scale
			Vector2KeyFrameAnimation ScaleVector2Animation()
			{
				// Frame 0.
				var result = CreateVector2KeyFrameAnimation(0F, new Vector2(4F, 4F), StepThenHoldEasingFunction());
				// Frame 16.
				result.InsertKeyFrame(0.571428597F, new Vector2(4F, 4F), HoldThenStepEasingFunction());
				// Frame 22.
				result.InsertKeyFrame(0.785714269F, new Vector2(3.79999995F, 3.79999995F), CubicBezierEasingFunction_0());
				// Frame 28.
				result.InsertKeyFrame(1F, new Vector2(4F, 4F), CubicBezierEasingFunction_0());
				return result;
			}

			internal Cancel_AnimatedVisual(
				Compositor compositor
				)
			{
				_c = compositor;
				_reusableExpressionAnimation = compositor.CreateExpressionAnimation();
				Root();
			}

			public Visual RootVisual => _root;
			public TimeSpan Duration => TimeSpan.FromTicks(c_durationTicks);
			public Vector2 Size => new Vector2(192F, 192F);
			void IDisposable.Dispose() => _root?.Dispose();

			public void CreateAnimations()
			{
				_containerShape_0.StartAnimation("Scale", ScaleVector2Animation(), AnimationController_0());
				_containerShape_1.StartAnimation("RotationAngleInDegrees", RotationAngleInDegreesScalarAnimation_0_to_m90(), AnimationController_0());
				_spriteShape_1.StartAnimation("RotationAngleInDegrees", RotationAngleInDegreesScalarAnimation_0_to_180(), AnimationController_0());
				_spriteShape_2.StartAnimation("RotationAngleInDegrees", RotationAngleInDegreesScalarAnimation_0_to_0(), AnimationController_0());
			}

			public void DestroyAnimations()
			{
				_containerShape_0.StopAnimation("Scale");
				_containerShape_1.StopAnimation("RotationAngleInDegrees");
				_spriteShape_1.StopAnimation("RotationAngleInDegrees");
				_spriteShape_2.StopAnimation("RotationAngleInDegrees");
			}

		}
	}
}
