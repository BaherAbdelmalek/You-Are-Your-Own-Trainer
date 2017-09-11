using RootSystem = System;
using System.Linq;
using System.Collections.Generic;
namespace Microsoft.Kinect.Face
{
    //
    // Microsoft.Kinect.Face.FaceFrameSource
    //
    public sealed partial class FaceFrameSource : Helper.INativeWrapper

    {
        internal RootSystem.IntPtr _pNative;
        RootSystem.IntPtr Helper.INativeWrapper.nativePtr { get { return _pNative; } }

        // Constructors and Finalizers
        internal FaceFrameSource(RootSystem.IntPtr pNative)
        {
            _pNative = pNative;
            Microsoft_Kinect_Face_FaceFrameSource_AddRefObject(ref _pNative);
        }

        ~FaceFrameSource()
        {
            Dispose(false);
        }

        [RootSystem.Runtime.InteropServices.DllImport("KinectFaceUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern void Microsoft_Kinect_Face_FaceFrameSource_ReleaseObject(ref RootSystem.IntPtr pNative);
        [RootSystem.Runtime.InteropServices.DllImport("KinectFaceUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern void Microsoft_Kinect_Face_FaceFrameSource_AddRefObject(ref RootSystem.IntPtr pNative);
        public void Dispose(bool disposing)
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                return;
            }

            __EventCleanup();

            Helper.NativeObjectCache.RemoveObject<FaceFrameSource>(_pNative);
                Microsoft_Kinect_Face_FaceFrameSource_ReleaseObject(ref _pNative);

            _pNative = RootSystem.IntPtr.Zero;
        }


        // Public Properties
        [RootSystem.Runtime.InteropServices.DllImport("KinectFaceUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern ulong Microsoft_Kinect_Face_FaceFrameSource_get_TrackingId(RootSystem.IntPtr pNative);
        [RootSystem.Runtime.InteropServices.DllImport("KinectFaceUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern void Microsoft_Kinect_Face_FaceFrameSource_put_TrackingId(RootSystem.IntPtr pNative, ulong trackingId);
        public  ulong TrackingId
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("FaceFrameSource");
                }

                return Microsoft_Kinect_Face_FaceFrameSource_get_TrackingId(_pNative);
            }
            set
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("FaceFrameSource");
                }

                Microsoft_Kinect_Face_FaceFrameSource_put_TrackingId(_pNative, value);
                Helper.ExceptionHelper.CheckLastError();
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("KinectFaceUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern Microsoft.Kinect.Face.FaceFrameFeatures Microsoft_Kinect_Face_FaceFrameSource_get_FaceFrameFeatures(RootSystem.IntPtr pNative);
        [RootSystem.Runtime.InteropServices.DllImport("KinectFaceUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern void Microsoft_Kinect_Face_FaceFrameSource_put_FaceFrameFeatures(RootSystem.IntPtr pNative, Microsoft.Kinect.Face.FaceFrameFeatures faceFrameFeatures);
        public  Microsoft.Kinect.Face.FaceFrameFeatures FaceFrameFeatures
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("FaceFrameSource");
                }

                return Microsoft_Kinect_Face_FaceFrameSource_get_FaceFrameFeatures(_pNative);
            }
            set
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("FaceFrameSource");
                }

                Microsoft_Kinect_Face_FaceFrameSource_put_FaceFrameFeatures(_pNative, value);
                Helper.ExceptionHelper.CheckLastError();
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("KinectFaceUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern bool Microsoft_Kinect_Face_FaceFrameSource_get_IsActive(RootSystem.IntPtr pNative);
        public  bool IsActive
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("FaceFrameSource");
                }

                return Microsoft_Kinect_Face_FaceFrameSource_get_IsActive(_pNative);
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("KinectFaceUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern bool Microsoft_Kinect_Face_FaceFrameSource_get_IsTrackingIdValid(RootSystem.IntPtr pNative);
        public  bool IsTrackingIdValid
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("FaceFrameSource");
                }

                return Microsoft_Kinect_Face_FaceFrameSource_get_IsTrackingIdValid(_pNative);
            }
        }

        [RootSystem.Runtime.InteropServices.DllImport("KinectFaceUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern RootSystem.IntPtr Microsoft_Kinect_Face_FaceFrameSource_get_KinectSensor(RootSystem.IntPtr pNative);
        public  Windows.Kinect.KinectSensor KinectSensor
        {
            get
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    throw new RootSystem.ObjectDisposedException("FaceFrameSource");
                }

                RootSystem.IntPtr objectPointer = Microsoft_Kinect_Face_FaceFrameSource_get_KinectSensor(_pNative);
                Helper.ExceptionHelper.CheckLastError();
                if (objectPointer == RootSystem.IntPtr.Zero)
                {
                    return null;
                }

                return Helper.NativeObjectCache.CreateOrGetObject<Windows.Kinect.KinectSensor>(objectPointer, n => new Windows.Kinect.KinectSensor(n));
            }
        }


        // Events
        private static RootSystem.Runtime.InteropServices.GCHandle _Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_Handle;
        [RootSystem.Runtime.InteropServices.UnmanagedFunctionPointer(RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private delegate void _Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate(RootSystem.IntPtr args, RootSystem.IntPtr pNative);
        private static Helper.CollectionMap<RootSystem.IntPtr, List<RootSystem.EventHandler<Microsoft.Kinect.Face.TrackingIdLostEventArgs>>> Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_callbacks = new Helper.CollectionMap<RootSystem.IntPtr, List<RootSystem.EventHandler<Microsoft.Kinect.Face.TrackingIdLostEventArgs>>>();
        [AOT.MonoPInvokeCallbackAttribute(typeof(_Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate))]
        private static void Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_Handler(RootSystem.IntPtr result, RootSystem.IntPtr pNative)
        {
            List<RootSystem.EventHandler<Microsoft.Kinect.Face.TrackingIdLostEventArgs>> callbackList = null;
            Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_callbacks.TryGetValue(pNative, out callbackList);
            lock(callbackList)
            {
                var objThis = Helper.NativeObjectCache.GetObject<FaceFrameSource>(pNative);
                var args = new Microsoft.Kinect.Face.TrackingIdLostEventArgs(result);
                foreach(var func in callbackList)
                {
                    Helper.EventPump.Instance.Enqueue(() => { try { func(objThis, args); } catch { } });
                }
            }
        }
        [RootSystem.Runtime.InteropServices.DllImport("KinectFaceUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern void Microsoft_Kinect_Face_FaceFrameSource_add_TrackingIdLost(RootSystem.IntPtr pNative, _Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate eventCallback, bool unsubscribe);
        public  event RootSystem.EventHandler<Microsoft.Kinect.Face.TrackingIdLostEventArgs> TrackingIdLost
        {
            add
            {
                Helper.EventPump.EnsureInitialized();

                Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_callbacks.TryAddDefault(_pNative);
                var callbackList = Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_callbacks[_pNative];
                lock (callbackList)
                {
                    callbackList.Add(value);
                    if(callbackList.Count == 1)
                    {
                        var del = new _Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate(Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_Handler);
                        _Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_Handle = RootSystem.Runtime.InteropServices.GCHandle.Alloc(del);
                        Microsoft_Kinect_Face_FaceFrameSource_add_TrackingIdLost(_pNative, del, false);
                    }
                }
            }
            remove
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    return;
                }

                Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_callbacks.TryAddDefault(_pNative);
                var callbackList = Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_callbacks[_pNative];
                lock (callbackList)
                {
                    callbackList.Remove(value);
                    if(callbackList.Count == 0)
                    {
                        Microsoft_Kinect_Face_FaceFrameSource_add_TrackingIdLost(_pNative, Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_Handler, true);
                        _Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_Handle.Free();
                    }
                }
            }
        }

        private static RootSystem.Runtime.InteropServices.GCHandle _Windows_Data_PropertyChangedEventArgs_Delegate_Handle;
        [RootSystem.Runtime.InteropServices.UnmanagedFunctionPointer(RootSystem.Runtime.InteropServices.CallingConvention.Cdecl)]
        private delegate void _Windows_Data_PropertyChangedEventArgs_Delegate(RootSystem.IntPtr args, RootSystem.IntPtr pNative);
        private static Helper.CollectionMap<RootSystem.IntPtr, List<RootSystem.EventHandler<Windows.Data.PropertyChangedEventArgs>>> Windows_Data_PropertyChangedEventArgs_Delegate_callbacks = new Helper.CollectionMap<RootSystem.IntPtr, List<RootSystem.EventHandler<Windows.Data.PropertyChangedEventArgs>>>();
        [AOT.MonoPInvokeCallbackAttribute(typeof(_Windows_Data_PropertyChangedEventArgs_Delegate))]
        private static void Windows_Data_PropertyChangedEventArgs_Delegate_Handler(RootSystem.IntPtr result, RootSystem.IntPtr pNative)
        {
            List<RootSystem.EventHandler<Windows.Data.PropertyChangedEventArgs>> callbackList = null;
            Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryGetValue(pNative, out callbackList);
            lock(callbackList)
            {
                var objThis = Helper.NativeObjectCache.GetObject<FaceFrameSource>(pNative);
                var args = new Windows.Data.PropertyChangedEventArgs(result);
                foreach(var func in callbackList)
                {
                    Helper.EventPump.Instance.Enqueue(() => { try { func(objThis, args); } catch { } });
                }
            }
        }
        [RootSystem.Runtime.InteropServices.DllImport("KinectFaceUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern void Microsoft_Kinect_Face_FaceFrameSource_add_PropertyChanged(RootSystem.IntPtr pNative, _Windows_Data_PropertyChangedEventArgs_Delegate eventCallback, bool unsubscribe);
        public  event RootSystem.EventHandler<Windows.Data.PropertyChangedEventArgs> PropertyChanged
        {
            add
            {
                Helper.EventPump.EnsureInitialized();

                Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(_pNative);
                var callbackList = Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[_pNative];
                lock (callbackList)
                {
                    callbackList.Add(value);
                    if(callbackList.Count == 1)
                    {
                        var del = new _Windows_Data_PropertyChangedEventArgs_Delegate(Windows_Data_PropertyChangedEventArgs_Delegate_Handler);
                        _Windows_Data_PropertyChangedEventArgs_Delegate_Handle = RootSystem.Runtime.InteropServices.GCHandle.Alloc(del);
                        Microsoft_Kinect_Face_FaceFrameSource_add_PropertyChanged(_pNative, del, false);
                    }
                }
            }
            remove
            {
                if (_pNative == RootSystem.IntPtr.Zero)
                {
                    return;
                }

                Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(_pNative);
                var callbackList = Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[_pNative];
                lock (callbackList)
                {
                    callbackList.Remove(value);
                    if(callbackList.Count == 0)
                    {
                        Microsoft_Kinect_Face_FaceFrameSource_add_PropertyChanged(_pNative, Windows_Data_PropertyChangedEventArgs_Delegate_Handler, true);
                        _Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
                    }
                }
            }
        }


        // Public Methods
        [RootSystem.Runtime.InteropServices.DllImport("KinectFaceUnityAddin", CallingConvention=RootSystem.Runtime.InteropServices.CallingConvention.Cdecl, SetLastError=true)]
        private static extern RootSystem.IntPtr Microsoft_Kinect_Face_FaceFrameSource_OpenReader(RootSystem.IntPtr pNative);
        public Microsoft.Kinect.Face.FaceFrameReader OpenReader()
        {
            if (_pNative == RootSystem.IntPtr.Zero)
            {
                throw new RootSystem.ObjectDisposedException("FaceFrameSource");
            }

            RootSystem.IntPtr objectPointer = Microsoft_Kinect_Face_FaceFrameSource_OpenReader(_pNative);
            Helper.ExceptionHelper.CheckLastError();
            if (objectPointer == RootSystem.IntPtr.Zero)
            {
                return null;
            }

            return Helper.NativeObjectCache.CreateOrGetObject<Microsoft.Kinect.Face.FaceFrameReader>(objectPointer, n => new Microsoft.Kinect.Face.FaceFrameReader(n));
        }

        private void __EventCleanup()
        {
            {
                Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_callbacks.TryAddDefault(_pNative);
                var callbackList = Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_callbacks[_pNative];
                lock (callbackList)
                {
                    if (callbackList.Count > 0)
                    {
                        callbackList.Clear();
                        if (_pNative != RootSystem.IntPtr.Zero)
                        {
                            Microsoft_Kinect_Face_FaceFrameSource_add_TrackingIdLost(_pNative, Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_Handler, true);
                        }
                        _Microsoft_Kinect_Face_TrackingIdLostEventArgs_Delegate_Handle.Free();
                    }
                }
            }
            {
                Windows_Data_PropertyChangedEventArgs_Delegate_callbacks.TryAddDefault(_pNative);
                var callbackList = Windows_Data_PropertyChangedEventArgs_Delegate_callbacks[_pNative];
                lock (callbackList)
                {
                    if (callbackList.Count > 0)
                    {
                        callbackList.Clear();
                        if (_pNative != RootSystem.IntPtr.Zero)
                        {
                            Microsoft_Kinect_Face_FaceFrameSource_add_PropertyChanged(_pNative, Windows_Data_PropertyChangedEventArgs_Delegate_Handler, true);
                        }
                        _Windows_Data_PropertyChangedEventArgs_Delegate_Handle.Free();
                    }
                }
            }
        }
    }

}
