// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using System.Collections.Generic;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Vision.FaceLandmarker;
using UnityEngine;
using UnityEngine.Rendering;

namespace Mediapipe.Unity.Sample.FaceLandmarkDetection
{
  public class FaceLandmarkerRunner : VisionTaskApiRunner<FaceLandmarker>
  {
    [SerializeField] private FaceLandmarkerResultAnnotationController _faceLandmarkerResultAnnotationController;

    private Experimental.TextureFramePool _textureFramePool;

    public readonly FaceLandmarkDetectionConfig config = new FaceLandmarkDetectionConfig();

    public override void Stop()
    {
      base.Stop();
      _textureFramePool?.Dispose();
      _textureFramePool = null;
    }

    protected override IEnumerator Run()
    {
      Debug.Log($"Delegate = {config.Delegate}");
      Debug.Log($"Running Mode = {config.RunningMode}");
      Debug.Log($"NumFaces = {config.NumFaces}");
      Debug.Log($"MinFaceDetectionConfidence = {config.MinFaceDetectionConfidence}");
      Debug.Log($"MinFacePresenceConfidence = {config.MinFacePresenceConfidence}");
      Debug.Log($"MinTrackingConfidence = {config.MinTrackingConfidence}");
      Debug.Log($"OutputFaceBlendshapes = {config.OutputFaceBlendshapes}");
      Debug.Log($"OutputFacialTransformationMatrixes = {config.OutputFacialTransformationMatrixes}");

      yield return AssetLoader.PrepareAssetAsync(config.ModelPath);

      var options = config.GetFaceLandmarkerOptions(config.RunningMode == Tasks.Vision.Core.RunningMode.LIVE_STREAM ? OnFaceLandmarkDetectionOutput : null);
      taskApi = FaceLandmarker.CreateFromOptions(options, GpuManager.GpuResources);
      var imageSource = ImageSourceProvider.ImageSource;

      yield return imageSource.Play();

      if (!imageSource.isPrepared)
      {
        Debug.LogError("Failed to start ImageSource, exiting...");
        yield break;
      }

      // Use RGBA32 as the input format.
      // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so maybe the following code needs to be fixed.
      _textureFramePool = new Experimental.TextureFramePool(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, 10);

      // NOTE: The screen will be resized later, keeping the aspect ratio.
      screen.Initialize(imageSource);

      SetupAnnotationController(_faceLandmarkerResultAnnotationController, imageSource);

      var transformationOptions = imageSource.GetTransformationOptions();
      var flipHorizontally = transformationOptions.flipHorizontally;
      var flipVertically = transformationOptions.flipVertically;
      var imageProcessingOptions = new Tasks.Vision.Core.ImageProcessingOptions(rotationDegrees: (int)transformationOptions.rotationAngle);

      AsyncGPUReadbackRequest req = default;
      var waitUntilReqDone = new WaitUntil(() => req.done);
      var result = FaceLandmarkerResult.Alloc(options.numFaces);

      // NOTE: we can share the GL context of the render thread with MediaPipe (for now, only on Android)
      var canUseGpuImage = options.baseOptions.delegateCase == Tasks.Core.BaseOptions.Delegate.GPU &&
        SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLES3 &&
        GpuManager.GpuResources != null;
      using var glContext = canUseGpuImage ? GpuManager.GetGlContext() : null;

      while (true)
      {
        if (isPaused)
        {
          yield return new WaitWhile(() => isPaused);
        }

        if (!_textureFramePool.TryGetTextureFrame(out var textureFrame))
        {
          yield return new WaitForEndOfFrame();
          continue;
        }

        // Build the input Image
        Image image;
        if (canUseGpuImage)
        {
          yield return new WaitForEndOfFrame();
          textureFrame.ReadTextureOnGPU(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
          image = textureFrame.BuildGpuImage(glContext);
        }
        else
        {
          req = textureFrame.ReadTextureAsync(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
          yield return waitUntilReqDone;

          if (req.hasError)
          {
            Debug.LogError($"Failed to read texture from the image source, exiting...");
            break;
          }
          image = textureFrame.BuildCPUImage();
          textureFrame.Release();
        }

        switch (taskApi.runningMode)
        {
          case Tasks.Vision.Core.RunningMode.IMAGE:
            if (taskApi.TryDetect(image, imageProcessingOptions, ref result))
            {
              _faceLandmarkerResultAnnotationController.DrawNow(result);
            }
            else
            {
              _faceLandmarkerResultAnnotationController.DrawNow(default);
            }
            break;
          case Tasks.Vision.Core.RunningMode.VIDEO:
            if (taskApi.TryDetectForVideo(image, GetCurrentTimestampMillisec(), imageProcessingOptions, ref result))
            {
              _faceLandmarkerResultAnnotationController.DrawNow(result);
            }
            else
            {
              _faceLandmarkerResultAnnotationController.DrawNow(default);
            }
            break;
          case Tasks.Vision.Core.RunningMode.LIVE_STREAM:
            taskApi.DetectAsync(image, GetCurrentTimestampMillisec(), imageProcessingOptions);
            break;
        }
      }
    }

    // ------------------------------------------------------------------------------------------------------------------------------

    public event Action<bool> OnFaceAppear;
    private bool noFaceExist;
    public bool NoFaceExist
    {
      get {return noFaceExist;}
      private set
      {
        if (noFaceExist != value)
        {
          noFaceExist = value;
          MainThreadDispatcher.Enqueue(() => OnFaceAppear?.Invoke(noFaceExist));
        }
      }
    }

    private Dictionary<string, float> BlendshapesStandard = new Dictionary<string, float>()
    {
      ["browDownLeft"] = 0.4f,
      ["browDownRight"] = 0.4f,
      ["browInnerUp"] = 0.3f,
      ["browOuterUpLeft"] = 0.3f,
      ["browOuterUpRight"] = 0.3f,
      ["cheekPuff"] = 0.2f,
      ["cheekSquintLeft"] = 0.6f,
      ["cheekSquintRight"] = 0.6f,
      ["eyeBlinkLeft"] = 0.5f,
      ["eyeBlinkRight"] = 0.5f,
      ["eyeLookDownLeft"] = 0.4f,
      ["eyeLookDownRight"] = 0.4f,
      ["eyeLookInLeft"] = 0.5f,
      ["eyeLookInRight"] = 0.5f,
      ["eyeLookOutLeft"] = 0.5f,
      ["eyeLookOutRight"] = 0.5f,
      ["eyeLookUpLeft"] = 0.4f,
      ["eyeLookUpRight"] = 0.4f,
      ["eyeSquintLeft"] = 0.6f,
      ["eyeSquintRight"] = 0.6f,
      ["eyeWideLeft"] = 0.025f,
      ["eyeWideRight"] = 0.025f,
      ["jawForward"] = 0.3f,
      ["jawLeft"] = 0.3f,
      ["jawOpen"] = 0.2f,
      ["jawRight"] = 0.3f,
      ["mouthClose"] = 0.5f,
      ["mouthDimpleLeft"] = 0.5f,
      ["mouthDimpleRight"] = 0.5f,
      ["mouthFrownLeft"] = 0.1f,
      ["mouthFrownRight"] = 0.1f,
      ["mouthFunnel"] = 0.4f,
      ["mouthLeft"] = 0.4f,
      ["mouthLowerDownLeft"] = 0.4f,
      ["mouthLowerDownRight"] = 0.4f,
      ["mouthPressLeft"] = 0.4f,
      ["mouthPressRight"] = 0.4f,
      ["mouthPucker"] = 0.4f,
      ["mouthRight"] = 0.4f,
      ["mouthRollLower"] = 0.5f,
      ["mouthRollUpper"] = 0.5f,
      ["mouthShrugLower"] = 0.5f,
      ["mouthShrugUpper"] = 0.5f,
      ["mouthSmileLeft"] = 0.1f,
      ["mouthSmileRight"] = 0.1f,
      ["mouthStretchLeft"] = 0.4f,
      ["mouthStretchRight"] = 0.4f,
      ["mouthUpperUpLeft"] = 0.4f,
      ["mouthUpperUpRight"] = 0.4f,
      ["noseSneerLeft"] = 0.3f,
      ["noseSneerRight"] = 0.3f
    };
    public Dictionary<string, bool> BlendshapesBool = new Dictionary<string, bool>();
    public Dictionary<string, float> BlendshapesWeight = new Dictionary<string, float>();
    public string categoryDetails = "";

    private void OnFaceLandmarkDetectionOutput(FaceLandmarkerResult result, Image image, long timestamp)
    {
      _faceLandmarkerResultAnnotationController.DrawLater(result);

      NoFaceExist = result.faceBlendshapes == null;

      foreach (Classifications classifications in result.faceBlendshapes)
      {
        categoryDetails = "";
        foreach (Category cat in classifications.categories)
        {
          if (cat.categoryName == "_neutral")
          {
            continue;
          }
          BlendshapesWeight[cat.categoryName] = cat.score;
          if (cat.score > BlendshapesStandard[cat.categoryName]) 
          {
            BlendshapesBool[cat.categoryName] = true;
            // categoryDetails += cat.index + ". " + cat.categoryName + " - " + cat.score + "\n";
          }
          else 
          {
            BlendshapesBool[cat.categoryName] = false;
          }
          categoryDetails += cat.index + ". " + cat.categoryName + " - " + cat.score + "\n";
        }
      }

      foreach (var normalizedLandmarks in result.faceLandmarks)
      {
        GetLandmarks(normalizedLandmarks.landmarks);
      }
    }

    const int LeftEarIndex = 366;
    const int RightEarIndex = 137;
    const int NoseTipIndex = 4;

    public Tasks.Components.Containers.NormalizedLandmark LeftEarLandmark;
    public Tasks.Components.Containers.NormalizedLandmark RightEarLandmark;
    public Tasks.Components.Containers.NormalizedLandmark NoseTipLandmark;

    private void GetLandmarks(IReadOnlyList<Tasks.Components.Containers.NormalizedLandmark> landmarks)
    {
      LeftEarLandmark = landmarks[LeftEarIndex];
      RightEarLandmark = landmarks[RightEarIndex];
      NoseTipLandmark = landmarks[NoseTipIndex];
    }
  }
}
